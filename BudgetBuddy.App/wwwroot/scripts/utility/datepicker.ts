import flatpickr from "flatpickr";

export function initDatePickers() {

    flatpickr("[data-datepicker]", {
        dateFormat: "Y-m-d",
        altInput: true,
        altFormat: "j F Y",
        onReady: function (_, __, instance) {
            instance.currentYearElement.readOnly = true;
        }
    });
}
