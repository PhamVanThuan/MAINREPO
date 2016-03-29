using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.ActiveNaturalPersonClientModelSpecs
{
    public class when_all_properties_are_provided : WithFakes
    {
        private static SalutationType salutationTypeKey;
        private static string preferredName;
        private static Language homeLanguageKey;
        private static CorrespondenceLanguage correspondenceLanguageKey;
        private static Exception ex;
        private static ActiveNaturalPersonClientModel model;
        private static Education education = Education.Other;
        private static string homePhoneCode;
        private static string homePhone;
        private static string workPhoneCode;
        private static string workPhone;
        private static string faxCode;
        private static string faxNumber;
        private static string cellphone;
        private static string emailAddress;

        private Establish context = () =>
        {
            homePhoneCode = "031";
            homePhone = "5555555";
            workPhoneCode = "021";
            faxCode = "011";
            faxNumber = "7777777";
            cellphone = "0555555555";
            emailAddress = "bob@another.com";
            salutationTypeKey = SalutationType.Mr;
            preferredName = "preferredName";
            homeLanguageKey = Language.Afrikaans;
            correspondenceLanguageKey = CorrespondenceLanguage.Afrikaans;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new ActiveNaturalPersonClientModel(salutationTypeKey, preferredName, homeLanguageKey, 
                                                            correspondenceLanguageKey, education, homePhoneCode, 
                                                            homePhone, workPhoneCode, workPhone, faxCode, 
                                                            faxNumber, cellphone, emailAddress);
            });
        };

        private It should_not_throw_any_exceptions = () =>
        {
            ex.ShouldBeNull();
        };

        private It should_construct_the_model = () =>
        {
            model.ShouldNotBeNull();
        };
    }
}