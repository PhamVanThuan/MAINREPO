using System;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Castle.ActiveRecord;


namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// Presenter for RateChangeInstallmentChange presenter 
    /// </summary>
    public class RateChangeInstallmentChange : RateChangeBase
    {

        /// <summary>
        /// Constructor for RatechangeInstallmentChange
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public RateChangeInstallmentChange(IRateChange view, SAHLCommonBaseController controller)
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
                _lstMortgageLoans.Insert(_view.Messages, 1, _mlFixed);

            if (_mlVar != null)
                if (_mlVar.HasInterestOnly())

                    _view.BindGridInstallmentInterestOnly(_lstMortgageLoans);
                else
                    _view.BindGridInstallmentNotInterestOnly(_lstMortgageLoans);

            _view.SetTermControlVisibilityForTermComment = false;
            _view.SetTermControlsVisibility = false;
            _view.SetRatesControlVisibility = false;
            _view.SetSubmitButtonText("Change Instalment", "I");

            if (_view.IsValid)
                _view.SubmitButtonClicked += (_view_SubmitButtonClicked);
            _view.CancelButtonClicked += (_view_CancelButtonClicked);
        }


        
        void _view_SubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope txns = new TransactionScope();
            IMortgageLoanRepository mlRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
            string User = _view.GetLoggedOnUser;
            
            try
            {
                mlRepo.InstallmentChange(_mlVar.Account.Key, User);
                Navigator.Navigate("Submit");
                txns.VoteCommit();
            }
            catch (Exception)
            {
                txns.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                txns.Dispose();
            }

        }

        void _view_CancelButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("Cancel");
        }
    }
}