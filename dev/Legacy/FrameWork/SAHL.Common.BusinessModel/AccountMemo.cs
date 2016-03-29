using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.AccountMemo_DAO
	/// </summary>
	public partial class AccountMemo : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AccountMemo_DAO>, IAccountMemo
	{
				public AccountMemo(SAHL.Common.BusinessModel.DAO.AccountMemo_DAO AccountMemo) : base(AccountMemo)
		{
			this._DAO = AccountMemo;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AccountMemo_DAO.InsertDate
		/// </summary>
		public DateTime InsertDate 
		{
			get { return _DAO.InsertDate; }
			set { _DAO.InsertDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AccountMemo_DAO.ReminderDate
		/// </summary>
		public DateTime ReminderDate 
		{
			get { return _DAO.ReminderDate; }
			set { _DAO.ReminderDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AccountMemo_DAO.ExpiryDate
		/// </summary>
		public DateTime ExpiryDate 
		{
			get { return _DAO.ExpiryDate; }
			set { _DAO.ExpiryDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AccountMemo_DAO.Memo
		/// </summary>
		public String Memo 
		{
			get { return _DAO.Memo; }
			set { _DAO.Memo = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AccountMemo_DAO.UserID
		/// </summary>
		public String UserID 
		{
			get { return _DAO.UserID; }
			set { _DAO.UserID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AccountMemo_DAO.ChangeDate
		/// </summary>
		public DateTime ChangeDate 
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AccountMemo_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AccountMemo_DAO.Account
		/// </summary>
		public IAccount Account 
		{
			get
			{
				if (null == _DAO.Account) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IAccount, Account_DAO>(_DAO.Account);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Account = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Account = (Account_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AccountMemo_DAO.AccountMemoStatus
		/// </summary>
		public IAccountMemoStatus AccountMemoStatus 
		{
			get
			{
				if (null == _DAO.AccountMemoStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IAccountMemoStatus, AccountMemoStatus_DAO>(_DAO.AccountMemoStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.AccountMemoStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.AccountMemoStatus = (AccountMemoStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


