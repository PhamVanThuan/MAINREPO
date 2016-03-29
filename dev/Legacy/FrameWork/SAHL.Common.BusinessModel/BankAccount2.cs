using System;
using System.Collections.Generic;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Utils;

namespace SAHL.Common.BusinessModel
{
    public partial class BankAccount : BusinessModelBase<SAHL.Common.BusinessModel.DAO.BankAccount_DAO>, IBankAccount
    {
        private IBankAccount _original;

        public override void ExtendedConstructor()
        {
            base.ExtendedConstructor();
            if (this.Key > 0)
            {
                SimpleBankAccount sba = new SimpleBankAccount();
                sba.ACBBranch = this.ACBBranch;
                sba.ACBType = this.ACBType;
                sba.AccountName = this.AccountName;
                sba.AccountNumber = this.AccountNumber;
                sba.ChangeDate = this.ChangeDate;
                sba.Key = this.Key;
                sba.UserID = this.UserID;
                _original = sba;
            }
        }

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);

            Rules.Add("BankAccountUnique");
            Rules.Add("BankAccountUpdateNotUsed");
            Rules.Add("BankAccountCDVValidation");
        }

        public string GetDisplayName(BankAccountNameFormat Format)
        {
            switch (Format)
            {
                case BankAccountNameFormat.Full:
                    return StringUtils.Delimit("-", new string[] {ACBBranch.ACBBank.ACBBankDescription, ACBBranch.Key, ACBBranch.ACBBranchDescription,
                        ACBType.ACBTypeDescription, AccountNumber, AccountName});
                case BankAccountNameFormat.Short:
                    return StringUtils.Delimit("-", new string[] {ACBBranch.ACBBank.ACBBankDescription, ACBBranch.Key, ACBBranch.ACBBranchDescription,
                        ACBType.ACBTypeDescription, AccountNumber, AccountName});
                default:
                    return "";
            }
        }

        public IReadOnlyEventList<IFinancialServiceBankAccount> GetFinancialServiceBankAccounts()
        {
            return GetFinancialServiceBankAccounts(50);
        }

        public IReadOnlyEventList<IFinancialServiceBankAccount> GetFinancialServiceBankAccounts(int maxRecords)
        {
            List<IFinancialServiceBankAccount> list = new List<IFinancialServiceBankAccount>();

            string hql = "SELECT f from FinancialServiceBankAccount_DAO f WHERE f.BankAccount.Key = ?";
            SimpleQuery<FinancialServiceBankAccount_DAO> q = new SimpleQuery<FinancialServiceBankAccount_DAO>(hql, Key);
            q.SetQueryRange(maxRecords);
            FinancialServiceBankAccount_DAO[] results = q.Execute();

            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            for (int i = 0; i < results.Length; i++)
            {
                list.Add(BMTM.GetMappedType<IFinancialServiceBankAccount, FinancialServiceBankAccount_DAO>(results[i]));
            }

            return new ReadOnlyEventList<IFinancialServiceBankAccount>(list);
        }

        /// <summary>
        /// Gets a shallow copy of the object when it was first loaded.  For new bank accounts, this will
        /// be null.  Collections and methods are not implemented.
        /// </summary>
        public IBankAccount Original
        {
            get
            {
                return _original;
            }
        }

        private class SimpleBankAccount : IBankAccount
        {
            private IACBBranch _acbBranch;
            private string _accountNumber;
            private IACBType _acbType;
            private string _accountName;
            private string _userID;
            private DateTime? _changeDate;
            private int _key;

            #region IBankAccount Members

            public IACBBranch ACBBranch
            {
                get
                {
                    return _acbBranch;
                }
                set
                {
                    _acbBranch = value;
                }
            }

            public string AccountNumber
            {
                get
                {
                    return _accountNumber;
                }
                set
                {
                    _accountNumber = value;
                }
            }

            public IACBType ACBType
            {
                get
                {
                    return _acbType;
                }
                set
                {
                    _acbType = value;
                }
            }

            public string AccountName
            {
                get
                {
                    return _accountName;
                }
                set
                {
                    _accountName = value;
                }
            }

            public string UserID
            {
                get
                {
                    return _userID;
                }
                set
                {
                    _userID = value;
                }
            }

            public DateTime? ChangeDate
            {
                get
                {
                    return _changeDate;
                }
                set
                {
                    _changeDate = value;
                }
            }

            public int Key
            {
                get
                {
                    return _key;
                }
                set
                {
                    _key = value;
                }
            }

            public string GetDisplayName(BankAccountNameFormat Format)
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }

            public IReadOnlyEventList<IFinancialServiceBankAccount> GetFinancialServiceBankAccounts()
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }

            public IReadOnlyEventList<IFinancialServiceBankAccount> GetFinancialServiceBankAccounts(int maxRecords)
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }

            public IBankAccount Original
            {
                get { throw new NotImplementedException("The method or operation is not implemented."); }
            }

            #endregion IBankAccount Members

            #region IEntityValidation Members

            public bool ValidateEntity()
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }

            #endregion IEntityValidation Members

            #region IBusinessModelObject Members

            public object Clone()
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }

            public void Refresh()
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }

            #endregion IBusinessModelObject Members
        }
    }
}