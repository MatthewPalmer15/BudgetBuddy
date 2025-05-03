import '../styles/main.scss';

import 'preline/dist/preline.js';
import "@preline/accordion";
import "@preline/dropdown";
import "@preline/overlay";
import "@preline/carousel";
import "@preline/stepper";
import "@preline/datepicker";
import { createIcons, icons } from 'lucide';

declare global {
    interface Window {
        marukiInit: () => void;
    }
}

window.marukiInit = () => {
    createIcons({ icons });
    // @ts-ignore
    window.HSStaticMethods?.autoInit?.();
    console.log(`Hit at ${new Date()}`);
};
