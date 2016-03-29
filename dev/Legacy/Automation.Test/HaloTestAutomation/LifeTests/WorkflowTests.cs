#region
using System.Linq;
using Automation.DataModels;
using Automation.Framework;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Life;
using BuildingBlocks.Services.Contracts;
using BuildingBlocks.Timers;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using WatiN.Core;
using WorkflowAutomation.Harness;

#endregion

namespace SAHL.LifeTests
{
    [TestFixture, RequiresSTA]
    public class WorkflowTests : LifeTestBase
    {
        protected override void OnTestStart()
        {
            if (base.Browser == null)
            {
                base.Browser = new TestBrowser(TestUsers.LifeConsultant);
            }
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.OnTestStart();
        }

        [Test]
        public void _A_BatchLeadCreate()
        {
            base.ProcessLifeLeads(60);
        }


        [Test]
        public void _B_45DayTimerAtContactClient()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.ContactClient);
            scriptEngine.ExecuteScript(WorkflowEnum.LifeOrigination, Common.Constants.WorkflowAutomationScripts.LifeOrigination._45DayTimeout, lifeLead.OfferKey);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.ParentClosed);
        }

        [Test, Repeat(6)]
        public void _C_CreateCallBackAtContactClient()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.ContactClient);
            base.Browser.Page<WorkflowSuperSearch>().Search(lifeLead.OfferKey);
            base.Browser.ClickAction(WorkflowActivities.LifeOrigination.CreateCallback);
            base.Browser.Page<Life_Callback>().CreateCallBack("Quoted", DateTime.Now.AddDays(1.0), "Test");
            base.Service<IX2WorkflowService>().WaitForX2State(lifeLead.OfferKey, Workflows.LifeOrigination, WorkflowStates.LifeOriginationWF.CallbackHold);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.CallbackHold);
        }

        [Test, Repeat(6)]
        public void _D_WaitForCallbackTimerAtCallbackHold()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.ContactClient);
            base.Browser.Page<WorkflowSuperSearch>().Search(lifeLead.OfferKey);
            base.Browser.ClickAction(WorkflowActivities.LifeOrigination.CreateCallback);
            base.Browser.Page<Life_Callback>().CreateCallBack("Quoted", DateTime.Now.AddDays(1.0), "Test");
            scriptEngine.ExecuteScript(WorkflowEnum.LifeOrigination, Common.Constants.WorkflowAutomationScripts.LifeOrigination.WaitforCallback, lifeLead.OfferKey);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.ReadytoCallback);
        }

        [Test, Repeat(1)]
        public void _E_NTUPolicyAtContactClient()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.ContactClient);
            base.Browser.Page<WorkflowSuperSearch>().Search(lifeLead.OfferKey);
            base.Browser.ClickAction(WorkflowActivities.LifeOrigination.NTUPolicy);
            base.Browser.Page<Life_NTU>().NTULifePolicy("Ceded a policy");
            base.Service<IX2WorkflowService>().WaitForX2State(lifeLead.OfferKey, Workflows.LifeOrigination, WorkflowStates.LifeOriginationWF.NTUHold);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.NTUHold);
            scriptEngine.ExecuteScript(WorkflowEnum.LifeOrigination, Common.Constants.WorkflowAutomationScripts.LifeOrigination.ArchiveNTU, lifeLead.OfferKey);
            OfferAssertions.AssertOfferStatus(lifeLead.OfferKey, OfferStatusEnum.NTU);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.PolicyNTUd);
        }

        [Test, Repeat(5)]
        public void _F_ConfirmDetailsAtContactClient()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.ContactClient);
            base.Browser.Page<WorkflowSuperSearch>().Search(lifeLead.OfferKey);
            base.Browser.Page<Life_Contact>().ConfirmDetailsGoNext();
            base.Service<IX2WorkflowService>().WaitForX2State(lifeLead.OfferKey, Workflows.LifeOrigination, WorkflowStates.LifeOriginationWF.LOA);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.LOA);
        }

        [Test, Repeat(5)]
        public void _G_ConfirmLOA()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.LOA);
            base.Browser.Page<WorkflowSuperSearch>().Search(lifeLead.OfferKey);
            base.Browser.Page<Life_LOA>().ConfirmLOAGoNext();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Service<IX2WorkflowService>().WaitForX2State(lifeLead.OfferKey, Workflows.LifeOrigination, WorkflowStates.LifeOriginationWF.Benefits);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.Benefits);
        }

        [Test, Repeat(1)]
        public void _H_ChangeToAccidentalPolicy()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.Benefits);
            lifeLead.ProductSwitchReason = "Too expensive";
            base.Browser.Page<WorkflowSuperSearch>().Search(lifeLead.OfferKey);
            base.Browser.Page<Life_Benefits>().CheckAcceptBenefits();
            base.Browser.Page<Life_Benefits>().ClickNext();
            base.Browser.ClickAction(WorkflowActivities.LifeOrigination.ChangePolicyType);
            base.Browser.Page<Life_ProductSwitch>().SwitchTo("Accident Only Cover");
            base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(lifeLead.ProductSwitchReason);
            LifeAssertions.AssertLifeOfferPolicyType(lifeLead.OfferKey, "Accident Only Cover");
        }

        [Test, Repeat(4)]
        public void _I_AcceptBenefits()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.Benefits);
            base.Browser.Page<WorkflowSuperSearch>().Search(lifeLead.OfferKey);
            base.Browser.Page<Life_Benefits>().BenefitsMustBeExplainedRule();
            base.Browser.Page<BasePageAssertions>().AssertFrameContainsText("Please tick either checkbox: Client is proceeding with policy OR Client has refused cover.");
            base.Browser.Page<Life_Benefits>().CheckAcceptBenefits();
            base.Browser.Page<Life_Benefits>().ClickNext();
            base.Service<IX2WorkflowService>().WaitForX2State(lifeLead.OfferKey, Workflows.LifeOrigination, WorkflowStates.LifeOriginationWF.Quote);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.Quote);
        }

        [Test, Repeat(1)]
        public void _J_DeclineQuoteAndReActivatePolicy()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.Quote);
            base.Browser.Page<WorkflowSuperSearch>().Search(lifeLead.OfferKey);
            base.Browser.ClickAction(WorkflowActivities.LifeOrigination.DeclineQuote);
            base.Browser.Page<Life_NTU>().NTULifePolicy("Ceded a policy");
            base.Service<IX2WorkflowService>().WaitForX2State(lifeLead.OfferKey, Workflows.LifeOrigination, WorkflowStates.LifeOriginationWF.NTUHold, 4);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.NTUHold);
            base.Browser.ClickAction(WorkflowActivities.LifeOrigination.ReactivateNTUdPolicy);
            base.Service<IX2WorkflowService>().WaitForX2State(lifeLead.OfferKey, Workflows.LifeOrigination, WorkflowStates.LifeOriginationWF.Quote);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.Quote);
        }

        [Test, Repeat(3)]
        public void _K_AcceptQuote()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.Quote);
            base.Browser.Page<WorkflowSuperSearch>().Search(lifeLead.OfferKey);
            base.Browser.Page<Life_PolicyWorkFlow>().AcceptPlan();
            base.Service<IX2WorkflowService>().WaitForX2State(lifeLead.OfferKey, Workflows.LifeOrigination, WorkflowStates.LifeOriginationWF.Exclusions);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.Exclusions);
        }

        [Test, Repeat(2)]
        public void _L_AcceptExclusionsDeathOnly()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.Exclusions);
            base.Browser.Page<WorkflowSuperSearch>().Search(lifeLead.OfferKey);
            //Choose Death Benefit only
            base.Browser.Page<Life_Exclusions>().SelectDeathOnly();
            //Screen Validation Test
            base.Browser.Page<Life_Exclusions>().AssertExclusionsMustBeAccepted();
            base.Browser.Page<Life_Exclusions>().AcceptExclusionsGoNext();
            base.Service<IX2WorkflowService>().WaitForX2State(lifeLead.OfferKey, Workflows.LifeOrigination, WorkflowStates.LifeOriginationWF.RPAR);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.RPAR);
        }

        [Test, Repeat(1)]
        public void _M_AcceptExclusionsDeathWithIPB()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.Exclusions);
            base.Browser.Page<WorkflowSuperSearch>().Search(lifeLead.OfferKey);
            //Choose Death With IB
            base.Browser.Page<Life_Exclusions>().SelectDeathWithIPB();
            //Screen Validation Test
            base.Browser.Page<Life_Exclusions>().AssertExclusionsMustBeAccepted();
            base.Browser.Page<Life_Exclusions>().AcceptExclusionsGoNext();
            base.Service<IX2WorkflowService>().WaitForX2State(lifeLead.OfferKey, Workflows.LifeOrigination, WorkflowStates.LifeOriginationWF.RPAR);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.RPAR);
        }

        [Test, Repeat(3)]
        public void _N_AcceptRPAR()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.RPAR);
            base.Browser.Page<WorkflowSuperSearch>().Search(lifeLead.OfferKey);
            base.Browser.Page<Life_RPAR>().AcceptRPARGoNext(false);
            base.Service<IX2WorkflowService>().WaitForX2State(lifeLead.OfferKey, Workflows.LifeOrigination, WorkflowStates.LifeOriginationWF.Declaration);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.Declaration);
        }

        [Test, Repeat(3)]
        public void _O_AcceptDeclaration()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.Declaration);
            base.Browser.Page<WorkflowSuperSearch>().Search(lifeLead.OfferKey);
            base.Browser.Page<Life_Declaration>().AllPointsMustBeAcceptedRule();
            base.Browser.Page<BasePageAssertions>().AssertFrameContainsText("All points must be accepted before you can continue.");
            base.Browser.Page<Life_Declaration>().AcceptDeclarationGoNext();
            base.Service<IX2WorkflowService>().WaitForX2State(lifeLead.OfferKey, Workflows.LifeOrigination, WorkflowStates.LifeOriginationWF.FAIS);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.FAIS);
        }

        [Test, Repeat(1)]
        public void _P_AcceptFAISWithoutConfirmation()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.FAIS);
            base.Browser.Page<WorkflowSuperSearch>().Search(lifeLead.OfferKey);
            base.Browser.WaitForComplete();

            //Assertion 01
            base.Browser.ClickAction(WorkflowActivities.LifeOrigination.AcceptFAIS);
            base.Browser.Page<Life_FAIS>().AcceptFAISGoNext();
            base.Browser.Page<BasePageAssertions>().AssertFrameContainsText("All points must be accepted before you can continue.");

            //Assertion 02
            base.Browser.ClickAction(WorkflowActivities.LifeOrigination.AcceptFAIS);
            base.Browser.Page<Life_FAIS>().Populate(false, false, true, "");
            base.Browser.Page<Life_FAIS>().AcceptFAISGoNext();
            base.Browser.Page<BasePageAssertions>().AssertFrameContainsText("Must select whether there is a second Life Insured.");

            //Assertion 03
            base.Browser.ClickAction(WorkflowActivities.LifeOrigination.AcceptFAIS);
            base.Browser.Page<Life_FAIS>().Populate(true, false, true, "");
            base.Browser.Page<Life_FAIS>().AcceptFAISGoNext();
            base.Browser.Page<BasePageAssertions>().AssertFrameContainsText("Contact Number must be entered.");

            base.Browser.ClickAction(WorkflowActivities.LifeOrigination.AcceptFAIS);
            base.Browser.Page<Life_FAIS>().Populate(false, true, true, "");
            base.Browser.Page<Life_FAIS>().AcceptFAISGoNext();

            base.Service<IX2WorkflowService>().WaitForX2State(lifeLead.OfferKey, Workflows.LifeOrigination, WorkflowStates.LifeOriginationWF.ReadytoSend);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.ReadytoSend);
        }

        [Test, Repeat(2)]
        public void _Q_AcceptFAISWithConfirmation()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.FAIS);
            base.Browser.Page<WorkflowSuperSearch>().Search(lifeLead.OfferKey);
            base.Browser.ClickAction(WorkflowActivities.LifeOrigination.AcceptFAIS);
            base.Browser.Page<Life_FAIS>().Populate(true, false, true, "1234567");
            base.Browser.Page<Life_FAIS>().AcceptFAISGoNext();
            base.Service<IX2WorkflowService>().WaitForX2State(lifeLead.OfferKey, Workflows.LifeOrigination, WorkflowStates.LifeOriginationWF.AwaitingConfirmation);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.AwaitingConfirmation);
        }

        [Test, Repeat(2)]
        public void _R_ConfirmQuote()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.AwaitingConfirmation);
            base.Browser.Page<WorkflowSuperSearch>().Search(lifeLead.OfferKey);
            base.Browser.Page<Life_PolicyWorkFlow>().AcceptPlan();
            base.Service<IX2WorkflowService>().WaitForX2State(lifeLead.OfferKey, Workflows.LifeOrigination, WorkflowStates.LifeOriginationWF.ExclusionsConfirmation);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.ExclusionsConfirmation);
        }

        [Test, Repeat(1)]
        public void _S_ConfirmExclusionsDeathOnly()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.ExclusionsConfirmation);
            base.Browser.Page<WorkflowSuperSearch>().Search(lifeLead.OfferKey);
            base.Browser.Page<Life_Exclusions>().SelectDeathOnly();
            base.Browser.Page<Life_Exclusions>().AcceptExclusionsGoNext();
            base.Service<IX2WorkflowService>().WaitForX2State(lifeLead.OfferKey, Workflows.LifeOrigination, WorkflowStates.LifeOriginationWF.DeclarationConfirmation);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.DeclarationConfirmation);
        }

        [Test, Repeat(1)]
        public void _T_ConfirmExclusionsDeathWithIPB()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.ExclusionsConfirmation);
            base.Browser.Page<WorkflowSuperSearch>().Search(lifeLead.OfferKey);
            base.Browser.Page<Life_Exclusions>().SelectDeathWithIPB();
            base.Browser.Page<Life_Exclusions>().AcceptExclusionsGoNext();
            base.Service<IX2WorkflowService>().WaitForX2State(lifeLead.OfferKey, Workflows.LifeOrigination, WorkflowStates.LifeOriginationWF.DeclarationConfirmation);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.DeclarationConfirmation);
        }

        [Test, Repeat(1)]
        public void _U_ConfirmDeclaration()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.DeclarationConfirmation);
            base.Browser.Page<WorkflowSuperSearch>().Search(lifeLead.OfferKey);
            base.Browser.Page<Life_Declaration>().AcceptDeclarationGoNext();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Service<IX2WorkflowService>().WaitForX2State(lifeLead.OfferKey, Workflows.LifeOrigination, WorkflowStates.LifeOriginationWF.FAISConfirmation);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.FAISConfirmation);
        }

        [Test, Repeat(1)]
        public void _V_ConfirmFAIS()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.FAISConfirmation);
            base.Browser.Page<WorkflowSuperSearch>().Search(lifeLead.OfferKey);
            base.Browser.Page<Life_FAIS>().Populate(false, false, true, "");
            base.Browser.Page<Life_FAIS>().AcceptFAISGoNext();
            base.Service<IX2WorkflowService>().WaitForX2State(lifeLead.OfferKey, Workflows.LifeOrigination, WorkflowStates.LifeOriginationWF.ReadytoSend);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.ReadytoSend);
        }

        [Test, Repeat(1)]
        public void _W_SendPolicyDocument()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.ReadytoSend);
            base.Browser.Page<WorkflowSuperSearch>().Search(lifeLead.OfferKey);
            base.Browser.Page<Life_Correspondence_LetterOfAcceptance>().SendPolicyDocument(CorrespondenceMediumEnum.Email, "mail@mail.co.za", string.Empty, string.Empty);
            LifeAssertions.AssertLifePolicyStatus(lifeLead, LifePolicyStatusEnum.Accepted);
        }

        [Test, Repeat(1)]
        public void _X_ContinueAtCallBackHold()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.CallbackHold);
            base.Browser.Page<WorkflowSuperSearch>().Search(lifeLead.OfferKey);
            base.Service<IX2WorkflowService>().WaitForX2State(lifeLead.OfferKey, Workflows.LifeOrigination, WorkflowStates.LifeOriginationWF.CallbackHold, 10);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.CallbackHold);
            base.Browser.ClickAction(WorkflowActivities.LifeOrigination.ContinueSale);
            base.Service<IX2WorkflowService>().WaitForX2State(lifeLead.OfferKey, Workflows.LifeOrigination, WorkflowStates.LifeOriginationWF.ContactClient);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.ContactClient);
        }

        [Test, Repeat(1)]
        public void _Y_ContinueAtReadyToCallback()
        {
            var lifeLead = this.GetUnusedLifeLeadAtState(Common.Constants.WorkflowStates.LifeOriginationWF.ReadytoCallback);
            base.Browser.Page<WorkflowSuperSearch>().Search(lifeLead.OfferKey);
            base.Browser.ClickAction(WorkflowActivities.LifeOrigination.ContinuewithSale);
            base.Service<IX2WorkflowService>().WaitForX2State(lifeLead.OfferKey, Workflows.LifeOrigination, WorkflowStates.LifeOriginationWF.ContactClient);
            LifeAssertions.AssertCurrentLifeX2State(lifeLead.OfferKey.ToString(), WorkflowStates.LifeOriginationWF.ContactClient);
        }
    }
}