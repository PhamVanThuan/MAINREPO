using System;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class ConditionsSet : SAHLCommonBaseView,IConditionsSet
    {

        protected void btnAddCondition_Click(object sender, EventArgs e)
        {
            if (btnAddConditionClicked != null)
                btnAddConditionClicked(sender, e);
        }

        protected void btnEditCondition_Click(object sender, EventArgs e)
        {         
            if (btnEditConditionClicked != null)
                btnEditConditionClicked(sender, e);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (btnUpdateClicked != null)
                btnUpdateClicked(sender, e);
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (btnSaveClicked != null)
                btnSaveClicked(sender, e);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancelClicked != null)
                btnCancelClicked(sender, e);
        }

        #region IConditions Members

        /// <summary>
        /// Implements <see cref="IConditionsSet.btnAddConditionClicked"/>.
        /// </summary>
        public event EventHandler btnAddConditionClicked;

        /// <summary>
        /// Implements <see cref="IConditionsSet.btnEditConditionClicked"/>.
        /// </summary>
        public event EventHandler btnEditConditionClicked;

        /// <summary>
        /// Implements <see cref="IConditionsSet.btnUpdateClicked"/>.
        /// </summary>
        public event EventHandler btnUpdateClicked;

        /// <summary>
        /// Implements <see cref="IConditionsSet.btnSaveClicked"/>.
        /// </summary>
        public event EventHandler btnSaveClicked;        
        
        /// <summary>
        /// Implements <see cref="IConditionsSet.btnCancelClicked"/>.
        /// </summary>
        public event EventHandler btnCancelClicked;


        //--------------------------------------------------------
        /// <summary>
        /// Property to show/hide the Valudation Message
        /// </summary>
        public bool ShowlblErrorMessage
        {
            get { return lblErrorMessage.Visible; }
            set { lblErrorMessage.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'Update Condition' button
        /// </summary>
        public bool ShowUpdateButton
        {
            set { btnUpdate.Visible = value; }
        }
        /// <summary>
        /// Property to show/hide the 'Save Condition Set' button
        /// </summary>
        public bool ShowSaveButton
        {
            set { btnSave.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'Edit Condition' button
        /// </summary>
        public bool ShowEditConditionButton
        {
            set { btnEditCondition.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'Add Condition' button
        /// </summary>
        public bool ShowAddButton
        {
            set { btnAddCondition.Visible = value; }
        }

        /// <summary>
        /// Property to enable the 'Save Condition Set' button
        /// </summary>
        public bool EnableSaveButton
        {
            set { btnSave.Enabled = value; }
        }

        /// <summary>
        /// Property to enable the 'listGenericConditions' list
        /// </summary>
        public bool SetlistGenericConditionsReadOnly
        {
            set { listGenericConditions.Enabled = value; }
        }

       ///// <summary>
       // /// Property to enable the 'listSelectedConditions' list
       // /// </summary>
       // public bool SetlistSelectedConditionsReadOnly
       // {
       //     set { listSelectedConditions.Enabled = value; }
       // }


            


        //******************************************************************
        // Get or Set the Hidden Field values

        /// <summary>
        /// Gets or Sets the txtSelectArrayStrings value
        /// </summary>
        public string SettxtSelectArrayStrings
        {
            set { txtSelectArrayStrings.Value = value; }
            get { return txtSelectArrayStrings.Value; }
        }

        /// <summary>
        /// Gets or Sets the txtSelectArrayCSSColor value
        /// </summary>
        public string SettxtSelectArrayCSSColor
        {
            set { txtSelectArrayCSSColor.Value = value; }
            get { return txtSelectArrayCSSColor.Value; }
        }

        /// <summary>
        /// Gets or Sets the txtSelectArrayCSSWeight value
        /// </summary>
        public string SettxtSelectArrayCSSWeight
        {
            set { txtSelectArrayCSSWeight.Value = value; }
            get { return txtSelectArrayCSSWeight.Value; }
        }

        /// <summary>
        /// Gets or Sets the txtSelectArrayValue value
        /// </summary>
        public string SettxtSelectArrayValue
        {
            set { txtSelectArrayValue.Value = value; }
            get { return txtSelectArrayValue.Value; }
        }

        /// <summary>
        /// Gets or Sets the txtSelectArrayID value
        /// </summary>
        public string SettxtSelectArrayID
        {
            set { txtSelectArrayID.Value = value; }
            get { return txtSelectArrayID.Value; }
        }

        /// <summary>
        /// Gets or Sets the txtSelectArrayUserEdited value
        /// </summary>
        public string SettxtSelectArrayUserEdited
        {
            set { txtSelectArrayUserEdited.Value = value; }
            get { return txtSelectArrayUserEdited.Value; }
        }

        /// <summary>
        /// Gets or Sets the txtSelectUserConditionType value
        /// </summary>
        public string SettxtSelectUserConditionType
        {
            set { txtSelectUserConditionType.Value = value; }
            get { return txtSelectUserConditionType.Value; }
        }

        /// <summary>
        /// Gets or Sets the txttxtSelectedOfferConditionKeys value
        /// </summary>
        public string SettxtSelectedOfferConditionKeys
        {
            set { txtSelectedOfferConditionKeys.Value = value; }
            get { return txtSelectedOfferConditionKeys.Value; }
        }


        /// <summary>
        /// Gets or Sets the txtSelectedOfferConditionSetKeys value
        /// </summary>
        public string SettxtSelectedOfferConditionSetKeys
        {
            set { txtSelectedOfferConditionSetKeys.Value = value; }
            get { return txtSelectedOfferConditionSetKeys.Value; }
        }


        /// <summary>
        /// Gets or Sets the txtChosenOfferConditionSetKeys value
        /// </summary>
        public string SettxtChosenOfferConditionSetKeys
        {
            set { txtChosenOfferConditionSetKeys.Value = value; }
            get { return txtChosenOfferConditionSetKeys.Value; }
        }

        /// <summary>
        /// Gets or Sets the txtChosenOfferConditionKeys value
        /// </summary>
        public string SettxtChosenOfferConditionKeys
        {
            set { txtChosenOfferConditionKeys.Value = value; }
            get { return txtChosenOfferConditionKeys.Value; }
        }


        /// <summary>
        /// Gets or Sets the txtChosenArrayStrings value
        /// </summary>
        public string SettxtChosenArrayStrings
        {
            set { txtChosenArrayStrings.Value = value; }
            get { return txtChosenArrayStrings.Value; }
        }

        /// <summary>
        /// Gets or Sets the txtChosenArrayCSSColor value
        /// </summary>
        public string SettxtChosenArrayCSSColor
        {
            set { txtChosenArrayCSSColor.Value = value; }
            get { return txtChosenArrayCSSColor.Value; }
        }

        /// <summary>
        /// Gets or Sets the txtChosenArrayCSSWeight value
        /// </summary>
        public string SettxtChosenArrayCSSWeight
        {
            set { txtChosenArrayCSSWeight.Value = value; }
            get { return txtChosenArrayCSSWeight.Value; }
        }

        /// <summary>
        /// Gets or Sets the txtChosenArrayValue value
        /// </summary>
        public string SettxtChosenArrayValue
        {
            set { txtChosenArrayValue.Value = value; }
            get { return txtChosenArrayValue.Value; }
        }

        /// <summary>
        /// Gets or Sets the txtChosenArrayID value
        /// </summary>
        public string SettxtChosenArrayID
        {
            set { txtChosenArrayID.Value = value; }
            get { return txtChosenArrayID.Value; }
        }

        /// <summary>
        /// Gets or Sets the txtChosenArrayUserEdited value
        /// </summary>
        public string SettxtChosenArrayUserEdited
        {
            set { txtChosenArrayUserEdited.Value = value; }
            get { return txtChosenArrayUserEdited.Value; }
        }

        /// <summary>
        /// Gets or Sets the txtChosenUserConditionType value
        /// </summary>
        public string SettxtChosenUserConditionType
        {
            set { txtChosenUserConditionType.Value = value; }
            get { return txtChosenUserConditionType.Value; }
        }


        /// <summary>
        /// Gets or Sets the txtSelectedIndex value
        /// </summary>
        public string SettxtSelectedIndex
        {
            set { txtSelectedIndex.Value = value; }
            get { return txtSelectedIndex.Value; }
        }


        /// <summary>
        /// Gets the txtDisplay value
        /// </summary>
        public string GettxtDisplayText
        {
            get { return txtDisplay.Text; }
        }


        ///// <summary>
        ///// Gets the listSelectedConditions.SelectedValue
        ///// </summary>
        //public string GetlistSelectedConditionsValue
        //{
        //    get { return listSelectedConditions.SelectedValue; }
        //}


         /// <summary>
        /// Gets or Sets the txtConditionTableKey value
        /// </summary>
        public string SettxttxtConditionTableKey
        {
            set { txtConditionTableKey.Value = value; }
            get { return txtConditionTableKey.Value; }
        }




        //**********************************************************************

        ///// <summary>
        ///// Gets the listSelectedConditions.ClientID
        ///// </summary>
        //public string GetlistSelectedConditionsClientID
        //{
        //    get { return listSelectedConditions.ClientID; }
        //}
        ///// <summary>
        ///// Gets the txtDisplay.ClientID
        ///// </summary>
        //public string GetlistGenericConditionsClientID
        //{
        //    get { return listGenericConditions.ClientID; }
        //}
        /// <summary>
        /// Gets the listGenericConditions.ClientID
        /// </summary>
        public string GettxtDisplayClientID
        {
            get { return txtDisplay.ClientID; }
        }
        /// <summary>
        /// Gets the btnEditCondition.ClientID;
        /// </summary>
        public string GetbtnEditConditionClientID
        {
            get { return btnEditCondition.ClientID; }
        }
        /// <summary>
        /// Gets the txtSelectedIndex.ClientID
        /// </summary>
        public string GettxtSelectedIndexClientID
        {
            get { return txtSelectedIndex.ClientID; }
        }
        /// <summary>
        /// Gets the txtSelectArrayStrings.ClientID;
        /// </summary>
        public string GettxtSelectArrayStringsClientID
        {
            get { return txtSelectArrayStrings.ClientID; }
        }
        /// <summary>
        /// Gets the txtSelectArrayCSSColor.ClientID
        /// </summary>
        public string GettxtSelectArrayCSSColorClientID
        {
            get { return txtSelectArrayCSSColor.ClientID; }
        }
        /// <summary>
        /// Gets the txtSelectArrayCSSWeight.ClientID
        /// </summary>
        public string GettxtSelectArrayCSSWeightClientID
        {
            get { return txtSelectArrayCSSWeight.ClientID; }
        }
        /// <summary>
        /// Gets the txtSelectArrayValue.ClientID
        /// </summary>
        public string GettxtSelectArrayValueClientID
        {
            get { return txtSelectArrayValue.ClientID; }
        }
        /// <summary>
        /// Gets the txtSelectArrayID.ClientID
        /// </summary>
        public string GettxtSelectArrayIDClientID
        {
            get { return txtSelectArrayID.ClientID; }
        }
        /// <summary>
        /// Gets the txtSelectArrayUserEdited.ClientID
        /// </summary>
        public string GettxtSelectArrayUserEditedClientID
        {
            get { return txtSelectArrayUserEdited.ClientID; }
        }

        /// <summary>
        /// Gets the txtSelectUserConditionType.ClientID
        /// </summary>
        public string GettxtSelectUserConditionType
        {
            get { return txtSelectUserConditionType.ClientID; }
        }

        /// <summary>
        /// Gets the txtChosenArrayStrings.ClientID
        /// </summary>
        public string GettxtChosenArrayStringsClientID
        {
            get { return txtChosenArrayStrings.ClientID; }
        }
        /// <summary>
        /// Gets the txtChosenArrayCSSColor.ClientID
        /// </summary>
        public string GettxtChosenArrayCSSColorClientID
        {
            get { return txtChosenArrayCSSColor.ClientID; }
        }
        /// <summary>
        /// Gets the txtChosenArrayCSSWeight.ClientID
        /// </summary>
        public string GettxtChosenArrayCSSWeightClientID
        {
            get { return txtChosenArrayCSSWeight.ClientID; }
        }
        /// <summary>
        /// Gets the txtChosenArrayValue.ClientID
        /// </summary>
        public string GettxtChosenArrayValueClientID
        {
            get { return txtChosenArrayValue.ClientID; }
        }
        /// <summary>
        /// Gets the txtChosenArrayID.ClientID
        /// </summary>
        public string GettxtChosenArrayIDClientID
        {
            get { return txtChosenArrayID.ClientID; }
        }
        /// <summary>
        /// Gets the txtChosenArrayUserEdited.ClientID
        /// </summary>
        public string GettxtChosenArrayUserEditedClientID
        {
            get { return txtChosenArrayUserEdited.ClientID; }
        }

        /// <summary>
        /// Gets the txtChosenUserConditionType.ClientID
        /// </summary>
        public string GettxtChosenUserConditionType
        {
            get { return txtChosenUserConditionType.ClientID; }
        }


        /// <summary>
        /// Gets the txtSelectedOfferConditionKeys.ClientID
        /// </summary>
        public string GettxtSelectedOfferConditionKeys
        {
            get { return txtSelectedOfferConditionKeys.ClientID; }
        }

        /// <summary>
        /// Gets the txtChosenOfferConditionKeys.ClientID
        /// </summary>
        public string GettxtChosenOfferConditionKeys
        {
            get { return txtChosenOfferConditionKeys.ClientID; }
        }


        /// <summary>
        /// Gets the txtChosenOfferConditionKeys.ClientID
        /// </summary>
        public string GettxtChosenOfferConditionSetKeys
        {
            get { return txtChosenOfferConditionSetKeys.ClientID; }
        }

        /// <summary>
        /// Gets the txtSelectedOfferConditionSetKeys.ClientID
        /// </summary>
        public string GettxtSelectedOfferConditionSetKeys
        {
            get { return txtSelectedOfferConditionSetKeys.ClientID; }
        }


        /// <summary>
        /// Gets the txtConditionTableKeyClientID
        /// </summary>
        public string GettxtConditionTableKeyClientID
        {
            get { return txtConditionTableKey.ClientID; }
        }

        #endregion 

        /// <summary>
        /// Create and Register the Views Javascript Model
        /// </summary>
        /// <param name="mBuilder"></param>
        public void RegisterClientScripts(System.Text.StringBuilder mBuilder)
        {

            if (!Page.ClientScript.IsClientScriptBlockRegistered(GetType(), "lstScripts"))
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "lstScripts", mBuilder.ToString(), true);
            }


            listGenericConditions.Attributes.Add("onchange", "cleartext(SelectArrayValue[selectedIndex]); document.getElementById('" + btnEditCondition.ClientID + "').disabled = true; document.getElementById('ctl00_Main_listSelectedConditions').selectedIndex = -1; document.getElementById('btnRemove').disabled = true; document.getElementById('btnAdd').disabled = false;");
            btnAddCondition.Attributes.Add("onclick", "StoreConditionsState(); document.getElementById('" + listGenericConditions.ClientID + "').selectedIndex = -1; document.getElementById('ctl00_Main_listSelectedConditions').selectedIndex = -1;  ");
            btnEditCondition.Attributes.Add("onclick", "StoreConditionsState(); document.getElementById('ctl00_Main_listSelectedConditions').selectedIndex = -1; document.getElementById('" + listGenericConditions.ClientID + "').selectedIndex = -1;  ");

            btnSave.Attributes.Add("onclick", "document.getElementById('ctl00_Main_listSelectedConditions').selectedIndex = -1; document.getElementById('" + listGenericConditions.ClientID + "').selectedIndex = -1; StoreConditionsState();");
            btnCancel.Attributes.Add("onclick", "document.getElementById('ctl00_Main_listSelectedConditions').selectedIndex = -1; document.getElementById('" + listGenericConditions.ClientID + "').selectedIndex = -1;");
            btnUpdate.Attributes.Add("onclick", "document.getElementById('ctl00_Main_listSelectedConditions').selectedIndex = -1; document.getElementById('" + listGenericConditions.ClientID + "').selectedIndex = -1; StoreConditionsState();");
        }





    }
}