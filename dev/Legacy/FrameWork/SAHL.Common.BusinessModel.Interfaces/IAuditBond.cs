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
	public partial interface IAuditBond : IEntityValidation
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
		System.Int32 BondKey
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Int32 DeedsOfficeKey
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Int32 AttorneyKey
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String BondRegistrationNumber
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		DateTime? BondRegistrationDate
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		Double? BondRegistrationAmount
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		Double? BondLoanAgreementAmount
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
		System.DateTime ChangeDate
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


