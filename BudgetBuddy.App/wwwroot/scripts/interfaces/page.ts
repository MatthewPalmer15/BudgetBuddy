declare global {
    interface Window {
        currentPage: IPage;
    }
}

export interface IPage {
    onLoad(): void;
    onReady(): void;
}


