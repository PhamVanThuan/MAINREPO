using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    public class DebtCounsellingCancelled : DebtCounsellingCancelledBase
    {
        public DebtCounsellingCancelled(IDebtCounsellingCancelled view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

      

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            BindReasons();
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
        }

        protected override void OnCancelButtonClicked(object sender, EventArgs e)
        {
            base.OnCancelButtonClicked(sender, e);
        }

        protected override void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            //Get the selected reason
            _selectedReasonDefinitionKey = _view.SelectedReasonDefinitionKey;
            if (_view.IsValid && _selectedReasonDefinitionKey > 0) // only save if reason has been selected
            {
                TransactionScope ts = new TransactionScope();
                try
                {
                    // save the selected reason
                    IReason res = _reasonRepo.CreateEmptyReason();
                    res.Comment = "";
                    res.GenericKey = _genericKey;
                    res.ReasonDefinition = _reasonRepo.GetReasonDefinitionByKey(Convert.ToInt32(_selectedReasonDefinitionKey));
                    _reasonRepo.SaveReason(res);

                    _insertedReasonKey = res.Key;

                    // get the debt counselling record
                    IDebtCounselling debtCounselling = _dcRepo.GetDebtCounsellingByKey(_genericKey);

                    // call repo method to cancel the debt counselling case
                    // this will
                    //  - update the debt counselling status
                    //  - call the out-out sp if there is an accepted proposal
                    _dcRepo.CancelDebtCounselling(debtCounselling, _view.CurrentPrincipal.Identity.Name, DebtCounsellingStatuses.Cancelled);
                    if (!_view.IsValid)
                        return;

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
            else
            {
                string errorMessage = "Please select a reason";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }
        }

      
    }
}