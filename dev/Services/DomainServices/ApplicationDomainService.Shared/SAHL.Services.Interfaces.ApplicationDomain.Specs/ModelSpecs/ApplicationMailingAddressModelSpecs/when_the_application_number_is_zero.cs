using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.ApplicationDomain.Specs.ModelSpecs.ApplicationMailingAddressModelSpecs
{
    public class when_the_application_number_is_zero : WithFakes
    {
        private static Exception ex;
        private static ApplicationMailingAddressModel model;
        private static int applicationNumber;
        private static int clientAddressKey;

        private Establish context = () =>
        {
            applicationNumber = 0;
            clientAddressKey = 1;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new ApplicationMailingAddressModel(applicationNumber, clientAddressKey,
                    CorrespondenceLanguage.English, OnlineStatementFormat.PDFFormat, CorrespondenceMedium.Email, 1,
                    true);
            });
        };

        private It should_throw_a_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(ValidationException));
        };

        private It should_return_a_message = () =>
        {
            ex.Message.ShouldEqual("An Application Number must be provided.");
        };
    }
}