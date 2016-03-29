function setDatepicker(datepicker) {
    datepicker = $(datepicker);
    var dateFormat = $(datepicker).attr("data-date-format");

    if (!dateFormat)
        dateFormat = "YYYY/MM/DD";
    dateFormat = dateFormat.toUpperCase();
    // Get the text from the date attributes.
    var startText = datepicker.attr("data-startDate");
    var endText = datepicker.attr("data-endDate");
    var currText = datepicker.val();
    // Parse the date text.
    if (startText)
        var startDate = moment(startText, dateFormat);
    if (endText)
        var endDate = moment(endText, dateFormat);
    var currDate = moment(currText, dateFormat);
    // Check if the entered date is within the bounds of the start and end dates.
    // If it is not, set it to either the start or end date.
    if (startDate && startDate.valueOf() > currDate.valueOf()) {
        datepicker.val(startText);
    }
    if (startDate && endDate && endDate.valueOf() < startDate.valueOf()) {
        console.log("End date cannot be less than start date.");
        datepicker.removeAttr("data-endDate");
        endDate = undefined;
    }
    else {
        if (endDate && endDate.valueOf() < currDate.valueOf()) {
            datepicker.val(endText);
        }
    }
    var picker = $(datepicker).datepicker({
        onRender: function (date) {

            var result = '';
            if (startDate) {
                if (date.valueOf() < startDate.valueOf())
                    result = 'disabled';
            }
            if (endDate) {
                if (date.valueOf() > endDate.valueOf())
                    result = 'disabled';
            }
            return result;
        }
    }).on("hide", function (ev) {
        // Check that the new entered date is within the start and end bounds.
        // Set the actualText to the entered date to prevent junk text being entered.
        var enteredDate = ev.date;
        var actualText = moment(enteredDate).format(dateFormat);
        if (endDate) {
            if (enteredDate.valueOf() > endDate.valueOf()) {
                actualText = endText;
            }
        }
        if (startDate) {
            if (enteredDate.valueOf() < startDate.valueOf()) {
                actualText = startText;
            }
        }
        if (actualText) {
            datepicker.val(actualText);
            picker.setValue(actualText);
        }
    }).data("datepicker");
}