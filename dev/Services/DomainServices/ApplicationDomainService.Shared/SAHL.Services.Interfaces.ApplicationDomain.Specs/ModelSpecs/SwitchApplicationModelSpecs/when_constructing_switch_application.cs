using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.ApplicationDomain.Specs.ModelSpecs.SwitchApplicationModelSpecs
{
    public class when_constructing_switch_application : WithFakes
    {
        private static SwitchApplicationModel model;

        private Because of = () =>
        {
            model = new SwitchApplicationModel(OfferStatus.Open, 1, OriginationSource.SAHomeLoans, 1, 1, 1, 0, Product.NewVariableLoan, "reference1", 1);
        };

        private It should_have_a_newpurchse_application_type_key = () =>
        {
            model.ApplicationType.ShouldEqual(OfferType.SwitchLoan);
        };

        private It should_have_correctly_calculated_the_loanAmountNoFees_value = () =>
        {
            model.LoanAmountNoFees.ShouldEqual(model.CashOut+model.ExistingLoan);
        };
    }
}