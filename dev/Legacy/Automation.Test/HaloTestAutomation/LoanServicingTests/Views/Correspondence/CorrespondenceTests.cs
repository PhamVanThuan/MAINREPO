using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LoanServicing.Correspondence;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;

namespace LoanServicingTests.Views.Correspondence
{
    [RequiresSTA]
    public class CorrespondenceTests : TestBase<CorrespondenceProcessing>
    {
        private int loanaccountkey;
        private int hocaccountkey;
        private int lifeaccountkey;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);

            //We are only getting accounts that have one Life and HOC account!
            var acc = base.Service<IClientEmailService>().GetUnusedMortgageAccountForCorrespondenceTests().FirstOrDefault(); ;
            loanaccountkey = acc.AccountKey;
            hocaccountkey = acc.HOCAccountKey;
            lifeaccountkey = acc.LifeAccountKey;
            var legalentitykeys = from role in base.Service<ILegalEntityService>().GetLegalEntityRoles(loanaccountkey)
                                  select role.LegalEntityKey;
            foreach (var leKey in legalentitykeys)
                base.Service<IClientEmailService>().UpdateLegalEntityEmailAddress(leKey, "testUser@testemail.com");

            base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(loanaccountkey);
        }

        #region Sending Instalment Letter

        [Test]
        public void Account_WhenSendingInstalmentLetter_ShouldWriteToImageIndex()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(3);
            base.Browser.Navigate<LoanServicingCBO>().InstalmentLetter();
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            CorrespondenceAssertions.AssertImageIndex(this.loanaccountkey.ToString(), CorrespondenceReports.InstalmentLetter, CorrespondenceMedium.Email, loanaccountkey, 0);
        }

        [Test]
        public void Account_WhenSendingInstalmentLetter_ShouldHaveValidControls()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(3);
            base.Browser.Navigate<LoanServicingCBO>().InstalmentLetter();
            base.View.AssertControlsValid();
        }

        [Test]
        public void Account_WhenSendingInstalmentLetter_ShouldHaveValidationMessages()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(3);
            base.Browser.Navigate<LoanServicingCBO>().InstalmentLetter();
            base.View.SendCorrespondence();
            base.View.AssertAllValidationMessagesExist();
        }

        #endregion Sending Instalment Letter

        #region Sending Acknowledge Cancellation Instruction

        [Test]
        public void Account_WhenSendingAcknowledgeCancellationInstruction_ShouldWriteToImageIndex()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(3);
            base.Browser.Navigate<LoanServicingCBO>().AcknowledgeCancellationInstruction();
            var cancellationType = "Arrear Accounts";
            base.Browser.Page<Correspondence_CancellationLetterAcknowledge>().Populate(cancellationType);
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            CorrespondenceAssertions.AssertImageIndex(this.loanaccountkey.ToString(), CorrespondenceReports.AcknowledgeCancellationInstruction, CorrespondenceMedium.Email, loanaccountkey, 0);
        }

        [Test]
        public void Account_WhenSendingAcknowledgeCancellationInstruction_ShouldHaveValidControls()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(3);
            base.Browser.Navigate<LoanServicingCBO>().AcknowledgeCancellationInstruction();
            base.View.AssertControlsValid();
            base.Browser.Page<Correspondence_CancellationLetterAcknowledge>().AssertControlsValid();
        }

        [Test]
        public void Account_WhenSendingAcknowledgeCancellationInstruction_ShouldHaveValidationMessages()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(3);
            base.Browser.Navigate<LoanServicingCBO>().AcknowledgeCancellationInstruction();
            base.View.SendCorrespondence();
            base.View.AssertAllValidationMessagesExist();
        }

        #endregion Sending Acknowledge Cancellation Instruction

        #region Sending Income Tax Statement

        [Test]
        public void Account_WhenSendingIncomeTaxStatement_ShouldWriteToImageIndex()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(3);
            base.Browser.Navigate<LoanServicingCBO>().IncomeTaxStatement();
            var taxPeriod = "1995/96";
            base.Browser.Page<Correspondence_IncomeTaxStatement>().Populate(taxPeriod);
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            CorrespondenceAssertions.AssertImageIndex(this.loanaccountkey.ToString(), CorrespondenceReports.IncomeTaxStatement, CorrespondenceMedium.Email, loanaccountkey, 0);
        }

        [Test]
        public void Account_WhenSendingIncomeTaxStatement_ShouldHaveValidControls()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(3);
            base.Browser.Navigate<LoanServicingCBO>().IncomeTaxStatement();
            base.View.AssertControlsValid();
            base.Browser.Page<Correspondence_IncomeTaxStatement>().AssertControlsValid();
        }

        [Test]
        public void Account_WhenSendingIncomeTaxStatement_ShouldHaveValidationMessages()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(3);
            base.Browser.Navigate<LoanServicingCBO>().IncomeTaxStatement();
            base.View.SendCorrespondence();
            base.View.AssertAllValidationMessagesExist();
        }

        #endregion Sending Income Tax Statement

        #region SendingZ299

        [Test]
        public void Account_WhenSendingZ299_ShouldWriteToImageIndex()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(3);
            base.Browser.Navigate<LoanServicingCBO>().Z299();
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            CorrespondenceAssertions.AssertImageIndex(this.loanaccountkey.ToString(), CorrespondenceReports.Z299, CorrespondenceMedium.Email, loanaccountkey, 0);
        }

        [Test]
        public void Account_WhenSendingZ299_ShouldHaveValidControls()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(3);
            base.Browser.Navigate<LoanServicingCBO>().Z299();
            base.View.AssertControlsValid();
        }

        [Test]
        public void Account_WhenSendingZ299_ShouldHaveValidationMessages()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(3);
            base.Browser.Navigate<LoanServicingCBO>().Z299();
            base.View.SendCorrespondence();
            base.View.AssertAllValidationMessagesExist();
        }

        #endregion SendingZ299

        #region Sending Insolvencies Power of Attorney

        [Test]
        public void Account_WhenSendingInsolvenciesPowerofAttorney_ShouldWriteToImageIndex()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(3);
            base.Browser.Navigate<LoanServicingCBO>().InsolvenciesPowerofAttorney();
            var litigationSupervisor = "SupervisorUser";
            var attorney = @"SAHL\HAUser";
            var legalentities = "Mr Neeran Besesar";
            base.Browser.Page<Correspondence_PowerOfAttorney>().Populate(litigationSupervisor, attorney, legalentities);
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            CorrespondenceAssertions.AssertImageIndex(this.loanaccountkey.ToString(), CorrespondenceReports.InsolvenciesPowerofAttorney, CorrespondenceMedium.Email, loanaccountkey, 0);
        }

        [Test]
        public void Account_WhenSendingInsolvenciesPowerofAttorney_ShouldHaveValidControls()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(3);
            base.Browser.Navigate<LoanServicingCBO>().InsolvenciesPowerofAttorney();
            base.View.AssertControlsValid();
            base.Browser.Page<Correspondence_PowerOfAttorney>().AssertControlsValid();
        }

        [Test]
        public void Account_WhenSendingInsolvenciesPowerofAttorney_ShouldHaveValidationMessages()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(3);
            base.Browser.Navigate<LoanServicingCBO>().InsolvenciesPowerofAttorney();
            base.View.SendCorrespondence();
            base.View.AssertAllValidationMessagesExist();
            base.Browser.Page<Correspondence_PowerOfAttorney>().AssertAllValidationMessagesExist();
        }

        #endregion Sending Insolvencies Power of Attorney

        #region Sending Insolvencies Statement of loanaccountkey

        [Test]
        public void Account_WhenSendingInsolvenciesStatementofAccount_ShouldWriteToImageIndex()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(3);
            base.Browser.Navigate<LoanServicingCBO>().InsolvenciesStatementofAccount();

            var legalentities = "Mr Neeran Besesar";
            var sequestrationDate = DateTime.Now;
            var supervisor = "SupervisorUser";

            base.Browser.Page<Correspondence_StatementOfAccount>().Populate(legalentities, sequestrationDate, supervisor);

            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            CorrespondenceAssertions.AssertImageIndex(this.loanaccountkey.ToString(), CorrespondenceReports.InsolvenciesStatementofAccount, CorrespondenceMedium.Email, loanaccountkey, 0);
        }

        [Test]
        public void Account_WhenSendingInsolvenciesStatementofAccount_ShouldHaveValidControls()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(3);
            base.Browser.Navigate<LoanServicingCBO>().InsolvenciesStatementofAccount();
            base.View.AssertControlsValid();
            base.Browser.Page<Correspondence_StatementOfAccount>().AssertControlsValid();
        }

        [Test]
        public void Account_WhenSendingInsolvenciesStatementofAccount_ShouldHaveValidationMessages()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(3);
            base.Browser.Navigate<LoanServicingCBO>().InsolvenciesStatementofAccount();
            base.View.SendCorrespondence();
            base.View.AssertAllValidationMessagesExist();
            base.Browser.Page<Correspondence_StatementOfAccount>().AssertValidationMessagesExist();
        }

        #endregion Sending Insolvencies Statement of loanaccountkey

        #region Sending Insolvencies Claim Affidavit

        [Test]
        public void Account_WhenSendingInsolvenciesClaimAffidavit_ShouldWriteToImageIndex()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(3);
            base.Browser.Navigate<LoanServicingCBO>().InsolvenciesClaimAffidavit();

            var legalentities = "Mr Neeran Besesar";
            var idnumber = "5903065081085";
            var supervisor = "SuperVisorUser";
            var sequestration = "Sequestration";
            var sequestrationDate = DateTime.Now;
            base.Browser.Page<Correspondence_ClaimAffidavit>().Populate(legalentities, idnumber, supervisor, sequestration, sequestrationDate);
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            CorrespondenceAssertions.AssertImageIndex(this.loanaccountkey.ToString(), CorrespondenceReports.InsolvenciesClaimAffidavit, CorrespondenceMedium.Email, loanaccountkey, 0);
        }

        [Test]
        public void Account_WhenSendingInsolvenciesClaimAffidavit_ShouldHaveValidControls()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(3);
            base.Browser.Navigate<LoanServicingCBO>().InsolvenciesClaimAffidavit();
            base.View.AssertControlsValid();
            base.Browser.Page<Correspondence_ClaimAffidavit>().AssertControlsValid();
        }

        [Test]
        public void Account_WhenSendingInsolvenciesClaimAffidavit_ShouldHaveValidationMessages()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(3);
            base.Browser.Navigate<LoanServicingCBO>().InsolvenciesClaimAffidavit();
            base.View.SendCorrespondence();
            base.Browser.Page<Correspondence_ClaimAffidavit>().AssertValidationMessagesExist();
        }

        #endregion Sending Insolvencies Claim Affidavit

        #region Sending Loan Statement

        [Test]
        public void Loan_WhenSendingLoanStatement_ShouldWriteToImageIndex()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(0);
            base.Browser.Navigate<LoanServicingCBO>().LoanStatement();
            base.Browser.Page<Correspondence_LoanStatement>().Populate(DateTime.Parse("01-01-2011"), DateTime.Parse("01-31-2011"), "All", "Print", "English");
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            CorrespondenceAssertions.AssertImageIndex(this.loanaccountkey.ToString(), CorrespondenceReports.LoanStatement, CorrespondenceMedium.Email, loanaccountkey, 0);
        }

        [Test]
        public void Loan_WhenSendingLoanStatement_ShouldShowLoanStatementControls()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(0);
            base.Browser.Navigate<LoanServicingCBO>().LoanStatement();
            base.View.SendCorrespondence();
            base.View.AssertControlsValid();
            base.Browser.Page<Correspondence_LoanStatement>().AssertControlsValid();
        }

        [Test]
        public void Loan_WhenSendingLoanStatement_ShouldHaveValidationMessages()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(0);
            base.Browser.Navigate<LoanServicingCBO>().LoanStatement();
            base.View.SendCorrespondence();
            base.View.AssertAllValidationMessagesExist();
            base.Browser.Page<Correspondence_LoanStatement>().AssertValidationMessagesExist();
        }

        #endregion Sending Loan Statement

        #region Sending CapLetter

        [Test]
        public void Loan_WhenSendingCapLetter_ShouldWriteToImageIndex()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(0);
            base.Browser.Navigate<LoanServicingCBO>().CapLetter();
            base.Browser.Page<Correspondence_CapLetter>().Populate(this.loanaccountkey);
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            CorrespondenceAssertions.AssertImageIndex(this.loanaccountkey.ToString(), CorrespondenceReports.CAPLetter, CorrespondenceMedium.Email, loanaccountkey, 0);
        }

        [Test]
        public void Loan_WhenSendingCapLetter_ShouldHaveValidControls()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(0);
            base.Browser.Navigate<LoanServicingCBO>().CapLetter();
            base.Browser.Page<Correspondence_CapLetter>().AssertControlsValid();
        }

        [Test]
        public void Loan_WhenSendingCapLetter_ShouldHaveValidationMessages()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(0);
            base.Browser.Navigate<LoanServicingCBO>().CapLetter();
            base.View.SendCorrespondence();
            base.View.AssertAllValidationMessagesExist();
        }

        #endregion Sending CapLetter

        #region Sending Legal Instruction Foreclosure

        [Test]
        public void Loan_WhenSendingLegalInstructionForeclosure_ShouldWriteToImageIndex()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(0);
            base.Browser.Navigate<LoanServicingCBO>().LegalInstructionForeclosure();
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            CorrespondenceAssertions.AssertImageIndex(this.loanaccountkey.ToString(), CorrespondenceReports.LegalInstructionForeclosure, CorrespondenceMedium.Email, loanaccountkey, 0);
        }

        [Test]
        public void Loan_WhenSendingLegalInstructionForeclosure_ShouldHaveValidControls()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(0);
            base.Browser.Navigate<LoanServicingCBO>().LegalInstructionForeclosure();
            base.View.AssertControlsValid();
        }

        [Test]
        public void Loan_WhenSendingLegalInstructionForeclosure_ShouldHaveValidationMessages()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(0);
            base.Browser.Navigate<LoanServicingCBO>().LegalInstructionForeclosure();
            base.View.SendCorrespondence();
            base.View.AssertAllValidationMessagesExist();
        }

        #endregion Sending Legal Instruction Foreclosure

        #region Sending Legal Instruction Letter of Demand

        [Test]
        public void Loan_WhenSendingLegalInstructionLetterofDemand_ShouldWriteToImageIndex()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(0);
            base.Browser.Navigate<LoanServicingCBO>().LegalInstructionLetterofDemand();
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            CorrespondenceAssertions.AssertImageIndex(this.loanaccountkey.ToString(), CorrespondenceReports.LegalInstructionForeclosure, CorrespondenceMedium.Email, loanaccountkey, 0);
        }

        [Test]
        public void Loan_WhenSendingLegalInstructionLetterofDemand_ShouldHaveValidControls()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(0);
            base.Browser.Navigate<LoanServicingCBO>().LegalInstructionLetterofDemand();
            base.View.AssertControlsValid();
        }

        [Test]
        public void Loan_SendingLegalInstructionLetterofDemand_ShouldHaveValidationMessages()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(0);
            base.Browser.Navigate<LoanServicingCBO>().LegalInstructionLetterofDemand();
            base.View.SendCorrespondence();
            base.View.AssertAllValidationMessagesExist();
        }

        #endregion Sending Legal Instruction Letter of Demand

        #region Sending NCA Levied Charges Form27

        [Test]
        public void Loan_WhenSendingNCALeviedChargesForm27_ShouldWriteToImageIndex()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(0);
            base.Browser.Navigate<LoanServicingCBO>().NCALeviedChargesForm27();
            base.Browser.Page<Correspondence_Form27>().Populate(@"SAHL\HAUser", "Print");
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            CorrespondenceAssertions.AssertImageIndex(this.loanaccountkey.ToString(), CorrespondenceReports.Form27, CorrespondenceMedium.Email, loanaccountkey, 0);
        }

        [Test]
        public void Loan_WhenSendingNCALeviedChargesForm27_ShouldHaveValidControls()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(0);
            base.Browser.Navigate<LoanServicingCBO>().NCALeviedChargesForm27();
            base.View.SendCorrespondence();
            base.View.AssertControlsValid();
            base.Browser.Page<Correspondence_Form27>().AssertControlsValid();
        }

        [Test]
        public void Loan_WhenSendingNCALeviedChargesForm27_ShouldHaveValidationMessages()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(0);
            base.Browser.Navigate<LoanServicingCBO>().NCALeviedChargesForm27();
            base.View.SendCorrespondence();
            base.View.AssertAllValidationMessagesExist();
            base.Browser.Page<Correspondence_Form27>().AssertValidationMessageExists();
        }

        #endregion Sending NCA Levied Charges Form27

        #region Sending MS65Form

        [Test]
        public void Loan_WhenSendingMS65Form_ShouldWriteToImageIndex()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(0);
            base.Browser.Navigate<LoanServicingCBO>().MS65Form();
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            CorrespondenceAssertions.AssertImageIndex(this.loanaccountkey.ToString(), CorrespondenceReports.MS65, CorrespondenceMedium.Email, loanaccountkey, 0);
        }

        [Test]
        public void Loan_WhenSendingMS65Form_ShouldHaveValidControls()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(0);
            base.Browser.Navigate<LoanServicingCBO>().MS65Form();
            base.View.AssertControlsValid();
        }

        [Test]
        public void Loan_WhenSendingMS65Form_ShouldHaveValidationMessages()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(0);
            base.Browser.Navigate<LoanServicingCBO>().MS65Form();
            base.View.SendCorrespondence();
            base.View.AssertAllValidationMessagesExist();
        }

        #endregion Sending MS65Form

        #region Sending BondCancelled Letter

        [Test]
        public void Loan_WhenSendingBondCancelledLetter_ShouldWriteToImageIndex()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(0);
            base.Browser.Navigate<LoanServicingCBO>().BondCancelledLetter();
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            CorrespondenceAssertions.AssertImageIndex(this.loanaccountkey.ToString(), CorrespondenceReports.BondCancellationLetter, CorrespondenceMedium.Email, loanaccountkey, 0);
        }

        [Test]
        public void Loan_WhenSendingBondCancelledLetter_ShouldHaveValidControls()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(0);
            base.Browser.Navigate<LoanServicingCBO>().BondCancelledLetter();
            base.View.AssertControlsValid();
        }

        [Test]
        public void Loan_WhenSendingBondCancelledLetter_ShouldHaveValidationMessages()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(0);
            base.Browser.Navigate<LoanServicingCBO>().BondCancelledLetter();
            base.View.SendCorrespondence();
            base.View.AssertAllValidationMessagesExist();
        }

        #endregion Sending BondCancelled Letter

        #region Sending HOC Endorsement And Schedule

        [Test]
        public void HOC_WhenSendingHOCEndorsementAndSchedule_ShouldWriteToImageIndex()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(1);
            base.Browser.Navigate<LoanServicingCBO>().HOCEndorsementAndSchedule();
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            CorrespondenceAssertions.AssertImageIndex(this.hocaccountkey.ToString(), CorrespondenceReports.HOCEndorsement, CorrespondenceMedium.Email, loanaccountkey, 0);
        }

        [Test]
        public void HOC_WhenSendingHOCEndorsementAndSchedule_ShouldHaveValidControls()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(1);
            base.Browser.Navigate<LoanServicingCBO>().HOCEndorsementAndSchedule();
            base.View.AssertControlsValid();
        }

        [Test]
        public void HOC_WhenSendingHOCEndorsementAndSchedule_ShouldHaveValidationMessages()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(1);
            base.Browser.Navigate<LoanServicingCBO>().HOCEndorsementAndSchedule();
            base.View.SendCorrespondence();
            base.View.AssertAllValidationMessagesExist();
        }

        #endregion Sending HOC Endorsement And Schedule

        #region Sending HOC Cancellation Letter

        [Test]
        public void HOC_SendHOCCancellationLetter_ShouldWriteToImageIndex()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(1);
            base.Browser.Navigate<LoanServicingCBO>().HOCCancellationLetter();
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            CorrespondenceAssertions.AssertImageIndex(this.hocaccountkey.ToString(), CorrespondenceReports.HOCCancellationLetter, CorrespondenceMedium.Email, loanaccountkey, 0);
        }

        [Test]
        public void HOC_SendHOCCancellationLetter_ShouldHaveValidControls()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(1);
            base.Browser.Navigate<LoanServicingCBO>().HOCCancellationLetter();
            base.View.AssertControlsValid();
        }

        [Test]
        public void HOC_SendHOCCancellationLetter_ShouldHaveValidationMessages()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(1);
            base.Browser.Navigate<LoanServicingCBO>().HOCCancellationLetter();
            base.View.SendCorrespondence();
            base.View.AssertAllValidationMessagesExist();
        }

        #endregion Sending HOC Cancellation Letter

        #region Sending HOC Cover Letter And Policy Schedule

        [Test]
        public void HOC_WhenSendingHOCCoverLetterAndPolicySchedule_ShouldWriteToImageIndex()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(1);
            base.Browser.Navigate<LoanServicingCBO>().HOCCoverLetterAndPolicySchedule();
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            CorrespondenceAssertions.AssertImageIndex(this.hocaccountkey.ToString(), CorrespondenceReports.HOCCoverLetter, CorrespondenceMedium.Email, loanaccountkey, 0);
        }

        [Test]
        public void HOC_WhenSendingHOCCoverLetterAndPolicySchedule_ShouldHaveValidControls()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(1);
            base.Browser.Navigate<LoanServicingCBO>().HOCCoverLetterAndPolicySchedule();
            base.View.AssertControlsValid();
        }

        [Test]
        public void HOC_WhenSendingHOCCoverLetterAndPolicySchedule_ShouldHaveValidationMessages()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(1);
            base.Browser.Navigate<LoanServicingCBO>().HOCCoverLetterAndPolicySchedule();
            base.View.SendCorrespondence();
            base.View.AssertAllValidationMessagesExist();
        }

        #endregion Sending HOC Cover Letter And Policy Schedule

        #region Life Correspondence Send Tests

        /// <summary>
        /// Test is FAILING due to button level security for HaloUser!
        /// </summary>
        [Test]
        public void Life_WhenSendingEndorsementLetter_ShouldWriteToImageIndex()
        {
            //NOte: UPDATE TEST! its wrong
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(2);
            base.Browser.Navigate<LoanServicingCBO>().SendEndorsementLetter();
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            CorrespondenceAssertions.AssertImageIndex(this.lifeaccountkey.ToString(), CorrespondenceReports.EndorsementLetter, CorrespondenceMedium.Email, loanaccountkey, 0);
        }

        [Test]
        public void Life_WhenSendingEndorsementLetter_ShouldHaveValidControls()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(2);
            base.Browser.Navigate<LoanServicingCBO>().SendEndorsementLetter();
            base.View.AssertControlsValid();
        }

        [Test]
        public void Life_WhenSendingEndorsementLetter_ShouldHaveValidationMessages()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(2);
            base.Browser.Navigate<LoanServicingCBO>().SendEndorsementLetter();
            base.View.SendCorrespondence();
            base.View.AssertAllValidationMessagesExist();
        }

        #endregion Life Correspondence Send Tests

        #region Sending HOC Form 23

        [Test]
        public void HOC_WhenSendingForm23_ShouldWriteToImageIndex()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(1);
            base.Browser.Navigate<LoanServicingCBO>().SendForm23Letter();
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            //against the loan account key, not sure why it is different from the other HOC correspondence.
            CorrespondenceAssertions.AssertImageIndex(this.hocaccountkey.ToString(), CorrespondenceReports.HOCForm23Letter, CorrespondenceMedium.Email, loanaccountkey, hocaccountkey);
        }

        #endregion Sending HOC Form 23
    }
}