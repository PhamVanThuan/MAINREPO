using BuildingBlocks;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using NUnit.Framework;
using System;
namespace Origination.Users
{
    [TestFixture, RequiresSTA]
    public class CheckTestUsersLocked : TestBase<BasePage>
    {
        [Test]
        public void TestAgainstAD()
        {
            var lockedUsers = Service<IActiveDirectoryService>().GetAllLockedTestUsers();
            CollectionAssert.IsEmpty(lockedUsers, String.Join("\n",lockedUsers));
        }
    }
}

