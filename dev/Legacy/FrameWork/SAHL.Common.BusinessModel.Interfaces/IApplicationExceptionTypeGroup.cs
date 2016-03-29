using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationExceptionTypeGroup_DAO
    /// </summary>
    public partial interface IApplicationExceptionTypeGroup : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExceptionTypeGroup_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExceptionTypeGroup_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExceptionTypeGroup_DAO.ApplicationExceptionTypes
        /// </summary>
        IEventList<IApplicationExceptionType> ApplicationExceptionTypes
        {
            get;
        }
    }
}