using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using SAHL.Common.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Events;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Exceptions;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.CacheData;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common.Presenters.Memo
{
    /// <summary>
    /// Class MemoAccountUpdate : Inherits from MemoBase
    /// </summary>
    public class GenericMemoUpdate : MemoBase
    {
        /// <summary>
        /// Constructor MemoAccountUpdate
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public GenericMemoUpdate(SAHL.Web.Views.Common.Interfaces.IMemo view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            {
            }
        }

        /// <summary>
        /// Populates lists on view initialised
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _view.OnMemoStatusChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnMemoStatusChanged);
            _view.OnMemoGridsSelectedIndexChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnMemoGridsSelectedIndexChanged);
            _view.UpdateButtonClicked += new KeyChangedEventHandler(_view_UpdateButtonClicked);

            _statusSelectedIndex = _view.MemoStatusSelectedValue;
            _memo = _memoRepository.GetMemoRelatedToAccount(_genericKey, _genericKeyType, _statusSelectedIndex);
            _memo.Sort();
        }
        /// <summary>
        /// On View PreRender Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            _view.ShowControlsDisplay = false;
            _view.ShowControlsUpdate = true;

            _view.ShowButtons = true;
            _view.SetGridPostBackType();

            // If there is an error refresh list
            if (_view.Messages.Count > 0)
            {
                _statusSelectedIndex = _view.MemoStatusSelectedValue;
                _memo = _memoRepository.GetMemoRelatedToAccount(_genericKey, _genericKeyType, _statusSelectedIndex);
                _memo.Sort();
            }

            if (_memo == null || _memo.Count == 0)
                _view.HideUpdateButton();

            _view.SetLabelData(_genericCBONodeDescription, "Update");

            _view.BindMemoGridForDisplayAndUpdate(_memo, _memoIndexSelected);

            if (_memo.Count > 0)
            {
                if (_memo.Count <= _memoIndexSelected)
                    _memoIndexSelected = 0;

                _view.BindMemoFields(_memo[_memoIndexSelected]);

                // if logged in user is different to user on memo then hide the update button
                if (String.Compare(_memo[_memoIndexSelected].ADUser.ADUserName, _view.CurrentPrincipal.Identity.Name, true) != 0)
                    _view.HideUpdateButton();                
            }         
        }
        /// <summary>
        /// MemoStatus changed event - repopulate lists
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnMemoStatusChanged(object sender, KeyChangedEventArgs e)
        {
            _statusSelectedIndex = Convert.ToInt32(e.Key);
            _memo = _memoRepository.GetMemoRelatedToAccount(_genericKey, _genericKeyType, _statusSelectedIndex);
            _memo.Sort();
            
            // When Change of Status, the first record must be selected by default
            _memoIndexSelected = 0;
        }

        void _view_OnMemoGridsSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            _memoIndexSelected = (Convert.ToInt32(e.Key));
        }


        void _view_UpdateButtonClicked(object sender, KeyChangedEventArgs e)
        {
            SAHL.Common.BusinessModel.Interfaces.IMemo memo = _memo[Convert.ToInt32(e.Key)];

            _view.GetUpdatedMemo(memo);

            TransactionScope txn = new TransactionScope();

            try
            {
                _memoRepository.SaveMemo(memo);
                txn.VoteCommit();

                _view.Navigator.Navigate(_genericMemoSummary);
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
        }
    }
}
