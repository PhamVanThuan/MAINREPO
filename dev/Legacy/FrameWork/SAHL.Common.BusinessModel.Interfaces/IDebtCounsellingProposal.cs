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
	/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposal_DAO
	/// </summary>
	public partial interface IDebtCounsellingProposal : IEntityValidation, IBusinessModelObject
	{
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposal_DAO.DebtCounsellingProposalKey
		/// </summary>
		System.Int32 DebtCounsellingProposalKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposal_DAO.ProposalStatusKey
		/// </summary>
		System.Int32 ProposalStatusKey
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposal_DAO.HOCInclusive
		/// </summary>
		System.Boolean HOCInclusive
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposal_DAO.LifeInclusive
		/// </summary>
		System.Boolean LifeInclusive
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposal_DAO.AcceptedProposal
		/// </summary>
		System.Boolean AcceptedProposal
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposal_DAO.ProposalDate
		/// </summary>
		System.DateTime ProposalDate
		{
			get;
			set;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposal_DAO.DebtCounsellingProposalItems
		/// </summary>
		IEventList<IDebtCounsellingProposalItem> DebtCounsellingProposalItems
		{
			get;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Migration.DebtCounsellingProposal_DAO.DebtCounselling
		/// </summary>
		IDebtCounselling DebtCounselling
		{
			get;
			set;
		}
	}
}


