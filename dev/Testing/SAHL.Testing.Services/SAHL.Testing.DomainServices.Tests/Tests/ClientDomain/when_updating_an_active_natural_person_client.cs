using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ClientDomain
{
    public class when_updating_an_active_natural_person_client : ServiceTestBase<IClientDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            GetNaturalPersonClientQuery getNaturalPersonClientQuery = new GetNaturalPersonClientQuery(isActive: true);
            base.PerformQuery(getNaturalPersonClientQuery);
            var client = getNaturalPersonClientQuery.Result.Results.First();
            var activeNaturalPersonClientModel = new ActiveNaturalPersonClientModel(SalutationType.Mr, "Luke", Language.isiZulu, CorrespondenceLanguage.Afrikaans, Education.UniversityDegree,
                "011", "2222222", "011", "3333333", "011", "4444444", "0825555555", "luke@luke.com");
            UpdateActiveNaturalPersonClientCommand command = new UpdateActiveNaturalPersonClientCommand(client.LegalEntityKey, activeNaturalPersonClientModel);
            base.Execute<UpdateActiveNaturalPersonClientCommand>(command);
            var existingActiveClient = TestApiClient.GetByKey<LegalEntityDataModel>(client.LegalEntityKey);
            Assert.That(existingActiveClient.PreferredName == activeNaturalPersonClientModel.PreferredName);
            Assert.That(existingActiveClient.HomeLanguageKey == (int)activeNaturalPersonClientModel.HomeLanguage);
            Assert.That(existingActiveClient.DocumentLanguageKey == (int)activeNaturalPersonClientModel.CorrespondenceLanguage);
            Assert.That(existingActiveClient.EducationKey == (int)activeNaturalPersonClientModel.Education);
            Assert.That(existingActiveClient.HomePhoneCode == activeNaturalPersonClientModel.HomePhoneCode);
            Assert.That(existingActiveClient.HomePhoneNumber == activeNaturalPersonClientModel.HomePhone);
            Assert.That(existingActiveClient.WorkPhoneCode == activeNaturalPersonClientModel.WorkPhoneCode);
            Assert.That(existingActiveClient.WorkPhoneNumber == activeNaturalPersonClientModel.WorkPhone);
            Assert.That(existingActiveClient.FaxCode == activeNaturalPersonClientModel.FaxCode);
            Assert.That(existingActiveClient.FaxNumber == activeNaturalPersonClientModel.FaxNumber);
            Assert.That(existingActiveClient.CellPhoneNumber == activeNaturalPersonClientModel.Cellphone);
            Assert.That(existingActiveClient.EmailAddress == activeNaturalPersonClientModel.EmailAddress);
        }

        [Test]
        public void when_unsuccessful()
        {
            GetNaturalPersonClientQuery getNaturalPersonClientQuery = new GetNaturalPersonClientQuery(isActive: true);
            base.PerformQuery(getNaturalPersonClientQuery);
            var client = getNaturalPersonClientQuery.Result.Results.First();
            var activeNaturalPersonClientModel = new ActiveNaturalPersonClientModel(SalutationType.Mr, "Luke", Language.isiZulu, CorrespondenceLanguage.Afrikaans, Education.UniversityDegree,
                "", "", "", "", "", "", "", "");
            UpdateActiveNaturalPersonClientCommand command = new UpdateActiveNaturalPersonClientCommand(client.LegalEntityKey, activeNaturalPersonClientModel);
            base.Execute<UpdateActiveNaturalPersonClientCommand>(command).AndExpectThatErrorMessagesContain("At least one valid contact detail (An Email Address, Home, Work or Cell Number) is required.");
        }
    }
}