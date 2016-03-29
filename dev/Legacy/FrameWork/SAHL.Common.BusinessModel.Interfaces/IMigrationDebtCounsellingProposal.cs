using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposal_DAO
    /// </summary>
    public partial interface IMigrationDebtCounsellingProposal : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposal_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposal_DAO.ProposalStatusKey
        /// </summary>
        System.Int32 ProposalStatusKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposal_DAO.HOCInclusive
        /// </summary>
        System.Boolean HOCInclusive
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposal_DAO.LifeInclusive
        /// </summary>
        System.Boolean LifeInclusive
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposal_DAO.AcceptedProposal
        /// </summary>
        System.Boolean AcceptedProposal
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposal_DAO.ProposalDate
        /// </summary>
        System.DateTime ProposalDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposal_DAO.DebtCounsellingProposalItems
        /// </summary>
        IEventList<IMigrationDebtCounsellingProposalItem> DebtCounsellingProposalItems
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposal_DAO.DebtCounselling
        /// </summary>
        IMigrationDebtCounselling DebtCounselling
        {
            get;
            set;
        }
    }
}