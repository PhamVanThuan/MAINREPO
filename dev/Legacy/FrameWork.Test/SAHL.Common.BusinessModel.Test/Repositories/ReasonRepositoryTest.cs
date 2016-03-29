using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Castle.ActiveRecord;
using NUnit.Framework;
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
using SAHL.Test;
using SAHL.Test.DAOHelpers;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class ReasonRepositoryTest : TestBase
    {
        private static IReasonRepository _reasonRepo = RepositoryFactory.GetRepository<IReasonRepository>();

        [Test]
        public void GetReasonByGenericTypeAndKey()
        {
            using (new SessionScope())
            {
                string query = "SELECT TOP 1 RT.GenericKeyTypeKey, R.GenericKey, count(R.GenericKey) "
                + "FROM [2AM].[dbo].[ReasonType] RT (NOLOCK) "
                + "JOIN [2AM].[dbo].[ReasonDefinition] RD (NOLOCK) ON RD.ReasonTypeKey = RT.ReasonTypeKey "
                + "JOIN [2AM].[dbo].[Reason] R (NOLOCK) ON R.ReasonDefinitionKey = RD.ReasonDefinitionKey "
                + "GROUP BY RT.GenericKeyTypeKey, R.GenericKey ORDER BY count(R.GenericKey) desc";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                int gtKey = Convert.ToInt32(DT.Rows[0][0]);
                int gKey = Convert.ToInt32(DT.Rows[0][1]);
                int count = Convert.ToInt32(DT.Rows[0][2]);

                IReasonRepository reasonRepo = new ReasonRepository();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                IReadOnlyEventList<IReason> list = reasonRepo.GetReasonByGenericTypeAndKey(gtKey, gKey);

                Assert.That(list.Count == count);
            }
        }

        [Test]
        public void GetReasonDefinitionByKey()
        {
            using (new SessionScope())
            {
                IReasonRepository reasonRepo = new ReasonRepository();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                ReasonDefinition_DAO reasonDefinitionCheck = ReasonDefinition_DAO.FindFirst();

                IReasonDefinition reasonDefinition = reasonRepo.GetReasonDefinitionByKey(reasonDefinitionCheck.Key);

                Assert.IsTrue(reasonDefinition.Key == reasonDefinitionCheck.Key);
                Assert.IsTrue(reasonDefinition.ReasonDescription.Description == reasonDefinitionCheck.ReasonDescription.Description);
            }
        }

        [Test]
        public void GetReasonDefinitionsByReasonType()
        {
            using (new SessionScope())
            {
                IReasonRepository reasonRepo = new ReasonRepository();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                IReadOnlyEventList<IReasonDefinition> reasonDefinitions = reasonRepo.GetReasonDefinitionsByReasonType(SAHL.Common.Globals.ReasonTypes.LifeCallback);
                Assert.IsTrue(reasonDefinitions.Count > 0);
            }
        }

        [Test]
        public void GetReasonDefinitionsByReasonDescription()
        {
            using (new SessionScope())
            {
                IReasonRepository reasonRepo = new ReasonRepository();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                // get first reason definition
                ReasonDefinition_DAO reasondef = ReasonDefinition_DAO.FindFirst();
                string reasonDescription = reasondef.ReasonDescription.Description;
                SAHL.Common.Globals.ReasonTypes reasonType = (SAHL.Common.Globals.ReasonTypes)reasondef.ReasonType.Key;

                // get reason definitions using repository method
                IReadOnlyEventList<IReasonDefinition> reasonDefinitions = reasonRepo.GetReasonDefinitionsByReasonDescription(reasonType, reasonDescription);

                // compare results
                Assert.IsTrue(reasonDefinitions.Count == 1);
                Assert.IsTrue(reasonDefinitions[0].ReasonDescription.Description == reasonDescription);
            }
        }

        [Test]
        public void GetReasonDefinitionsByReasonTypeGroup()
        {
            using (new SessionScope())
            {
                IReasonRepository reasonRepo = new ReasonRepository();
                IDomainMessageCollection Messages = new DomainMessageCollection();

                // get first reason definition
                ReasonTypeGroup_DAO reasontypegroup = ReasonTypeGroup_DAO.FindFirst();
                SAHL.Common.Globals.ReasonTypeGroups reasonTypeGroup = (SAHL.Common.Globals.ReasonTypeGroups)reasontypegroup.Key;

                IReadOnlyEventList<IReasonDefinition> reasonDefinitions = reasonRepo.GetReasonDefinitionsByReasonTypeGroup(reasonTypeGroup);
                Assert.IsTrue(reasonDefinitions.Count > 0);
            }
        }

        [Test]
        public void GetReasonsByGenericKeyTypeAndKeys()
        {
            using (new SessionScope())
            {
                string query = "SELECT TOP 1 RT.GenericKeyTypeKey, R.GenericKey "
                + "FROM [2AM].[dbo].[ReasonType] RT (NOLOCK) "
                + "JOIN [2AM].[dbo].[ReasonDefinition] RD (NOLOCK) ON RD.ReasonTypeKey = RT.ReasonTypeKey "
                + "JOIN [2AM].[dbo].[Reason] R (NOLOCK) ON R.ReasonDefinitionKey = RD.ReasonDefinitionKey ";
                DataTable DT = base.GetQueryResults(query);
                IReasonRepository reasonRepo = new ReasonRepository();
                IDomainMessageCollection Messages = new DomainMessageCollection();
                IEventList<IReason> list = reasonRepo.GetReasonsByGenericKeyTypeAndKeys(DT);
                Assert.IsNotNull(list);
            }
        }

        [Test]
        public void GetReasonByKey()
        {
            using (new SessionScope())
            {
                string sqlQuery = "SELECT TOP 1 R.ReasonKey FROM Reason R ";

                ParameterCollection parameters = new ParameterCollection();

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);
                if (obj != null)
                {
                    int iReasonKey = Convert.ToInt32(obj);
                    IReasonRepository reasonRepo = new ReasonRepository();
                    IReason reason = reasonRepo.GetReasonByKey(iReasonKey);
                    Assert.IsNotNull(reason);
                }
                else
                {
                    Assert.Fail("There are no valid keys for this test");
                }
            }
        }

        [Test]
        public void GetReasonTypeByKey()
        {
            using (new SessionScope())
            {
                string sqlQuery = "SELECT TOP 1 RT.ReasonTypeKey FROM ReasonType RT ";

                ParameterCollection parameters = new ParameterCollection();

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);
                if (obj != null)
                {
                    int iReasonTypeKey = Convert.ToInt32(obj);
                    IReasonRepository reasonRepo = new ReasonRepository();
                    IReasonType reasonType = reasonRepo.GetReasonTypeByKey(iReasonTypeKey);
                    Assert.IsNotNull(reasonType);
                }
                else
                {
                    Assert.Fail("There are no valid keys for this test");
                }
            }
        }

        [Test]
        public void GetReasonByGenericKeyAndReasonGroupTypeKey()
        {
            using (new SessionScope())
            {
                string sqlQuery = "Select TOP 1 R.GenericKey, RT.ReasonTypeGroupKey" +
                                  " From Reason R" +
                                  " Inner Join ReasonDefinition RD On R.ReasonDefinitionKey = RD.ReasonDefinitionKey" +
                                  " Inner Join ReasonType RT On RD.ReasonTypeKey = RT.ReasonTypeKey";

                ParameterCollection parameters = new ParameterCollection();

                DataSet dsReason = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);
                if (dsReason != null)
                {
                    if (dsReason.Tables.Count > 0)
                    {
                        if (dsReason.Tables[0].Rows.Count > 0)
                        {
                            int iGenericKey = Convert.ToInt32(dsReason.Tables[0].Rows[0]["GenericKey"]);
                            int iReasonTypeGroupKey = Convert.ToInt32(dsReason.Tables[0].Rows[0]["ReasonTypeGroupKey"]);

                            IReasonRepository reasonRepo = new ReasonRepository();
                            IReadOnlyEventList<IReason> reasons = reasonRepo.GetReasonByGenericKeyAndReasonGroupTypeKey(iGenericKey, iReasonTypeGroupKey);
                            Assert.IsNotNull(reasons);
                        }
                    }
                }
                else
                {
                    Assert.Fail("There are no valid keys for this test");
                }
            }
        }

        [Test]
        public void GetReasonByGenericKeyListAndReasonTypeGroupKey()
        {
            using (new SessionScope())
            {
                string sqlQuery = "Select TOP 1 R.GenericKey, RT.ReasonTypeGroupKey" +
                                  " From Reason R" +
                                  " Inner Join ReasonDefinition RD On R.ReasonDefinitionKey = RD.ReasonDefinitionKey" +
                                  " Inner Join ReasonType RT On RD.ReasonTypeKey = RT.ReasonTypeKey";

                ParameterCollection parameters = new ParameterCollection();

                DataSet dsReason = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);
                if (dsReason != null)
                {
                    if (dsReason.Tables.Count > 0)
                    {
                        if (dsReason.Tables[0].Rows.Count > 0)
                        {
                            int iGenericKey = Convert.ToInt32(dsReason.Tables[0].Rows[0]["GenericKey"]);
                            int iReasonTypeGroupKey = Convert.ToInt32(dsReason.Tables[0].Rows[0]["ReasonTypeGroupKey"]);

                            List<int> GenericKeyList = new List<int>();
                            GenericKeyList.Add(iGenericKey);

                            IReasonRepository reasonRepo = new ReasonRepository();
                            IReadOnlyEventList<IReason> reasons = reasonRepo.GetReasonByGenericKeyListAndReasonTypeGroupKey(GenericKeyList, iReasonTypeGroupKey);
                            Assert.IsNotNull(reasons);
                        }
                    }
                }
                else
                {
                    Assert.Fail("There are no valid keys for this test");
                }
            }
        }

        [Test]
        public void GetReasonByGenericKeyListAndReasonTypeKey()
        {
            using (new SessionScope())
            {
                string sqlQuery = "Select TOP 1 R.GenericKey, RD.ReasonTypeKey" +
                                  " From Reason R" +
                                  " Inner Join ReasonDefinition RD On R.ReasonDefinitionKey = RD.ReasonDefinitionKey";

                ParameterCollection parameters = new ParameterCollection();

                DataSet dsReason = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), parameters);
                if (dsReason != null)
                {
                    if (dsReason.Tables.Count > 0)
                    {
                        if (dsReason.Tables[0].Rows.Count > 0)
                        {
                            int iGenericKey = Convert.ToInt32(dsReason.Tables[0].Rows[0]["GenericKey"]);
                            int iReasonTypeKey = Convert.ToInt32(dsReason.Tables[0].Rows[0]["ReasonTypeKey"]);

                            List<int> GenericKeyList = new List<int>();
                            GenericKeyList.Add(iGenericKey);

                            IReasonRepository reasonRepo = new ReasonRepository();
                            IReadOnlyEventList<IReason> reasons = reasonRepo.GetReasonByGenericKeyListAndReasonTypeKey(GenericKeyList, iReasonTypeKey);
                            Assert.IsNotNull(reasons);
                        }
                    }
                }
                else
                {
                    Assert.Fail("There are no valid keys for this test");
                }
            }
        }

        [Test]
        public void CreateEmptyReason()
        {
            IReasonRepository reasonRepo = new ReasonRepository();
            IReason reason = reasonRepo.CreateEmptyReason();
            Assert.IsNotNull(reason);
        }

        [Test]
        public void GeReasonsByGenericKeysTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 genericKey
                from [2am].dbo.reason (nolock)
                where genericKey is not null";

                object res = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());

                if (res != null)
                {
                    int[] genericKeys = new int[] { Convert.ToInt32(res) };

                    IReadOnlyEventList<IReason> reasons = _reasonRepo.GeReasonsByGenericKeys(genericKeys);
                    Assert.IsTrue(reasons.Count > 0);
                }
            }
        }

        [Test]
        public void GetReasonByGenericKeyAndReasonTypeKeyTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 r.genericKey, rd.reasonTypeKey
                from [2am].dbo.reason r (nolock)
                inner join [2am].dbo.reasonDefinition rd (nolock)
	                on r.reasonDefinitionKey = rd.reasonDefinitionKey
                where r.genericKey is not null and rd.reasonTypeKey is not null";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                if (ds != null & ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int genericKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    int reasonTypeKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                    IReadOnlyEventList<IReason> reasons = _reasonRepo.GetReasonByGenericKeyAndReasonTypeKey(genericKey, reasonTypeKey);
                    Assert.IsTrue(reasons.Count > 0);
                }
            }
        }

        [Test]
        public void GetReasonDefinitionsByReasonTypeKeyTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 reasonTypeKey
                from [2am].dbo.reasonDefinition (nolock)
                where reasonTypeKey is not null";

                object res = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());

                if (res != null)
                {
                    int reasonTypeKey = Convert.ToInt32(res);
                    IReadOnlyEventList<IReasonDefinition> reasons = _reasonRepo.GetReasonDefinitionsByReasonTypeKey(reasonTypeKey);
                    Assert.IsTrue(reasons.Count > 0);
                }
            }
        }

        [Test]
        public void GetReasonTypeByReasonTypeGroupTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 rt.reasonTypeGroupKey
                from [2am].dbo.reasonType rt (nolock)
                where rt.reasonTypeGroupKey is not null";

                object res = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());

                if (res != null)
                {
                    int[] genericKeys = new int[] { Convert.ToInt32(res) };
                    IReadOnlyEventList<IReasonType> reasonTypes = _reasonRepo.GetReasonTypeByReasonTypeGroup(genericKeys);
                    Assert.IsTrue(reasonTypes.Count > 0);
                }
            }
        }

        [Test]
        public void DeleteReason()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
				//Had to add this so that it actually selects a reason it can delete
				string sql = @"select top 1 r.ReasonKey from Reason r
							left join Callback c on r.ReasonKey = c.ReasonKey
							where c.CallbackKey is null";
				object res = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
				Reason_DAO rDao = Reason_DAO.Find(res);
                IReason r = null;

                if (rDao != null)
                {
                    r = new Reason(rDao);
                    _reasonRepo.DeleteReason(r);
                    Assert.IsNotNull(r);
                }
                else
                {
                    Assert.Fail("No data to test IReasonRepository.DeleteReason");
                }
            }
        }

        [Test]
        public void GetLatestReasonByGenericKeyAndReasonTypeKey()
        {
            using (new SessionScope())
            {
                int reasonKey = 0;

                // get a reason for a generickey that has more than one reason
                string sql = @"select top 1 r.GenericKey,rd.ReasonTypeKey,count(r.GenericKey)
                            from [2am]..Reason r (nolock) join [2am]..ReasonDefinition rd (nolock) on rd.ReasonDefinitionKey = r.ReasonDefinitionKey
                            group by r.GenericKey,rd.ReasonTypeKey having count(r.GenericKey) > 1 ";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                if (ds != null & ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int genericKey = Convert.ToInt32(ds.Tables[0].Rows[0]["GenericKey"]);
                    int reasonTypeKey = Convert.ToInt32(ds.Tables[0].Rows[0]["ReasonTypeKey"]);

                    // get latest reason for a generickey that has more than one reason
                    sql = String.Format(@"select	r.* from [2am]..Reason r (nolock) join [2am]..ReasonDefinition rd (nolock) on rd.ReasonDefinitionKey = r.ReasonDefinitionKey
                            where r.GenericKey = {0} and rd.ReasonTypeKey = {1} order by r.ReasonKey desc", genericKey, reasonTypeKey);
                    ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                    if (ds != null & ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        reasonKey = Convert.ToInt32(ds.Tables[0].Rows[0]["ReasonKey"]);
                    }

                    IReason reason = _reasonRepo.GetLatestReasonByGenericKeyAndReasonTypeKey(genericKey, reasonTypeKey);
                    Assert.IsTrue(reason.Key == reasonKey);
                }
            }
        }
    }
}