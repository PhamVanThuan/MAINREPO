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
using SAHL.Common.Service.Interfaces;
using System.Collections.Generic;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Exceptions;
using SAHL.Common.CacheData;
using SAHL.Common.UI;
using SAHL.Web.Views.QuickCash.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.Memo
{
     /// <summary>
    /// 
    /// </summary>
    public class MemoQuickCashEscalateToCredit : SAHLCommonBasePresenter<IMemo>
    {
        int _genericKeyType;
        int _genericKey;
        ILookupRepository lookups;
        IMemoRepository _memoRepository;
        IEventList<SAHL.Common.BusinessModel.Interfaces.IMemo> _memo;

        /// <summary>
        /// Constructor for Quick Cash Memo Add
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public MemoQuickCashEscalateToCredit(IMemo view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            {
            }
        }

        /// <summary>
        /// OnViewInitialised for FollowUp Memo Add
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _genericKeyType = (int)SAHL.Common.Globals.GenericKeyTypes.Offer;

            if (_view.IsMenuPostBack)
                GlobalCacheData.Remove(ViewConstants.GenericKey);

            if (GlobalCacheData.ContainsKey(ViewConstants.GenericKey))
                _genericKey = Convert.ToInt32(GlobalCacheData[ViewConstants.GenericKey]);

            _memoRepository = RepositoryFactory.GetRepository<IMemoRepository>();
            lookups = RepositoryFactory.GetRepository<ILookupRepository>();

            _memo = _memoRepository.GetMemoByGenericKey(_genericKey, _genericKeyType, (int)GeneralStatuses.Active);
        
            _view.PopulateStatusDropDown();
            _view.PopulateStatusUpdateDropDown();

            _view.AddButtonClicked += new EventHandler(_view_AddButtonClicked);
            _view.CancelButtonClicked += new EventHandler(_view_CancelButtonClicked);
            _view.OnMemoStatusChanged += new KeyChangedEventHandler(_view_OnMemoStatusChanged);
        }

        /// <summary>
        /// PreRender event for OnViewInitialised
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
            _view.HideExpiryDateControlForFollowUp();
            _view.HideReminderDate();
            _view.SetLabelData("QuickCash Memo", "Add");
            _view.BindMemoGrid(_memo, "Application & QuickCash");

        }
        /// <summary>
        /// MemoStatus Changed event - repopulate Memo grid
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

        void _view_CancelButtonClicked(object sender, EventArgs e)
        {
         //   base.X2Service.CancelActivity(_view.CurrentPrincipal);
        }

        void _view_AddButtonClicked(object sender, EventArgs e)
        {
            SAHL.Common.BusinessModel.Interfaces.IMemo memo;
            memo = _memoRepository.CreateMemo();

            _view.GetCapturedMemoAddWithoutFollowup(memo);

            memo.GenericKey = _genericKey;
            memo.GenericKeyType = lookups.GenericKeyType.ObjectDictionary[((int)GenericKeyTypes.Offer).ToString()];
            memo.Description = "QUICK CASH : " + memo.Description;

            TransactionScope txn = new TransactionScope();
            try
            {
                _memoRepository.SaveMemo(memo);
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
            {
                GlobalCacheData.Remove(ViewConstants.GenericKey);

              //  base.X2Service.CompleteActivity(_view.CurrentPrincipal, null, false);
              //  _view.Navigator.Navigate("");
            }
        }


    }

}


