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
using System.Text;
using SAHL.Common.Authentication;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Life
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Declaration : SAHLCommonBaseView, IDeclarations
    {
        private bool _activityDone;
        private bool _confirmationMode;
        private bool _allOptionsChecked;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!this.ShouldRunPage)
                return;

            if (_confirmationMode == true)
                lblPageHeader.Text += " ( CONFIRMATION )";
        }

        #region IDeclarationsView Members
        /// <summary>
        /// implements <see cref="IDeclarations.OnNextButtonClicked"/>
        /// </summary>
        public event EventHandler OnNextButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public bool ConfirmationMode
        {
            get { return _confirmationMode; }
            set { _confirmationMode = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ActivityDone
        {
            get { return _activityDone; }
            set { _activityDone = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AllOptionsChecked
        {
            get { return _allOptionsChecked; }
            set { _allOptionsChecked = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="textItems"></param>
        public void BindDeclarations(IReadOnlyEventList<ITextStatement> textItems)
        {
            StringBuilder JS = new StringBuilder();
            JS.AppendLine("function allChecked()");
            JS.AppendLine("{");
            JS.AppendLine("var form = document.getElementById('aspnetForm');");
            JS.AppendLine("var hid = form['" + HiddenField1.UniqueID + "'];");
            JS.AppendLine("hid.value = true;");
            JS.AppendLine("var cbx = null;");

            if (textItems.Count > 0)
            {
                TableRow spacer = new TableRow();
                spacer.Height = new Unit(5);
                tblText.Rows.Add(spacer);

                for (int i = 0; i < textItems.Count; i++)
                {
                    TableRow Row = new TableRow();
                    Row.Cells.Add(new TableCell());
                    Row.Cells.Add(new TableCell());

                    CheckBox cbx = new CheckBox();
                    cbx.ID = "cbx" + i.ToString();

                    if (_activityDone)
                    {
                        cbx.Checked = true;
                        cbx.Enabled = false;
                    }

                    Label lbl = new Label();
                    lbl.ID = "lbl" + i.ToString();
                    lbl.Text = textItems[i].Statement;

                    lbl.Font.Size = new FontUnit(FontSize.Small);

                    Row.Cells[0].Controls.Add(cbx);
                    Row.Cells[0].VerticalAlign = VerticalAlign.Top;
                    Row.Cells[1].Controls.Add(lbl);
                    Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                    Row.Cells[1].VerticalAlign = VerticalAlign.Top;
                    Row.Height = new Unit(40);
                    tblText.Rows.Add(Row);

                    cbx.Attributes.Add("onclick", "allChecked();");

                    JS.AppendLine("cbx = document.getElementById('" + cbx.ClientID + "');");
                    JS.AppendLine("if (cbx.checked == false) hid.value = false;");
                }

            }

            JS.AppendLine("}");
            if (!Page.ClientScript.IsClientScriptBlockRegistered("allChecked"))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "allChecked", JS.ToString(), true);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (HiddenField1.Value == "false" && _activityDone == false)
                _allOptionsChecked = false;
            else
            {
                _allOptionsChecked = true;
                _activityDone = true;
            }

            OnNextButtonClicked(sender, e);
        }
    }
}