using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;
using System;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    internal class InternetRepositoryTest : TestBase
    {
        private IInternetRepository _repo = RepositoryFactory.GetRepository<IInternetRepository>();

        [NUnit.Framework.Test]
        public void GetAllInternetLeadUsersTest()
        {
            SimpleQuery<InternetLeadUsers_DAO> q = new SimpleQuery<InternetLeadUsers_DAO>("select inl from InternetLeadUsers_DAO inl");
            InternetLeadUsers_DAO[] res = q.Execute();

            IEventList<IInternetLeadUsers> internetLeadUsers = _repo.GetAllInternetLeadUsers();

            Assert.AreEqual(internetLeadUsers.Count, res.Length);
        }

        [NUnit.Framework.Test]
        public void GetAllActiveInternetLeadUsersTest()
        {
            SimpleQuery<InternetLeadUsers_DAO> q = new SimpleQuery<InternetLeadUsers_DAO>("select inl from InternetLeadUsers_DAO inl where inl.GeneralStatus.Key = 1");
            InternetLeadUsers_DAO[] res = q.Execute();

            IEventList<IInternetLeadUsers> internetLeadUsers = _repo.GetAllActiveInternetLeadUsers();

            Assert.AreEqual(internetLeadUsers.Count, res.Length);
        }

        [NUnit.Framework.Test]
        public void GetActiveGeneralStatusTest()
        {
            IGeneralStatus generalStatus = _repo.GetActiveGeneralStatus();

            Assert.AreEqual(generalStatus.Key, (int)GeneralStatuses.Active);
        }

        [NUnit.Framework.Test]
        public void GetInActiveGeneralStatusTest()
        {
            IGeneralStatus generalStatus = _repo.GetInActiveGeneralStatus();

            Assert.AreEqual(generalStatus.Key, (int)GeneralStatuses.Inactive);
        }

        [NUnit.Framework.Test]
        public void UpdateInternetLeadUserTest()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                InternetLeadUsers_DAO ilu = new InternetLeadUsers_DAO();
                ADUser_DAO aduser = ADUser_DAO.FindFirst();
                ilu.ADUser = aduser;

                GeneralStatus_DAO generalStatusActive = GeneralStatus_DAO.Find((int)GeneralStatuses.Active);
                ilu.GeneralStatus = generalStatusActive;
                ilu.CreateAndFlush();
                IGeneralStatus generalStatusInactive = _repo.GetInActiveGeneralStatus();
                bool success = _repo.UpdateInternetLeadUser(ilu.Key, generalStatusInactive, false);

                // now try and load it
                InternetLeadUsers_DAO ilu2 = InternetLeadUsers_DAO.Find(ilu.Key);
                Assert.AreEqual(ilu.GeneralStatus.Key, (int)GeneralStatuses.Inactive);
            }
        }

        [NUnit.Framework.Test]
        public void AssignInternetLeadUserTest()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                int iluKey = 0;

                // need to get the next person who is supposed to get the flag.
                // also need to check if its null then start at the begining again
                string sql = String.Format(@"select min(InternetLeadUsersKey) from dbo.InternetLeadUsers
                                where InternetLeadUsersKey > (select InternetLeadUsersKey from dbo.InternetLeadUsers where GeneralStatusKey = {0} and Flag = 1)
                                and GeneralStatusKey = {0}", (int)GeneralStatuses.Active);

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(InternetLeadUsers_DAO), new ParameterCollection());
                if (o != DBNull.Value)
                {
                    iluKey = Convert.ToInt32(o);
                }
                else
                {
                    // object returned is null so need to query again but from the start this time.
                    sql = String.Format(@"select min(InternetLeadUsersKey) from dbo.InternetLeadUsers
                                where InternetLeadUsersKey > 0 and GeneralStatusKey = {0}", (int)GeneralStatuses.Active);

                    o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(InternetLeadUsers_DAO), new ParameterCollection());
                    if (o != null)
                        iluKey = Convert.ToInt32(o);
                }
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IInternetLeadUsers ilu = BMTM.GetMappedType<IInternetLeadUsers, InternetLeadUsers_DAO>(InternetLeadUsers_DAO.Find(iluKey) as InternetLeadUsers_DAO);

                Application_DAO app = Application_DAO.FindFirst();
                IADUser aduser = _repo.AssignInternetLeadUser(app.Key);

                Assert.AreEqual(aduser.Key, ilu.ADUser.Key);
            }
        }

        [NUnit.Framework.Test]
        public void RefreshInternetLeadUsersTest()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                Assert.IsTrue(_repo.RefreshInternetLeadUsers());
            }
        }
    }
}