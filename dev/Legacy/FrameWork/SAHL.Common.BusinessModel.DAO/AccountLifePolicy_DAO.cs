using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Derived from Account_DAO. Instantiated to represent a lifepolicy account.
    /// </summary>
    /// <seealso cref="Account_DAO"/>
    [GenericTest(Globals.TestType.Find)]

    [ActiveRecord("Account", Schema = "dbo", DiscriminatorValue = "4", Lazy = true)]
    public partial class AccountLifePolicy_DAO : Account_DAO
    {
        private IList<LifeInsurableInterest_DAO> _lifeInsurableInterests;

        /// <summary>
        /// The Parent Mortgage Loan Account associated to the Life Policy.
        /// </summary>
        public virtual Account_DAO ParentMortgageLoan
        {
            get
            {
                Account_DAO LoanAccount = null;
                if (this.ParentAccount != null)
                {
                    if ((this.ParentAccount.GetType() != typeof(AccountLifePolicy_DAO)) && (this.ParentAccount.GetType() != typeof(AccountHOC_DAO)))
                    {
                        LoanAccount = (Account_DAO)this.ParentAccount;
                    }
                }

                return LoanAccount;
            }
        }

        /// <summary>
        /// The Financial Service associated to the Life Policy
        /// </summary>
        public virtual LifePolicy_DAO LifePolicyFinancialService
        {
            get
            {
                //LifePolicy_DAO LifePolicyFinancialService = null;

                for (int i = 0; i < this.FinancialServices.Count; i++)
                {
                    if (this.FinancialServices[i].FinancialServiceType.Key == (int)FinancialServiceTypes.LifePolicy)
                    {
                        return LifePolicy_DAO.FindByPrimaryKey(this.FinancialServices[i].Key);
                        //LifePolicyFinancialService = (LifePolicy_DAO)this.FinancialServices[i];
                    }
                }
                return null;
            }
        }

        //[HasMany(typeof(FinancialService_DAO), ColumnKey = "AccountKey", Table = "FinancialService", Lazy = true)]
        /// <summary>
        /// Each of the Legal Entities playing a Role on a Life Policy Account require an Insurable Interest. This relationship is defined
        /// in the LifeInsurableInterest table, which relates the Account to the Legal Entity and the type of Insurable Interest that the Legal
        /// Entity has on the Account.
        /// </summary>
        [HasMany(typeof(LifeInsurableInterest_DAO), ColumnKey = "AccountKey", Table = "LifeInsurableInterest", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<LifeInsurableInterest_DAO> LifeInsurableInterests
        {
            get { return _lifeInsurableInterests; }
            set { _lifeInsurableInterests = value; }
        }

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountLifePolicy_DAO Find(int id)
        {
            return ActiveRecordBase<AccountLifePolicy_DAO>.Find(id).As<AccountLifePolicy_DAO>();
        }

        public new static AccountLifePolicy_DAO Find(object id)
        {
            return ActiveRecordBase<AccountLifePolicy_DAO>.Find(id).As<AccountLifePolicy_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountLifePolicy_DAO FindFirst()
        {
            return ActiveRecordBase<AccountLifePolicy_DAO>.FindFirst().As<AccountLifePolicy_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AccountLifePolicy_DAO FindOne()
        {
            return ActiveRecordBase<AccountLifePolicy_DAO>.FindOne().As<AccountLifePolicy_DAO>();
        }

        #endregion Static Overrides
    }
}