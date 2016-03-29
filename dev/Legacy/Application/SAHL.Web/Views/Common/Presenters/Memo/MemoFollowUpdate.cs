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
using SAHL.Common.Globals;
using System.Collections.Generic;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Views.Common.Presenters.Memo
{
    /// <summary>
    /// 
    /// </summary>
    public class MemoFollowUpUpdate : MemoBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public MemoFollowUpUpdate(IMemo view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            {
            }
        }

        /// <summary>
        /// Onviewinitialised event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            //_genericKeyType = (int)SAHL.Common.Globals.GenericKeyTypes.Offer;

            _memo = _memoRepository.GetMemoByGenericKey(_genericKey, _genericKeyType, (int)GeneralStatuses.Active); // Default to Unresolved on Load of Page
          
            RemoveNullReminderDates();

            _view.UpdateButtonClicked += new KeyChangedEventHandler(_view_UpdateButtonClicked);
            _view.CancelButtonClicked += new EventHandler(_view_CancelButtonClicked);
            _view.OnMemoStatusChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnMemoStatusChanged);
            _view.OnMemoGridsSelectedIndexChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnMemoGridsSelectedIndexChanged);
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
            _view.HideExpiryDateControlForFollowUp();
            _view.ShowHours();

            _view.SetLabelData("FollowUp", "Update");

            _view.BindMemoGridForFollowUp(_memo, _memoIndexSelected);

            if (_memo.Count > 0)
            {
                if (_memo.Count <= _memoIndexSelected)
                    _memoIndexSelected = 0;

                _view.BindMemoFieldsForFollowUpUpdate(_memo[_memoIndexSelected]);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnMemoStatusChanged(object sender, KeyChangedEventArgs e)
        {
            if (Convert.ToInt32(e.Key) == 0)
                _memo = _memoRepository.GetMemoByGenericKey(_genericKey, _genericKeyType);
            else
                _memo = _memoRepository.GetMemoByGenericKey(_genericKey, _genericKeyType, Convert.ToInt32(e.Key));
        
            RemoveNullReminderDates();
        }

        private void RemoveNullReminderDates()
        {
            if (_memo != null && _memo.Count > 0)
            {
                for (int i = 0; i < _memo.Count; i++)
                {
                    if (_memo[i].ReminderDate == null)
                        _memo.Remove(_view.Messages, _memo[i]);
                }
            }
        }

        void _view_OnMemoGridsSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            _memoIndexSelected = (Convert.ToInt32(e.Key));
        }

        void _view_CancelButtonClicked(object sender, EventArgs e)
        {
            base.X2Service.CancelActivity(_view.CurrentPrincipal);
            base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        void _view_UpdateButtonClicked(object sender, KeyChangedEventArgs e)
        {
            // Add exclusion set
            this.ExclusionSets.Add(RuleExclusionSets.MemoExpiryDateExclude);

            SAHL.Common.BusinessModel.Interfaces.IMemo memo;
            memo = _memo[Convert.ToInt32(e.Key)];

            _view.GetUpdatedMemoForFollowUp(memo);

            TransactionScope txn = new TransactionScope();
            try
            {
                _memoRepository.SaveMemo(memo);
                this.ExclusionSets.Remove(RuleExclusionSets.MemoExpiryDateExclude);
                txn.VoteCommit();

                Dictionary<string, string> FieldInputs = new Dictionary<string, string>();
                FieldInputs.Add("GenericKey", memo.Key.ToString());

                base.X2Service.CompleteActivity(_view.CurrentPrincipal, FieldInputs, false);
                base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                {
                    base.X2Service.CancelActivity(_view.CurrentPrincipal);
                    throw;
                }
            }

            finally
            {
                txn.Dispose();
            }
        }
    }
}
