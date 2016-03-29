using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.PaymentAdjustment_DAO
    /// </summary>
    public partial interface IPaymentAdjustment : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PaymentAdjustment_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PaymentAdjustment_DAO.Amount
        /// </summary>
        System.Double Amount
        {
            get;
        }
    }
}