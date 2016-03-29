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
using SAHL.Common.Exceptions;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Common.Presenters.Memo
{
    /// <summary>
    /// Class MemoLoanAdjustments for Add Presenter of Account Memo
    /// </summary>
    public class MemoLoanAdjustments : MemoBase
    {
        /// <summary>
        /// constructor for MemoAccountAdd
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public MemoLoanAdjustments(IMemo view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            {
            }
        }

        /// <summary>
        /// Get event lists and set event handlers on View Initialised
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _memo = _memoRepository.GetMemoRelatedToAccount(_genericKey, _genericKeyType, 0); 
            _memo.Sort();
            _view.MemoStatusSelectedValue = 0;
            
            _view.OnMemoStatusChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnMemoStatusChanged);
            _view.AddButtonClicked += new EventHandler(_view_AddButtonClicked);
            _view.CancelButtonClicked += new EventHandler(_view_CancelButtonClicked);
        }

        /// <summary>
        /// Set control display/visibility on pre render
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            _view.ShowControlsDisplay = false;
            _view.ShowControlsUpdate = false;
            _view.ShowControlsAdd = true;
            _view.ShowButtons = true;
            _view.SetDefaultDateForAdd();
            _view.SetLabelData(_genericCBONodeDescription + " Memo", "Add");
            _view.BindMemoGrid(_memo, _genericCBONodeDescription);

        }
        /// <summary>
        /// Event fired when user changes Memo Status - repopulate List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnMemoStatusChanged(object sender, KeyChangedEventArgs e)
        {
            _memo = _memoRepository.GetMemoRelatedToAccount(_genericKey, _genericKeyType, Convert.ToInt32(e.Key));
            _memo.Sort();
        }

        void _view_CancelButtonClicked(object sender, EventArgs e)
        {

            _view.Navigator.Navigate(_genericMemoSummary);
        }

        void _view_AddButtonClicked(object sender, EventArgs e)
        {
            SAHL.Common.BusinessModel.Interfaces.IMemo memo;
            memo = _memoRepository.CreateMemo();

            _view.GetCapturedMemo(memo);
            memo.GenericKey = _genericKey;
            memo.GenericKeyType = lookups.GenericKeyType.ObjectDictionary[(_genericKeyType).ToString()];

            TransactionScope txn = new TransactionScope();

            try
            {
                _memoRepository.SaveMemo(memo);
                txn.VoteCommit();

                if (_view.IsValid)
                {
                    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                    this.X2Service.CompleteActivity(_view.CurrentPrincipal, null, spc.IgnoreWarnings);
                    //base.X2Service.CompleteActivity(_view.CurrentPrincipal, null, false);
                    this.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
                }
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

            //_view.Navigator.Navigate(_genericMemoSummary);


        }


    }

}

