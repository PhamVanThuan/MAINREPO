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
    [ActiveRecord("Account", Schema = "dbo", DiscriminatorValue = "13", Lazy = true)]
    public class AccountCreditProtectionPlan_DAO : Account_DAO
    {
        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
		public static AccountCreditProtectionPlan_DAO Find(int id)
        {
			return ActiveRecordBase<AccountCreditProtectionPlan_DAO>.Find(id).As<AccountCreditProtectionPlan_DAO>();
        }

		public new static AccountCreditProtectionPlan_DAO Find(object id)
        {
			return ActiveRecordBase<AccountCreditProtectionPlan_DAO>.Find(id).As<AccountCreditProtectionPlan_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
		public static AccountCreditProtectionPlan_DAO FindFirst()
        {
			return ActiveRecordBase<AccountCreditProtectionPlan_DAO>.FindFirst().As<AccountCreditProtectionPlan_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
		public static AccountCreditProtectionPlan_DAO FindOne()
        {
			return ActiveRecordBase<AccountCreditProtectionPlan_DAO>.FindOne().As<AccountCreditProtectionPlan_DAO>();
        }

        #endregion Static Overrides
    }
}