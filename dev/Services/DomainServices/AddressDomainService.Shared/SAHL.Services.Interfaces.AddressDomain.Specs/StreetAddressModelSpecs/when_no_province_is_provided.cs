﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.AddressDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.AddressDomain.Specs.StreetAddressModelSpecs
{
    public class when_no_province_is_provided : WithFakes
    {
        private static string unitNumber;
        private static string buildingName;
        private static string buildingNumber;
        private static string streetNumber;
        private static string streetName;
        private static string city;
        private static string province;
        private static string suburb;
        private static string postalCode;
        private static StreetAddressModel model;
        private static Exception ex;

        private Establish context = () =>
        {
            unitNumber = string.Empty;
            buildingName = string.Empty;
            buildingNumber = string.Empty;
            streetNumber = "394";
            streetName = "Esther Roberts Road";
            city = "Durban";
            province = string.Empty;
            suburb = "Glenwood";
            postalCode = "4001";
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new StreetAddressModel(unitNumber, buildingName, buildingNumber, streetNumber, streetName, suburb, city, province, postalCode);
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