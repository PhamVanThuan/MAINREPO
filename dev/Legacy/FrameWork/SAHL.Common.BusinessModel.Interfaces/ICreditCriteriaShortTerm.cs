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
	/// SAHL.Common.BusinessModel.DAO.CreditCriteriaShortTerm_DAO
	/// </summary>
	public partial interface ICreditCriteriaShortTerm : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteriaShortTerm_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteriaShortTerm_DAO.MinLoanAmount
		/// </summary>
		System.Double MinLoanAmount
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteriaShortTerm_DAO.MaxLoanAmount
		/// </summary>
		System.Double MaxLoanAmount
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteriaShortTerm_DAO.Term
		/// </summary>
		System.Int32 Term
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteriaShortTerm_DAO.CreditMatrixShortTerm
		/// </summary>
		ICreditMatrixShortTerm CreditMatrixShortTerm
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteriaShortTerm_DAO.Margin
		/// </summary>
		IMargin Margin
		{
			get;
			set;
		}
	}
}


