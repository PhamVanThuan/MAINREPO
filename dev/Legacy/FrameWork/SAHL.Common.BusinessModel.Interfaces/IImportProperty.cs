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
	public partial interface IImportProperty : IEntityValidation
	{
		/// <summary>
		/// 
		/// </summary>
		System.String PropertyTypeKey
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String TitleTypeKey
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String AreaClassificationKey
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String OccupancyTypeKey
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String PropertyDescription1
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String PropertyDescription2
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String PropertyDescription3
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.Double DeedsOfficeValue
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		DateTime? CurrentBondDate
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String ErfNumber
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String ErfPortionNumber
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String SectionalSchemeName
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String SectionalUnitNumber
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String DeedsPropertyTypeKey
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String ErfSuburbDescription
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String ErfMetroDescription
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String TitleDeedNumber
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


