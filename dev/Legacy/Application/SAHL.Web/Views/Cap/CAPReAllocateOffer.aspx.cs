using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Cap.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel;

namespace SAHL.Web.Views.Cap
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CAPReAllocateOffer : SAHLCommonBaseView, ICAPReAllocateOffer
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public int AllocatedToListSelectedValue
        {
            get
            {
                if (Request.Form[AllocatedToList.UniqueID] != null && Request.Form[AllocatedToList.UniqueID] != "-select-")
                    return Convert.ToInt32(Request.Form[AllocatedToList.UniqueID]);
                else
                    return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ReAllocateToListSelectedValue
        {
            get
            {
                if (Request.Form[ReAllocateToList.UniqueID] != null && Request.Form[ReAllocateToList.UniqueID] != "-select-")
                    return Convert.ToInt32(Request.Form[ReAllocateToList.UniqueID]);
                else
                    return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<int> SelectedOffers
        {
            get
            {
                IList<int> selectedList = new List<int>();
                for (int iC = 0; iC < AllocateGrid.Rows.Count; iC++)
                {
                    CheckBox checkBox = (CheckBox)AllocateGrid.Rows[iC].Cells[1].Controls[0];
                    if (checkBox.Checked)
                    {
                        selectedList.Add(int.Parse(AllocateGrid.Rows[iC].Cells[0].Text));
                    }
                }
                return selectedList;
            }
        }

        #endregion

        #region Event Handlers

        public event EventHandler OnUpdateButtonClicked;

        #endregion


        #region Protected Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AllocateGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            if (OnUpdateButtonClicked != null)
            {
                OnUpdateButtonClicked(sender, e);
            }
        }

        #endregion


        #region ICAPReAllocateOffer members

        /// <summary>
        /// 
        /// </summary>
        public void BindGrid(DataTable capOfferList)
        {
            AllocateGrid.AddGridBoundColumn("CapOfferKey", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            AllocateGrid.AddCheckBoxColumn("", "", true, Unit.Percentage(1), HorizontalAlign.Left, true);
            AllocateGrid.AddGridBoundColumn("AccountKey", "Account Key", Unit.Percentage(14), HorizontalAlign.Left, true);
            AllocateGrid.AddGridBoundColumn("Information", "Legal Entity Name", Unit.Percentage(40), HorizontalAlign.Left, true);
            AllocateGrid.AddGridBoundColumn("OfferStartDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Offer Start Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);
            AllocateGrid.AddGridBoundColumn("OfferEndDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Offer End Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);
            AllocateGrid.AddGridBoundColumn("CapEffectiveDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Effective Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);

            AllocateGrid.DataSource = capOfferList;
            AllocateGrid.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userlist"></param>
        public void BindBrokerLists(IList<IBroker> userlist)
        {
            AllocatedToList.DataSource = userlist;
            AllocatedToList.DataTextField = "FullName";
            AllocatedToList.DataValueField = "Key";
            AllocatedToList.DataBind();

            ReAllocateToList.DataSource = userlist;
            ReAllocateToList.DataTextField = "FullName";
            ReAllocateToList.DataValueField = "Key";
            ReAllocateToList.DataBind();
        }

        #endregion

    }
}
