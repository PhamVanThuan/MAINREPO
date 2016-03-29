
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.Application_DAO
	/// </summary>
	public partial interface IApplication_WTF : IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationTypeKey
		/// </summary>
		System.Int32 ApplicationTypeKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationStatusKey
		/// </summary>
		System.Int32 ApplicationStatusKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationStartDate
		/// </summary>
		System.DateTime? ApplicationStartDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationEndDate
		/// </summary>
		System.DateTime? ApplicationEndDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.AccountKey
		/// </summary>
		System.Int32 AccountKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.Reference
		/// </summary>
		System.String Reference
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationCampaignKey
		/// </summary>
		System.Int32 ApplicationCampaignKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationSourceKey
		/// </summary>
		System.Int32 ApplicationSourceKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ReservedAccountKey
		/// </summary>
		System.Int32 ReservedAccountKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.OriginationSourceKey
		/// </summary>
		System.Int32 OriginationSourceKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.EstimateNumberApplicants
		/// </summary>
		System.Int32 EstimateNumberApplicants
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Application_DAO.ApplicationRoles
		/// </summary>
		IEventList<IApplicationRole_WTF> ApplicationRoles
		{
			get;
		}
	}
}



