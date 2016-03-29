using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ExpenseTypeGroup_DAO
    /// </summary>
    public partial interface IExpenseTypeGroup : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExpenseTypeGroup_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExpenseTypeGroup_DAO.Fee
        /// </summary>
        Boolean? Fee
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExpenseTypeGroup_DAO.Expense
        /// </summary>
        Boolean? Expense
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExpenseTypeGroup_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExpenseTypeGroup_DAO.ExpenseTypes
        /// </summary>
        IEventList<IExpenseType> ExpenseTypes
        {
            get;
        }
    }
}