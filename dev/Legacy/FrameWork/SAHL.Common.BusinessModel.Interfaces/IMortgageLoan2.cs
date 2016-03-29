using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
	/// <summary>
	///
	/// </summary>
	public partial interface IMortgageLoan : IEntityValidation, IFinancialService
	{
		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		IBond GetLatestRegisteredBond();

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		DateTime? GetLatestBondRegistrationDate();

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		bool HasInterestOnly();

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		IEventList<IMargin> GetLoanAttributeBasedMargins();

		IEventList<IMargin> GetAllMargins();

		/// <summary>
		/// Gets the valuation with IsActive = true.
		/// This method should be used instead of GetLatestPropertyValuation()
		/// </summary>
		/// <returns>The active valuation or null if not found</returns>
		IValuation GetActiveValuation();

		double GetActiveValuationAmount();

		DateTime? GetActiveValuationDate();

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		double SumBondRegistrationAmounts();

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		double SumBondLoanAgreementAmounts();

		int RemainingInstallments { get; }

		double CurrentBalance { get; }

		double ArrearBalance { get; }

		double InterestRate { get; }

		int InitialInstallments { get; }

		double ActiveMarketRate { get; }

		double? AccruedInterestMTD { get; }

		double RateAdjustment { get; }

		IResetConfiguration ResetConfiguration { get; }

		IRateConfiguration RateConfiguration { get; }

		/// <summary>
		///
		/// </summary>
		ICAP CAP { get; }

		IInterestOnly InterestOnly { get; }
	}
}