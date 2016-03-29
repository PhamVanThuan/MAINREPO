using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Collections.Generic;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace LoanServicingTests.Views.LegalEntity
{
    [RequiresSTA]
    public class LegalEntityMemo : TestBase<GenericMemoAdd>
    {
        private int _legalEntityKey;
        private Automation.DataModels.Account Account;

        #region SetupTearDown

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            Login(TestUsers.HaloUser);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            if (base.Browser == null)
            {
                Login(TestUsers.HaloUser);
            }
        }

        #endregion SetupTearDown

        /// <summary>
        /// Tests that a legal entity memo can be added with an unresolved status and that the record is persisted to the database correctly.
        /// </summary>
        [Test, Sequential, Description("Tests that a legal entity memo can be added with an unresolved status and that the record is persisted to the database correctly.")]
        public void AddLegalEntityMemo([Values(MemoStatus.Resolved, MemoStatus.UnResolved)] string memoStatus)
        {
            string memoText = "Loan Servicing Legal Entity Memo";
            Navigate(NodeTypeEnum.Add);
            var generalStatus = memoStatus == MemoStatus.UnResolved ? GeneralStatusEnum.Active : GeneralStatusEnum.Inactive;
            base.View.AddMemoRecord(memoStatus, memoText);
            MemoAssertions.AssertLatestMemoInformation(_legalEntityKey, GenericKeyTypeEnum.LegalEntity_LegalEntityKey, MemoTable.Memo, memoText);
            MemoAssertions.AssertLatestMemoInformation(_legalEntityKey, GenericKeyTypeEnum.LegalEntity_LegalEntityKey, MemoTable.GeneralStatusKey,
                ((int)generalStatus).ToString());
        }

        /// <summary>
        /// Ensures that a legal entity memo cannot be added without a description.
        /// </summary>
        [Test, Description("Ensures that a legal entity memo cannot be added without a description.")]
        public void AddLegalEntityMemoFieldValidation()
        {
            Navigate(NodeTypeEnum.Add);
            base.View.AddMemoWithoutMemoText(MemoStatus.UnResolved);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Description cannot be empty.", "Memo is a mandatory field");
        }

        /// <summary>
        /// Ensures that a legal entity memo cannot be added without a valid expiry date
        /// </summary>
        [Test, Description("Ensures that a legal entity memo cannot be added without a valid expiry date")]
        public void AddLegalEntityMemoWithoutExpiryDate()
        {
            Navigate(NodeTypeEnum.Add);
            base.View.AddMemoWithoutExpiryDate(MemoStatus.UnResolved);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Memo must have a valid Expiry Date");
        }

        /// <summary>
        /// Ensures that a legal entity memo cannot be added without a valid expiry date.
        /// </summary>
        [Test, Description("Ensures that a legal entity memo cannot be added without a valid expiry date.")]
        public void AddLegalEntityMemoWithoutReminderDate()
        {
            Navigate(NodeTypeEnum.Add);
            base.View.AddMemoWithoutReminderDate(MemoStatus.UnResolved);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Memo must have a valid Reminder Date");
        }

        /// <summary>
        /// This test will capture a memo record as unresolved and then go and update the same memo record to be resolved with new text. It will assert that we
        /// have not inserted a new memo record but have rather updated the one that was selected from the grid.
        /// </summary>
        [Test, Description(@"This test will capture a memo record as unresolved and then go and update the same memo record to be resolved with new text. It will
        assert that we have not inserted a new memo record but have rather updated the one that was selected from the grid.")]
        public void UpdateLegalEntityMemoToResolved()
        {
            Navigate(NodeTypeEnum.Add);
            string memoText = "Update Legal Entity Memo to Resolved";
            base.View.AddMemoRecord(MemoStatus.UnResolved, memoText);
            int memoKey = MemoAssertions.AssertLatestMemoInformation(_legalEntityKey, GenericKeyTypeEnum.LegalEntity_LegalEntityKey, MemoTable.Memo,
                memoText);
            memoText = "Resolved Memo";
            base.Browser.Navigate<LoanServicingCBO>().LegalEntityMemo(NodeTypeEnum.Update);
            base.View.UpdateMemo(memoKey, MemoStatus.Resolved, memoText);
            int updatedMemoKey = MemoAssertions.AssertLatestMemoInformation(_legalEntityKey, GenericKeyTypeEnum.LegalEntity_LegalEntityKey, MemoTable.Memo,
                memoText);
            MemoAssertions.AssertLatestMemoInformation(_legalEntityKey, GenericKeyTypeEnum.LegalEntity_LegalEntityKey, MemoTable.GeneralStatusKey,
                ((int)GeneralStatusEnum.Inactive).ToString());
            Assert.AreEqual(memoKey, updatedMemoKey);
        }

        /// <summary>
        /// This test will capture a memo record and then ensure that another user cannot login and change that memo record.
        /// </summary>
        [Test, Description("This test will capture a memo record and then ensure that another user cannot login and change that memo record.")]
        public void OtherUserCannotUpdateMemoRecord()
        {
            Navigate(NodeTypeEnum.Add);
            string memoText = "Update Legal Entity Memo to Resolved";
            base.View.AddMemoRecord(MemoStatus.UnResolved, memoText);
            int memoKey = MemoAssertions.AssertLatestMemoInformation(_legalEntityKey, GenericKeyTypeEnum.LegalEntity_LegalEntityKey, MemoTable.Memo, memoText);
            base.Browser.Dispose();
            Login(TestUsers.FLManager, Account.AccountKey);
            Navigate(NodeTypeEnum.Update);
            base.View.SelectMemoRecord(memoKey);
            Assert.That(base.View.UpdateButtonExists() == false,
                "The Update button is being displayed when it should be disabled for this user.");
            base.Browser.Dispose();
            base.Browser = null;
        }

        /// <summary>
        /// Tests that the expected elements of the legal entity memo add screen are displayed.
        /// </summary>
        [Test, Description("Tests that the expected elements of the legal entity memo screen are displayed.")]
        public void LegalEntityMemoAddViewCheck()
        {
            Navigate(NodeTypeEnum.Add);
            base.View.AssertAddFieldsExistsAndEnabled();
            var expectedContents = new List<string>() { MemoStatus.Resolved, MemoStatus.UnResolved };
            base.View.AssertMemoStatusList(expectedContents);
            expectedContents = new List<string>() { MemoStatus.Resolved, MemoStatus.UnResolved, MemoStatus.All };
            base.View.AssertAccountStatusList(expectedContents);
        }

        /// <summary>
        /// Tests that the expected elements of the legal entity memo update screen are displayed.
        /// </summary>
        [Test, Description("Tests that the expected elements of the legal entity memo update screen are displayed.")]
        public void LegalEntityMemoUpdateViewCheck()
        {
            Navigate(NodeTypeEnum.Update);
            var expectedContents = new List<string>() { MemoStatus.Resolved, MemoStatus.UnResolved };
            base.View.AssertMemoStatusList(expectedContents);
            expectedContents = new List<string>() { MemoStatus.Resolved, MemoStatus.UnResolved, MemoStatus.All };
            base.View.AssertAccountStatusList(expectedContents);
        }

        #region Helpers

        /// <summary>
        /// Logs in and loads up a random account into the Loan Servicing CBO. It will then navigate to the legal entity memo node provided.
        /// </summary>
        /// <param name="node"></param>
        private void Login(string user, int? overrideAccountKey = null)
        {
            base.Browser = new TestBrowser(user);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            Account = base.Service<IAccountService>().GetVariableLoanAccountByMainApplicantCount(1, 1, Common.Enums.AccountStatusEnum.Open);
            Account = overrideAccountKey == null ? Account : base.Service<IAccountService>().GetAccountByKey(overrideAccountKey.Value);
            _legalEntityKey = base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(Account.AccountKey);
        }

        private void Navigate(NodeTypeEnum node)
        {
            base.Browser.Navigate<LoanServicingCBO>().LegalEntityMemo(node);
        }

        #endregion Helpers
    }
}