using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.FixedLongTermInvestmentLiabilityModelSpecs
{
    public class when_no_company_name_is_provided : WithFakes
    {
        private static string companyName;
        private static int liabilityValue;
        private static FixedLongTermInvestmentLiabilityModel model;
        private static Exception ex;

        private Establish context = () =>
        {
            companyName = string.Empty;
            liabilityValue = 1000;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new FixedLongTermInvestmentLiabilityModel(companyName, liabilityValue);
            });
        };

        private It should_throw_a_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(ValidationException));
        };

        private It should_contain_a_message = () =>
        {
            ex.Message.ShouldEqual("The CompanyName field is required.");
        };
    }
}
