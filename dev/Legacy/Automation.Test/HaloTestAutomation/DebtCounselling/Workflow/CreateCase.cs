using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.DebtCounselling;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;
using Navigation = BuildingBlocks.Navigation;

namespace DebtCounsellingTests.Workflow
{
    [RequiresSTA]
    public sealed class CreateCase : DebtCounsellingTests.TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingAdmin);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<Navigation.MenuNode>().LossControlNode();
            base.Browser.Navigate<Navigation.MenuNode>().CreateCase();
        }

        /// <summary>
        /// Test ensures the user is required to select a debt counsellor before continuing to the next screen of the case creation.
        /// </summary>
        [Test, Description("Ensures the user is required to select a debt counsellor before continuing to the next screen of the case creation.")]
        public void CreateCaseDebtCounsellorMandatory()
        {
            //click the button
            base.Browser.Page<DebtCounsellorSelect>().ClickButton(ButtonTypeEnum.Select);
            //check the validation message exists
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("No item selected to update.");
        }

        /// <summary>
        /// This will create and assert that a debtcounselling case is created correctly
        /// </summary>
        public void CreateDebtCounsellingCase()
        {
            string idNumber = WorkflowHelper.CreateCase(false, 1, base.Browser, false, DateTime.Now);
            var account = Service<IDebtCounsellingService>().GetDebtCounsellingAccountListByLegalEntityIDNumber(idNumber).FirstOrDefault(); ;
            int debtCounsellingKey = Service<IDebtCounsellingService>().GetDebtCounsellingKey(account);
            //assertions
            StageTransitionAssertions.AssertStageTransitionCreated(debtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_17_1Received);
        }

        /// <summary>
        /// This test ensures that at least one person has to be selected before a case can be created
        /// </summary>
        [Test, Description("This test ensures that at least one person has to be selected before a case can be created")]
        public void CreateCaseSelectionMandatory()
        {
            base.Browser.Page<DebtCounsellorSelect>().SearchByNCRNumber(Service<IDebtCounsellingService>().GetNCRNumber());
            base.Browser.Page<DebtCounsellorSelect>().ClickButton(ButtonTypeEnum.Select);
            base.Browser.Page<DebtCounsellingCreateCase>().PopulateView(DateTime.Now);
            //on the create screen
            base.Browser.Page<DebtCounsellingCreateCase>().SearchAndAddPeopleofImportance(
                Service<ILegalEntityService>().GetRandomLegalEntityIdNumberOnAccount()
                );
            //deselect all LE's
            base.Browser.Page<DebtCounsellingCreateCase>().DeselectAllCheckBoxes();
            base.Browser.Page<DebtCounsellingCreateCase>().ClickCreateCase();
            //assertions
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("No items selected to create a Debt Counseling case.");
        }

        /// <summary>
        /// Ensures that the 17.1 Date is mandatory when creating a case
        /// </summary>
        [Test, Description("Ensures that the 17.1 Date is mandatory when creating a case")]
        public void Mandatory17pt1DateOnCaseCreate()
        {
            base.Browser.Page<DebtCounsellorSelect>().SearchByNCRNumber(Service<IDebtCounsellingService>().GetNCRNumber());
            base.Browser.Page<DebtCounsellorSelect>().ClickButton(ButtonTypeEnum.Select);
            base.Browser.Page<DebtCounsellingCreateCase>().Remove17pt1Date();
            //on the create screen
            base.Browser.Page<DebtCounsellingCreateCase>().SearchAndAddPeopleofImportance(
                Service<ILegalEntityService>().GetRandomLegalEntityIdNumberOnAccount()
                );
            //deselect all LE's
            base.Browser.Page<DebtCounsellingCreateCase>().ClickCreateCase();
            //assertions
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The 17.1 Date is required.");
        }

        /// <summary>
        /// Ensures that the 17.1 Date is mandatory when creating a case
        /// </summary>
        [Test, Description("Ensures that the 17.1 Date cannot be a future date when creating a case")]
        public void FutureDated17pt1DateOnCaseCreateNotAllowed()
        {
            base.Browser.Page<DebtCounsellorSelect>().SearchByNCRNumber(Service<IDebtCounsellingService>().GetNCRNumber());
            base.Browser.Page<DebtCounsellorSelect>().ClickButton(ButtonTypeEnum.Select);
            base.Browser.Page<DebtCounsellingCreateCase>().PopulateView(DateTime.Now.AddDays(1));
            //on the create screen
            base.Browser.Page<DebtCounsellingCreateCase>().SearchAndAddPeopleofImportance(
                Service<ILegalEntityService>().GetRandomLegalEntityIdNumberOnAccount()
                );
            //deselect all LE's
            base.Browser.Page<DebtCounsellingCreateCase>().ClickCreateCase();
            //assertions
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The 17.1 Date cannot be in the future.");
        }

        /// <summary>
        /// When a DCA user creates a case, a user should be able to add a reference number to the debt counselling case
        /// </summary>
        [Test, Description(@"When a DCA user creates a case, a user should be able to add a reference number to the debt counselling case")]
        public void AddReferenceToDebtCounsellingCase()
        {
            var random = new Random();
            //search for a case at Pend Proposal
            DateTime dateFor17pt1 = DateTime.Now.AddDays(-1);
            string referenceNumber = string.Format("Test-Ref-{0}-{1}", random.Next(0, 100000).ToString(), random.Next(0, 1000).ToString());
            //we need to create one
            string idNumber = WorkflowHelper.CreateCase(true, 1, base.Browser, true, dateFor17pt1, reference: referenceNumber);
            var accountList = Service<ILegalEntityService>().GetLegalEntityLoanAccountsByIDNumber(idNumber);
            //get the debtcounselling key
            int accountkey = (from a in accountList select a.AccountKey).FirstOrDefault();
            int debtCounsellingKey = Service<IDebtCounsellingService>().GetDebtCounsellingKey(accountkey);
            //the reference number should be correct
            DebtCounsellingAssertions.AssertReference(debtCounsellingKey, referenceNumber);
        }
    }
}