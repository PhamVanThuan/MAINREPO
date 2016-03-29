using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SAHL.Core.Rules;
using SAHL.Services.WorkflowAssignmentDomain.Rules;
using SAHL.Services.WorkflowAssignmentDomain.Rules.Models;

namespace SAHL.Config.Services.WorkflowAssignmentDomain.Server.Tests
{
    [TestFixture]
    public class IocRegistryTests
    {
        [Test]
        public void GivenValidConfig_CanRetrieveInstanceOfUserOrganisationStructureMustHaveCapabilityRule()
        {
            var bootstrapper = new ServiceBootstrapper();
            var container = bootstrapper.Initialise();

            var instance = container.GetInstance<IDomainRuleManager<UserHasCapabilityRuleModel>>();

            Assert.That(instance, Is.Not.Null);
        }
    }
}
