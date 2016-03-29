using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.ApplicationDomain.Specs.ModelSpecs.ApplicationMailingAddressModelSpecs
{
    public class when_all_properties_are_provided : WithFakes
    {
        private static Exception ex;
        private static ApplicationMailingAddressModel model;
        private static int applicationNumber;
        private static int clientAddressKey;

        private Establish context = () =>
        {
            applicationNumber = 1;
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