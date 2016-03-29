using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ImportFile_DAO
    /// </summary>
    public partial interface IImportFile : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportFile_DAO.FileName
        /// </summary>
        System.String FileName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportFile_DAO.FileType
        /// </summary>
        System.String FileType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportFile_DAO.DateImported
        /// </summary>
        System.DateTime DateImported
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportFile_DAO.Status
        /// </summary>
        System.String Status
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportFile_DAO.UserID
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportFile_DAO.XmlData
        /// </summary>
        System.String XmlData
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportFile_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}