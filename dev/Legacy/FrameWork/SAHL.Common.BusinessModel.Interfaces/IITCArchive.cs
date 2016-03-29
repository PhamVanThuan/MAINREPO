using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ITCArchive_DAO
    /// </summary>
    public partial interface IITCArchive : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCArchive_DAO.LegalEntityKey
        /// </summary>
        System.Int32 LegalEntityKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCArchive_DAO.AccountKey
        /// </summary>
        System.Int32? AccountKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCArchive_DAO.ChangeDate
        /// </summary>
        System.DateTime ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCArchive_DAO.ResponseXML
        /// </summary>
        System.String ResponseXML
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCArchive_DAO.ResponseStatus
        /// </summary>
        System.String ResponseStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCArchive_DAO.UserID
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCArchive_DAO.ArchiveUser
        /// </summary>
        System.String ArchiveUser
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCArchive_DAO.ArchiveDate
        /// </summary>
        System.DateTime ArchiveDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCArchive_DAO.RequestXML
        /// </summary>
        System.String RequestXML
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ITCArchive_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}