using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.AddressDomain.Specs.FreeTextAddressModelSpecs
{
    public class when_country_is_not_provided : WithFakes
    {
        private static string freeText1, freeText2, freeText3, freeText4, freeText5, country;
        private static AddressFormat addressFormat;
        private static FreeTextAddressModel model;
        private static Exception ex;

        private Establish context = () =>
        {
            freeText1 = "42 Wallaby Way";
            freeText2 = "Sydney";
            freeText3 = "";
            freeText4 = "";
            freeText5 = "";
            country = "";
            addressFormat = AddressFormat.FreeText;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new FreeTextAddressModel(addressFormat, freeText1, freeText2, freeText3, freeText4, freeText5, country);
            });
        };

        private It should_throw_a_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(System.ComponentModel.DataAnnotations.ValidationException));
        };

        private It should_contain_an_message = () =>
        {
            ex.Message.ShouldEqual("The Country field is required.");
        };
    }
}