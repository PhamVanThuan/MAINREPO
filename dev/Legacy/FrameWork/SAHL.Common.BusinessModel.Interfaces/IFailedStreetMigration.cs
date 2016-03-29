using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO
    /// </summary>
    public partial interface IFailedStreetMigration : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.RecordType
        /// </summary>
        System.String RecordType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.ClientNumber
        /// </summary>
        System.Decimal ClientNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.Add1
        /// </summary>
        System.String Add1
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.Add2
        /// </summary>
        System.String Add2
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.Add3
        /// </summary>
        System.String Add3
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.Add4
        /// </summary>
        System.String Add4
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.Add5
        /// </summary>
        System.String Add5
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.PCode
        /// </summary>
        System.String PCode
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.City
        /// </summary>
        System.String City
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.Province
        /// </summary>
        System.String Province
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.Country
        /// </summary>
        System.String Country
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.ErrCode
        /// </summary>
        System.String ErrCode
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.ResultSet
        /// </summary>
        System.String ResultSet
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.FailedLegalEntityAddresses
        /// </summary>
        IEventList<IFailedLegalEntityAddress> FailedLegalEntityAddresses
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.FailedPropertyAddresses
        /// </summary>
        IEventList<IFailedPropertyAddress> FailedPropertyAddresses
        {
            get;
        }
    }
}