using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LoanServicing.LoanDetail;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core;

namespace LoanServicingTests.Views.Account
{
    [RequiresSTA]
    public class LoanDetailTests : TestBase<LoanDetailAdd>
    {
        #region PrivateVariables

        private Automation.DataModels.Account Account;

        #endregion PrivateVariables

        #region SetupTearDown

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser, TestUsers.Password);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            Account = base.Service<IAccountService>().GetVariableLoanAccountByMainApplicantCount(1, 1, AccountStatusEnum.Open);
            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(Account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(Account.AccountKey);
        }

        #endregion SetupTearDown

        #region Validation

        /// <summary>
        /// This test ensures that the expected detail classes are available in the select list.
        /// </summary>
        [Test]
        public void ValidateDetailClasses()
        {
            base.Browser.Navigate<LoanServicingCBO>().LoanDetail(NodeTypeEnum.Add);
            var actualList = base.View.GetDetailClassList();
            var detailClasses = Service<IDetailTypeService>().GetDetailClassRecords();
            var expectedList = (from dc in detailClasses select dc.Description).ToList();
            WatiNAssertions.AssertSelectListContents(actualList, expectedList);
        }

        /// <summary>
        /// This test ensures that the expected detail types per detail class are available in the select list.
        /// </summary>
        /// <param name="detailclass">Detail Class</param>
        /// <param name="detailclassEnum">Detail Class</param>
        [Test, Sequential]
        public void ValidateDetailTypesByDetailClass(
            [Values(DetailClass.LoanManagement,
                    DetailClass.LoanIdentification,
                    DetailClass.CivilServants)] string detailclass,
            [Values(DetailClassEnum.LoanManagement,
                    DetailClassEnum.LoanIdentification,
                    DetailClassEnum.CivilServants)] DetailClassEnum detailclassEnum)
        {
            base.Browser.Navigate<LoanServicingCBO>().LoanDetail(NodeTypeEnum.Add);
            base.View.SelectDetailClass(detailclass);
            var actualList = base.View.GetDetailTypeList();
            var detailTypes = Service<IDetailTypeService>().GetDetailTypesForLoanServicingScreen(detailclassEnum, GeneralStatusEnum.Active, true);
            var expectedList = (from dt in detailTypes select dt.Description.Trim()).ToList();
            WatiNAssertions.AssertSelectListContents(actualList, expectedList);
        }

        /// <summary>
        /// This test ensures that the expected cancellation types are available in the select list.
        /// </summary>
        [Test]
        public void ValidateCancellationTypes()
        {
            base.Browser.Navigate<LoanServicingCBO>().LoanDetail(NodeTypeEnum.Add);
            base.View.SelectDetailClass(DetailClass.LoanManagement);
            base.View.SelectDetailType(DetailType.UnderCancellation);
            List<string> expectedList = new List<string> { CancellationType.Sale, CancellationType.Switch, CancellationType.Unkown };
            var actualList = base.View.GetCancellationTypeList();
            WatiNAssertions.AssertSelectListContents(actualList, expectedList);
        }

        /// <summary>
        /// This test validates the expected mandatory fields for adding a detail type.
        /// </summary>
        [Test]
        public void ValidateMandatoryFields()
        {
            base.Browser.Navigate<LoanServicingCBO>().LoanDetail(NodeTypeEnum.Add);
            base.View.ClickAdd();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Detail Class must be selected.",
                "Detail Type must be selected.",
                "Detail Date must be entered.");
        }

        /// <summary>
        /// This test validates the expected mandatory fields for adding a cancellation detail type.
        /// </summary>
        [Test]
        public void ValidateMandatoryFieldsCancellationType()
        {
            base.Browser.Navigate<LoanServicingCBO>().LoanDetail(NodeTypeEnum.Add);
            base.View.SelectDetailClass(DetailClass.LoanManagement);
            base.View.SelectDetailType(DetailType.UnderCancellation);
            base.Browser.Page<LoanDetailBase>().AddDetailDate(DateTime.Now);
            base.View.ClickAdd();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Cancellation Type must be selected.");
        }

        #endregion Validation

        #region AddLoanDetail

        /// <summary>
        /// This test ensures that a user can add a detail type against an account. It checks that the expected detail type now exists against
        /// the correct account.
        /// </summary>
        [Test]
        public void AddLoanDetail()
        {
            Service<IDetailTypeService>().RemoveDetailTypes(Account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanDetail(NodeTypeEnum.Add);
            var loanDetail = Service<IDetailTypeService>().GetEmptyDetailRecord();
            loanDetail.AccountKey = Account.AccountKey;
            loanDetail.LoanDetailType.LoanDetailClass.Description = DetailClass.LoanManagement;
            loanDetail.LoanDetailType.Description = DetailType.DebitOrderSuspended;
            loanDetail.DetailDate = DateTime.Now;
            loanDetail.Description = "Add Loan Detail Test.";
            base.View.PopulateView(loanDetail, null);
            base.View.ClickAdd();
            AccountAssertions.AssertDetailType(Account.AccountKey, DetailTypeEnum.DebitOrderSuspended, DetailClassEnum.LoanManagement, true);
        }

        #endregion AddLoanDetail

        #region UpdateLoanDetail

        /// <summary>
        /// This test ensures that a user can update a detail type record. It will check that the expected detail type record now exists with the
        /// updated details against the correct account.
        /// </summary>
        [Test]
        public void UpdateLoanDetail()
        {
            Service<IDetailTypeService>().RemoveDetailTypes(Account.AccountKey);
            Service<IDetailTypeService>().InsertDetailType(DetailTypeEnum.DebitOrderSuspended, Account.AccountKey);
            string description = "Update Loan Detail Test";
            var loanDetail = Service<IDetailTypeService>().GetLoanDetailRecord(DetailTypeEnum.DebitOrderSuspended, Account.AccountKey, DetailClassEnum.LoanManagement);
            loanDetail.AccountKey = Account.AccountKey;
            loanDetail.DetailDate = DateTime.Now.AddDays(1);
            loanDetail.Amount = 5000;
            loanDetail.Description = description;
            base.Browser.Navigate<LoanServicingCBO>().LoanDetail(NodeTypeEnum.Update);
            base.Browser.Page<LoanDetailUpdate>().PopulateUpdateView(loanDetail, null);
            base.Browser.Page<LoanDetailUpdate>().ClickUpdate();
            loanDetail = Service<IDetailTypeService>().GetLoanDetailRecord(DetailTypeEnum.DebitOrderSuspended, Account.AccountKey, DetailClassEnum.LoanManagement);
            Assert.That(loanDetail.Description == description, "");
        }

        /// <summary>
        /// This test ensures that a user can not update a specified detail type. Specific detail types are configured to not allow the record to be
        /// updated. The test checks that the relevant fields are not editable and the update button is disabled when selecting one of these records.
        /// </summary>
        [Test]
        public void UpdateLoanDetailDisallowed()
        {
            Service<IDetailTypeService>().RemoveDetailTypes(Account.AccountKey);
            Service<IDetailTypeService>().InsertDetailType(DetailTypeEnum.SuperLoelectedbyclient, Account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanDetail(NodeTypeEnum.Update);
            base.Browser.Page<LoanDetailBase>().SelectDetailType(DetailType.SuperLoelectedbyclient);
            base.Browser.Page<LoanDetailUpdate>().AssertUpdateFieldsDoNotExist();
            base.Browser.Page<LoanDetailUpdate>().AssertUpdateElementsDisabled();
        }

        #endregion UpdateLoanDetail

        #region DeleteLoanDetail

        /// <summary>
        /// This test ensures that a user can delete a detail type record. It will check that the expected detail type record no longer exists against
        /// the account.
        /// </summary>
        [Test]
        public void DeleteLoanDetail()
        {
            Service<IDetailTypeService>().RemoveDetailTypes(Account.AccountKey);
            Service<IDetailTypeService>().InsertDetailType(DetailTypeEnum.DebitOrderSuspended, Account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanDetail(NodeTypeEnum.Delete);
            base.Browser.Page<LoanDetailBase>().SelectDetailType(DetailType.DebitOrderSuspended);
            base.Browser.Page<LoanDetailDelete>().ClickDelete();
            AccountAssertions.AssertDetailType(Account.AccountKey, DetailTypeEnum.DebitOrderSuspended, DetailClassEnum.LoanManagement, false);
        }

        /// <summary>
        /// This test ensures that a user can not delete a specified detail type. Specific detail types are configured to not allow the record to be
        /// deleted. The test checks that the relevant fields are not editable and that the delete button is disabled when selecting one of these records.
        /// </summary>
        [Test]
        public void DeleteLoanDetailDisallowed()
        {
            Service<IDetailTypeService>().RemoveDetailTypes(Account.AccountKey);
            Service<IDetailTypeService>().InsertDetailType(DetailTypeEnum.DirectMortgageBondRegistered, Account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanDetail(NodeTypeEnum.Delete);
            base.Browser.Page<LoanDetailBase>().SelectDetailType(DetailType.DirectMortgageBondRegistered);
            base.Browser.Page<LoanDetailDelete>().AssertDeleteFieldsDoNotExist();
            base.Browser.Page<LoanDetailDelete>().AssertDeleteElementsDisabled();
        }

        #endregion DeleteLoanDetail
    }
}