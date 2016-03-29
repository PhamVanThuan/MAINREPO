using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO
    /// </summary>
    public partial interface IAccountExpense : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.ExpenseAccountNumber
        /// </summary>
        System.String ExpenseAccountNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.ExpenseAccountName
        /// </summary>
        System.String ExpenseAccountName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.ExpenseReference
        /// </summary>
        System.String ExpenseReference
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.TotalOutstandingAmount
        /// </summary>
        System.Double TotalOutstandingAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.MonthlyPayment
        /// </summary>
        System.Double MonthlyPayment
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.ToBeSettled
        /// </summary>
        System.Boolean ToBeSettled
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.OverRidden
        /// </summary>
        System.Boolean OverRidden
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.AccountDebtSettlements
        /// </summary>
        IEventList<IAccountDebtSettlement> AccountDebtSettlements
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.Account
        /// </summary>
        IAccount Account
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.ExpenseType
        /// </summary>
        IExpenseType ExpenseType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.LegalEntity
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }
    }
}