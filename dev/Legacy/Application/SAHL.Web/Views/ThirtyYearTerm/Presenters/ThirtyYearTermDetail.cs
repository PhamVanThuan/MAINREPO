using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.V3.Framework;
using SAHL.V3.Framework.Model;
using SAHL.V3.Framework.Repositories;
using SAHL.Web.Views.ThirtyYearTerm.Interfaces;
using System;
using System.Linq;

namespace SAHL.Web.Views.ThirtyYearTerm.Presenters
{
    /// <summary>
    ///
    /// </summary>
    public class ThirtyYearTermDetail : SAHLCommonBasePresenter<IThirtyYearTermDetail>
    {
        private InstanceNode node;
        private int applicationKey;
        private IDecisionTreeRepository decisionTreeRepository;
        private IV3ServiceManager v3ServiceManager;
        private IApplicationRepository applicationRepo;
        private IApplication application;
        private QualifyApplicationFor30YearLoanTermResult decisionTreeResult;

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ThirtyYearTermDetail(IThirtyYearTermDetail view, SAHLCommonBaseController controller)
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
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            // Get the CBO Node
            node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            if (node == null)
            {
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);
            }

            applicationKey = node.GenericKey;
            decisionTreeRepository = ServiceManager.Get<IDecisionTreeRepository>();

            try
            {
                _view.ApplicationQualifiesFor30Year = DecisionTreeResult.QualifiesForThirtyYearTerm;
                if (DecisionTreeResult.QualifiesForThirtyYearTerm)
                {
                    _view.Display30YearTermDetails(DecisionTreeResult.LoanDetailFor30YearTerm);
                }
                else
                {
                    _view.DecisionTreeMessages = DecisionTreeResult.Messages.AllMessages.Select(x => x.Message).ToList();
                }
                _view.DisplayCurrentTermDetails(DecisionTreeResult.LoanDetailForCurrentTerm);

            }
            catch (Exception)
            {
                if (_view.IsValid)
                {
                    throw;
                }
                _view.ShowSubmitButton = false;
                _view.ShowCancelButton = false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.ShowSubmitButton = false;
            _view.ShowCancelButton = false;
        }

        public IApplicationRepository ApplicationRepo
        {
            get
            {
                if (applicationRepo == null)
                {
                    applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                }
                return applicationRepo;
            }
        }

        public IDecisionTreeRepository DecisionTreeRepo
        {
            get
            {
                if (decisionTreeRepository == null)
                {
                    decisionTreeRepository = ServiceManager.Get<IDecisionTreeRepository>();
                }
                return decisionTreeRepository;
            }
        }

        public IV3ServiceManager ServiceManager
        {
            get
            {
                if (v3ServiceManager == null)
                {
                    v3ServiceManager = V3ServiceManager.Instance;
                }
                return v3ServiceManager;
            }
        }

        public IApplication Application
        {
            get
            {
                if (application == null)
                {
                    application = ApplicationRepo.GetApplicationByKey(applicationKey);
                }
                return application;
            }
        }

        public QualifyApplicationFor30YearLoanTermResult DecisionTreeResult 
        {
            get
            {
                if (decisionTreeResult == null)
                {
                    decisionTreeResult = DecisionTreeRepo.QualifyApplication(applicationKey);
                }
                return decisionTreeResult;
            }
        }
    }
}