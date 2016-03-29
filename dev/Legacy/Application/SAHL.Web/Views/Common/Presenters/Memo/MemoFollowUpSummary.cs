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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.CacheData;
using System.Collections.Generic;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Common.Presenters.Memo
{
    /// <summary>
    /// Class  - inherits from MemoBase
    /// </summary>
    public class MemoFollowUpSummary : MemoBase
    {
        /// <summary>
        /// Constructor for MemoAccountDisplay
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public MemoFollowUpSummary(SAHL.Web.Views.Common.Interfaces.IMemo view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            {
            }
        }


        /// <summary>
        /// Populate lists on view initialised
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            //_genericKeyType = (int)SAHL.Common.Globals.GenericKeyTypes.Offer;

            _view.OnMemoStatusChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnMemoStatusChanged);
            _view.OnMemoGridsSelectedIndexChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnMemoGridsSelectedIndexChanged);

            _memo = _memoRepository.GetMemoByGenericKey(_genericKey, _genericKeyType, (int)GeneralStatuses.Active);
      
        }
        /// <summary>
        /// Set control visibility
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            _view.ShowControlsDisplay = true;
            _view.ShowControlsUpdate = false;
            _view.ShowButtons = false;
            _view.SetGridPostBackType();
            _view.HideExpiryDateControlForFollowUp();

            _view.SetLabelData("FollowUp Memo", "");
            _view.BindMemoGrid(_memo,"FollowUp");

            _view.BindMemoGridForFollowUp(_memo, _memoIndexSelected);

            if (_memo.Count > 0)
                _view.BindMemoFields(_memo[_memoIndexSelected]);
        }

        /// <summary>
        /// MemoStatus Change - repopulate lists
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnMemoStatusChanged(object sender, KeyChangedEventArgs e)
        {
            if (Convert.ToInt32(e.Key) == 0)
                _memo = _memoRepository.GetMemoByGenericKey(_genericKey, _genericKeyType);
            else
                _memo = _memoRepository.GetMemoByGenericKey(_genericKey, _genericKeyType, Convert.ToInt32(e.Key));
          
            // When Change of Status, the first record must be selected by default
            _memoIndexSelected = 0;
        }

        void _view_OnMemoGridsSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            _memoIndexSelected = (Convert.ToInt32(e.Key));
        }
    }
}
