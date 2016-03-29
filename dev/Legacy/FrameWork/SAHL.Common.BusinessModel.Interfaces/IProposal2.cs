using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Proposal_DAO
    /// </summary>
    public partial interface IProposal : IEntityValidation, IBusinessModelObject
    {
        IMemo Memo { get; set; }

        int TotalTerm { get; }

        IReason AcceptedReason { get; }

        double? AcceptedRate { get; }

        DateTime? AcceptedDate { get; }

        IADUser AcceptedUser { get; }

        IReason ActiveReason { get; }
    }
}