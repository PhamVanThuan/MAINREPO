using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Interfaces;

namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    ///
    /// </summary>
    public class PremiumHistoryBase : SAHLCommonBasePresenter<IPremiumHistory>
    {
        private CBOMenuNode _node;
        private ILifeRepository _lifeRepository;
        private IList<ILifePremiumHistory> _lstLifePremiumHistory;

        private int _genericKey;

        public int GenericKey
        {
            get { return _genericKey; }
            set { _genericKey = value; }
        }

        private int _accountKey;

        /// <summary>
        ///
        /// </summary>
        public int AccountKey
        {
            get { return _accountKey; }
            set { _accountKey = value; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public PremiumHistoryBase(IPremiumHistory view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _lifeRepository = RepositoryFactory.GetRepository<ILifeRepository>();

            // Get the Instance Node
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _genericKey = _node.GenericKey;
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);

            // Get the LifePolicyHistory collections
            _lstLifePremiumHistory = _lifeRepository.GetLifePremiumHistory(_accountKey);

            if (_lstLifePremiumHistory.Count > 0)
            {
                // bind the life premium history grid
                _view.BindPremiumHistoryGrid(_lstLifePremiumHistory);
            }
        }

        /// <summary>
        /// Set the relevant properties for displaying controls within the view
        /// </summary>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo))
            {
                string navigateTo = GlobalCacheData[ViewConstants.NavigateTo].ToString();
                GlobalCacheData.Remove(ViewConstants.NavigateTo);
                _view.Navigator.Navigate(navigateTo);
            }
            else
                _view.Navigator.Navigate("Life_Contact");
        }
    }
}