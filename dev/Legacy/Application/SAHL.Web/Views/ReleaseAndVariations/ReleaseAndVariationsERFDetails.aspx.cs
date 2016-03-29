using System;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.ReleaseAndVariations
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ERFDetails : SAHLCommonBaseView, Interfaces.IReleaseAndVariationsERFDetails
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
        ///  Process the Update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (btnUpdateClicked != null)
                btnUpdateClicked(sender, e);
        }

        /// <summary>
        /// Cancel The Edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancelClicked != null)
                btnCancelClicked(sender, e);
        } 


        /// <summary>
        /// Set the Text value of 'txtExistingSecurity'
        /// </summary>
        public string SettxtExistingSecurity
        {
            set { txtExistingSecurity.Text = value; }
            get { return txtExistingSecurity.Text; }
        }

        /// <summary>
        /// Set the Text value of 'txtExistingSecurityEXT'
        /// </summary>
        public string SettxtExistingSecurityEXT
        {
            set { txtExistingSecurityEXT.Text = value; }
            get { return txtExistingSecurityEXT.Text; }
        }

        /// <summary>
        /// Set the Text value of 'txtExistingSecurityValuation'
        /// </summary>
        public string SettxtExistingSecurityValuation
        {
            set { txtExistingSecurityValuation.Text = value; }
            get { return txtExistingSecurityValuation.Text; }
        }

        /// <summary>
        /// Set the Text value of 'txtToBeReleased'
        /// </summary>
        public string SettxtToBeReleased
        {
            set { txtToBeReleased.Text = value; }
            get { return txtToBeReleased.Text; }
        }

        /// <summary>
        /// Set the Text value of 'txtToBeReleasedExt'
        /// </summary>
        public string SettxtToBeReleasedExt
        {
            set { txtToBeReleasedExt.Text = value; }
            get { return txtToBeReleasedExt.Text; }
        }

        /// <summary>
        /// Set the Text value of 'txtToBeReleasedValuation'
        /// </summary>
        public string SettxtToBeReleasedValuation
        {
            set { txtToBeReleasedValuation.Text = value; }
            get { return txtToBeReleasedValuation.Text; }
        }

        /// <summary>
        /// Set the Text value of 'txtRemainingSecurity'
        /// </summary>
        public string SettxtRemainingSecurity
        {
            set { txtRemainingSecurity.Text = value; }
            get { return txtRemainingSecurity.Text; }
        }

        /// <summary>
        /// Set the Text value of 'txtRemainingSecurityExt'
        /// </summary>
        public string SettxtRemainingSecurityExt
        {
            set { txtRemainingSecurityExt.Text = value; }
            get { return txtRemainingSecurityExt.Text; }
        }

        /// <summary>
        /// Set the Text value of 'txtRemainingSecurityValuation'
        /// </summary>
        public string SettxtRemainingSecurityValuation
        {
            set { txtRemainingSecurityValuation.Text = value; }
            get { return txtRemainingSecurityValuation.Text; }
        } 

        /// <summary>
        /// Property to show/hide the 'btnUpdate' button
        /// </summary>
        public bool ShowbtnUpdate
        {
            set { btnUpdate.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'btnCancel' button
        /// </summary>
        public bool ShowbtnCancel
        {
            set { btnCancel.Visible = value; }
        }    

        // SET ALL OF THE READONLY PROPERTIES

        /// <summary>
        /// Set the readonly attribute of 'txtExistingSecurity'
        /// </summary>
        public bool SetReadOnlytxtExistingSecurity
        {
            set { txtExistingSecurity.ReadOnly = value; }
        }

        /// <summary>
        /// Set the readonly attribute of 'txtExistingSecurityEXT'
        /// </summary>
        public bool SetReadOnlytxtExistingSecurityEXT
        {
            set { txtExistingSecurityEXT.ReadOnly = value; }
        }

        /// <summary>
        /// Set the readonly attribute of 'txtExistingSecurityValuation'
        /// </summary>
        public bool SetReadOnlytxtExistingSecurityValuation
        {
            set { txtExistingSecurityValuation.ReadOnly = value; }
        }

        /// <summary>
        /// Set the readonly attribute of 'txtToBeReleased'
        /// </summary>
        public bool SetReadOnlytxtToBeReleased
        {
            set { txtToBeReleased.ReadOnly = value; }
        }

        /// <summary>
        ///Set the readonly attribute of 'txtToBeReleasedExt'
        /// </summary>
        public bool SetReadOnlytxtToBeReleasedExt
        {
            set { txtToBeReleasedExt.ReadOnly = value; }
        }

        /// <summary>
        /// Set the readonly attribute of 'txtToBeReleasedValuation'
        /// </summary>
        public bool SetReadOnlytxtToBeReleasedValuation
        {
            set { txtToBeReleasedValuation.ReadOnly = value; }
        }

        /// <summary>
        /// Set the readonly attribute of 'txtRemainingSecurity'
        /// </summary>
        public bool SetReadOnlytxtRemainingSecurity
        {
            set { txtRemainingSecurity.ReadOnly = value; }
        }

        /// <summary>
        /// Set the readonly attribute of 'txtRemainingSecurityExt'
        /// </summary>
        public bool SetReadOnlytxtRemainingSecurityExt
        {
            set { txtRemainingSecurityExt.ReadOnly = value; }
        }

        /// <summary>
        /// Set the readonly attribute of 'txtRemainingSecurityValuation'
        /// </summary>
        public bool SetReadOnlytxtRemainingSecurityValuation
        {
            set { txtRemainingSecurityValuation.ReadOnly = value; }
        } 

    }
}