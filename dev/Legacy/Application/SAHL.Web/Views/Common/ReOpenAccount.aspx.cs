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
using SAHL.Common.Authentication;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Collections;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common
{
    /// <summary>
    /// ReOpen AccountView
    /// </summary>
    public partial class Views_ReOpenAccount : SAHLCommonBaseView, IReOpenAccount
    {

        /// <summary>
        /// Click event of Yes/No button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void YesButton_Click(object sender, EventArgs e)
        {
            if (SubmitButtonClicked != null)
                SubmitButtonClicked(sender, e);
        }
        /// <summary>
        /// Click event of No button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void NoButton_Click(object sender, EventArgs e)
        {
            if (CancelButtonClicked != null)
                CancelButtonClicked(sender, e);
        }
      
        /// <summary>
        /// Submit Button Click Event
        /// </summary>
        public event EventHandler SubmitButtonClicked;
        /// <summary>
        /// Cancel Button Click Event
        /// </summary>
        public event EventHandler CancelButtonClicked;

    }

}
    #region OldCode

   // #region Private Variables

   // ReOpenAccountController m_MyController = null;

   // #endregion


   // protected override void OnInit(EventArgs e)
   // {
   //     base.OnInit(e);
   //     m_MyController = base.Controller as ReOpenAccountController ;

   //     if (!ShouldRunPage())
   //         return;

   //     PopulateLookups();

   //     m_MyController.AccountKey = int.Parse(m_CBONavigator.SelectedItem.GenericKey);
   //}


   // protected void Page_Load(object sender, EventArgs e)
   // {
   //     if (!ShouldRunPage())
   //         return;
   // }


   // protected void YesButton_Click(object sender, EventArgs e)
   // {
   //     CBO.UserSelectsRow uRow = m_CBONavigator.GetSelectedItemsParentByType(SAHL.Common.CBONodeType.StaticCBONode);

   //     m_MyController.ReOpenAccount(m_CBONavigator.DataSource, uRow.UserSelectsKey, base.GetClientMetrics());

   //     m_MyController.Navigator.Navigate("Yes");
   // }


   // protected void NoButton_Click(object sender, EventArgs e)
   // {
   //     m_MyController.Navigator.Navigate("No");
   // }
    
#endregion  OldCode


