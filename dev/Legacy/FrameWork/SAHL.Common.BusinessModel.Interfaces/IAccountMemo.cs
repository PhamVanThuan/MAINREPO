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
	/// SAHL.Common.BusinessModel.DAO.AccountMemo_DAO
	/// </summary>
	public partial interface IAccountMemo : IEntityValidation
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AccountMemo_DAO.InsertDate
		/// </summary>
		System.DateTime InsertDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AccountMemo_DAO.ReminderDate
		/// </summary>
		System.DateTime ReminderDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AccountMemo_DAO.ExpiryDate
		/// </summary>
		System.DateTime ExpiryDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AccountMemo_DAO.Memo
		/// </summary>
		System.String Memo
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AccountMemo_DAO.UserID
		/// </summary>
		System.String UserID
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AccountMemo_DAO.ChangeDate
		/// </summary>
		System.DateTime ChangeDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AccountMemo_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AccountMemo_DAO.Account
		/// </summary>
		IAccount Account
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AccountMemo_DAO.AccountMemoStatus
		/// </summary>
		IAccountMemoStatus AccountMemoStatus
		{
			get;
			set;
		}
	}
}


