using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Detail_DAO
    /// </summary>
    public partial interface IDetail : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Detail_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}