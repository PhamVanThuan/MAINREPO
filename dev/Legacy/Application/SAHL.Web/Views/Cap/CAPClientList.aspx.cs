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
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System.Diagnostics.CodeAnalysis;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Web.Views.Cap.Interfaces;
using System.Text;

namespace SAHL.Web.Views.Cap
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CAPClientList :  SAHLCommonBaseView, ICAPClientList
    {

        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnExtractButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnImportButtonClicked;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public bool FileNameRowVisible
        {
            set
            {
                FileNameRow.Visible = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SPVRowVisible
        {
            set
            {
                SPVRow.Visible = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool DateExcludeRowVisible
        {
            set
            {
                DateExcludeRow.Visible = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ArrearRowVisible
        {
            set
            {
                ArrearRow.Visible = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ExtractButtonVisible
        {
            set
            {
                ExtractButton.Visible = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ImportButtonVisible
        {
            set
            {
                ImportButton.Visible = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SelectedCapResetConfigDate
        {
            get
            {
                if (Request.Form[ddlResetDate.UniqueID] != null && Request.Form[ddlResetDate.UniqueID] != "-select-")
                    return Request.Form[ddlResetDate.UniqueID];
                else
                    return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SelectedOperatorValue
        {
            get
            {
                if (Request.Form[ddlOperator.UniqueID] != null && Request.Form[ddlOperator.UniqueID] != "-select-")
                    return Request.Form[ddlOperator.UniqueID];
                else
                    return "";
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public int SelectedCapType
        {
            get
            {
                if (Request.Form[ddlOfferDates.UniqueID] != null && Request.Form[ddlOfferDates.UniqueID] != "-select-")
                    return Convert.ToInt32( Request.Form[ddlOfferDates.UniqueID] );
                else
                    return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double? LoanArrearBalanceValue
        {
            get
            {
                if (String.IsNullOrEmpty(LoanArrearBalance.Text))
                    return new double?();
                else
                    return new double?(Convert.ToDouble(LoanArrearBalance.Text));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? DateExcludeValue
        {
            get
            {
                return DateExclude.Date;
                // return DateExclude.Date.Value.ToString("yyyy/MM/dd") + " 12:00:00 AM";
            }
            set
            {

                DateExclude.Date = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<string> SPVList
        {
            get
            {
                List<string> spvList = new List<string>();
                foreach (ListItem li in SPVNumber.Items)
                {
                    if (li.Selected)
                        spvList.Add(li.Value);
                }
                return spvList;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FileNameValue
        {
            get
            {
                string filename = FileName.PostedFile.FileName;
                char[] splitBy = { '/' };
                string[] splitFileName = filename.Split(splitBy);
                filename = splitFileName[splitFileName.Length - 1];
                char[] splitBy2 = { '\\' };
                string[] splitFileName2 = filename.Split(splitBy2);
                filename = splitFileName2[splitFileName2.Length - 1];
                if (filename.Length > 0)
                    return filename;
                else
                    return null;
            }
        }

        /// <summary>
        /// Gets the file posted.
        /// </summary>
        public HttpPostedFile File
        {
            get
            {
                return FileName.PostedFile;
            }
        }
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
        protected void CapListGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ICapTypeConfigurationDetail ro = e.Row.DataItem as ICapTypeConfigurationDetail;
                e.Row.Cells[0].Text = ro.CapType.Description;
                e.Row.Cells[2].Text = ro.CapTypeConfiguration.CapEffectiveDate.ToString();
                e.Row.Cells[3].Text = ro.CapTypeConfiguration.Term.ToString();
                e.Row.Cells[4].Text = ro.CapTypeConfiguration.CapClosureDate.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ExtractButton_Click(object sender, EventArgs e)
        {
            if (OnExtractButtonClicked != null)
            {
                OnExtractButtonClicked(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImportButton_Click(object sender, EventArgs e)
        {
            if (OnImportButtonClicked != null)
            {
                OnImportButtonClicked(sender, e);
            }
        }

        #endregion


        #region ICapClientList Method


        /// <summary>
        /// 
        /// </summary>
        /// <param name="resetDates"></param>
        public void BindResetDateDropDown(DataTable resetDates)
        {
            ddlResetDate.Items.Clear();
            for (int i = 0; i < resetDates.Rows.Count; i++)
            {
                ddlResetDate.Items.Add(new ListItem(
                    resetDates.Rows[i]["Description"].ToString(),
                    Convert.ToDateTime(resetDates.Rows[i]["ResetDate"]).ToString(SAHL.Common.Constants.DateFormat)
                    ));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offerDates"></param>
        public void BindOfferDateDropDown(DataTable offerDates)
        {
            ddlOfferDates.DataSource = offerDates;
            ddlOfferDates.DataTextField = "Value";
            ddlOfferDates.DataValueField = "Key";
            ddlOfferDates.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        public void BindOperatorDropDown()
        {
            ddlOperator.Items.Clear();
            ddlOperator.Items.Add(new ListItem("<", "<"));
            ddlOperator.Items.Add(new ListItem("<=", "<="));
            ddlOperator.Items.Add(new ListItem("=", "="));
        }

        /// <summary>
        /// 
        /// </summary>
        public void BindGrid(IList<ICapTypeConfigurationDetail> detailList) 
        {
            CapListGrid.Columns.Clear();
            CapListGrid.AddGridBoundColumn("", "Cap Type", Unit.Percentage(40), HorizontalAlign.Left, true);
            CapListGrid.AddGridBoundColumn("Rate", SAHL.Common.Constants.RateFormat, GridFormatType.GridNumber, "CAP Base Rate", false, Unit.Percentage(15), HorizontalAlign.Left, true);
            CapListGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "CAP Effective Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);
            CapListGrid.AddGridBoundColumn("", "Term", Unit.Percentage(15), HorizontalAlign.Center, true);
            CapListGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "CAP Closure Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);
            CapListGrid.DataSource = detailList;
            CapListGrid.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offerDateList"></param>
        public void BindOfferDateDropDown(Dictionary<int, string> offerDateList)
        {
            ddlOfferDates.DataSource = offerDateList;
            ddlOfferDates.DataTextField = "Value";
            ddlOfferDates.DataValueField = "Key";
            ddlOfferDates.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spvList"></param>
        public void BindSPVList(DataTable spvList)
        {
            SPVNumber.Items.Clear();
            for (int i = 0; i < spvList.Rows.Count; i++)
            {
                SPVNumber.Items.Add(
                            new ListItem(spvList.Rows[i]["Value"].ToString(), spvList.Rows[i]["Key"].ToString())
                            );
            }
        }


        #endregion

    }
}
