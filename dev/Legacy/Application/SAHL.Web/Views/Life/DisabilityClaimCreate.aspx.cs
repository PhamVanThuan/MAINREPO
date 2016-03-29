using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Views.Life.Interfaces;
using System;
using System.Web.UI.WebControls;

namespace SAHL.Web.Views.Life
{
    public partial class DisabilityClaimCreate : SAHLCommonBaseView, IDisabilityClaimCreate
    {
        private int policyNumber;
        private DateTime? dateOfAcceptance;
        private int selectedLegalEntityKey;

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
        }

        public event EventHandler OnSubmitButtonClicked;

        public event EventHandler OnCancelButtonClicked;

        public void BindAssuredLivesGrid(IReadOnlyEventList<ILegalEntity> assuredLives)
        {
            LegalEntityGrid.HeaderCaption = "Assured Lives";
            LegalEntityGrid.GridHeight = 100;
            LegalEntityGrid.PostBackType = SAHL.Common.Web.UI.Controls.GridPostBackType.NoneWithClientSelect;
            LegalEntityGrid.ColumnIDPassportVisible = true;
            LegalEntityGrid.ColumnIDPassportHeadingType = SAHL.Web.Controls.LegalEntityGrid.GridColumnIDPassportHeadingType.IDAndPassportNumber;
            LegalEntityGrid.ColumnInsurableInterestVisible = true;
            LegalEntityGrid.ColumnBenefitsVisible = true;
            LegalEntityGrid.ColumnLifeStatusVisible = true;

            // Set the Data related properties
            LegalEntityGrid.AccountKey = policyNumber; 
            if (dateOfAcceptance.HasValue)
                LegalEntityGrid.DateOfAcceptance = dateOfAcceptance.Value;

            // Bind the grid
            LegalEntityGrid.BindLegalEntities(assuredLives);
        }

        public void BindClaimants(IReadOnlyEventList<ILegalEntity> claimants)
        {
            foreach (ILegalEntityNaturalPerson claimant in claimants)
            {
                ddlClaimant.Items.Add(new ListItem(claimant.FirstNames + " " + claimant.Surname, claimant.Key.ToString()));
            }

            ddlClaimant.DataBind();
        }

        public int SelectedLegalEntityKey
        {
            get { return selectedLegalEntityKey; }
            set { selectedLegalEntityKey = value; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Get the selected claimant
            if (ddlClaimant.SelectedItem.Value != "-select-")
                selectedLegalEntityKey = Convert.ToInt32(ddlClaimant.SelectedItem.Value);

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


        public int PolicyNumber
        {
            get { return policyNumber; }
            set 
            {
                policyNumber = value;
                lblPolicyNumber.Text = policyNumber.ToString(); 
            }
        }

        public string PolicyType
        {
            set { lblPolicyType.Text = value;  }
        }


        public string PolicyStatus
        {
            set { lblPolicyStatus.Text = value; }
        }

        public DateTime? DateOfAcceptance
        {
            get { return dateOfAcceptance;  }
            set 
            {
                dateOfAcceptance = value;
                lblDateOfAcceptance.Text = dateOfAcceptance.HasValue ? dateOfAcceptance.Value.ToString(SAHL.Common.Constants.DateFormat) : "-"; 
            }
        }

        public DateTime? CommencementDate
        {
            set { lblDateOfCommencement.Text = value.HasValue ? value.Value.ToString(SAHL.Common.Constants.DateFormat) : "-"; }
        }

        public double CurrentSumAssured
        {
            set { lblCurrentSumAssured.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat); }
        }

        public double ReassuredIPBAmount
        {
            set { lblReassuredIPBAmount.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat); }
        }

        public int LoanNumber
        {
            set { lblLoanNumber.Text = value.ToString(); }
        }

        public string LoanStatus
        {
            set { lblLoanStatus.Text = value; }
        }

        public int LoanTerm
        {
            set { lblLoanTerm.Text = value.ToString() + " months" ; }
        }

        public double LoanAmount
        {
            set { lblLoanAmount.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat); }
        }

        public int DebitOrderDay
        {
            set { lblLoanDebitOrderDay.Text = value.ToString(); }
        }

        public double OutstandingBondAmount
        {
            set { lblOutstandingBondAmount.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat); }
        }

        public double BondInstalment
        {
            set { lblBondInstalment.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat); }
        }
    }
}