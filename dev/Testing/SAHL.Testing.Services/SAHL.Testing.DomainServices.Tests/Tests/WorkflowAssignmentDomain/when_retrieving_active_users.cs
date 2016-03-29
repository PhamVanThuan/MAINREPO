using System.Linq;
using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain;
using SAHL.Services.Interfaces.WorkflowAssignmentDomain.Queries;

namespace SAHL.Testing.Services.Tests.WorkflowAssignmentDomain
{
    public class when_retrieving_active_users : ServiceTestBase<IWorkflowAssignmentDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var getActiveUsersWithCapabilityQuery = new GetActiveUsersWithCapabilityQuery((int)Capability.InvoiceProcessor);
            base.Execute<GetActiveUsersWithCapabilityQuery>(getActiveUsersWithCapabilityQuery);
            var result = getActiveUsersWithCapabilityQuery.Result;
            foreach (var item in getActiveUsersWithCapabilityQuery.Result.Results)
            {
                var user = TestApiClient.Get<HaloUsersDataModel>(new { userorganisationstructurekey = item.UserOrganisationStructureKey }).First();
                Assert.That(user.Capabilities.Contains("Invoice Processor"), 
                    string.Format("User: {0} did not contain the Invoice Processor Capability but was returned by the query.", item.UserName));
            }
            Assert.That(result.Results.Any() == true);
        }
    }
}