using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.DebitOrderDay_DAO
    /// </summary>
    public partial interface IDebitOrderDay : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DebitOrderDay_DAO.Day
        /// </summary>
        System.Int32 Day
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DebitOrderDay_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}