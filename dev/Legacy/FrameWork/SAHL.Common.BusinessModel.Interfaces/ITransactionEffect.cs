using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.TransactionEffect_DAO
    /// </summary>
    public partial interface ITransactionEffect : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionEffect_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionEffect_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionEffect_DAO.Coefficient
        /// </summary>
        System.Double Coefficient
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionEffect_DAO.BalanceEffect
        /// </summary>
        System.Double BalanceEffect
        {
            get;
        }
    }
}