using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.NaturalPersonClientModelSpecs
{
    public class when_all_properties_are_provided : WithFakes
    {
        private static string idNumber;
        private static string passportNumber;
        private static SalutationType salutationTypeKey;
        private static string firstName;
        private static string surname;
        private static string preferredName;
        private static Gender genderKey;
        private static MaritalStatus maritalStatusKey;
        private static PopulationGroup populationGroupKey;
        private static CitizenType citizenshipTypeKey;
        private static DateTime dateOfBirth;
        private static Language homeLanguageKey;
        private static CorrespondenceLanguage correspondenceLanguageKey;
        private static Exception ex;
        private static NaturalPersonClientModel model;
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
            workPhone = "6666666";
            faxCode = "011";
            faxNumber = "7777777";
            cellphone = "0555555555";
            idNumber = "1234567890";
            emailAddress = "bob@another.com";
            passportNumber = string.Empty;
            salutationTypeKey = SalutationType.Mr;
            firstName = "Firstname";
            surname = "Surname";
            preferredName = "preferredName";
            genderKey = Gender.Female;
            maritalStatusKey = MaritalStatus.Divorced;
            populationGroupKey = PopulationGroup.African;
            citizenshipTypeKey = CitizenType.SACitizen;
            dateOfBirth = DateTime.Now;
            homeLanguageKey = Language.Afrikaans;
            correspondenceLanguageKey = CorrespondenceLanguage.Afrikaans;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new NaturalPersonClientModel(idNumber, passportNumber, salutationTypeKey, firstName, surname,
                                                    "i", preferredName, genderKey, maritalStatusKey, populationGroupKey,
                                                    citizenshipTypeKey, dateOfBirth, homeLanguageKey, correspondenceLanguageKey,
                                                    education, homePhoneCode, homePhone, workPhoneCode, workPhone, faxCode,
                                                    faxNumber, cellphone, emailAddress);
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