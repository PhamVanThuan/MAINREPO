using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;
namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.ApplicationRoleDomicilium_DAO
	/// </summary>
	public partial interface IApplicationRoleDomicilium : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OfferRoleDomicilium_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OfferRoleDomicilium_DAO.LegalEntityDomicilium
		/// </summary>
		ILegalEntityDomicilium LegalEntityDomicilium
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OfferRoleDomicilium_DAO.ApplicationRole
		/// </summary>
		IApplicationRole ApplicationRole
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OfferRoleDomicilium_DAO.ADUser
		/// </summary>
		IADUser ADUser
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OfferRoleDomicilium_DAO.ChangeDate
		/// </summary>
		DateTime? ChangeDate
		{
			get;
			set;
		}
	}
}


