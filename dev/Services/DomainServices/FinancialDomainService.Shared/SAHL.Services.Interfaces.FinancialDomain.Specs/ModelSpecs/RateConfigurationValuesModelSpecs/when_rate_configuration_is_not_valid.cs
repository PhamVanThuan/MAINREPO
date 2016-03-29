using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FinancialDomain.Specs.ModelSpecs.RateConfigurationValuesModelSpecs
{
    public class when_rate_configuration_is_not_valid : WithFakes
    {
        static RateConfigurationValuesModel model;
        static Exception ex;

        static int RateConfigurationKey = -1;
        static decimal MarketRateValue = 0.55m;
        static decimal MarginValue = 0.021m;

        Establish context = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new RateConfigurationValuesModel(RateConfigurationKey, MarketRateValue, MarginValue);
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
            ex.Message.ShouldEqual("RateConfigurationKey must be greater than 0.");
        };

    }
}
