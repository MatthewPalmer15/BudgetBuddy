import flatpickr from "flatpickr";

export function initDatePickers() {

    const datePickers = document.querySelectorAll("[data-datepicker]") as NodeListOf<HTMLElement>;
    datePickers.forEach(datePicker => {
        let options: Partial<flatpickr.Options.Options> = {
            ariaDateFormat: "l J F Y",
            disableMobile: true,
            dateFormat: "Y-m-d",
            altInput: true,
            altFormat: "j F Y",
            onReady: function (_, __, instance) {
                instance.currentYearElement.readOnly = true;
            }
        };

        let minDate = datePicker.dataset.minDate;
        if (minDate) options.minDate = minDate;

        let maxDate = datePicker.dataset.maxDate;
        if (maxDate) options.maxDate = maxDate;

        flatpickr(datePicker, options);
    });

    // flatpickr("[data-datepicker]", {
    //     dateFormat: "Y-m-d",
    //     altInput: true,
    //     altFormat: "j F Y",
    //     onReady: function (_, __, instance) {
    //         instance.currentYearElement.readOnly = true;
    //     }
    // });
}


