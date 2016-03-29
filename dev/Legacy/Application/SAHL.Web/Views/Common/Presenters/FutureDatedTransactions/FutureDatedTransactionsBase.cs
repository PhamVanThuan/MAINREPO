using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.FutureDatedTransactions
{
    /// <summary>
    ///
    /// </summary>
    public class FutureDatedTransactionsBase : SAHLCommonBasePresenter<IFutureDatedTransactions>
    {
        #region Properties

        protected IFinancialServiceRepository _financialServiceRepo;
        protected CBOMenuNode _node;
        protected IManualDebitOrderRepository _manualDebitOrderRepository;
        private IEventList<IManualDebitOrder> _recurringTransactionsCurrent;
        private IFinancialService _financialService;
        private IAccount _account;

        /// <summary>
        ///
        /// </summary>
        protected IEventList<IManualDebitOrder> RecurringTransactions
        {
            get
            {
                _recurringTransactionsCurrent = ManualDebitOrderRepository.GetPendingManualDebitOrdersByFinancialServiceKey(FinancialService.Key);
                //_recurringTransactionsCurrent.Sort(
                //    delegate(IManualDebitOrder manDebitOrder1, IManualDebitOrder manDebitOrder2)
                //    {
                //        return DateTime.Compare(manDebitOrder1.ActionDate, manDebitOrder1.ActionDate);
                //    });

                return _recurringTransactionsCurrent;
            }
            set
            {
                _recurringTransactionsCurrent = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected IFinancialService FinancialService
        {
            get
            {
                if (_financialService == null)
                {
                    _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
                    switch (_node.GenericKeyTypeKey)
                    {
                        case (int)GenericKeyTypes.ParentAccount:
                        case (int)GenericKeyTypes.Account:
                            {
                                int _accountKey = Convert.ToInt32(_node.GenericKey);
                                IAccountRepository _accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                                IAccount acc = _accountRepo.GetAccountByKey(_accountKey);
                                _financialService = acc.FinancialServices[0];
                                break;
                            }
                        default:
                            {
                                int fsKey = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal).GenericKey;
                                _financialService = FinancialServiceRepo.GetFinancialServiceByKey(fsKey);
                                break;
                            }
                    }
                }
                return _financialService;
            }
            set
            {
                _financialService = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected IFinancialServiceRepository FinancialServiceRepo
        {
            get
            {
                if (_financialServiceRepo == null)
                    _financialServiceRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
                return _financialServiceRepo;
            }
        }

        protected IManualDebitOrderRepository ManualDebitOrderRepository
        {
            get
            {
                if (_manualDebitOrderRepository == null)
                {
                    _manualDebitOrderRepository = RepositoryFactory.GetRepository<IManualDebitOrderRepository>();
                }
                return _manualDebitOrderRepository;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public CBOMenuNode Node
        {
            get
            {
                return _node;
            }
            set
            {
                _node = value;
            }
        }

        #endregion Properties

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public FutureDatedTransactionsBase(IFutureDatedTransactions view, SAHLCommonBaseController controller)
            : base(view, controller)
        { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            IAccount account = FinancialService.Account;
            _account = account;
            _view.BindOrdersToGrid(RecurringTransactions);
            int[] roleTypes = new int[2] { (int)RoleTypes.MainApplicant, (int)RoleTypes.Suretor };
            IReadOnlyEventList<ILegalEntity> lstLegalEntities = account.GetLegalEntitiesByRoleType(_view.Messages, roleTypes);

            foreach (ILegalEntity le in lstLegalEntities)
            {
                foreach (ILegalEntityBankAccount ba in le.LegalEntityBankAccounts)
                {
                    if (ba.GeneralStatus.Key == (int)GeneralStatuses.Active)
                        _view.LegalEntityBankAccounts.Add(ba);
                }
            }

            //Run rules
            CheckRules();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.tdRequestedByVisible = false;
            _view.lblRequestedByVisible = false;
            _view.tdProcessedByVisible = false;
            _view.lblProcessedByVisible = false;
        }

        /// <summary>
        /// Gets the arrears balance for the account attached to the current financial service.
        /// </summary>
        /// <returns></returns>
        protected double AccountArrearsBalance
        {
            get
            {
                IAccountRepository _accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                IAccount account = _accountRepo.GetAccountByKey(FinancialService.Account.Key);

                if (account == null)
                    return 0D;
                else
                {
                    return account.InstallmentSummary.TotalArrearsBalance;
                }
            }
        }

        protected void PopulateManualDebitOrder(IManualDebitOrder manDebitOrder, DateTime? overridenActionDate)
        {
            var bankAccountRepo = RepositoryFactory.GetRepository<IBankAccountRepository>();
            var lookUpRepo = RepositoryFactory.GetRepository<ILookupRepository>();

            manDebitOrder.FinancialService = FinancialService;
            manDebitOrder.InsertDate = DateTime.Now;
            manDebitOrder.Amount = (_view.Amount.HasValue ? _view.Amount.Value : 0);

            if (overridenActionDate.HasValue)
            {
                manDebitOrder.ActionDate = overridenActionDate.Value;
            }
            else
            {
                manDebitOrder.ActionDate = (_view.EffectiveDate.HasValue ? _view.EffectiveDate.Value : new DateTime());
            }

            if (_view.SelectedBankAccountKey.HasValue)
            {
                manDebitOrder.BankAccount = bankAccountRepo.GetBankAccountByKey(_view.SelectedBankAccountKey.Value);
            }
            manDebitOrder.Reference = _view.Reference;
            manDebitOrder.UserID = _view.CurrentPrincipal.Identity.Name;
            manDebitOrder.GeneralStatus = lookUpRepo.GeneralStatuses[GeneralStatuses.Active];
            manDebitOrder.TransactionType = lookUpRepo.TransactionTypes.ObjectDictionary[((int)TransactionTypes.ManualDebitOrderPayment).ToString()];
        }

        protected void PopulateMemo(SAHL.Common.BusinessModel.Interfaces.IMemo memo)
        {
            var lookups = RepositoryFactory.GetRepository<ILookupRepository>();
            var orgStructureRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

            memo.GenericKey = _financialService.Key;
            memo.GenericKeyType = lookups.GenericKeyType.ObjectDictionary[((int)GenericKeyTypes.FinancialService).ToString()];
            memo.Description = _view.Note;
            memo.GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Active];
            memo.InsertedDate = DateTime.Now.Date;
            memo.ADUser = orgStructureRepo.GetAdUserForAdUserName(_view.CurrentPrincipal.Identity.Name);
            memo.ReminderDate = DateTime.Now.AddDays(1);
            memo.ExpiryDate = DateTime.Now.AddDays(1);
        }

        protected bool SaveRecurringTransaction(IManualDebitOrder manDebitOrder)
        {
            var memoRepository = RepositoryFactory.GetRepository<IMemoRepository>();
            var memo = memoRepository.CreateMemo(); // Create a new
            PopulateMemo(memo);

            TransactionScope ts = new TransactionScope();
            try
            {
                if (!String.IsNullOrEmpty(_view.Note))
                {
                    memoRepository.SaveMemo(memo);
                }
                else
                {
                    memo = null;
                }

                if (manDebitOrder.Key > 0) // If this is an update, cancel current debit order, and create new one.
                {
                    ManualDebitOrderRepository.CancelManualDebitOrder(manDebitOrder);

                    var newManDebitOrder = ManualDebitOrderRepository.GetEmptyManualDebitOrder();
                    PopulateManualDebitOrder(newManDebitOrder, null);
                    newManDebitOrder.Memo = memo;
                    ManualDebitOrderRepository.SaveManualDebitOrder(newManDebitOrder);
                }
                else
                {
                    PopulateManualDebitOrder(manDebitOrder, null);
                    manDebitOrder.Memo = memo;
                    ManualDebitOrderRepository.SaveManualDebitOrder(manDebitOrder);
                }

                ts.VoteCommit();
                return true;
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

            return false;
        }

        public void CheckRules()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            svc.ExecuteRule(_view.Messages, "NaedoDebitOrderPendingWarning", _account);
        }
    }
}