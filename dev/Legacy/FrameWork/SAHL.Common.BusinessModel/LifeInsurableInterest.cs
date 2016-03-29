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
	/// This class is used to represent the composite primary key in the LifeInsurableInterest_DAO class.
	/// </summary>
	public partial class LifeInsurableInterest : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LifeInsurableInterest_DAO>, ILifeInsurableInterest
	{
				public LifeInsurableInterest(SAHL.Common.BusinessModel.DAO.LifeInsurableInterest_DAO LifeInsurableInterest) : base(LifeInsurableInterest)
		{
			this._DAO = LifeInsurableInterest;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeInsurableInterest_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeInsurableInterest_DAO.LifeInsurableInterestType
		/// </summary>
		public ILifeInsurableInterestType LifeInsurableInterestType 
		{
			get
			{
				if (null == _DAO.LifeInsurableInterestType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILifeInsurableInterestType, LifeInsurableInterestType_DAO>(_DAO.LifeInsurableInterestType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.LifeInsurableInterestType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.LifeInsurableInterestType = (LifeInsurableInterestType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LifeInsurableInterest_DAO.LegalEntity
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
		/// SAHL.Common.BusinessModel.DAO.LifeInsurableInterest_DAO.Account
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
	}
}


