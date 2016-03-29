using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AccountSequence_DAO
    /// </summary>
    public partial interface IAccountSequence : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountSequence_DAO.IsUsed
        /// </summary>
        System.Boolean IsUsed
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountSequence_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}