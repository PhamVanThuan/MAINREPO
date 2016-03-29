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
	/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO
	/// </summary>
	public partial interface ICreditCriteria : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.MinLoanAmount
		/// </summary>
		Double? MinLoanAmount
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.MaxLoanAmount
		/// </summary>
		Double? MaxLoanAmount
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.MinPropertyValue
		/// </summary>
		Double? MinPropertyValue
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.MaxPropertyValue
		/// </summary>
		Double? MaxPropertyValue
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.LTV
		/// </summary>
		Double? LTV
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.PTI
		/// </summary>
		Double? PTI
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.MinIncomeAmount
		/// </summary>
		Double? MinIncomeAmount
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.MaxIncomeAmount
		/// </summary>
		Double? MaxIncomeAmount
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.ExceptionCriteria
		/// </summary>
		Boolean? ExceptionCriteria
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.MinEmpiricaScore
		/// </summary>
		Int32? MinEmpiricaScore
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.Category
		/// </summary>
		ICategory Category
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.CreditMatrix
		/// </summary>
		ICreditMatrix CreditMatrix
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.EmploymentType
		/// </summary>
		IEmploymentType EmploymentType
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.Margin
		/// </summary>
		IMargin Margin
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO.MortgageLoanPurpose
		/// </summary>
		IMortgageLoanPurpose MortgageLoanPurpose
		{
			get;
			set;
		}
	}
}


