using System.Linq;
using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Queries;

namespace SAHL.Testing.Services.Tests.WorkflowAssignmentDomain
{
    [TestFixture]
    public class when_resolving_a_workflow_role : ServiceTestBase<IWorkflowAssignmentDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var openInvoice = TestApiClient.GetAny<OpenThirdPartyInvoicesDataModel>(new { workflowstate = "Accepted for Processing" }, 50);
            var query = new GetUserCurrentlyAssignedInCapabilityQuery(openInvoice.InstanceID, (int)Capability.InvoiceProcessor);
            base.Execute<GetUserCurrentlyAssignedInCapabilityQuery>(query);
            if (query.Result == null || query.Result.Results.Count() == 0)
            {
                Assert.Fail(string.Format("Failed to retrieve any user for InstanceId: {0} for Capability: {1}", openInvoice.InstanceID, Capability.InvoiceProcessor.ToString()));
            }
            var result = query.Result;
            var actualUser = result.Results.First().UserName;
            Assert.That(actualUser.Contains(openInvoice.AssignedUser), 
                string.Format(@"Failed to resolve role for InstanceID: {0}. Expected: {1}, Was: {2}", openInvoice.InstanceID, openInvoice.AssignedUser, actualUser));
        }

        [Test]
        public void when_unsuccessful()
        {
            var query = new GetUserCurrentlyAssignedInCapabilityQuery(111112222222222L, (int)Capability.InvoiceProcessor);
            base.Execute<GetUserCurrentlyAssignedInCapabilityQuery>(query);
            var result = query.Result;
            Assert.That(result.Results.Count().Equals(0));
        }
    }
}