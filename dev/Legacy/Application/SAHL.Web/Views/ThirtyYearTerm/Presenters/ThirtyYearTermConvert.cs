using Castle.ActiveRecord;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.ThirtyYearTerm.Interfaces;
using System;

namespace SAHL.Web.Views.ThirtyYearTerm.Presenters
{
    /// <summary>
    ///
    /// </summary>
    public class ThirtyYearTermConvert : ThirtyYearTermDetail
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ThirtyYearTermConvert(IThirtyYearTermDetail view, SAHLCommonBaseController controller)
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

            _view.OnSubmitButtonClicked += new EventHandler(OnSubmitButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.ShowSubmitButton = _view.ApplicationQualifiesFor30Year;
            _view.ShowCancelButton = true;
            _view.SubmitButtonText = "Convert to 30 Year Term";
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            base.X2Service.CancelActivity(_view.CurrentPrincipal);
            base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        private void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope txn = new TransactionScope();
            try
            {
                ApplicationRepo.ConvertAcceptedApplicationToExtendedTerm(Application, DecisionTreeResult.LoanDetailFor30YearTerm.Term.Value, DecisionTreeResult.QualifiesForThirtyYearTerm, DecisionTreeResult.PricingAdjustmentThirtyYear);
                X2ServiceResponse rsp = base.X2Service.CompleteActivity(_view.CurrentPrincipal, null, false);
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
    }
}