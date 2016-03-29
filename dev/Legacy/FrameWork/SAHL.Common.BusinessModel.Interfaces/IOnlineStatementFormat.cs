using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.OnlineStatementFormat_DAO
    /// </summary>
    public partial interface IOnlineStatementFormat : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OnlineStatementFormat_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OnlineStatementFormat_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}