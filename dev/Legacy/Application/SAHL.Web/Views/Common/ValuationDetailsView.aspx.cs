using System;
using System.Data;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class ValuationDetailsView : SAHLCommonBaseView, IValuationDetailsView
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnCancelClicked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnViewDetailClicked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnSubmitClicked; 
        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler grdValuationsGridSelectedIndexChanged;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnPropertyClicked;



        //grdValuations_SelectedIndexChanged;

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        public int ValuationItemIndex
        {
            set { grdValuations.SelectedIndex = value; }
            get { return grdValuations.SelectedIndex; }
        }


        private int propertyItemIndex;

        /// <summary>
        /// Gets or sets the Proeprty Item index
        /// </summary>
        public int PropertyItemIndex
        {
            set { propertyItemIndex = value; }
            get { return propertyItemIndex; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="DT"></param>
        public void BindgrdValuations(DataTable DT)
        {
            if (DT.Rows.Count > 0)
            {
                grdValuations.AutoGenerateColumns = false;
                grdValuations.AddGridBoundColumn("Key", "", Unit.Percentage(0), HorizontalAlign.Left, false);
                grdValuations.AddGridBoundColumn("Valuer", "Valuer", Unit.Percentage(20), HorizontalAlign.Left, true);
                grdValuations.AddGridBoundColumn("ValuationDate", "Valuation Date", Unit.Percentage(10), HorizontalAlign.Left, true);
                grdValuations.AddGridBoundColumn("ValuationStatus", "Valuation Status", Unit.Percentage(5), HorizontalAlign.Left, true); //10
                grdValuations.AddGridBoundColumn("IsActive", "Active", Unit.Percentage(10), HorizontalAlign.Left, true);
                grdValuations.AddGridBoundColumn("ValuationAmount", "Valuation Amount", Unit.Percentage(15), HorizontalAlign.Left, true); //10
                grdValuations.AddGridBoundColumn("HOCValuation", "HOC Valuation", Unit.Percentage(15), HorizontalAlign.Left, true); //10
                grdValuations.AddGridBoundColumn("ValuationType", "Valuation Type", Unit.Percentage(15), HorizontalAlign.Left, true); //20
                grdValuations.AddGridBoundColumn("RequestedBy", "Requested By", Unit.Percentage(10), HorizontalAlign.Left, true);
                grdValuations.DataSource = DT;
                grdValuations.DataBind();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DT"></param>
        public void BindPropertyGrid(DataTable DT)
        {
            if (DT.Rows.Count > 0)
            {
                grdProperty.AutoGenerateColumns = false;
                grdProperty.AddGridBoundColumn("Address", "Property Details", Unit.Percentage(99), HorizontalAlign.Left, true);
                grdProperty.DataSource = DT;
                grdProperty.DataBind();
            }
        }



        protected void grdValuations_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValuationItemIndex = grdValuations.SelectedIndex;
            if (grdValuationsGridSelectedIndexChanged != null)
                grdValuationsGridSelectedIndexChanged(sender, new KeyChangedEventArgs(grdValuations.SelectedIndex));
        }





        //protected void grdValuationsDouble_Click(object sender, GridSelectEventArgs e)
        //{
        //    //btnViewDetail_Click(sender, e);
        //    if (btnViewDetailClicked != null)
        //        btnViewDetailClicked(sender, e);
        //}

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Return to Default Screen
            if (btnCancelClicked != null)
                btnCancelClicked(sender, e);
        }

        protected void btnProperty_Click(object sender, EventArgs e)
        {
            // Return to Default Screen
            if (btnPropertyClicked != null)
                btnPropertyClicked(sender, e);
        }

        protected void btnViewDetail_Click(object sender, EventArgs e)
        {
            
            // Choose the correct valuation type ad navigate to it
            if (btnViewDetailClicked != null)
                btnViewDetailClicked(sender, e);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Not sure what this does....yet..
            if (btnSubmitClicked != null)
                btnSubmitClicked(sender, e);
        }

        //***********************************************************************************
        // SET UP THE PROPERTY CHANGE METHODS

        /// <summary>
        /// Property to show/hide the Valudation Message
        /// </summary>
        public bool ShowlblErrorMessage
        {
            get { return lblErrorMessage.Visible; }
            set { lblErrorMessage.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'btnCancel' button
        /// </summary>
        public bool ShowbtnCancel
        {
            set { btnCancel.Visible = value; }
        }


        /// <summary>
        /// Property to show/hide the 'btnProperty' button
        /// </summary>
        public bool ShowbtnProperty
        {
            set { btnProperty.Visible = value; }
        }


        /// <summary>
        /// Property to show/hide the 'btnViewDetail' button
        /// </summary>
        public bool ShowbtnViewDetail
        {
            set { btnViewDetail.Visible = value; }
        }
        /// <summary>
        /// Property to show/hide the 'btnSubmit' button
        /// </summary>
        public bool ShowbtnSubmit
        {
            set { btnSubmit.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'pnlValuations' panel
        /// </summary>
        public bool ShowpnlValuations
        {
            set { pnlValuations.Visible = value; }
        }


        /// <summary>
        /// Property to show/hide the 'grdValuations' grid
        /// </summary>
        public bool ShowgrdValuations
        {
            set { grdValuations.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'pnlInspectionContactDetails' panel including subcomponents
        /// </summary>
        public bool ShowpnlInspectionContactDetails
        {
            set { pnlInspectionContactDetails.Visible = value; }
        }

        // SET UP pnlInspectionContactDetails VALUES


        /// <summary>
        ///  Set or Get the Valuations Grid Postbacktype
        /// </summary>
        public GridPostBackType SetPostBackTypegrdValuations
        {

            set { grdValuations.PostBackType = value; }
            get { return grdValuations.PostBackType; }
        }



        /// <summary>
        ///  enable or disable grdValuations
        /// </summary>
        public bool  EnablegrdValuations
        {
            
            set { grdValuations.Enabled  = value; }
            get { return grdValuations.Enabled; }
        }

        /// <summary>
        ///  Set The error Messages Message
        /// </summary>
        public string SetlblErrorMessage
        {
            set { lblErrorMessage.Text = value; }
            get { return lblErrorMessage.Text; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SettxtContact1Name
        {
            set { txtContact1Name.Text = value; }
            get { return txtContact1Name.Text; }
        }
            
        /// <summary>
        /// 
        /// </summary>
        public string SettxtContact1Phone
        {
            set { txtContact1Phone.Text = value; }
            get { return txtContact1Phone.Text; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SettxtContact1WorkPhone
        {
            set { txtContact1WorkPhone.Text = value; }
            get { return txtContact1WorkPhone.Text; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SettxtContact1MobilePhone
        {
            set { txtContact1MobilePhone.Text = value; }
            get { return txtContact1MobilePhone.Text; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SettxtContact2Name
        {
            set { txtContact2Name.Text = value; }
            get { return txtContact2Name.Text; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SettxtContact2Phone
        {
            set { txtContact2Phone.Text = value; }
            get { return txtContact2Phone.Text; }
        }

        //***********************************************************************************
    }
}
