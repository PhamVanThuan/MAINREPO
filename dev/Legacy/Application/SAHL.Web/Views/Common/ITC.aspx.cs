using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SAHL.Common.Web.UI;
using SAHL.Web.Controls;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class ITC : SAHLCommonBaseView, IITC
    {
        private Int32 _legalEntityKeyForHistory;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ShouldRunPage)
                return;

            if (IsPostBack)
            {
                string ctrlname = Page.Request.Params.Get("__EVENTTARGET");
                if (ctrlname != null && !String.IsNullOrEmpty(ctrlname))
                {
                    if (ctrlname.EndsWith("btnDoEnquiry")) // DoEnquiry was clicked
                        OnDoEnquiryButtonClicked(sender, e);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDoEnquiry_Click(object sender, EventArgs e)
        {
            OnDoEnquiryButtonClicked(sender, e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Navigator.Navigate("Back");
        }

        protected void grdITC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // use the CommandName property to determine which button was clicked.
            if (e.CommandName == "ViewHistory")
            {
                // Convert the row index stored in the CommandArgument
                // property to an Integer.
                int rowindex = Convert.ToInt32(e.CommandArgument);

                // Retrieve the row that contains the button clicked
                // by the user from the Rows collection.
                GridViewRow grdRow = grdITC.Rows[rowindex];
                _legalEntityKeyForHistory = Convert.ToInt32(grdRow.Cells[(int)ITCGrid.GridColumns.LegalEntityKey].Text);

                OnViewHistoryButtonClicked(sender, e);
            }
        }

        #region IITC Members

        /// <summary>
        ///
        /// </summary>
        /// <param name="listITC"></param>
        public void BindITCGrid(List<BindableITC> listITC)
        {
            grdITC.BindITCList(listITC);

            //Create a client side array for javascript access to controls
            //Have to do this after it has been added to the pnl, cause the ClientID's change when moved
            foreach (GridViewRow gvr in grdITC.Rows)
            {
                if (gvr.Cells[(int)ITCGrid.GridColumns.DoEnquiry].Controls.Count > 0)
                {
                    CheckBox cb = (CheckBox)gvr.Cells[(int)ITCGrid.GridColumns.DoEnquiry].Controls[0];
                    ClientScript.RegisterArrayDeclaration("CheckBoxIDs", String.Concat("'", cb.ClientID, "'"));
                }
            }
        }

        #region BindOtherAccountITCGrid

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="listITC"></param>
        //public void BindOtherAccountITCGrid(List<BindableITC> listITC)
        //{
        //    //apOtherAccountITC.Visible = true;
        //    pnlGrid2.Visible = true;
        //    grdITCOtherAccount.BindITCList(listITC);
        //}

        #endregion BindOtherAccountITCGrid

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnDoEnquiryButtonClicked;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnViewHistoryButtonClicked;

        #region Properties

        /// <summary>
        ///
        /// </summary>
        public List<Int32> ListITCDoEnquiry
        {
            get
            {
                List<Int32> listITCDoEnquiry = new List<Int32>();

                foreach (GridViewRow gvr in grdITC.Rows)
                {
                    if (gvr.Cells[(int)ITCGrid.GridColumns.DoEnquiry].Controls.Count > 0)
                    {
                        CheckBox cb = (CheckBox)gvr.Cells[(int)ITCGrid.GridColumns.DoEnquiry].Controls[0];
                        if (cb.Checked)
                        {
                            listITCDoEnquiry.Add(Convert.ToInt32(gvr.Cells[(int)ITCGrid.GridColumns.LegalEntityKey].Text));
                        }
                    }
                }
                return listITCDoEnquiry;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 LegalEntityKeyForHistory
        {
            get { return _legalEntityKeyForHistory; }
        }

        /// <summary>
        ///
        /// </summary>
        public bool ViewHistoryColumnVisible
        {
            set { grdITC.ViewHistoryColumnVisible = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public bool DoEnquiryColumnVisible
        {
            set { grdITC.DoEnquiryColumnVisible = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public bool DoEnquiryButtonVisible
        {
            set { btnDoEnquiry.Visible = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public bool AccountColumnVisible
        {
            set
            {
                grdITC.AccountColumnVisible = value;
                //grdITCOtherAccount.AccountColumnVisible = value; //itcs from other accounts, AccountKey should always be visible
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool StatusColumnVisible
        {
            set
            {
                grdITC.StatusColumnVisible = value;
                //grdITCOtherAccount.AccountColumnVisible = value; //itcs from other accounts, AccountKey should always be visible
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool BackButtonVisible
        {
            set { btnBack.Visible = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public string HeaderCaption
        {
            get { return grdITC.HeaderCaption; }
            set { grdITC.HeaderCaption = value; }
        }

        #endregion Properties

        #endregion IITC Members
    }
}