using System;
using System.Collections.Generic;
using System.Data;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// SuperLoLoyaltyInfo presenter
    /// </summary>
    public class SuperLoLoyaltyInfo : SAHLCommonBasePresenter<ISuperLoLoyaltyInfo>
    {
        private IAccount account;
        private IMortgageLoan _mlVar;
        private ISuperLo superLo;
        private IReadOnlyEventList<IFinancialService> _mlVarLst;
        private CBONode _node;

        private IFinancialServiceRepository financialServiceRepository;

        protected IFinancialServiceRepository FSR
        {
            get
            {
                if (financialServiceRepository == null)
                    financialServiceRepository = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
                return financialServiceRepository;
            }
        }

        private IMortgageLoanRepository mortgageloanrepository;

        protected IMortgageLoanRepository MLR
        {
            get
            {
                if (mortgageloanrepository == null)
                    mortgageloanrepository = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
                return mortgageloanrepository;
            }
        }

        /// <summary>
        /// Constrctor for LoyaltyBenefitInfo
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public SuperLoLoyaltyInfo(ISuperLoLoyaltyInfo view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// OnviewInitlised event - retrieve data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            if (_node != null)
            {
                IFinancialService fs = FSR.GetFinancialServiceByKey(Convert.ToInt32(_node.GenericKey));
                if (fs != null && fs.Account != null)
                {
                    account = fs.Account;
                }
            }

            if (account != null)
            {
                for (int i = 0; i < account.FinancialServices.Count; i++)
                {
                    if (account.FinancialServices[i].FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan)
                    {
                        List<int> tranTypes = new List<int>();
                        tranTypes.Add((int)TransactionTypes.LoyaltyBenefitPayment); // 237
                        tranTypes.Add((int)TransactionTypes.LoyaltyBenefitPaymentCorrection); // 1237
                        DataTable dtLoanTransactions = account.FinancialServices[i].GetTransactions(_view.Messages, (int)GeneralStatuses.Active, tranTypes);
                        _view.BindLoyaltyBenefitPaymentGrid(dtLoanTransactions);
                    }
                }

                _mlVarLst = account.GetFinancialServicesByType(FinancialServiceTypes.VariableLoan, new AccountStatuses[] { AccountStatuses.Open });

                if (_mlVarLst.Count == 0)
                    _mlVarLst = account.GetFinancialServicesByType(FinancialServiceTypes.VariableLoan, new AccountStatuses[] { AccountStatuses.Closed });
                if (_mlVarLst.Count != 0) _mlVar = _mlVarLst[0] as IMortgageLoan;

                var superLoAccount = account as IAccountSuperLo;
                if (superLoAccount != null)
                {
                    superLo = superLoAccount.SuperLo;

                    View.BindLoyaltyBenefitInfo(superLo);

                    if (superLo.Exclusion != null && superLo.Exclusion == true)
                    {
                        _view.ExcludeFromOptOut = superLo.Exclusion.Value;
                        _view.ExclusionDate = superLo.ExclusionEndDate.HasValue ? superLo.ExclusionEndDate.Value : DateTime.Now;
                    }
                    if (superLo.ExclusionReason != null)
                        View.ExclusionReason = superLo.ExclusionReason;
                }
            }
            _view.CancelButtonClicked += _view_CancelButtonClicked;
            _view.UpdateButtonClicked += _view_UpdateButtonClicked;

            _view.SuperLoOptOutButtonVisible = false;
            _view.UpdateThresholdButtonVisible = true;
        }

        private void _view_CancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("LoanFinancialServiceSummary");
        }

        private void _view_UpdateButtonClicked(object sender, EventArgs e)
        {
            // Get the values back for the update and update the Mortgage record
            for (int i = 0; i < account.FinancialServices.Count; i++)
            {
                if (account.FinancialServices[i].FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan)
                {
                    if (account.FinancialServices[i].AccountStatus.Key == (int)GeneralStatusKey.Active)
                    {
                        if (View.ExclusionDate == null)
                        {
                            View.Messages.Add(new Warning("Please add an Exclusion End Date before updating.", "Please add an Exclusion End Date before updating."));
                        }
                        else
                        {
                            superLo.ExclusionEndDate = View.ExclusionDate;
                        }

                        superLo.Exclusion = View.ExcludeFromOptOut;

                        if (View.ExclusionReason.Length == 0)
                        {
                            View.Messages.Add(new Warning("Please add an Exclusion Reason before updating.", "Please add an Exclusion Reason before updating."));
                        }
                        else
                        {
                            superLo.ExclusionReason = View.ExclusionReason;
                            TransactionScope txn = new TransactionScope();
                            try
                            {
                                MLR.SaveMortgageLoan(_mlVar);
                                txn.VoteCommit();
                            }

                            catch (Exception)
                            {
                                txn.VoteRollBack();
                                if (View.IsValid)
                                    throw;
                            }
                            finally
                            {
                                txn.Dispose();
                                _view.Navigator.Navigate("LoanFinancialServiceSummary");
                            }
                        }
                    }
                }
            }
        }
    }
}