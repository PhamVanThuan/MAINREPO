using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using System.Collections.Generic;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Common.Presenters.CommonReason
{
    public class CommonReasonQuickCashDecline: CommonReasonBase
    {

        private ILookupRepository _lookupRepo;
        private IReasonRepository _reasonRepo;
        private IReadOnlyEventList<IReason> _reasons;

        public CommonReasonQuickCashDecline(ICommonReason view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }


        /// <summary>
        /// OnViewInitialised event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            Int32 _appInfokey = 0;

            int reasonTypeKey = Convert.ToInt32(_view.ViewAttributes["reasontypekey"]);

            IReasonType reasonType = ReasonRepo.GetReasonTypeByKey(reasonTypeKey);

            switch (reasonType.GenericKeyType.Key)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.OfferInformation:
                    IApplicationInformation latestApplicationInformation = RepositoryFactory.GetRepository<IApplicationRepository>().GetApplicationByKey(GenericKey).GetLatestApplicationInformation();
                    _appInfokey = latestApplicationInformation.Key;
                    break;
                default:
                    break;
            }

            _view.SetHistoryPanelGroupingText = LookupRepo.ReasonTypes.ObjectDictionary[Convert.ToString(reasonTypeKey)].Description;

            _reasons = ReasonRepo.GetReasonByGenericKeyAndReasonTypeKey(_appInfokey, reasonTypeKey);

            if (_reasons != null && _reasons.Count > 0)
            {
                _view.HistoryDisplay = true;
                _view.ShowHistoryPanel = true;
                _view.ShowUpdatePanel = false;
                _view.ShowSubmitButtons = true;

                _view.BindReasonHistoryGrid(_reasons);

                _view.SubmitButtonText = "Redo reasons";
            }
            else
            {
                _view.HistoryDisplay = false;
                _view.ShowHistoryPanel = false;
                //_view.ShowUpdatePanel = true;
            }

            _view.CancelButtonVisible = true;
        }

        protected override void _view_OnSubmitButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            if (_reasons != null && _reasons.Count > 0)
            {
                foreach (IReason reason in _reasons)
                {
                    ReasonRepo.DeleteReason(reason);
                }

                if (_view.IsValid)
                    Navigator.Navigate("Orig_QCDecline");
            }
            else
            {
                // write away the reason data
                base._view_OnSubmitButtonClicked(sender, e);

                CompleteActivityAndNavigate();
            }
        }

        public override void CancelActivity()
        {
            if (!NavigateNoWorkFlow())
            {
                base.X2Service.CancelActivity(_view.CurrentPrincipal);
                base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
            }
        }

        public override void CompleteActivityAndNavigate()
        {
            if (!NavigateNoWorkFlow())
            {
                X2ServiceResponse rsp = base.X2Service.CompleteActivity(_view.CurrentPrincipal, null, false);
                if (base.sdsdgKeys.Count > 0)
                {
                    UpdateReasonsWithStageTransitionKey();
                }
                base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
            }
        }

        /// <summary>
        /// If there is a cache view to navigate to, navigate there
        /// This means we are not in WorkFlow, so no Cancel or Complete activity should be called
        /// </summary>
        /// <returns></returns>
        private bool NavigateNoWorkFlow()
        {
            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo))
            {
                string navigate = GlobalCacheData[ViewConstants.NavigateTo] as string;
                GlobalCacheData.Remove(ViewConstants.NavigateTo);
                Navigator.Navigate(navigate);
                return true;
            }
            return false;
        }

        private ILookupRepository LookupRepo
        {
            get 
            {
                if (_lookupRepo == null)
                    _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                return _lookupRepo;
            }
        }

        public IReasonRepository ReasonRepo
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
