using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Proposal_DAO
    /// </summary>
    public partial interface IProposal : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Proposal_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Proposal_DAO.DebtCounselling
        /// </summary>
        IDebtCounselling DebtCounselling
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Proposal_DAO.ADUser
        /// </summary>
        IADUser ADUser
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Proposal_DAO.CreateDate
        /// </summary>
        System.DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Proposal_DAO.ProposalItems
        /// </summary>
        IEventList<IProposalItem> ProposalItems
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Proposal_DAO.ProposalStatus
        /// </summary>
        IProposalStatus ProposalStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Proposal_DAO.ProposalType
        /// </summary>
        IProposalType ProposalType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Proposal_DAO.HOCInclusive
        /// </summary>
        Boolean? HOCInclusive
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Proposal_DAO.LifeInclusive
        /// </summary>
        Boolean? LifeInclusive
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Proposal_DAO.Accepted
        /// </summary>
        Boolean? Accepted
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Proposal_DAO.ReviewDate
        /// </summary>
        DateTime? ReviewDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Proposal_DAO.MonthlyServiceFeeInclusive
        /// </summary>
        System.Boolean MonthlyServiceFeeInclusive
        {
            get;
            set;
        }

    }
}