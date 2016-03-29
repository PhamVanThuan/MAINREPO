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
	/// The LegalEntityAssetLiability_DAO class links the entries in the AssetLiability table to the LegalEntity
	/// </summary>
	public partial class LegalEntityAssetLiability : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LegalEntityAssetLiability_DAO>, ILegalEntityAssetLiability
	{
				public LegalEntityAssetLiability(SAHL.Common.BusinessModel.DAO.LegalEntityAssetLiability_DAO LegalEntityAssetLiability) : base(LegalEntityAssetLiability)
		{
			this._DAO = LegalEntityAssetLiability;
		}
		/// <summary>
		/// The foreign key reference to the AssetLiability table where the information regarding the Asset/Liability is stored. Each 
		/// LegalEntityAssetLiabilityKey belongs to a single AssetLiabilityKey.
		/// </summary>
		public IAssetLiability AssetLiability 
		{
			get
			{
				if (null == _DAO.AssetLiability) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IAssetLiability, AssetLiability_DAO>(_DAO.AssetLiability);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.AssetLiability = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.AssetLiability = (AssetLiability_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// The status of the record.
		/// </summary>
		public IGeneralStatus GeneralStatus 
		{
			get
			{
				if (null == _DAO.GeneralStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IGeneralStatus, GeneralStatus_DAO>(_DAO.GeneralStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.GeneralStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.GeneralStatus = (GeneralStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// Primary Key.
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// The foreign key reference to the LegalEntity table where the information regarding the Legal Entity is stored. Each 
		/// LegalEntityAssetLiabilityKey belongs to a single LegalEntityKey.
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


