using System;
using System.Linq;
using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Testing.Services.Tests.ApplicationDomain
{
    public class when_linking_domicilium_address_to_applicant : ServiceTestBase<IApplicationDomainServiceClient>
    {
        private ActiveNewBusinessApplicantsDataModel newBusinessApplicant;

        [TearDown]
        new public void OnTestTearDown()
        {
            if (newBusinessApplicant != null)
            {
                var command = new RemoveApplicantFromActiveNewBusinessApplicantsCommand(newBusinessApplicant.OfferRoleKey);
                base.PerformCommand(command);
                newBusinessApplicant = null;
            }
            base.OnTestTearDown();
        }

        [Test]
        public void when_successful()
        {
            newBusinessApplicant = TestApiClient.Get<ActiveNewBusinessApplicantsDataModel>(new { hasdomicilium = 0, hasresidentialaddress = 1 }, limit: 1).First();
            //get the applicant's residential address
            var address = TestApiClient.Get<LegalEntityAddressDataModel>(new { legalentitykey = newBusinessApplicant.LegalEntityKey, addresstypekey = (int)AddressType.Residential })
                .Where(x => x.GeneralStatusKey == (int)GeneralStatus.Active).FirstOrDefault();
            var domicilium = TestApiClient.Get<LegalEntityDomiciliumDataModel>(new { legalentityaddresskey = address.LegalEntityAddressKey, generalstatuskey = (int)GeneralStatus.Pending })
                .FirstOrDefault();
            if (domicilium == null)
            {
                //add it as a pending domicilium
                var messages = base.clientDomainManager.AddPendingClientDomicilium(address.LegalEntityAddressKey, newBusinessApplicant.LegalEntityKey);
                if (messages.HasErrors)
                {
                    Assert.Fail("Pending Domicilium setup failed. Message: {0}. LegalEntityAddressKey: {1}", messages.ErrorMessages().First().Message, address.LegalEntityAddressKey);
                }
                domicilium = TestApiClient.Get<LegalEntityDomiciliumDataModel>(new { legalentityaddresskey = address.LegalEntityAddressKey, generalstatuskey = (int)GeneralStatus.Pending })
                    .First();
            }
            var applicantDomiciliumAddressModel = new ApplicantDomiciliumAddressModel(domicilium.LegalEntityDomiciliumKey, newBusinessApplicant.OfferKey, newBusinessApplicant.LegalEntityKey);
            var command = new LinkDomiciliumAddressToApplicantCommand(applicantDomiciliumAddressModel);
            base.Execute<LinkDomiciliumAddressToApplicantCommand>(command).WithoutErrors();
            var offerRoleDomicilium = TestApiClient.Get<OfferRoleDomiciliumDataModel>(new { offerrolekey = newBusinessApplicant.OfferRoleKey }).First();
            Assert.IsNotNull(offerRoleDomicilium,
                string.Format("Test failed for ClientDomiciliumKey: {0}, OfferKey: {1}, LegalEntityKey: {2}", command.ApplicantDomicilium.ClientDomiciliumKey,
                command.ApplicationNumber, command.ClientKey));
        }

        [Test]
        public void when_unsuccessful()
        {
            newBusinessApplicant = TestApiClient.Get<ActiveNewBusinessApplicantsDataModel>(new { hasdomicilium = 0, hasresidentialaddress = 1 }, limit: 1).First();
            var applicantDomiciliumAddressModel = new ApplicantDomiciliumAddressModel(Int32.MaxValue, newBusinessApplicant.OfferKey, newBusinessApplicant.LegalEntityKey);
            var command = new LinkDomiciliumAddressToApplicantCommand(applicantDomiciliumAddressModel);
            base.Execute<LinkDomiciliumAddressToApplicantCommand>(command)
                .AndExpectThatErrorMessagesContain("An applicant can only be linked to a domicilium address that is marked as pending.");
        }
    }
}