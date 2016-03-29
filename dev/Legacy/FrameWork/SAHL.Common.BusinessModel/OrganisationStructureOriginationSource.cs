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
	/// SAHL.Common.BusinessModel.DAO.OrganisationStructureOriginationSource_DAO
	/// </summary>
	public partial class OrganisationStructureOriginationSource : BusinessModelBase<SAHL.Common.BusinessModel.DAO.OrganisationStructureOriginationSource_DAO>, IOrganisationStructureOriginationSource
	{
				public OrganisationStructureOriginationSource(SAHL.Common.BusinessModel.DAO.OrganisationStructureOriginationSource_DAO OrganisationStructureOriginationSource) : base(OrganisationStructureOriginationSource)
		{
			this._DAO = OrganisationStructureOriginationSource;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructureOriginationSource_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructureOriginationSource_DAO.OrganisationStructure
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
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructureOriginationSource_DAO.OriginationSource
		/// </summary>
		public IOriginationSource OriginationSource 
		{
			get
			{
				if (null == _DAO.OriginationSource) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IOriginationSource, OriginationSource_DAO>(_DAO.OriginationSource);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.OriginationSource = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.OriginationSource = (OriginationSource_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


