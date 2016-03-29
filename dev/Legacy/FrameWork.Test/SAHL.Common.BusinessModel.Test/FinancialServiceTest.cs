using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Data;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class FinancialServiceTest : TestBase
    {
        [Test]
        public void TestFS()
        {
            using (new SessionScope())
            {
                FinancialService_DAO fs = FinancialService_DAO.FindFirst();
            }
        }

        [Test]
        public void GetTransactions()
        {
            using (new SessionScope())
            {
                string query = @"SELECT TOP 5 LT.FinancialTransactionkey, FS.FinancialServiceKey, LT.TransactionTypekey
                                    FROM [2AM].[fin].[FinancialTransaction] LT (nolock)
                                    Join [2AM].[fin].[TransactionType] TT (nolock) ON LT.TransactionTypekey = TT.TransactionTypekey
                                    Join [2AM].[fin].[MortgageLoan] ML (nolock) ON LT.FinancialServiceKey = ML.FinancialServiceKey
                                    Join [2AM].[dbo].[FinancialService] FS (nolock) ON LT.FinancialServiceKey = FS.FinancialServiceKey
                                    Join [2AM].[dbo].[FinancialServiceType] FST (nolock) ON FS.FinancialServiceTypeKey = FST.FinancialServiceTypeKey
                                    ORDER BY LT.FinancialTransactionKey";
                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count > 0);

                List<int> types = new List<int>();
                string typesStr = "";

                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    int type = Convert.ToInt32(DT.Rows[i][2]);

                    if (!types.Contains(type))
                    {
                        types.Add(type);
                        typesStr += "," + type;
                    }
                }

                typesStr = typesStr.Remove(0, 1);

                int fsKey = Convert.ToInt32(DT.Rows[0][1]);

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IFinancialService fs = BMTM.GetMappedType<IFinancialService, FinancialService_DAO>(FinancialService_DAO.Find(fsKey));

                query = String.Format(@"SELECT LT.FinancialTransactionkey, FS.FinancialServiceKey, LT.TransactionTypekey
                                    FROM [2AM].[fin].[FinancialTransaction] LT (nolock)
                                    Join [2AM].[fin].[TransactionType] TT (nolock) ON LT.TransactionTypekey = TT.TransactionTypekey
                                    Join [2AM].[fin].[MortgageLoan] ML (nolock) ON LT.FinancialServiceKey = ML.FinancialServiceKey
                                    Join [2AM].[dbo].[FinancialService] FS (nolock) ON LT.FinancialServiceKey = FS.FinancialServiceKey
                                    Join [2AM].[dbo].[FinancialServiceType] FST (nolock) ON FS.FinancialServiceTypeKey = FST.FinancialServiceTypeKey
									join [2am].[fin].FinancialTransactionAccountBalance FTAB (NOLOCK) ON LT.FinancialTransactionKey = FTAB.FinancialTransactionKey
                                    WHERE LT.FinancialServiceKey = {0}  AND LT.TransactionTypekey in ({1})
                                    ORDER BY LT.FinancialTransactionKey", fs.Key, typesStr);

                DT = base.GetQueryResults(query);

                DataTable DT2 = fs.GetTransactions(new DomainMessageCollection(), 1, types);
                Assert.That(DT2.Rows.Count == DT.Rows.Count);

                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    Assert.That(Convert.ToInt32(DT.Rows[i][0]) == Convert.ToInt32(DT2.Rows[i][0]), "");
                    Assert.That(Convert.ToInt32(DT.Rows[i][1]) == Convert.ToInt32(DT2.Rows[i][1]));
                    Assert.That(Convert.ToInt32(DT.Rows[i][2]) == Convert.ToInt32(DT2.Rows[i][2]));
                }
            }
        }
    }
}