using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Factories;
using SAHL.Common.Security;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class AffordabilityAssessmentRepositoryTest : TestBase
    {
        private IAffordabilityAssessmentRepository affordabilityAssessmentRepository;

        public AffordabilityAssessmentRepositoryTest()
        {
            affordabilityAssessmentRepository = RepositoryFactory.GetRepository<IAffordabilityAssessmentRepository>();
        }

        [SetUp()]
        public void Setup()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.DomainMessages.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.DomainMessages.Clear();
        }


        [Test]
        public void GetApplicationAffordabilityAssessmentsTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                //get an open application
                var query = @"select top 1 GenericKey from AffordabilityAssessment where GeneralStatusKey = 1 order by 1 asc";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                {
                    Assert.Ignore("No Data for AffordabilityAssessmentRepositoryTest.GetApplicationAffordabilityAssessmentsTest");
                }

                var applicationKey = Convert.ToInt32(DT.Rows[0][0]);

                var assessments = affordabilityAssessmentRepository.GetActiveApplicationAffordabilityAssessments(applicationKey);

                Assert.Greater(assessments.Count(), 0, string.Format("Expected to get 1 or more affordability assessments for applicationkey: {0}", applicationKey));
            }
        }

        [Test]
        public void GetAffordabilityAssessmentByKeyTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                var query = @"select top 1 AffordabilityAssessmentKey from AffordabilityAssessment where GeneralStatusKey = 1 order by 1 desc";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                {
                    Assert.Ignore("No Data for AffordabilityAssessmentRepositoryTest.GetAffordabilityAssessmentByKeyTest");
                }

                var affordabilityAssessmentKey = Convert.ToInt32(DT.Rows[0][0]);

                var assessment = affordabilityAssessmentRepository.GetAffordabilityAssessmentByKey(affordabilityAssessmentKey);

                Assert.NotNull(assessment, string.Format("Expected assessment not to be null for AffordabilityAssessmentKey: {0}", affordabilityAssessmentKey));
            }
        }

    }
}
