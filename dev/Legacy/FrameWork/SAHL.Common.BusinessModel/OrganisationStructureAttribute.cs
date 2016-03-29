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
	/// SAHL.Common.BusinessModel.DAO.OrganisationStructureAttribute_DAO
	/// </summary>
	public partial class OrganisationStructureAttribute : BusinessModelBase<SAHL.Common.BusinessModel.DAO.OrganisationStructureAttribute_DAO>, IOrganisationStructureAttribute
	{
				public OrganisationStructureAttribute(SAHL.Common.BusinessModel.DAO.OrganisationStructureAttribute_DAO OrganisationStructureAttribute) : base(OrganisationStructureAttribute)
		{
			this._DAO = OrganisationStructureAttribute;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructureAttribute_DAO.AttributeValue
		/// </summary>
		public String AttributeValue 
		{
			get { return _DAO.AttributeValue; }
			set { _DAO.AttributeValue = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructureAttribute_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructureAttribute_DAO.OrganisationStructure
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
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructureAttribute_DAO.OrganisationStructureAttributeType
		/// </summary>
		public IOrganisationStructureAttributeType OrganisationStructureAttributeType 
		{
			get
			{
				if (null == _DAO.OrganisationStructureAttributeType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IOrganisationStructureAttributeType, OrganisationStructureAttributeType_DAO>(_DAO.OrganisationStructureAttributeType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.OrganisationStructureAttributeType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.OrganisationStructureAttributeType = (OrganisationStructureAttributeType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


