import '../styles/main.scss';
import './utility/background';
import './interfaces/page';

import 'preline/dist/preline.js';
import "@preline/accordion";
import "@preline/dropdown";
import "@preline/overlay";
import "@preline/carousel";
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

