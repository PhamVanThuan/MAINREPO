using System;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Web.UI.Events;
using Castle.ActiveRecord;


namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// Presenter for RateChangeInstallmentChange presenter 
    /// </summary>
    public class RateChangeChangeRate : RateChangeBase
    {
        private IEventList<IMargin> margin;
        /// <summary>
        /// Constructor for RatechangeInstallmentChange
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public RateChangeChangeRate(IRateChange view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }
        /// <summary>
        /// Initialise event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            if (_mlVar != null)
                _lstMortgageLoans.Insert(_view.Messages, 0, _mlVar);

            if (_mlFixed != null)
            {
                _lstMortgageLoans.Insert(_view.Messages, 1, _mlFixed);
                _view.SetRateControlVisibilityForVarifix = true;
            }
            else
                _view.SetRateControlVisibilityForVarifix = false;

            if (_mlVar != null)
            {
                if (_mlVar.HasInterestOnly())
                {
                    _view.BindGridRateChangeInterestOnly(_lstMortgageLoans);
                    _view.SetTermControlVisibilityForInterestOnly = true;
                    _view.SetRateAmortisationControlVisibility = true;
                }
                else
                {
                    _view.BindGridRateChangeNotInterestOnly(_lstMortgageLoans);
                    _view.SetTermControlVisibilityForInterestOnly = false;
                    _view.SetRateAmortisationControlVisibility = false;
                }

                margin = _mlVar.GetAllMargins();
            }

            if (margin != null) _view.PopulateLinkRates(margin);

            _view.SetTermControlsVisibility = false;
            _view.SetRatesControlVisibility = true;
            _view.SetTermControlVisibilityForTermComment = false;
            _view.SetSubmitButtonText("Change Rate", "R");
            _view.SetAbilityofCalculateButton = false;

            // do not attach the buttons if there is no data
            if (margin != null && _view.IsValid)
                _view.SubmitButtonClicked += (_view_SubmitButtonClicked);

            _view.CancelButtonClicked += (_view_CancelButtonClicked);
        }

        void _view_SubmitButtonClicked(object sender, KeyChangedEventArgs e)
        {
            IDisbursementRepository disbRepo = RepositoryFactory.GetRepository<IDisbursementRepository>();

            TransactionScope txn = new TransactionScope();
            try
            {
                if (_mlVar != null)
                {
                    //The repo method does the work for all financial services (variable & fixed if it exists)
                    //The repo also saves all the necessary and new objects
                    //The first key is the blank
                    if(Convert.ToInt32(e.Key) > 0)
                    disbRepo.UpdateRate(_account, margin[Convert.ToInt32(e.Key) - 1], _view.GetLoggedOnUser, false);
                }
                txn.VoteCommit();

                if (_view.IsValid)
                    Navigator.Navigate("Submit");
            }
            catch (Exception)
            {
                txn.VoteRollBack();

                if (_view.IsValid)
                    throw;  // not a domain validation exception.
            }
            finally
            {
                txn.Dispose();
            }
        }
        void _view_CancelButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("Cancel");
        }
    }
}