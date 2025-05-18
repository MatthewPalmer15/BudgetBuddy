import '../../styles/components/modal.scss';
import { IPage } from '../interfaces/page';
import { initDatePickers } from '../utility/datepicker';

export class Modal implements IPage {
    constructor() {
        this.onLoad();
    }

    onLoad(): void {
        console.log("Page loaded.");
    }

    onReady(): void {
        console.log("Page ready.");
    }

    open(): void {
        initDatePickers();
        console.log("Date picker initialized.");
    }

    close(): void {
        console.log("Doing something.");
    }
}

window.currentPage = new Modal();
