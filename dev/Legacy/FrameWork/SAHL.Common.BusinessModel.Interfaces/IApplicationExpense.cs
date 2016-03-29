using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO
    /// </summary>
    public partial interface IApplicationExpense : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.ExpenseAccountNumber
        /// </summary>
        System.String ExpenseAccountNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.ExpenseAccountName
        /// </summary>
        System.String ExpenseAccountName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.ExpenseReference
        /// </summary>
        System.String ExpenseReference
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.TotalOutstandingAmount
        /// </summary>
        System.Double TotalOutstandingAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.MonthlyPayment
        /// </summary>
        System.Double MonthlyPayment
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.ToBeSettled
        /// </summary>
        System.Boolean ToBeSettled
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.OverRidden
        /// </summary>
        System.Boolean OverRidden
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.ExpenseType
        /// </summary>
        IExpenseType ExpenseType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.LegalEntity
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.Application
        /// </summary>
        IApplication Application
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.ApplicationDebtSettlements
        /// </summary>
        IEventList<IApplicationDebtSettlement> ApplicationDebtSettlements
        {
            get;
        }
    }
}