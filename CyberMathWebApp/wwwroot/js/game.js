let score = 0;
let timeLeft = 60;
let currentAnswer = 0;
let timerInterval;
let currentMultiplier = 1;
let dbEquations = []; // Сюди завантажимо інтеграли з БД

document.addEventListener("DOMContentLoaded", () => {
    // Завантажуємо складності
    fetch('/api/difficulties')
        .then(res => res.json())
        .then(data => {
            const select = document.getElementById('difficultySelect');
            data.forEach(d => {
                select.innerHTML += `<option value="${d.id}" data-mult="${d.multiplier}">${d.name} (x${d.multiplier} pts)</option>`;
            });
        });

    // ОДРАЗУ завантажуємо хардкорні рівняння з бази даних!
    fetch('/api/equations/batch')
        .then(res => res.json())
        .then(data => {
            dbEquations = data;
        });
});

function startGame() {
    const nick = document.getElementById('nickname').value;
    if (!nick) { alert("Введи нікнейм!"); return; }

    const select = document.getElementById('difficultySelect');
    currentMultiplier = parseInt(select.options[select.selectedIndex].getAttribute('data-mult'));

    document.getElementById('startScreen').classList.add('hidden');
    document.getElementById('gameScreen').classList.remove('hidden');
    document.getElementById('answer').focus();

    generateQuestion();

    timerInterval = setInterval(() => {
        timeLeft--;
        document.getElementById('time').innerText = timeLeft;
        if (timeLeft <= 0) endGame(nick, select.value);
    }, 1000);
}

function generateQuestion() {
    // ЯКЩО ВИБРАНО РІВЕНЬ "EXPERT" (Множник 5) - БЕРЕМО З БАЗИ ДАНИХ!
    if (currentMultiplier === 5 && dbEquations.length > 0) {
        const randomIndex = Math.floor(Math.random() * dbEquations.length);
        const eq = dbEquations[randomIndex];

        currentAnswer = eq.answer;
        document.getElementById('question').innerText = eq.expression;
        return;
    }

    // Звичайна генерація для Easy, Medium, Hard
    const maxNum = 20 * currentMultiplier;
    const a = Math.floor(Math.random() * maxNum) + 1;
    const b = Math.floor(Math.random() * maxNum) + 1;

    if (currentMultiplier === 3 && Math.random() > 0.5) {
        const m1 = Math.floor(Math.random() * 10) + 1;
        const m2 = Math.floor(Math.random() * 10) + 1;
        currentAnswer = m1 * m2;
        document.getElementById('question').innerText = `${m1} * ${m2}`;
        return;
    }

    const isPlus = Math.random() > 0.5;
    if (isPlus) {
        currentAnswer = a + b;
        document.getElementById('question').innerText = `${a} + ${b}`;
    } else {
        const max = Math.max(a, b);
        const min = Math.min(a, b);
        currentAnswer = max - min;
        document.getElementById('question').innerText = `${max} - ${min}`;
    }
}

function checkAnswer() {
    const input = document.getElementById('answer');
    if (parseInt(input.value) === currentAnswer) {
        score += currentMultiplier;
        document.getElementById('score').innerText = score;
        input.value = '';
        generateQuestion();
    }
}

function endGame(nickname, difficultyId) {
    clearInterval(timerInterval);
    document.getElementById('gameScreen').classList.add('hidden');
    document.getElementById('endScreen').classList.remove('hidden');
    document.getElementById('finalScore').innerText = score;

    fetch('/api/game/save', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ nickname: nickname, score: score, difficultyId: parseInt(difficultyId) })
    }).then(() => loadLeaderboard());
}

function loadLeaderboard() {
    fetch('/api/game/leaderboard')
        .then(res => res.json())
        .then(data => {
            const list = document.getElementById('leaderboardList');
            list.innerHTML = '';
            data.forEach((p, index) => {
                list.innerHTML += `<li><b>${index + 1}. ${p.nickname}</b> - ${p.score} pts <small style="color:#888;">(${p.difficulty})</small></li>`;
            });
        });
}