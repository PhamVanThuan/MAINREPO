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
	/// SAHL.Common.BusinessModel.DAO.AffordabilityType_DAO
	/// </summary>
	public partial interface IAffordabilityType : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AffordabilityType_DAO.Description
		/// </summary>
		System.String Description
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AffordabilityType_DAO.IsExpense
		/// </summary>
		System.Boolean IsExpense
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AffordabilityType_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AffordabilityType_DAO.LegalEntityAffordabilities
		/// </summary>
		IEventList<ILegalEntityAffordability> LegalEntityAffordabilities
		{
			get;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AffordabilityType_DAO.AffordabilityTypeGroup
		/// </summary>
		IAffordabilityTypeGroup AffordabilityTypeGroup
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AffordabilityType_DAO.DescriptionRequired
		/// </summary>
		System.Boolean DescriptionRequired
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AffordabilityType_DAO.Sequence
		/// </summary>
		System.Int32 Sequence
		{
			get;
			set;
		}
	}
}


