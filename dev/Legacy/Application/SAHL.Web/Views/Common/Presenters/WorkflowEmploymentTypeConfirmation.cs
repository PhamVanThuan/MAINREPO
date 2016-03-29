using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Web.Views.Common.Presenters
{
    public class WorkflowEmploymentTypeConfirmation : SAHLCommonBasePresenter<IWorkflowEmploymentTypeConfirmation>
    {
        private ILookupRepository _lookupRepository;

        private ILookupRepository lookupRepository
        {
            get
            {
                if (_lookupRepository == null)
                {
                    _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
                }
                return _lookupRepository;
            }
        }

        private IDictionary<int, string> employmentTypes;

        public WorkflowEmploymentTypeConfirmation(IWorkflowEmploymentTypeConfirmation view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.OnConfirmButtonClicked += new EventHandler(_view_OnConfirmEmploymentTypeClicked);
            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelEmploymentTypeConfirmationCancelClicked);

            employmentTypes = lookupRepository
                                    .EmploymentTypes
                                        .Where(x => x.Key != 5 /* Exclude "Unknown" employment type */)
                                            .ToDictionary(x => x.Key, y => y.Description);

            _view.BindEmploymentTypes(employmentTypes);
        }

        private void _view_OnCancelEmploymentTypeConfirmationCancelClicked(object sender, EventArgs e)
        {
            CancelWorkFlowActivity();
        }

        private void CancelWorkFlowActivity()
        {
            X2Service.CancelActivity(_view.CurrentPrincipal);
            this.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        private void _view_OnConfirmEmploymentTypeClicked(object sender, EventArgs e)
        {
            ValidateViewInput();
            if (_view.IsValid == false)
                return;

            CompleteWorkflowActivityAndNavigate();
        }

        private void CompleteWorkflowActivityAndNavigate()
        {
            var selectedEmploymentTypeKey = _view.SelectedEmploymentTypeKey;
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            // Pass the SelectedEmploymentTypeKey to the workflow engine
            this.X2Service.CompleteActivity(_view.CurrentPrincipal, null, spc.IgnoreWarnings, selectedEmploymentTypeKey);
            this.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        private void ValidateViewInput()
        {
            if (_view.SelectedEmploymentTypeKey < 0)
            {
                var err = "Please select a valid employment type.";
                _view.Messages.Add(new Error(err, err));
            }
        }
    }
}