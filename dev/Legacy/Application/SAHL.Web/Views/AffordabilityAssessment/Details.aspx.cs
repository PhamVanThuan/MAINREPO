using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Web.Views.AffordabilityAssessment.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAHL.Web.Views.AffordabilityAssessment
{
    /// <summary>
    ///
    /// </summary>
    public partial class Details : SAHLCommonBaseView, IDetails
    {
        private AffordabilityAssessmentMode affordabilityAssessmentMode;
        private bool applicationInCreditWorkflow;
        private IList<string> invalidCommentsList;
        private string numericFormat;
        private bool sahlBondValueRetrieved;
        private bool sahlHocValueRetrieved;
        private bool furtherLendingUser;

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.ShouldRunPage)
                return;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.ShouldRunPage)
                return;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!this.ShouldRunPage)
                return;

            SetFieldAccessability(tblFields);

            // hide or show the stress factor percentage dropdown list depending on the 'mode' we are in
            bool showStressFactorDropdown = false;
            if (affordabilityAssessmentMode == AffordabilityAssessmentMode.Update_Credit)
                showStressFactorDropdown = true;

            ddlStressFactorPercentage.Visible = showStressFactorDropdown;
            lblStressFactorPercentage.Visible = !showStressFactorDropdown;

            // set the screen mode in a hidden field so we can use on client side
            hidScreenMode.Value = affordabilityAssessmentMode.ToString();
        }

        /// <summary>
        /// Recursive method to loop thru all the textboxes in the table and set the accessability depending on the screen mode
        /// </summary>
        /// <param name="parent"></param>
        private void SetFieldAccessability(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c.GetType() == typeof(SAHLTextBox))
                {
                    TextBox txtField = (TextBox)c;
                    bool clientInputField = txtField.ID.EndsWith("_Client") || txtField.ID.EndsWith("_Monthly") ? true : false;
                    bool creditInputField = txtField.ID.EndsWith("_Credit") || txtField.ID.EndsWith("_Consolidate") || txtField.ID.EndsWith("_AsIs") ? true : false;
                    bool totalField = txtField.ID.EndsWith("_Total") || txtField.ID.EndsWith("_ToBe") ? true : false;
                    bool debtToConsolidateField = txtField.ID.EndsWith("_Consolidate") ? true : false;

                    // set all the total fields to readonly
                    if (totalField)
                    {
                        txtField.ReadOnly = true;
                    }

                    switch (affordabilityAssessmentMode)
                    {
                        case AffordabilityAssessmentMode.Display:
                            // if we are in display mode, make all fields readonly
                            txtField.ReadOnly = true;
                            break;

                        case AffordabilityAssessmentMode.Update_NonCredit:
                            //  if we are in update mode & a non-credit user & the case is not in the credit workflow then enable the client input fields
                            if (clientInputField)
                            {
                                // if we have retreived a sahl bond or hoc value then make the relevant field readonly
                                if ((txtField.ID.EndsWith("SAHLBond_Client") && sahlBondValueRetrieved )|| (txtField.ID.EndsWith("HOC_Client") && sahlHocValueRetrieved)) 
                                    txtField.ReadOnly = true;
                                else
                                    txtField.BackColor = System.Drawing.Color.White;
                            }
                            else
                            {
                                // if we are a Further Lending user then we must enable the "Debt to Consolidate" fields
                                if (furtherLendingUser && debtToConsolidateField)
                                {
                                    txtField.BackColor = System.Drawing.Color.White;
                                }
                                else
                                {
                                    txtField.ReadOnly = true;
                                    txtField.TabIndex = 0; // we dont want to tab to credit fields
                                }
                            }
                            break;

                        case AffordabilityAssessmentMode.Update_Credit:
                            //  if we are in update mode & a credit user then enable the credit input fields
                            if (creditInputField)
                            {
                                // if we have retreived a sahl bond or hoc value then make the relevant field readonly
                                if ((txtField.ID.EndsWith("SAHLBond_Credit") && sahlBondValueRetrieved) || (txtField.ID.EndsWith("HOC_Credit") && sahlHocValueRetrieved)) 
                                    txtField.ReadOnly = true;
                                else
                                    txtField.BackColor = System.Drawing.Color.White;
                            }
                            else
                            {
                                txtField.ReadOnly = true;
                                txtField.TabIndex = 0; // we dont want to tab to client fields
                            }
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    SetFieldAccessability(c);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // loop thru all images and check if any comments need to be eneterd - addto list if they do

            invalidCommentsList = new List<string>();

            OnSubmitButtonClicked(sender, e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        #region IDetails Members

        public AffordabilityAssessmentMode AffordabilityAssessmentMode
        {
            get { return affordabilityAssessmentMode; }
            set { affordabilityAssessmentMode = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public bool ButtonRowVisible
        {
            set
            {
                btnSubmit.Visible = value;
                btnCancel.Visible = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool ApplicationInCreditWorkflow
        {
            set { applicationInCreditWorkflow = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;

        public int AssessmentStressFactorKey
        {
            get
            {
                return Convert.ToInt32(String.IsNullOrWhiteSpace(ddlStressFactorPercentage.SelectedValue) ? hidStressFactorKey.Value : ddlStressFactorPercentage.SelectedValue);
            }
        }

        public int MinimumMonthlyFixedExpenses
        {
            get
            {
                int value;
                return int.TryParse(this.txtMinMonthlyFixedExpenses.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out value) ? value : 0;
            }
        }

        public int? BasicGrossSalary_Drawings_Client
        {
            get { return GetFieldValueFromScreen(txtBasicGrossSalary_Drawings_Client.Text); }
        }

        public int? BasicGrossSalary_Drawings_Credit
        {
            get { return GetFieldValueFromScreen(txtBasicGrossSalary_Drawings_Credit.Text); }
        }

        public int? Commission_Overtime_Client
        {
            get { return GetFieldValueFromScreen(txtCommission_Overtime_Client.Text); }
        }

        public int? Commission_Overtime_Credit
        {
            get { return GetFieldValueFromScreen(txtCommission_Overtime_Credit.Text); }
        }

        public int? Net_Rental_Client
        {
            get { return GetFieldValueFromScreen(txtNet_Rental_Client.Text); }
        }

        public int? Net_Rental_Credit
        {
            get { return GetFieldValueFromScreen(txtNet_Rental_Credit.Text); }
        }

        public int? Investments_Client
        {
            get { return GetFieldValueFromScreen(txtInvestments_Client.Text); }
        }

        public int? Investments_Credit
        {
            get { return GetFieldValueFromScreen(txtInvestments_Credit.Text); }
        }

        public int? OtherIncome1_Client
        {
            get { return GetFieldValueFromScreen(txtOtherIncome1_Client.Text); }
        }

        public int? OtherIncome1_Credit
        {
            get { return GetFieldValueFromScreen(txtOtherIncome1_Credit.Text); }
        }

        public int? OtherIncome2_Client
        {
            get { return GetFieldValueFromScreen(txtOtherIncome2_Client.Text); }
        }

        public int? OtherIncome2_Credit
        {
            get { return GetFieldValueFromScreen(txtOtherIncome2_Credit.Text); }
        }

        public int? GrossMonthlyIncome_Client
        {
            get { return GetFieldValueFromScreen(txtGrossIncome_Client_Total.Text); }
        }

        public int? PayrollDeductions_Client
        {
            get { return GetFieldValueFromScreen(txtPayrollDeductions_Client.Text); }
        }

        public int? PayrollDeductions_Credit
        {
            get { return GetFieldValueFromScreen(txtPayrollDeductions_Credit.Text); }
        }

        public int? Accomodation_Client
        {
            get { return GetFieldValueFromScreen(txtAccomodation_Client.Text); }
        }

        public int? Accomodation_Credit
        {
            get { return GetFieldValueFromScreen(txtAccomodation_Credit.Text); }
        }

        public int? Transport_Client
        {
            get { return GetFieldValueFromScreen(txtTransport_Client.Text); }
        }

        public int? Transport_Credit
        {
            get { return GetFieldValueFromScreen(txtTransport_Credit.Text); }
        }

        public int? Food_Client
        {
            get { return GetFieldValueFromScreen(txtFood_Client.Text); }
        }

        public int? Food_Credit
        {
            get { return GetFieldValueFromScreen(txtFood_Credit.Text); }
        }

        public int? Education_Client
        {
            get { return GetFieldValueFromScreen(txtEducation_Client.Text); }
        }

        public int? Education_Credit
        {
            get { return GetFieldValueFromScreen(txtEducation_Credit.Text); }
        }

        public int? Medical_Client
        {
            get { return GetFieldValueFromScreen(txtMedical_Client.Text); }
        }

        public int? Medical_Credit
        {
            get { return GetFieldValueFromScreen(txtMedical_Credit.Text); }
        }

        public int? Utilities_Client
        {
            get { return GetFieldValueFromScreen(txtUtilities_Client.Text); }
        }

        public int? Utilities_Credit
        {
            get { return GetFieldValueFromScreen(txtUtilities_Credit.Text); }
        }

        public int? ChildSupport_Client
        {
            get { return GetFieldValueFromScreen(txtChildSupport_Client.Text); }
        }

        public int? ChildSupport_Credit
        {
            get { return GetFieldValueFromScreen(txtChildSupport_Credit.Text); }
        }

        public int? OtherBonds_Client
        {
            get { return GetFieldValueFromScreen(txtOtherBonds_Client.Text); }
        }

        public int? OtherBonds_Credit
        {
            get { return GetFieldValueFromScreen(txtOtherBonds_Credit.Text); }
        }

        public int? OtherBonds_Consolidate
        {
            get { return GetFieldValueFromScreen(txtOtherBonds_Consolidate.Text); }
        }

        public int? Vehicle_Client
        {
            get { return GetFieldValueFromScreen(txtVehicle_Client.Text); }
        }

        public int? Vehicle_Credit
        {
            get { return GetFieldValueFromScreen(txtVehicle_Credit.Text); }
        }

        public int? Vehicle_Consolidate
        {
            get { return GetFieldValueFromScreen(txtVehicle_Consolidate.Text); }
        }

        public int? CreditCards_Client
        {
            get { return GetFieldValueFromScreen(txtCreditCards_Client.Text); }
        }

        public int? CreditCards_Credit
        {
            get { return GetFieldValueFromScreen(txtCreditCards_Credit.Text); }
        }

        public int? CreditCards_Consolidate
        {
            get { return GetFieldValueFromScreen(txtCreditCards_Consolidate.Text); }
        }

        public int? PersonalLoans_Client
        {
            get { return GetFieldValueFromScreen(txtPersonalLoans_Client.Text); }
        }

        public int? PersonalLoans_Credit
        {
            get { return GetFieldValueFromScreen(txtPersonalLoans_Credit.Text); }
        }

        public int? PersonalLoans_Consolidate
        {
            get { return GetFieldValueFromScreen(txtPersonalLoans_Consolidate.Text); }
        }

        public int? RetailAccounts_Client
        {
            get { return GetFieldValueFromScreen(txtRetailAccounts_Client.Text); }
        }

        public int? RetailAccounts_Credit
        {
            get { return GetFieldValueFromScreen(txtRetailAccounts_Credit.Text); }
        }

        public int? RetailAccounts_Consolidate
        {
            get { return GetFieldValueFromScreen(txtRetailAccounts_Consolidate.Text); }
        }

        public int? OtherDebtExpenses_Client
        {
            get { return GetFieldValueFromScreen(txtOtherDebtExpenses_Client.Text); }
        }

        public int? OtherDebtExpenses_Credit
        {
            get { return GetFieldValueFromScreen(txtOtherDebtExpenses_Credit.Text); }
        }

        public int? OtherDebtExpenses_Consolidate
        {
            get { return GetFieldValueFromScreen(txtOtherDebtExpenses_Consolidate.Text); }
        }

        public int? SAHLBond_Client
        {
            get { return GetFieldValueFromScreen(txtSAHLBond_Client.Text); }
            set { this.txtSAHLBond_Client.Text = value.ToString(); }
        }

        public int? SAHLBond_Credit
        {
            get { return GetFieldValueFromScreen(txtSAHLBond_Credit.Text); }
            set { this.txtSAHLBond_Credit.Text = value.ToString(); }
        }

        public int? SAHLBond_ToBe
        {
            set { this.txtSAHLBond_ToBe.Text = value.ToString(); }
        }

        public int? HOC_Client
        {
            get { return GetFieldValueFromScreen(txtHOC_Client.Text); }
            set { this.txtHOC_Client.Text = value.ToString(); }
        }

        public int? HOC_Credit
        {
            get { return GetFieldValueFromScreen(txtHOC_Credit.Text); }
            set { this.txtHOC_Credit.Text = value.ToString(); }
        }

        public int? HOC_ToBe
        {
            set { this.txtHOC_ToBe.Text = value.ToString(); }
        }

        public int? DomesticSalary_Client
        {
            get { return GetFieldValueFromScreen(txtDomesticSalary_Client.Text); }
        }

        public int? DomesticSalary_Credit
        {
            get { return GetFieldValueFromScreen(txtDomesticSalary_Credit.Text); }
        }

        public int? InsurancePolicies_Client
        {
            get { return GetFieldValueFromScreen(txtInsurancePolicies_Client.Text); }
        }

        public int? InsurancePolicies_Credit
        {
            get { return GetFieldValueFromScreen(txtInsurancePolicies_Credit.Text); }
        }

        public int? CommittedSavings_Client
        {
            get { return GetFieldValueFromScreen(txtCommittedSavings_Client.Text); }
        }

        public int? CommittedSavings_Credit
        {
            get { return GetFieldValueFromScreen(txtCommittedSavings_Credit.Text); }
        }

        public int? Security_Client
        {
            get { return GetFieldValueFromScreen(txtSecurity_Client.Text); }
        }

        public int? Security_Credit
        {
            get { return GetFieldValueFromScreen(txtSecurity_Credit.Text); }
        }

        public int? TelephoneTV_Client
        {
            get { return GetFieldValueFromScreen(txtTelephoneTV_Client.Text); }
        }

        public int? TelephoneTV_Credit
        {
            get { return GetFieldValueFromScreen(txtTelephoneTV_Credit.Text); }
        }

        public int? Other_Client
        {
            get { return GetFieldValueFromScreen(txtOther_Client.Text); }
        }

        public int? Other_Credit
        {
            get { return GetFieldValueFromScreen(txtOther_Credit.Text); }
        }

        public string BasicGrossSalary_Drawings_Comments
        {
            get { return GetCommentsFromHiddenField(hidBasicGrossSalary_Drawings_Comments); }
        }

        public string Commission_Overtime_Comments
        {
            get { return GetCommentsFromHiddenField(hidCommission_Overtime_Comments); }
        }

        public string Net_Rental_Comments
        {
            get { return GetCommentsFromHiddenField(hidNet_Rental_Comments); }
        }

        public string Investments_Comments
        {
            get { return GetCommentsFromHiddenField(hidInvestments_Comments); }
        }

        public string OtherIncome1_Comments
        {
            get { return GetCommentsFromHiddenField(hidOtherIncome1_Comments); }
        }

        public string OtherIncome2_Comments
        {
            get { return GetCommentsFromHiddenField(hidOtherIncome2_Comments); }
        }

        public string PayrollDeductions_Comments
        {
            get { return GetCommentsFromHiddenField(hidPayrollDeductions_Comments); }
        }

        public string Accomodation_Comments
        {
            get { return GetCommentsFromHiddenField(hidAccomodation_Comments); }
        }

        public string Transport_Comments
        {
            get { return GetCommentsFromHiddenField(hidTransport_Comments); }
        }

        public string Food_Comments
        {
            get { return GetCommentsFromHiddenField(hidFood_Comments); }
        }

        public string Education_Comments
        {
            get { return GetCommentsFromHiddenField(hidEducation_Comments); }
        }

        public string Medical_Comments
        {
            get { return GetCommentsFromHiddenField(hidMedical_Comments); }
        }

        public string Utilities_Comments
        {
            get { return GetCommentsFromHiddenField(hidUtilities_Comments); }
        }

        public string ChildSupport_Comments
        {
            get { return GetCommentsFromHiddenField(hidChildSupport_Comments); }
        }

        public string OtherBonds_Comments
        {
            get { return GetCommentsFromHiddenField(hidOtherBonds_Comments); }
        }

        public string Vehicle_Comments
        {
            get { return GetCommentsFromHiddenField(hidVehicle_Comments); }
        }

        public string CreditCards_Comments
        {
            get { return GetCommentsFromHiddenField(hidCreditCards_Comments); }
        }

        public string PersonalLoans_Comments
        {
            get { return GetCommentsFromHiddenField(hidPersonalLoans_Comments); }
        }

        public string RetailAccounts_Comments
        {
            get { return GetCommentsFromHiddenField(hidRetailAccounts_Comments); }
        }

        public string OtherDebtExpenses_Comments
        {
            get { return GetCommentsFromHiddenField(hidOtherDebtExpenses_Comments); }
        }

        public string SAHLBond_Comments
        {
            get { return GetCommentsFromHiddenField(hidSAHLBond_Comments); }
        }

        public string HOC_Comments
        {
            get { return GetCommentsFromHiddenField(hidHOC_Comments); }
        }

        public string DomesticSalary_Comments
        {
            get { return GetCommentsFromHiddenField(hidDomesticSalary_Comments); }
        }

        public string InsurancePolicies_Comments
        {
            get { return GetCommentsFromHiddenField(hidInsurancePolicies_Comments); }
        }

        public string CommittedSavings_Comments
        {
            get { return GetCommentsFromHiddenField(hidCommittedSavings_Comments); }
        }

        public string Security_Comments
        {
            get { return GetCommentsFromHiddenField(hidSecurity_Comments); }
        }

        public string TelephoneTV_Comments
        {
            get { return GetCommentsFromHiddenField(hidTelephoneTV_Comments); }
        }

        public string Other_Comments
        {
            get { return GetCommentsFromHiddenField(hidOther_Comments); }
        }

        private AffordabilityAssessmentModel affordabilityAssessment;

        public AffordabilityAssessmentModel AffordabilityAssessment
        {
            get
            {
                return affordabilityAssessment;
            }
            set
            {
                affordabilityAssessment = value;
            }
        }

        public IList<string> InvalidCommentsList
        {
            get
            {
                return invalidCommentsList;
            }
        }

        public bool CommentsValid
        {
            get
            {
                return Convert.ToBoolean(hidAllCommentsEntered.Value);
            }
        }


        public bool SAHLBondValueRetrieved
        {
            get
            {
               return sahlBondValueRetrieved;
            }
            set
            {
                sahlBondValueRetrieved = value;
            }
        }

        public bool SAHLHocValueRetrieved
        {
            get
            {
                return sahlHocValueRetrieved;
            }
            set
            {
                sahlHocValueRetrieved = value;
            }
        }

        public bool FurtherLendingUser
        {
            get
            {
                return furtherLendingUser;
            }
            set
            {
                furtherLendingUser = value;
            }
        }


        /// <summary>
        ///
        /// </summary>
        public void BindAssessmentStressFactors(IEnumerable<AffordabilityAssessmentStressFactorModel> affordabilityAssessmentStressFactors, int assessmentStressFactorKey)
        {
            ddlStressFactorPercentage.Items.Clear();
            ddlStressFactorPercentageLookup.Items.Clear();
            foreach (var assessmentStressFactor in affordabilityAssessmentStressFactors)
            {
                ddlStressFactorPercentage.Items.Add(new ListItem(assessmentStressFactor.StressFactorPercentage, assessmentStressFactor.Key.ToString()));
                // we also need to bind the hidden lookup dropdownlist which will have the percantage and the increase percentage which we will use as a lookup
                ddlStressFactorPercentageLookup.Items.Add(new ListItem(assessmentStressFactor.PercentageIncreaseOnRepayments.ToString(), assessmentStressFactor.Key.ToString()));
            }

            // set the selected item to the value saved against the affordability assessment
            ddlStressFactorPercentage.SelectedValue = assessmentStressFactorKey.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        public void BindAffordabilityAssessmentDetail()
        {
            numericFormat = affordabilityAssessmentMode == AffordabilityAssessmentMode.Display ? SAHL.Common.Constants.CurrencyFormatNoSymbolNoCents : "";

            AffordabilityAssessmentDetailModel affordabilityAssessmentDetail = affordabilityAssessment.AffordabilityAssessmentDetail;

            // Income
            var income = affordabilityAssessmentDetail.Income;

            SetAssessmentItemDetailFields(income.BasicGrossSalary_Drawings, txtBasicGrossSalary_Drawings_Client, txtBasicGrossSalary_Drawings_Credit, hidBasicGrossSalary_Drawings_Comments, imgBasicGrossSalary_Drawings);
            SetAssessmentItemDetailFields(income.Commission_Overtime, txtCommission_Overtime_Client, txtCommission_Overtime_Credit, hidCommission_Overtime_Comments, imgCommission_Overtime);
            SetAssessmentItemDetailFields(income.Net_Rental, txtNet_Rental_Client, txtNet_Rental_Credit, hidNet_Rental_Comments, imgNet_Rental);
            SetAssessmentItemDetailFields(income.Investments, txtInvestments_Client, txtInvestments_Credit, hidInvestments_Comments, imgInvestments);
            SetAssessmentItemDetailFields(income.OtherIncome1, txtOtherIncome1_Client, txtOtherIncome1_Credit, hidOtherIncome1_Comments, imgOtherIncome1);
            SetAssessmentItemDetailFields(income.OtherIncome2, txtOtherIncome2_Client, txtOtherIncome2_Credit, hidOtherIncome2_Comments, imgOtherIncome2);

            var incomeDeductions = affordabilityAssessmentDetail.IncomeDeductions;
            SetAssessmentItemDetailFields(incomeDeductions.PayrollDeductions, txtPayrollDeductions_Client, txtPayrollDeductions_Credit, hidPayrollDeductions_Comments, imgPayrollDeductions);

            SetAssessmentItemTotalFields(txtGrossIncome_Client_Total, txtGrossIncome_Credit_Total, income.GrossIncome_Client, income.GrossIncome_Credit);
            SetAssessmentItemTotalFields(txtNetIncome_Client_Total, txtNetIncome_Credit_Total, affordabilityAssessmentDetail.NetIncome_Client, affordabilityAssessmentDetail.NetIncome_Credit);

            // Necessary Expenses
            var necessaryExpenses = affordabilityAssessmentDetail.NecessaryExpenses;
            SetAssessmentItemDetailFields(necessaryExpenses.AccommodationExpenses_Rental, txtAccomodation_Client, txtAccomodation_Credit, hidAccomodation_Comments, imgAccomodation);
            SetAssessmentItemDetailFields(necessaryExpenses.Transport, txtTransport_Client, txtTransport_Credit, hidTransport_Comments, imgTransport);
            SetAssessmentItemDetailFields(necessaryExpenses.Food, txtFood_Client, txtFood_Credit, hidFood_Comments, imgFood);
            SetAssessmentItemDetailFields(necessaryExpenses.Education, txtEducation_Client, txtEducation_Credit, hidEducation_Comments, imgEducation);
            SetAssessmentItemDetailFields(necessaryExpenses.Medical, txtMedical_Client, txtMedical_Credit, hidMedical_Comments, imgMedical);
            SetAssessmentItemDetailFields(necessaryExpenses.Utilities, txtUtilities_Client, txtUtilities_Credit, hidUtilities_Comments, imgUtilities);
            SetAssessmentItemDetailFields(necessaryExpenses.ChildSupport, txtChildSupport_Client, txtChildSupport_Credit, hidChildSupport_Comments, imgChildSupport);

            SetAssessmentItemTotalFields(txtMonthlyNecessaryExpenses_Client_Total, txtMonthlyNecessaryExpenses_Credit_Total, necessaryExpenses.MonthlyTotal_Client, necessaryExpenses.MonthlyTotal_Credit);

            // NCR Override
            txtAppliedNCROverride_Credit_Total.Text = affordabilityAssessmentDetail.AppliedNCROverride > 0 ? affordabilityAssessmentDetail.AppliedNCROverride.ToString(numericFormat) : "";

            // Allocable Income
            SetAssessmentItemTotalFields(txtAllocableIncome_Client_Total, txtAllocableIncome_Credit_Total, affordabilityAssessmentDetail.AllocableIncome_Client, affordabilityAssessmentDetail.AllocableIncome_Credit);

            // NCR Assessment Guidline
            txtMinMonthlyFixedExpenses.Text = affordabilityAssessmentDetail.MinimumMonthlyFixedExpenses > 0 ? affordabilityAssessmentDetail.MinimumMonthlyFixedExpenses.ToString(numericFormat) : "-";

            // Payment Obligations
            var paymentObligations = affordabilityAssessmentDetail.PaymentObligations;
            SetAssessmentConsolidatableItemDetailFields(paymentObligations.OtherBonds, txtOtherBonds_Client, txtOtherBonds_Credit, txtOtherBonds_Consolidate, txtOtherBonds_ToBe, hidOtherBonds_Comments, imgOtherBonds);
            SetAssessmentConsolidatableItemDetailFields(paymentObligations.Vehicle, txtVehicle_Client, txtVehicle_Credit, txtVehicle_Consolidate, txtVehicle_ToBe, hidVehicle_Comments, imgVehicle);
            SetAssessmentConsolidatableItemDetailFields(paymentObligations.CreditCards, txtCreditCards_Client, txtCreditCards_Credit, txtCreditCards_Consolidate, txtCreditCards_ToBe, hidCreditCards_Comments, imgCreditCards);
            SetAssessmentConsolidatableItemDetailFields(paymentObligations.PersonalLoans, txtPersonalLoans_Client, txtPersonalLoans_Credit, txtPersonalLoans_Consolidate, txtPersonalLoans_ToBe, hidPersonalLoans_Comments, imgPersonalLoans);
            SetAssessmentConsolidatableItemDetailFields(paymentObligations.RetailAccounts, txtRetailAccounts_Client, txtRetailAccounts_Credit, txtRetailAccounts_Consolidate, txtRetailAccounts_ToBe, hidRetailAccounts_Comments, imgRetailAccounts);
            SetAssessmentConsolidatableItemDetailFields(paymentObligations.OtherDebtExpenses, txtOtherDebtExpenses_Client, txtOtherDebtExpenses_Credit, txtOtherDebtExpenses_Consolidate, txtOtherDebtExpenses_ToBe, hidOtherDebtExpenses_Comments, imgOtherDebtExpenses);

            SetAssessmentItemTotalFields(txtPayment_Client_Total, txtPayment_Consolidate_Total, txtPayment_Credit_Total, txtPayment_ToBe_Total, paymentObligations.MonthlyTotal_Client, paymentObligations.MonthlyTotal_DebtToConsolidate, paymentObligations.MonthlyTotal_Credit, paymentObligations.MonthlyTotal_ToBe);

            // Discretionary Income
            SetAssessmentItemTotalFields(txtDiscretionaryIncome_Client_Total, txtDiscretionaryIncome_Credit_Total, txtDiscretionaryIncome_ToBe_Total, affordabilityAssessmentDetail.DiscretionaryIncome_Client, affordabilityAssessmentDetail.DiscretionaryIncome_Credit, affordabilityAssessmentDetail.DiscretionaryIncome_ToBe);

            // SAHL Payment Obligations
            var sahlPaymentObligations = affordabilityAssessmentDetail.SAHLPaymentObligations;
            SetAssessmentItemDetailFields(sahlPaymentObligations.SAHLBond, txtSAHLBond_Client, txtSAHLBond_Credit, txtSAHLBond_ToBe, hidSAHLBond_Comments, imgSAHLBond);
            SetAssessmentItemDetailFields(sahlPaymentObligations.HOC, txtHOC_Client, txtHOC_Credit, txtHOC_ToBe, hidHOC_Comments, imgHOC);

            // Other Expenses
            var otherExpenses = affordabilityAssessmentDetail.OtherExpenses;
            SetAssessmentItemDetailFields(otherExpenses.DomesticSalary, txtDomesticSalary_Client, txtDomesticSalary_Credit, txtDomesticSalary_ToBe, hidDomesticSalary_Comments, imgDomesticSalary);
            SetAssessmentItemDetailFields(otherExpenses.InsurancePolicies, txtInsurancePolicies_Client, txtInsurancePolicies_Credit, txtInsurancePolicies_ToBe, hidInsurancePolicies_Comments, imgInsurancePolicies);
            SetAssessmentItemDetailFields(otherExpenses.CommittedSavings, txtCommittedSavings_Client, txtCommittedSavings_Credit, txtCommittedSavings_ToBe, hidCommittedSavings_Comments, imgCommittedSavings);

            SetAssessmentItemDetailFields(otherExpenses.Security, txtSecurity_Client, txtSecurity_Credit, txtSecurity_ToBe, hidSecurity_Comments, imgSecurity);
            SetAssessmentItemDetailFields(otherExpenses.Telephone_TV, txtTelephoneTV_Client, txtTelephoneTV_Credit, txtTelephoneTV_ToBe, hidTelephoneTV_Comments, imgTelephoneTV);
            SetAssessmentItemDetailFields(otherExpenses.Other, txtOther_Client, txtOther_Credit, txtOther_ToBe, hidOther_Comments, imgOther);

            SetAssessmentItemTotalFields(txtOther_Client_Total, txtOther_Credit_Total, txtOther_ToBe_Total, otherExpenses.MonthlyTotal_Client, otherExpenses.MonthlyTotal_Credit, otherExpenses.MonthlyTotal_Credit);

            // Surplus/Deficit
            SetAssessmentItemTotalFields(txtSurplusDeficit_Client_Total, txtSurplusDeficit_Credit_Total, txtSurplusDeficit_ToBe_Total, affordabilityAssessmentDetail.Surplus_Deficit_Client, affordabilityAssessmentDetail.Surplus_Deficit_Credit, affordabilityAssessmentDetail.Surplus_Deficit_ToBe);

            // Stress Factor
            txtStressFactorValue.Text = affordabilityAssessmentDetail.StressFactorValue != 0 ? affordabilityAssessmentDetail.StressFactorValue.ToString(numericFormat) : "";
            txtAfterStressFactorApplied.Text = affordabilityAssessmentDetail.SurplusToBeAfterStressFactorApplied != 0 ? affordabilityAssessmentDetail.SurplusToBeAfterStressFactorApplied.ToString(numericFormat) : "";
            lblStressFactorPercentage.Text = affordabilityAssessmentDetail.StressFactorPercentageDisplay;
            hidStressFactorKey.Value = affordabilityAssessment.StressFactorKey.ToString();
            hidStressFactorPercentageIncrease.Value = affordabilityAssessmentDetail.StressFactorPercentageIncrease.ToString();

            // Contributors
            lblContributingApplicants.Text = "   " + Convert.ToString(affordabilityAssessment.NumberOfContributingApplicants);
            lblHouseholdDependants.Text = "   " + Convert.ToString(affordabilityAssessment.NumberOfHouseholdDependants);

            // Summary
            txtSummaryDebtToConsolidate.Text = affordabilityAssessmentDetail.DebtToConsolidate.HasValue && affordabilityAssessmentDetail.DebtToConsolidate.Value != 0 ? affordabilityAssessmentDetail.DebtToConsolidate.Value.ToString(numericFormat) : "";
            txtSummaryNetIncome.Text = affordabilityAssessmentDetail.NetIncome_Credit != 0 ? affordabilityAssessmentDetail.NetIncome_Credit.ToString(numericFormat) : "";
            txtSummaryTotalExpenses.Text = affordabilityAssessmentDetail.TotalExpenses != 0 ? affordabilityAssessmentDetail.TotalExpenses.ToString(numericFormat) : "";
            txtSummarySurpusDeficit.Text = affordabilityAssessmentDetail.Surplus_Deficit != 0 ? affordabilityAssessmentDetail.Surplus_Deficit.ToString(numericFormat) : "";
            txtSummarySurplusPercent.Text = affordabilityAssessmentDetail.SurplusToNetHouseholdIncomePercentage != 0 ? affordabilityAssessmentDetail.SurplusToNetHouseholdIncomePercentage.ToString() + "%" : "";
        }

        #endregion IDetails Members

        #region Helper Methods

        private void SetAssessmentItemDetailFields(AffordabilityAssessmentItemModel itemModel, SAHLTextBox txtClient, SAHLTextBox txtCredit, HiddenField hidComments, Image imgComments)
        {
            SetAssessmentItemDetailFields(itemModel, txtClient, txtCredit, null, hidComments, imgComments);
        }

        private void SetAssessmentItemDetailFields(AffordabilityAssessmentItemModel itemModel, SAHLTextBox txtClient, SAHLTextBox txtCredit, SAHLTextBox txtToBe, HiddenField hidComments, Image imgComments)
        {
            txtClient.Text = itemModel.ClientValue.HasValue && itemModel.ClientValue.Value > 0 ? itemModel.ClientValue.Value.ToString(numericFormat) : "";
            txtCredit.Text = itemModel.CreditValue.HasValue && itemModel.CreditValue.Value > 0 ? itemModel.CreditValue.Value.ToString(numericFormat) : "";
            if (txtToBe != null)
                txtToBe.Text = txtCredit.Text;

            hidComments.Value = itemModel.ItemNotes;
        }

        private void SetAssessmentConsolidatableItemDetailFields(AffordabilityAssessmentConsolidatableItemModel itemModel, SAHLTextBox txtClient, SAHLTextBox txtCredit, SAHLTextBox txtConsolidate, SAHLTextBox txtToBe, HiddenField hidComments, Image imgComments)
        {
            txtClient.Text = itemModel.ClientValue.HasValue && itemModel.ClientValue.Value > 0 ? itemModel.ClientValue.Value.ToString(numericFormat) : "";
            txtConsolidate.Text = itemModel.ConsolidationValue.HasValue && itemModel.ConsolidationValue.Value > 0 ? itemModel.ConsolidationValue.Value.ToString(numericFormat) : "";
            txtCredit.Text = itemModel.CreditValue.HasValue && itemModel.CreditValue.Value > 0 ? itemModel.CreditValue.Value.ToString(numericFormat) : "";
            txtToBe.Text = itemModel.ToBeValue > 0 ? itemModel.ToBeValue.ToString(numericFormat) : "";

            hidComments.Value = itemModel.ItemNotes;
        }

        private void SetAssessmentItemTotalFields(SAHLTextBox txtClient, SAHLTextBox txtCredit, int clientTotal, int creditTotal)
        {
            SetAssessmentItemTotalFields(txtClient, null, txtCredit, null, clientTotal, 0, creditTotal, 0);
        }

        private void SetAssessmentItemTotalFields(SAHLTextBox txtClient, SAHLTextBox txtCredit, SAHLTextBox txtToBe, int clientTotal, int creditTotal, int toBeTotal)
        {
            SetAssessmentItemTotalFields(txtClient, null, txtCredit, txtToBe, clientTotal, 0, creditTotal, toBeTotal);
        }

        private void SetAssessmentItemTotalFields(SAHLTextBox txtClient, SAHLTextBox txtConsolidate, SAHLTextBox txtCredit, SAHLTextBox txtToBe, int clientTotal, int consolidateTotal, int creditTotal, int toBeTotal)
        {
            txtClient.Text = clientTotal != 0 ? clientTotal.ToString(SAHL.Common.Constants.CurrencyFormatNoSymbolNoCents) : "";
            txtCredit.Text = creditTotal != 0 ? creditTotal.ToString(SAHL.Common.Constants.CurrencyFormatNoSymbolNoCents) : "";
            if (txtConsolidate != null)
                txtConsolidate.Text = consolidateTotal != 0 ? consolidateTotal.ToString(SAHL.Common.Constants.CurrencyFormatNoSymbolNoCents) : "";
            if (txtToBe != null)
                txtToBe.Text = toBeTotal != 0 ? toBeTotal.ToString(SAHL.Common.Constants.CurrencyFormatNoSymbolNoCents) : "";
        }

        private int? GetFieldValueFromScreen(string fieldText)
        {
            int value;
            return int.TryParse(fieldText, NumberStyles.Currency, CultureInfo.CurrentCulture, out value) ? (int?)value : null;
        }

        private string GetCommentsFromHiddenField(HiddenField hiddenField)
        {
            return String.IsNullOrWhiteSpace(hiddenField.Value) ? null : hiddenField.Value.Trim();
        }

        #endregion Helper Methods

    }
}