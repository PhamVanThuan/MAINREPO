using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationException_DAO
    /// </summary>
    public partial interface IApplicationException : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationException_DAO.Application
        /// </summary>
        IApplication Application
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationException_DAO.OverRidden
        /// </summary>
        System.Boolean OverRidden
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationException_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationException_DAO.ApplicationExceptionType
        /// </summary>
        IApplicationExceptionType ApplicationExceptionType
        {
            get;
            set;
        }
    }
}