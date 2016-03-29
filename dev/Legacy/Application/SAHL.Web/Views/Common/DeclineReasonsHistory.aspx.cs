using System;
using System.Data;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class DeclineReasonsHistory : SAHLCommonBaseView, IDeclineReasonsHistory
    {
        #region IDeclineReasonsHistory Members

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OngrdRevisionHistoryIndexChanged;

        /// <summary>
        /// grdRevisionHistory_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdRevisionHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeyChangedEventArgs args = new KeyChangedEventArgs(grdRevisionHistory.SelectedIndex);
            if (OngrdRevisionHistoryIndexChanged != null)
            {
                OngrdRevisionHistoryIndexChanged(sender, args);
            }
        }

        /// <summary>
        /// Property to set  the 'lblTotalLoanRequired' text
        /// </summary>
        public string SetlblTotalLoanRequired
        {
            set { lblTotalLoanRequired.Text = value; }
        }

        /// <summary>
        /// Property to set  the 'lblTerm' text
        /// </summary>
        public string SetlblTerm
        {
            set { lblTerm.Text = value; }
        }

        /// <summary>
        /// Property to set  the 'lblLinkRate' text
        /// </summary>
        public string SetlblLinkRate
        {
            set { lblLinkRate.Text = value; }
        }

        /// <summary>
        /// Property to set  the 'lblEffectiveRate' text
        /// </summary>
        public string SetlblEffectiveRate
        {
            set { lblEffectiveRate.Text = value; }
        }

        /// <summary>
        /// Property to set  the 'lblBondToRegister' text
        /// </summary>
        public string SetlblBondToRegister
        {
            set { lblBondToRegister.Text = value; }
        }

        /// <summary>
        /// Property to set  the 'lblPTI' text
        /// </summary>
        public string SetlblPTI
        {
            set { lblPTI.Text = value; }
        }

        /// <summary>
        /// Property to set  the 'lblLTV' text
        /// </summary>
        public string SetlblLTV
        {
            set { lblLTV.Text = value; }
        }

        /// <summary>
        /// Property to set  the 'lblTotalInstallment' text
        /// </summary>
        public string SetlblTotalInstallment
        {
            set { lblTotalInstallment.Text = value; }
        }

        /// <summary>
        /// Property to set  the 'lblHouseHoldIncome' text
        /// </summary>
        public string SetlblHouseHoldIncome
        {
            set { lblHouseHoldIncome.Text = value; }
        }

        /// <summary>
        /// Property to set  the 'lblCategory' text
        /// </summary>
        public string SetlblCategory
        {
            set { lblCategory.Text = value; }
        }

        /// <summary>
        /// Property to set  the 'lblSPVName' text
        /// </summary>
        public string SetlblSPVName
        {
            set { lblSPVName.Text = value; }
        }

        /// <summary>
        ///  Gets and Sets the Selected Value for the Revision HistoryGrid
        /// </summary>
        public int grdRevisionHistoryKey
        {
            get { return Convert.ToInt32(grdRevisionHistory.SelectedRow.Cells[0].Text); }
        }

        /// <summary>
        ///  Gets and Sets the Selected Index for the Revision HistoryGrid
        /// </summary>
        public int grdRevisionHistorySelectedIndex
        {
            get { return grdRevisionHistory.SelectedIndex; }
            set { grdRevisionHistory.SelectedIndex = value; }
        }


        /// <summary>
        /// Bind The Revision History Data To The Grid
        /// </summary>
        /// <param name="DT"></param>
        public void BindgrdRevisionHistory(DataTable DT)
        {
            if (DT.Rows.Count > 0)
            {
                grdRevisionHistory.Columns.Clear();
                grdRevisionHistory.AddGridBoundColumn("Key", "Key", Unit.Percentage(10), HorizontalAlign.Left, true);
                grdRevisionHistory.Columns[0].Visible = false;
                grdRevisionHistory.AddGridBoundColumn("Revision", "Revision", Unit.Percentage(10), HorizontalAlign.Left, true);
                grdRevisionHistory.AddGridBoundColumn("DateRevised", "Date Revised", Unit.Percentage(20), HorizontalAlign.Left, true);
                grdRevisionHistory.AddGridBoundColumn("ApplicationType", "Application Type", Unit.Percentage(50), HorizontalAlign.Left, true);
                grdRevisionHistory.AddGridBoundColumn("Product", "Product", Unit.Percentage(20), HorizontalAlign.Left, true);
                grdRevisionHistory.DataSource = DT;
                grdRevisionHistory.DataBind();
            }
        }

        /// <summary>
        /// Bind the Decline Reason data to the Grid
        /// </summary>
        /// <param name="DT"></param>
        public void BindgrdDeclineReasons(DataTable DT)
        {
            if (DT.Rows.Count > 0)
            {
                grdDeclineReasons.Columns.Clear();
                grdDeclineReasons.AddGridBoundColumn("ReasonType", "Reason Type", Unit.Percentage(20), HorizontalAlign.Left, true);
                grdDeclineReasons.AddGridBoundColumn("Description", "Description", Unit.Percentage(40), HorizontalAlign.Left, true);
                grdDeclineReasons.AddGridBoundColumn("Comment", "Comment", Unit.Percentage(40), HorizontalAlign.Left, true);
                grdDeclineReasons.DataSource = DT;
                grdDeclineReasons.DataBind();
            }
        }

        #endregion





    }
}