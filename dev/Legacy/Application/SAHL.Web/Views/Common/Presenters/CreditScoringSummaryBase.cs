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
using SAHL.Web.Views.Common.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using SAHL.Common.Web.UI.Events;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Common.Presenters
{
    public class CreditScoringSummaryBase : SAHLCommonBasePresenter<ICreditScoringSummary>
    {
        protected IApplication _application;
        protected IApplicationRepository _appRepo;
        protected ICreditScoringRepository _csRepo;
        protected ILookupRepository _lookupRepo;
        protected ICommonRepository _commonRepo;
        protected List<ICacheObjectLifeTime> _lifeTimes;
        protected IEventList<IApplicationCreditScore> _scores;
        protected CBOMenuNode _node;
        protected int _offerKey;

        public IApplicationRepository appRepo
        {
            get
            {
                if (_appRepo == null)
                    _appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                return _appRepo;
            }
        }
        public ICreditScoringRepository csRepo
        {
            get
            {
                if (_csRepo == null)
                    _csRepo = RepositoryFactory.GetRepository<ICreditScoringRepository>();
                return _csRepo;
            }
        }
        public ILookupRepository lookupRepo
        {
            get
            {
                if (_lookupRepo == null)
                    _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
                return _lookupRepo;
            }
        }
        public ICommonRepository commonRepo
        {
            get
            {
                if (_commonRepo == null)
                    _commonRepo = RepositoryFactory.GetRepository<ICommonRepository>();
                return _commonRepo;
            }
        }
        protected List<ICacheObjectLifeTime> LifeTimes
        {
            get
            {
                if (_lifeTimes == null)
                {
                    List<string> views = new List<string>();
                    views.Add(this.View.ViewName);
                    _lifeTimes = new List<ICacheObjectLifeTime>();
                    _lifeTimes.Add(new SimplePageCacheObjectLifeTime(views));
                }

                return _lifeTimes;
            }
        }

        /// <summary>
        /// CreditScoringSummaryPresenter Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CreditScoringSummaryBase(ICreditScoringSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            switch (_node.GenericKeyTypeKey)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
                    // get the latest open application from the account
                    IAccount account = RepositoryFactory.GetRepository<IAccountRepository>().GetAccountByKey(_node.GenericKey);
                   
                    if (account != null && account.Applications.Count > 0)
                    {
                        foreach (IApplication app in account.Applications)
                        {
                            if (app.ApplicationStatus.Key == (int)SAHL.Common.Globals.OfferStatuses.Open || app.ApplicationStatus.Key == (int)SAHL.Common.Globals.OfferStatuses.Accepted)
                            {
                                _offerKey = app.Key;
                                break;
                            }
                        }
                    }
                    break;
                case (int)SAHL.Common.Globals.GenericKeyTypes.Offer:
                    _offerKey = _node.GenericKey;
                    break;
                default:
                    break;
            }

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            _application = appRepo.GetApplicationByKey(_offerKey);
            _scores = csRepo.GetApplicationCreditScoreByApplicationKeySorted(_application.Key, true);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;
        }
    }
}
