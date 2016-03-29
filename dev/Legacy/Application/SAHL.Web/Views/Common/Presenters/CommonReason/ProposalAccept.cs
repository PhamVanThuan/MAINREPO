using System;
using System.Data;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using System.Collections.Generic;
using SAHL.Common.CacheData;
using Castle.ActiveRecord;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common.Presenters.CommonReason
{
    /// <summary>
    /// 
    /// </summary>
    public class ProposalAccept : CommonReasonProposalBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ProposalAccept(ICommonReason view, SAHLCommonBaseController controller)
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

            _view.CancelButtonVisible = true;

            // limit the selection to one reason
            _view.OnlyOneReasonCanBeSelected = true;
        }

        /// <summary>
        /// Overrides the base OnSubmitButtonClicked event so that specific credit decline actions can be performed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void _view_OnSubmitButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            SelectedReason selectedReason = ((List<SelectedReason>)e.Key)[0];
            IReasonDefinition selectedReasonDefinition = _reasonRepo.GetReasonDefinitionByKey(selectedReason.ReasonDefinitionKey);

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            List<string> rulesToRun = new List<string>();
            rulesToRun.Add("DebtCounsellingProposalTermLimit");
            svc.ExecuteRuleSet(spc.DomainMessages, rulesToRun, base.DebtCounselling, selectedReasonDefinition);

            if (_view.IsValid)
            {
                base.InsertedReasonKeys = new List<int>();

                TransactionScope ts = new TransactionScope();

                try
                {
                    // populate and save the reason
                    IReason res = _reasonRepo.CreateEmptyReason();
                    res.Comment = selectedReason.Comment;
                    res.GenericKey = base.ActiveProposal.Key;
                    res.ReasonDefinition = _reasonRepo.GetReasonDefinitionByKey(selectedReason.ReasonDefinitionKey);
                    _reasonRepo.SaveReason(res);
                    base.InsertedReasonKeys.Add(res.Key);

                    // update the active proposal record to accepted and ceate account snapshot
                    base.ActiveProposal.Accepted = true;
                    base.DebtCounsellingRepo.CreateAccountSnapShot(base.ActiveProposal.DebtCounselling.Key);
                    base.DebtCounsellingRepo.SaveProposal(base.ActiveProposal);
                    
                    // complete the activity
                    CompleteActivityAndNavigate();

                    ts.VoteCommit();
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
        }

        public override void CancelActivity()
        {
            base.CancelActivity();
        }

        public override void CompleteActivityAndNavigate()
        {
            base.CompleteActivityAndNavigate();
        }
    }
}
