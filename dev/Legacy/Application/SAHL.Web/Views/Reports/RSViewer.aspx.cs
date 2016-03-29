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
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Reports.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using System.Collections.Specialized;
using SAHL.Common.Web.UI.Controls;
using System.Globalization;
using SAHL.Common.DomainMessages;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Reports
{
    public partial class RSViewer : SAHLCommonBaseView, IRSViewer
    {
        #region Private Variables

        private IList<IReportParameter> _reportParms;
        private IEventList<IReportStatement> _reportStatements;
        private IDictionary<string, object> _ReportParameterValues;
        private List<string> _reportParameterDefaultValues;
        private IReportStatement _selectedReportStatement;
        private int _selectedReportType;
        private string _selectedReportName;
        private IList<string> _originalReportParameters;

        /// <summary>
        /// eNums for Report Parameter Types
        /// </summary>
        public enum ReportParameterTypes
        {
            eNone = 0,
            eBoolean = 1,
            eDateTime = 2,
            eFloat = 3,
            eInteger = 4,
            eString = 5,
            eSelectList = 6,
            eMultiValueString = 7,
            eMultiValueInt = 8,
            eMultiValueSelect = 9,
        }

        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!this.ShouldRunPage)
                return;                     
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!this.ShouldRunPage)
                return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDataReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tblReports_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // TODO: Uncomment when in use - removed for FxCop
            //if (e.Row.DataItem != null)
            //{
            //    IReportStatement rs = e.Row.DataItem as IReportStatement;
            //}
        }
        /// <summary>
        /// Event fired when user clicks on Cancel Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            if (onCancelButtonClicked != null)
                onCancelButtonClicked(sender, e);
        }
        /// <summary>
        /// Event fired when user clicks on View Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ViewReport_Click(object sender, EventArgs e)
        {
            ReportParametersEventArgs ReportArgs = new ReportParametersEventArgs();
            IReportStatement rs = null;

            if (_reportStatements == null || _reportStatements.Count <= 0)
                return;

            for (int y = 0; y < _reportStatements.Count; y++)
            {
                if (tblReports.SelectedIndex > -1 &&  tblReports.SelectedRow.Cells[0].Text == _reportStatements[y].Key.ToString())
                {
                    rs = _reportStatements[y];
                    _selectedReportStatement = rs;
                    _selectedReportType = rs.ReportType.Key;
                    _selectedReportName = rs.StatementName;
                    break;
                }
            }

            if (_reportParms != null)
            {                
                for (int i = 0; i < _reportParms.Count; i++)
                {
                    string ControlName = GetControlName(_reportParms[i]);
                    StringCollection SC = new StringCollection();
                    SC.AddRange(Request.Form.AllKeys);
                    if (SC.Contains(pnlParameters.NamingContainer.UniqueID + '$' + ControlName))
                    {                        
                        string FormStrValue = Request.Form[pnlParameters.NamingContainer.UniqueID + '$' + ControlName];
                        string msg = "Parameter '" + _reportParms[i].DisplayName + "' is required.";
                        if (!ValidateParameter(_reportParms[i], FormStrValue))
                        {
                            if (FormStrValue == null)
                                msg = "Parameter '" + _reportParms[i].DisplayName + "' contains an invalid value";

                            Messages.Add(new Error(msg, msg));
                        }
                        if (FormStrValue.Length == 0 
                            && _reportParms[i].ReportParameterType.Key != (int)SAHL.Common.Globals.ReportParameterTypes.String 
                            && _reportParms[i].ReportParameterType.Key != (int)SAHL.Common.Globals.ReportParameterTypes.MultiStringDropDown)
                        {
                            if (_reportParms[i].ReportParameterType.Key != (int)SAHL.Common.Globals.ReportParameterTypes.Float 
                                || _reportParms[i].ReportParameterType.Key != (int)SAHL.Common.Globals.ReportParameterTypes.Integer)
                            {
                                if (FormStrValue.ToString().Length > 0)
                                    ReportArgs.Parameters.Add(_reportParms[i], (object)int.Parse(FormStrValue.ToString()));
                                else
                                    ReportArgs.Parameters.Add(_reportParms[i], "0");
                            }
                            else
                                ReportArgs.Parameters.Add(_reportParms[i], FormStrValue);

                            continue;
                        }     
         
                        object FormValue = GetFormValue(FormStrValue, _reportParms[i].ReportParameterType);
                       
                        if (FormValue != null)
                        {                           
                            if (FormValue.ToString().Contains("\r\n") || FormValue.ToString().Contains(","))
                            {
                                // if this is a data report then replace the "\r\n" with ","
                                // if this is a sql report then replace the "\r\n" or "," with a "|"
                                string str = FormValue.ToString();
                                if (_selectedReportType == (int)SAHL.Common.Globals.ReportTypes.ReportingServicesReport)
                                {
                                    str = str.Replace("\r\n", "|");
                                    str = str.Replace(",", "|");
                                    str = str.TrimEnd(new char[] { '|' });
                                }
                                else
                                {
                                    str = str.Replace("\r\n", ",");
                                    str = str.TrimEnd(new char[] { ',' });
                                }
                                FormValue = str;
                            }
                        }
                        else
                        {
                            if (!ValidateParameter(_reportParms[i], FormValue))
                            {
                                if (FormValue == null)
                                    msg ="Parameter '" +  _reportParms[i].DisplayName + "' contains an invalid value";

                                Messages.Add(new Error(msg, msg));
                            }

                            FormValue = string.Empty;
                        }

                        ReportArgs.Parameters.Add(_reportParms[i], FormValue);
                    }
                }
                if (Messages.Count > 0)
                    return;
            }

            if (onViewButtonClicked != null)
                onViewButtonClicked(this.Page, ReportArgs);
        }

        private static bool ValidateParameter(IReportParameter p,object paramValue)
        {
            if (p.Required.HasValue && p.Required.Value)
            {
                if (paramValue == null || paramValue.ToString().Length == 0)
                    return false;
            }
            return true;
        }

        private static bool ValidateParameter(IReportParameter p, string paramValue)
        {
            if (p.Required.HasValue && p.Required.Value)
            {
                if (paramValue.Length == 0)
                    return false;
            }
            return true;
        }

        private static object GetFormValue(string FormStrValue, IReportParameterType reportParameterType)
        {
            switch (reportParameterType.Description)
            {
                case "Drop Down":
                    //return Convert.ToInt32(FormStrValue);
                    return FormStrValue;

                case "Boolean":
                    return Convert.ToBoolean(FormStrValue);
               
                case "DateTime":
                    DateTime result;
                    if (DateTime.TryParse(FormStrValue, CultureInfo.GetCultureInfo(SAHL.Common.Web.UI.Controls.Constants.CultureGb), DateTimeStyles.None, out result))
                        return result;
                    else
                        return null;
                
                case "Float":
                    return Convert.ToSingle(FormStrValue);

                case "Integer":
                    return Convert.ToInt32(FormStrValue);

                case "String":
                    return FormStrValue;

                case "Multi Integer Drop Down":
                case "Multi String Drop Down":
                    return FormStrValue;

                case "Drop Down using Description":
                    return FormStrValue;

                default:
                    return null;
            }
        }

        /// <summary>
        /// Event fired when user clicks on Export Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ExportReport_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Fires when user selects a group from Report group drop down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlReportGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            tblReports.Controls.Clear();
            
            if (OnReportsGroupChanged != null)
                OnReportsGroupChanged(this.Page, new KeyChangedEventArgs(ddlReportGroup.SelectedValue));
        }

        #region IRSViewer Members
        /// <summary>
        /// Enables/Disables  the Cancel Button
        /// </summary>

        public bool CancelButtonEnable
        {
            set { CancelButton.Enabled = value; }
        }

        /// <summary>
        /// Enables/Disables  the View Button
        /// </summary>
        public bool ViewButtonEnable
        {
            set { ViewReport.Enabled = value; }
        }

        /// <summary>
        /// Sets visibility of the Reports Grid
        /// </summary>
        public bool ReportsGridShow
        {
            set { tblReports.Visible = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool grdDataReportShow
        {
            set
            {
                grdDataReport.Visible = value;
                ReportTable.Visible = value;
            }
        }


        /// <summary>
        /// Sets visibility of the Parameters Panel
        /// </summary>
        public bool pnlParametersShow
        {
            set
            {
                pnlParameters.Visible = value;
                tblParameters.Visible = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>

        public string SelectedReportGroup
        {
            get { return ddlReportGroup.SelectedValue; }
            set { ddlReportGroup.SelectedValue = value; }
        }


        /// <summary>
        /// KeyChangedEventHandler for Selected Index change on Grid
        /// </summary>
        public event KeyChangedEventHandler OntblReportsSelectedIndexChanged;

        /// <summary>
        /// Binds the Report Groups to the Drop Down
        /// </summary>
        public void BindReportGroup(IEventList<IReportGroup> reportGroups)
        {
            ddlReportGroup.DataSource = reportGroups;
            ddlReportGroup.DataValueField = "Key";
            ddlReportGroup.DataTextField = "Description";
            ddlReportGroup.DataBind();
        }

        private static string GetControlName(IReportParameter Parameter)
        {
            switch (Parameter.ReportParameterType.Description)
            {
                case "Drop Down":
                case "Boolean":                     // ReportParameterTypeEnum.eBoolean:
                case "DateTime":                    // ReportParameterTypeEnum.eDateTime:
                case "Float":                       // ReportParameterTypeEnum.eFloat:
                case "Integer":                     // ReportParameterTypeEnum.eInteger:
                case "String":                      // ReportParameterTypeEnum.eString:
                case "Multi Integer Drop Down":     // ReportParameterTypeEnum.eMultiValueInt:
                case "Multi String Drop Down":      // ReportParameterTypeEnum.eMultiValueString:
                case "Drop Down using Description": // ReportParameterTypeEnum.eMultiValueSelect:
                    return Parameter.ParameterName;
            }

            throw new ArgumentOutOfRangeException("Parameter", "Invalid Report Parameter Type");
        }


        /// <summary>
        /// Dynamically builds the controls for the Parameters for the Selected Report
        /// </summary>
        public void BindReportParameterList(IList<IReportParameter> reportParms, IEventList<IReportStatement> reportstatements)
        {
            _reportParms = new List<IReportParameter>();
            foreach (object rp in reportParms)
            {
                _reportParms.Add(rp as IReportParameter);
            }
            _reportStatements = reportstatements;
        }

        /// <summary>
        /// Binds the ReportStatements to Grid for the selected group
        /// </summary>
        public void BindReportStatement(IEventList<IReportStatement> lstReports)
        {
            _reportStatements = lstReports;

            tblReports.Columns.Clear();
            tblReports.AutoGenerateColumns = false;

            tblReports.AddGridBoundColumn("Key", "ReportStatementKey", Unit.Percentage(0), HorizontalAlign.Left, false);
            tblReports.AddGridBoundColumn("ReportName", "Report Name", Unit.Percentage(33), HorizontalAlign.Left, true);
            tblReports.AddGridBoundColumn("Description", "Description", Unit.Percentage(66), HorizontalAlign.Left, true);

            tblReports.DataSource = lstReports;

            tblReports.DataBind();           
        }

        /// <summary>
        /// Fires when user selects  a report off grid
        /// </summary>
        protected void tblReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tblReports.SelectedIndex >= 0)
            {
                OntblReportsSelectedIndexChanged(sender, new KeyChangedEventArgs(tblReports.SelectedIndex));
            }
        }


        public event EventHandler onCancelButtonClicked;

        public event EventHandler<ReportParametersEventArgs> onViewButtonClicked;

        public event KeyChangedEventHandler OnReportsGroupChanged;

        public IReportStatement SelectedReportStatement
        {
            get { return _selectedReportStatement; }
        }


        public int SelectedReportType
        {
            get { return _selectedReportType; }
        }

        public string SelectedReportName
        {
            get { return _selectedReportName; }

            set 
            {
                for (int x = 0; x < tblReports.Rows.Count; x++)
                {
                    if (tblReports.Rows[x].Cells[0].Text == value)
                    {
                        tblReports.SelectedIndex = x;
                       
                        break;
                    }
                }
                if (tblReports.SelectedIndex >= 0)
                {
                    OntblReportsSelectedIndexChanged(this, new KeyChangedEventArgs(tblReports.SelectedIndex));
                }
            }
        }

        public IList<string> OriginalReportParameters
        {
            get
            {
                return _originalReportParameters;
            }
            set
            {
                _originalReportParameters = value;
            }
        }

        public int tblReportsSelectedIndex
        {
            get
            {
                if (Request.Form["ctl00$Main$tblReports_SELECTED"] != null)
                    return int.Parse(Request.Form["ctl00$Main$tblReports_SELECTED"]);
                else
                    return -1;
            }
            set
            {
                tblReports.SelectedIndex = value;
            }
        }

        public void BuildParameterControls(IList<IReportParameter> reportParms, IEventList<IReportStatement> reportstatements)
        {           
            tblParameters.Rows.Clear();            

            if (tblReports.SelectedIndexInternal == -1)
            {
                return;
            }
            if (!CancelButton.Enabled)
            {
                CancelButton.Enabled = true;
                ViewReport.Enabled = true;
            }

            if (reportParms != null && reportParms.Count > 0)
            {
                pnlParameters.Visible = true;

                TableRow tr;
                int x = 0;

                for (int i = 0; i <= reportParms.Count - 1; i++)
                {                                                        
                    tr = new TableRow();

                    // Add the Label
                    TableCell tcLabel = new TableCell();
                    IReportParameter rp = reportParms[x] as IReportParameter;
                    tcLabel.Text = rp.DisplayName;
                    tcLabel.CssClass = "TitleText";
                    tcLabel.Width = new Unit(20, UnitType.Percentage);

                    // Add the Input control 
                    TableCell tcInput = new TableCell();
                    Control inputControl = null;
                    HtmlGenericControl divHint = null;

                    // Get Parameter Values & Default Values
                    IReportParameter reportParameter = reportParms[i] as IReportParameter;
                    if (tblReports.Rows.Count > tblReports.SelectedIndexInternal )
                    {
                        // get valid values
                        _ReportParameterValues = reportParameter.ValidValues;

                        // get default values
                        ISqlReportParameter sqlReportParameter = reportParameter as ISqlReportParameter;
                        if (sqlReportParameter != null)
                        {
                            List<string> defaultValues = new List<string>();
                            foreach (string k in sqlReportParameter.DefaultValues)
                            {
                                defaultValues.Add(k);
                            }

                            _reportParameterDefaultValues = defaultValues;
                        }
                    }

                    bool reportParameterRequired = reportParameter.Required.HasValue ? reportParameter.Required.Value : false;

                    switch (reportParameter.ReportParameterType.Key)
                    {
                        case (int)SAHL.Common.Globals.ReportParameterTypes.DropDown:    // ReportParameterTypeEnum.eSelectList
                            #region DropDown
                            SAHLDropDownList ddlDropDown = new SAHLDropDownList();
                            ddlDropDown.Mandatory = reportParameterRequired;

                            if (_ReportParameterValues != null)
                            {
                                foreach (string desc in _ReportParameterValues.Keys)
                                {
                                    string key = _ReportParameterValues[desc].ToString();
                                    ddlDropDown.Items.Add(new ListItem(key, desc));
                                }
                            }                           
                            if (_reportParameterDefaultValues != null && _reportParameterDefaultValues.Count > 0)
                            {
                                for (int p = 0; p < _reportParameterDefaultValues.Count; p++)
                                {
                                    for(int w=0;w<ddlDropDown.Items.Count;w++)
                                    {
                                        if (ddlDropDown.Items[w].Value == _reportParameterDefaultValues[p])
                                        {
                                            ddlDropDown.SelectedValue = ddlDropDown.Items[w].Value;
                                            break;
                                        }
                                  }
                                }
                            }

                            if (_originalReportParameters != null && _originalReportParameters.Count > x)
                            {
                                ddlDropDown.SelectedValue = _originalReportParameters[x];
                            }

                            inputControl = ddlDropDown;
                            #endregion
                            break;
                        case (int)SAHL.Common.Globals.ReportParameterTypes.Boolean:     // ReportParameterTypeEnum.eBoolean
                            #region Boolean
                            CheckBox cBox = new CheckBox();

                            //todo: needs to be tested
                            if (_reportParameterDefaultValues != null && _reportParameterDefaultValues.Count >0)
                            {
                                if (_reportParameterDefaultValues[0] == "1")
                                    cBox.Checked = true;
                                else
                                    cBox.Checked = false;
                            }

                            if (_originalReportParameters != null && _originalReportParameters.Count > x)
                            {
                                cBox.Checked = bool.Parse(_originalReportParameters[x]);
                            }
                            inputControl = cBox;
                            #endregion
                            break;
                        case (int)SAHL.Common.Globals.ReportParameterTypes.DateTime:    // ReportParameterTypeEnum.eDateTime:
                            #region DateTime
                            SAHLDateBox dateBox = new SAHLDateBox();                           
                                                     
                            //todo: needs to be tested
                            if (_reportParameterDefaultValues != null && _reportParameterDefaultValues.Count > 0)
                            {
                                DateTime dt1;

                                if (DateTime.TryParse(_reportParameterDefaultValues[0], CultureInfo.GetCultureInfo(SAHL.Common.Web.UI.Controls.Constants.CultureGb), DateTimeStyles.None, out dt1))
                                {
                                    dateBox.Date = dt1;
                                }
                            }

                            if (_originalReportParameters != null && _originalReportParameters.Count > x)
                            {
                                DateTime dt2;

                                if (DateTime.TryParse(_originalReportParameters[x], CultureInfo.GetCultureInfo(SAHL.Common.Web.UI.Controls.Constants.CultureGb), DateTimeStyles.None, out dt2))
                                {
                                    dateBox.Date = dt2;
                                }
                            }

                            dateBox.Mandatory = reportParameterRequired;
                            inputControl = dateBox;
                            #endregion
                            break;
                        case (int)SAHL.Common.Globals.ReportParameterTypes.Float:       // ReportParameterTypeEnum.eFloat:
                            #region Float
                            SAHLCurrencyBox txtFloat = new SAHLCurrencyBox();

                            txtFloat.Mandatory = reportParameterRequired;
                            
                            //todo: needs to be tested
                            if (_reportParameterDefaultValues != null && _reportParameterDefaultValues.Count > 0)
                                txtFloat.Text = _reportParameterDefaultValues[0];

                            if (_originalReportParameters != null && _originalReportParameters.Count > x)
                                txtFloat.Text = _originalReportParameters[x];

                            inputControl = txtFloat;
                            #endregion
                            break;
                        case (int)SAHL.Common.Globals.ReportParameterTypes.Integer:     // ReportParameterTypeEnum.eInteger:
                            #region Integer
                            SAHLTextBox txtInteger = new SAHLTextBox();

                            txtInteger.Mandatory = reportParameterRequired;
                            txtInteger.DisplayInputType = InputType.Number;
                           
                            if (_reportParameterDefaultValues != null && _reportParameterDefaultValues.Count > 0)
                                txtInteger.Text = _reportParameterDefaultValues[0];

                            if (_originalReportParameters != null && _originalReportParameters.Count > x)
                                txtInteger.Text = _originalReportParameters[x];

                            inputControl = txtInteger;
                            #endregion
                            break;
                        case (int)SAHL.Common.Globals.ReportParameterTypes.String:      // ReportParameterTypeEnum.eString:
                            #region String
                            SAHLTextBox txtString = new SAHLTextBox();
                            
                            txtString.Mandatory = reportParameterRequired;
                            txtString.DisplayInputType = InputType.Normal;

                            //todo: needs to be tested
                            if (_reportParameterDefaultValues != null && _reportParameterDefaultValues.Count > 0)
                                txtString.Text = _reportParameterDefaultValues[0];

                            if (_originalReportParameters != null && _originalReportParameters.Count > x)
                                txtString.Text = _originalReportParameters[x];

                            inputControl = txtString;
                            #endregion
                            break;
                        case (int)SAHL.Common.Globals.ReportParameterTypes.MultiStringDropDown:    // ReportParameterTypeEnum.eMultiValueInt:
                        case (int)SAHL.Common.Globals.ReportParameterTypes.MultiIntegerDropDown:   // ReportParameterTypeEnum.eMultiValueString:
                            #region MultiValue
                            SAHLTextBox txtMulti = new SAHLTextBox();

                            if (reportParameter.ReportParameterType.Key == (int)SAHL.Common.Globals.ReportParameterTypes.MultiIntegerDropDown)
                                txtMulti.DisplayInputType = InputType.Number;
                            else
                                txtMulti.DisplayInputType = InputType.Normal;

                            txtMulti.Mandatory = reportParameterRequired;

                            txtMulti.TextMode = TextBoxMode.MultiLine;
                            txtMulti.Rows = 4;

                            //todo: needs to be tested
                            if (_reportParameterDefaultValues != null && _reportParameterDefaultValues.Count > 0)
                            {
                                for (int p = 0; p < _reportParameterDefaultValues.Count; p++)
                                {
                                    txtMulti.Text += _reportParameterDefaultValues[p] + "\r\n";
                                }
                            }

                            if (_originalReportParameters != null && _originalReportParameters.Count > x)
                            {
                                txtMulti.Text = _originalReportParameters[x].Replace("|", "\r\n");
                                txtMulti.Text = _originalReportParameters[x].Replace(",", "\r\n");
                            }

                            // wrap the input control in a div so that we can float left and have the "hint" div next to it on the right
                            HtmlGenericControl divInput = new HtmlGenericControl("div");
                            divInput.Style.Add("float", "left");

                            divHint = new HtmlGenericControl("div");
                            divHint.Attributes.Add("class", "cellDisplay");
                            divHint.InnerHtml = @"<strong>Tip:</strong> Multiple " + tcLabel.Text + @" values must be entered on <em>separate lines</em>.";
                            divHint.Style.Add("float", "left");
                            divHint.Style.Add(HtmlTextWriterStyle.Padding, "5px 0px 0px 5px"); // pad 5px top and left

                            // set the Id of the input control to the parametername
                            txtMulti.ID = GetControlName(reportParameter);
                            // add the textbox control to the div
                            divInput.Controls.Add(txtMulti);
                            // set the whole div to the generic input control
                            inputControl = divInput;
                            #endregion
                            break;
                        case (int)SAHL.Common.Globals.ReportParameterTypes.DropDownusingDescription: // ReportParameterTypeEnum.eMultiValueSelect:
                            #region MultiValueSelect
                            SAHLCheckboxList chkList = new SAHLCheckboxList();

                            if (_ReportParameterValues != null)
                            {
                                foreach (string desc in _ReportParameterValues.Keys)
                                {
                                    string key = _ReportParameterValues[desc].ToString();
                                    chkList.Items.Add(new ListItem(key, desc));
                                }
                            }

                            chkList.Height = new Unit(100, UnitType.Pixel);
                            chkList.Width = new Unit(60, UnitType.Percentage);

                            //todo: needs to be tested
                            if (_reportParameterDefaultValues != null && _reportParameterDefaultValues.Count > 0)
                            {
                                for (int p = 0; p < _reportParameterDefaultValues.Count; p++)
                                {
                                    for (int w = 0; w < chkList.Items.Count; w++)
                                    {
                                        if (chkList.Items[w].Value == _reportParameterDefaultValues[p])
                                        {
                                            chkList.Items[w].Selected = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (_originalReportParameters != null && _originalReportParameters.Count > x)
                                chkList.SelectedValue = _originalReportParameters[x];

                            inputControl = chkList;
                            #endregion
                            break;
                    }

                    if (inputControl != null)
                    {
                        string controlID = GetControlName(reportParameter);
                        // if our input control is wrapped in a div then set the name with the 'div' prefix
                        inputControl.ID = divHint != null ? "div" + controlID : controlID;

                        tcInput.Controls.Add(inputControl);
                        // if we have a hint div, then add it to the table cell
                        if (divHint != null)
                            tcInput.Controls.Add(divHint);
                    }                  
                    
                    tr.Cells.Add(tcLabel);
                    tr.Cells.Add(tcInput);
                    tblParameters.Rows.Add(tr);

                    x++;
                }
            }
            else
            {
                pnlParameters.Visible = false;
            }
        }

      
        public int SelectedReportGroupIndex
        {
            get
            {
                if (Request.Form["ctl00$Main$ddlReportGroup"] != null)
                {
                    int selReportGroup = -1;
                    if (int.TryParse(Request.Form["ctl00$Main$ddlReportGroup"], out selReportGroup))
                        return selReportGroup;
                    else
                        return -1;
                }
                else
                    return -1;                
            }
        }    

        #endregion
    }
}

