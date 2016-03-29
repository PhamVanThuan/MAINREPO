using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IDebtCounselling : IEntityValidation
    {
        /// <summary>
        ///
        /// </summary>
        IList<ILegalEntity> Clients
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        ILegalEntity DebtCounsellor
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        ILegalEntityOrganisationStructure DebtCounsellorLEOrganisationStructure
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        ILegalEntityOrganisationStructure PaymentDistributionAgentLEOrganisationStructure
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        ILegalEntity DebtCounsellorCompany
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        ILegalEntity PaymentDistributionAgent
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        ILegalEntity NationalCreditRegulator
        {
            get;
        }

        /// <summary>
        /// Litigation Attorney
        /// </summary>
        IAttorney LitigationAttorney
        {
            get;
        }

        /// <summary>
        /// Active Proposal (either Proposal or CounterProposal)
        /// </summary>
        IProposal GetActiveProposal(SAHL.Common.Globals.ProposalTypes proposalType);

        /// <summary>
        /// Get the Accepted Active Proposal for the Debt Counselling case
        /// </summary>
        IProposal AcceptedActiveProposal { get; }

        /// <summary>
        ///
        /// </summary>
        IList<IHearingDetail> GetActiveHearingDetails
        {
            get;
        }
    }
}