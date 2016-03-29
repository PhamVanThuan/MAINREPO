using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    public class PaymentReceivedBase : SAHLCommonBasePresenter<IPayment>
    {
        public CBOMenuNode _node;
        public InstanceNode _instanceNode;

		private ILookupRepository _lookupRepository;
		public ILookupRepository LookupRepository
		{
			get
			{
				if (_lookupRepository == null)
					_lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

				return _lookupRepository;
			}
		}

		private IFutureDatedChangeRepository _futureDatedChangeRepository;
		public IFutureDatedChangeRepository FutureDatedChangeRepository
		{
			get
			{
				if (_futureDatedChangeRepository == null)
					_futureDatedChangeRepository = RepositoryFactory.GetRepository<IFutureDatedChangeRepository>();

				return _futureDatedChangeRepository;
			}
		}

		private IFinancialServiceRepository _financialServiceRepository;
		public IFinancialServiceRepository FinancialServiceRepository
		{
			get
			{
				if (_financialServiceRepository == null)
					_financialServiceRepository = RepositoryFactory.GetRepository<IFinancialServiceRepository>();

				return _financialServiceRepository;
			}
		}

        private IDebtCounselling _debtCounselling;
        public IDebtCounselling DebtCounselling
        {
            get { return _debtCounselling; }
            set { _debtCounselling = value; }
        }

		public IList<DebitOrderDetail> DebitOrderDetails { get; set; }

        private IDebtCounsellingRepository _dcRepo;
        public IDebtCounsellingRepository DCRepo
        {
            get
            {
                if (_dcRepo == null)
                    _dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                return _dcRepo;
            }
        }
        
        private int _genericKey;
        public int GenericKey
        {
            get { return _genericKey; }
            set { _genericKey = value; }
        }

        private int _genericKeyTypeKey;
        public int GenericKeyTypeKey
        {
            get { return _genericKeyTypeKey; }
            set { _genericKeyTypeKey = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public PaymentReceivedBase(IPayment view, SAHLCommonBaseController controller) : base(view, controller)
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
            if (!_view.ShouldRunPage)
                return;

			 //Get the Node   
			_node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
			if (_node == null)
			    throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

			if (_node is InstanceNode)
			{
			    InstanceNode instanceNode = _node as InstanceNode;
			    _genericKey = instanceNode.GenericKey; // this will be the debtcounsellingkey
			    _genericKeyTypeKey = instanceNode.GenericKeyTypeKey;
			}
			else
			{
			    _genericKey = _node.GenericKey;
			    _genericKeyTypeKey = _node.GenericKeyTypeKey;
			}
			
            DebtCounselling = DCRepo.GetDebtCounsellingByKey(_genericKey);
            _view.DebtCounselling = DebtCounselling;
        }

		/// <summary>
		/// Get Debit Order Details
		/// </summary>
		/// <param name="financialService"></param>
		/// <returns></returns>
		public IList<DebitOrderDetail> LoadDebitOrderDetails(IFinancialService financialService)
		{
			IList<DebitOrderDetail> debitOrderDetails = new List<DebitOrderDetail>();
			DebitOrderDetail currentDebitOrderDetail = new DebitOrderDetail();
			// add the financial service bank accounts
			foreach (IFinancialServiceBankAccount fsBankAccount in financialService.FinancialServiceBankAccounts)
			{
				if (fsBankAccount.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active)
				{
					currentDebitOrderDetail.EffectiveDate = "Current";
					currentDebitOrderDetail.FutureDatedChangeKey = "-1";
					currentDebitOrderDetail.DebitOrderDay = fsBankAccount.DebitOrderDay.ToString();
					currentDebitOrderDetail.ChangeDate = fsBankAccount.ChangeDate.ToString(SAHL.Common.Constants.DateFormat);
					currentDebitOrderDetail.PaymentType = fsBankAccount.FinancialServicePaymentType.Description;

					debitOrderDetails.Add(currentDebitOrderDetail);
				}
			}

			// add the future dated changes
			IList<IFutureDatedChange> lstFutureChanges = FutureDatedChangeRepository.GetFutureDatedChangesByGenericKey(financialService.Key, (int)SAHL.Common.Globals.FutureDatedChangeTypes.NormalDebitOrder);
			foreach (IFutureDatedChange futureDatedChange in lstFutureChanges)
			{

				if (futureDatedChange.EffectiveDate < DateTime.Today)
					continue;

				DebitOrderDetail debitOrderDetail = null;

				foreach (IFutureDatedChangeDetail futureDatedChangeDetail in futureDatedChange.FutureDatedChangeDetails)
				{
					if (futureDatedChangeDetail.TableName == "FinancialServiceBankAccount")
					{
						// check for null on the grid item - if it is then we're working with a new record and 
						// we need to load up the IFinancialServiceBankAccount to set the default values
						if (debitOrderDetail == null)
						{
							debitOrderDetail = new DebitOrderDetail();
							IFinancialServiceBankAccount detailFsBankAccount = FinancialServiceRepository.GetFinancialServiceBankAccountByKey(futureDatedChangeDetail.ReferenceKey);
							if (detailFsBankAccount != null)
							{
								debitOrderDetail.DebitOrderDay = detailFsBankAccount.DebitOrderDay.ToString();
								debitOrderDetail.ChangeDate = detailFsBankAccount.ChangeDate.ToString(SAHL.Common.Constants.DateFormat);
								debitOrderDetail.PaymentType = detailFsBankAccount.FinancialServicePaymentType.Description;
							}

							// set detail items that only need to get set once but appear on all the detail items
							debitOrderDetail.FutureDatedChangeKey = futureDatedChange.Key.ToString();
							debitOrderDetail.EffectiveDate = futureDatedChange.EffectiveDate.ToString(SAHL.Common.Constants.DateFormat);
						}

						// now we can check the unique detail item values and update anything else
						switch (futureDatedChangeDetail.ColumnName)
						{
							case "DebitOrderDay":
								{
									debitOrderDetail.DebitOrderDay = futureDatedChangeDetail.Value;
									break;
								}
							case "FinancialServicePaymentTypeKey":
								{
									debitOrderDetail.PaymentType = LookupRepository.FinancialServicePaymentTypes.ObjectDictionary[futureDatedChangeDetail.Value].Description;
									break;
								}
						}
					}
				}

				if (!String.IsNullOrEmpty(debitOrderDetail.EffectiveDate))
					debitOrderDetails.Add(debitOrderDetail);
			}
			DebitOrderDetails = debitOrderDetails;
			return debitOrderDetails;
		}

		/// <summary>
		/// Debit Order Detail
		/// </summary>
		public class DebitOrderDetail
		{
			public string EffectiveDate { get; set; }
			public string FutureDatedChangeKey { get; set; }
			public string DebitOrderDay { get; set; }
			public string ChangeDate { get; set; }
			public string PaymentType { get; set; }
		}
    }
}