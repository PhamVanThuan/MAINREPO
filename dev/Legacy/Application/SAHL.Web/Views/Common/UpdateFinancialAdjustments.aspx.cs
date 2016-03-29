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
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using DevExpress.Web.ASPxGridView;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Common
{
    public partial class UpdateFinancialAdjustments : SAHLCommonBaseView, IFinancialAdjustments
    {
		public string InformationLabel
		{
			get
			{
				return lblInformation.Text;
			}
			set
			{
				lblInformation.Text = value;
			}
		}
        #region Events

        public event KeyChangedEventHandler FinancialAdjustmentsGridRowUpdating;
        public event KeyChangedEventHandler OnUpdateButtonClicked;
        public event KeyChangedEventHandler OnCancelButtonClicked;

        #endregion

        /// <summary>
        /// Returns logged on user
        /// </summary>
        public string GetLoggedOnUser
        {
            get { return CurrentPrincipal.Identity.Name; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
			gvUpdateFinancialAdjustments.HtmlRowPrepared += new ASPxGridViewTableRowEventHandler(OnHtmlRowPrepared);
        }

		void OnHtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
		{
			if (e.RowType == GridViewRowType.Data && e.Row.Cells.Count > 1)
			{
				var effectiveDate = DateTime.Parse(e.GetValue("EffectiveDate").ToString());
				var status = e.GetValue("Status").ToString();

				if (status == FinancialAdjustmentStatuses.Inactive.ToString() &&
					effectiveDate.Date >= DateTime.Today.Date)
				{
					e.Row.Attributes.Add("style", "color: #FFFFFF; background-color:green;");
				}
			}
		}

        public void FinancialAdjustmentGridClear()
        {
            gvUpdateFinancialAdjustments.Selection.UnselectAll();
            gvUpdateFinancialAdjustments.FocusedRowIndex = -1;
        }

        protected void FinancialAdjustmentsGridRowUpdating_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            if (FinancialAdjustmentsGridRowUpdating != null)
            {
                e.Cancel = true;
                int editingIndex = ((DXGridView)sender).FindVisibleIndexByKeyValue(e.Keys[0]);
                Dictionary<string, string> dictNewValues = new Dictionary<string, string>();
                dictNewValues.Add("Status", e.NewValues["Status"].ToString());
                FinancialAdjustmentsGridRowUpdating(dictNewValues, new KeyChangedEventArgs(editingIndex));
            }
        }

		protected void OnGridPreRender(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
			if (FinancialAdjustmentsGridRowUpdating != null)
			{
				e.Cancel = true;
				int editingIndex = ((DXGridView)sender).FindVisibleIndexByKeyValue(e.Keys[0]);
				Dictionary<string, string> dictNewValues = new Dictionary<string, string>();
				dictNewValues.Add("Status", e.NewValues["Status"].ToString());
				FinancialAdjustmentsGridRowUpdating(dictNewValues, new KeyChangedEventArgs(editingIndex));
			}
		}

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (OnUpdateButtonClicked != null)
                OnUpdateButtonClicked(sender, null);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
                OnCancelButtonClicked(sender, null);
        }

        private static void AddGridColumn(SAHL.Common.Web.UI.Controls.DXGridView gridview, string fieldName, string caption, int width, GridFormatType formatType, string format, HorizontalAlign align, bool visible, bool readOnly)
        {
            switch (formatType)
            {
                case GridFormatType.GridDate:
                    DXGridViewDateColumn colDate = new DXGridViewDateColumn();
                    colDate.FieldName = fieldName;
                    colDate.Caption = caption;
                    colDate.Width = Unit.Percentage(width);
                    colDate.Visible = visible;
                    colDate.CellStyle.HorizontalAlign = align;
                    colDate.ReadOnly = readOnly;
                    colDate.PropertiesDateEdit.DropDownButton.Visible = false;

                    gridview.Columns.Add(colDate);
                    break;
                case GridFormatType.GridCurrency:
                case GridFormatType.GridDateTime:
                case GridFormatType.GridNumber:
                case GridFormatType.GridRate:
                case GridFormatType.GridRate3Decimal:
                case GridFormatType.GridString:
                    DXGridViewFormattedTextColumn colText = new DXGridViewFormattedTextColumn();
                    colText.FieldName = fieldName;
                    colText.Caption = caption;
                    colText.Width = Unit.Percentage(width);
                    colText.Format = formatType;
                    if (!String.IsNullOrEmpty(format))
                        colText.FormatString = format;
                    colText.Visible = visible;
                    colText.CellStyle.HorizontalAlign = align;
                    colText.ReadOnly = readOnly;
                    gridview.Columns.Add(colText);
                    break;
                default:
                    break;
            }

        }

        private static void AddGridCommandColumn(DXGridView gridview, string fieldName, string caption, int width, HorizontalAlign align, bool visible)
        {
            DXGridViewComboBoxColumn col = new DXGridViewComboBoxColumn();
            ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();
            IEventList<IFinancialAdjustmentStatus> filteredStatuses = new EventList<IFinancialAdjustmentStatus>();
            foreach (IFinancialAdjustmentStatus fadjStatus in lookups.FinancialAdjustmentStatuses)
            {
                if (fadjStatus.Key != (int)FinancialAdjustmentStatuses.Inactive)
                    filteredStatuses.Add(null, fadjStatus);
            }
            col.PropertiesComboBox.DataSource = (object)filteredStatuses;
            col.PropertiesComboBox.TextField = "Description";
            col.PropertiesComboBox.ValueField = "Description";
            col.PropertiesComboBox.ClientSideEvents.SelectedIndexChanged = "function(s,e){grid.UpdateEdit()}";
            col.ReadOnly = false;
            col.FieldName = fieldName;
            col.Caption = caption;
            col.Width = Unit.Percentage(width);
            col.Visible = visible;
            col.CellStyle.HorizontalAlign = align;
            gridview.Columns.Add(col);
        }

        public void SetUpFinancialAdjustmentGrid()
        {
            gvUpdateFinancialAdjustments.SettingsPager.PageSize = 30;
            gvUpdateFinancialAdjustments.KeyFieldName = "FinancialAdjustmentKey";
            AddGridColumn(gvUpdateFinancialAdjustments, "FinancialAdjustment", "Financial Adjustment", 0, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            AddGridColumn(gvUpdateFinancialAdjustments, "FinancialAdjustmentType", "Financial Adjustment Type", 0, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            AddGridColumn(gvUpdateFinancialAdjustments, "EffectiveDate", "Effective Date", 0, GridFormatType.GridDate, null, HorizontalAlign.Left, true, true);
            AddGridColumn(gvUpdateFinancialAdjustments, "Term", "Term", 0, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            AddGridColumn(gvUpdateFinancialAdjustments, "Value", "Value", 0, GridFormatType.GridString, null, HorizontalAlign.Left, true, true);
            AddGridCommandColumn(gvUpdateFinancialAdjustments, "Status", "Status", 0, HorizontalAlign.Left, true);
        }

        public void BindFinancialAdjustmentGrid(DataTable dtFinancialAdjustment)
        {
            gvUpdateFinancialAdjustments.Selection.UnselectAll();

            gvUpdateFinancialAdjustments.DataSource = dtFinancialAdjustment;
            gvUpdateFinancialAdjustments.DataBind();
        }

        public void BindFinancialAdjustmentGridPost(DataTable dtFinancialAdjustment)
        {
            gvUpdateFinancialAdjustments.CancelEdit();

            gvUpdateFinancialAdjustments.DataSource = dtFinancialAdjustment;
            gvUpdateFinancialAdjustments.DataBind();
        }

    }
}
