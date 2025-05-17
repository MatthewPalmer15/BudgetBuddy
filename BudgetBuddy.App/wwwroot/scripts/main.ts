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
