using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.DataSets;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common.Presenters
{
    public class AmortisationSchedule : SAHLCommonBasePresenter<IAmortisationSchedule>
    {
        private Dictionary<string, double> _calcDict;
        private ILoanTransactionRepository loanTransactionRepo;

        public ILoanTransactionRepository LoanTransactionRepo
        {
            get
            {
                if (loanTransactionRepo == null)
                {
                    loanTransactionRepo = RepositoryFactory.GetRepository<ILoanTransactionRepository>();
                }

                return loanTransactionRepo;
            }
        }

        public AmortisationSchedule(IAmortisationSchedule view, SAHLCommonBaseController controller)
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

            if (!_view.ShouldRunPage) return;

            _view.OnBackClicked += new EventHandler(_view_OnBackClicked);

            //check the required object data exists
            if (!GlobalCacheData.ContainsKey(ViewConstants.AmortisationSchedule))
            {
                _view.Messages.Add(new Error("Amortise data does not exist.", "Amortise data does not exist."));
                return;
            }

            _calcDict = (Dictionary<string, double>)GlobalCacheData[ViewConstants.AmortisationSchedule];

            if (_calcDict.ContainsKey("CurrentBalanceV"))
                _view.CurrentBalanceV = _calcDict["CurrentBalanceV"];
            if (_calcDict.ContainsKey("InterestRateV"))
                _view.InterestRateV = _calcDict["InterestRateV"];
            if (_calcDict.ContainsKey("InstalmentTotalV"))
                _view.InstalmentTotalV = _calcDict["InstalmentTotalV"];
            if (_calcDict.ContainsKey("RemainingTermV"))
                _view.RemainingTermV = Convert.ToInt32(_calcDict["RemainingTermV"]);

            if (_calcDict.ContainsKey("CurrentBalanceF"))
                _view.CurrentBalanceF = _calcDict["CurrentBalanceF"];
            if (_calcDict.ContainsKey("InstalmentTotalF"))
                _view.InstalmentTotalF = _calcDict["InstalmentTotalF"];
            if (_calcDict.ContainsKey("InterestRateF"))
                _view.InterestRateF = _calcDict["InterestRateF"];
            if (_calcDict.ContainsKey("RemainingTermF"))
                _view.RemainingTermF = Convert.ToInt32(_calcDict["RemainingTermF"]);

            _view.DisplayFixedAndVariableGrids = false;

            if (GlobalCacheData.ContainsKey(ViewConstants.GenericCalc))
                _view.DisplayLoanValues = true;
            else
                _view.DisplayLoanValues = false;

            if (_view.CurrentBalanceF > 0)
                _view.DisplayFixedAndVariableGrids = true;

            if (_view.CurrentBalanceV > 0)
            {
                //Populate the dataset and bind to the grid
                LoanCalculations.AmortisationScheduleDataTable dt = LoanTransactionRepo.GenerateAmortiseData(_view.CurrentBalanceV, _view.InstalmentTotalV, _view.InterestRateV);
                _view.BindAmortisationGrid((int)FinancialServiceTypes.VariableLoan, dt);
            }

            if (_view.CurrentBalanceF > 0)
            {
                //Populate the dataset and bind to the grid
                LoanCalculations.AmortisationScheduleDataTable dt = LoanTransactionRepo.GenerateAmortiseData(_view.CurrentBalanceF, _view.InstalmentTotalF, _view.InterestRateF);
                _view.BindAmortisationGrid((int)FinancialServiceTypes.FixedLoan, dt);
            }
        }

        private void _view_OnBackClicked(object sender, EventArgs e)
        {
            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo))
            {
                Navigator.Navigate((string)GlobalCacheData[ViewConstants.NavigateTo]);
                GlobalCacheData.Remove(ViewConstants.NavigateTo);
            }
        }
    }
}