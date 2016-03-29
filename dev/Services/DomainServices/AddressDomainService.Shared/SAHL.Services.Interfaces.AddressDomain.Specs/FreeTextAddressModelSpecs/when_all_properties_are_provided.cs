using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.AddressDomain.Specs.FreeTextAddressModelSpecs
{
    public class when_all_properties_are_provided : WithFakes
    {
        private static string freeText1, freeText2, freeText3, freeText4, freeText5, country;
        private static AddressFormat addressFormat;
        private static FreeTextAddressModel model;
        private static Exception ex;

        private Establish context = () =>
        {
            freeText1 = "12 Twelfth street";
            freeText2 = "Morningside";
            freeText3 = "Durban";
            freeText4 = "Kwazulu-Natal";
            freeText5 = "4001";
            country = "Australia";
            addressFormat = AddressFormat.FreeText;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new FreeTextAddressModel(addressFormat, freeText1, freeText2, freeText3, freeText4, freeText5, country);
            });
        };

        private It should_not_throw_a_validation_exception = () =>
        {
            ex.ShouldBeNull();
        };

        private It should_construct_a_model = () =>
        {
            model.ShouldNotBeNull();
        };
    }
}