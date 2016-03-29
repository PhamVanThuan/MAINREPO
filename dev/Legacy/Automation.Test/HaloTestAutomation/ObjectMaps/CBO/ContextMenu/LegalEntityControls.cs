using ObjectMaps.NavigationControls;
using System.Linq;
using WatiN.Core;

namespace ObjectMaps.CBO.LoanServicing
{
    public abstract class LoanServicingControls : BaseNavigation
    {
        protected Link LegalEntityParentNode(string LegalEntityName)
        {
            //There are cases where the link title attribute is generated leaving more white space in the legal name than what the getlegalname function returns
            // this is only in the title, the name (text) that displays on the screen matches what the function returns.
            return base.Document.Links.Filter(Find.ByText(LegalEntityName))[0];
        }

        [FindBy(Title = "Legal Entity Details")]
        protected Link LegalEntityDetails { get; set; }

        [FindBy(Title = "Update Legal Entity Details")]
        protected Link UpdateLegalEntityDetails { get; set; }

        [FindBy(Title = "Products")]
        protected Link Products { get; set; }

        protected Link ParentLoanAccount(int AccountKey)
        {
            return base.Document.Links.Filter(Find.ByText(AccountKey.ToString()))[0];
        }

        protected Link LoanAccount(int AccountKey)
        {
            return base.Document.Links.Filter(Find.ByText(AccountKey.ToString()))[1];
        }

        protected Link LifeAccount(int AccountKey)
        {
            return base.Document.Links.Filter(Find.ByText(AccountKey.ToString()))[0];
        }

        protected Link VariableLoan(int accountKey)
        {
            var links = base.Document.Links.Filter(Find.ByText("Variable Loan"));
            return (from l in links where l.GetAttributeValue("href").Contains(accountKey.ToString()) select l).FirstOrDefault();
        }

        protected ImageCollection imageCollectionTreeImage
        {
            get
            {
                return base.Document.Images.Filter(
                    Find.By("onmouseover", "src='../../Images/delete.png'"));
            }
        }

        [FindBy(Text = "Capture Client Survey")]
        protected Link CaptureClientSurvey { get; set; }

        #region LoanAccountNode

        [FindBy(Title = "CATS Disbursement")]
        protected Link CATSDisbursement { get; set; }

        [FindBy(Title = "Manage Disbursements")]
        protected Link ManageDisbursements { get; set; }

        [FindBy(Title = "Create Personal Loan Lead")]
        protected Link CreatePersonalLoanLead { get; set; }

        [FindBy(Title = "Personal Loan")]
        protected Link PersonalLoan { get; set; }

        [FindBy(Title = "Capitalisation Letter")]
        protected Link CapitalisationLetter { get; set; }

        [FindBy(Title = "Delete Disbursements")]
        protected Link DeleteDisbursements { get; set; }

        [FindBy(Title = "Rollback Disbursements")]
        protected Link RollbackDisbursements { get; set; }

        [FindBy(Title = "Reports")]
        protected Link Reports { get; set; }

        [FindBy(Title = "Calculators")]
        protected Link Calculators { get; set; }

        [FindBy(Title = "Loan Summary")]
        protected Link LoanSummary { get; set; }

        [FindBy(Title = "Loan Transactions")]
        protected Link LoanTransactions { get; set; }

        [FindBy(Title = "Post Transactions")]
        protected Link PostTransactions { get; set; }

        [FindBy(Title = "Rollback Transactions")]
        protected Link RollbackTransactions { get; set; }

        [FindBy(Title = "Arrear Transactions")]
        protected Link ArrearTransactions { get; set; }

        [FindBy(Title = "Post Arrear Transactions")]
        protected Link PostArrearTransactions { get; set; }

        [FindBy(Title = "Create Disability Claim")]
        protected Link CreateDisabilityClaim { get; set; }

        [FindBy(Title = "Rollback Arrear Transactions")]
        protected Link RollbackArrearTransactions { get; set; }

        [FindBy(Title = "Property Details")]
        protected Link PropertyDetails { get; set; }

        [FindBy(Title = "Update Property")]
        protected Link UpdateProperty { get; set; }

        [FindBy(Title = "Update Deeds Office Details")]
        protected Link UpdateDeedsOfficeDetails { get; set; }

        [FindBy(Title = "Update Property Address")]
        protected Link UpdatePropertyAddress { get; set; }

        [FindBy(Title = "Valuation Details")]
        protected Link ValuationDetails { get; set; }

        [FindBy(Title = "Add Valuation")]
        protected Link AddValuation { get; set; }

        [FindBy(Title = "Update Valuation")]
        protected Link UpdateValuation { get; set; }

        [FindBy(Title = "Loan Adjustments")]
        protected Link LoanAdjustments { get; set; }

        [FindBy(Title = "Change Instalment")]
        protected Link ChangeInstalment { get; set; }

        [FindBy(Title = "Change Rate")]
        protected Link ChangeRate { get; set; }

        [FindBy(Title = "Change Term")]
        protected Link ChangeTerm { get; set; }

        [FindBy(Title = "Convert Staff Loan")]
        protected Link ConvertStaffLoan { get; set; }

        [FindBy(Title = "Mark Non-Performing")]
        protected Link MarkNonPerforming { get; set; }

        [FindBy(Title = "Loan Calculator")]
        protected Link LoanCalculator { get; set; }

        [FindBy(Title = "Further Lending Calculator")]
        protected Link FurtherLendingCalculator { get; set; }

        [FindBy(Title = "Update Financial Adjustments")]
        protected Link UpdateFinancialAdjustments { get; set; }

        [FindBy(Title = "Group Exposure")]
        protected Link GroupExposure { get; set; }

        [FindBy(Title = "Applications Summary")]
        protected Link ApplicationsSummary { get; set; }

        [FindBy(Title = "Loan Detail Summary")]
        protected Link LoanDetailSummary { get; set; }

        [FindBy(Title = "Add Loan Detail")]
        protected Link AddLoanDetail { get; set; }

        [FindBy(Title = "Update Loan Detail")]
        protected Link UpdateLoanDetail { get; set; }

        [FindBy(Title = "Delete Loan Detail")]
        protected Link DeleteLoanDetail { get; set; }

        [FindBy(Title = "Accepted Cap History")]
        protected Link AcceptedCapHistory { get; set; }

        [FindBy(Title = "Cancel Cap")]
        protected Link CancelCap { get; set; }

        [FindBy(Title = "ITC Summary")]
        protected Link ITCSummary { get; set; }

        [FindBy(Title = "Perform ITC")]
        protected Link PerformITC { get; set; }

        [FindBy(Title = "Suretor")]
        protected Link Suretor { get; set; }

        [FindBy(Title = "Add Suretor")]
        protected Link AddSuretor { get; set; }

        [FindBy(Title = "Remove Suretor")]
        protected Link RemoveSuretor { get; set; }

        [FindBy(Title = "Business Event History")]
        protected Link BusinessEventHistory { get; set; }

        #endregion LoanAccountNode

        #region ParentLoanNode

        [FindBy(Title = "Parent Account Summary")]
        protected Link ParentAccountSummary { get; set; }

        [FindBy(Title = "Account Memo")]
        protected Link AccountMemo { get; set; }

        [FindBy(Title = "Add Account Memo")]
        protected Link AddAccountMemo { get; set; }

        [FindBy(Title = "Update Account Memo")]
        protected Link UpdateAccountMemo { get; set; }

        [FindBy(Title = "Fixed Debit Order Summary")]
        protected Link FixedDebitOrderSummary { get; set; }

        [FindBy(Title = "Update Fixed Debit Order")]
        protected Link UpdateFixedDebitOrder { get; set; }

        [FindBy(Title = "Delete Fixed Debit Order")]
        protected Link DeleteFixedDebitOrder { get; set; }

        [FindBy(Title = "Disbursement History")]
        protected Link DisbursementHistory { get; set; }

        [FindBy(Title = "Account Mailing Address")]
        protected Link AccountMailingAddress { get; set; }

        [FindBy(Title = "Update Account Mailing Address")]
        protected Link UpdateAccountMailingAddress { get; set; }

        [FindBy(Title = "Santam Policy Detail")]
        protected Link SantamPolicyDetail { get; set; }

        #endregion ParentLoanNode

        #region VariableLoanNode

        [FindBy(Title = "Financial Service Summary")]
        protected Link FinancialServiceSummary { get; set; }

        [FindBy(Title = "Debit Order Details")]
        protected Link DebitOrderDetails { get; set; }

        [FindBy(Title = "Add Debit Order")]
        protected Link AddDebitOrder { get; set; }

        [FindBy(Title = "Update Debit Order")]
        protected Link UpdateDebitOrder { get; set; }

        [FindBy(Title = "Delete Debit Order")]
        protected Link DeleteDebitOrder { get; set; }

        [FindBy(Title = "Bond and Loan Agreement")]
        protected Link BondandLoanAgreement { get; set; }

        [FindBy(Title = "Update Bond")]
        protected Link UpdateBond { get; set; }

        [FindBy(Title = "Add Loan Agreement")]
        protected Link AddLoanAgreement { get; set; }

        [FindBy(Title = "Manual Debit Orders")]
        protected Link ManualDebitOrders { get; set; }

        [FindBy(Title = "Add Manual Debit Order")]
        protected Link AddManualDebitOrder { get; set; }

        [FindBy(Title = "Update Manual Debit Order")]
        protected Link UpdateManualDebitOrder { get; set; }

        [FindBy(Title = "Delete Manual Debit Order")]
        protected Link DeleteManualDebitOrder { get; set; }

        #endregion VariableLoanNode

        #region LifeAccount

        [FindBy(Text = "Cancel Policy")]
        protected Link CancelPolicy { get; set; }

        #endregion LifeAccount

        #region Address

        [FindBy(Text = "Address Details")]
        protected Link AddressDetails { get; set; }

        [FindBy(Text = "Add Address Details")]
        protected Link AddAddressDetails { get; set; }

        [FindBy(Text = "Update Address Details")]
        protected Link UpdateAddressDetails { get; set; }

        [FindBy(Text = "Delete Address Details")]
        protected Link DeleteAddressDetails { get; set; }

        #endregion Address

        [FindBy(Text = "Enable Update LegalEntity")]
        protected Link EnableUpdateLegalEntity { get; set; }

        [FindBy(Text = "Legal Entity Memo")]
        protected Link LegalEntityMemo { get; set; }

        [FindBy(Text = "Add Legal Entity Memo")]
        protected Link AddLegalEntityMemo { get; set; }

        [FindBy(Text = "Update Legal Entity Memo")]
        protected Link UpdateLegalEntityMemo { get; set; }

        [FindBy(Title = "Assets and Liabilities")]
        protected Link AssetsAndLiabilities { get; set; }

        [FindBy(Title = "Add Asset/Liability")]
        protected Link AddAssetsAndLiabilities { get; set; }

        [FindBy(Title = "Update Asset/Liability")]
        protected Link UpdateAssetsAndLiabilities { get; set; }

        [FindBy(Title = "Delete Asset/Liability")]
        protected Link DeleteAssetsAndLiabilities { get; set; }

        protected Link HomeOwnersCover(int HOCAccountKey)
        {
            var links = base.Document.Links.Filter(Find.ByText("Home Owners Cover(HOC)"));
            return (from l in links where l.GetAttributeValue("href").Contains(HOCAccountKey.ToString()) select l).FirstOrDefault();
        }

        [FindBy(Title = "Update HOC Details")]
        protected Link UpdateHOCDetails { get; set; }

        protected LinkCollection CorrespondenceNodes
        {
            get
            {
                return base.Document.Links.Filter(Find.ByTitle("Correspondence"));
            }
        }

        [FindBy(Title = "Loan Statement")]
        protected Link LoanStatement { get; set; }

        [FindBy(Title = "Legal Instruction - Foreclosure")]
        protected Link LegalInstructionForeclosure { get; set; }

        [FindBy(Title = "Legal Instruction - Letter of Demand")]
        protected Link LegalInstructionLetterofDemand { get; set; }

        [FindBy(Title = "NCA Levied Charges (Form 27)")]
        protected Link NCALeviedChargesForm27 { get; set; }

        [FindBy(Title = "MS65 Form")]
        protected Link MS65Form { get; set; }

        [FindBy(Title = "Cap Letter")]
        protected Link CapLetter { get; set; }

        [FindBy(Title = "SuperLo Introduction Letter")]
        protected Link SuperLoIntroductionLetter { get; set; }

        [FindBy(Title = "Bond Cancelled Letter")]
        protected Link BondCancelledLetter { get; set; }

        [FindBy(Title = "Send Endorsement Letter")]
        protected Link SendEndorsementLetter { get; set; }

        [FindBy(Title = "HOC Cancellation Letter")]
        protected Link HOCCancellationLetter { get; set; }

        [FindBy(Title = "HOC Endorsement & Schedule")]
        protected Link HOCEndorsementAndSchedule { get; set; }

        [FindBy(Title = "HOC Cover Letter & Policy Schedule")]
        protected Link HOCCoverLetterAndPolicySchedule { get; set; }

        [FindBy(Title = "Instalment Letter")]
        protected Link InstalmentLetter { get; set; }

        [FindBy(Title = "Acknowledge Cancellation Instruction")]
        protected Link AcknowledgeCancellationInstruction { get; set; }

        [FindBy(Title = "Income Tax Statement")]
        protected Link IncomeTaxStatement { get; set; }

        [FindBy(Title = "Z299")]
        protected Link Z299 { get; set; }

        [FindBy(Title = "Insolvencies - Power of Attorney")]
        protected Link InsolvenciesPowerofAttorney { get; set; }

        [FindBy(Title = "Insolvencies - Statement of Account")]
        protected Link InsolvenciesStatementofAccount { get; set; }

        [FindBy(Title = "Insolvencies - Claim Affidavit")]
        protected Link InsolvenciesClaimAffidavit { get; set; }

        [FindBy(Text = "Loss Control")]
        protected Link LossControl { get; set; }

        [FindBy(TextRegex = "Debt Counselling")]
        protected Link DebtCounselling { get; set; }

        [FindBy(TextRegex = "Pre Debt Counselling Account Details")]
        protected Link PreDebtCounsellingAccountDetails { get; set; }

        protected Link GetLink(string node)
        {
            return base.Document.Link(Find.ByText(node));
        }

        [FindBy(TextRegex = "Request Physical Valuation")]
        protected Link RequestPhysicalValuation { get; set; }

        [FindBy(Title = "HOC Form 23 Letter")]
        protected Link HOCForm23Letter { get; set; }

        [FindBy(Title = "Life Policy Claim")]
        protected Link LifePolicyClaim { get; set; }

        [FindBy(Title = "Life Policy Claim Add")]
        protected Link LifePolicyClaimAdd { get; set; }

        [FindBy(Title = "Life Policy Claim Update")]
        protected Link LifePolicyClaimUpdate { get; set; }

        [FindBy(Title = "External Life")]
        protected Link ExternalLife { get; set; }
    }
}