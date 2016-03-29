using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    public class PostTransactionBase : SAHLCommonBasePresenter<IPostTransaction>
    {
        private CBONode _cboNode;
        private int _accountKey;
        private IReadOnlyEventList<ITransactionType> _transactionTypes;
        private IAccountRepository _accRepo;
        private IBulkBatchRepository _bulkBatchRepo;
        private ILoanTransactionRepository _LoanTranRepo;

        public int AccountKey { get { return _accountKey; } }

        public IAccountRepository AccRepo
        {
            get
            {
                if (_accRepo == null)
                {
                    _accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                }
                return _accRepo;
            }
        }

        public IBulkBatchRepository BulkBatchRepo
        {
            get
            {
                if (_bulkBatchRepo == null)
                {
                    _bulkBatchRepo = RepositoryFactory.GetRepository<IBulkBatchRepository>();
                }
                return _bulkBatchRepo;
            }
        }

        public ILoanTransactionRepository LoanTranRepo
        {
            get
            {
                if (_LoanTranRepo == null)
                {
                    _LoanTranRepo = RepositoryFactory.GetRepository<ILoanTransactionRepository>();
                }
                return _LoanTranRepo;
            }
        }

        public IReadOnlyEventList<ITransactionType> TransactionTypeList
        {
            get { return _transactionTypes; }
            set { _transactionTypes = value; }
        }

        public PostTransactionBase(IPostTransaction view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            //Get the CBO Node
            _cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_cboNode == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _accountKey = Convert.ToInt32(_cboNode.GenericKey);
            _view.BindTransactionTypes(TransactionTypeList);
            _view.onSelectedTransctionTypeChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(OnSelectedTransctionTypeChanged);
            BindFinancialService();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectedTransctionTypeChanged(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            _view.FinancialServicesVisible = !IsTransactionPrePayment;
        }

        /// <summary>
        ///
        /// </summary>
        protected void PostTransaction()
        {
            if (IsTransactionPrePayment)
            {
                // Post on the Account Level
                LoanTranRepo.PostTransactionByAccountKey(_accountKey, (short)_view.TransactionType, _view.EffectiveDate, _view.Amount, _view.Reference, _view.CurrentPrincipal.Identity.Name);
            }
            else
            {
                // Post on the Financial Service Level
                LoanTranRepo.PostTransactionByFinancialServiceKey(_view.SelectedFinancialServiceKey, _view.TransactionType, _view.EffectiveDate, _view.Amount, _view.Reference, _view.CurrentPrincipal.Identity.Name);
            }
        }

        #region Helper Methods

        /// <summary>
        ///
        /// </summary>
        private void BindFinancialService()
        {
            IAccount acct = AccRepo.GetAccountByKey(_accountKey);
            Dictionary<string, string> dictFinancialServices = new Dictionary<string, string>();

            foreach (IFinancialService fs in acct.FinancialServices)
            {
                if ((fs.AccountStatus.Key == (int)AccountStatuses.Open) &&
                    (fs.FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan
                    || fs.FinancialServiceType.Key == (int)FinancialServiceTypes.FixedLoan
                    || fs.FinancialServiceType.Key == (int)FinancialServiceTypes.PersonalLoan))
                {
                    dictFinancialServices.Add(fs.Key.ToString(), fs.FinancialServiceType.Description);
                }
            }

            _view.BindFinancialServiceTypes(dictFinancialServices);
        }

        /// <summary>
        ///
        /// </summary>
        private bool IsTransactionPrePayment
        {
            get
            {
                ITransactionType tranType = BulkBatchRepo.GetTransactionTypeByKey(_view.TransactionType);
                bool isPrePayment = false;

                foreach (var item in tranType.TransactionGroups)
                {
                    if (item.Key == (int)TransactionGroups.Payment)
                    {
                        {
                            isPrePayment = true;
                            break;
                        }
                    }
                }
                return isPrePayment;
            }
        }

        protected void Validate()
        {
            string errorMessage = string.Empty;

            if (_view.TransactionType == -1)
            {
                errorMessage = "Please select a transaction type.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }

            if (_view.TransactionType != -1 && !IsTransactionPrePayment && _view.SelectedFinancialServiceKey == -1)
            {
                errorMessage = "Please select a financial service.";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }

            if (!_view.IsValid)
                throw new DomainValidationException();
        }

        #endregion Helper Methods
    }
}