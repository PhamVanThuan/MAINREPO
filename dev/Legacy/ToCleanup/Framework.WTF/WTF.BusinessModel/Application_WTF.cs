
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
	/// SAHL.Common.BusinessModel.DAO.Application_DAO
	/// </summary>
    public partial class Application_WTF : BusinessModelBase<Application_WTF_DAO>, IApplication_WTF
	{
        public Application_WTF(Application_WTF_DAO Application_WTF) : base(Application_WTF)
		{
            this._DAO = Application_WTF;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationTypeKey
		/// </summary>
		public Int32 ApplicationTypeKey 
		{
			get { return _DAO.ApplicationTypeKey; }
			set { _DAO.ApplicationTypeKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationStatusKey
		/// </summary>
		public Int32 ApplicationStatusKey 
		{
			get { return _DAO.ApplicationStatusKey; }
			set { _DAO.ApplicationStatusKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationStartDate
		/// </summary>
		public DateTime? ApplicationStartDate 
		{
			get { return _DAO.ApplicationStartDate; }
			set { _DAO.ApplicationStartDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationEndDate
		/// </summary>
		public DateTime? ApplicationEndDate 
		{
			get { return _DAO.ApplicationEndDate; }
			set { _DAO.ApplicationEndDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.AccountKey
		/// </summary>
		public Int32 AccountKey 
		{
			get { return _DAO.AccountKey; }
			set { _DAO.AccountKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.Reference
		/// </summary>
		public String Reference 
		{
			get { return _DAO.Reference; }
			set { _DAO.Reference = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationCampaignKey
		/// </summary>
		public Int32 ApplicationCampaignKey 
		{
			get { return _DAO.ApplicationCampaignKey; }
			set { _DAO.ApplicationCampaignKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationSourceKey
		/// </summary>
		public Int32 ApplicationSourceKey 
		{
			get { return _DAO.ApplicationSourceKey; }
			set { _DAO.ApplicationSourceKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ReservedAccountKey
		/// </summary>
		public Int32 ReservedAccountKey 
		{
			get { return _DAO.ReservedAccountKey; }
			set { _DAO.ReservedAccountKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.OriginationSourceKey
		/// </summary>
		public Int32 OriginationSourceKey 
		{
			get { return _DAO.OriginationSourceKey; }
			set { _DAO.OriginationSourceKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.EstimateNumberApplicants
		/// </summary>
		public Int32 EstimateNumberApplicants 
		{
			get { return _DAO.EstimateNumberApplicants; }
			set { _DAO.EstimateNumberApplicants = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationRoles
		/// </summary>
        private DAOEventList<ApplicationRole_WTF_DAO, IApplicationRole_WTF, ApplicationRole_WTF> _ApplicationRoles;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationRoles
		/// </summary>
        public IEventList<IApplicationRole_WTF> ApplicationRoles
		{
			get
			{
				if (null == _ApplicationRoles) 
				{
					if(null == _DAO.ApplicationRoles)
                        _DAO.ApplicationRoles = new List<ApplicationRole_WTF_DAO>();
                    _ApplicationRoles = new DAOEventList<ApplicationRole_WTF_DAO, IApplicationRole_WTF, ApplicationRole_WTF>(_DAO.ApplicationRoles);
					_ApplicationRoles.BeforeAdd += new EventListHandler(OnApplicationRoles_BeforeAdd);					
					_ApplicationRoles.BeforeRemove += new EventListHandler(OnApplicationRoles_BeforeRemove);					
					_ApplicationRoles.AfterAdd += new EventListHandler(OnApplicationRoles_AfterAdd);					
					_ApplicationRoles.AfterRemove += new EventListHandler(OnApplicationRoles_AfterRemove);					
				}
				return _ApplicationRoles;
			}
		}
	}
}



