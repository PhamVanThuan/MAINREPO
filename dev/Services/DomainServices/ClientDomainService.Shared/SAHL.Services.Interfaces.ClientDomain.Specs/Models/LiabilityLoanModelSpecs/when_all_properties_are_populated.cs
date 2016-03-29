using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.LiabilityLoanModelSpecs
{
    public class when_all_properties_are_populated : WithFakes
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
            financialInstitute = "Financial institute";
            dateRepayable = DateTime.Now;
            instalmentValue = 0d;
            liabilityValue = 1d;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() => { model = new LiabilityLoanModel(loanType, financialInstitute, dateRepayable, instalmentValue, liabilityValue); });
        };

        private It should_not_throw_an_exception = () =>
        {
            ex.ShouldBeNull();
        };

        private It should_construct_the_model = () =>
        {
            ex.ShouldBeNull();
        };
    }
}