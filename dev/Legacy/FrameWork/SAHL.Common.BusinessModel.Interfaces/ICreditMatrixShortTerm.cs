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
	/// SAHL.Common.BusinessModel.DAO.CreditMatrixShortTerm_DAO
	/// </summary>
	public partial interface ICreditMatrixShortTerm : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditMatrixShortTerm_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditMatrixShortTerm_DAO.NewBusinessIndicator
		/// </summary>
		System.Char NewBusinessIndicator
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditMatrixShortTerm_DAO.ImplementationDate
		/// </summary>
		System.DateTime? ImplementationDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditMatrixShortTerm_DAO.CreditCriteriaShortTerms
		/// </summary>
		IEventList<ICreditCriteriaShortTerm> CreditCriteriaShortTerms
		{
			get;
		}
	}
}


