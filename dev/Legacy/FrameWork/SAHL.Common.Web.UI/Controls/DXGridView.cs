using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.Web.ASPxGridView;
using System.Data;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using DevExpress.Utils;

namespace SAHL.Common.Web.UI.Controls
{
    public class DXGridView : ASPxGridView
    {
        public const string SelectedIndexChangedUpdateEdit = "function(s,e){grid.UpdateEdit()}";
        public const string RowClickEditRow = "function(s, e) {s.StartEditRow(e.visibleIndex);}";
        private GridPostBackType _postBackType = GridPostBackType.DoubleClick;

        #region Overridden Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // this is a horrible HACK - for some reason when grouping is used and a CBO item is clicked, 
            // the grid falls over with an internal error, so this just clears out any bindings
            IViewBase view = Page as IViewBase;
            if (view != null && view.IsMenuPostBack)
            {
                DataSource = new DataTable();
                DataBind();
            }

            AutoGenerateColumns = false;
            //Settings.ShowTitlePanel = true;
            Border.BorderWidth = Unit.Pixel(2);
            Styles.AlternatingRow.Enabled = DefaultBoolean.True;

            //SettingsBehavior.AllowMultiSelection = false;
            SettingsBehavior.AllowSelectSingleRowOnly = true;

            this.HtmlDataCellPrepared += new ASPxGridViewTableDataCellEventHandler(DXGridView_HtmlDataCellPrepared);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            RegisterClientScript();

            // make the cursor a pointer on rows if the postback type is anything other than none
            if (PostBackType != GridPostBackType.None)
            {
                Styles.Row.CssClass = String.Format("{0} {1}", Styles.Row.CssClass, " action").Trim();
            }
        }

        public override void DataBind()
        {
            base.DataBind();
            if (IsCallback)
                RegisterClientScript();
        }

        protected override void DoCallBackPostBack(string eventArgument)
        {
            // for some reason the dexexpress control thows an exception here 
            // which is why we have to override and catch it.
            // this never happened in v8.2 but is happening in 9.2
            // this is a hack but it works...see trac ticket #13941
            try
            {
                base.DoCallBackPostBack(eventArgument);
            }
            catch
            {
            }
        }
        #endregion

        #region Event Handlers

        private void DXGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            DXGridViewFormattedTextColumn column = e.DataColumn as DXGridViewFormattedTextColumn;
            if (column != null)
            {
                string text = "";
                if (e.CellValue != null)
                    text = e.CellValue.ToString();

                // check that the cell is not empty
                if (String.IsNullOrEmpty(text) || text.Equals("&nbsp;"))
                {
                    if (column.Format == GridFormatType.GridDate)
                        e.Cell.Text = "-";
                    return;
                }

                // apply the formatting
                switch (column.Format)
                {
                    case GridFormatType.GridNumber:
                    case GridFormatType.GridCurrency:
                    case GridFormatType.GridRate:
                    case GridFormatType.GridRate3Decimal:
                        e.Cell.Text = Double.Parse(text).ToString(column.FormatString);
                        break;
                    case GridFormatType.GridDate:
                    case GridFormatType.GridDateTime:
                        e.Cell.Text = DateTime.Parse(text).ToString(column.FormatString);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets up the client-script used by the grid.
        /// </summary>
        private void RegisterClientScript()
        {
            StringBuilder sb = new StringBuilder();
            string jsEventHandler = "function(grid, eventArgs) {";
            string jsClearRows = "\t grid._selectAllRowsOnPage(false);";
            string jsSelectRow = "\t grid.SelectRow(eventArgs.visibleIndex, true);";
            string jsPostBack = "\t grid.SendPostBack(eventArgs);";

            if (PostBackType != GridPostBackType.None)
            {
                // row click client-side event
                sb.Append(jsEventHandler);
                if (SettingsBehavior.AllowSelectSingleRowOnly)
                    sb.Append(jsClearRows);
                sb.Append(jsSelectRow);
                if (PostBackType == GridPostBackType.SingleClick)
                    sb.Append(jsPostBack);
                sb.Append("}");
                ClientSideEvents.RowClick = sb.ToString();

                // double click client-side events
                if (PostBackType == GridPostBackType.DoubleClick)
                {
                    sb = new StringBuilder();
                    sb.Append(jsEventHandler);
                    sb.Append(jsPostBack);
                    sb.Append("}");
                    ClientSideEvents.RowDblClick = sb.ToString();
                }
            }

            //// if we do a callback - clear all the rows if we don't allow multi-select
            //if (SettingsBehavior.AllowSelectSingleRowOnly)
            //{
            //    sb = new StringBuilder();
            //    sb.Append(jsEventHandler);
            //    sb.Append(jsClearRows);
            //    sb.Append("}");
            //    ClientSideEvents.BeginCallback = sb.ToString();
            //}
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Exposed for convenience - this gets the Key Value of the selected row when only single row 
        /// selection (the default) is enabled.  If no KeyFieldName is selected or the 
        /// selected row count is not one, this will return null.
        /// </summary>
        public string SelectedKeyValue
        {
            get
            {
                if (String.IsNullOrEmpty(KeyFieldName))
                    return null;

                List<object> selectedValues = GetSelectedFieldValues(KeyFieldName);
                // if there is more than one selected value then return the last one selected
                // this may be the case because re-binding the grid doesnt reset the selections
                // you have to explicitly call gridview.Selection.UnselectAll(); after DataBind
                if (selectedValues.Count >= 1)
                    return selectedValues[selectedValues.Count - 1].ToString();

                return null;
            }
        }

        /// <summary>
        /// Gets/sets the way the grid posts back when rows are selected.  This defaults to 
        /// <see cref="GridPostBackType.DoubleClick"/>.
        /// </summary>
        public GridPostBackType PostBackType
        {
            get { return _postBackType; }
            set
            {
                switch (value)
                {
                    case GridPostBackType.DoubleClickWithClientSelect:
                    case GridPostBackType.SingleAndDoubleClick:
                        throw new NotSupportedException("These click types are obsolete and will be removed");
                }
                _postBackType = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="caption"></param>
        /// <param name="width"></param>
        /// <param name="formatType"></param>
        /// <param name="format"></param>
        /// <param name="align"></param>
        /// <param name="visible"></param>
        /// <param name="readOnly"></param>
        public void AddGridColumn(string fieldName, string caption, int width, GridFormatType formatType, string format, HorizontalAlign align, bool visible, bool readOnly)
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

                    this.Columns.Add(colDate);
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
                    this.Columns.Add(colText);
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="caption"></param>
        /// <param name="width"></param>
        /// <param name="align"></param>
        /// <param name="visible"></param>
        /// <param name="readOnly"></param>
        public void AddGridDateColumn(string fieldName, string caption, int width, HorizontalAlign align, bool visible, bool readOnly)
        {
            DXGridViewDateColumn col = new DXGridViewDateColumn();
            col.FieldName = fieldName;
            col.Caption = caption;
            col.Width = Unit.Percentage(width);
            col.Visible = visible;
            col.CellStyle.HorizontalAlign = align;
            col.ReadOnly = readOnly;
            col.PropertiesDateEdit.DropDownButton.Visible = false;
            this.Columns.Add(col);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="caption"></param>
        /// <param name="width"></param>
        /// <param name="align"></param>
        /// <param name="visible"></param>
        /// <param name="dataSource"></param>
        /// <param name="textField"></param>
        /// <param name="valueField"></param>
        public void AddGridCommandColumnComboBox(string fieldName, string caption, int width, HorizontalAlign align, bool visible, object dataSource, string textField, string valueField)
        {
            GridViewDataComboBoxColumn col = new GridViewDataComboBoxColumn();
            col.PropertiesComboBox.DataSource = dataSource;
            col.PropertiesComboBox.TextField = textField;
            col.PropertiesComboBox.ValueField = valueField;
            col.PropertiesComboBox.ClientSideEvents.SelectedIndexChanged = SelectedIndexChangedUpdateEdit;
            col.ReadOnly = false;
            col.FieldName = fieldName;
            col.Caption = caption;
            col.Width = Unit.Percentage(width);
            col.Visible = visible;
            col.CellStyle.HorizontalAlign = align;
            this.Columns.Add(col);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="align"></param>
        /// <param name="visible"></param>
        public void AddGridCheckBoxColumn(string caption, HorizontalAlign align, bool visible)
        {
            GridViewCommandColumn col = new GridViewCommandColumn();
            col.ShowSelectCheckbox = true;
            col.Caption = caption;
            col.Visible = visible;
            col.CellStyle.HorizontalAlign = align;
            this.Columns.Add(col);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="width"></param>
        /// <param name="align"></param>
        /// <param name="visible"></param>
        /// <param name="textField"></param>
        /// <param name="postbackOnCheckedChanged"></param>
        /// <param name="readOnly"></param>
        public void AddGridCheckBoxColumn(string caption, int width, HorizontalAlign align, bool visible, string textField, bool postbackOnCheckedChanged, bool readOnly)
        {
            DXGridViewCheckBoxColumn col = new DXGridViewCheckBoxColumn();
            col.FieldName = textField;
            col.Caption = caption;
            col.Visible = visible;
            col.CellStyle.HorizontalAlign = align;
            col.ReadOnly = readOnly;
            col.Width = Unit.Percentage(width);
            // only wire this up if a value has been passed
            if (postbackOnCheckedChanged)
                col.PropertiesCheckEdit.ClientSideEvents.CheckedChanged = SelectedIndexChangedUpdateEdit;

            this.Columns.Add(col);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="caption"></param>
        /// <param name="width"></param>
        /// <param name="align"></param>
        /// <param name="visible"></param>
        public void AddGridCommandColumnDateEdit(string fieldName, string caption, int width, HorizontalAlign align, bool visible)
        {
            GridViewDataDateColumn col = new GridViewDataDateColumn();
            col.PropertiesDateEdit.PopupVerticalAlign = DevExpress.Web.ASPxClasses.PopupVerticalAlign.Below;
            col.PropertiesDateEdit.DropDownButton.Enabled = true;
            col.PropertiesDateEdit.AllowUserInput = true;
            col.ReadOnly = false;
            col.FieldName = fieldName;
            col.Caption = caption;
            col.Width = Unit.Percentage(width);
            col.Visible = visible;
            col.CellStyle.HorizontalAlign = align;
            col.PropertiesDateEdit.ClientSideEvents.DateChanged = SelectedIndexChangedUpdateEdit;
            this.Columns.Add(col);
        }

        #endregion

    }

}
