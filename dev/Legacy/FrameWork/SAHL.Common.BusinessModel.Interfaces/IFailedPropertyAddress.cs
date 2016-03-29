using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.FailedPropertyAddress_DAO
    /// </summary>
    public partial interface IFailedPropertyAddress : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedPropertyAddress_DAO.IsCleaned
        /// </summary>
        System.Boolean IsCleaned
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedPropertyAddress_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedPropertyAddress_DAO.Property
        /// </summary>
        IProperty Property
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedPropertyAddress_DAO.FailedStreetMigration
        /// </summary>
        IFailedStreetMigration FailedStreetMigration
        {
            get;
            set;
        }
    }
}