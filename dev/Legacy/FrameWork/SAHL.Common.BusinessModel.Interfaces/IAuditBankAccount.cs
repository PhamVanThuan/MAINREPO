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
	public partial interface IAuditBankAccount : IEntityValidation
	{
		/// <summary>
		/// 
		/// </summary>
		System.String AuditLogin
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String AuditHostName
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String AuditProgramName
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.DateTime AuditDate
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Char AuditAddUpdateDelete
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Int32 BankAccountKey
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String ACBBranchCode
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String AccountNumber
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Int32 ACBTypeNumber
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String AccountName
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String UserID
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		DateTime? ChangeDate
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Decimal Key
		{
			get;
			set;
		}
	}
}


