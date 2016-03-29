using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.DebtCounsellorDetail_DAO
    /// </summary>
    public partial interface IDebtCounsellorDetail : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DebtCounsellorDetail_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DebtCounsellorDetail_DAO.NCRDCRegistrationNumber
        /// </summary>
        System.String NCRDCRegistrationNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DebtCounsellorDetail_DAO.LegalEntity
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }
    }
}