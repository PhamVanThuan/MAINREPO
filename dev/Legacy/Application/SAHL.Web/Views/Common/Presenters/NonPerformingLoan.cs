using System;
using System.Data;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.Service.Interfaces;
using System.Linq;


namespace SAHL.Web.Views.Common.Presenters
{
	public class NonPerformingLoan : SAHLCommonBasePresenter<INonPerformingLoan>
	{
		protected IAccountRepository _accRepository;
		protected IFinancialServiceRepository _finRepository;
		protected IFinancialAdjustmentRepository _finAdjustmentRepository;
		protected ILookupRepository _lookupRepository;
		protected IAccount _account;
		protected IMortgageLoan vML;
		protected IMortgageLoan fML;
		protected int _accountKey;
		protected bool _pendingNPL;


		public NonPerformingLoan(INonPerformingLoan View, SAHLCommonBaseController Controller)
			: base(View, Controller)
		{

		}

		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			base.OnViewInitialised(sender, e);

			if (!_view.ShouldRunPage)
				return;

			if (!_view.IsPostBack)
			{
				if (this.PrivateCacheData.ContainsKey("NonPerformingLoanIsLoanNP"))
					this.PrivateCacheData.Remove("NonPerformingLoanIsLoanNP");
			}

			_view.onCancelButtonClicked += new EventHandler(_view_onCancelButtonClicked);
			_view.onProceedButtonClicked += new EventHandler(_view_onProceedButtonClicked);
			_view.onCheckBoxChecked += new EventHandler(_view_onCheckBoxChecked);

			CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
			if (cboNode != null)
			{
				_accountKey = -1;
				switch (cboNode.GenericKeyTypeKey)
				{
					case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
						_accountKey = cboNode.GenericKey;
						break;
					case (int)SAHL.Common.Globals.GenericKeyTypes.Offer:
						IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
						IApplication application = appRepo.GetApplicationByKey(cboNode.GenericKey);
						_accountKey = application.ReservedAccount.Key;
						break;
					default:
						break;
				}

				if (_accountKey > 0)
				{
					_accRepository = RepositoryFactory.GetRepository<IAccountRepository>();
					_account = _accRepository.GetAccountByKey(_accountKey);
					_view.ProceedButtonVisible = false;

					IFinancialServiceRepository financialServiceRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
					DateTime? suspendedDate;
					//_view.SetSuspendedInterestAmount(financialServiceRepo.GetSuspendedInterest(_accountKey, out suspendedDate));

					IMortgageLoanAccount mortgageLoanAccount = _account as IMortgageLoanAccount;
					//PendingFinancialAdjustment

					IFinancialService suspendedInterestFinancialService;
					decimal mtdInterestVariable = 0M;
					double monthToDateInterest = 0;
					IFinancialAdjustment nonPerforminingLoanAdjustment;
					foreach (IFinancialService fs in _account.FinancialServices)
					{
						if ((fs.AccountStatus.Key == (int)AccountStatuses.Open
							|| fs.AccountStatus.Key == (int)AccountStatuses.Dormant
							|| fs.AccountStatus.Key == (int)AccountStatuses.Locked)
							)
						{
							switch (fs.FinancialServiceType.Key)
							{
								case (int)FinancialServiceTypes.VariableLoan:
								case (int)FinancialServiceTypes.FixedLoan:
									suspendedInterestFinancialService = _account.FinancialServices.Where(x => x.FinancialServiceParent != null &&
																											  x.AccountStatus.Key == (int)AccountStatuses.Open &&
																											  x.FinancialServiceParent.Key == fs.Key &&
																											  x.FinancialServiceType.Key == (int)FinancialServiceTypes.SuspendedInterest).FirstOrDefault();

									nonPerforminingLoanAdjustment = fs.GetPendingFinancialAdjustmentByTypeSource(FinancialAdjustmentTypeSources.NonPerforming);
									if (nonPerforminingLoanAdjustment != null)
										_pendingNPL = true;

									if (suspendedInterestFinancialService != null)
									{
										if (fs.FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan)
										{
											_view.SetMTDInterest((decimal)fs.Balance.LoanBalance.MTDInterest);
											_view.SetSuspendedInterestAmount((decimal)suspendedInterestFinancialService.Balance.Amount);
										}
										else if (fs.FinancialServiceType.Key == (int)FinancialServiceTypes.FixedLoan)
										{
											_view.ShowFixedLeg();
											_view.SetMTDInterestFixed((decimal)fs.Balance.LoanBalance.MTDInterest);
											_view.SetSuspendedInterestAmountFixed((decimal)suspendedInterestFinancialService.Balance.Amount);
										}
									}
									break;
							}
						}
					}

					SetNonPerformingControls();
				}
				else
				{
					_view.ProceedButtonVisible = false;
					_view.CheckBoxVisible = false;
				}
			}
		}

		public void SetNonPerformingControls()
		{
			if (_account.OriginationSourceProduct.Product.Key == (int)Products.SuperLo || _account.OriginationSourceProduct.Product.Key == (int)Products.VariFixLoan)
			{
				_view.CheckBoxVisible = false;
				_view.CheckBoxEnabled = false;
				_view.ProceedButtonVisible = false;
			}
			else
			{
				IFinancialServiceRepository financialServiceRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();

				if (_pendingNPL || financialServiceRepo.IsLoanNonPerforming(_accountKey))
				{
					_view.CheckBoxValue = true;
					_view.CheckBoxEnabled = true;
					if (!this.PrivateCacheData.ContainsKey("NonPerformingLoanIsLoanNP"))
						this.PrivateCacheData.Add("NonPerformingLoanIsLoanNP", true);
					else
						this.PrivateCacheData["NonPerformingLoanIsLoanNP"] = true;
				}
				else
				{
					_view.CheckBoxValue = false;
					//_view.CheckBoxEnabled = false;
					if (!this.PrivateCacheData.ContainsKey("NonPerformingLoanIsLoanNP"))
						this.PrivateCacheData.Add("NonPerformingLoanIsLoanNP", false);
					else
						this.PrivateCacheData["NonPerformingLoanIsLoanNP"] = false;
				}
			}
		}

		public void _view_onCheckBoxChecked(Object sender, EventArgs e)
		{
			if (Convert.ToBoolean(this.PrivateCacheData["NonPerformingLoanIsLoanNP"]) != _view.CheckBoxValue)
			{
				_view.ShowConfirmWhenProceedClicked = true;
				_view.ProceedButtonVisible = true;
			}
		}

		public void _view_onCancelButtonClicked(Object sender, EventArgs e)
		{
			Navigator.Navigate("Cancel");
		}

		public void _view_onProceedButtonClicked(Object sender, EventArgs e)
		{
			if (_view.CheckBoxValue)
			{
				// Mark Non Perform
				MarkNonPerformingLoan();
			}
			else
			{
				// UnMark Non Perform
				UnMarkNonPerformingLoan();
			}

		}

		public void MarkNonPerformingLoan()
		{
			IMortgageLoanRepository mlRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
			IMortgageLoan ml = mlRepo.GetMortgageloanByAccountKey(_account.Key);

			IRuleService svc = ServiceFactory.GetService<IRuleService>();
			svc.ExecuteRule(_view.Messages, "FinancialServiceNonPerformingLoan", ml);
			svc.ExecuteRule(_view.Messages, "FinancialAdjustmentPending", ml, FinancialAdjustmentTypeSources.NonPerforming);
			if (!_view.IsValid)
				return;

			var trans = new TransactionScope();
			try
			{

				IAccountRepository accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
				accountRepo.OptInNonPerforming(_account.Key, _view.CurrentPrincipal.Identity.Name);
				trans.VoteCommit();
			}
			catch (Exception)
			{
				trans.VoteRollBack();
				if (_view.IsValid)
					throw;
			}
			finally
			{
				trans.Dispose();
			}

			if (_view.Messages.Count == 0)
				_view.Navigator.Navigate("Submit");

		}

		public void UnMarkNonPerformingLoan()
		{

			// #15051 - Rule added to  respond if the change cannot be made.
			var svc = ServiceFactory.GetService<IRuleService>();

			//IMortgageLoanRepository mlRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
			//IMortgageLoan ml = mlRepo.GetMortgageloanByAccountKey(_account.Key);            

			//svc.ExecuteRule(_view.Messages, "FinancialAdjustmentPending", ml, FinancialAdjustmentTypeSources.NonPerforming);
			//if (!_view.IsValid)
			//    return;

			foreach (var detail in _account.Details)
			{
				if (svc.ExecuteRule(_view.Messages, "CheckNonPerformingLoanByDetailType", detail) == 0)
					return;
			}

			var trans = new TransactionScope();
			try
			{

				IAccountRepository accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
				accountRepo.OptOutNonPerforming(_account.Key, _view.CurrentPrincipal.Identity.Name, (int)CancellationReasons.CancelNonPerfoming);

				trans.VoteCommit();
			}
			catch (Exception)
			{
				trans.VoteRollBack();
				if (_view.IsValid)
					throw;
			}
			finally
			{
				trans.Dispose();
			}

			if (_view.Messages.Count == 0)
				_view.Navigator.Navigate("Submit");
		}
	}
}
