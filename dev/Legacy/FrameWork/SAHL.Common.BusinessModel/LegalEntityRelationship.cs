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
	/// The LegalEntityRelationship_DAO class is used in order to store relationships that exist between Legal Entities involved 
		/// in SA Home Loans accounts.
	/// </summary>
	public partial class LegalEntityRelationship : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LegalEntityRelationship_DAO>, ILegalEntityRelationship
	{
				public LegalEntityRelationship(SAHL.Common.BusinessModel.DAO.LegalEntityRelationship_DAO LegalEntityRelationship) : base(LegalEntityRelationship)
		{
			this._DAO = LegalEntityRelationship;
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
		/// The foreign key reference to the LegalEntity table. Each LegalEntityRelationship belongs to a single LegalEntity.
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
		/// The foreign key reference to the LegalEntity table. This is the Legal Entity which that referenced in the Legal Entity
		/// property is related to.
		/// </summary>
		public ILegalEntity RelatedLegalEntity 
		{
			get
			{
				if (null == _DAO.RelatedLegalEntity) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(_DAO.RelatedLegalEntity);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.RelatedLegalEntity = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.RelatedLegalEntity = (LegalEntity_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// The foreign key reference to to the RelationshipType table. Each LegalEntityRelationship belongs to a single Relationship Type.
		/// </summary>
		public ILegalEntityRelationshipType LegalEntityRelationshipType 
		{
			get
			{
				if (null == _DAO.LegalEntityRelationshipType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILegalEntityRelationshipType, LegalEntityRelationshipType_DAO>(_DAO.LegalEntityRelationshipType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.LegalEntityRelationshipType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.LegalEntityRelationshipType = (LegalEntityRelationshipType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


