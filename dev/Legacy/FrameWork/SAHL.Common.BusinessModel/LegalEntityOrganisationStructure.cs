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
	/// SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructure_DAO
	/// </summary>
	public partial class LegalEntityOrganisationStructure : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructure_DAO>, ILegalEntityOrganisationStructure
	{
				public LegalEntityOrganisationStructure(SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructure_DAO LegalEntityOrganisationStructure) : base(LegalEntityOrganisationStructure)
		{
			this._DAO = LegalEntityOrganisationStructure;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructure_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructure_DAO.LegalEntity
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
		/// SAHL.Common.BusinessModel.DAO.LegalEntityOrganisationStructure_DAO.OrganisationStructure
		/// </summary>
		public IOrganisationStructure OrganisationStructure 
		{
			get
			{
				if (null == _DAO.OrganisationStructure) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IOrganisationStructure, OrganisationStructure_DAO>(_DAO.OrganisationStructure);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.OrganisationStructure = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.OrganisationStructure = (OrganisationStructure_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


