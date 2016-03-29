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
	/// The LegalEntityBankAccount_DAO class links the Legal Entity to one or more Bank Accounts.
	/// </summary>
	public partial class LegalEntityBankAccount : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LegalEntityBankAccount_DAO>, ILegalEntityBankAccount
	{
				public LegalEntityBankAccount(SAHL.Common.BusinessModel.DAO.LegalEntityBankAccount_DAO LegalEntityBankAccount) : base(LegalEntityBankAccount)
		{
			this._DAO = LegalEntityBankAccount;
		}
		/// <summary>
		/// The foreign key reference to the Bank Account table which has the details regarding the Bank Account.
		/// Each LegalEntityBankAccountKey belongs to a single BankAccountKey
		/// </summary>
		public IBankAccount BankAccount 
		{
			get
			{
				if (null == _DAO.BankAccount) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IBankAccount, BankAccount_DAO>(_DAO.BankAccount);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.BankAccount = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.BankAccount = (BankAccount_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// The UserID of the last person who updated information on the LegalEntityBankAccount.
		/// </summary>
		public String UserID 
		{
			get { return _DAO.UserID; }
			set { _DAO.UserID = value;}
		}
		/// <summary>
		/// The date the record was last changed.
		/// </summary>
		public DateTime? ChangeDate
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
		}
		/// <summary>
		/// Primary Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// The foreign key reference to the LegalEntity table where the details of the Legal Entity are stored. Each LegalEntityBankAccountKey
		/// belongs to a single LegalEntityKey.
		/// </summary>
		public ILegalEntity LegalEntity 
		{
			get
			{
				if (null == _DAO.LegalEntity) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(_DAO.LegalEntity);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.LegalEntity = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.LegalEntity = (LegalEntity_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


