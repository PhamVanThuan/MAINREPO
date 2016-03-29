using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.PersonalLoan.Interfaces;
using System;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    public class PersonalLoanLeadSummary : SAHLCommonBasePresenter<IPersonalLoanImportLeads>
    {
        private IPersonalLoanRepository plRepo;

        public IPersonalLoanRepository PLRepo
        {
            get
            {
                if (plRepo == null)
                {
                    plRepo = RepositoryFactory.GetRepository<IPersonalLoanRepository>();
                }
                return plRepo;
            }
        }

        public PersonalLoanLeadSummary(IPersonalLoanImportLeads view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            _view.Messages.Clear();
            _view.SetUpLeadSummaryGrid();
            _view.OnDownLoadClicked += OnDownLoadClicked;
            var results = PLRepo.GetBatchServiceResults(SAHL.Common.Globals.BatchServiceTypes.PersonalLoanLeadImport);
            _view.BindLeadSummaryGrid(results);
        }

 
        private void OnDownLoadClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            IBatchServiceRepository bsRepo = RepositoryFactory.GetRepository<IBatchServiceRepository>();
            var batchService = bsRepo.GetBatchService(Convert.ToInt32(e.Key));
            if (batchService != null)
            {
                _view.FileReadyForDownload(batchService);
                _view.Navigator.Navigate("PL_LeadSummary");
            }
        }
    }
}