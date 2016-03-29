using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using SAHL.Web.Views.Capitec.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.X2.Framework.Interfaces;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.UI;
using Castle.ActiveRecord;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using System.Collections.Generic;


namespace SAHL.Web.Views.Capitec.Presenters
{
    public class ContactClient : SAHLCommonBasePresenter<IContactClient>
	{
        protected int genericKey;
        protected int genericKeyTypeKey;
        protected int insertedReasonKey;
        protected CBOMenuNode node;

        protected IReasonRepository reasonRepository;
        protected IStageDefinitionRepository stageDefinitionRepository;

        public ContactClient(IContactClient view, SAHLCommonBaseController controller)
			: base(view, controller)
		{

		}

		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			base.OnViewInitialised(sender, e);
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

            reasonRepository = RepositoryFactory.GetRepository<IReasonRepository>();
            stageDefinitionRepository = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
		}

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            if (!_view.ShouldRunPage)
                return;

            if (PrivateCacheData.ContainsKey("ContactDate") == false)
            {
                PrivateCacheData.Add("ContactDate", _view.ContactDate);
                _view.ContactDate = System.DateTime.Now;
            }
        }

        void _view_CancelButtonClicked(object sender, EventArgs e)
        {
            CancelActivity();
        }

        void CancelActivity()
        {
            base.X2Service.CancelActivity(_view.CurrentPrincipal);
            base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        void _view_SubmitButtonClicked(object sender, EventArgs e)
        {
            if (ValidateInput() == false)
                return;

            TransactionScope ts = new TransactionScope();
            try
            {
                // save the reason
                IReason reason = reasonRepository.CreateEmptyReason();
                reason.Comment = _view.Comments;
                reason.GenericKey = genericKey;

                IReadOnlyEventList<IReasonDefinition> reasonDefinitions = reasonRepository.GetReasonDefinitionsByReasonTypeKey((int)ReasonTypes.ContactWithClient);
                reason.ReasonDefinition = reasonDefinitions[0];

                reasonRepository.SaveReason(reason);
                insertedReasonKey = reason.Key;

                ts.VoteCommit();
            }
            catch (Exception)
            {
                CancelActivity();
                ts.VoteRollBack();

                if (_view.IsValid)
                    throw;
            }
            finally
            {
                ts.Dispose();
            }

            if (_view.Messages.Count > 0)
                return;

            CompleteActivityAndNavigate();
        }

        void CompleteActivityAndNavigate()
        {
            TransactionScope txn = new TransactionScope();
            try
            {
                X2ServiceResponse rsp = base.X2Service.CompleteActivity(_view.CurrentPrincipal, null, false);

                // we need to update the inserted reason with the stage transition key
                IStageTransition stageTransition = stageDefinitionRepository.GetLastStageTransitionByGenericKeyAndSDSDGKey(genericKey, genericKeyTypeKey, (int)SAHL.Common.Globals.StageDefinitionStageDefinitionGroups.CapitecClientContacted);
                if (stageTransition != null && insertedReasonKey > 0)
                {
                    IReason reason = reasonRepository.GetReasonByKey(insertedReasonKey);

                    if (reason != null)
                    {
                        reason.StageTransition = stageTransition;
                        reasonRepository.SaveReason(reason);
                    }

                    // update the stage transition ewith the contact date the user has input
                    stageTransition.EndTransitionDate = _view.ContactDate;
                    stageTransition.Comments = "Contacted on : " + _view.ContactDate.Value.ToString(SAHL.Common.Constants.DateFormat);
                    stageDefinitionRepository.SaveStageTransition(stageTransition);
                }                                

                base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);

                txn.VoteCommit();
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                txn.Dispose();
            }
        }

        private bool ValidateInput()
        {
            bool valid = true;
            string errorMessage = "";

            if (!_view.ContactDate.HasValue)
            {
                errorMessage = "Contact Date must be entered";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
                valid = false;
            }

            if (_view.ContactDate.HasValue && _view.ContactDate.Value > DateTime.Now)
            {
                errorMessage = "Contact Date cannot be greater than today";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
                valid = false;
            }

            if (String.IsNullOrEmpty(_view.Comments.Trim()))
            {
                errorMessage = "Comments must captured";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
                valid = false;
            }

            return valid;
        }
	}
}
