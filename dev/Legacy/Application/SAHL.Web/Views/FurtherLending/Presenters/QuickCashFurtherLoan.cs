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
using SAHL.Web.Views.FurtherLending.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.CacheData;
using System.Collections.Generic;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.FurtherLending.Presenters
{
    public class QuickCashFurtherLoan : SAHLCommonBasePresenter<IQuickCashFurtherLoan>
    {

        private IApplication _app;

        #region Repositories/Services

        private IApplicationRepository _applicationRepo;
        private IReasonRepository _reasonRepo;

        #endregion
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public QuickCashFurtherLoan(IQuickCashFurtherLoan view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;


            CBONode cboNode;
            InstanceNode instanceNode;
            

            cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal);

            if (cboNode == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            // Get the Application from the CBO
            instanceNode = cboNode as InstanceNode;
            _app = ApplicationRepo.GetApplicationByKey(Convert.ToInt32(instanceNode.GenericKey));
            
            //Check that the application is FurtherLoan
            if (_app.ApplicationType.Key != (int)OfferTypes.FurtherLoan)
                throw new NotSupportedException("Only Further Loans are supported here.");


            //_view.OnCalculateButtonClicked += new EventHandler(_view_OnCalculateButtonClicked);
            //_view.OnContactUpdateButtonClicked += new EventHandler(_view_ContactUpdateButtonClicked);

            CheckQCReasons();

            _view.BindQuickCashDetails(_app);

            _view.OnQCDeclineReasons += new EventHandler(_view_OnQCDeclineReasons);
            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSaveButtonClicked += new EventHandler(_view_OnSaveButtonClicked);
        }

        void _view_OnSaveButtonClicked(object sender, EventArgs e)
        {
            //get app details
            ISupportsQuickCashApplicationInformation qcai = _app as ISupportsQuickCashApplicationInformation;
            _view.GetQuickCashDetails(qcai.ApplicationInformationQuickCash);
            //save
            SaveQC();
            //navigate
            if (_view.IsValid)
                _view_OnCancelButtonClicked(sender, e); //navigate back
        }

        private void SaveQC()
        {
            TransactionScope trnScope = new TransactionScope();

            try
            {
                ApplicationRepo.SaveApplication(_app);
                trnScope.VoteCommit();
            }
            catch (Exception)
            {
                trnScope.VoteRollBack();

                if (_view.IsValid)
                    throw;
            }
            finally
            {
                trnScope.Dispose();
            }
        }

        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            if (GlobalCacheData.ContainsKey("NavigateBack"))
            {
                string navigateTo = GlobalCacheData["NavigateBack"].ToString();
                GlobalCacheData.Remove("NavigateBack");
                _view.Navigator.Navigate(navigateTo);
            }
        }

        void _view_OnQCDeclineReasons(object sender, EventArgs e)
        {
            //set QC values to 0
            ISupportsQuickCashApplicationInformation qcai = _app as ISupportsQuickCashApplicationInformation;
            qcai.ApplicationInformationQuickCash.CreditApprovedAmount = 0D;
            qcai.ApplicationInformationQuickCash.CreditUpfrontApprovedAmount = 0D;
            //save
            ExclusionSets.Add(RuleExclusionSets.LoanDetailsUpdate);
            SaveQC();
            ExclusionSets.Remove(RuleExclusionSets.LoanDetailsUpdate);

            //load stuff into cache
            if (GlobalCacheData.ContainsKey(ViewConstants.Application))
                GlobalCacheData.Remove(ViewConstants.Application);

            GlobalCacheData.Add(ViewConstants.Application, _app, new List<ICacheObjectLifeTime>());

            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo))
                GlobalCacheData.Remove(ViewConstants.NavigateTo);

            GlobalCacheData.Add(ViewConstants.NavigateTo, _view.ViewName, new List<ICacheObjectLifeTime>());

            if (_view.IsValid)
                Navigator.Navigate("Orig_QCDecline");
        }

        protected void CheckQCReasons()
        {
            IReadOnlyEventList<IReason> reasons = ReasonRepo.GetReasonByGenericKeyAndReasonTypeKey(_app.GetLatestApplicationInformation().Key, (int)ReasonTypes.QuickCashDecline);
            if (reasons != null && reasons.Count > 0)
            {
                _view.HasQuickCashDeclineReasons = true;
            }
        }

        private IApplicationRepository ApplicationRepo
        {
            get
            {
                if (_applicationRepo == null)
                    _applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                return _applicationRepo;
            }
        }

        private IReasonRepository ReasonRepo
        {
            get
            {
                if (_reasonRepo == null)
                    _reasonRepo = RepositoryFactory.GetRepository<IReasonRepository>();

                return _reasonRepo;
            }
        }
    }
}
