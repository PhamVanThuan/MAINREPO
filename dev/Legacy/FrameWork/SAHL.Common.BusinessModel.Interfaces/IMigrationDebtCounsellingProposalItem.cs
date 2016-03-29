using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO
    /// </summary>
    public partial interface IMigrationDebtCounsellingProposalItem : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.StartDate
        /// </summary>
        System.DateTime StartDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.EndDate
        /// </summary>
        System.DateTime EndDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.MarketRateKey
        /// </summary>
        System.Int32 MarketRateKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.InterestRate
        /// </summary>
        System.Decimal InterestRate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.Amount
        /// </summary>
        System.Decimal Amount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.AdditionalAmount
        /// </summary>
        System.Decimal AdditionalAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.CreateDate
        /// </summary>
        System.DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.InstalmentPercentage
        /// </summary>
        System.Decimal InstalmentPercentage
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.AnnualEscalation
        /// </summary>
        System.Decimal AnnualEscalation
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.StartPeriod
        /// </summary>
        System.Int32 StartPeriod
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.EndPeriod
        /// </summary>
        System.Int32 EndPeriod
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingProposalItem_DAO.DebtCounsellingProposal
        /// </summary>
        IMigrationDebtCounsellingProposal DebtCounsellingProposal
        {
            get;
            set;
        }
    }
}