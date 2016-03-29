using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Reports.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using System.Xml.Linq;
using DevExpress.XtraPivotGrid;
using System.Xml;

namespace SAHL.Web.Views.Reports
{
	/// <summary>
	/// DevExpress Pivot Report Viewer
	/// </summary>
	public partial class DevExPivotViewer : SAHLCommonBaseView, IDevExPivotViewer
	{
		private string _reportText;
		public event EventHandler OnSubmitButtonClicked;

		/// <summary>
		/// On Pre Initialization
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreInit(EventArgs e)
		{
			base.OnPreInit(e);

			
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!ShouldRunPage) return;

			IReportRepository rr = RepositoryFactory.GetRepository<IReportRepository>();
			IReportStatement rs = rr.GetReportStatementByKey(int.Parse(_reportText));
			string uiStatementText = rr.GetUIStatementText(rs).ToString();
			uiStatementText = uiStatementText.Replace(Convert.ToChar(34), Convert.ToChar(39));
			BuildPivotReport(uiStatementText);
		}


		protected void btnCancel_Click(object sender, EventArgs e)
		{
			OnSubmitButtonClicked(sender, e);
		}

		public string ReportText
		{
			set { _reportText = value; }
		}

		/// <summary>
		/// On Pre Render
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (!ShouldRunPage) return;
		}

		/// <summary>
		/// Build Pivot Report
		/// </summary>
		private void BuildPivotReport(string xml)
		{
			XDocument xdoc = XDocument.Parse(xml);

			var query = from element in xdoc.Element("root").Elements("ConnectionString")
						select element;

			if (query.Count() > 0)
			{
				aspxPivotGrid.OLAPConnectionString = query.First().Attribute("value").Value;
			}

			if (!IsPostBack)
			{
				var pivotitems = from pivotfields in xdoc.Descendants("PivotField")
								 select new
								 {
									 Header = pivotfields.Attribute("name").Value,
									 val = pivotfields.Attribute("value").Value,
									 area = pivotfields.Attribute("PivotArea").Value,
									 Children = pivotfields.Descendants("FieldAttributes")
								 };

				foreach (var pivotfield in pivotitems)
				{
					string area = pivotfield.area.ToString();
					string fieldname = pivotfield.val.ToString();
					DevExpress.Web.ASPxPivotGrid.PivotGridField pgf = new DevExpress.Web.ASPxPivotGrid.PivotGridField();
					pgf.FieldName = fieldname;
					switch (area)
					{
						case "Column":
							pgf.Area = PivotArea.ColumnArea;
							break;
						case "Filter":
							pgf.Area = PivotArea.FilterArea;
							pgf.FilterValues.Clear();
							break;
						case "Row":
							pgf.Area = PivotArea.RowArea;
							break;
						case "Data":
							pgf.Area = PivotArea.DataArea;
							break;
						default:
							break;
					}

					foreach (var pivotfieldAttr in pivotfield.Children)
					{
						string fieldAttribute = pivotfieldAttr.FirstAttribute.Value.ToString();
						string fieldAttributeVal = pivotfieldAttr.FirstAttribute.NextAttribute.Value.ToString(); ;

						switch (fieldAttribute)
						{

							case "Caption":
								pgf.Caption = fieldAttributeVal;
								break;
							case "Format":
								pgf.ValueFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
								break;
							case "FilterInclude":
								pgf.FilterValues.Add(fieldAttributeVal);
								pgf.FilterValues.FilterType = DevExpress.XtraPivotGrid.PivotFilterType.Included;
								break;
							case "FilterExclude":
								pgf.FilterValues.Add(fieldAttributeVal);
								pgf.FilterValues.FilterType = DevExpress.XtraPivotGrid.PivotFilterType.Included;
								break;
							case "Visibility":
								if (fieldAttributeVal == "true")
									pgf.Visible = true;
								else
									pgf.Visible = false;
								break;
						}
					}

					aspxPivotGrid.Fields.AddField(pgf);
				}
			}
			aspxPivotGrid.DataBind();
		}
	}
}
