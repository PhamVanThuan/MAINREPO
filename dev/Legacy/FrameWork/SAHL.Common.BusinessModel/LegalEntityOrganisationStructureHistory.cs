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
	/// SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructureHistory_DAO
	/// </summary>
	public partial class LegalEntityOrganisationStructureHistory : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructureHistory_DAO>, ILegalEntityOrganisationStructureHistory
	{
				public LegalEntityOrganisationStructureHistory(SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructureHistory_DAO LegalEntityOrganisationStructureHistory) : base(LegalEntityOrganisationStructureHistory)
		{
			this._DAO = LegalEntityOrganisationStructureHistory;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructureHistory_DAO.LegalEntityOrganisationStructure
		/// </summary>
		public ILegalEntityOrganisationStructure LegalEntityOrganisationStructure 
		{
			get
			{
				if (null == _DAO.LegalEntityOrganisationStructure) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILegalEntityOrganisationStructure, LegalEntityOrganisationStructure_DAO>(_DAO.LegalEntityOrganisationStructure);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.LegalEntityOrganisationStructure = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.LegalEntityOrganisationStructure = (LegalEntityOrganisationStructure_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructureHistory_DAO.LegalEntity
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
		/// SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructureHistory_DAO.OrganisationStructureKey
		/// </summary>
		public IOrganisationStructure OrganisationStructureKey 
		{
			get
			{
				if (null == _DAO.OrganisationStructureKey) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IOrganisationStructure, OrganisationStructure_DAO>(_DAO.OrganisationStructureKey);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.OrganisationStructureKey = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.OrganisationStructureKey = (OrganisationStructure_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructureHistory_DAO.ChangeDate
		/// </summary>
		public DateTime ChangeDate 
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructureHistory_DAO.Action
		/// </summary>
		public String Action 
		{
			get { return _DAO.Action; }
			set { _DAO.Action = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructureHistory_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
	}
}


