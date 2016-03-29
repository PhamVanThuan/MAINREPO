using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ApplicationDomain
{
    [TestFixture]
    public class when_adding_an_applicant_to_an_application : ServiceTestBase<IApplicationDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var query = new GetRandomOpenApplicationQuery();
            base.PerformQuery(query);
            int applicationNumber = query.Result.Results.FirstOrDefault().ApplicationNumber;
            var getClientToAddQuery = new GetClientWhoIsNotAnApplicantOnApplicationQuery(applicationNumber);
            base.PerformQuery(getClientToAddQuery);
            var client = getClientToAddQuery.Result.Results.First();
            AddLeadApplicantToApplicationCommand command = new AddLeadApplicantToApplicationCommand(this.linkedGuid, client.LegalEntityKey, applicationNumber, LeadApplicantOfferRoleTypeEnum.Lead_MainApplicant);
            base.Execute<AddLeadApplicantToApplicationCommand>(command).WithoutErrors();
            var applicationRoles = TestApiClient.Get<OfferRoleDataModel>(new { offerkey = applicationNumber, generalstatuskey = (int)GeneralStatus.Active });
            var newApplicationRole = (from r in applicationRoles
                                      where r.LegalEntityKey.Equals(client.LegalEntityKey)
                                      select r).First();
            Assert.NotNull(newApplicationRole, "Application role details were not added.");
            Assert.That(newApplicationRole.LegalEntityKey == client.LegalEntityKey && newApplicationRole.OfferKey == applicationNumber
                && newApplicationRole.OfferRoleTypeKey == (int)OfferRoleType.Lead_MainApplicant, "Incorrect application role details were added.");
        }

        [Test]
        public void when_unsuccessful()
        {
            var query = new GetRandomOpenApplicationQuery();
            base.PerformQuery(query);
            int applicationNumber = query.Result.Results.FirstOrDefault().ApplicationNumber;
            var applicationRoles = TestApiClient.Get<OfferRoleDataModel>(
                new { offerkey = applicationNumber, generalstatuskey = (int)GeneralStatus.Active });
            var clientKey = (from r in applicationRoles
                             where r.OfferRoleTypeKey == (int)OfferRoleType.Lead_MainApplicant || r.OfferRoleTypeKey == (int)OfferRoleType.MainApplicant
                             select r.LegalEntityKey).FirstOrDefault();
            AddLeadApplicantToApplicationCommand command = new AddLeadApplicantToApplicationCommand(this.linkedGuid, clientKey, applicationNumber, LeadApplicantOfferRoleTypeEnum.Lead_MainApplicant);
            base.Execute<AddLeadApplicantToApplicationCommand>(command)
                .AndExpectThatErrorMessagesContain("The client provided is already an active applicant on this application.");
        }

        [Test]
        public void when_unsuccessful_2()
        {
            var query = new GetRandomOpenApplicationQuery();
            base.PerformQuery(query);
            int applicationNumber = query.Result.Results.FirstOrDefault().ApplicationNumber;
            AddLeadApplicantToApplicationCommand command = new AddLeadApplicantToApplicationCommand(this.linkedGuid, 81547, applicationNumber, LeadApplicantOfferRoleTypeEnum.Lead_MainApplicant);
            string expectedMessage = "An applicant requires at least one valid contact detail (An Email Address, Home, Work or Cell Number).";
            base.Execute<AddLeadApplicantToApplicationCommand>(command)
                .AndExpectThatErrorMessagesContain(expectedMessage);
        }
    }
}