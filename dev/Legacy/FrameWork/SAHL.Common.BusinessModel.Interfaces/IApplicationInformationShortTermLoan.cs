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
	/// SAHL.Common.BusinessModel.DAO.ApplicationInformationShortTermLoan_DAO
	/// </summary>
	public partial interface IApplicationInformationShortTermLoan : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// Primary Key. This is also a foreign key reference to the OfferInformation table.
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationInformationShortTermLoan_DAO.LoanAmount
		/// </summary>
		System.Double LoanAmount
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationInformationShortTermLoan_DAO.Term
		/// </summary>
		System.Int32 Term
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationInformationShortTermLoan_DAO.MonthlyInstalment
		/// </summary>
		System.Double MonthlyInstalment
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationInformationShortTermLoan_DAO.LifePremium
		/// </summary>
		System.Double LifePremium
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationInformationShortTermLoan_DAO.MonthlyFee
		/// </summary>
		System.Double MonthlyFee
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationInformationShortTermLoan_DAO.InitiationFee
		/// </summary>
		System.Double InitiationFee
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationInformationShortTermLoan_DAO.FeesTotal
		/// </summary>
		System.Double FeesTotal
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationInformationShortTermLoan_DAO.CreditCriteriaShortTerm
		/// </summary>
		ICreditCriteriaShortTerm CreditCriteriaShortTerm
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationInformationShortTermLoan_DAO.CreditMatrixShortTerm
		/// </summary>
		ICreditMatrixShortTerm CreditMatrixShortTerm
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationInformationShortTermLoan_DAO.Margin
		/// </summary>
		IMargin Margin
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationInformationShortTermLoan_DAO.MarketRate
		/// </summary>
		IMarketRate MarketRate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationInformationShortTermLoan_DAO.ApplicationInformation
		/// </summary>
		IApplicationInformation ApplicationInformation
		{
			get;
			set;
		}
	}
}


