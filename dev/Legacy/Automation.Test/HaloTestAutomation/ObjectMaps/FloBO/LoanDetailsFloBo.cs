using ObjectMaps.NavigationControls;
using WatiN.Core;

namespace ObjectMaps.FLoBO
{
    public class LoanDetailsNodeControls : BaseNavigation
    {
        #region LoanDetails

        [FindBy(Title = "Loan Details")]
        protected Link LoanDetails { get; set; }

        protected Link LoanDetailsSummary
        {
            get
            {
                return base.Document.Link(Find.ByText("Loan Details Summary"));
            }
        }

        #endregion LoanDetails

        #region BondAndLoanAgreement

        protected Link BondLoanAgreement
        {
            get
            {
                return base.Document.Link(Find.ByText("Bond and Loan Agreement"));
            }
        }

        protected Link AddLoanAgreement
        {
            get
            {
                return base.Document.Link(Find.ByText("Add Loan Agreement"));
            }
        }

        #endregion BondAndLoanAgreement

        #region LoanConditions

        protected Link LoanConditions
        {
            get
            {
                return base.Document.Link(Find.ByText("Loan Conditions"));
            }
        }

        protected Link ManageLoanConditions
        {
            get
            {
                return base.Document.Link(Find.ByText("Manage Loan Conditions"));
            }
        }

        #endregion LoanConditions

        #region SettlementBankDetails

        protected Link SettlementBankDetails
        {
            get
            {
                return base.Document.Link(Find.ByText("Settlement Bank Details"));
            }
        }

        protected Link AddSettlementBankingDetails
        {
            get
            {
                return base.Document.Link(Find.ByText("Add Settlement Banking Details"));
            }
        }

        protected Link DeleteSettlementBankingDetails
        {
            get
            {
                return base.Document.Link(Find.ByText("Delete Settlement Banking Details"));
            }
        }

        #endregion SettlementBankDetails

        #region DebitOrderDetails

        protected Link DebitOrderDetails
        {
            get
            {
                return base.Document.Link(Find.ByText("Debit Order Details"));
            }
        }

        protected Link UpdateDebitOrderDetails
        {
            get
            {
                return base.Document.Link(Find.ByText("Update Debit Order Details"));
            }
        }

        #endregion DebitOrderDetails

        #region ApplicationMailingAddress

        protected Link ApplicationMailingAddress
        {
            get
            {
                return base.Document.Link(Find.ByText("Application Mailing Address"));
            }
        }

        protected Link UpdateApplicationMailingAddress
        {
            get
            {
                return base.Document.Link(Find.ByText("Update Application Mailing Address"));
            }
        }

        #endregion ApplicationMailingAddress

        #region LoanAttributes

        protected Link LoanAttributes
        {
            get
            {
                return base.Document.Link(Find.ByText("Loan Attributes"));
            }
        }

        protected Link UpdateApplicationLoanAttributes
        {
            get
            {
                return base.Document.Link(Find.ByText("Update Application Loan Attributes"));
            }
        }

        #endregion LoanAttributes

        #region CATSDisbursement

        protected Link CATSDisbursement
        {
            get
            {
                return base.Document.Link(Find.ByText("CATS Disbursement"));
            }
        }

        protected Link ManageDisbursements
        {
            get
            {
                return base.Document.Link(Find.ByText("Manage Disbursements"));
            }
        }

        protected Link DeleteDisbursements
        {
            get
            {
                return base.Document.Link(Find.ByText("Delete Disbursements"));
            }
        }

        protected Link RollbackDisbursements
        {
            get
            {
                return base.Document.Link(Find.ByText("Rollback Disbursements"));
            }
        }

        #endregion CATSDisbursement

        #region AccountDomiciliumAddress

        protected Link UpdateAccountDomiciliumAddress
        {
            get
            {
                return base.Document.Link(Find.ByText("Update Account Domicilium Address"));
            }
        }

        protected Link AccountDomiciliumAddress
        {
            get
            {
                return base.Document.Link(Find.ByText("Account Domicilium Address"));
            }
        }

        #endregion AccountDomiciliumAddress

        #region FutureDatedTransactions

        [FindBy(Title = "Manual Debit Orders")]
        protected Link ManualDebitOrders;

        [FindBy(Title = "Add Manual Debit Order")]
        protected Link AddManualDebitOrder;

        [FindBy(Title = "Update Manual Debit Order")]
        protected Link UpdateManualDebitOrder;

        [FindBy(Title = "Delete Manual Debit Order")]
        protected Link DeleteManualDebitOrder;

        #endregion FutureDatedTransactions
    }
}