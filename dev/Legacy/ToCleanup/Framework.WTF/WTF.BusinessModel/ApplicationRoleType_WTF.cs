
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;

using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.ApplicationRoleType_DAO
	/// </summary>
    public partial class ApplicationRoleType_WTF : BusinessModelBase<ApplicationRoleType_WTF_DAO>, IApplicationRoleType_WTF
	{
        public ApplicationRoleType_WTF(ApplicationRoleType_WTF_DAO ApplicationRoleType_WTF) : base(ApplicationRoleType_WTF)
		{
            this._DAO = ApplicationRoleType_WTF;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRoleType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRoleType_DAO.ApplicationRoleTypeGroupKey
		/// </summary>
		public Int32 ApplicationRoleTypeGroupKey 
		{
			get { return _DAO.ApplicationRoleTypeGroupKey; }
			set { _DAO.ApplicationRoleTypeGroupKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRoleType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRoleType_DAO.ApplicationRoleTypeOrganisationStructureMappings
		/// </summary>
        private DAOEventList<ApplicationRoleTypeOrganisationStructureMapping_WTF_DAO, IApplicationRoleTypeOrganisationStructureMapping_WTF, ApplicationRoleTypeOrganisationStructureMapping_WTF> _ApplicationRoleTypeOrganisationStructureMappings;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRoleType_DAO.ApplicationRoleTypeOrganisationStructureMappings
		/// </summary>
        public IEventList<IApplicationRoleTypeOrganisationStructureMapping_WTF> ApplicationRoleTypeOrganisationStructureMappings
		{
			get
			{
				if (null == _ApplicationRoleTypeOrganisationStructureMappings) 
				{
					if(null == _DAO.ApplicationRoleTypeOrganisationStructureMappings)
                        _DAO.ApplicationRoleTypeOrganisationStructureMappings = new List<ApplicationRoleTypeOrganisationStructureMapping_WTF_DAO>();
                    _ApplicationRoleTypeOrganisationStructureMappings = new DAOEventList<ApplicationRoleTypeOrganisationStructureMapping_WTF_DAO, IApplicationRoleTypeOrganisationStructureMapping_WTF, ApplicationRoleTypeOrganisationStructureMapping_WTF>(_DAO.ApplicationRoleTypeOrganisationStructureMappings);
					_ApplicationRoleTypeOrganisationStructureMappings.BeforeAdd += new EventListHandler(OnApplicationRoleTypeOrganisationStructureMappings_BeforeAdd);					
					_ApplicationRoleTypeOrganisationStructureMappings.BeforeRemove += new EventListHandler(OnApplicationRoleTypeOrganisationStructureMappings_BeforeRemove);					
					_ApplicationRoleTypeOrganisationStructureMappings.AfterAdd += new EventListHandler(OnApplicationRoleTypeOrganisationStructureMappings_AfterAdd);					
					_ApplicationRoleTypeOrganisationStructureMappings.AfterRemove += new EventListHandler(OnApplicationRoleTypeOrganisationStructureMappings_AfterRemove);					
				}
				return _ApplicationRoleTypeOrganisationStructureMappings;
			}
		}
	}
}



