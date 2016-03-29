using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.FailedLegalEntityAddress_DAO
    /// </summary>
    public partial interface IFailedLegalEntityAddress : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedLegalEntityAddress_DAO.IsCleaned
        /// </summary>
        System.Boolean IsCleaned
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedLegalEntityAddress_DAO.PostalIsCleaned
        /// </summary>
        System.Boolean PostalIsCleaned
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedLegalEntityAddress_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedLegalEntityAddress_DAO.LegalEntity
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedLegalEntityAddress_DAO.FailedPostalMigration
        /// </summary>
        IFailedPostalMigration FailedPostalMigration
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedLegalEntityAddress_DAO.FailedStreetMigration
        /// </summary>
        IFailedStreetMigration FailedStreetMigration
        {
            get;
            set;
        }
    }
}