using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingExternalRole_DAO
    /// </summary>
    public partial interface IMigrationDebtCounsellingExternalRole : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingExternalRole_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingExternalRole_DAO.LegalEntityKey
        /// </summary>
        System.Int32 LegalEntityKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingExternalRole_DAO.ExternalRoleTypeKey
        /// </summary>
        System.Int32 ExternalRoleTypeKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.MigrationDebtCounsellingExternalRole_DAO.DebtCounselling
        /// </summary>
        IMigrationDebtCounselling DebtCounselling
        {
            get;
            set;
        }
    }
}