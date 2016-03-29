using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO
    /// </summary>
    public partial interface IMigrationDebtCounselling : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.AccountKey
        /// </summary>
        System.Int32 AccountKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.DebtCounsellingConsultantKey
        /// </summary>
        Int32? DebtCounsellingConsultantKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.DebtCounsellorKey
        /// </summary>
        Int32? DebtCounsellorKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.ProposalTypeKey
        /// </summary>
        System.Int32 ProposalTypeKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.DateOf171
        /// </summary>
        DateTime? DateOf171
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.ReviewDate
        /// </summary>
        DateTime? ReviewDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.CourtOrderDate
        /// </summary>
        DateTime? CourtOrderDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.TerminateDate
        /// </summary>
        DateTime? TerminateDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.SixtyDaysDate
        /// </summary>
        DateTime? SixtyDaysDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.ApprovalDate
        /// </summary>
        DateTime? ApprovalDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.ApprovalUserKey
        /// </summary>
        Int32? ApprovalUserKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.PaymentReceivedDate
        /// </summary>
        DateTime? PaymentReceivedDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.DebtCounsellingExternalRoles
        /// </summary>
        IEventList<IMigrationDebtCounsellingExternalRole> DebtCounsellingExternalRoles
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounselling_DAO.DebtCounsellingProposals
        /// </summary>
        IEventList<IMigrationDebtCounsellingProposal> DebtCounsellingProposals
        {
            get;
        }
    }
}