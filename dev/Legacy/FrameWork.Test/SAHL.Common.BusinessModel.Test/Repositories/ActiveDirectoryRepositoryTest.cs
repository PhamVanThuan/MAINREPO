using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Test;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Data;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;
using Rhino.Mocks;


namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class ActiveDirectoryRepositoryTest : TestBase
    {
        private IActiveDirectoryRepository _activeDirectoryRepo = RepositoryFactory.GetRepository<IActiveDirectoryRepository>();

        [Test]
        public void GetActiveDirectoryUsersTest()
        {
            using (new SessionScope())
            {
                string adUserNamePartial = "CR";
                // get all ad users starting with 'CR'
                IList<ActiveDirectoryUserBindableObject> activeDirectoryUsers = _activeDirectoryRepo.GetActiveDirectoryUsers(adUserNamePartial);
                Assert.IsTrue(activeDirectoryUsers.Count > 0);
            }
        }

        [Test]
        public void GetActiveDirectoryUserTest()
        {
            using (new SessionScope())
            {
                string adUserNamePartial = "CR";
                // get all ad users starting with 'CR'
                IList<ActiveDirectoryUserBindableObject> activeDirectoryUsers = _activeDirectoryRepo.GetActiveDirectoryUsers(adUserNamePartial);
                // get the first user from above list
                string adUser = activeDirectoryUsers[0].ADUserName;
                ActiveDirectoryUserBindableObject activeDirectoryUser = _activeDirectoryRepo.GetActiveDirectoryUser(adUser);
                Assert.IsTrue(activeDirectoryUser.ADUserName == adUser);
            }
        }
    }
}
