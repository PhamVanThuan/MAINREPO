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
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Web.UI.Controls;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Events;
using System.Linq;

namespace SAHL.Web.Views.Administration
{
    public partial class ProductManagement : SAHLCommonBaseView, IProductManagement
    {
        ArrayList arrMaxLoanAmounts = new ArrayList();

        private int _selectedOriginationSourceKey;
        private int _selectedProductKey;
        private bool _updateMode;
        private ILookupRepository _lookupRepo;

        #region Grid Colors
        public enum GridColor
        {
            [StringValue("#c0c0c0")] // grey
            Blank,
            [StringValue("#000000")] // black
            Foreground,
            [StringValue("#ff99cc")] // pink
            Salaried,
            [StringValue("#ffcc00")] // orange
            SelfEmployed,
            [StringValue("#1ee673")] // greenish
            OtherEmploy,
            [StringValue("#ffff99")] // pale yellow
            MaxLoan1,
            [StringValue("#ffffff")] // white
            MaxLoan2,
            [StringValue("#ccffff")] // light blue
            MaxLoan3,
            [StringValue("#1ee673")] // greenish
            Category0,
            [StringValue("#ffcc99")] // salmon
            Category1,
            [StringValue("#ccffff")] // light blue
            Category2,
            [StringValue("#99ccff")] // darker blue
            Category3,
            [StringValue("#ffcc99")] // salmon
            Category4,
            [StringValue("#ffcc99")] // salmon
            Category5,
            [StringValue("#ff0000")] // salmon
            Exception
        }
        #endregion

        private enum GridColumnPositions
        {
            CreditMatrixKey = 0,
            EmploymentTypeKey = 1,
            EmploymentType = 2,
            MaxLoanAmount = 3,
            MortgageLoanPurposeKey = 4,
            MortgageLoanPurpose = 5,
            ExceptionCriteria = 6
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            chklLinkedOSP.Attributes.Add("onclick", "CheckListBoxClick()");

            RegisterClientJavascript();

            foreach (ListItem li in chklLinkedOSP.Items)
            {
                if (li.Selected)
                    li.Attributes.Add("class", "CheckListBoxSelected");
                else
                    li.Attributes.Add("class", "CheckListBoxUnSelected");

            }

            btnLoad.Visible = false;
            btnSubmit.Visible = false;
        }

         protected void CreditCriteriaGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell tc in cells)
                {
                    if (tc.Text.Contains("Category"))
                    {
                        tc.ColumnSpan = 2;
                        tc.HorizontalAlign = HorizontalAlign.Center;
                    }
                    if (tc.Text.Contains("-"))
                        tc.Visible = false;
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_lookupRepo == null)
                    _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                // Employment Type
                cells[(int)GridColumnPositions.EmploymentType].Text = _lookupRepo.EmploymentTypes.ObjectDictionary[cells[(int)GridColumnPositions.EmploymentTypeKey].Text].Description;
                // Loan Purpose
                cells[(int)GridColumnPositions.MortgageLoanPurpose].Text = _lookupRepo.MortgageLoanPurposes.ObjectDictionary[cells[(int)GridColumnPositions.MortgageLoanPurposeKey].Text].Description;

                #region Colors
                switch (Convert.ToInt32(cells[(int)GridColumnPositions.EmploymentTypeKey].Text))
                {
                    case 1: // Salaried
                        cells[(int)GridColumnPositions.EmploymentType].Style["background-color"] = StringEnum.GetStringValue(GridColor.Salaried);
                        break;
                    case 2: // Self Employed
                        cells[(int)GridColumnPositions.EmploymentType].Style["background-color"] = StringEnum.GetStringValue(GridColor.SelfEmployed);
                        break;
                    default:
                        cells[(int)GridColumnPositions.EmploymentType].Style["background-color"] = StringEnum.GetStringValue(GridColor.OtherEmploy);
                        break;
                }

                if (arrMaxLoanAmounts.Count > 0 && Convert.ToInt32(cells[(int)GridColumnPositions.MaxLoanAmount].Text) == Convert.ToInt32(arrMaxLoanAmounts[0]))
                {
                    cells[(int)GridColumnPositions.MaxLoanAmount].Style["background-color"] = StringEnum.GetStringValue(GridColor.MaxLoan1);
                    cells[(int)GridColumnPositions.MortgageLoanPurpose].Style["background-color"] = StringEnum.GetStringValue(GridColor.MaxLoan1);
                }
                else if (arrMaxLoanAmounts.Count > 1 && Convert.ToInt32(cells[(int)GridColumnPositions.MaxLoanAmount].Text) == Convert.ToInt32(arrMaxLoanAmounts[1]))
                {
                    cells[(int)GridColumnPositions.MaxLoanAmount].Style["background-color"] = StringEnum.GetStringValue(GridColor.MaxLoan2);
                    cells[(int)GridColumnPositions.MortgageLoanPurpose].Style["background-color"] = StringEnum.GetStringValue(GridColor.MaxLoan2);
                }
                else
                {
                    cells[(int)GridColumnPositions.MaxLoanAmount].Style["background-color"] = StringEnum.GetStringValue(GridColor.MaxLoan3);
                    cells[(int)GridColumnPositions.MortgageLoanPurpose].Style["background-color"] = StringEnum.GetStringValue(GridColor.MaxLoan3);
                }

                //Exception columns
                if (cells[(int)GridColumnPositions.ExceptionCriteria].Text == "Y")
                    cells[(int)GridColumnPositions.ExceptionCriteria].Style["background-color"] = StringEnum.GetStringValue(GridColor.Exception);

                // Set the colors of the LTV / PTI cells
                int icol = 0;
                foreach (TableCell tc in cells)
                {
                    if (icol > (int)GridColumnPositions.MortgageLoanPurpose)
                    {
                        if (CreditCriteriaGrid.Columns[icol].HeaderText.Contains("0"))
                            tc.Style["background-color"] = StringEnum.GetStringValue(GridColor.Category0);
                        if (CreditCriteriaGrid.Columns[icol].HeaderText.Contains("1"))
                            tc.Style["background-color"] = StringEnum.GetStringValue(GridColor.Category1);
                        if (CreditCriteriaGrid.Columns[icol].HeaderText.Contains("2"))
                            tc.Style["background-color"] = StringEnum.GetStringValue(GridColor.Category2);
                        if (CreditCriteriaGrid.Columns[icol].HeaderText.Contains("3"))
                            tc.Style["background-color"] = StringEnum.GetStringValue(GridColor.Category3);
                        if (CreditCriteriaGrid.Columns[icol].HeaderText.Contains("4"))
                            tc.Style["background-color"] = StringEnum.GetStringValue(GridColor.Category4);
                        if (CreditCriteriaGrid.Columns[icol].HeaderText.Contains("5"))
                            tc.Style["background-color"] = StringEnum.GetStringValue(GridColor.Category5);

                        // blank values
                        if (tc.Text == "&nbsp;")
                            tc.Style["background-color"] = StringEnum.GetStringValue(GridColor.Blank);
                    }

                    icol++;
                }
                #endregion
            }
        }

        protected void chklLinkedOSP_DataBound(object sender, EventArgs e)
        {
            DataTable dtOSP = (DataTable)chklLinkedOSP.DataSource;
            int irow = 0;

            if (dtOSP != null)
            {
                foreach (DataRow dr in dtOSP.Rows)
                {
                    chklLinkedOSP.Items[irow].Selected = Convert.ToBoolean(dr["Linked"]);

                    irow++;
                }
            }
        }

        protected void CreditCriteriaGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void CreditCriteriaGrid_DataBound(object sender, EventArgs e)
        {
        }

        protected void ddlOriginationSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedOriginationSourceKey = Convert.ToInt32(ddlOriginationSource.SelectedValue);

            KeyChangedEventArgs keyChangedEventArgs = new KeyChangedEventArgs(_selectedOriginationSourceKey);

            onOriginationSource_SelectedIndexChanged(sender, keyChangedEventArgs);
        }

        protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedProductKey = Convert.ToInt32(ddlProduct.SelectedValue);

            KeyChangedEventArgs keyChangedEventArgs = new KeyChangedEventArgs(_selectedProductKey);

            onProduct_SelectedIndexChanged(sender, keyChangedEventArgs);
        }
        
        protected void btnLoad_Click(object sender, EventArgs e)
        {
            _selectedOriginationSourceKey = Convert.ToInt32(ddlOriginationSource.SelectedValue);
            _selectedProductKey = Convert.ToInt32(ddlProduct.SelectedValue);
           
            onLoadButtonClicked(sender, e);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Check at least one SPV selected.
            StringBuilder sb = new StringBuilder();
            foreach (ListItem li in chklLinkedOSP.Items)
            {
                if (li.Selected)
                    sb.Append(li.Value + ",");
            }
            //if (sb.Length == 0)
            //{
            //    SPVNumberVal.IsValid = false;
            //    return;
            //}

            onSubmitButtonClicked(sender, e);
        }

        private void RegisterClientJavascript()
        {
            // string sFormName = "window." + this.Form.Name;

            StringBuilder sbJavascript = new StringBuilder();

            sbJavascript.AppendLine("function CheckListBoxClick ()");
            sbJavascript.AppendLine("{");
            sbJavascript.AppendLine("var checklistbox = document.getElementById('" + this.chklLinkedOSP.ClientID + "');");
            sbJavascript.AppendLine("var controltype = this.event.srcElement.type;");
            sbJavascript.AppendLine("var controlvalue = this.event.srcElement.value;");
            //sbJavascript.AppendLine("x = checklistbox.options[checklistbox.selectedIndex].value;");
            sbJavascript.AppendLine("if (controltype=='checkbox')");
            sbJavascript.AppendLine("{");
            sbJavascript.AppendLine("if (controlvalue=='on')");
            sbJavascript.AppendLine("{");
            sbJavascript.AppendLine("alert('Item has been checked');");
            sbJavascript.AppendLine("}");
            sbJavascript.AppendLine("else");
            sbJavascript.AppendLine("{");
            sbJavascript.AppendLine("alert('Item has been un-checked');");
            sbJavascript.AppendLine("}");
            sbJavascript.AppendLine("}");
            sbJavascript.AppendLine("}");

            if (!Page.ClientScript.IsClientScriptBlockRegistered("CheckListBoxClick"))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CheckListBoxClick", sbJavascript.ToString(), true);
        }

        public event EventHandler onLoadButtonClicked;

        public event EventHandler onSubmitButtonClicked;

        public event EventHandler onOriginationSource_SelectedIndexChanged;

        public event EventHandler onProduct_SelectedIndexChanged;

        public int SelectedOriginationSourceKey
        {
            get
            {
                return Convert.ToInt32(ddlOriginationSource.SelectedValue);
            }
            set
            {
                _selectedOriginationSourceKey = value;
            }
        }

        public int SelectedProductKey
        {
            get
            {
                return Convert.ToInt32(ddlProduct.SelectedValue);
            }
            set
            {
                _selectedProductKey = value;
            }
        }

        public bool UpdateMode
        {
            get
            {
                return _updateMode;
            }
            set
            {
                _updateMode = value;
            }
        }

        public void BindCreditCriteria(DataSet dsCreditCriteria)
        {
			var creditMatrixCategoryCount = dsCreditCriteria.Tables["CreditCriteria"].Columns.Cast<DataColumn>().Where(column => column.ColumnName.StartsWith("CCKey_")).Count();
            arrMaxLoanAmounts.Clear();
            int iMaxLoanAmount = 0;
            foreach (DataRow dr in dsCreditCriteria.Tables["CreditCriteria"].Rows)
            {
                // Get the Max Loan Amounts
                if (Convert.ToInt32(dr["MaxLoanAmount"]) != iMaxLoanAmount)
                {
                    arrMaxLoanAmounts.Add(Convert.ToInt32(dr["MaxLoanAmount"]));
                    iMaxLoanAmount = Convert.ToInt32(dr["MaxLoanAmount"]);
                }
            }

            CreditCriteriaGrid.Columns.Clear();

            CreditCriteriaGrid.FixedHeader = false;

            CreditCriteriaGrid.AddGridBoundColumn("CreditMatrixKey", "", Unit.Pixel(100), HorizontalAlign.Left, false);
            CreditCriteriaGrid.AddGridBoundColumn("EmploymentTypeKey", "", Unit.Pixel(100), HorizontalAlign.Left, false);
            CreditCriteriaGrid.AddGridBoundColumn("", "Employment", Unit.Pixel(100), HorizontalAlign.Left, true);
            CreditCriteriaGrid.AddGridBoundColumn("MaxLoanAmount", "R##,###,##0", GridFormatType.GridNumber, "Max Loan", false, Unit.Pixel(100), HorizontalAlign.Center, true);
            CreditCriteriaGrid.AddGridBoundColumn("MortgageLoanPurposeKey", "", Unit.Pixel(100), HorizontalAlign.Left, false);
            CreditCriteriaGrid.AddGridBoundColumn("", "Loan Purpose", Unit.Pixel(100), HorizontalAlign.Left, true);
            CreditCriteriaGrid.AddGridBoundColumn("ExceptionCriteria", "Is Exception", Unit.Pixel(100), HorizontalAlign.Center, true);

			for (int icol = 0; icol < creditMatrixCategoryCount; icol++)
            {
                // if this is the last colum then set to 99
                if (icol == creditMatrixCategoryCount - 1)
                    icol = 99;

                CreditCriteriaGrid.AddGridBoundColumn("CCKey_" + icol, "", Unit.Percentage(0), HorizontalAlign.Left, false);
                CreditCriteriaGrid.AddGridBoundColumn("LTV_" + icol, "##0", GridFormatType.GridNumber, "Category " + icol, false, Unit.Pixel(50), HorizontalAlign.Center, true);
                CreditCriteriaGrid.AddGridBoundColumn("PTI_" + icol, "##0", GridFormatType.GridNumber, String.Format(@"{0}{1}", "-", icol), false, Unit.Pixel(50), HorizontalAlign.Center, true);
            }

            CreditCriteriaGrid.Columns[(int)GridColumnPositions.MaxLoanAmount].HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

            if (dsCreditCriteria.Tables["CreditCriteria"] != null && dsCreditCriteria.Tables["CreditCriteria"].Rows.Count > 0)
                CreditCriteriaGrid.DataSource = dsCreditCriteria.Tables["CreditCriteria"];
            else
                CreditCriteriaGrid.DataSource = null;

            CreditCriteriaGrid.DataBind();

            // Linked OSP's
            chklLinkedOSP.ClearSelection();
            chklLinkedOSP.DataSource = dsCreditCriteria.Tables["LinkedOSP"];
            chklLinkedOSP.DataValueField = "OriginationSourceProductKey";
            chklLinkedOSP.DataTextField = "OriginationSourceProduct";

            if (CreditCriteriaGrid.Rows.Count <= 0)
                chklLinkedOSP.DataSource = null;

            chklLinkedOSP.DataBind();

        }

        public void BindOriginationSources(System.Collections.Generic.IDictionary<string, string> originationSource, string defaultValue, bool pleaseSelect)
        {
            ddlOriginationSource.DataSource = originationSource;
            ddlOriginationSource.DataBind();
            if (pleaseSelect)
                ddlOriginationSource.VerifyPleaseSelect();

            // Set the default value if supplied
            if (!String.IsNullOrEmpty(defaultValue))
                ddlOriginationSource.SelectedValue = defaultValue;
        }

        public void BindProducts(System.Collections.Generic.IDictionary<string, string> product, string defaultValue, bool pleaseSelect)
        {
            ddlProduct.DataSource = product;
            ddlProduct.DataBind();
            if (pleaseSelect)
                ddlProduct.VerifyPleaseSelect();

            // Set the default value if supplied
            if (!String.IsNullOrEmpty(defaultValue))
                ddlProduct.SelectedValue = defaultValue;
        }
    }
}
