using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.ApplicationDomain.Specs.ModelSpecs.RefinanceApplicationModelSpecs
{
    public class when_constructing_refinance_application : WithFakes
    {
        private static RefinanceApplicationModel model;

        private Because of = () =>
        {
            model = new RefinanceApplicationModel(OfferStatus.Open, 1, OriginationSource.SAHomeLoans, 985000, 240, 1, Product.NewVariableLoan, "reference1", 1);
        };

        private It should_have_a_refinance_application_type = () =>
        {
            model.ApplicationType.ShouldEqual(OfferType.RefinanceLoan);
        };

        private It should_have_correctly_calculated_the_loanAmountNoFees_value = () =>
        {
            model.LoanAmountNoFees.ShouldEqual(model.CashOut);
        };
    }
}