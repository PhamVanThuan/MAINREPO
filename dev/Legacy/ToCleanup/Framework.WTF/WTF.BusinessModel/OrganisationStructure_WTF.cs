
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
	/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO
	/// </summary>
    public partial class OrganisationStructure_WTF : BusinessModelBase<OrganisationStructure_WTF_DAO>, IOrganisationStructure_WTF
	{
        public OrganisationStructure_WTF(OrganisationStructure_WTF_DAO OrganisationStructure_WTF)  : base(OrganisationStructure_WTF)
		{
            this._DAO = OrganisationStructure_WTF;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.ParentKey
		/// </summary>
		public Int32 ParentKey 
		{
			get { return _DAO.ParentKey; }
			set { _DAO.ParentKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.OrganisationTypeKey
		/// </summary>
		public Int32 OrganisationTypeKey 
		{
			get { return _DAO.OrganisationTypeKey; }
			set { _DAO.OrganisationTypeKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.GeneralStatusKey
		/// </summary>
		public Int32 GeneralStatusKey 
		{
			get { return _DAO.GeneralStatusKey; }
			set { _DAO.GeneralStatusKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.UserOrganisationStructures
		/// </summary>
        private DAOEventList<UserOrganisationStructure_WTF_DAO, IUserOrganisationStructure_WTF, UserOrganisationStructure_WTF> _UserOrganisationStructures;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.UserOrganisationStructures
		/// </summary>
        public IEventList<IUserOrganisationStructure_WTF> UserOrganisationStructures
		{
			get
			{
				if (null == _UserOrganisationStructures) 
				{
					if(null == _DAO.UserOrganisationStructures)
                        _DAO.UserOrganisationStructures = new List<UserOrganisationStructure_WTF_DAO>();
                    _UserOrganisationStructures = new DAOEventList<UserOrganisationStructure_WTF_DAO, IUserOrganisationStructure_WTF, UserOrganisationStructure_WTF>(_DAO.UserOrganisationStructures);
					_UserOrganisationStructures.BeforeAdd += new EventListHandler(OnUserOrganisationStructures_BeforeAdd);					
					_UserOrganisationStructures.BeforeRemove += new EventListHandler(OnUserOrganisationStructures_BeforeRemove);					
					_UserOrganisationStructures.AfterAdd += new EventListHandler(OnUserOrganisationStructures_AfterAdd);					
					_UserOrganisationStructures.AfterRemove += new EventListHandler(OnUserOrganisationStructures_AfterRemove);					
				}
				return _UserOrganisationStructures;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.ApplicationRoleTypeOrganisationStructureMappings
		/// </summary>
        private DAOEventList<ApplicationRoleTypeOrganisationStructureMapping_WTF_DAO, IApplicationRoleTypeOrganisationStructureMapping_WTF, ApplicationRoleTypeOrganisationStructureMapping_WTF> _ApplicationRoleTypeOrganisationStructureMappings;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.ApplicationRoleTypeOrganisationStructureMappings
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



