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
	/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingLegalEntity_DAO
	/// </summary>
	public partial interface IDebtCounsellingLegalEntity : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingLegalEntity_DAO.DebtCounsellingLegalEntityKey
		/// </summary>
		System.Int32 DebtCounsellingLegalEntityKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingLegalEntity_DAO.LegalEntityKey
		/// </summary>
		System.Int32 LegalEntityKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingLegalEntity_DAO.DebtCounselling
		/// </summary>
		IDebtCounselling DebtCounselling
		{
			get;
			set;
		}
	}
}


