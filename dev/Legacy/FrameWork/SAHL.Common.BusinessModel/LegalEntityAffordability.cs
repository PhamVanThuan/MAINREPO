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
	/// The LegalEntityAffordability_DAO class contains the information regarding the Legal Entity's Affordability Assessment.
	/// </summary>
	public partial class LegalEntityAffordability : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LegalEntityAffordability_DAO>, ILegalEntityAffordability
	{
				public LegalEntityAffordability(SAHL.Common.BusinessModel.DAO.LegalEntityAffordability_DAO LegalEntityAffordability) : base(LegalEntityAffordability)
		{
			this._DAO = LegalEntityAffordability;
		}
		/// <summary>
		/// The Rand Value of the Affordability Assessment Entry.
		/// </summary>
		public Double Amount 
		{
			get { return _DAO.Amount; }
			set { _DAO.Amount = value;}
		}
		/// <summary>
		/// Description field.
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
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
		/// The specific Affordability Assessment category that we are capturing information for. e.g. Basic Salary, Rental, Commission etc.
		/// </summary>
		public IAffordabilityType AffordabilityType 
		{
			get
			{
				if (null == _DAO.AffordabilityType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IAffordabilityType, AffordabilityType_DAO>(_DAO.AffordabilityType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.AffordabilityType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.AffordabilityType = (AffordabilityType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// The foreign key reference to the Legal Entity table. Each Affordability Assessment entry belongs to a single Legal Entity.
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
		/// The foreign key reference to the Offer table. Each Affordability Assessment entry belongs to a single Application.
		/// </summary>
		public IApplication Application 
		{
			get
			{
				if (null == _DAO.Application) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IApplication, Application_DAO>(_DAO.Application);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Application = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Application = (Application_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


