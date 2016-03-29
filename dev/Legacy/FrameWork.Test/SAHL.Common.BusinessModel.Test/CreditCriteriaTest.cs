using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class CreditCriteriaTest : TestBase
    {
        [Test]
        public void GetCreditCriteria()
        {
            using (new SessionScope())
            {
                string query = "SELECT DISTINCT CC.* FROM [2AM].[dbo].[CreditCriteria] CC (NOLOCK) "
                   + "JOIN [2AM].[dbo].[CreditMatrix] CM (NOLOCK) ON CC.CreditMatrixKey = CM.CreditMatrixKey "
                   + "JOIN [2AM].[dbo].[OriginationSourceProductCreditMatrix] OSPCM (NOLOCK) ON CM.CreditMatrixKey = OSPCM.CreditMatrixKey "
                   + "JOIN [2AM].[dbo].[OriginationSourceProduct] OSP (NOLOCK) ON OSPCM.OriginationSourceProductKey = OSP.OriginationSourceProductKey "
                   + "WHERE CC.MortgageLoanPurposeKey = 2 AND CC.EmploymentTypeKey = 1 AND CC.MaxLoanAmount >= 300000 "
                   + "AND OSP.OriginationSourceKey = 1 AND OSP.ProductKey = 1 AND CM.NewBusinessIndicator = 'Y' AND CC.ExceptionCriteria = 0 ";
                DataTable DT = base.GetQueryResults(query);

                IReadOnlyEventList<ICreditCriteria> list = CCRepo.GetCreditCriteria(new DomainMessageCollection(), 1, 1, 2, 1, 300000);

                Assert.That(DT.Rows.Count == list.Count);
            }
        }

        [Test]
        public void GetMaxPTI()
        {
            MaxPTIHelper(1, 9, 3, 1, 1);
            MaxPTIHelper(1, 9, 3, 1, 1);
            MaxPTIHelper(1, 9, 3, 1, 50000000);
        }

        private void MaxPTIHelper(int OriginationSourceKey, int ProductKey, int MortgageLoanPurposeKey, int EmploymentTypeKey, double TotalLoanAmount)
        {
            using (new SessionScope(FlushAction.Never))
            {
                string query = String.Format(@"select top 1 PTI--, *
                    from dbo.CreditCriteria cc
                    inner join dbo.CreditMatrix cm on cc.CreditMatrixKey=cm.CreditMatrixKey
                    inner join dbo.OriginationSourceProductCreditMatrix ospcm on cm.CreditMatrixKey=ospcm.CreditMatrixKey
                    inner join dbo.OriginationSourceProduct osp on ospcm.OriginationSourceProductKey=osp.OriginationSourceProductKey
                    inner join dbo.Margin m on cc.MarginKey = m.MarginKey
                    where cc.MortgageLoanPurposeKey={2}
                    and cc.EmploymentTypeKey={3}
                    and cc.MaxLoanAmount>={4}
                    and cm.NewBusinessIndicator='Y'
                    and osp.OriginationSourceKey={0}
                    and osp.ProductKey={1}
                    and cc.ExceptionCriteria=0
                    order by cc.MaxLoanAmount asc, m.Value asc, cc.PTI desc", OriginationSourceKey, ProductKey, MortgageLoanPurposeKey, EmploymentTypeKey, TotalLoanAmount);

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), null);

                double pti = CCRepo.GetMaxPTI(null, OriginationSourceKey, ProductKey, MortgageLoanPurposeKey, EmploymentTypeKey, TotalLoanAmount);

                Assert.AreEqual(Convert.ToDouble(obj), pti);
            }
        }

        private ICreditCriteriaRepository _ccRepo;

        public ICreditCriteriaRepository CCRepo
        {
            get
            {
                if (_ccRepo == null)
                    _ccRepo = RepositoryFactory.GetRepository<ICreditCriteriaRepository>();

                return _ccRepo;
            }
        }
    }
}