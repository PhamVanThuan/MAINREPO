using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.AddressDomain.Specs.PostalAddressModelSpecs
{
    public class when_no_province_is_provided : WithFakes
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
            province = string.Empty;
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

        private It should_throw_a_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(System.ComponentModel.DataAnnotations.ValidationException));
        };

        private It should_contain_an_message = () =>
        {
            ex.Message.ShouldEqual("The Province field is required.");
        };
    }
}