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
	/// SAHL.Common.BusinessModel.DAO.LegalEntityDomicilium_DAO
	/// </summary>
	public partial interface ILegalEntityDomicilium : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityDomicilium_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityDomicilium_DAO.LegalEntityAddress
		/// </summary>
		ILegalEntityAddress LegalEntityAddress
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityDomicilium_DAO.GeneralStatus
		/// </summary>
		IGeneralStatus GeneralStatus
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityDomicilium_DAO.ADUser
		/// </summary>
		IADUser ADUser
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityDomicilium_DAO.ChangeDate
		/// </summary>
		DateTime? ChangeDate
		{
			get;
			set;
		}
	}
}


