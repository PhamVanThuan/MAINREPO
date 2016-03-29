using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ManualDebitOrder_DAO
    /// </summary>
    public partial interface IManualDebitOrder : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ManualDebitOrder_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ManualDebitOrder_DAO.InsertDate
        /// </summary>
        System.DateTime InsertDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ManualDebitOrder_DAO.ActionDate
        /// </summary>
        System.DateTime ActionDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ManualDebitOrder_DAO.Reference
        /// </summary>
        System.String Reference
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ManualDebitOrder_DAO.Amount
        /// </summary>
        System.Double Amount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ManualDebitOrder_DAO.UserID
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ManualDebitOrder_DAO.BankAccount
        /// </summary>
        IBankAccount BankAccount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ManualDebitOrder_DAO.FinancialService
        /// </summary>
        IFinancialService FinancialService
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ManualDebitOrder_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ManualDebitOrder_DAO.Memo
        /// </summary>
        IMemo Memo
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ManualDebitOrder_DAO.TransactionType
        /// </summary>
        ITransactionType TransactionType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ManualDebitOrder_DAO.BatchTotal
        /// </summary>
        IBatchTotal BatchTotal
        {
            get;
            set;
        }
    }
}