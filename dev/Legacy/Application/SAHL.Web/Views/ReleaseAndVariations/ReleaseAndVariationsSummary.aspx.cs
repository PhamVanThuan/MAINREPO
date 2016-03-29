using System;
using System.Data;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.ReleaseAndVariations
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ReleaseAndVariationsSummary : SAHLCommonBaseView, Interfaces.IReleaseAndVariationsSummary
    {

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnPrintRequestClicked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnUpdateConditionsClicked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnERFInformationClicked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnUpdateSummaryClicked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnSubmitClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnConfirmClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnCancelClicked;

        //public event EventHandler gridConditionsGridSelectedIndexChanged;

        /// <summary>
        ///  Print the Request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrintRequest_Click(object sender, EventArgs e)
        {
            if (btnPrintRequestClicked != null)
                btnPrintRequestClicked(sender, e);
        }

        /// <summary>
        /// Update the conditions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdateConditions_Click(object sender, EventArgs e)
        {
            if (btnUpdateConditionsClicked != null)
                btnUpdateConditionsClicked(sender, e);
        }

        /// <summary>
        /// View/Modify the ERF information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnERFInformation_Click(object sender, EventArgs e)
        {
            if (btnERFInformationClicked != null)
                btnERFInformationClicked(sender, e);
        }

        /// <summary>
        /// Update the Summary
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdateSummary_Click(object sender, EventArgs e)
        {
            if (btnUpdateSummaryClicked != null)
                btnUpdateSummaryClicked(sender, e);
        }

        /// <summary>
        /// Submit the new Release and Variations request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (btnSubmitClicked != null)
                btnSubmitClicked(sender, e);
        }


        /// <summary>
        /// Confirm the New Creation of a Record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (btnConfirmClicked != null)
                btnConfirmClicked(sender, e);
        }


        /// <summary>
        /// Confirm that the client cancels the request to add a new record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancelClicked != null)
                btnCancelClicked(sender, e);
        }


        public void BindgridBondDetails(DataTable DT)
        {
            gridBondDetails.AutoGenerateColumns = false;
            gridBondDetails.AddGridBoundColumn("InFavourOf", "Favour Of", new Unit(0, UnitType.Percentage), HorizontalAlign.Left, true);
            gridBondDetails.AddGridBoundColumn("BondRegistrationAmount", "Amount", new Unit(0, UnitType.Percentage), HorizontalAlign.Left, true);
            gridBondDetails.AddGridBoundColumn("CoverAmount", "Cover Amt", new Unit(0, UnitType.Percentage), HorizontalAlign.Left, true);
            gridBondDetails.AddGridBoundColumn("BondRegistrationDate", "Reg Date", new Unit(0, UnitType.Percentage), HorizontalAlign.Left, true);
            gridBondDetails.DataSource = DT;
            gridBondDetails.DataBind();

        }

        public void BindgridConditions(DataTable DT)
        {
            gridConditions.AutoGenerateColumns = false;
            gridConditions.AddGridBoundColumn("Condition", "Condition Description", new Unit(0, UnitType.Percentage), HorizontalAlign.Left, true);
            gridConditions.DataSource = DT;
            gridConditions.DataBind();
        }


        /// <summary>
        /// bind the object list to the 'ddlRequestType' drop down list
        /// </summary>
        public void bindddlRequestType(DataTable DT)
        {
            ddlRequestType.Items.Clear();
            foreach (DataRow drow in DT.Rows)
            {

                ListItem item = new ListItem(drow[0].ToString(), drow[1].ToString(), true);
                if (ddlRequestType.Items != null) ddlRequestType.Items.Add(item);
            }
        }

        /// <summary>
        /// bind the object list to the 'ddlRequestType' drop down list
        /// </summary>
        public void bindddlApplyChangeTo(DataTable DT)
        {
            ddlApplyChangeTo.Items.Clear();
            foreach (DataRow drow in DT.Rows)
            {

                ListItem item = new ListItem(drow[0].ToString(), drow[1].ToString(), true);
                if (ddlApplyChangeTo.Items != null) ddlApplyChangeTo.Items.Add(item);
            }
        }


        // SET THE OBJECTS VISIBILITY *****************************************************************

        /// <summary>
        /// Property to show/hide the 'lblAccountName' 
        /// </summary>
        public bool ShowlblAccountName
        {
            set { lblAccountName.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'lblAccountNumber'  object
        /// </summary>
        public bool ShowlblAccountNumber
        {
            set { lblAccountNumber.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'lblLinkedToOffer'  object
        /// </summary>
        public bool ShowlblLinkedToOffer
        {
            set { lblLinkedToOffer.Visible = value; }
        }


        /// <summary>
        /// Property to show/hide the 'ddlRequestType'  object
        /// </summary>
        public bool ShowddlRequestType
        {
            set { ddlRequestType.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'lblRequestType'  object
        /// </summary>
        public bool ShowlblRequestType
        {
            set { lblRequestType.Visible = value; }
        }


        /// <summary>
        /// Property to show/hide the 'ddlApplyChangeTo'  object
        /// </summary>
        public bool ShowddlApplyChangeTo
        {
            set { ddlApplyChangeTo.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'lblApplyChangeTo'  object
        /// </summary>
        public bool ShowlblApplyChangeTo
        {
            set { lblApplyChangeTo.Visible = value; }
        }


        /// <summary>
        /// Property to show/hide the 'lblLoanBalance'  object
        /// </summary>
        public bool ShowlblLoanBalance
        {
            set { lblLoanBalance.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'lblArrears'  object
        /// </summary>
        public bool ShowlblArrears
        {
            set { lblArrears.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'lblSPV'  object
        /// </summary>
        public bool ShowlblSPV
        {
            set { lblSPV.Visible = value; }
        }


        /// <summary>
        /// Property to show/hide the 'txtNotes'  object
        /// </summary>
        public bool ShowtxtNotes
        {
            set { txtNotes.Visible = value; }
        }

        /// <summary>
        /// Property to set The readonly attribure of the 'txtNotes'  object
        /// </summary>
        public bool SetReadOnlytxtNotes
        {
            set { txtNotes.ReadOnly = value; }
        }

        /// <summary>
        /// Property to show/hide the 'lblCurrentLTV'  object
        /// </summary>
        public bool ShowlblCurrentLTV
        {
            set { lblCurrentLTV.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'lblCurrentPTI'  object
        /// </summary>
        public bool ShowlblCurrentPTI
        {
            set { lblCurrentPTI.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'lblCurrentLoan'  object
        /// </summary>
        public bool ShowlblCurrentLoan
        {
            set { lblCurrentLoan.Visible = value; }
        }


        /// <summary>
        /// Property to show/hide the 'lblProducts'  object
        /// </summary>
        public bool ShowlblProducts
        {
            set { lblProducts.Visible = value; }
        }


        /// <summary>
        /// Property to show/hide the 'gridBondDetails'  object
        /// </summary>
        public bool ShowgridBondDetails
        {
            set { gridBondDetails.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'gridConditions'  object
        /// </summary>
        public bool ShowgridConditions
        {
            set { gridConditions.Visible = value; }
        }


        /// <summary>
        /// Property to show/hide the 'lblCaption'  object
        /// </summary>
        public bool ShowlblCaption
        {
            set { lblCaption.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'btnConfirm'  object
        /// </summary>
        public bool ShowbtnConfirm
        {
            set { btnConfirm.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'btnCancel'  object
        /// </summary>
        public bool ShowbtnCancel
        {
            set { btnCancel.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'btnSubmit'  object
        /// </summary>
        public bool ShowbtnSubmit
        {
            set { btnSubmit.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'btnPrintRequest'  object
        /// </summary>
        public bool ShowbtnPrintRequest
        {
            set { btnPrintRequest.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'btnUpdateConditions'  object
        /// </summary>
        public bool ShowbtnUpdateConditions
        {
            set { btnUpdateConditions.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'btnERFInformation'  object
        /// </summary>
        public bool ShowbtnERFInformation
        {
            set { btnERFInformation.Visible = value; }
        }
        /// <summary>
        /// Property to show/hide the 'btnUpdateSummary'  object
        /// </summary>
        public bool ShowbtnUpdateSummary
        {
            set { btnUpdateSummary.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'pnlContact'  object
        /// </summary>
        public bool ShowpnlContact
        {
            set { pnlContact.Visible = value; }
        }
        /// <summary>
        /// Property to show/hide the 'pnlMemo'  object
        /// </summary>
        public bool ShowpnlMemo
        {
            set { pnlMemo.Visible = value; }
        }
        /// <summary>
        /// Property to show/hide the 'pnlLoanDetails'  object
        /// </summary>
        public bool ShowpnlLoanDetails
        {
            set { pnlLoanDetails.Visible = value; }
        }
        /// <summary>
        /// Property to show/hide the 'pnlBondDetails'  object
        /// </summary>
        public bool ShowpnlBondDetails
        {
            set { pnlBondDetails.Visible = value; }
        }
        /// <summary>
        /// Property to show/hide the 'pnlConditions'  object
        /// </summary>
        public bool ShowpnlConditions
        {
            set { pnlConditions.Visible = value; }
        }

        // DO THE GET SET VALUES FOR THE SCREEN OBJECTS


        /// <summary>
        ///  Set or Get 'lblAccountName' value
        /// </summary>
        public string GetSetlblAccountName
        {
            set { lblAccountName.Text = value; }
            get { return lblAccountName.Text; }
        }

        /// <summary>
        ///  Set or Get 'lblAccountNumber' value
        /// </summary>
        public string GetSetlblAccountNumber
        {
            set { lblAccountNumber.Text = value; }
            get { return lblAccountNumber.Text; }
        }


        /// <summary>
        ///  Set or Get 'lblLinkedToOffer' value
        /// </summary>
        public string GetSetlblLinkedToOffer
        {
            set { lblLinkedToOffer.Text = value; }
            get { return lblLinkedToOffer.Text; }
        }

        /// <summary>
        ///  Set or Get 'ddlRequestType' value
        /// </summary>
        public string GetSetddlRequestType
        {
            set { ddlRequestType.Text = value; }
            get
            {
                if (Request.Form[ddlRequestType.UniqueID] != null)
                    return Convert.ToString(Request.Form[ddlRequestType.UniqueID]);
                return "";
            }
        }

        /// <summary>
        ///  Set or Get 'ddlApplyChangeTo' value
        /// </summary>
        public string GetSetddlApplyChangeTo
        {
            set { ddlApplyChangeTo.Text = value; }
            get
            {
                if (Request.Form[ddlApplyChangeTo.UniqueID] != null)
                    return Convert.ToString(Request.Form[ddlApplyChangeTo.UniqueID]);

                return "";
            }
        }

        /// <summary>
        ///  Set or Get 'ddlRequestType' SelectedIndex
        /// </summary>
        public int GetSetddlRequestTypeSelectedIndex
        {
            set { ddlRequestType.SelectedIndex = value; }
            get
            {
                return ddlRequestType.SelectedIndex;
                //if (Request.Form[ddlRequestType.UniqueID] != null)
                //    return Convert.ToString(Request.Form[ddlRequestType.UniqueID]);
                //else
                //    return "";
            }
        }

        /// <summary>
        ///  Set or Get 'GetSetddlApplyChangeTo' SelectedIndex
        /// </summary>
        public int GetSetddlApplyChangeToSelectedIndex
        {
            set { ddlApplyChangeTo.SelectedIndex = value; }
            get
            {
                return ddlApplyChangeTo.SelectedIndex;
                //if (Request.Form[ddlRequestType.UniqueID] != null)
                //    return Convert.ToString(Request.Form[ddlRequestType.UniqueID]);
                //else
                //    return "";
            }
        }

        /// <summary>
        ///  Set or Get 'lblRequestType' value
        /// </summary>
        public string GetSetlblRequestType
        {
            set { lblRequestType.Text = value; }
            get { return lblRequestType.Text; }
        }

        /// <summary>
        ///  Set or Get 'lblApplyChangeTo' value
        /// </summary>
        public string GetSetlblApplyChangeTo
        {
            set { lblApplyChangeTo.Text = value; }
            get { return lblApplyChangeTo.Text; }
        }

        /// <summary>
        ///  Set or Get 'lblLoanBalance' value
        /// </summary>
        public string GetSetlblLoanBalance
        {
            set { lblLoanBalance.Text = value; }
            get { return lblLoanBalance.Text; }
        }

        /// <summary>
        ///  Set or Get 'lblArrears' value
        /// </summary>
        public string GetSetlblArrears
        {
            set { lblArrears.Text = value; }
            get { return lblArrears.Text; }
        }

        /// <summary>
        ///  Set or Get 'lblSPV' value
        /// </summary>
        public string GetSetlblSPV
        {
            set { lblSPV.Text = value; }
            get { return lblSPV.Text; }
        }

        /// <summary>
        ///  Set or Get 'txtNotes' value
        /// </summary>
        public string GetSettxtNotes
        {
            set { txtNotes.Text = value; }
            get { return txtNotes.Text; }
        }

        /// <summary>
        ///  Set or Get 'lblCurrentLTV' value
        /// </summary>
        public string GetSetlblCurrentLTV
        {
            set { lblCurrentLTV.Text = value; }
            get { return lblCurrentLTV.Text; }
        }

        /// <summary>
        ///  Set or Get 'lblCurrentPTI' value
        /// </summary>
        public string GetSetlblCurrentPTI
        {
            set { lblCurrentPTI.Text = value; }
            get { return lblCurrentPTI.Text; }
        }

        /// <summary>
        ///  Set or Get 'lblCurrentLoan' value
        /// </summary>
        public string GetSetlblCurrentLoan
        {
            set { lblCurrentLoan.Text = value; }
            get { return lblCurrentLoan.Text; }
        }

        /// <summary>
        ///  Set or Get 'lblProducts' value
        /// </summary>
        public string GetSetlblProducts
        {
            set { lblProducts.Text = value; }
            get { return lblProducts.Text; }
        }


        /// <summary>
        ///  Set or Get 'lblCaption' value
        /// </summary>
        public string GetSetlblCaption
        {
            set { lblCaption.Text = value; }
            get { return lblCaption.Text; }
        }

    }
}