using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using NHibernate;
using SAHL.Common.BusinessModel.DAO;
using NHibernate.Criterion;

namespace SAHL.Common.BusinessModel.Repositories
{
    public class BankAccountLegalEntitySearchQuery : ActiveRecordBaseQuery
    {
        private IBankAccountSearchCriteria _searchCriteria;
        private int _maxRowCount = 50;

        #region Constructors

        public BankAccountLegalEntitySearchQuery(IBankAccountSearchCriteria searchCriteria)
            : this(searchCriteria, 50)
        {
        }

        public BankAccountLegalEntitySearchQuery(IBankAccountSearchCriteria searchCriteria, int maxRowCount)
            : base(typeof(LegalEntityBankAccount_DAO))
        {
            _searchCriteria = searchCriteria;
            _maxRowCount = maxRowCount;
        }

        #endregion

        /// <summary>
        /// The maximum number of records to return.  This defaults to 50, and can be ignored by setting to a negative value.
        /// </summary>
        protected int MaxRowCount
        {
            get
            {
                return _maxRowCount;
            }
            set
            {
                _maxRowCount = value;
            }
        }

        protected IBankAccountSearchCriteria SearchCriteria
        {
            get
            {
                return _searchCriteria;
            }
            set
            {
                _searchCriteria = value;
            }
        }

        protected override NHibernate.IQuery CreateQuery(NHibernate.ISession session)
        {
            string HQL = "from LegalEntityBankAccount_DAO leba";
            return session.CreateQuery(HQL);
        }

        /// <summary>
        /// Overrides <see cref="ActiveRecordBaseQuery.InternalExecute(ISession)"/>.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        protected override object InternalExecute(ISession session)
        {
            // build up our criteria
            ICriteria criteria = session.CreateCriteria(typeof(LegalEntityBankAccount_DAO));
            ICriteria bankAccountCriteria = criteria.CreateCriteria("BankAccount");

            // branch key
            if (!String.IsNullOrEmpty(SearchCriteria.ACBBranchKey))
                bankAccountCriteria.CreateCriteria("ACBBranch").Add(Expression.Like("Key", SearchCriteria.ACBBranchKey, MatchMode.Start));

            // account type
            if (SearchCriteria.ACBTypeKey.HasValue)
                bankAccountCriteria.CreateCriteria("ACBType").Add(Expression.Eq("Key", SearchCriteria.ACBTypeKey));

            // account name
            if (!String.IsNullOrEmpty(SearchCriteria.AccountName))
                bankAccountCriteria.Add(Expression.Like("AccountName", SearchCriteria.AccountName, MatchMode.Start));

            // account number
            if (!String.IsNullOrEmpty(SearchCriteria.AccountNumber))
                bankAccountCriteria.Add(Expression.Eq("AccountNumber",SearchCriteria.AccountNumber));

            // account name
            //LegalEntityBankAccount_DAO l = new LegalEntityBankAccount_DAO();
            //l.BankAccount.AccountName
            //SearchCriteria.ACBTypeKey;
            //SearchCriteria.AccountName;
            //SearchCriteria.AccountNumber;

            //// post office
            //if (SearchCriteria.PostOfficeKey.HasValue)
            //    criteria.CreateCriteria("PostOffice").Add(Expression.Eq("Key", SearchCriteria.PostOfficeKey.Value));

            // set max rowcounts 
            if (MaxRowCount >= 0)
                criteria.SetMaxResults(MaxRowCount);

            return criteria.List<LegalEntityBankAccount_DAO>();
        }

    }

    

}
