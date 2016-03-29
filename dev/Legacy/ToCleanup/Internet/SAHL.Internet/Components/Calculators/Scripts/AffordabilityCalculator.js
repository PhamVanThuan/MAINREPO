(function ($) {
    var settings = null;

    function _calculateMaxInstallment() { return (_calculateJointIncome() / 100) * 30; };

    function _calculateJointIncome() {
        var salaryOne = parseInt($("#" + settings.controls.txtSalaryOne).val());
        var salaryTwo = parseInt($("#" + settings.controls.txtSalaryTwo).val());
        if (isNaN(salaryOne) || salaryOne < 0) salaryOne = 0;
        if (isNaN(salaryTwo) || salaryTwo < 0) salaryTwo = 0;
        return salaryOne + salaryTwo;
    };

    function _updateAffordability() {
        var max = _calculateMaxInstallment();
        //var currentinstallment = parseInt($("#" + settings.controls.txtMonthlyInstalment).val());

        //if (isNaN(currentinstallment)) currentinstallment = 0;
        //if (max > currentinstallment)
        //    $("#" + settings.controls.txtMonthlyInstalment).val((isNaN(currentinstallment) || currentinstallment == 0) ? 0 : currentinstallment);
        ///else 
       // {
            $("#" + settings.controls.txtMonthlyInstalment).val((isNaN(max) || max == 0) ? 0 : max);
       // }

        $("#" + settings.controls.lblStatus).html("How much can you afford?");
        //$("#" + settings.controls.lblStatus).html((isNaN(max) || max == 0) ? "How much can you afford?" : "You can afford up to R " + max + " per month");
    };

    $.affordabilityCalculator = {
        init: function (options) {
            settings = $.extend({
                controls: {
                    txtSalaryOne: null,
                    txtSalaryTwo: null,
                    txtProfitFromSale: null,
                    txtOtherContribution: null,
                    txtMonthlyInstalment: null,
                    txtLoanTerm: null,
                    txtInterestRate: null,
                    lblStatus: null
                },
                minHouseholdIncome: 0
            }, options);

            $("#" + settings.controls.txtSalaryOne).restrictInt().change(_updateAffordability);
            $("#" + settings.controls.txtSalaryTwo).restrictInt().change(_updateAffordability);
            $("#" + settings.controls.txtProfitFromSale).restrictInt();
            $("#" + settings.controls.txtOtherContribution).restrictInt();
            $("#" + settings.controls.txtMonthlyInstalment).restrictInt();
            $("#" + settings.controls.txtLoanTerm).restrictInt();
            $("#" + settings.controls.txtInterestRate).restrictDbl();

            //_updateAffordability();
        },
        validateJointIncome: function (source, args) {
            var income = _calculateJointIncome();
            args.IsValid = (income >= settings.minHouseholdIncome);
        },
        validateMonthlyInstallment: function (source, args) {
            var max = _calculateMaxInstallment();
            var installment = $("#" + settings.controls.txtMonthlyInstalment).val();
            if (installment.isNaN) installment = 0;
            if (installment <= max && installment !== 0) args.IsValid = true;
            else {
                source.errormessage = "Your Monthly installment should not exceed R " + max + " per month";
                arguments.IsValid = false;
            }
        }
    };
})(jQuery);