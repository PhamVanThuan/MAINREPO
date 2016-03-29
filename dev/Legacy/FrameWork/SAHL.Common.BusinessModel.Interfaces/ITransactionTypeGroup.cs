using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.TransactionTypeGroup_DAO
    /// </summary>
    public partial interface ITransactionTypeGroup : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionTypeGroup_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionTypeGroup_DAO.TransactionGroup
        /// </summary>
        ITransactionGroup TransactionGroup
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.TransactionTypeGroup_DAO.TransactionType
        /// </summary>
        ITransactionType TransactionType
        {
            get;
        }
    }
}