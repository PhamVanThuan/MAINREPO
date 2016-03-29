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
	public partial interface IDocumentReferenceObject : IEntityValidation
	{
		/// <summary>
		/// 
		/// </summary>
		System.String TableName
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String ColumnName
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
	}
}


