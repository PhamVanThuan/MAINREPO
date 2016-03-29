using System;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common
{
    /// <summary>
    /// 
    /// </summary>
    public partial class BondLoanAgreement : SAHLCommonBaseView, IBondLoanAgreement
    {
        private IEventList<IBond> _bonds;
        private IEventList<ILoanAgreement> _loanAgreements;
        private bool _bondGridPostBack;
        //private Dictionary<int, string> _attorney;

        /// <summary>
        /// 
        /// </summary>
        enum BondGridColumns
        {
            DeedsOffice = 1,
            Attorney = 2,
            BondRegistrationDate = 3,
            BondRegistrationNumber = 4,
            BondRegistrationAmount = 5,
            BondLoanAgreementAmount = 6,
            DeedsOfficeKey = 7,
            AttorneyKey = 8
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            OnSubmitButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BondGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnBondGrid_SelectedIndexChanged(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BondGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                IBond bond = e.Row.DataItem as IBond;

                if (bond != null)
                {
                    e.Row.Cells[(int)BondGridColumns.DeedsOffice].Text = bond.DeedsOffice.Description;
                    e.Row.Cells[(int)BondGridColumns.Attorney].Text = bond.Attorney.LegalEntity.GetLegalName(LegalNameFormat.Full);

                    e.Row.Cells[(int)BondGridColumns.DeedsOfficeKey].Text = bond.DeedsOffice.Key.ToString();
                    e.Row.Cells[(int)BondGridColumns.AttorneyKey].Text = bond.Attorney.Key.ToString();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeedsOfficeUpdate_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnDeedsOfficeUpdate_SelectedIndexChanged(sender, e);
        }

        #region IBondLoanAgreement Members

        /// <summary>
        /// 
        /// </summary>
        public void BindBonds()
        {
            if (_bondGridPostBack)
                BondGrid.PostBackType = GridPostBackType.SingleClick;

            BondGrid.Columns.Clear();
            BondGrid.AddGridBoundColumn("Key", "BondKey", Unit.Percentage(0), HorizontalAlign.Left, false);
            BondGrid.AddGridBoundColumn("DeedsOffice", "Deeds Office", Unit.Percentage(10), HorizontalAlign.Left, true);
            BondGrid.AddGridBoundColumn("Attorney", "Attorney", Unit.Percentage(20), HorizontalAlign.Left, true);
            BondGrid.AddGridBoundColumn("BondRegistrationDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Registration Date", false, Unit.Percentage(8), HorizontalAlign.Center, true);
            BondGrid.AddGridBoundColumn("BondRegistrationNumber", "Registration Number", Unit.Percentage(10), HorizontalAlign.Left, true);
            BondGrid.AddGridBoundColumn("BondRegistrationAmount", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Registration Amount", false, Unit.Percentage(10), HorizontalAlign.Right, true);
            BondGrid.AddGridBoundColumn("BondLoanAgreementAmount", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Loan Agreement Amount", false, Unit.Percentage(12), HorizontalAlign.Right, true);
            BondGrid.AddGridBoundColumn("DeedsOffice", "Deeds Office Key", Unit.Percentage(10), HorizontalAlign.Left, false);
            BondGrid.AddGridBoundColumn("Attorney", "Attorney Key", Unit.Percentage(20), HorizontalAlign.Left, false);

            BondGrid.DataSource = _bonds;
            BondGrid.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        public void BindLoanAgreement()
        {
            LoanAgreeGrid.Columns.Clear();
            LoanAgreeGrid.AddGridBoundColumn("Key", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            LoanAgreeGrid.AddGridBoundColumn("AgreementDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Loan Agreement Date", false, Unit.Percentage(20), HorizontalAlign.Center, true);
            LoanAgreeGrid.AddGridBoundColumn("Amount", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Loan Agreement Amount", false, Unit.Percentage(25), HorizontalAlign.Right, true);
            //LoanAgreeGrid.AddGridBoundColumn("DocumentVersionKey", "Version", Unit.Percentage(15), HorizontalAlign.Center, true);
            LoanAgreeGrid.AddGridBoundColumn("UserName", "Captured By", Unit.Percentage(20), HorizontalAlign.Left, true);
            LoanAgreeGrid.AddGridBoundColumn("ChangeDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Date Changed", false, Unit.Percentage(20), HorizontalAlign.Center, true);
            //LoanAgreeGrid.AddGridBoundColumn("MortgageLoan", "", Unit.Percentage(0), HorizontalAlign.Left, false);

            LoanAgreeGrid.DataSource = _loanAgreements;
            LoanAgreeGrid.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DeedsOffice"></param>
        public void BindDeedsOffice(IEventList<IDeedsOffice> DeedsOffice)
        {
            DeedsOfficeUpdate.DataTextField = "Desciption";
            DeedsOfficeUpdate.DataValueField = "Key";
            DeedsOfficeUpdate.DataSource = DeedsOffice.BindableDictionary;
            DeedsOfficeUpdate.DataBind();

            if (BondGrid.Rows.Count > 0)
                DeedsOfficeUpdate.SelectedValue = BondGrid.SelectedRow.Cells[(int)BondGridColumns.DeedsOfficeKey].Text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Attorney"></param>
        public void BindAttorney(Dictionary<int, string> Attorney)
        {
            //_attorney = Attorney;

            AttorneyUpdate.DataTextField = "AttorneyName";
            AttorneyUpdate.DataValueField = "Key";
            AttorneyUpdate.DataSource = Attorney;
            AttorneyUpdate.DataBind();

            if (BondGrid.Rows.Count>0 && Attorney.ContainsKey(Convert.ToInt32(BondGrid.SelectedRow.Cells[(int)BondGridColumns.AttorneyKey].Text)))
                AttorneyUpdate.SelectedValue = BondGrid.SelectedRow.Cells[(int)BondGridColumns.AttorneyKey].Text;
        }

        /// <summary>
        /// Populate the controls used to update the Bond record
        /// </summary>
        public void PopulateUpdateBond(int BondIndex)
        {
            if (BondGrid.Rows.Count > 0)
            {
                BondRegNumberUpdate.Text = Bonds[BondIndex].BondRegistrationNumber;
                BondRegAmountUpdate.Text = Bonds[BondIndex].BondRegistrationAmount.ToString();
                BondRegDate.Text = Bonds[BondIndex].BondRegistrationDate.ToString();
                LoanAgreeAmount.Text = Bonds[BondIndex].BondLoanAgreementAmount.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnDeedsOfficeUpdate_SelectedIndexChanged;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnBondGrid_SelectedIndexChanged;

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public IEventList<ILoanAgreement> LoanAgreements
        {
            get { return _loanAgreements; }
            set { _loanAgreements = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public int BondGridIndex
        {
            set { BondGrid.SelectedIndex = value; }
            get { return BondGrid.SelectedIndex; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEventList<IBond> Bonds
        {
            get { return _bonds; }
            set { _bonds = value; }
        }

        /// <summary>
        /// Set the Visible property on the Bond gridview
        /// </summary>
        public bool ShowBondGrid
        {
            //get { return myVar; }
            set { BondGrid.Visible = value; }
        }

        /// <summary>
        /// Set the Visible property on the Loan Agreement gridview
        /// </summary>
        public bool ShowLoanAgreeGrid
        {
            //get { return myVar; }
            set { LoanAgreeGrid.Visible = value; }
        }

        /// <summary>
        /// Set the Visible property on the Bond Detail Row to display controls to update a Bond record
        /// </summary>
        public bool UpdateBond
        {
            //get { return myVar; }
            set { BondDetailRow.Visible = value; }
        }

        /// <summary>
        /// Set the Visible property on the Add Loan Agreement Row to display controls to add a new Loan Agreement record
        /// </summary>
        public bool AddLoanAgreement
        {
            //get { return myVar; }
            set { AddRow.Visible = value; }
        }

        /// <summary>
        /// Set the Visible property on the Cancel button
        /// </summary>
        public bool ShowCancel
        {
            //get { return myVar; }
            set { CancelButton.Visible = value; }
        }

        /// <summary>
        /// Set the Visible property on the Submit button
        /// </summary>
        public bool ShowSubmit
        {
            //get { return myVar; }
            set { SubmitButton.Visible = value; }
        }

        /// <summary>
        /// Set the display text on the Submit button
        /// </summary>
        public string SubmitButtonText
        {
            //get { return _submitButtonText; }
            set { SubmitButton.Text = value; }
        }

        /// <summary>
        /// Get the value from the Date control
        /// </summary>
        public DateTime? LoanAgreementDate
        {
            get { return LoanAgreeDateAdd.Date; }
            set { LoanAgreeDateAdd.Date = value; }
        }

        /// <summary>
        /// Get the text value from the Amount input
        /// </summary>
        public string LoanAgreementAmount
        {
            get { return LoanAgreeAmountAdd.Text; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int DeedsOfficeSelectedValue
        {
            get 
            {
                if (!String.IsNullOrEmpty(DeedsOfficeUpdate.SelectedValue))
                    return Convert.ToInt32(DeedsOfficeUpdate.SelectedValue);
                
                return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int AttorneySelectedValue
        {
            get
            {
                if (!String.IsNullOrEmpty(Page.Request.Form[AttorneyUpdate.UniqueID]))
                    return Int32.Parse(Page.Request.Form[AttorneyUpdate.UniqueID]);

                return -1;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string BondRegNumber
        {
            get { return BondRegNumberUpdate.Text; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double BondRegAmount
        {
            get
            {
                string bondAmount = BondRegAmountUpdate.Text.Replace("R", "").Trim();
                return Convert.ToDouble(bondAmount.Length > 0 ? bondAmount : "0");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool BondGridPostBack
        {
            set { _bondGridPostBack = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int BondGridSelectedKey
        {
            get
            {
                return Convert.ToInt32(BondGrid.SelectedRow.Cells[0].Text);
            }
        }

        #endregion

        #endregion
    }
}
