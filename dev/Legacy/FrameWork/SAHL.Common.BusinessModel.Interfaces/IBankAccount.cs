using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BankAccount_DAO
    /// </summary>
    public partial interface IBankAccount : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankAccount_DAO.ACBBranch
        /// </summary>
        IACBBranch ACBBranch
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankAccount_DAO.AccountNumber
        /// </summary>
        System.String AccountNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankAccount_DAO.ACBType
        /// </summary>
        IACBType ACBType
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankAccount_DAO.AccountName
        /// </summary>
        System.String AccountName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankAccount_DAO.UserID
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankAccount_DAO.ChangeDate
        /// </summary>
        DateTime? ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BankAccount_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}