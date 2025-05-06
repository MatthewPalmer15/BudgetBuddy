import '../styles/main.scss';

import 'preline/dist/preline.js';
import "@preline/accordion";
import "@preline/dropdown";
import "@preline/overlay";
import "@preline/carousel";
import { createIcons, icons } from 'lucide';
import flatpickr from "flatpickr";

declare global {
    interface Window {
        marukiInit: () => void;
    }
}

window.marukiInit = () => {
    createIcons({ icons });
    // @ts-ignore
    window.HSStaticMethods?.autoInit?.();

    flatpickr("[data-datepicker]", {
        dateFormat: "Y-m-d",
    });

    console.log(`Hit at ${new Date()}`);

};
