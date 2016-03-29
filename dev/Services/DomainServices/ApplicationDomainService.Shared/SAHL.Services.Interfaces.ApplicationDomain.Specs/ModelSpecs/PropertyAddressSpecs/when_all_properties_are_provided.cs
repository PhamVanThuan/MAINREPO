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
    public class when_all_properties_are_provided : WithFakes
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
            streetName = "street name";
            suburb = "suburb";
            city = "city";
            province = "province";
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

        private It should_not_throw_a_validation_exception = () =>
        {
            ex.ShouldBeNull();
        };

        private It should_construct_the_model = () =>
        {
            model.ShouldNotBeNull();
        };
    }
}
