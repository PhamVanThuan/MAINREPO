using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;
using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.CacheData;
using SAHL.Common;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Life.Presenters
{
    public class ProductSwitch : SAHLCommonBasePresenter<ILifeProductSwitch>
    {

        private InstanceNode _node;
        private IApplicationLife _applicationLife;
        private ILookupRepository _lookUpRepo;
        private IApplicationRepository _appRepo;
        private List<ICacheObjectLifeTime> _lifeTimes;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ProductSwitch(ILifeProductSwitch view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;

            base.OnViewInitialised(sender, e);

            // Init Repos
            _lookUpRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            _appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            // Get the CBO Node    
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _applicationLife = _appRepo.GetApplicationLifeByKey((int)_node.GenericKey);
            //base.AccountKey = _applicationLife.Account.Key;

            _view.OnCancelButtonClicked +=new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked +=new EventHandler(_view_OnSubmitButtonClicked);
            _view.PolicyTypeSelectedIndexChanged +=new KeyChangedEventHandler(_view_PolicyTypeSelectedIndexChanged);
            _view.BindLifePolicyTypes(_lookUpRepo.LifePolicyTypes);
            _view.PolicyTypeSelectedValue = _applicationLife.LifePolicyType.Key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;

            base.OnViewPreRender(sender, e);
        }


        #region Events

        public void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            // Navigate back to the calling page
            _view.Navigator.Navigate(GlobalCacheData[ViewConstants.NavigateTo].ToString());
        }

        public void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            if (Validate())
            {
                GlobalCacheData.Add("ApplicationLifeKey", _applicationLife.Key,LifeTimes);
                GlobalCacheData.Add("LifePolicyTypeKey", _view.PolicyTypeSelectedValue, LifeTimes);
                _view.Navigator.Navigate("Next");
            }
        }

        public void _view_PolicyTypeSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (Convert.ToInt32(e.Key) != _applicationLife.LifePolicyType.Key)
                _view.SubmitButtonVisible = true;
            else
                _view.SubmitButtonVisible = false;
        }

        #endregion

        #region Helper Methods

        private bool Validate()
        {
            string msg = string.Empty;

            if (_view.PolicyTypeSelectedValue == -1)
            {
                msg = "Please select a Policy Type.";
                _view.Messages.Add(new Error(msg,msg));
                return false;
            }
            return true;
        }

        private List<ICacheObjectLifeTime> LifeTimes
        {
            get
            {
                if (_lifeTimes == null)
                {
                    List<string> views = new List<string>();
                    views.Add("Life_ProductSwitch");
                    views.Add("Life_CommonReasonProductSwitch");
                    _lifeTimes = new List<ICacheObjectLifeTime>();
                    _lifeTimes.Add(new SimplePageCacheObjectLifeTime(views));
                }
                return _lifeTimes;
            }
        }

        #endregion
    }
}
