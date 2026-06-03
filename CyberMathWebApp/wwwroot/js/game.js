const Game = {
    score: 0,
    timeLeft: 60,
    currentAnswer: 0,
    timerInterval: null,
    currentMultiplier: 1,

    // Нові змінні для режиму "Кастомний рівень"
    isCompetitionMode: false,
    compEquations: [],
    compCurrentIndex: 0,
    allCompetitions: [],

    init: function () {
        fetch('/api/difficulties').then(res => res.json()).then(data => {
            const select = document.getElementById('difficultySelect');
            data.forEach(d => select.innerHTML += `<option value="${d.id}" data-mult="${d.multiplier}">${d.name} (x${d.multiplier} pts)</option>`);
        });

        fetch('/api/competitions').then(res => res.json()).then(data => {
            this.allCompetitions = data;
            const select = document.getElementById('competitionSelect');
            data.forEach(c => select.innerHTML += `<option value="${c.id}">${c.name} (${c.timeLimitSeconds}с, ${c.equations.length} завдань)</option>`);
        });
    },

    // СТАРТ ЗВИЧАЙНОЇ ГРИ (нескінченний рандом)
    startEndless: function () {
        const nick = document.getElementById('nickname').value;
        if (!nick) { alert("Введи нікнейм!"); return; }

        const select = document.getElementById('difficultySelect');
        this.currentMultiplier = parseInt(select.options[select.selectedIndex].getAttribute('data-mult'));
        this.isCompetitionMode = false;
        this.timeLeft = 60;

        this.launchGameUI(nick);
    },

    startCompetition: function () {
        const nick = document.getElementById('nickname').value;
        if (!nick) { alert("Введи нікнейм!"); return; }

        const compId = document.getElementById('competitionSelect').value;
        if (!compId) { alert("Обери рівень!"); return; }

        const comp = this.allCompetitions.find(c => c.id == compId);
        this.isCompetitionMode = true;
        this.compEquations = comp.equations.sort((a, b) => a.orderIndex - b.orderIndex);
        this.compCurrentIndex = 0;
        this.timeLeft = comp.timeLimitSeconds;
        this.currentMultiplier = 5;

        this.launchGameUI(nick);
    },

    launchGameUI: function (nick) {
        document.getElementById('startScreen').classList.add('hidden');
        document.getElementById('gameScreen').classList.remove('hidden');
        document.getElementById('answer').focus();

        this.generateQuestion();

        this.timerInterval = setInterval(() => {
            this.timeLeft--;
            document.getElementById('time').innerText = this.timeLeft;
            if (this.timeLeft <= 0) this.end(nick, false);
        }, 1000);
    },

    generateQuestion: function () {
        if (this.isCompetitionMode) {
            if (this.compCurrentIndex >= this.compEquations.length) {
                const nick = document.getElementById('nickname').value;
                this.score += this.timeLeft * 2; // Бонус за зекономлений час!
                this.end(nick, true);
                return;
            }
            const eq = this.compEquations[this.compCurrentIndex];
            this.currentAnswer = eq.answer;
            document.getElementById('question').innerText = eq.expression;
            return;
        }

        const maxNum = 20 * this.currentMultiplier;
        const a = Math.floor(Math.random() * maxNum) + 1;
        const b = Math.floor(Math.random() * maxNum) + 1;
        const isPlus = Math.random() > 0.5;
        if (isPlus) {
            this.currentAnswer = a + b;
            document.getElementById('question').innerText = `${a} + ${b}`;
        } else {
            const max = Math.max(a, b);
            const min = Math.min(a, b);
            this.currentAnswer = max - min;
            document.getElementById('question').innerText = `${max} - ${min}`;
        }
    },

    checkAnswer: function () {
        const input = document.getElementById('answer');
        if (parseInt(input.value) === this.currentAnswer) {
            this.score += this.currentMultiplier;
            document.getElementById('score').innerText = this.score;
            input.value = '';

            if (this.isCompetitionMode) this.compCurrentIndex++; // Йдемо до наступного прикладу
            this.generateQuestion();
        }
    },

    end: function (nickname, isWin) {
        clearInterval(this.timerInterval);
        document.getElementById('gameScreen').classList.add('hidden');
        document.getElementById('endScreen').classList.remove('hidden');

        if (isWin) {
            document.getElementById('finalScore').innerHTML = `${this.score} <br><span class="text-success">РІВЕНЬ ПРОЙДЕНО!</span>`;
        } else {
            document.getElementById('finalScore').innerHTML = `${this.score} <br><span class="text-danger">ЧАС ВИЙШОВ!</span>`;
        }

        // Зберігаємо результат у загальний лідерборд
        fetch('/api/game/save', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ nickname: nickname, score: this.score, difficultyId: 1 }) // Зберігаємо як загальний скор
        }).then(() => this.loadLeaderboard());
    },

    loadLeaderboard: function () {
        fetch('/api/game/leaderboard').then(res => res.json()).then(data => {
            const list = document.getElementById('leaderboardList');
            list.innerHTML = '';
            data.forEach((p, index) => {
                list.innerHTML += `<li class="list-group-item bg-dark text-info d-flex justify-content-between">
                    <span><b>${index + 1}.</b> ${p.nickname}</span><span class="text-warning fw-bold">${p.score}</span></li>`;
            });
        });
    },



    showCreateScreen: function () {
        document.getElementById('startScreen').classList.add('hidden');
        document.getElementById('createScreen').classList.remove('hidden');

        document.getElementById('eqList').innerHTML = '';
        this.addEqRow();
    },

    addEqRow: function () {
        const div = document.createElement('div');
        div.className = "eq-row";
        div.style.marginBottom = "10px";
        div.innerHTML = `
            <input type="text" class="eqExp" style="width: 250px;" placeholder="Приклад (напр. 2+2)">
            <input type="number" class="eqAns" style="width: 120px;" placeholder="Відповідь">
        `;
        document.getElementById('eqList').appendChild(div);
    },

    removeLastEqRow: function () {
        const eqList = document.getElementById('eqList');
        const rows = eqList.getElementsByClassName('eq-row');

        if (rows.length > 1) {
            eqList.removeChild(rows[rows.length - 1]);
        } else {
            alert("У рівні має бути хоча б один приклад!");
        }
    },

    saveCompetition: function () {
        const name = document.getElementById('compName').value;
        const time = document.getElementById('compTime').value;
        if (!name || !time) { alert("Заповни назву і час!"); return; }

        const rows = document.querySelectorAll('.eq-row');
        const equations = [];
        rows.forEach(row => {
            const exp = row.querySelector('.eqExp').value;
            const ans = row.querySelector('.eqAns').value;
            if (exp && ans) equations.push({ expression: exp, answer: parseInt(ans) });
        });

        if (equations.length === 0) { alert("Додай хоча б один приклад!"); return; }

        fetch('/api/competitions', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name: name, timeLimit: parseInt(time), equations: equations })
        }).then(res => {
            if (res.ok) {
                alert("Рівень відправлено на модерацію! Він з'явиться у списку після перевірки адміністратором.");
                location.reload();
            }
        });
    }
};

// Запуск при завантаженні (замість старого DOMContentLoaded)
document.addEventListener("DOMContentLoaded", () => Game.init());