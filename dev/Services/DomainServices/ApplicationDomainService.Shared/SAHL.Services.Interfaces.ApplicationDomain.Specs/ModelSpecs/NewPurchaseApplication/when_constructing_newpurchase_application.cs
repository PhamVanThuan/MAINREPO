using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.ApplicationDomain.Specs.ModelSpecs.NewPurchaseApplication
{
    public class when_constructing_newpurchase_application : WithFakes
    {
        private static NewPurchaseApplicationModel model;

        Because of = () =>
        {
            model = new NewPurchaseApplicationModel(OfferStatus.Open, 1, OriginationSource.SAHomeLoans, 1, 1, 1, 0, Product.NewVariableLoan, "reference1", 1, null);
        };

        It should_have_a_newpurchse_application_type_key = () =>
        {
            model.ApplicationType.ShouldEqual(OfferType.NewPurchaseLoan);
        };

        It should_have_correctly_calculated_the_loanAmountNoFees_value = () =>
        {
            model.LoanAmountNoFees.ShouldEqual(model.PurchasePrice - model.Deposit);
        };
    }
}