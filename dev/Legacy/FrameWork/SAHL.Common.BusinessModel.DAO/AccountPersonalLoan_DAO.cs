using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Derived from Account_DAO and is instantiated to represent Personal Loan accounts.
    /// </summary>
    /// <seealso cref="Account_DAO"/>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("Account", Schema = "dbo", DiscriminatorValue = "12", Lazy = true)]
    public class AccountPersonalLoan_DAO : Account_DAO
    {
        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountPersonalLoan_DAO Find(int id)
        {
            return ActiveRecordBase<AccountPersonalLoan_DAO>.Find(id).As<AccountPersonalLoan_DAO>();
        }

        public new static AccountPersonalLoan_DAO Find(object id)
        {
            return ActiveRecordBase<AccountPersonalLoan_DAO>.Find(id).As<AccountPersonalLoan_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountPersonalLoan_DAO FindFirst()
        {
            return ActiveRecordBase<AccountPersonalLoan_DAO>.FindFirst().As<AccountPersonalLoan_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountPersonalLoan_DAO FindOne()
        {
            return ActiveRecordBase<AccountPersonalLoan_DAO>.FindOne().As<AccountPersonalLoan_DAO>();
        }

        #endregion Static Overrides
    }
}