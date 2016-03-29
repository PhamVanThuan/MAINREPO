using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FinancialDomain.Specs.ModelSpecs.PricedCreditCriteriaModelSpecs
{
    public class when_all_properties_are_set : WithFakes
    {
        static PricedCreditCriteriaModel model;
        static Exception ex;

        static int CreditCriteriaKey = 2;
        static int CreditMatrixKey = 3;
        static int CategoryKey = 4;

        Establish context = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new PricedCreditCriteriaModel(CreditCriteriaKey, CreditMatrixKey, CategoryKey);
            });
        };

        It should_not_throw_a_validation_exception = () =>
        {
            ex.ShouldBeNull();
        };

        It should_construct_the_model = () =>
        {
            model.ShouldNotBeNull();
        };
    }
}
