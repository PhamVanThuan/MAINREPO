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
    public class SuperLoReadonly : SAHLCommonBasePresenter<ISuperLoLoyaltyInfo>
    {
        private IAccount account;
        private IApplication application;
        //private IAccountRepository _accRepo;
        //private IMortgageLoan _mlVar;
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
        public SuperLoReadonly(ISuperLoLoyaltyInfo view, SAHLCommonBaseController controller)
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
                        application = appRepo.GetApplicationByKey(_genericKey);
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

            if (application != null)
            {
                IRuleService rules = ServiceFactory.GetService<IRuleService>();
                List<string> rulesToRun = new List<string>();
                rulesToRun.Add("ProductSuperLoFLSPVChange");
                rulesToRun.Add("SuperLoOptOutRequired");
                rules.ExecuteRuleSet(_view.Messages, rulesToRun, application);
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

                //if (_mlVarLst.Count != 0)
                //{
                //    _mlVar = _mlVarLst[0] as IMortgageLoan;
                //}

                var superLoAccount = account as IAccountSuperLo;
                if (superLoAccount != null)
                {
                    superLo = superLoAccount.SuperLo;

                    if (superLo != null)
                    {
                        View.BindLoyaltyBenefitInfo(superLo);

                        if (superLo.Exclusion != null)
                        {
                            _view.ExcludeFromOptOut = superLo.Exclusion.Value;

                            _view.ExclusionDate = superLo.ExclusionEndDate; //? mlInfoOpen.ExclusionEndDate : DateTime.Now;
                        }
                        if (superLo.ExclusionReason != null)
                        {
                            View.ExclusionReason = superLo.ExclusionReason;
                        }
                    }
                }
            }

            _view.SuperLoOptOutButtonVisible = false;
            _view.CancelButtonVisible = false;
            _view.ThresholdManagementPanelVisible = false;
            _view.AddOptOutConfirmation();

            InstanceNode _Inode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            if (_Inode == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _instanceID = _Inode.InstanceID;
        }
    }
}