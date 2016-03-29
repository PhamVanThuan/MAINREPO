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
	public partial interface IImportAccountExpense : IEntityValidation
	{
		/// <summary>
		/// 
		/// </summary>
		System.String ExpenseTypeKey
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String ExpenseAccountNumber
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String ExpenseAccountName
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String ExpenseReference
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Double TotalOutstandingAmount
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Double MonthlyPayment
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Boolean ToBeSettled
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


