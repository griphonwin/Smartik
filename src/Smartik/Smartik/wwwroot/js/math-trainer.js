window.mathTrainer = {
    // Безопасные пустые заглушки для совместимости со старыми вызовами страниц
    registerPageRef: function (dotNetRef) { },
    initAutoAdvance: function () { },

    triggerPrint: function () {
        window.print();
    },

    launchConfetti: function () {
        const canvas = document.createElement('canvas');
        canvas.style.position = 'fixed';
        canvas.style.top = '0'; canvas.style.left = '0';
        canvas.style.width = '100vw'; canvas.style.height = '100vh';
        canvas.style.pointerEvents = 'none'; canvas.style.zIndex = '99999';
        document.body.appendChild(canvas);

        const ctx = canvas.getContext('2d');
        let width = canvas.width = window.innerWidth;
        let height = canvas.height = window.innerHeight;

        const colors = ['#FFC107', '#FF5722', '#E91E63', '#9C27B0', '#3F51B5', '#00BCD4', '#4CAF50', '#8BC34A'];
        const particles = [];

        function createSpurt(startX, startY, angleRangeMin, angleRangeMax) {
            for (let i = 0; i < 70; i++) {
                const angle = (Math.random() * (angleRangeMax - angleRangeMin) + angleRangeMin) * Math.PI / 180;
                const speed = Math.random() * 15 + 10;
                particles.push({
                    x: startX, y: startY,
                    vx: Math.cos(angle) * speed, vy: Math.sin(angle) * speed,
                    size: Math.random() * 8 + 6,
                    color: colors[Math.floor(Math.random() * colors.length)],
                    rotation: Math.random() * 360, rotationSpeed: Math.random() * 10 - 5,
                    opacity: 1
                });
            }
        }

        createSpurt(0, height * 0.9, -75, -15);
        createSpurt(width, height * 0.9, -165, -105);

        function update() {
            ctx.clearRect(0, 0, width, height);
            for (let i = particles.length - 1; i >= 0; i--) {
                const p = particles[i];
                p.vy += 0.3; p.vx *= 0.98; p.x += p.vx; p.y += p.vy; p.rotation += p.rotationSpeed;
                if (p.vy > 0) p.opacity -= 0.01;
                if (p.opacity <= 0 || p.x < -20 || p.x > width + 20 || p.y > height + 20) {
                    particles.splice(i, 1); continue;
                }
                ctx.save(); ctx.translate(p.x, p.y); ctx.rotate(p.rotation * Math.PI / 180);
                ctx.globalAlpha = p.opacity; ctx.fillStyle = p.color;
                ctx.fillRect(-p.size / 2, -p.size / 2, p.size, p.size); ctx.restore();
            }
            if (particles.length > 0) requestAnimationFrame(update); else canvas.remove();
        }
        requestAnimationFrame(update);
    }
};
