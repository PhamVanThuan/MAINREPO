using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.LiabilityLoanModelSpecs
{
    public class when_value_is_less_then_zero : WithFakes
    {
        private static AssetLiabilitySubType loanType;
        private static string financialInstitute;
        private static DateTime dateRepayable;
        private static double instalmentValue;
        private static double liabilityValue;
        private static LiabilityLoanModel model;
        private static Exception ex;

        private Establish context = () =>
        {
            loanType = AssetLiabilitySubType.PersonalLoan;
            financialInstitute = "Financial institution";
            dateRepayable = DateTime.Now;
            instalmentValue = -1d;
            liabilityValue = 1d;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() => { model = new LiabilityLoanModel(loanType, financialInstitute, dateRepayable, instalmentValue, liabilityValue); });
        };

        private It should_throw_a_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(ValidationException));
        };

        private It should_contain_a_message = () =>
        {
            ex.Message.ShouldEqual("Instalment Value must be greater than or equal to zero.");
        };
    }
}
