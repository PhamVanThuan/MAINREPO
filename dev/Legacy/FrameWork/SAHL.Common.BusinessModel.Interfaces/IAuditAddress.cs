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
	public partial interface IAuditAddress : IEntityValidation
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
		System.Int32 AddressKey
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		Int32? AddressFormatKey
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String BoxNumber
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String UnitNumber
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String BuildingNumber
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String BuildingName
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String StreetNumber
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String StreetName
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		Int32? SuburbKey
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		Int32? PostOfficeKey
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String RRR_CountryDescription
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String RRR_ProvinceDescription
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String RRR_CityDescription
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String RRR_SuburbDescription
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String RRR_PostalCode
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
		System.String SuiteNumber
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String FreeText1
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String FreeText2
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String FreeText3
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String FreeText4
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String FreeText5
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


