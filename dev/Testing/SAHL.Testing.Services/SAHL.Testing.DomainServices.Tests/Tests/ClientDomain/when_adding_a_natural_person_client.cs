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
    public class when_adding_a_natural_person_client : ServiceTestBase<IClientDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var query = new GetUnusedIDNumberQuery();
            base.PerformQuery(query);
            string idNumber = query.Result.Results.FirstOrDefault().IDNumber;
            var dateOfBirth = DateTime.Now.AddYears(-30);
            var naturalPersonClientModel = new NaturalPersonClientModel(idNumber, string.Empty, SalutationType.Mr, "John", "Lock", "J", "John", Gender.Male, MaritalStatus.Single, PopulationGroup.White, CitizenType.SACitizen,
                dateOfBirth, Language.English, CorrespondenceLanguage.English, Education.UniversityDegree, "011", "5555555", "011", "6666666", "011", "7777777", "0823333333", "john@lock.com");
            AddNaturalPersonClientCommand command = new AddNaturalPersonClientCommand(naturalPersonClientModel);
            base.Execute<AddNaturalPersonClientCommand>(command);
            this.linkedKey = linkedKeyManager.RetrieveLinkedKey(command.Id);
            var newClient = TestApiClient.GetByKey<LegalEntityDataModel>(linkedKey);
            Assert.NotNull(newClient, "Client details were not added.");
            TimeSpan dateOfBirthCheck = Convert.ToDateTime(newClient.DateOfBirth) - Convert.ToDateTime(naturalPersonClientModel.DateOfBirth.Value);
            Assert.That(newClient.IDNumber == naturalPersonClientModel.IDNumber);
            Assert.That(newClient.PassportNumber == naturalPersonClientModel.PassportNumber);
            Assert.That(newClient.Salutationkey == (int)naturalPersonClientModel.Salutation);
            Assert.That(newClient.FirstNames == naturalPersonClientModel.FirstName);
            Assert.That(newClient.Surname == naturalPersonClientModel.Surname);
            Assert.That(newClient.PreferredName == naturalPersonClientModel.PreferredName);
            Assert.That(newClient.GenderKey == (int)naturalPersonClientModel.Gender);
            Assert.That(newClient.MaritalStatusKey == (int)naturalPersonClientModel.MaritalStatus);
            Assert.That(newClient.PopulationGroupKey == (int)naturalPersonClientModel.PopulationGroup);
            Assert.That(newClient.CitizenTypeKey == (int)naturalPersonClientModel.CitizenshipType);
            Assert.That(dateOfBirthCheck.Days == 0);
            Assert.That(newClient.HomeLanguageKey == (int)naturalPersonClientModel.HomeLanguage);
            Assert.That(newClient.DocumentLanguageKey == (int)naturalPersonClientModel.CorrespondenceLanguage);
            Assert.That(newClient.EducationKey == (int)naturalPersonClientModel.Education);
            Assert.That(newClient.HomePhoneCode == naturalPersonClientModel.HomePhoneCode);
            Assert.That(newClient.HomePhoneNumber == naturalPersonClientModel.HomePhone);
            Assert.That(newClient.WorkPhoneCode == naturalPersonClientModel.WorkPhoneCode);
            Assert.That(newClient.WorkPhoneNumber == naturalPersonClientModel.WorkPhone);
            Assert.That(newClient.FaxCode == naturalPersonClientModel.FaxCode);
            Assert.That(newClient.FaxNumber == naturalPersonClientModel.FaxNumber);
            Assert.That(newClient.CellPhoneNumber == naturalPersonClientModel.Cellphone);
            Assert.That(newClient.EmailAddress == naturalPersonClientModel.EmailAddress);
        }

        [Test]
        public void when_unsuccessful()
        {
            var query = new GetUnusedIDNumberQuery();
            base.PerformQuery(query);
            string IdNumber = query.Result.Results.FirstOrDefault().IDNumber;
            var naturalPersonClientModel = new NaturalPersonClientModel(string.Empty, IdNumber, SalutationType.Mr, "John", "Lock", "J", "John", Gender.Male, MaritalStatus.Single, PopulationGroup.White, CitizenType.SACitizen,
                DateTime.Now.AddYears(-30), Language.English, CorrespondenceLanguage.English, Education.UniversityDegree, "", "", "", "", "", "", "", "");
            AddNaturalPersonClientCommand command = new AddNaturalPersonClientCommand(naturalPersonClientModel);
            base.Execute<AddNaturalPersonClientCommand>(command).AndExpectThatErrorMessagesContain("A passport number cannot be a valid identity number.");
            var getClientQuery = new GetClientByIdNumberQuery(IdNumber);
            base.PerformQuery(getClientQuery);
            Assert.That(getClientQuery.Result.Results.Count() == 0, "Client details shouldn't have been added.");
        }

        [Test]
        public void when_adding_with_minimum_details()
        {
            var naturalPersonClientModel = new NaturalPersonClientModel(string.Empty, null, null, "John", "Smith", "J", string.Empty, null, null, null, null,
                    null, null, null, null, "031", "7685451", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            AddNaturalPersonClientCommand command = new AddNaturalPersonClientCommand(naturalPersonClientModel);
            base.Execute<AddNaturalPersonClientCommand>(command);
            this.linkedKey = linkedKeyManager.RetrieveLinkedKey(command.Id);
            var newClient = TestApiClient.GetByKey<LegalEntityDataModel>(linkedKey);
            Assert.NotNull(newClient, "Client details were not added.");
            Assert.That(newClient.FirstNames == naturalPersonClientModel.FirstName);
            Assert.That(newClient.Surname == naturalPersonClientModel.Surname);
            Assert.That(newClient.HomePhoneCode == naturalPersonClientModel.HomePhoneCode);
            Assert.That(newClient.HomePhoneNumber == naturalPersonClientModel.HomePhone);
        }
    }
}