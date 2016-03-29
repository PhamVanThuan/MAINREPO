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
	public partial interface IAuditAccount : IEntityValidation
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
		System.Int32 AccountKey
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		Double? FixedPayment
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		Int32? AccountStatusKey
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		DateTime? InsertedDate
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		Int32? OriginationSourceProductKey
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		DateTime? OpenDate
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		DateTime? CloseDate
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		Int32? RRR_ProductKey
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		Int32? RRR_OriginationSourceKey
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


