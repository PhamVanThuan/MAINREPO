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
	/// Certain products require the effective rate on the Account to be overriden. For example, a Super Lo loan will result in a 
		/// 0.60% discount to the effective rate or an invoked CAP will result in a discounted rate. RateOverride_DAO is instantiated 
		/// when the client elects to take up these types of products.
	/// </summary>
	public partial interface IRateOverride : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// The date from which the Rate Override will be in effect.
		/// </summary>
		DateTime? FromDate
		{
			get;
			set;
		}
		/// <summary>
		/// The period, in months, for which the Rate Override will be applied.
		/// </summary>
		Int32? Term
		{
			get;
			set;
		}
		/// <summary>
		/// The rate at which the client has elected to CAP the rate applicable to their CAP Balance.
		/// </summary>
		Double? CapRate
		{
			get;
			set;
		}
		/// <summary>
		/// This is not currently being used.
		/// </summary>
		Double? FloorRate
		{
			get;
			set;
		}
		/// <summary>
		/// This is not currently being used.
		/// </summary>
		Double? FixedRate
		{
			get;
			set;
		}
		/// <summary>
		/// The percentage discount which applies for the rate override. For a Super Lo loan this would be 0.60%.
		/// </summary>
		Double? Discount
		{
			get;
			set;
		}
		/// <summary>
		/// The date on which the Rate Override was cancelled.
		/// </summary>
		DateTime? CancellationDate
		{
			get;
			set;
		}
		/// <summary>
		/// The balance which the client elected to CAP. If the client has taken a further loan, the CapRate is only applied
		/// to the CapBalance and not any subsequent increase.
		/// </summary>
		Double? CapBalance
		{
			get;
			set;
		}
		/// <summary>
		/// This property is used to capture a reduced debit order amount for the client, which will override the normal instalment due
		/// on the account. This could be used when a client is under debt review and could only afford a certain instalment or even a zero
		/// instalment.
		/// </summary>
		Double? Amount
		{
			get;
			set;
		}
		/// <summary>
		/// The date until which the Rate Override will be in effect.
		/// </summary>
		DateTime? EndDate
		{
			get;
			set;
		}
		/// <summary>
		/// Primary Key
		/// </summary>
		System.Int32 Key
		{
			get;
			set;
		}
		/// <summary>
		/// This is the foreign key reference to the Reason table. Each Rateoverride that is cancelled by a client requires a cancellation reason.
		/// </summary>
		ICancellationReason CancellationReason
		{
			get;
			set;
		}
		/// <summary>
		/// This is the foreign key reference to the FinancialService table. Each Rateoverride belongs to a Financial Service.
		/// </summary>
		IFinancialService FinancialService
		{
			get;
			set;
		}
		/// <summary>
		/// This is the foreign key reference to the GeneralStatus table. Each RateOverride belongs to a specific status which determines
		/// whether it is active or not.
		/// </summary>
		IGeneralStatus GeneralStatus
		{
			get;
			set;
		}
		/// <summary>
		/// This is the foreign key reference to the RateOverrideType table. Each RateOverride belongs to a specific type i.e.
		/// Super Lo, Interest Only, CAP.
		/// </summary>
		IRateOverrideType RateOverrideType
		{
			get;
			set;
		}
		/// <summary>
		/// This is the foreign key reference to the Trade table. A CAP RateOverride belongs to a specific trade. 
		/// The CapBalance on the RateOverride is allocated to this trade, which is bought in order to fund the CAP product.
		/// </summary>
		ITrade Trade
		{
			get;
			set;
		}
	}
}


