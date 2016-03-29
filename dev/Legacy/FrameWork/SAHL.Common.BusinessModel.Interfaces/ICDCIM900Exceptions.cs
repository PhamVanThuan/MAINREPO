using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CDCIM900Exceptions_DAO
    /// </summary>
    public partial interface ICDCIM900Exceptions : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDCIM900Exceptions_DAO.FileDate
        /// </summary>
        System.DateTime FileDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDCIM900Exceptions_DAO.RecordNumber
        /// </summary>
        System.Int32 RecordNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDCIM900Exceptions_DAO.BranchNumber
        /// </summary>
        System.Int32 BranchNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDCIM900Exceptions_DAO.Exception
        /// </summary>
        System.String Exception
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDCIM900Exceptions_DAO.ActionDate
        /// </summary>
        System.DateTime ActionDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDCIM900Exceptions_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}