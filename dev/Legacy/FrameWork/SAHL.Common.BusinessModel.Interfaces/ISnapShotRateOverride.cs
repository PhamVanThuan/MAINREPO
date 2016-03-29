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
	/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO
	/// </summary>
    public partial interface ISnapShotFinancialAdjustment : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.AccountKey
		/// </summary>
		System.Int32 AccountKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.RateOverrideKey
		/// </summary>
        System.Int32 FinancialAdjustmentKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.FinancialServiceKey
		/// </summary>
		System.Int32 FinancialServiceKey
		{
			get;
			set;
		}
		/// <summary>
        /// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.FinancialAdjustmentTypeKey
		/// </summary>
        System.Int32 FinancialAdjustmentTypeKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.FromDate
		/// </summary>
		System.DateTime FromDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.Term
		/// </summary>
		System.Int32 Term
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.CapRate
		/// </summary>
		System.Double CapRate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.FloorRate
		/// </summary>
		System.Double FloorRate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.FixedRate
		/// </summary>
		System.Double FixedRate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.Discount
		/// </summary>
		System.Double Discount
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.GeneralStatusKey
		/// </summary>
		System.Int32 GeneralStatusKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.TradeKey
		/// </summary>
		System.Int32 TradeKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.CancellationDate
		/// </summary>
		System.DateTime CancellationDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.CancellationReasonKey
		/// </summary>
		System.Int32 CancellationReasonKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.CapBalance
		/// </summary>
		System.Double CapBalance
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.Amount
		/// </summary>
		System.Double Amount
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.CAPPaymentOptionKey
		/// </summary>
		System.Int32 CAPPaymentOptionKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.EndDate
		/// </summary>
		System.DateTime EndDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SnapShotFinancialAdjustment_DAO.SnapShotFinancialService
		/// </summary>
		ISnapShotFinancialService SnapShotFinancialService
		{
			get;
			set;
		}
	}
}


