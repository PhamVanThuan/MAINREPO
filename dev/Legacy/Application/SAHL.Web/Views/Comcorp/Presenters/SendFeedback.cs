using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.V3.Framework;
using SAHL.V3.Framework.Services;
using SAHL.Web.Views.Comcorp.Interfaces;
using System;

namespace SAHL.Web.Views.Comcorp.Presenters
{
    public class SendFeedback : SAHLCommonBasePresenter<ISendFeedback>
    {
        protected int genericKey;
        protected int genericKeyTypeKey;
        protected CBOMenuNode node;
        private IV3ServiceManager v3ServiceManager;
        private ICommunicationService communicationService;

        protected IApplicationRepository applicationRepository;

        public SendFeedback(ISendFeedback view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            v3ServiceManager = V3ServiceManager.Instance;
            communicationService = v3ServiceManager.Get<ICommunicationService>();

            if (!_view.ShouldRunPage)
                return;

            node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (node != null)
            {
                genericKey = Convert.ToInt32(node.GenericKey);
                genericKeyTypeKey = Convert.ToInt32(node.GenericKeyTypeKey);
            }

            _view.SubmitButtonClicked += _view_SubmitButtonClicked;
            _view.CancelButtonClicked += _view_CancelButtonClicked;

            applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;
        }

        private void _view_CancelButtonClicked(object sender, EventArgs e)
        {
            CancelActivity();
        }

        private void CancelActivity()
        {
            base.X2Service.CancelActivity(_view.CurrentPrincipal);
            base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        private void _view_SubmitButtonClicked(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            TransactionScope ts = new TransactionScope();
            try
            {
                string comment = "Bank Feedback: " + _view.EventComment;
                IApplication app = applicationRepository.GetApplicationByKey(genericKey);
                var messages = communicationService.SendComcorpLiveReply(Guid.NewGuid(), app.Key.ToString(), "", app.Reference, comment, DateTime.Now, (int)CompcorpLiveReplyEventStatus.BankFeedback, 0, 0, 0);

                if (!messages.HasErrors)
                {
                    applicationRepository.SaveComcorpLiveRepyMemo(genericKey, comment);
                }
                else
                {
                    v3ServiceManager.HandleSystemMessages(messages);
                }

                if (_view.IsValid)
                {
                    base.X2Service.CompleteActivity(_view.CurrentPrincipal, null, false);
                    this.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
                    ts.VoteCommit();
                }
            }
            catch (Exception)
            {
                ts.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                ts.Dispose();
            }
        }

        private bool ValidateInput()
        {
            bool valid = true;

            if (String.IsNullOrWhiteSpace(_view.EventComment))
            {
                _view.Messages.Add(new Error("A comment must be captured", "A comment must be captured"));
                valid = false;
            }

            return valid;
        }
    }
}