declare const VANTA: any;
declare const THREE: any;

let vantaEffect: any;

function initVanta() {
    const el = document.getElementById("animated-background");
    if (!el) return;

    vantaEffect = VANTA.WAVES({
        el,
        THREE,
        mouseControls: false,
        touchControls: false,
        gyroControls: false,
        minHeight: 200,
        minWidth: 200,
        scale: 1.0,
        scaleMobile: 1.0,
        color: 0x509,
        shininess: 20,
        waveHeight: 15,
        waveSpeed: 0.45,
        zoom: 0.5,
    });

    // Force a resize after layout is complete
    setTimeout(() => {
        vantaEffect.resize?.();
    }, 100);
}

window.addEventListener("DOMContentLoaded", initVanta);
