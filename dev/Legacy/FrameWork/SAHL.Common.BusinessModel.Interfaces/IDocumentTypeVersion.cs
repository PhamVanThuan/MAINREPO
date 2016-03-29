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
	public partial interface IDocumentTypeVersion : IEntityValidation
	{
		/// <summary>
		/// 
		/// </summary>
		System.Int32 Version
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.DateTime EffectiveDate
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
		IDocumentType DocumentType
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		IGeneralStatus GeneralStatus
		{
			get;
			set;
		}
	}
}


