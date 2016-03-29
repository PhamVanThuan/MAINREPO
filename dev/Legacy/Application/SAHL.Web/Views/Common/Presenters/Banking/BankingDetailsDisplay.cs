using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.CacheData;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.SearchCriteria;
using SAHL.Common.UI;
using System.Diagnostics.CodeAnalysis;

namespace SAHL.Web.Views.Common.Presenters.Banking
{
    /// <summary>
    /// Display presenter for banking details
    /// </summary>
    public class BankingDetailsDisplay : BankingDetailsBase
    {       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public BankingDetailsDisplay(IBankingDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.OnCancelClicked += new EventHandler(_view_CancelClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_UpdateBankAccountClicked);  
        }

        /// <summary>
        /// Set the relevant properties for displaying controls within the view
        /// </summary>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;
            
            _view.ControlsVisible = false;
            _view.ShowButtons = false;
            _view.ShowStatus = false;
            _view.BankAccountGridEnabled = false;
        }

       
        /// <summary>
        /// Hooks the View's UpdateBankAccountClicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_UpdateBankAccountClicked(object sender, EventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
        }
      

       

       /// <summary>
        ///  Hooks the View's DeleteBankAccountClicked event
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="IBankAccount"></param>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        protected void _view_DeleteBankAccountClicked(object sender, object IBankAccount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Hooks the View's CancelClicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_CancelClicked(object sender, EventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Hooks the View's AddBankAccountClicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_AddBankAccountClicked(object sender,EventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
        }       
    }
}
