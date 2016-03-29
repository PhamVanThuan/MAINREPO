using Automation.DataAccess;
using BuildingBlocks;
using BuildingBlocks.Navigation.CBO;
using BuildingBlocks.Presenters.Admin;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using NUnit.Framework;
using System;
using System.Threading;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace LoanServicingTests.Views.Administration
{
    [RequiresSTA]
    public class UserStatusMaintenanceTests : TestBase<AdminUserStatusMaintenance>
    {
        [Test, Description("Positive test to ensure that Further Lending Managers have access to this admin section")]
        public void UserStatusMaintenanceAccessFL()
        {
            HasAdminAccess(TestUsers.FLManager);
        }

        [Test, Description("Positive test to ensure that New Business Managers have access to this admin section")]
        public void UserStatusMaintenanceAccessNB()
        {
            HasAdminAccess(TestUsers.NewBusinessManager);
        }

        [Test, Description("Positive test to ensure that Credit Managers have access to this admin section")]
        public void UserStatusMaintenanceAccessCredit()
        {
            HasAdminAccess(TestUsers.CreditManager1);
        }

        [Test, Description("Positive test to ensure that Credit Managers only have access to their role types in this admin section")]
        public void UserStatusMaintenanceRolesCredit()
        {
            HasAccessToRoles(TestUsers.CreditManager1);
        }

        [Test, Description("Positive test to ensure that FL Managers only have access to their role types in this admin section")]
        public void UserStatusMaintenanceRolesFL()
        {
            HasAccessToRoles(TestUsers.FLManager);
        }

        [Test, Description("Positive test to ensure that New Business Managers only have access to their role types in this admin section")]
        public void UserStatusMaintenanceRolesNB()
        {
            HasAccessToRoles(TestUsers.NewBusinessManager);
        }

        #region UserStatusMaintenanceCheckAtLeastOneUseStillActive

        [Test, Description("At least one user of each offer role type needs to have their ADUser status set to active")]
        public void UserStatusMaintenanceCheckAtLeastOneUseStillActive()
        {
            int orgStructKey = -1;
            try
            {
                var results = Service<IADUserService>().SetUserInactiveAndGetTestDataSQL(TestUsers.FLManager);

                // Get Test Results
                string testADUser = results.RowList[0].Column("TestADUser").Value;
                string offerRoleType = results.RowList[0].Column("OfferRoleType").Value;
                orgStructKey = results.RowList[0].Column("OrgStructKey").GetValueAs<int>();
                string loginADUser = results.RowList[0].Column("LoginADUser").Value;

                base.Browser = new TestBrowser(loginADUser, TestUsers.Password);

                base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().AdministrationNode();
                base.Browser.Navigate<AdministrationActions>().UserStatusMaintenance();
                base.View.SelectRoleType(offerRoleType);
                base.View.SetUserStatus(testADUser, ADUserStatus.Inactive);
                base.View.Submit();

                // RULE CHECK : Make sure validation message does appear which confirms rule is firing correctly
                base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("There needs to be at least one active user in the group");

                // SQL CHECK : Check at least one user still active for the Org Struct
                results = Service<IADUserService>().CheckAtLeastOneUserStillActiveForOrgStructKeySQL(orgStructKey);
                Assert.IsTrue(Convert.ToInt32(results.RowList[0].Column(0).Value) > 0);
            }
            finally
            {
                Service<IADUserService>().SetAllUsersForOrgStructKeyActive(orgStructKey);
            }
        }

        #endregion UserStatusMaintenanceCheckAtLeastOneUseStillActive

        #region UserStatusMaintenanceCheckUpdateADUserStatusToInactive

        [Test, Description("Changing the ADUser Status column to Inactive and clicking the Submit button results in the ADUser.GeneralStatusKey being set to 2")]
        public void UserStatusMaintenanceCheckUpdateADUserStatusToInactive()
        {
            string testADUser = string.Empty;

            try
            {
                var results = Service<IADUserService>().TestAduserSQL(TestUsers.FLManager);
                // Get Test Results
                testADUser = results.RowList[0].Column("TestADUser").Value;
                string offerRoleType = results.RowList[0].Column("OfferRoleType").Value;
                string loginADUser = results.RowList[0].Column("LoginADUser").Value;

                // make sure user is set to active
                base.Service<IADUserService>().UpdateADUserStatusSQL(ADUserStatus.Active, testADUser);

                base.Browser = new TestBrowser(loginADUser, TestUsers.Password);

                base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().AdministrationNode();
                base.Browser.Navigate<AdministrationActions>().UserStatusMaintenance();
                base.View.SelectRoleType(offerRoleType);
                base.View.SetUserStatus(testADUser, ADUserStatus.Inactive);
                base.View.Submit();

                // Check if the ADUser is set to Inactive
                Assert.AreEqual(base.Service<IADUserService>().GetADUserStatus(testADUser), ADUserStatus.Inactive);
            }
            finally
            {
                // revert changes to the ADUser record
                base.Service<IADUserService>().UpdateADUserStatusSQL(ADUserStatus.Active, testADUser);
            }
        }

        #endregion UserStatusMaintenanceCheckUpdateADUserStatusToInactive

        #region UserStatusMaintenanceCheckUpdateADUserStatusToActive

        [Test, Description("Changing the ADUser Status column to Active and clicking the Submit button results in the ADUser.GeneralStatusKey being set to 1 ")]
        public void UserStatusMaintenanceCheckUpdateADUserStatusToActive()
        {
            string testADUser = string.Empty;
            try
            {
                var results = Service<IADUserService>().TestAduserSQL(TestUsers.FLManager);
                //db.GetData(TestAduserSQL());

                // Get Test Results
                testADUser = results.RowList[0].Column("TestADUser").Value;
                string offerRoleType = results.RowList[0].Column("OfferRoleType").Value;
                string loginADUser = results.RowList[0].Column("LoginADUser").Value;

                // make sure user is set to Inactive
                base.Service<IADUserService>().UpdateADUserStatusSQL(ADUserStatus.Inactive, testADUser);

                base.Browser = new TestBrowser(loginADUser, TestUsers.Password);

                base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().AdministrationNode();
                base.Browser.Navigate<AdministrationActions>().UserStatusMaintenance();
                base.View.SelectRoleType(offerRoleType);
                base.View.SetUserStatus(testADUser, ADUserStatus.Active);
                base.View.Submit();

                // Check if the ADUser is set to Active
                Assert.AreEqual(base.Service<IADUserService>().GetADUserStatus(testADUser), ADUserStatus.Active);
            }
            finally
            {
                // revert changes to the ADUser record
                base.Service<IADUserService>().UpdateADUserStatusSQL(ADUserStatus.Active, testADUser);
            }
        }

        #endregion UserStatusMaintenanceCheckUpdateADUserStatusToActive

        #region UserStatusMaintenanceCheckUpdateADUserRoundRobinStatusToInactive

        [Test, Description("Changing the Round Robin Status column to Inactive and clicking the Submit button results in the ADUserRoundRobin.GeneralStatusKey being set to 2")]
        public void UserStatusMaintenanceCheckUpdateADUserRoundRobinStatusToInactive()
        {
            string testADUser = string.Empty;
            try
            {
                var results = Service<IADUserService>().TestAduserSQL(TestUsers.FLManager);

                // Get Test Results
                testADUser = results.RowList[0].Column("TestADUser").Value;
                string offerRoleType = results.RowList[0].Column("OfferRoleType").Value;
                int orgStructKey = results.RowList[0].Column("OrgStructKey").GetValueAs<int>();
                string loginADUser = results.RowList[0].Column("LoginADUser").Value;

                // make sure user is set to active
                base.Service<IADUserService>().UpdateADUserRoundRobinStatusSQL(ADUserStatus.Active, testADUser);

                base.Browser = new TestBrowser(loginADUser, TestUsers.Password);

                base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().AdministrationNode();
                base.Browser.Navigate<AdministrationActions>().UserStatusMaintenance();
                base.View.SelectRoleType(offerRoleType);
                base.View.SetRoundRobinStatus(testADUser, ADUserStatus.Inactive);
                base.View.Submit();
                // Check if the ADUser is set to Inactive
                Assert.AreEqual(base.Service<IADUserService>().GetRoundRobinStatus(testADUser, orgStructKey), ADUserStatus.Inactive);
            }
            finally
            {
                // revert changes to the ADUser record
                base.Service<IADUserService>().UpdateADUserRoundRobinStatusSQL(ADUserStatus.Active, testADUser);
            }
        }

        #endregion UserStatusMaintenanceCheckUpdateADUserRoundRobinStatusToInactive

        #region UserStatusMaintenanceCheckUpdateADUserRoundRobinStatusToActive

        [Test, Description("Changing the Round Robin Status column to active and clicking the Submit button results in the ADUserRoundRobin.GeneralStatusKey being set to 1")]
        public void UserStatusMaintenanceCheckUpdateADUserRoundRobinStatusToActive()
        {
            string testADUser = string.Empty;

            try
            {
                var results = Service<IADUserService>().TestAduserSQL(TestUsers.FLManager);

                // Get Test Results
                testADUser = results.RowList[0].Column("TestADUser").Value;
                string offerRoleType = results.RowList[0].Column("OfferRoleType").Value;
                int orgStructKey = results.RowList[0].Column("OrgStructKey").GetValueAs<int>();
                string loginADUser = results.RowList[0].Column("LoginADUser").Value;

                // make sure user is set to Inactive
                base.Service<IADUserService>().UpdateADUserRoundRobinStatusSQL(ADUserStatus.Inactive, testADUser);

                base.Browser = new TestBrowser(loginADUser, TestUsers.Password);

                base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().AdministrationNode();
                base.Browser.Navigate<AdministrationActions>().UserStatusMaintenance();
                base.View.SelectRoleType(offerRoleType);
                base.View.SetRoundRobinStatus(testADUser, ADUserStatus.Active);
                base.View.Submit();
                // Check if the ADUser is set to active
                Assert.AreEqual(base.Service<IADUserService>().GetRoundRobinStatus(testADUser, orgStructKey), ADUserStatus.Active);
            }
            finally
            {
                // revert changes to the ADUser record
                base.Service<IADUserService>().UpdateADUserRoundRobinStatusSQL(ADUserStatus.Active, testADUser);
            }
        }

        #endregion UserStatusMaintenanceCheckUpdateADUserRoundRobinStatusToActive

        #region AllUsersLoaded

        /// <summary>
        /// Ensures that the number of users in the results/page matches the users that we expect
        /// </summary>
        [Test, Description("Ensures that the number of users in the results/page matches the users that we expect")]
        public void EnsureUsersInRoleMatchExpectedCreditManager()
        {
            EnsureUsersInRoleMatchExpected(TestUsers.CreditManager1);
        }

        /// <summary>
        /// Ensures that the number of users in the results/page matches the users that we expect
        /// </summary>
        [Test, Description("Ensures that the number of users in the results/page matches the users that we expect")]
        public void EnsureUsersInRoleMatchExpectedNewBusinessManager()
        {
            EnsureUsersInRoleMatchExpected(TestUsers.NewBusinessManager);
        }

        /// <summary>
        /// Ensures that the number of users in the results/page matches the users that we expect
        /// </summary>
        [Test, Description("Ensures that the number of users in the results/page matches the users that we expect")]
        public void EnsureUsersInRoleMatchExpectedFurtherLendingManager()
        {
            EnsureUsersInRoleMatchExpected(TestUsers.FLManager);
        }

        /// <summary>
        /// Ensure Users in Role Match Expected
        /// </summary>
        /// <param name="adUsername"></param>
        private void EnsureUsersInRoleMatchExpected(string adUsername)
        {
            var results = Service<IADUserService>().RoleSQLCount(adUsername);

            base.Browser = new TestBrowser(adUsername, TestUsers.Password);

            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().Menu();
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().AdministrationNode();
            base.Browser.Navigate<AdministrationActions>().UserStatusMaintenance();

            int count = 0;
            foreach (QueryResultsRow roleTypeRow in results.RowList)
            {
                base.View.SelectRoleType(roleTypeRow.Column(0).Value);
                base.View.ResetPaging();

                count += base.View.GetUserRowCountInPage();
                Thread.Sleep(3000);

                while (base.View.NextPage())
                {
                    Thread.Sleep(3000);
                    count += base.View.GetUserRowCountInPage();
                }

                Assert.IsTrue(count == int.Parse(roleTypeRow.Column(1).Value));
                count = 0;
            }
        }

        #endregion AllUsersLoaded

        #region Helper Methods

        private void HasAccessToRoles(string userLoginName)
        {
            //Login as User
            base.Browser = new TestBrowser(userLoginName);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().AdministrationNode();
            //navigate to section
            base.Browser.Navigate<AdministrationActions>().UserStatusMaintenance();
            var qr = Service<IADUserService>().RoleSQL(userLoginName);
            base.View.ConfirmRoleTypesDDLItems(qr);
        }

        /// <summary>
        /// Helper method to return whether the specified user has access to the user status maintenance screen
        /// </summary>
        /// <param name="userPassword"></param>
        /// <param name="userLoginName"></param>
        private void HasAdminAccess(string userLoginName)
        {
            //Login as User
            base.Browser = new TestBrowser(userLoginName);
            //Navigate
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().AdministrationNode();
            //Assert
            base.Browser.Page<BasePageAssertions>().AssertFrameContainsText(Features.UserStatusMaintenance);
        }

        #endregion Helper Methods
    }
}