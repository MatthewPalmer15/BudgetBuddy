import '../styles/main.scss';

import 'preline/dist/preline.js';
import "@preline/accordion";
import "@preline/dropdown";
import "@preline/overlay";
import "@preline/carousel";
import { createIcons, icons } from 'lucide';
import { initDatePickers } from "./utility/datepicker";

declare global {
    interface Window {
        marukiInit: () => void;
    }
}

window.marukiInit = () => {
    createIcons({ icons });
    // @ts-ignore
    window.HSStaticMethods?.autoInit?.();
    initDatePickers();
    console.log(`Hit at ${new Date()}`);
};

declare var VANTA: any;

// Make sure VANTA is declared globally if you're not using a module
declare var VANTA: any;

window.addEventListener("DOMContentLoaded", () => {
    VANTA.WAVES({
        el: "#animated-background",          // Attach to body or a wrapper div
        mouseControls: false,
        touchControls: false,
        gyroControls: false,
        minHeight: 200.00,
        minWidth: 200.00,
        scale: 1.00,
        scaleMobile: 1.00,

        color: 0x509,//  0xf1e,            // Dark navy blue/black tone
        shininess: 20.0,            // Less reflection for a darker look
        waveHeight: 15.0,
        waveSpeed: 0.25,            // Slow and ambient
        zoom: 0.5
    });
});
