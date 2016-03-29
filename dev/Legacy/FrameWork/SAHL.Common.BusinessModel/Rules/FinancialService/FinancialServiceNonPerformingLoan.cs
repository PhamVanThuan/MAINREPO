using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Rules.FinancialService
{
	[RuleDBTag("FinancialServiceNonPerformingLoan",
	"Checks for when a loan is marked as non-performing",
	"SAHL.Rules.DLL",
	"SAHL.Common.BusinessModel.Rules.FinancialService.FinancialServiceNonPerformingLoan")]
	[RuleInfo]
	public class FinancialServiceNonPerformingLoan : BusinessRuleBase
	{
		public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
		{
			#region Check for allowed object type(s)
			if (Parameters.Length == 0)
				throw new ArgumentException("The FinancialServiceNonPerformingLoan rule expects a Domain object to be passed.");

			if (!(Parameters[0] is IFinancialService))
				throw new ArgumentException("The FinancialServiceNonPerformingLoan rule expects the following objects to be passed: IFinancialService.");
			#endregion

			IFinancialService fs = Parameters[0] as IFinancialService;

			if (fs == null)
				throw new Exception("NonPerformingLoan rule expects a IFinancialService object");

			if (fs.FinancialServiceType.Key == (int)FinancialServiceTypes.FixedLoan || fs.FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan)
			{
				IApplicationRepository AR = RepositoryFactory.GetRepository<IApplicationRepository>();
				if (AR.HasFurtherLendingInProgress(fs.Account.Key))
				{
					string errMsg = "This account has a further loan /re-advance in progress. Please refer this account to Litigation";
					AddMessage(errMsg, errMsg, Messages);
					return 0;
				}
			}
			return 1;
		}
	}
}
