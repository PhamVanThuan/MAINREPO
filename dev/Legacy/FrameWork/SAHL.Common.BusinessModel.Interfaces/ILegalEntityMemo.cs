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
	public partial interface ILegalEntityMemo : IEntityValidation
	{
		/// <summary>
		/// 
		/// </summary>
		System.DateTime InsertDate
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.DateTime ReminderDate
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.DateTime ExpiryDate
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		System.String Memo
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
		System.Int32 AccountMemoStatusKey
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
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		ILegalEntity LegalEntity
		{
			get;
			set;
		}
	}
}


