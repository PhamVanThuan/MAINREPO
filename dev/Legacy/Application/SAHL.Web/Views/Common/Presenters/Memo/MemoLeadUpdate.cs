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
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Views.Common.Presenters.Memo
{
    public class MemoLeadUpdate : MemoBase
    {
                /// <summary>
        /// Constructor MemoAccountUpdate
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public MemoLeadUpdate(SAHL.Web.Views.Common.Interfaces.IMemo view, SAHLCommonBaseController controller)
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
            _memo = _memoRepository.GetMemoByGenericKey(_genericKey, _genericKeyType, (int)GeneralStatuses.Active);
         
            _view.UpdateButtonClicked += new KeyChangedEventHandler(_view_UpdateButtonClicked);
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
            _view.HideReminderDate();

            if (_memo == null || _memo.Count == 0)
                _view.HideUpdateButton();

            _view.SetLabelData(_genericCBONodeDescription, "Update");

            _view.BindMemoGridForDisplayAndUpdate(_memo, _memoIndexSelected);

            if (_memo.Count > 0)
            {
                if (_memo.Count <= _memoIndexSelected)
                    _memoIndexSelected = 0;

                _view.BindMemoFields(_memo[_memoIndexSelected]);

                if (string.Compare(_memo[_memoIndexSelected].ADUser.ADUserName,_view.CurrentPrincipal.Identity.Name,true) != 0)
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
            if (Convert.ToInt32(e.Key) == 0)
                _memo = _memoRepository.GetMemoByGenericKey(_genericKey, _genericKeyType);
            else
                _memo = _memoRepository.GetMemoByGenericKey(_genericKey, _genericKeyType, Convert.ToInt32(e.Key));
         
        }

        void _view_OnMemoGridsSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            _memoIndexSelected = (Convert.ToInt32(e.Key));
        }


        void _view_UpdateButtonClicked(object sender, KeyChangedEventArgs e)
        {
            SAHL.Common.BusinessModel.Interfaces.IMemo memo;
            memo = _memo[Convert.ToInt32(e.Key)];

            _view.GetCapturedMemoAddWithoutFollowup(memo);

            TransactionScope txn = new TransactionScope();

            try
            {
                _memoRepository.SaveMemo(memo);
                txn.VoteCommit();

                this.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
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
