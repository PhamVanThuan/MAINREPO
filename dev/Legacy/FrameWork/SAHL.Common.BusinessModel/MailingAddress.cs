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
	/// SAHL.Common.BusinessModel.DAO.MailingAddress_DAO
	/// </summary>
	public partial class MailingAddress : BusinessModelBase<SAHL.Common.BusinessModel.DAO.MailingAddress_DAO>, IMailingAddress
	{
				public MailingAddress(SAHL.Common.BusinessModel.DAO.MailingAddress_DAO MailingAddress) : base(MailingAddress)
		{
			this._DAO = MailingAddress;
		}
		/// <summary>
		/// The Electronic Format they would like to receive their Loan Statement in.
		/// </summary>
		public IOnlineStatementFormat OnlineStatementFormat 
		{
			get
			{
				if (null == _DAO.OnlineStatementFormat) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IOnlineStatementFormat, OnlineStatementFormat_DAO>(_DAO.OnlineStatementFormat);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.OnlineStatementFormat = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.OnlineStatementFormat = (OnlineStatementFormat_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MailingAddress_DAO.OnlineStatement
		/// </summary>
		public Boolean OnlineStatement 
		{
			get { return _DAO.OnlineStatement; }
			set { _DAO.OnlineStatement = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MailingAddress_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MailingAddress_DAO.Account
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
		/// SAHL.Common.BusinessModel.DAO.MailingAddress_DAO.Address
		/// </summary>
		public IAddress Address 
		{
			get
			{
				if (null == _DAO.Address) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IAddress, Address_DAO>(_DAO.Address);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Address = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Address = (Address_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MailingAddress_DAO.Language
		/// </summary>
		public ILanguage Language 
		{
			get
			{
				if (null == _DAO.Language) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILanguage, Language_DAO>(_DAO.Language);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Language = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Language = (Language_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MailingAddress_DAO.LegalEntity
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
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MailingAddress_DAO.CorrespondenceMedium
		/// </summary>
		public ICorrespondenceMedium CorrespondenceMedium 
		{
			get
			{
				if (null == _DAO.CorrespondenceMedium) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ICorrespondenceMedium, CorrespondenceMedium_DAO>(_DAO.CorrespondenceMedium);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.CorrespondenceMedium = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.CorrespondenceMedium = (CorrespondenceMedium_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


