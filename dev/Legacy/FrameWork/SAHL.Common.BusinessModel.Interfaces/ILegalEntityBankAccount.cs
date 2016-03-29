using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// The LegalEntityBankAccount_DAO class links the Legal Entity to one or more Bank Accounts.
    /// </summary>
    public partial interface ILegalEntityBankAccount : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The foreign key reference to the Bank Account table which has the details regarding the Bank Account.
        /// Each LegalEntityBankAccountKey belongs to a single BankAccountKey
        /// </summary>
        IBankAccount BankAccount
        {
            get;
            set;
        }

        /// <summary>
        /// The UserID of the last person who updated information on the LegalEntityBankAccount.
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// The date the record was last changed.
        /// </summary>
        DateTime? ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign key reference to the LegalEntity table where the details of the Legal Entity are stored. Each LegalEntityBankAccountKey
        /// belongs to a single LegalEntityKey.
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }
    }
}