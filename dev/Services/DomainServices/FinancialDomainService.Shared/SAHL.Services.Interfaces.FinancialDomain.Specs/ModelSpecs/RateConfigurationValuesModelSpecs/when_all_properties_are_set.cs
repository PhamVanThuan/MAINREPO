using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FinancialDomain.Specs.ModelSpecs.RateConfigurationValuesModelSpecs
{
    class when_all_properties_are_set : WithFakes
    {
        static RateConfigurationValuesModel model;
        static Exception ex;

        static int RateConfigurationKey = 2;
        static decimal MarketRateValue = 0.55m;
        static decimal MarginValue = 0.021m;

        Establish context = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new RateConfigurationValuesModel(RateConfigurationKey, MarketRateValue, MarginValue);
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