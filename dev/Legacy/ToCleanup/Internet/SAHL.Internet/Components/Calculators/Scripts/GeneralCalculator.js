(function ($) {

    var settings = null;

    function _setupVariableRateLoanDisplay() {
        $.hideInput("#" + settings.controls.pnlFixPercentage);
        if (settings.mode !== 3) $.showInput("#" + settings.controls.chkCapitaliseFees);
        else $.hideInput("#" + settings.controls.chkCapitaliseFees);
        $.showInput("#" + settings.controls.chkInterestOnly);
    };

    function _setupFixedRateLoanDisplay() {
        $.showInput("#" + settings.controls.pnlFixPercentage);
        if (settings.mode !== 3) $.showInput("#" + settings.controls.chkCapitaliseFees);
        else $.hideInput("#" + settings.controls.chkCapitaliseFees);
        $.hideInput("#" + settings.controls.chkInterestOnly);
    };

    function _setupLoyaltyLoanDisplay() {
        $.hideInput("#" + settings.controls.pnlFixPercentage);
        if (settings.mode !== 3) $.showInput("#" + settings.controls.chkCapitaliseFees);
        else $.hideInput("#" + settings.controls.chkCapitaliseFees);
        $.showInput("#" + settings.controls.chkInterestOnly);
    };

    $.calculator = {
        init: function (options) {
            settings = $.extend({
                controls: {
                    rbFixedRate: null,
                    rbVariableRate: null,
                    chkCapitaliseFees: null,
                    chkInterestOnly: null,
                    pnlFixPercentage: null,
                    tbLoanTerm: null,
                    tbCurrentLoan: null,
                    tbPurchasePrice: null,
                    tbCashOut: null,
                    tbFixPercentage: null,
                    tbCashDeposit: null,
                    tbMarketValue: null,
                    tbHouseholdIncome: null
                },
                totalFees: 0,
                minFixFinServiceAmount: 0,
                mode: 0
            }, options);

            $("#" + settings.controls.pnlFixPercentage).hide();

            if (settings.mode !== 3) $("#" + settings.controls.chkCapitaliseFees).show().next("label").show();
            else $("#" + settings.controls.chkCapitaliseFees).hide().next("label").hide();

            if ($("#" + settings.controls.rbFixedRate).is(":checked")) _setupFixedRateLoanDisplay();
            if ($("#" + settings.controls.rbVariableRate).is(":checked")) _setupVariableRateLoanDisplay();

            $("#" + settings.controls.rbFixedRate).change(_setupFixedRateLoanDisplay);
            $("#" + settings.controls.rbVariableRate).change(_setupVariableRateLoanDisplay);

            $("#" + settings.controls.tbLoanTerm).restrictInt();
            $("#" + settings.controls.tbFixPercentage).restrictInt();
            $("#" + settings.controls.tbPurchasePrice).restrictInt();
            $("#" + settings.controls.tbCashDeposit).restrictInt();
            $("#" + settings.controls.tbMarketValue).restrictInt();
            $("#" + settings.controls.tbCurrentLoan).restrictInt();
            $("#" + settings.controls.tbCashOut).restrictInt();
            $("#" + settings.controls.tbHouseholdIncome).restrictInt();
        },
        validateFixedPercentageValues: function (source, args) {
            if (!$("#" + settings.controls.rbFixedRate).is(":checked")) {
                args.isValid = true;
                return;
            }

            var fixedPercentage = parseFloat($("#" + settings.controls.tbFixPercentage).val()) / 100;
            if (isNaN(fixedPercentage) || fixedPercentage > 1) {
                //if (isNaN(fixedPercentage)) $("#" + settings.controls.customValidator2)[0].errormessage = "The fixed portion of your loan must be a number between 0 and 100%";
                //else $("#" + settings.controls.customValidator2)[0].errormessage = "The fixed portion of your loan cannot exceed 100%.";
                arguments.isValid = false;
                return;
            }

            var required = 0;
            switch (settings.mode) {
                case 2: //switch loan
                    required = parseInt($("#" + settings.controls.tbCurrentLoan).val());
                    break;
                case 3: // new purchase
                    required = parseInt($("#" + settings.controls.tbPurchasePrice).val());
                    break;
                case 4: // refinance
                    required = parseInt($("#" + settings.controls.tbCashOut).val());
                    break;
            }
            if (!isNaN(required)) required = 0;
            if ($("#" + settings.controls.chkCapitaliseFees).is(":checked")) required += settings.totalFees;
            if (!$("#" + settings.controls.rbFixedRate).is(":checked") || fixedPercentage <= 0) fixedPercentage = 0;

            var minPercent = settings.minFixFinServiceAmount / required;
            //$("#" + settings.controls.customValidator2)[0].errormessage = "The selected fixed amount is R " + (required * fixedPercentage) + ", less than the minimum of R " + settings.minFixFinServiceAmount;
            arguments.isValid = (minPercent <= fixedPercentage);
        }
    };
})(jQuery);