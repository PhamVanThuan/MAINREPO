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
	/// SAHL.Common.BusinessModel.DAO.ITC_DAO
	/// </summary>
	public partial class ITC : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ITC_DAO>, IITC
	{
				public ITC(SAHL.Common.BusinessModel.DAO.ITC_DAO ITC) : base(ITC)
		{
			this._DAO = ITC;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITC_DAO.ChangeDate
		/// </summary>
		public DateTime ChangeDate 
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITC_DAO.ResponseXML
		/// </summary>
		public String ResponseXML 
		{
			get { return _DAO.ResponseXML; }
			set { _DAO.ResponseXML = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITC_DAO.ResponseStatus
		/// </summary>
		public String ResponseStatus 
		{
			get { return _DAO.ResponseStatus; }
			set { _DAO.ResponseStatus = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITC_DAO.UserID
		/// </summary>
		public String UserID 
		{
			get { return _DAO.UserID; }
			set { _DAO.UserID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITC_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITC_DAO.ReservedAccount
		/// </summary>
		public IAccountSequence ReservedAccount 
		{
			get
			{
				if (null == _DAO.ReservedAccount) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IAccountSequence, AccountSequence_DAO>(_DAO.ReservedAccount);
				}
			}

			set
			{
				if(value == null)
				{
					_DAO.ReservedAccount = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ReservedAccount = (AccountSequence_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ITC_DAO.LegalEntity
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
		/// SAHL.Common.BusinessModel.DAO.ITC_DAO.RequestXML
		/// </summary>
		public String RequestXML 
		{
			get { return _DAO.RequestXML; }
			set { _DAO.RequestXML = value;}
		}
	}
}


