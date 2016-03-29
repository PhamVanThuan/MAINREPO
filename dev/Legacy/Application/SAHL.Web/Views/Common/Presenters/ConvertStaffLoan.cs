using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;

using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel;
using SAHL.Common.DomainMessages;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using System.Linq;

namespace SAHL.Web.Views.Common.Presenters
{
	public class ConvertStaffLoan : SAHLCommonBasePresenter<IConvertStaffLoan>
	{
		protected IAccountRepository _accRepository;
		protected IAccount _account;
		protected IMortgageLoan vML;
		protected IMortgageLoan fML;
		protected int _accountKey;

		public ConvertStaffLoan(IConvertStaffLoan View, SAHLCommonBaseController Controller)
			: base(View, Controller)
		{

		}

		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			base.OnViewInitialised(sender, e);

			if (!_view.ShouldRunPage)
				return;

			_view.onConvertButtonClicked += new EventHandler(_view_onConvertButtonClicked);
			_view.onUnConvertButtonClicked += new EventHandler(_view_onUnConvertButtonClicked);

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

					IMortgageLoanAccount mortgageLoanAccount = _account as IMortgageLoanAccount;
					if (mortgageLoanAccount != null)
					{
						vML = mortgageLoanAccount.SecuredMortgageLoan;
					}

					IAccountVariFixLoan varifixLoanAccount = _account as IAccountVariFixLoan;
					if (varifixLoanAccount != null)
					{
						fML = varifixLoanAccount.FixedSecuredMortgageLoan;
					}

					_view.BindAccountDetails(_account, vML, fML);
				}
			}
		}

		protected override void OnViewPreRender(object sender, EventArgs e)
		{
			vML.Refresh();
			_view.UnConvertButtonEnabled = vML.FinancialAdjustments.Where(x => 
																			(x.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.SAHLStaff &&
																			 x.FinancialAdjustmentStatus.Key == (int)FinancialAdjustmentStatuses.Active) ||
																			 (x.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.SAHLStaff &&
																			 x.FinancialAdjustmentStatus.Key == (int)FinancialAdjustmentStatuses.Inactive &&
																			 x.FromDate.HasValue && x.FromDate.Value.Date >= DateTime.Now.Date)
																		  ).Count() > 0;
			_view.ConvertButtonEnabled = !_view.UnConvertButtonEnabled;
			base.OnViewPreRender(sender, e);
		}

		public void _view_onConvertButtonClicked(Object sender, EventArgs e)
		{
			_accRepository = RepositoryFactory.GetRepository<IAccountRepository>();
			TransactionScope tx = new TransactionScope();

			try
			{
				_accRepository.ConvertStaffLoan(_accountKey, _view.CurrentPrincipal.Identity.Name);
				if (!View.IsValid)
				{
					return;
				}
				tx.VoteCommit();
			}

			catch (Exception)
			{
				tx.VoteRollBack();
				if (_view.IsValid)
					throw;
			}
			finally
			{
				tx.Dispose();
			}
		}

		public void _view_onUnConvertButtonClicked(object sender, EventArgs e)
		{
			_accRepository = RepositoryFactory.GetRepository<IAccountRepository>();
			TransactionScope tx = new TransactionScope();
			try
			{

				_accRepository.UnConvertStaffLoan(_accountKey, _view.CurrentPrincipal.Identity.Name, CancellationReasons.Staffoptoutnolongeramemberofstaff);
				tx.VoteCommit();
			}

			catch (Exception)
			{
				tx.VoteRollBack();
				if (_view.IsValid)
					throw;
			}
			finally
			{
				tx.Dispose();
			}
		}
	}
}
