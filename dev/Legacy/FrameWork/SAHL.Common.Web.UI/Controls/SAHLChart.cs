using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Drawing;
using DevExpress.XtraCharts;
using System.Data;

[assembly: TagPrefix("SAHL.Common.Web.UI.Controls", "SAHL")]
namespace SAHL.Common.Web.UI.Controls
{
	[ToolboxBitmap(typeof(SAHLChart))]
	[ToolboxData("<{0}:SAHLChart runat=server></{0}:SAHLChart>")]
	public class SAHLChart : DevExpress.XtraCharts.Web.WebChartControl
	{
		/// <summary>
		/// Render Graph
		/// </summary>
		/// <param name="graphName">The name of the graph, which will also appear as the legend</param>
		/// <param name="graphDataTable">Data Table that contains the data for the graph to be rendered</param>
		/// <param name="xAxisName">The name of the column contained in the dataset to represent the X Axis</param>
		/// <param name="yAxisName">The name of the column contained in the dataset to represent the Y Axis</param>
        /// <param name="color"></param>
		public void RenderGraph(string graphName, DataTable graphDataTable, string xAxisName, string yAxisName, Color color)
		{
			Series series = new Series(graphName, ViewType.Line);
            ((LineSeriesView)series.View).LineMarkerOptions.Visible = true;
            ((LineSeriesView)series.View).LineStyle.Thickness = 4;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 3;
            ((LineSeriesView)series.View).Color = color;

            series.DataSource = graphDataTable;
			series.ValueDataMembers.AddRange(new string[] { yAxisName });
			series.ValueScaleType = ScaleType.Numerical;
			series.ArgumentScaleType = ScaleType.Numerical;
			series.ArgumentDataMember = xAxisName;

			series.Label.Visible = false;
			this.Series.Add(series);
		}

		/// <summary>
		/// Render a Line
		/// </summary>
		/// <param name="graphName"></param>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <param name="showLabel"></param>
        /// <param name="color"></param>
		public void RenderLine(string graphName, double x1, double y1, double x2, double y2, bool showLabel, Color color)
		{
			Series series = new DevExpress.XtraCharts.Series(graphName, ViewType.Line);

            ((LineSeriesView)series.View).LineMarkerOptions.Visible = true;
            ((LineSeriesView)series.View).LineStyle.Thickness = 4;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 3;
            ((LineSeriesView)series.View).Color = color;

            series.Points.Add(new SeriesPoint(x1, y1));
			series.Points.Add(new SeriesPoint(x2, y2));
			series.ValueScaleType = ScaleType.Numerical;
			series.ArgumentScaleType = ScaleType.Numerical;
			series.Label.Visible = showLabel;
			this.Series.Add(series);
		}
	}
}
