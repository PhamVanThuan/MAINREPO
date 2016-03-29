using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Threading;

namespace ApplicationCaptureTests.Views.LegalEntityFloBo
{
    /// <summary>
    /// Contains the tests for the Legal Entity Memo FloBo Node
    /// </summary>
    [RequiresSTA]
    public class LegalEntityMemoTests : TestBase<GenericMemoAdd>
    {
        /// <summary>
        /// OfferKey for tests
        /// </summary>
        private int _offerKey;

        #region Setup/Teardown

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = Helper.CreateApplicationWithBrowser(TestUsers.BranchConsultant, out _offerKey);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.ApplicationCaptureWF.ApplicationCapture, _offerKey);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.Browser.Navigate<LegalEntityNode>().LegalEntity(_offerKey);
        }

        #endregion Setup/Teardown

        #region Tests

        /// <summary>
        /// This test ensures a memo record can be added to a Legal Entity.
        /// </summary>
        [Test, Description("This test ensures a memo record can be added to a Legal Entity.")]
        public void _001_AddLegalEntityMemo()
        {
            base.Browser.Navigate<LegalEntityNode>().LegalEntityMemo(NodeTypeEnum.Add);
            const string memoText = "Test Memo Description";
            base.View.AddMemoRecord(MemoStatus.UnResolved, memoText);
            //assert it exists
            int legalEntityKey = base.Service<IApplicationService>().GetFirstApplicantLegalEntityKeyOnOffer(_offerKey);
            MemoAssertions.AssertLatestMemoInformation(legalEntityKey, GenericKeyTypeEnum.LegalEntity_LegalEntityKey, MemoTable.Memo, memoText);
        }

        /// <summary>
        /// This test ensures that a user cannot update another users memo records.
        /// </summary>
        [Test, Description("This test ensures that a user cannot update another users memo records.")]
        public void _002_UpdateButtonDisabledForOtherUsers()
        {
            //login is a different consultant
            var consultantbrowser = new TestBrowser(TestUsers.BranchConsultant10);
            consultantbrowser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(consultantbrowser);
            consultantbrowser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            consultantbrowser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(consultantbrowser);
            consultantbrowser.Page<WorkflowSuperSearch>().Search(_offerKey);
            //go to the legal entity node
            consultantbrowser.Navigate<LegalEntityNode>().LegalEntity(_offerKey);
            consultantbrowser.Navigate<LegalEntityNode>().LegalEntityMemo(NodeTypeEnum.Update);
            //the update button should not exist
            Thread.Sleep(1500);
            Assert.True(!consultantbrowser.Page<GenericMemoAdd>().UpdateButtonExists(),
                "The Update button is being displayed when it should be disabled for this user.");
            consultantbrowser.Dispose();
        }

        /// <summary>
        /// This test ensures a memo record against a Legal Entity can be updated.
        /// </summary>
        [Test, Description("This test ensures a memo record against a Legal Entity can be updated.")]
        public void _003_UpdateLegalEntityMemo()
        {
            base.Browser.Navigate<LegalEntityNode>().LegalEntityMemo(NodeTypeEnum.Update);
            const string memoText = "Test Memo Description";
            const string newMemoText = "Updated Memo Description";
            base.View.UpdateMemo(memoText, MemoStatus.Resolved, newMemoText);
            //assert it exists
            int legalEntityKey = base.Service<IApplicationService>().GetFirstApplicantLegalEntityKeyOnOffer(_offerKey);
            MemoAssertions.AssertLatestMemoInformation(legalEntityKey, GenericKeyTypeEnum.LegalEntity_LegalEntityKey, MemoTable.Memo, newMemoText);
        }

        /// <summary>
        /// This test ensures that a memo description is provided before the record can be added.
        /// </summary>
        [Test, Description("This test ensures that a reminder date is provided before the record can be added.")]
        public void _004_MemoDescriptionRequired()
        {
            base.Browser.Navigate<LegalEntityNode>().LegalEntityMemo(NodeTypeEnum.Add);
            base.View.AddMemoWithoutMemoText(MemoStatus.UnResolved);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Description cannot be empty.");
        }

        /// <summary>
        /// This test ensures that a reminder date is provided before the record can be added.
        /// </summary>
        [Test, Description("This test ensures that a reminder date is provided before the record can be added.")]
        public void _005_ReminderDateRequired()
        {
            base.Browser.Navigate<LegalEntityNode>().LegalEntityMemo(NodeTypeEnum.Add);
            base.View.AddMemoWithoutReminderDate(MemoStatus.UnResolved);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Memo must have a valid Reminder Date");
        }

        /// <summary>
        /// This test ensures that an expiry date is provided before the record can be added.
        /// </summary>
        [Test, Description("This test ensures that an expiry date is provided before the record can be added.")]
        public void _006_ExpiryDateRequired()
        {
            base.Browser.Navigate<LegalEntityNode>().LegalEntityMemo(NodeTypeEnum.Add);
            base.View.AddMemoWithoutExpiryDate(MemoStatus.UnResolved);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Memo must have a valid Expiry Date");
        }

        #endregion Tests
    }
}