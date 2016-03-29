using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.AddressDomain.Specs.PostalAddressModelSpecs
{
    public class when_all_properties_are_provided : WithFakes
    {
        private static string boxNumber;
        private static string postNetSuiteNumber;
        private static string city;
        private static string province;
        private static string postOffice;
        private static string postalCode;
        private static AddressFormat addressFormat;
        private static PostalAddressModel model;
        private static Exception ex;

        private Establish context = () =>
        {
            boxNumber = "12";
            postNetSuiteNumber = string.Empty;
            city = "Durban";
            province = "Kwazulu-Natal";
            postOffice = "Durban";
            postalCode = "4001";
            addressFormat = AddressFormat.Box;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new PostalAddressModel(boxNumber, postNetSuiteNumber, postOffice, province, city, postalCode, addressFormat);
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