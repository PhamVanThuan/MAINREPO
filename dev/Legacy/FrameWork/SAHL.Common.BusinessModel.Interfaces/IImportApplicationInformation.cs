using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	public partial interface IImportApplicationInformation : IEntityValidation
	{
		/// <summary>
		/// 
		/// </summary>
		System.Int32 ApplicationTerm
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Double CashDeposit
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Double PropertyValuation
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Double FeesTotal
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Double InterimInterest
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Double MonthlyInstalment
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Double HOCPremium
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Double LifePremium
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Double PreApprovedAmount
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Double MaxCashAllowed
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Double MaxQuickCashAllowed
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Double RequestedQuickCashAmount
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Double BondToRegister
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Double LTV
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Double PTI
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		IImportApplication ImportApplication
		{
			get;
			set;
		}
	}
}


