using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Castle.ActiveRecord;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.X2.Framework.Interfaces;

namespace SAHL.Web.Views.Common.Presenters
{
    public class SuperLoOptOut : SAHLCommonBasePresenter<ISuperLoLoyaltyInfo>
    {
        private IAccount account;
        //private IAccountRepository _accRepo;
        private IMortgageLoan _mlVar;
        private ISuperLo superLo;
        //private IEventList<IMortgageLoanInfo> mlInfo;
        private IReadOnlyEventList<IFinancialService> _mlVarLst;
        private CBONode _node;
        protected Int64 _instanceID;
        private int _genericKey;
        private int _genericKeyType;

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

        private IMortgageLoanRepository mottgageloanrepository;

        protected IMortgageLoanRepository MLR
        {
            get
            {
                if (mottgageloanrepository == null)
                    mottgageloanrepository = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
                return mottgageloanrepository;
            }
        }

        private IApplicationRepository apprepository;

        protected IApplicationRepository appRepo
        {
            get
            {
                if (apprepository == null)
                    apprepository = RepositoryFactory.GetRepository<IApplicationRepository>();
                return apprepository;
            }
        }

        private IAccountRepository accrepository;

        protected IAccountRepository accRepo
        {
            get
            {
                if (accrepository == null)
                    accrepository = RepositoryFactory.GetRepository<IAccountRepository>();
                return accrepository;
            }
        }

        /// <summary>
        /// Constructor for Super Lo Op Out
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public SuperLoOptOut(ISuperLoLoyaltyInfo view, SAHLCommonBaseController controller)
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
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            if (_node != null)
            {
                _genericKey = Convert.ToInt32(_node.GenericKey);
                _genericKeyType = Convert.ToInt32(_node.GenericKeyTypeKey);

                switch (_genericKeyType)
                {
                    case (int)GenericKeyTypes.Account:
                        account = accRepo.GetAccountByKey(_genericKey);
                        break;
                    case (int)GenericKeyTypes.ParentAccount:
                        account = accRepo.GetAccountByKey(_genericKey);
                        break;
                    case (int)GenericKeyTypes.LegalEntity:
                        break;
                    case (int)GenericKeyTypes.RelatedLegalEntity:
                        break;
                    case (int)GenericKeyTypes.Valuation:
                        break;
                    case (int)GenericKeyTypes.Offer:
                        account = accRepo.GetAccountByApplicationKey(_genericKey);
                        break;
                    case (int)GenericKeyTypes.CapOffer:
                        break;
                    case (int)GenericKeyTypes.FinancialService:
                        IFinancialService fs = FSR.GetFinancialServiceByKey(Convert.ToInt32(_genericKey));
                        if (fs != null && fs.Account != null)
                        {
                            account = fs.Account;
                        }
                        break;
                }
            }
            //  _accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

            //if (_node != null)
            //{
            //    IFinancialService fs = FSR.GetFinancialServiceByKey(Convert.ToInt32(_node.GenericKey));
            //    if (fs != null && fs.Account != null)
            //    {
            //        account = fs.Account;
            //    }
            //}

            if (account != null)
            {
                IApplication application = appRepo.GetApplicationByKey(_genericKey);
                if (application != null)
                {
                    svc.ExecuteRule(spc.DomainMessages, "SuperLoOptOutCheck", application);
                }
                if (_view.IsValid)
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
                    //if (_mlVar != null)
                    //{
                    //    mlInfo = _mlVar.MortgageLoanInfoes;

                    //    for (int i = 0; i < mlInfo.Count; i++)
                    //    {
                    //        if (_mlVar.MortgageLoanInfoes[i].GeneralStatusKey == (int)GeneralStatuses.Active)
                    //            superLo = _mlVar.MortgageLoanInfoes[i];

                    var superLoAccount = account as IAccountSuperLo;
                    if (superLoAccount != null)
                    {
                        superLo = superLoAccount.SuperLo;

                        if (superLo != null)
                        {
                            View.BindLoyaltyBenefitInfo(superLo);

                            //  if (mlInfoOpen.OverPaymentAmount != null)
                            //    {
                            if (superLo.Exclusion != null)
                            {
                                _view.ExcludeFromOptOut = superLo.Exclusion.Value;
                                _view.ExclusionDate = superLo.ExclusionEndDate; //? superLo.ExclusionEndDate.Value : DateTime.Now;
                            }
                            if (superLo.ExclusionReason != null)
                            {
                                View.ExclusionReason = superLo.ExclusionReason;
                            }
                            //  }
                        }
                    }
                    //    }
                    //}
                }
            }
            _view.CancelButtonClicked += _view_CancelButtonClicked;
            if (_view.IsValid)
            {
                _view.UpdateButtonClicked += _view_UpdateButtonClicked;
                _view.SuperLoOptOutButtonClicked += _view_SuperLoOptOutButtonClicked;
                //_view.SetThresholdManagementEditable = true;

                _view.SuperLoOptOutButtonVisible = true;
                _view.ThresholdManagementPanelVisible = false;
                _view.AddOptOutConfirmation();

                InstanceNode _Inode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
                if (_Inode == null)
                    throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

                _instanceID = _Inode.InstanceID;
            }
            else
            {
                //disable all controls
                _view.SuperLoOptOutButtonVisible = false;
                _view.PrepaymentThresholdsPanelVisible = false;
                _view.LoyaltyBenefitPanelVisible = false;
                _view.ThresholdManagementPanelVisible = false;
                _view.LoyaltyPaymentGridVisible = false;
                _view.CreateSpaceTable = true;
            }
        }

        private void _view_CancelButtonClicked(object sender, EventArgs e)
        {
            //cancel activity
            X2Service.CancelActivity(_view.CurrentPrincipal);
            //Navigate();
            this.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        private void _view_SuperLoOptOutButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (_view.IsValid)
                {
                    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                    this.X2Service.CompleteActivity(_view.CurrentPrincipal, null, spc.IgnoreWarnings);
                    this.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
                }
                //View.Messages.Add(new Warning("All benefits of the Super Lo product will be forfeited. Are you sure?", "Confirmation of Super Lo Opt Out."));
            }
            catch (Exception)
            {
                // we must cancel the activity here, otherwise if the user navigates to another node and
                // tries to perform a workflow action, X2 may try to perform the action on the wrong
                // activity
                if (_view.IsValid)
                {
                    this.X2Service.CancelActivity(_view.CurrentPrincipal);
                    throw;
                }
            }
        }

        private void _view_UpdateButtonClicked(object sender, EventArgs e)
        {
            // Get the values back for the update and update the Mortgage record
            for (int i = 0; i < account.FinancialServices.Count; i++)
            {
                if (account.FinancialServices[i].FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan)
                    if (account.FinancialServices[i].AccountStatus.Key == (int)GeneralStatusKey.Active)
                    {
                        // Robins New Code

                        //for (int j = 0; j < _mlVar.MortgageLoanInfoes.Count; j++)
                        //{
                        //    if (_mlVar.MortgageLoanInfoes[j].GeneralStatusKey == (int)GeneralStatusKey.Active)
                        //    {
                        if (View.ExclusionDate == null)
                        {
                            View.Messages.Add(new Warning("Please add an Exclusion End Date before updating.", "Please add an Exclusion End Date before updating."));
                        }
                        else
                        {
                            //_mlVar.MortgageLoanInfoes[j].ExclusionEndDate = View.ExclusionDate;
                            superLo.ExclusionEndDate = View.ExclusionDate;
                        }

                        //_mlVar.MortgageLoanInfoes[j].Exclusion = View.ExcludeFromOptOut ? "Y" : "N";
                        superLo.Exclusion = View.ExcludeFromOptOut;

                        if (View.ExclusionReason.Length == 0)
                        {
                            View.Messages.Add(new Warning("Please add an Exclusion Reason before updating.", "Please add an Exclusion Reason before updating."));
                        }
                        else
                        {
                            //_mlVar.MortgageLoanInfoes[j].ExclusionReason = View.ExclusionReason;
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
                        //    }

                        //}

                        //-----------------------------------------------------------------
                    }
            }
        }
    }
}