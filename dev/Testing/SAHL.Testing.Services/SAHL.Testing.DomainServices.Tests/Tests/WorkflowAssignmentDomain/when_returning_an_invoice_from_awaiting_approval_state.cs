using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Commands;
using System.Linq;
using SAHL.Core.Testing;
using SAHL.Core.Data.Models.FETest;
using System;

namespace SAHL.Testing.Services.Tests.WorkflowAssignmentDomain
{
    [TestFixture]
    public class when_returning_an_invoice_from_awaiting_approval_state : ServiceTestBase<IWorkflowAssignmentDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var openInvoice = TestApiClient.GetAny<OpenThirdPartyInvoicesDataModel>(new { workflowstate = "Accepted for Processing" }, 50);
            AssignCaseToInvoiceProcessor(openInvoice);
            openInvoice = TestApiClient.Get<OpenThirdPartyInvoicesDataModel>(new { thirdpartyinvoicekey = openInvoice.ThirdPartyInvoiceKey }).First();
            var userDetails = TestApiClient.Get<HaloUsersDataModel>(new { adusername = openInvoice.AssignedUser }).First();
            var newUser = TestApiClient.Get<HaloUsersDataModel>(new { adusername = @"InvoicePmtProcessor" })
                .Where(x=>x.Capabilities.Length > 0)
                .First();
            //we need to assign it to another capability
            var assignCommand = new AssignWorkflowCaseCommand(GenericKeyType.ThirdPartyInvoice, openInvoice.ThirdPartyInvoiceKey, openInvoice.InstanceID,
                newUser.UserOrganisationStructureKey, Capability.InvoicePaymentProcessor);
            base.Execute(assignCommand).WithoutErrors();
            //now we return it
            var command = new ReturnInstanceToLastUserInCapabilityCommand(GenericKeyType.ThirdPartyInvoice, openInvoice.ThirdPartyInvoiceKey, Capability.InvoiceProcessor, openInvoice.InstanceID);
            base.Execute(command);
            Assert.That(!base.messages.AllMessages.Any());
            openInvoice = TestApiClient.GetByKey<OpenThirdPartyInvoicesDataModel>(openInvoice.ThirdPartyInvoiceKey);
            Assert.That(openInvoice.AssignedUser.Equals(userDetails.ADUserName), string.Format(@"Expected ADUser {0} but was {1}", userDetails.ADUserName, openInvoice.AssignedUser));
        }

        private void AssignCaseToInvoiceProcessor(OpenThirdPartyInvoicesDataModel openInvoice)
        {
            var userDetails = TestApiClient.Get<HaloUsersDataModel>(new { adusername = "InvoiceProcessor" }).First();
            //we need to assign it to another capability
            var assignCommand = new AssignWorkflowCaseCommand(GenericKeyType.ThirdPartyInvoice, openInvoice.ThirdPartyInvoiceKey, openInvoice.InstanceID,
                userDetails.UserOrganisationStructureKey, Capability.InvoiceProcessor);
            var messages = base.Execute(assignCommand);
        }

        [Test]
        public void when_capability_never_worked_on_the_case()
        {
            Capability capability = Capability.LossControlFeeInvoiceApproverUnderR15000;
            long instanceId = 8918053L;
            var command = new ReturnInstanceToLastUserInCapabilityCommand(GenericKeyType.ThirdPartyInvoice, 1, capability, instanceId);
            base.Execute(command)
                .AndExpectThatErrorMessagesContain(string.Format("The case could not be returned to role: {0}. A user with that role has not previously worked on this case.",
                    capability.ToString()));
        }
    }
}