using NUnit.Framework;
using SAHL.Common.Globals;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;
using SAHL.V3.Framework.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Test.ServiceTests
{
    [TestFixture]
    public class ApplicationDomainServiceTest
    {
        [Ignore]
        [Test]
        public void Test_Get_AffordabilityAssessmentsForApplicationQuery()
        {
            IV3ServiceManager v3ServiceManager = V3ServiceManager.Instance;
            IApplicationDomainService appDomainService = v3ServiceManager.Get<IApplicationDomainService>();
            var query = new GetAffordabilityAssessmentsForApplicationQuery(616322);
            var result = appDomainService.PerformQuery(query);
        }

        [Ignore]
        [Test]
        public void Test_GetAffordabilityAssessmentByKeyQuery()
        {
            IV3ServiceManager v3ServiceManager = V3ServiceManager.Instance;
            IApplicationDomainService appDomainService = v3ServiceManager.Get<IApplicationDomainService>();
            var query = new GetAffordabilityAssessmentByKeyQuery(3);
            var result = appDomainService.PerformQuery(query);
        }

        [Ignore]
        [Test]
        public void Test_GetUnconfirmedAffordabilityAssessmentsForApplicationQuery()
        {
            IV3ServiceManager v3ServiceManager = V3ServiceManager.Instance;
            IApplicationDomainService appDomainService = v3ServiceManager.Get<IApplicationDomainService>();
            var query = new GetUnconfirmedAffordabilityAssessmentsForApplicationQuery(1764252);
            var result = appDomainService.PerformQuery(query);
        }

        [Ignore]
        [Test]
        public void Test_DeleteUnconfirmedAffordabilityAssessmentCommand()
        {
            IV3ServiceManager v3ServiceManager = V3ServiceManager.Instance;
            IApplicationDomainService appDomainService = v3ServiceManager.Get<IApplicationDomainService>();
            var command = new DeleteUnconfirmedAffordabilityAssessmentCommand(1);
            var result = appDomainService.DeleteUnconfirmedAffordabilityAssessment(1);
        }

        [Ignore]
        [Test]
        public void Test_AddApplicationAffordabilityAssessmentCommand()
        {
            IV3ServiceManager v3ServiceManager = V3ServiceManager.Instance;
            IApplicationDomainService appDomainService = v3ServiceManager.Get<IApplicationDomainService>();

            var genericKey = 1755705;
            var contributingApplicantLegalEntities = new List<int>() { 260652, 260651 };
            var numberOfContributingApplicants = 6;
            var numberOfhouseholdDependants = 7;
            AffordabilityAssessmentModel affordabilityAssessment = new AffordabilityAssessmentModel();
            var result = appDomainService.AddAffordabilityAssessment(affordabilityAssessment);
        }
    }
}
