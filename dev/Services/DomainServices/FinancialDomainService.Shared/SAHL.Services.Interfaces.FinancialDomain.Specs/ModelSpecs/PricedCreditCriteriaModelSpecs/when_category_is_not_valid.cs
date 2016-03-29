using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FinancialDomain.Specs.ModelSpecs.PricedCreditCriteriaModelSpecs
{
    public class when_category_is_not_valid : WithFakes
    {
        static PricedCreditCriteriaModel model;
        static Exception ex;

        static int CreditCriteriaKey = 2;
        static int CreditMatrixKey = 3;
        static int CategoryKey = -1;

        Establish context = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new PricedCreditCriteriaModel(CreditCriteriaKey, CreditMatrixKey, CategoryKey);
            });
        };

        It should_throw_a_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(ValidationException));
        };

        It should_not_construct_the_model = () =>
        {
            model.ShouldBeNull();
        };

        It should_return_a_message = () =>
        {
            ex.Message.ShouldEqual("CategoryKey must be provided.");
        };

    }
}
