using System;
using System.Data;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class ConditionsEdit : SAHLCommonBaseView, IConditionsEdit
    {
        /// <summary>
        /// Implements <see cref="IConditionsEdit.btnRestoreStringClicked"/>.
        /// </summary>
        public event EventHandler btnRestoreStringClicked;

        /// <summary>
        /// Implements <see cref="IConditionsEdit.btnAddClicked"/>.
        /// </summary>
        public event EventHandler btnAddClicked;

        /// <summary>
        /// Implements <see cref="IConditionsEdit.btnUpdateClicked"/>.
        /// </summary>
        public event EventHandler btnUpdateClicked;

        /// <summary>
        /// Implements <see cref="IConditionsEdit.btnCancelClicked"/>.
        /// </summary>
        public event EventHandler btnCancelClicked;


        protected void btnRestoreString_Click(object sender, EventArgs e)
        {
            if (btnRestoreStringClicked != null)
                btnRestoreStringClicked(sender, e);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAddClicked != null)
                btnAddClicked(sender, e);
        }


        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (btnUpdateClicked != null)
                btnUpdateClicked(sender, e);
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancelClicked != null)
                btnCancelClicked(sender, e);
        }

        // Enable or Disable the ADD and Update Buttons

        /// <summary>
        /// Property to enable/disable the 'ADD' button
        /// </summary>
        /// 
        public bool EnableAddButton
        {
            get { return btnAdd.Enabled; }
            set { btnAdd.Enabled = value; }
        }

        /// <summary>
        /// Property to enable/disable 'Update Condition' button
        /// </summary>
        public bool EnableUpdateButton
        {
            get { return btnUpdate.Enabled; }
            set { btnUpdate.Enabled = value; }
        }

        //--------------------------------------------------------
        /// <summary>
        /// Property to show/hide the 'RestoreString' button
        /// </summary>
        /// 
        public bool ShowRestoreStringButton
        {
            get { return btnRestoreString.Visible; }
            set { btnRestoreString.Visible = value; }
        }
        /// <summary>
        /// Property to show/hide the 'Show' button
        /// </summary>
        /// 
        public bool ShowAddButton
        {
            get { return btnAdd.Visible; }
            set { btnAdd.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'Update Condition' button
        /// </summary>
        public bool ShowUpdateButton
        {
            get { return btnUpdate.Visible; }
            set { btnUpdate.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'Candel' button
        /// </summary>
        public bool ShowCancelButton
        {
            get { return btnCancel.Visible; }
            set { btnCancel.Visible = value; }
        }


        /// <summary>
        /// Property to show/hide the 'pnlTranslation' panel
        /// </summary>
        /// 
        public bool ShowTranslatePanel
        {
            get { return pnlTranslation.Visible; }
            set { pnlTranslation.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'txtTranslation' Textbox
        /// </summary>
        /// 
        public bool ShowtxtTranslation
        {
            get { return txtTranslation.Visible; }
            set { txtTranslation.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'txtTranslation' ReadOnly Status
        /// </summary>
        /// 
        public bool SettxtTranslationReadonly
        {
            get { return txtTranslation.ReadOnly; }
            set { txtTranslation.ReadOnly = value; }
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get or Set the txtNotes Text
        /// </summary>
        public string SettxtNotesText
        {
            set { txtNotes.Text = value; }
            get { return txtNotes.Text; }
        }

        /// <summary>
        /// Get or Set Panel 2's Grouping Text
        /// </summary>
        public string SetPanel2GroupingText
        {
            set { Panel2.GroupingText = value; }
            get { return Panel2.GroupingText; }
        }

        /// <summary>
        /// Set the Readonly Value of txtNotes for Non-Editable Strings
        /// </summary>
        public bool SettxtNotesReadOnly
        {
            set { txtNotes.ReadOnly = value; }
            get { return txtNotes.ReadOnly; }
        }

        //*************************************************************************************
        // Get the two hidden fields values

        /// <summary>
        /// Get or Set the SettxtTokenIDs Text
        /// </summary>
        public string SettxtTokenIDs
        {
            set { txtTokenIDs.Value = value; }
            get { return txtTokenIDs.Value; }
        }
        /// <summary>
        /// Get or Set the txtTokenValues Text
        /// </summary>
        public string SettxtTokenValues
        {
            set { txtTokenValues.Value = value; }
            get { return txtTokenValues.Value; }
        }

        /// <summary>
        /// Get or Set the Value of The Translated Text
        /// </summary>
        public string SettxtTranslation
        {
            set { txtTranslation.Text = value; }
            get { return txtTranslation.Text; }
        }

        /// <summary>
        /// Add an Attribute to the txtNotes for editing the Text (when in edit mode)
        /// </summary>
        /// <param name="option">1=Add, 2=Update 3=Translation</param>
        public void txtNotesAddAttributeforEditable(int option)
        {
            switch (option)
            {
                case 1: // add a new condition and store it as unique BtnNormal4 className 
                    txtNotes.Attributes.Add("onKeyUp", "if (document.getElementById('" + txtNotes.ClientID + "').value.length > 0) {document.getElementById('" + btnAdd.ClientID + "').disabled = false ;} else {document.getElementById('" + btnAdd.ClientID + "').disabled = true ;}  document.getElementById('" + btnAdd.ClientID + "').className = 'SAHLButton SAHLButton6';");
                    break;
                case 2: // update an existing condition and store it as unique
                    txtNotes.Attributes.Add("onKeyUp", "if (document.getElementById('" + txtNotes.ClientID + "').value.length > 0) {document.getElementById('" + btnUpdate.ClientID + "').disabled = false ;} else {document.getElementById('" + btnUpdate.ClientID + "').disabled = true ;}  document.getElementById('" + btnUpdate.ClientID + "').className = 'SAHLButton SAHLButton6';");
                    //txtNotes.Attributes.Add("onKeyUp", "document.getElementById('" + btnUpdate.ClientID + "').disabled = false; document.getElementById('" + btnUpdate.ClientID + "').className = 'SAHLButton SAHLButton4';");
                    break;
                case 3: // update an existing condition and store it as unique
                    txtTranslation.Attributes.Add("onKeyUp", "if (document.getElementById('" + txtTranslation.ClientID + "').value.length > 0) {document.getElementById('" + btnUpdate.ClientID + "').disabled = false ;} else {document.getElementById('" + btnUpdate.ClientID + "').disabled = true ;}  document.getElementById('" + btnUpdate.ClientID + "').className = 'SAHLButton SAHLButton6';");
                    //txtTranslation.Attributes.Add("onKeyUp", "document.getElementById('" + btnUpdate.ClientID + "').disabled = false; document.getElementById('" + btnUpdate.ClientID + "').className = 'SAHLButton SAHLButton4';");
                    break;
            }


        }


        /// <summary>
        /// Create the Custom fields and parse the interface for them using a datatable
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="ConditionKey"></param>
        /// <param name="Translation"></param>
        public void ConfigureTokenCaptureFields(DataTable tokens, int ConditionKey, bool Translation)
        {

            // get the tokencount for the Condition
            int tokencount = 0;
            for (int a = 0; a < tokens.Rows.Count; a++)
            {
                int TokenKey = Convert.ToInt32(tokens.Rows[a][1]);
                if (TokenKey == ConditionKey)
                    tokencount++;
            }

            // tokentype = 4 - Machine Parsed
            int Arraycount = 0;
            string TokenKeyArray = "";
            string VariableNames = "'";
            string Quote = "??";
            string QuoteComma = "??,";

            if (tokencount > 0)
            {
                txtNotes.ReadOnly = true;
                // add the capture fields to capture Token values.
                textPanel.Visible = true;
                Literal Literal1 = new Literal();
                Literal1.Text = "<table><tr width='100%'><td align='right' width='25%'><b>";
                PlaceHolder1.Controls.Add(Literal1);
                for (int y = 0; y < tokens.Rows.Count; y++)
                {
                    int TokenKey = Convert.ToInt32(tokens.Rows[y][1]);
                    if (TokenKey == ConditionKey)
                        // Check if its an automated token. if not add it.
                        if (!Equals(tokens.Rows[y][4].ToString(), "4"))
                        {
                            // get the value if stored in the token
                            string TokenValue;
                            if (!Translation)
                                TokenValue = tokens.Rows[y][5].ToString();
                            else
                                TokenValue = tokens.Rows[y][10].ToString();

                            if (TokenValue != null)
                                TokenValue = ConvertStringForHTMLDisplay(TokenValue);

                            Literal Literal5 = new Literal();
                            Literal5.Text = tokens.Rows[y][3].ToString();
                            PlaceHolder1.Controls.Add(Literal5);


                            Literal Literal3 = new Literal();
                            Literal3.Text = "<b></td><td align='left' width='75%'>";
                            PlaceHolder1.Controls.Add(Literal3);

                            // Add the name of the field to the array for later parsing
                            Arraycount++;

                            // Create the fields based on the token datatype
                            switch ((int)tokens.Rows[y][8]) //tokknrow 8 = tokenParameterTypeKey
                            {
                                case 5: // datetime
                                    SAHLDateBox DateBox = new SAHLDateBox();
                                    DateBox.ID = "txt" + tokens.Rows[y][3];
                                    if (String.IsNullOrEmpty(TokenValue))
                                        DateBox.Date = DateTime.Now;
                                    else
                                        DateBox.Date = Convert.ToDateTime(TokenValue);
                                    DateBox.Attributes.Add("onKeyUp", "document.getElementById('" + btnUpdate.ClientID + "').disabled = false; document.getElementById('" + btnUpdate.ClientID + "').className = 'SAHLButton SAHLButton6';");
                                    PlaceHolder1.Controls.Add(DateBox);
                                    break;
                                case 6: // decimal
                                    SAHLTextBox txtBox6 = new SAHLTextBox();
                                    txtBox6.ID = "txt" + tokens.Rows[y][3];
                                    txtBox6.Width = Unit.Percentage(75);
                                    txtBox6.DisplayInputType = InputType.Number;
                                    txtBox6.Text = TokenValue;
                                    txtBox6.Attributes.Add("onKeyUp", "document.getElementById('" + btnUpdate.ClientID + "').disabled = false; document.getElementById('" + btnUpdate.ClientID + "').className = 'SAHLButton SAHLButton6';");
                                    PlaceHolder1.Controls.Add(txtBox6);
                                    break;
                                case 9: // integer
                                    SAHLTextBox txtBox9 = new SAHLTextBox();
                                    txtBox9.ID = "txt" + tokens.Rows[y][3];
                                    txtBox9.Width = Unit.Percentage(75);
                                    txtBox9.Attributes.Add("onKeyUp", "document.getElementById('" + btnUpdate.ClientID + "').disabled = false; document.getElementById('" + btnUpdate.ClientID + "').className = 'SAHLButton SAHLButton6';");
                                    txtBox9.DisplayInputType = InputType.Number;
                                    txtBox9.Text = TokenValue;
                                    PlaceHolder1.Controls.Add(txtBox9);
                                    break;
                                case 10: // money
                                    SAHLTextBox txtBox10 = new SAHLTextBox();
                                    txtBox10.ID = "txt" + tokens.Rows[y][3];
                                    txtBox10.DisplayInputType = InputType.Currency;
                                    txtBox10.Width = Unit.Percentage(75);
                                    txtBox10.MaxLength = 12; // 999 999 999 .99
                                    txtBox10.Text = TokenValue;
                                    txtBox10.Attributes.Add("onKeyUp", "document.getElementById('" + btnUpdate.ClientID + "').disabled = false; document.getElementById('" + btnUpdate.ClientID + "').className = 'SAHLButton SAHLButton6';");
                                    PlaceHolder1.Controls.Add(txtBox10);
                                    break;
                                case 23:  // text area
                                    SAHLTextBox txtBox23 = new SAHLTextBox();
                                    txtBox23.TextMode = TextBoxMode.MultiLine;
                                    txtBox23.ID = "txt" + tokens.Rows[y][3];
                                    txtBox23.Width = Unit.Percentage(99);
                                    txtBox23.Text = TokenValue;
                                    txtBox23.Wrap = true;
                                    txtBox23.Attributes.Add("onKeyUp", "document.getElementById('" + btnUpdate.ClientID + "').disabled = false; document.getElementById('" + btnUpdate.ClientID + "').className = 'SAHLButton SAHLButton6';");
                                    PlaceHolder1.Controls.Add(txtBox23);
                                    break;

                                default: // type 19 text box
                                    SAHLTextBox txtBox19 = new SAHLTextBox();
                                    txtBox19.ID = "txt" + tokens.Rows[y][3];
                                    txtBox19.Width = Unit.Percentage(75);
                                    txtBox19.Text = TokenValue;
                                    txtBox19.Attributes.Add("onKeyUp", "document.getElementById('" + btnUpdate.ClientID + "').disabled = false; document.getElementById('" + btnUpdate.ClientID + "').className = 'SAHLButton SAHLButton6';");
                                    PlaceHolder1.Controls.Add(txtBox19);
                                    break;

                            }

                            Literal Literal4 = new Literal();
                            Literal4.Text = "</td></tr><tr><td align='right'width='25%'><b>";
                            PlaceHolder1.Controls.Add(Literal4);

                            TokenKeyArray += "'" + tokens.Rows[y][0] + "',";
                            VariableNames += "txt" + tokens.Rows[y][3] + "','";
                        }

                }

                Literal Literal2 = new Literal();
                Literal2.Text = "</b></td></tr></table>";
                PlaceHolder1.Controls.Add(Literal2);


            }

            txtTokenIDs.Value = TokenKeyArray;

            // build the Javascript required to post back the custom added fields
            System.Text.StringBuilder mBuilder = new System.Text.StringBuilder();
            mBuilder.AppendLine("var VariableNames =  new Array(" + VariableNames + "');");
            mBuilder.AppendLine("var ArrayCount = " + Convert.ToInt32(Arraycount) + ";");
            mBuilder.AppendLine("var StringBuilder = '';");
            mBuilder.AppendLine("var FieldName = '';");
            mBuilder.AppendLine("var count = 0;");
            mBuilder.AppendLine("if (window.addEventListener) ");//DOM method for binding an event
            mBuilder.AppendLine("window.addEventListener('load', initIt, false);");
            mBuilder.AppendLine("else if (window.attachEvent)"); //IE exclusive method for binding an event
            mBuilder.AppendLine("window.attachEvent('onload', initIt);");
            mBuilder.AppendLine("else if (document.getElementById) ");//support older modern browsers
            mBuilder.AppendLine("window.onload=initIt;");
            // basic page initialisation function
            mBuilder.AppendLine("function initIt()");
            mBuilder.AppendLine("{");
            mBuilder.AppendLine("popped =  VariableNames.pop(); ");
            mBuilder.AppendLine("}");

            mBuilder.AppendLine("function ReturnVariables() ");
            mBuilder.AppendLine("{");
            mBuilder.AppendLine("   count = 0;");
            mBuilder.AppendLine("   	while (count < ArrayCount)");
            mBuilder.AppendLine("   	{");
            mBuilder.AppendLine("       FieldName = 'ctl00_Main_' + VariableNames[count] ;");
            mBuilder.AppendLine("   	StringBuilder += '" + Quote + "' + document.getElementById(FieldName).value + '" + QuoteComma + "';");
            mBuilder.AppendLine("   	count++;");
            mBuilder.AppendLine("   	}");
            mBuilder.AppendLine("   document.getElementById('" + txtTokenValues.ClientID + "').value = StringBuilder;");
            mBuilder.AppendLine("}");


            if (!Page.ClientScript.IsClientScriptBlockRegistered(GetType(), "lstScripts"))
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "lstScripts", mBuilder.ToString(), true);
            }


            btnUpdate.Attributes.Add("onclick", "ReturnVariables();");
        }

        /// <summary>
        /// convert ASCII linebreaks to display in HTML
        /// </summary>
        /// <param name="original">Origininal string containing ASCII linebreaks</param>
        /// <returns>String formatted for HTML linebreaks</returns>
        static string ConvertStringForHTMLDisplay(string original)
        {
            string pattern = " + char(13) + char(10) +";
            string replacement = "\r\n";
            original = original.Replace(pattern, replacement);
            pattern = "\'";
            replacement = "'";
            original = original.Replace(pattern, replacement);
            return original;
        }
    }
}