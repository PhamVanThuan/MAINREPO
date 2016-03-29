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
	/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO
	/// </summary>
	public partial interface IDebtCounsellingProposalItem : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO.DebtCounsellingProposalItemKey
		/// </summary>
		System.Int32 DebtCounsellingProposalItemKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO.StartDate
		/// </summary>
		System.DateTime StartDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO.EndDate
		/// </summary>
		System.DateTime EndDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO.MarketRateKey
		/// </summary>
		System.Decimal MarketRateKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO.InterestRate
		/// </summary>
		System.Decimal InterestRate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO.Amount
		/// </summary>
		System.Decimal Amount
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO.AdditionalAmount
		/// </summary>
		System.Decimal AdditionalAmount
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO.CreateDate
		/// </summary>
		System.DateTime CreateDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO.InstalmentPercentage
		/// </summary>
		System.Decimal InstalmentPercentage
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO.AnnualEscalation
		/// </summary>
		System.Decimal AnnualEscalation
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposalItem_DAO.DebtCounsellingProposal
		/// </summary>
		IDebtCounsellingProposal DebtCounsellingProposal
		{
			get;
			set;
		}
	}
}


