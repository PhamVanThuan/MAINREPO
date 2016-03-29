using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.AJAX;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.DebtCounselling
{
	public partial class CourtDetails : SAHLCommonBaseView, ICourtDetails
	{
		private bool _showMaintenancePanel;
		private string _img_src_path;

		private enum GridColumnPositions
		{
			CourtDetailKey = 0,
			DebtCounsellingKey = 1,
			HearingType = 2,
			HearingAppearanceType = 3,
			Court = 4,
			CaseNumber = 5,
			HearingDate = 6,
			GeneralStatus = 7,
			Comment = 8
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			if (!ShouldRunPage)
				return;

		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!ShouldRunPage)
				return;

			RegisterWebService(ServiceConstants.Court);

			ccdAppearanceType.ServicePath = ServiceConstants.Court;

            SetHearingDateLabel();
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (!this.ShouldRunPage)
				return;

			pnlCourtDetailsMaintenance.Visible = _showMaintenancePanel;
		}

		protected void gvHearingDetails_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				// Get the Hearing Detail Row
				IHearingDetail hearingDetail = e.Row.DataItem as IHearingDetail;

				// DebtCounslelingKey
				e.Row.Cells[(int)GridColumnPositions.DebtCounsellingKey].Text = hearingDetail.DebtCounselling.Key.ToString();
				// Hearing Type
				e.Row.Cells[(int)GridColumnPositions.HearingType].Text = hearingDetail.HearingType.Description;
				// Hearing Appearance Type
				e.Row.Cells[(int)GridColumnPositions.HearingAppearanceType].Text = hearingDetail.HearingAppearanceType.Description;
				// Court 
				e.Row.Cells[(int)GridColumnPositions.Court].Text = hearingDetail.Court != null ? hearingDetail.Court.Name : "";
				// Hearing Date
				e.Row.Cells[(int)GridColumnPositions.HearingDate].Text = hearingDetail.HearingDate.ToString(SAHL.Common.Constants.DateFormat);
				// General Status 
				e.Row.Cells[(int)GridColumnPositions.GeneralStatus].Text = hearingDetail.GeneralStatus.Description;
				// Comment 
				e.Row.Cells[(int)GridColumnPositions.Comment].Text = hearingDetail.Comment;
			}
		}

		protected void DXGridView1_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
			if (e.DataColumn.Name == "colHearingDate")
			{
				IHearingDetail hd = grdHearingDetail.GetRow(e.VisibleIndex) as IHearingDetail;

				e.Cell.Text = hd.HearingDate.ToString(SAHL.Common.Constants.DateFormat);

				//if (correspondence.HasDetail == false)
				//    e.Cell.Controls[0].Visible = false;
			}

			if (e.DataColumn.Name == "colComment")
			{
				e.Cell.ToolTip = "Click to view/maintain Comments";
				_img_src_path = "../../Images/application_edit.png";
			}
		}

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
			if (OnSubmitButtonClicked != null)
				OnSubmitButtonClicked(sender, e);
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			if (OnCancelButtonClicked != null)
				OnCancelButtonClicked(sender, e);
		}

		/// <summary>
		/// On Court Search Result Item Selected
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnCourtSearchResultItemSelected(object sender, KeyChangedEventArgs e)
		{
			hidSelectedCourtKey.Value = e.Key.ToString();
		}

		/// <summary>
		/// On Hearing Appearance Type Changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnHearingAppearanceTypeChanged(object sender, EventArgs e)
		{
            SetHearingDateLabel();
		}

        private void SetHearingDateLabel()
        {
            if (String.IsNullOrEmpty(ddlAppearanceType.SelectedValue))
            {
                return;
            }
            var hearingAppearanceType = int.Parse(ddlAppearanceType.SelectedValue);
            var hearingTypeLabel = "Next Hearing Date";
            if (
                hearingAppearanceType == (int)HearingAppearanceTypes.AppealDeclined ||
                hearingAppearanceType == (int)HearingAppearanceTypes.AppealGranted ||
                hearingAppearanceType == (int)HearingAppearanceTypes.ConsentOrderGranted ||
                hearingAppearanceType == (int)HearingAppearanceTypes.OrderGranted ||
                hearingAppearanceType == (int)HearingAppearanceTypes.TribunalAppealDeclined ||
                hearingAppearanceType == (int)HearingAppearanceTypes.TribunalAppealGranted ||
                hearingAppearanceType == (int)HearingAppearanceTypes.TribunalOrderGranted)
            {
                hearingTypeLabel = "Date";
            }
            lblHearingDate.Text = hearingTypeLabel;
        }

		#region ICourtDetails Members

		public event EventHandler OnCancelButtonClicked;

		public event EventHandler OnSubmitButtonClicked;

		public event KeyChangedEventHandler OnCommentClick;

		public bool ShowMaintenancePanel
		{
			get
			{
				return _showMaintenancePanel;
			}
			set
			{
				_showMaintenancePanel = value;
			}
		}

		public string Img_src_path
		{
			get
			{
				return _img_src_path;
			}
			set
			{
				_img_src_path = value;
			}
		}

		public int SelectedHearingTypeKey
		{
			get
			{
				string key = ddlHearingType.SelectedValue;
				int result;
				if (Int32.TryParse(key, out result) == false)
					result = -1;

				return result;
			}
		}

		public int SelectedHearingAppearanceTypeKey
		{
			get
			{
				string key = ddlAppearanceType.SelectedValue;
				int result;
				if (Int32.TryParse(key, out result) == false)
					result = -1;

				return result;
			}
		}

		public int SelectedCourtKey
		{
			get
			{
				string key = Page.Request.Form[hidSelectedCourtKey.UniqueID].ToString();
				int result;
				if (Int32.TryParse(key, out result) == false)
					result = -1;

				return result;
			}
		}

		public string CaseNumber
		{
			get
			{
				return txtCaseNumber.Text;
			}
		}

		public DateTime? HearingDate
		{
			get
			{
				return dteHearingDate.Date;
			}
		}

		public string Comments
		{
			get
			{
				return txtComments.Text;
			}
		}

		public string CommentEditor
		{
			set
			{
				txtCommentEditor.Text = value;
			}
			get
			{
				return txtCommentEditor.Text;
			}
		}

		public void BindHearingDetails(IEventList<IHearingDetail> hearingDetails)
		{
			grdHearingDetail.DataSource = hearingDetails;
			grdHearingDetail.DataBind();
		}

		public void BindHearingTypes(IDictionary<int, string> hearingTypes)
		{
			ddlHearingType.DataSource = hearingTypes;
			ddlHearingType.DataBind();
		}

		#region Comment changes

		public int HearingDetailKey
		{
			get
			{
				string key = hidHearingDetailKey.Value;
				int result;
				if (Int32.TryParse(key, out result) == false)
					result = -1;

				return result;
			}
			set
			{
				hidHearingDetailKey.Value = value.ToString();
			}
		}

		protected void imgOnCommentClick(object source, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
		{
			if (OnCommentClick != null)
			{
				// this will hide/show the update button and message if required
				btnCommentEditorSubmit.Visible = _showMaintenancePanel;
				//lblCommentMessage.Visible = _showMaintenancePanel;
				txtCommentEditor.ReadOnly = !_showMaintenancePanel;

				// get the comemnt from the grid
				object o = String.IsNullOrEmpty(e.Parameter) ? null : grdHearingDetail.GetRowValuesByKeyValue(e.Parameter, "Comment");
				string comments = o != null ? o.ToString() : null;

				OnCommentClick(e.Parameter, new KeyChangedEventArgs(comments));
			}
		}

		public event EventHandler OnCommentEditorSubmitButtonClicked;

		protected void btnCommentEditorSubmit_Click(object sender, EventArgs e)
		{
			if (OnCommentEditorSubmitButtonClicked != null)
				OnCommentEditorSubmitButtonClicked(sender, e);
		}

		#endregion

		#endregion
	}
}