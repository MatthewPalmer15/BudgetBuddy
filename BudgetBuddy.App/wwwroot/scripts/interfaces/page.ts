import { createIcons, icons } from 'lucide';

declare global {
    interface Window {
        currentPage: IPage;
    }
}

export interface IPage {
    name: string;
    onLoad(): void;
    onRefresh(): void;
}

export class Page implements IPage {

    name:string = "Base Page";

    onLoad(): void {
        console.log(`${this.name} loaded.`);
    }

    onRefresh(): void {
        createIcons({ icons });
        // @ts-ignore
        window.HSStaticMethods?.autoInit?.();
        console.log(`Refreshed ${this.name} at ${new Date().toLocaleTimeString()}`);
    }
}

if (!window.currentPage) {
    window.currentPage = new Page();
}

document.addEventListener("DOMContentLoaded", () => {
    window.currentPage.onLoad();
});