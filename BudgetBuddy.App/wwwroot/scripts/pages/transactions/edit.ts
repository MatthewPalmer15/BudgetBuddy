import '../../../styles/pages/transactions/edit.scss';

import { initDatePickers } from "../../utility/datepicker";

function onPageReady() {
    console.log("Edit transaction page loaded");
    initDatePickers();
    console.log("Date pickers initialized");
}

if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", onPageReady);
} else {
    // DOM is already ready
    onPageReady();
}
