using System;
using System.Data;
using System.Text;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Authentication;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Web.Views.Common.Interfaces;
using System.Collections.Generic;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using DevExpress.Web.ASPxGridView;
using SAHL.Web.Controls;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;


namespace SAHL.Web.Views.Common
{
    public partial class X2WorkList : SAHLCommonBaseView, IX2WorkList
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            gridInstance.HtmlDataCellPrepared += new ASPxGridViewTableDataCellEventHandler(gridInstance_HtmlDataCellPrepared);
            if (IsMenuPostBack)
            {
                gridInstance.ClearSort();
                
                foreach (GridViewDataColumn col in gridInstance.GetGroupedColumns())
                {
                    gridInstance.UnGroup(col);
                }
            }
        }

        void gridInstance_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
        }

        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public void BindGrid(DataTable data)
        {
            gridInstance.DataSource = data;
            gridInstance.DataBind();
        }

        public void SetupGrid(DataTable config, string gridHeading)
        {
            for (int i = 0; i < config.Rows.Count; i++)
            {
                string columnName = config.Rows[i].ItemArray[0].ToString();
                string description = config.Rows[i].ItemArray[1].ToString();
                int width = (int)config.Rows[i].ItemArray[2];
                bool visible = (bool)config.Rows[i].ItemArray[3];
                string formatString = config.Rows[i].ItemArray[4].ToString();
                GridFormatType formatType = (GridFormatType)config.Rows[i].ItemArray[5];

                HorizontalAlign hAlign = HorizontalAlign.Left;

                switch (formatType)
                {
                    case GridFormatType.GridCurrency:
                    case GridFormatType.GridNumber:
                    case GridFormatType.GridRate:
                        hAlign = HorizontalAlign.Right;
                        break;
                    case GridFormatType.GridDate:
                        hAlign = HorizontalAlign.Center;
                        formatString = SAHL.Common.Constants.DateFormatNoCentury;
                        break;
                    case GridFormatType.GridString:
                        break;
                    default:
                        break;
                }

                DXGridViewFormattedTextColumn col = new DXGridViewFormattedTextColumn();

                if (String.Compare(columnName, "IsCapitec", true) == 0) // if this is a capitec loan then display the capitec icon
                    col.DataItemTemplate = new SAHL.Web.Controls.CapitecImageTemplate();

                col.FieldName = columnName;
                col.Caption = description;
                col.Width = Unit.Percentage(width);
                col.Visible = visible;
                col.CellStyle.HorizontalAlign = hAlign;
                col.Format = formatType;
                if (formatString.Length > 0)
                    col.FormatString = formatString;

                gridInstance.Columns.Add(col);
            }

            gridInstance.SettingsText.Title = gridHeading;
            gridInstance.Settings.ShowGroupPanel = true;

        }
        public event KeyChangedEventHandler OnSelectButtonClicked;

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            if (gridInstance.SelectedKeyValue != null && OnSelectButtonClicked != null)
                OnSelectButtonClicked(sender, new KeyChangedEventArgs(gridInstance.SelectedKeyValue));
        }

        protected void gridInstance_SelectionChanged(object sender, EventArgs e)
        {
            btnSelect_Click(gridInstance, e);
        }
    }
}
