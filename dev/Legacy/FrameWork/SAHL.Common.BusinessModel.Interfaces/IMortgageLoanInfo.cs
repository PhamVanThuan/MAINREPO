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
	/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO
	/// </summary>
	public partial interface IMortgageLoanInfo : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.ElectionDate
		/// </summary>
		DateTime? ElectionDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.ConvertedDate
		/// </summary>
		DateTime? ConvertedDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.AccumulatedLoyaltyBenefit
		/// </summary>
		Double? AccumulatedLoyaltyBenefit
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.NextPaymentDate
		/// </summary>
		DateTime? NextPaymentDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.DiscountRate
		/// </summary>
		Double? DiscountRate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.PPThresholdYr1
		/// </summary>
		Double? PPThresholdYr1
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.PPThresholdYr2
		/// </summary>
		Double? PPThresholdYr2
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.PPThresholdYr3
		/// </summary>
		Double? PPThresholdYr3
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.PPThresholdYr4
		/// </summary>
		Double? PPThresholdYr4
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.PPThresholdYr5
		/// </summary>
		Double? PPThresholdYr5
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.MTDLoyaltyBenefit
		/// </summary>
		System.Double MTDLoyaltyBenefit
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.PPAllowed
		/// </summary>
		Double? PPAllowed
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.GeneralStatusKey
		/// </summary>
		System.Int32 GeneralStatusKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.MortgageLoan
		/// </summary>
		IMortgageLoan MortgageLoan
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.Exclusion
		/// </summary>
		System.String Exclusion
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.ExclusionEndDate
		/// </summary>
		DateTime? ExclusionEndDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.ExclusionReason
		/// </summary>
		System.String ExclusionReason
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanInfo_DAO.OverPaymentAmount
		/// </summary>
		Double? OverPaymentAmount
		{
			get;
			set;
		}
	}
}


