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
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common;
using SAHL.Common.Authentication;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Security.Principal;
using SAHL.Common.Service.Interfaces;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Web.AJAX;

namespace SAHL.Web.Views.Common
{
	public partial class NoteSummary : SAHLCommonBaseView, INoteSummary
	{
		private DateTime _diaryDate;
		private int _genericKey, _genericKeyTypeKey;

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

		protected void Page_Load(object sender, EventArgs e)
		{
			RegisterWebService(ServiceConstants.Diary);
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (!ShouldRunPage)
				return;

			lblDiaryDate.Text = _diaryDate == System.DateTime.MinValue ? "None" : _diaryDate.ToString(SAHL.Common.Constants.DateFormat);

			hidGenericKey.Value = _genericKey.ToString();
			hidGenericKeyTypeKey.Value = _genericKeyTypeKey.ToString();
		}

		#region INoteSummary Members

		/// <summary>
		/// 
		/// </summary>
		public DateTime DiaryDate
		{
			set { _diaryDate = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int GenericKey
		{
			get { return _genericKey; }
			set { _genericKey = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int GenericKeyTypeKey
		{
			get { return _genericKeyTypeKey; }
			set { _genericKeyTypeKey = value; }
		}

		/// <summary>
		/// Bind Notes Grid
		/// 
		/// </summary>
		public void BindNotesGrid(List<INoteDetail> noteDetails)
		{
			if (noteDetails != null)
			{
				noteDetails.Sort(delegate(INoteDetail n1, INoteDetail n2) { return n2.InsertedDate.CompareTo(n1.InsertedDate); });

				rptNotes.DataSource = noteDetails;
				rptNotes.DataBind();
			}
		}

		/// <summary>
		/// Bind LegalEntity Dropdown List
		/// 
		/// </summary>
		public void BindLegalEntityFilter(List<ILegalEntity> legalEntities)
		{
			ddlLegalEntities.DataValueField = "Key";
			ddlLegalEntities.DataTextField = "DisplayName";

			ddlLegalEntities.DataSource = legalEntities;
			ddlLegalEntities.DataBind();
		}

		/// <summary>
		/// Bind Date Dropdown Lists
		/// 
		/// </summary>
		public void BindDateFilters(Dictionary<string, string> uniqueDates)
		{
			ddlDateFrom.DataSource = uniqueDates;
			ddlDateFrom.DataBind();

			ddlDateTo.DataSource = uniqueDates;
			ddlDateTo.DataBind();
		}

		#endregion
	}
}