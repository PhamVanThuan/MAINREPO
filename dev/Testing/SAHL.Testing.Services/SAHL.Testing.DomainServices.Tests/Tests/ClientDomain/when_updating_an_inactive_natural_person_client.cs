using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ClientDomain
{
    [TestFixture]
    public class when_updating_an_inactive_natural_person_client : ServiceTestBase<IClientDomainServiceClient>
    {
        private int _clientKey;

        [SetUp]
        public void SetUp()
        {
            GetNaturalPersonClientQuery query = new GetNaturalPersonClientQuery(false);
            base.PerformQuery(query);
            _clientKey = query.Result.Results.First().LegalEntityKey;
        }

        [TearDown]
        public void TearDown()
        {
            _clientKey = 0;
        }

        [Test]
        public void when_successful()
        {
            var query = new GetUnusedIDNumberQuery();
            base.PerformQuery(query);
            string idNumber = query.Result.Results.FirstOrDefault().IDNumber;
            var naturalPersonClientModel = new NaturalPersonClientModel(idNumber, string.Empty, SalutationType.Mr, "John", "Lock", "J", "John", Gender.Male, MaritalStatus.Single,
                PopulationGroup.White, CitizenType.SACitizen,
                DateTime.Now.AddYears(-30), Language.English, CorrespondenceLanguage.English, Education.UniversityDegree, "011", "5555555", "011", "6666666", "011", "7777777", "0823333333",
                "john@lock.com");
            UpdateInactiveNaturalPersonClientCommand command = new UpdateInactiveNaturalPersonClientCommand(_clientKey, naturalPersonClientModel);
            base.Execute<UpdateInactiveNaturalPersonClientCommand>(command);
            var existingInactiveClient = TestApiClient.GetByKey<LegalEntityDataModel>(_clientKey);
            TimeSpan dateOfBirthCheck = Convert.ToDateTime(existingInactiveClient.DateOfBirth) - Convert.ToDateTime(naturalPersonClientModel.DateOfBirth.Value);
            Assert.That(existingInactiveClient.IDNumber == naturalPersonClientModel.IDNumber, "IDNumber not updated for inactive client");
            Assert.That(existingInactiveClient.PassportNumber == naturalPersonClientModel.PassportNumber, "PassportNumber not updated for inactive client");
            Assert.That(existingInactiveClient.Salutationkey == (int)naturalPersonClientModel.Salutation, " Salutation not updated for inactive client");
            Assert.That(existingInactiveClient.FirstNames == naturalPersonClientModel.FirstName, "FirstName not updated for inactive client");
            Assert.That(existingInactiveClient.Surname == naturalPersonClientModel.Surname, "Surname not updated for inactive client");
            Assert.That(existingInactiveClient.PreferredName == naturalPersonClientModel.PreferredName, "PreferredName not updated for inactive client");
            Assert.That(existingInactiveClient.GenderKey == (int)naturalPersonClientModel.Gender, "Gender not updated for inactive client");
            Assert.That(existingInactiveClient.MaritalStatusKey == (int)naturalPersonClientModel.MaritalStatus, "MaritalStatus not updated for inactive client");
            Assert.That(existingInactiveClient.PopulationGroupKey == (int)naturalPersonClientModel.PopulationGroup, "PopulationGroup not updated for inactive client");
            Assert.That(existingInactiveClient.CitizenTypeKey == (int)naturalPersonClientModel.CitizenshipType, "CitizenshipType not updated for inactive client");
            Assert.That(dateOfBirthCheck.Days == 0, "not updated for inactive client");
            Assert.That(existingInactiveClient.HomeLanguageKey == (int)naturalPersonClientModel.HomeLanguage, "HomeLanguage not updated for inactive client");
            Assert.That(existingInactiveClient.DocumentLanguageKey == (int)naturalPersonClientModel.CorrespondenceLanguage, "CorrespondenceLanguage not updated for inactive client");
            Assert.That(existingInactiveClient.EducationKey == (int)naturalPersonClientModel.Education, "Education not updated for inactive client");
            Assert.That(existingInactiveClient.HomePhoneCode == naturalPersonClientModel.HomePhoneCode, "HomePhoneCode not updated for inactive client");
            Assert.That(existingInactiveClient.HomePhoneNumber == naturalPersonClientModel.HomePhone, "HomePhone not updated for inactive client");
            Assert.That(existingInactiveClient.WorkPhoneCode == naturalPersonClientModel.WorkPhoneCode, "WorkPhoneCode not updated for inactive client");
            Assert.That(existingInactiveClient.WorkPhoneNumber == naturalPersonClientModel.WorkPhone, "WorkPhone not updated for inactive client");
            Assert.That(existingInactiveClient.FaxCode == naturalPersonClientModel.FaxCode, "FaxCode not updated for inactive client");
            Assert.That(existingInactiveClient.FaxNumber == naturalPersonClientModel.FaxNumber, "FaxNumber not updated for inactive client");
            Assert.That(existingInactiveClient.CellPhoneNumber == naturalPersonClientModel.Cellphone, "Cellphone not updated for inactive client");
            Assert.That(existingInactiveClient.EmailAddress == naturalPersonClientModel.EmailAddress, "EmailAddress not updated for inactive client");
        }

        [Test]
        public void when_unsuccessful()
        {
            var naturalPersonClientModel = new NaturalPersonClientModel(string.Empty, string.Empty, SalutationType.Mr, "John", "Lock", "J", "John", Gender.Male, MaritalStatus.Single, PopulationGroup.White, CitizenType.SACitizen,
                DateTime.Now.AddYears(-30), Language.English, CorrespondenceLanguage.English, Education.UniversityDegree, "", "", "", "", "", "", "", "");
            UpdateInactiveNaturalPersonClientCommand command = new UpdateInactiveNaturalPersonClientCommand(_clientKey, naturalPersonClientModel);
            base.Execute<UpdateInactiveNaturalPersonClientCommand>(command).AndExpectThatErrorMessagesContain("At least one valid contact detail (An Email Address, Home, Work or Cell Number) is required.");
        }
    }
}