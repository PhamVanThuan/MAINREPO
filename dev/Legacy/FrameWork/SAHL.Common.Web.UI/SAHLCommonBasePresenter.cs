using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI.Configuration;
using SAHL.Common.Web.UI.Events;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.CacheData;
using SAHL.Common.UI;
using SAHL.Common.Globals;

namespace SAHL.Common.Web.UI
{
    public class SAHLCommonBasePresenter<T> : PresenterBase<T>, IPresenterBase
    {

        #region Private Attributes

        private IX2Service _x2Service;
        //private ICBOService _cboService;
        private SAHLPrincipalCache _spc;
        private IViewBase _viewBase;

        #endregion

        #region Constructors

        public SAHLCommonBasePresenter(T view, SAHLCommonBaseController controller) :
            base(view, controller)
        {
            _viewBase = base._view as IViewBase;
            if (_viewBase.ShouldRunPage) // null check removed - we should always have an IViewBase or the page is not valid
            {
                _viewBase.CurrentPresenter = this.GetType().FullName;
                _viewBase.ViewInitialised += new ViewHandler(OnViewInitialised);
                _viewBase.ViewLoaded += new ViewHandler(OnViewLoaded);
                _viewBase.ViewPreRender += new ViewHandler(OnViewPreRender);
            }

            // create the SAHLPrincipalCache and ensure there are no exclusions from the outset
            _spc = SAHLPrincipalCache.GetPrincipalCache(_viewBase.CurrentPrincipal);

            // get a reference to an X2Service
            _x2Service = ServiceFactory.GetService<IX2Service>();
            // get a reference to the CBOService
            //_cboService = ServiceFactory.GetService<ICBOService>();

        }

        #endregion

        #region Properties


        /// <summary>
        /// Gets the presenter name.
        /// </summary>
        public string Name
        {
            get
            {
                ObjectTypeSettings presenterSettings = UIPConfiguration.Config.GetPresenterSettings(_viewBase.ViewName);
                return presenterSettings.Name;
            }
        }

        /// <summary>
        /// List of exclusion sets that apply to the current presenter and user.  Adding items to this means 
        /// that any rules in the added sets will not be run by the rule service.  This collection will clear 
        /// with each postback.
        /// </summary>
        protected IList<RuleExclusionSets> ExclusionSets
        {
            get
            {
                return _spc.ExclusionSets;
            }
        }

        /// <summary>
        /// Gets a reference to data cached for the current presenter only. 
        /// </summary>
        protected PresenterData PrivateCacheData
        {
            get
            {
                return _spc.GetPresenterData();
            }
        }

        /// <summary>
        /// Gets a reference to the global data cache.
        /// </summary>
        protected GlobalData GlobalCacheData
        {
            get
            {
                return _spc.GetGlobalData();
            }
        }

        /// <summary>
        /// Gets a reference to the X2 service.
        /// </summary>
        protected IX2Service X2Service
        {
            get
            {
                return _x2Service;
            }
        }

        public CBOManager CBOManager
        {
            get { return _viewBase.CBOManager; }
        }

        #endregion

        #region Methods

        protected virtual void OnViewPreRender(object sender, EventArgs e)
        {
        }


        protected virtual void OnViewInitialised(object sender, EventArgs e)
        {

        }

        protected virtual void OnViewLoaded(object sender, EventArgs e)
        {
            // do the work of setting up the exclusion sets on the spc - this must be done here and not in the 
            // constructor otherwise UIP throws initialisation exceptions
            _spc.ExclusionSets.Clear();

            ObjectTypeSettings presenterSettings = UIPConfiguration.Config.GetPresenterSettings(_viewBase.ViewName);
            string cfgDefaultSets = presenterSettings.DefaultExclusionSets;

            if (!String.IsNullOrEmpty(cfgDefaultSets))
            {
                string[] defaultSets = cfgDefaultSets.Split(',');
                ILookupRepository lookUps = RepositoryFactory.GetRepository<ILookupRepository>();
                foreach (string s in defaultSets)
                {
                    if (!lookUps.RuleExclusionSets.BindableDictionary.ContainsKey(s))
                        throw new Exception(String.Format("{0} does not translate to a valid exclusion set (Exclusion sets for current presenter: {1})", s, cfgDefaultSets));

                    RuleExclusionSets rs = (RuleExclusionSets)Enum.Parse(typeof(RuleExclusionSets), s);
                    _spc.ExclusionSets.Add(rs);
                }
            }
          
        }

        #endregion
    }
}
