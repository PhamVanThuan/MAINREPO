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
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Events;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;


namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// class FixedDebitOrderSummaryUpdate
    /// </summary>
    public class FixedDebitOrderSummaryDelete : FixedDebitOrderSummary
    {
        /// <summary>
        /// Constructor for FixedDebitOrderSummaryUpdate
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public FixedDebitOrderSummaryDelete(IFixedDebitOrderSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.SetGridPostBack();
            _view.selectedFirstRow = true;
            _view.BindFutureDatedDOGrid(_futureDatedChangeLst);
            _view.OnFutureOrderGridSelectedIndexChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnFutureOrderGridSelectedIndexChanged);
            _view.DeleteButtonClicked += new KeyChangedEventHandler(_view_DeleteButtonClicked);
            _view.CancelButtonClicked += new KeyChangedEventHandler(_view_CancelButtonClicked);

        }

        /// <summary>
        /// Set visibility of Controls on PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;
            _view.ShowButtons = true;
            _view.ShowUpdateableControl = false;
            _view.SetControlForDelete();
            _view.SubmitButtonVisible = (_futureDatedChangeLst.Count > 0);
        }

        void _view_OnFutureOrderGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            _view.BindUpdateableControlsData(_futureDatedChangeLst[Convert.ToInt32(e.Key)]);
        }

        void _view_DeleteButtonClicked(object sender, KeyChangedEventArgs e)
        {
            TransactionScope txn = new TransactionScope();

            try
            {
                _futureDatedRepo.DeleteFutureDateChangeByKey(_futureDatedChangeLst[Convert.ToInt32(e.Key)].Key,false);
                txn.VoteCommit();
            }

            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }

            finally
            {
                txn.Dispose();
            }

            if (_view.IsValid)
                Navigator.Navigate("Cancel");
        }

        void _view_CancelButtonClicked(object sender, KeyChangedEventArgs e)
        {
            Navigator.Navigate("Cancel");
        }
    }

}
