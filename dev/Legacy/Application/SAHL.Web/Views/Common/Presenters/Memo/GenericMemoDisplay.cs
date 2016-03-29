using System;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.CacheData;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common.Presenters.Memo
{
    /// <summary>
    /// Class  - inherits from MemoBase
    /// </summary>
    public class GenericMemoDisplay : MemoBase
    {
        /// <summary>
        /// Constructor for MemoAccountDisplay
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public GenericMemoDisplay(Interfaces.IMemo view, SAHLCommonBaseController controller)
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

            _view.OnMemoStatusChanged += (_view_OnMemoStatusChanged);
            _view.OnMemoGridsSelectedIndexChanged += (_view_OnMemoGridsSelectedIndexChanged);

            // On initial load will load all memos as it's summary data
            if (!_view.IsPostBack)
                _view.MemoStatusSelectedValue = 0;

            _statusSelectedIndex = _view.MemoStatusSelectedValue;

            _memo = _memoRepository.GetMemoRelatedToAccount(_genericKey, _genericKeyType, _statusSelectedIndex);
            _memo.Sort();

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

            _view.SetLabelData(_genericCBONodeDescription + " Memo", "");

            _view.BindMemoGridForDisplayAndUpdate(_memo, _memoIndexSelected);

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
    }
}
