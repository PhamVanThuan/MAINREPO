using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.FLOBO.PersonalLoan;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace PersonalLoansTests.Rules
{
    [RequiresSTA]
    public class SendTermExtensionLetter : PersonalLoansWorkflowTestBase<CorrespondenceProcessing>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new BuildingBlocks.TestBrowser(TestUsers.PersonalLoanClientServiceUser);
        }

        [Test]
        public void Cannot_Send_Term_Extension_Letter_When_Change_Term_Stage_Transition_Has_Not_Been_Written()
        {
            // Find a personal loan without a stage transition
            base.GenericKey = Service<IAccountService>().GetPersonalLoanAccountWithoutStageTransition();
            base.SearchAndLoadAccount();
            base.Browser.Navigate<PersonalLoanNode>().ClickCorrespondence();
            base.Browser.Navigate<PersonalLoanNode>().ClickTermExtensionLetter();
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessagesContains("The letter cannot be sent if the 'Change Term' stage transition is not written");
        }

        [Test]
        public void Can_Send_Term_Extension_Letter_When_Change_Term_Stage_Transition_Has_Been_Written()
        {
            base.GenericKey = Service<IAccountService>().GetPersonalLoanAccountWithoutStageTransition();
            base.SearchAndLoadAccount();
            Service<StageTransitionService>().InsertStageTransition(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_ChangeTerm, TestUsers.PersonalLoanClientServiceUser);
            base.Browser.Navigate<PersonalLoanNode>().ClickCorrespondence();
            base.Browser.Navigate<PersonalLoanNode>().ClickTermExtensionLetter();
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(this.GenericKey, CorrespondenceReports.PersonalLoanChangeTermExtensionLetter, CorrespondenceMedium.Email);
        }
    }
}