using System;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using System.Linq;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    public class DebtCounsellingPersonalLoansCancelled : DebtCounsellingCancelledBase
    {
        public DebtCounsellingPersonalLoansCancelled(IDebtCounsellingCancelled view, SAHLCommonBaseController controller)
            : base(view, controller)
        { }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            var reason = Reasons.Where(x => x.Value == "Bond Excluded In Arrears").SingleOrDefault();
            if (reason.Key != 0)
                Reasons.Remove(reason.Key);
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
            if (_view.IsValid && _view.SelectedReasonDefinitionKey > 0) // only save if reason has been selected
            {
                TransactionScope ts = new TransactionScope();
                try
                {
                    // save the selected reason
                    IReason res = _reasonRepo.CreateEmptyReason();
                    res.Comment = "";
                    res.GenericKey = _genericKey;
                    res.ReasonDefinition = _reasonRepo.GetReasonDefinitionByKey(Convert.ToInt32(_view.SelectedReasonDefinitionKey));
                    _reasonRepo.SaveReason(res);

                    _insertedReasonKey = res.Key;

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