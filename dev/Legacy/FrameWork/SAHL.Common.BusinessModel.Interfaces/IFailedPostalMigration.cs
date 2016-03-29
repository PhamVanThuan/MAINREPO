using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO
    /// </summary>
    public partial interface IFailedPostalMigration : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.RecordType
        /// </summary>
        System.String RecordType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.ClientNumber
        /// </summary>
        System.Decimal ClientNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.ClientBoxNumber
        /// </summary>
        System.String ClientBoxNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.ClientBoxNumber2
        /// </summary>
        System.String ClientBoxNumber2
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.NewAdd3
        /// </summary>
        System.String NewAdd3
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.ClientPostOffice
        /// </summary>
        System.String ClientPostOffice
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.ClientPostalCode
        /// </summary>
        System.String ClientPostalCode
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.NewCity
        /// </summary>
        System.String NewCity
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.NewProvince
        /// </summary>
        System.String NewProvince
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.NewCountry
        /// </summary>
        System.String NewCountry
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.Faults
        /// </summary>
        System.String Faults
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.ResultSet
        /// </summary>
        System.String ResultSet
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.FailedLegalEntityAddresses
        /// </summary>
        IEventList<IFailedLegalEntityAddress> FailedLegalEntityAddresses
        {
            get;
        }
    }
}