using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Test;
using System;
using System.Data;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class ACBBankTest : TestBase
    {
        [Test]
        public void GetACBBranchesByPrefix()
        {
            using (new SessionScope())
            {
                string query = @"select top 1 br.ACBBankCode, br.ACBBranchCode, br.ACBBranchDescription
                    from [2AM].[dbo].[ACBBranch] br (nolock)
                    where ActiveIndicator = '0'
                    and ACBBankCode > 0
                    and ACBBranchCode > 0
                    ";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                int bankCode = Convert.ToInt32(DT.Rows[0][0]);
                string branchCode = Convert.ToString(DT.Rows[0][1]);
                string branchDesc = Convert.ToString(DT.Rows[0][2]);

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IACBBank bank = BMTM.GetMappedType<IACBBank>(ACBBank_DAO.Find(bankCode));

                IReadOnlyEventList<IACBBranch> list = bank.GetACBBranchesByPrefix(branchDesc, 10);

                bool found = false;
                foreach (IACBBranch branch in list)
                {
                    if (branchCode == branch.Key)
                    {
                        found = true;
                        break;
                    }
                }

                Assert.IsTrue(found);
            }
        }
    }
}