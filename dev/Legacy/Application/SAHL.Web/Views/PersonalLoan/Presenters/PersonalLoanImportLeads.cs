using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.PersonalLoan.Interfaces;
using System;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    public class PersonalLoanImportLeads : SAHLCommonBasePresenter<IPersonalLoanImportLeads>
    {
        public PersonalLoanImportLeads(IPersonalLoanImportLeads view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage)
                return;

            _view.EditMode = true;
            this.View.OnImportClicked += _view_OnImportClicked;
        }

        private void _view_OnImportClicked(object sender, EventArgs e)
        {
            using (var tx = new TransactionScope(OnDispose.Rollback))
            {
                try
                {
                    ILeadImportPublisherService importService = ServiceFactory.GetService<ILeadImportPublisherService>();
                    importService.PublishLeadsForImport<PersonalLoanLead>(_view.File.InputStream, _view.ImportFileName);
                    tx.VoteCommit();
                }
                catch (Exception)
                {
                    if (_view.Messages.Count == 0)
                        throw;
                }
            }

            if (_view.IsValid)
            {
                _view.Navigator.Navigate("PL_LeadSummary");
            }
        }
    }
}