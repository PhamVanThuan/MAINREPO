using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.RegentStatus_DAO
    /// </summary>
    public partial interface IRegentStatus : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegentStatus_DAO.RegentStatusDescription
        /// </summary>
        System.String RegentStatusDescription
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.RegentStatus_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}