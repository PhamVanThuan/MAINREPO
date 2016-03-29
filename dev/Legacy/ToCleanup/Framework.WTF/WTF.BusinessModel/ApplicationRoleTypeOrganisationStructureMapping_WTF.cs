
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;

using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.ApplicationRoleTypeOrganisationStructureMapping_DAO
	/// </summary>
    public partial class ApplicationRoleTypeOrganisationStructureMapping_WTF : BusinessModelBase<ApplicationRoleTypeOrganisationStructureMapping_WTF_DAO>, IApplicationRoleTypeOrganisationStructureMapping_WTF
	{
        public ApplicationRoleTypeOrganisationStructureMapping_WTF(ApplicationRoleTypeOrganisationStructureMapping_WTF_DAO ApplicationRoleTypeOrganisationStructureMapping_WTF) : base(ApplicationRoleTypeOrganisationStructureMapping_WTF)
		{
            this._DAO = ApplicationRoleTypeOrganisationStructureMapping_WTF;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRoleTypeOrganisationStructureMapping_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRoleTypeOrganisationStructureMapping_DAO.ApplicationRoleType
		/// </summary>
        public IApplicationRoleType_WTF ApplicationRoleType 
		{
			get
			{
				if (null == _DAO.ApplicationRoleType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationRoleType_WTF, ApplicationRoleType_WTF_DAO>(_DAO.ApplicationRoleType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ApplicationRoleType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
                    _DAO.ApplicationRoleType = (ApplicationRoleType_WTF_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRoleTypeOrganisationStructureMapping_DAO.OrganisationStructure
		/// </summary>
        public IOrganisationStructure_WTF OrganisationStructure 
		{
			get
			{
				if (null == _DAO.OrganisationStructure) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IOrganisationStructure_WTF, OrganisationStructure_WTF_DAO>(_DAO.OrganisationStructure);
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
                    _DAO.OrganisationStructure = (OrganisationStructure_WTF_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}



