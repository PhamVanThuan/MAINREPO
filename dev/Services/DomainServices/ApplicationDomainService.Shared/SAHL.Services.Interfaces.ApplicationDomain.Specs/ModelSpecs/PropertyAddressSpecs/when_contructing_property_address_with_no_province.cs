using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.ApplicationDomain.Specs.ModelSpecs.PropertyAddressSpecs
{
    public class when_contructing_property_address_with_no_province : WithFakes
    {
        private static Exception ex;
        private static PropertyAddressModel model;

        private static string streetNumber;
        private static string streetName;
        private static string suburb;
        private static string city;
        private static string province;
        private static string postalCode;
        private static string erfNumber;
        private static string erfPortionNumber;

        private Establish context = () =>
        {
            streetNumber = "1";
            streetName = "streetname";
            suburb = "suburb";
            city = "city";
            province = null;
            postalCode = "postcode";
            erfNumber = "erfnumber";
            erfPortionNumber = "erfPortion";
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new PropertyAddressModel("","","",streetNumber,streetName,suburb,city,province,postalCode,erfNumber,erfPortionNumber);
            });
        };

        private It should_throw_a_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(System.ComponentModel.DataAnnotations.ValidationException));
        };
    }
}
