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
	public partial interface IImportEmployment : IEntityValidation
	{
		/// <summary>
		/// 
		/// </summary>
		System.String EmploymentTypeKey
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String RemunerationTypeKey
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String EmploymentStatusKey
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String EmployerName
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String EmployerContactPerson
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String EmployerPhoneCode
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String EmployerPhoneNumber
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		DateTime? EmploymentStartDate
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		DateTime? EmploymentEndDate
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Double MonthlyIncome
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
		IImportLegalEntity ImportLegalEntity
		{
			get;
			set;
		}
	}
}


