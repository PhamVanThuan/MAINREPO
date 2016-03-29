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
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;

namespace SAHL.Web.Views.Administration
{
    public partial class MarketingSource : SAHLCommonBaseView, IMarketingSource
    {

        #region Members
        public bool UpdatePanelVisible
        {
            set { UpdatePanel.Visible = value; }
        }


        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IMarketRates.SubmitButtonVisible">IMarketRates.SubmitButtonVisible</see>.
        /// </summary>
        public bool SubmitButtonVisible
        {
            set { SubmitButton.Visible = value; }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IMarketRates.SubmitButtonVisible">IMarketRates.SubmitButtonEnabled</see>.
        /// </summary>
        public bool SubmitButtonEnabled
        {
            set { SubmitButton.Enabled = value; }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IMarketRates.CancelButtonVisible">IMarketRates.CancelButtonVisible</see>.
        /// </summary>
        public bool CancelButtonVisible
        {
            set { CancelButton.Visible = value; }
        }

        /// <summary>
        /// Gets the market rate key selected on the grid.
        /// </summary>
        public int SelectedMarketingSourceKey
        {
            get
            {
                return Convert.ToInt32(gvMarketingSources.Rows[gvMarketingSources.SelectedIndex].Cells[0].Text);

            }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IMarketingSource.txtMarketingSourceDescriptionText">IMarketingSource.txtMarketingSourceDescriptionText</see>.
        /// </summary>
        public string txtMarketingSourceDescriptionText
        {
            set { txtMarketingSourceDescription.Text = value; }
            get { return txtMarketingSourceDescription.Text; }
        }

        public string SubmitButtonText
        {
            set { SubmitButton.Text = value; }
        }

        /// <summary>
        /// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IMarketingSource.ddlMarketingSourceStatusSelected">IMarketingSource.ddlMarketingSourceStatus</see>.
        /// </summary>
        public string ddlMarketingSourceStatusSelected
        {
            set { ddlMarketingSourceStatus.SelectedValue = value; }
            get { return ddlMarketingSourceStatus.SelectedValue; }
        }

        #endregion


        #region EventHandlers
        public event EventHandler MarketingSourceSelected;

        public event EventHandler CancelClick;

        public event EventHandler SubmitClick;




        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void BindStatusDropDown(ICollection<IGeneralStatus> generalStatus)
        {
            ddlMarketingSourceStatus.DataValueField = "Key";
            ddlMarketingSourceStatus.DataTextField = "Description";
            ddlMarketingSourceStatus.DataSource = generalStatus;
            ddlMarketingSourceStatus.DataBind();
        }

        /// <summary>
        /// Cancel button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Cancel_Click(object sender, EventArgs e)
        {
            if (CancelClick != null)
                CancelClick(sender, e);
        }

        /// <summary>
        /// Submit button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Submit_Click(object sender, EventArgs e)
        {
            if (SubmitClick != null)
                SubmitClick(sender, e);
        }

        protected void gvMarketingSources_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvMarketingSources.SelectedIndexInternal > 0)
            {
                if (MarketingSourceSelected != null)
                    MarketingSourceSelected(sender, new GridRowSelectEventArgs(SelectedMarketingSourceKey));
            }
        }

        /// <param name="marketingSources"></param>
        public void BindMarketingSourcesGrid(IReadOnlyEventList<IApplicationSource> marketingSources)
        {
            gvMarketingSources.Columns.Clear();
            gvMarketingSources.AddGridBoundColumn("Key", "Number", Unit.Percentage(10), HorizontalAlign.Left, true);
            gvMarketingSources.AddGridBoundColumn("Description", "Description", Unit.Percentage(60), HorizontalAlign.Left, true);
            gvMarketingSources.AddGridBoundColumn("", "Status", Unit.Percentage(30), HorizontalAlign.Left, true);
            gvMarketingSources.DataSource = marketingSources;
            gvMarketingSources.DataBind();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMarketingSources_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                // Get the ApplicationSource Row
                IApplicationSource applicationSource = e.Row.DataItem as IApplicationSource;

                //Status
                e.Row.Cells[2].Text = applicationSource.GeneralStatus.Description;

            }
        }
        #endregion
       
    }
}
