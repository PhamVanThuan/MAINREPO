using DevExpress.Web.ASPxGridView;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace SAHL.Web.Views.DebtCounselling
{
    /// <summary>
    ///
    /// </summary>
    public partial class DebtCounsellingSummary : SAHLCommonBaseView, IDebtCounsellingSummary
    {
        private bool _displayEworkCaseDetails;

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!ShouldRunPage)
                return;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!ShouldRunPage)
                return;
            apEWorkDetails.Visible = _displayEworkCaseDetails;
            lblEWorkWarningMessage.Visible = _displayEworkCaseDetails;
        }

        protected void gvDCDetailTypes_RowDataBound(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            int budget = Convert.ToInt32(e.GetValue("Amount"));
            if (budget < 100000)
                e.Row.ForeColor = System.Drawing.Color.Red;
        }

        #region IApplicationSummary Members

        /// <summary>
        /// Set whether to display details from the case in the eWork Loss Control map
        /// </summary>
        public bool DisplayEworkCaseDetails
        {
            set
            {
                _displayEworkCaseDetails = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        ///
        public void BindDetailGrid(IEventList<IDetail> listDetail)
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Clear();
            dt.Columns.Add("DetailDate");
            dt.Columns.Add("DetailType");

            if (listDetail != null && listDetail.Count > 0)
            {
                foreach (IDetail det in listDetail)
                {
                    dr = dt.NewRow();
                    dr["DetailDate"] = det.DetailDate;
                    dr["DetailType"] = det.DetailType.Description;
                    dt.Rows.Add(dr);
                }
            }

            gvDCDetailTypes.Columns.Clear();
            gvDCDetailTypes.AddGridColumn("DetailDate", "Date", 10, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);
            gvDCDetailTypes.AddGridColumn("DetailType", "Description", 10, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);

            gvDCDetailTypes.DataSource = dt;
            gvDCDetailTypes.DataBind();
        }

        public void BindControls(IDebtCounselling debtCounselling)
        {
            if (debtCounselling == null)
                return;

            IDebtCounsellingRepository dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

            if (debtCounselling.DebtCounsellor != null)
            {
                if (debtCounselling.DebtCounsellor.LegalEntityType.Key == (int)SAHL.Common.Globals.LegalEntityTypes.NaturalPerson)
                {
                    lblDCContact.Text = debtCounselling.DebtCounsellor.DisplayName;

                    if (debtCounselling.DebtCounsellorCompany != null)
                        lblDCCompany.Text = debtCounselling.DebtCounsellorCompany.DisplayName;
                }
                else
                {
                    lblDCCompany.Text = debtCounselling.DebtCounsellor.DisplayName;
                }

                if (debtCounselling.DebtCounsellor.DebtCounsellorDetail != null)
                    lblNCRNumber.Text = debtCounselling.DebtCounsellor.DebtCounsellorDetail.NCRDCRegistrationNumber;
            }

            if (debtCounselling.PaymentDistributionAgent != null)
                lblPDA.Text = debtCounselling.PaymentDistributionAgent.DisplayName;

            IAccount acc = debtCounselling.Account;
            IAccountInstallmentSummary ais = acc.InstallmentSummary;

            lblLifeSAHL.Text = "N/A";
            lblHOCPremium.Text = "N/A";

            lblAccountKey.Text = acc.Key.ToString();

            BindDetailGrid(acc.Details);

            foreach (IAccount ca in acc.RelatedChildAccounts)
            {
                IAccountLifePolicy aLP = ca as IAccountLifePolicy;
                if (aLP != null && aLP.AccountStatus.Key == (int)GeneralStatuses.Active)
                {
                    lblLifeSAHL.Text = (aLP.LifePolicy.MonthlyPremium).ToString(SAHL.Common.Constants.CurrencyFormat);
                }

                IAccountCreditProtectionPlan aCPP = ca as IAccountCreditProtectionPlan;
                if (aCPP != null && aCPP.AccountStatus.Key == (int)GeneralStatuses.Active)
                {
                    lblLifeSAHL.Text = (aCPP.FinancialServices.Where(x => x.FinancialServiceType.Key == (int)FinancialServiceTypes.SAHLCreditProtectionPlan)
                        .FirstOrDefault()
                        .Payment.ToString(SAHL.Common.Constants.CurrencyFormat));
                }

                IAccountHOC aH = ca as IAccountHOC;
                if (aH != null && aH.AccountStatus.Key == (int)GeneralStatuses.Active)
                {
                    lblHOCPremium.Text = (aH.HOC.HOCMonthlyPremium ?? 0).ToString(SAHL.Common.Constants.CurrencyFormat);
                }
            }

            double cBalance = ais.CurrentBalance;

            //find the relevant FS
            IFinancialService fs = acc.FinancialServices.Where(x => x.AccountStatus.Key == (int)AccountStatuses.Open
                                    && (x.FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan
                                        || x.FinancialServiceType.Key == (int)FinancialServiceTypes.PersonalLoan)).FirstOrDefault();

            //if there are no fs's then find the variable one 
            //this will happen when the account is closed.
            if (fs == null)
            {
                fs = acc.FinancialServices.Where(x => x.FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan
                                    && (x.FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan
                                        || x.FinancialServiceType.Key == (int)FinancialServiceTypes.PersonalLoan)).FirstOrDefault();
            }

            lblDODay.Text = fs.FinancialServiceBankAccounts[0].DebitOrderDay.ToString();

            lblLTV.Text = "N/A";
            lblValuation.Text = "N/A";
            lblValuationDate.Text = "N/A";

            lblRemainingTerm.Text = fs.Balance.LoanBalance.RemainingInstalments.ToString() + " months";
            lblRemainingTerm.Visible = true;

            #region MortgageLoan

            IMortgageLoan ml = fs as IMortgageLoan;

            if (ml != null)
            {
                double val = ml.GetActiveValuationAmount();
                double ltv = cBalance / val;

                lblLTV.Text = (ltv).ToString(SAHL.Common.Constants.RateFormat);

                lblValuation.Text = ml.GetActiveValuationAmount().ToString(SAHL.Common.Constants.CurrencyFormat);
                lblValuationDate.Text = ml.GetActiveValuationDate().Value.ToString(SAHL.Common.Constants.DateFormat);

                IMortgageLoanAccount mortgageLoanAccount = debtCounselling.Account as IMortgageLoanAccount;
                if (mortgageLoanAccount != null)
                {
                    IMortgageLoan mortgageLoan = mortgageLoanAccount.SecuredMortgageLoan;
                    if (mortgageLoan != null)
                    {
                        //IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
                        //IControl ctrl = ctrlRepo.GetControlByDescription("MaxProposalTerm");

                        //get latest snapshotAccount prior to the proposal acceptance date
                        //use the remaininginstallments there. If the new one is > than that, make red
                        int previousTerm = dcRepo.GetRemainingTermPriorToProposalAcceptance(debtCounselling.Key);

                        int RemainingTerm = mortgageLoan.RemainingInstallments;

                        if (previousTerm > -1 && RemainingTerm > previousTerm)
                        {
                            lblRemainingTermHighlight.Text = RemainingTerm.ToString() + " months";
                            lblRemainingTermHighlight.Visible = true;
                            lblRemainingTerm.Visible = false;
                        }
                        else
                        {
                            lblRemainingTerm.Text = RemainingTerm.ToString() + " months";
                            lblRemainingTerm.Visible = true;
                        }
                    }
                }
            }

            #endregion MortgageLoan

            lblCurrentBalance.Text = cBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblInstalmentAmount.Text = ais.TotalLoanInstallment.ToString(SAHL.Common.Constants.CurrencyFormat);

            // To do: Check why we have a zero value for personal loans arrears
            lblArrearBalance.Text = ais.TotalArrearsBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblAdminFee.Text = ais.MonthlyServiceFee.ToString(SAHL.Common.Constants.CurrencyFormat);

            lblMonthsInArrear.Text = ais.MonthsInArrears.ToString();
            lblLifeRegent.Text = ais.TotalRegentPremium.ToString(SAHL.Common.Constants.CurrencyFormat);

            lblSPV.Text = acc.SPV.Description;

            lblAdminFee.Text = ais.MonthlyServiceFee.ToString(SAHL.Common.Constants.CurrencyFormat);

            lblFixedDOAmount.Text = acc.FixedPayment.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblAccountOpenDate.Text = acc.OpenDate.Value.ToString(SAHL.Common.Constants.DateFormat);

            lblTotalInstalment.Text = (ais.TotalAmountDue).ToString(SAHL.Common.Constants.CurrencyFormat);

            if (debtCounselling.AcceptedActiveProposal != null)
            {
                apAcceptedProposal.Visible = true;

                //lblReviewDate.Text = debtCounselling.AcceptedProposal.ReviewDate.HasValue ? debtCounselling.AcceptedProposal.ReviewDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
                lblAcceptedDate.Text = debtCounselling.AcceptedActiveProposal.AcceptedDate.HasValue ? debtCounselling.AcceptedActiveProposal.AcceptedDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
                lblAcceptedProposalRate.Text = debtCounselling.AcceptedActiveProposal.AcceptedRate.HasValue ? debtCounselling.AcceptedActiveProposal.AcceptedRate.Value.ToString(SAHL.Common.Constants.RateFormat) : "-";
                lblAcceptedReason.Text = debtCounselling.AcceptedActiveProposal.AcceptedReason == null ? "-" : debtCounselling.AcceptedActiveProposal.AcceptedReason.ReasonDefinition.ReasonDescription.Description;
                lblAcceptedUser.Text = debtCounselling.AcceptedActiveProposal.AcceptedUser == null ? "-" : debtCounselling.AcceptedActiveProposal.AcceptedUser.ADUserName;
            }
            else
            {
                apAcceptedProposal.Visible = false;
            }

            IADUser consultant = dcRepo.GetActiveDebtCounsellingUser(debtCounselling.Key, WorkflowRoleTypes.DebtCounsellingConsultantD);
            IADUser supervisor = dcRepo.GetActiveDebtCounsellingUser(debtCounselling.Key, WorkflowRoleTypes.DebtCounsellingSupervisorD);
            IADUser courtConsultant = dcRepo.GetActiveDebtCounsellingUser(debtCounselling.Key, WorkflowRoleTypes.DebtCounsellingCourtConsultantD);

            lblConsultant.Text = consultant == null ? "-" : consultant.ADUserName;
            lblSupervisor.Text = supervisor == null ? "-" : supervisor.ADUserName;
            lblCourtConsultant.Text = courtConsultant == null ? "-" : courtConsultant.ADUserName;

            lblPaymentAmount.Text = debtCounselling.PaymentReceivedAmount.HasValue ? debtCounselling.PaymentReceivedAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : "-";
            lblPaymentDueDate.Text = debtCounselling.PaymentReceivedDate.HasValue ? debtCounselling.PaymentReceivedDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";

            lblReferenceNumber.Text = string.IsNullOrEmpty(debtCounselling.ReferenceNumber) ? "-" : debtCounselling.ReferenceNumber;

            lblAttorney.Text = debtCounselling.LitigationAttorney != null ? debtCounselling.LitigationAttorney.LegalEntity.DisplayName : "-";
            IHearingDetail hearingDetail = (from hd in debtCounselling.HearingDetails orderby hd.Key descending select hd).FirstOrDefault();
            lblCourtDate.Text = (hearingDetail != null) ? hearingDetail.HearingDate.ToString(SAHL.Common.Constants.DateFormat) : "-";
        }

        public void BindEworkCaseDetails(string stageName, IADUser assignedUser)
        {
            lblEworkStage.Text = String.IsNullOrEmpty(stageName) ? "None case exists" : stageName;
            lblEworkUser.Text = assignedUser != null ? assignedUser.ADUserName : "";
        }

        #endregion IApplicationSummary Members
    }
}