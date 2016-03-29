using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO
    /// </summary>
    public partial interface IBatchTotal : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO.Amount
        /// </summary>
        System.Double Amount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO.DebitOrderDate
        /// </summary>
        System.DateTime DebitOrderDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO.ActionDate
        /// </summary>
        System.DateTime ActionDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO.TransactionTypeKey
        /// </summary>
        System.Int32 TransactionTypeKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO.ManualDebitOrders
        /// </summary>
        IEventList<IManualDebitOrder> ManualDebitOrders
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO.Account
        /// </summary>
        IAccount Account
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BatchTotal_DAO.TransactionType
        /// </summary>
        ITransactionType TransactionType
        {
            get;
            set;
        }
    }
}