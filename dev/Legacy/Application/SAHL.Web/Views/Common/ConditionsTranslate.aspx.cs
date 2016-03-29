using System;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class ConditionsTranslate : SAHLCommonBaseView, IConditionsTranslate
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnUpdateClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnCancelClicked;
        
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnEditClicked;

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancelClicked != null)
                btnCancelClicked(sender, e);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (btnUpdateClicked != null)
                btnUpdateClicked(sender, e);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (btnEditClicked != null)
                btnEditClicked(sender, e);
        }



        

        /// <summary>
        /// Property to show/hide the 'lblErrorMessage' label
        /// </summary>
        public bool ShowlblErrorMessage { set { lblErrorMessage.Visible = value; } }

        /// <summary>
        /// Property to show/hide the 'btnCancel' button
        /// </summary>
        public bool ShowbtnCancel {set { btnCancel.Visible = value; }}

        /// <summary>
        /// Property to show/hide the 'btnUpdate' button
        /// </summary>
        public bool ShowbtnUpdate { set { btnUpdate.Visible = value; }}

        /// <summary>
        /// Property to show/hide the 'btnEdit' button
        /// </summary>
        public bool ShowbtnEdit { set { btnEdit.Visible = value; } }

        /// <summary>
        /// Property to Set the English Text display
        /// </summary>
        public string SettxtDisplay { set { txtDisplay.Text = value; }}
        /// <summary>
        /// Property to Get the English Text display
        /// </summary>
        public string GettxtDisplay {get { return txtDisplay.Text; }}

                /// <summary>
        /// Property to Set the Translate Text display
        /// </summary>
        public string SettxtTranslate { set { txtTranslate.Text = value; }}
        /// <summary>
        /// Property to Get the Translate Text display
        /// </summary>
        public string GettxtTranslate {get { return txtTranslate.Text; }}

        /// <summary>
        /// Set the read only status of the txtTranslate field
        /// </summary>
        public bool SettxtTranslateReadOnly {set { txtTranslate.ReadOnly = value; }}
        /// <summary>
        /// Get the read only status of the txtTranslate field
        /// </summary>
        public bool GettxtTranslateReadOnly {get { return txtTranslate.ReadOnly; }}

        //*******************************************************************
        // HANDLE THE HIDDEN FIELDS

        /// <summary>
        /// Get the Value of the txtConditionToEdit hidden field
        /// </summary>
        public string SettxtConditionToEdit { set { txtConditionToEdit.Value = value; } }
        /// <summary>
        /// Set the Value of the txtConditionToEdit hidden field
        /// </summary>
        public string GettxtConditionToEdit { get { return txtConditionToEdit.Value; } }


        /// <summary>
        /// Get the Value of the txtConditionsKey hidden field
        /// </summary>
        public string SettxtConditionsKey { set { txtConditionsKey.Value = value; } }
        /// <summary>
        /// Set the Value of the txtConditionsKey hidden field
        /// </summary>
        public string GettxtConditionsKey { get { return txtConditionsKey.Value; } }

  
        //*******************************************************************
        // GET THE CLIENT IDS FOR THE SCREEN OBJECTS

        /// <summary>
        /// Get the ClientID for listGenericConditions
        /// </summary>
        public string GetlistGenericConditionsClientID { get { return listGenericConditions.ClientID; } }

        /// <summary>
        /// Get the ClientID for txtDisplay
        /// </summary>
        public string GettxtDisplayClientID { get { return txtDisplay.ClientID; } }

        /// <summary>
        /// Get the ClientID for txtTranslate
        /// </summary>
        public string GettxtTranslateClientID { get { return txtTranslate.ClientID; } }

        /// <summary>
        /// Get the ClientID for txtConditionToEdit
        /// </summary>
        public string GettxtConditionToEditClientID { get { return txtConditionToEdit.ClientID; } }


        /// <summary>
        /// Get the ClientID for txAfrikaansArrayStrings
        /// </summary>
        public string GettxtConditionsKeyClientID { get { return txtConditionsKey.ClientID; } }

 /// <summary>
        /// Get the ClientID for btnTokens
        /// </summary>
        public string GetbtnEditClientID { get { return btnEdit.ClientID; } }

        /// <summary>
        /// Get the ClientID for btnUpdate
        /// </summary>
        public string GetbtnUpdateClientID { get { return btnUpdate.ClientID; } }

        /// <summary>
        /// Get the ClientID for btnUpdate
        /// </summary>
        public string GetbtnCancelClientID { get { return btnCancel.ClientID; } }
        
        //*******************************************************************
        // HANDLE THE UNIQUE SCRIPTING FOR THIS VIEW
        /// <summary>
        /// 
        /// </summary>
        public void RegisterClientScripts(System.Text.StringBuilder mBuilder)
        {
            if (!Page.ClientScript.IsClientScriptBlockRegistered(GetType(), "lstScripts"))
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "lstScripts", mBuilder.ToString(), true);
            }

            listGenericConditions.Attributes.Add("onchange", "cleartext(); StoreConditionsState();");
            btnEdit.Attributes.Add("onclick", "document.getElementById('" + listGenericConditions.ClientID + "').selectedIndex = -1;");
            btnUpdate.Attributes.Add("onclick", "document.getElementById('" + listGenericConditions.ClientID + "').selectedIndex = -1;");
        }

        /// <summary>
        /// Adds an item to the Translatablle conditions List
        /// </summary>
        /// <param name="li"></param>
        public void AddListBoxItem(ListItem li)
        {
            if (listGenericConditions != null) listGenericConditions.Items.Add(li);
        }

    }
}