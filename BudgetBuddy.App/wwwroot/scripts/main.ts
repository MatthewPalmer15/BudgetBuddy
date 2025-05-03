import '../styles/main.scss';

import "@preline/accordion";
import "@preline/dropdown";
import "@preline/overlay";
import "@preline/carousel";
import "@preline/stepper";
import { createIcons, icons } from 'lucide';
import 'preline/dist/preline.js';

declare global {
    interface Window {
        marukiInit: () => void;
    }
}

window.marukiInit = () => {
    createIcons({ icons });
    // @ts-ignore
    window.HSStaticMethods?.autoInit?.();
};
