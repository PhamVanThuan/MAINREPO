using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.TransactionTypeBalanceEffect_DAO
    /// </summary>
    public partial interface ITransactionTypeBalanceEffect : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionTypeBalanceEffect_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionTypeBalanceEffect_DAO.TransactionType
        /// </summary>
        ITransactionType TransactionType
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionTypeBalanceEffect_DAO.BalanceType
        /// </summary>
        IBalanceType BalanceType
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionTypeBalanceEffect_DAO.TransactionEffect
        /// </summary>
        ITransactionEffect TransactionEffect
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionTypeBalanceEffect_DAO.SPVTransactionEffect
        /// </summary>
        ITransactionEffect SPVTransactionEffect
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionTypeBalanceEffect_DAO.ParentTransactionTypeBalanceEffectKey
        /// </summary>
        ITransactionTypeBalanceEffect ParentTransactionTypeBalanceEffectKey
        {
            get;
        }
    }
}