using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.DAO;
using NHibernate;
using System.Collections;
using NHibernate.Transform;
using NHibernate.Criterion;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AccountSearchQueryBase : ActiveRecordBaseQuery
    {
        private int _maxRowCount;
        /// <summary>
        /// 
        /// </summary>
        private IAccountSearchCriteria _searchCriteria;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="searchCriteria"></param>
        /// <param name="maxRowCount"></param>
        public AccountSearchQueryBase(Type type, IAccountSearchCriteria searchCriteria, int maxRowCount)
            : base(type)
        {
            _searchCriteria = searchCriteria;
            _maxRowCount = maxRowCount;
        }

        /// <summary>
        /// 
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

        /// <summary>
        /// 
        /// </summary>
        protected IAccountSearchCriteria SearchCriteria
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
    }

    /// <summary>
    /// 
    /// </summary>
    public class AccountSearchQuery : AccountSearchQueryBase
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="maxRowCount"></param>
        public AccountSearchQuery(IAccountSearchCriteria searchCriteria, int maxRowCount)
            : base(typeof(Account_DAO), searchCriteria, maxRowCount)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        protected override NHibernate.IQuery CreateQuery(NHibernate.ISession session)
        {
            string HQL = "from Account_DAO a";
            IQuery q = session.CreateQuery(HQL);
            q.SetResultTransformer(new NHibernate.Transform.DistinctRootEntityResultTransformer());
            return q;
        }

        /// <summary>
        /// Overrides <see cref="ActiveRecordBaseQuery.InternalExecute(ISession)"/>.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        protected override object InternalExecute(ISession session)
        {
            // build up our criteria
            ICriteria criteria = session.CreateCriteria(typeof(Account_DAO));

            // accountkey
            if (SearchCriteria.AccountKey.HasValue && SearchCriteria.AccountKey > 0)
                criteria.Add(Expression.Eq("Key", SearchCriteria.AccountKey));

            // set the discriminator on product types
            // if no product types have been specified then it will return all
            if (SearchCriteria.Products.Count > 0)
                criteria.Add(Expression.In("class", SearchCriteria.Products));

            ICriteria RC = criteria.CreateCriteria("Roles");
            criteria.SetFetchMode("Roles", FetchMode.Eager);

            ICriteria LE = RC.CreateCriteria("LegalEntity");
            RC.SetFetchMode("LegalEntity", FetchMode.Eager);

            LE.Add(Expression.Eq("class", (int)SAHL.Common.Globals.LegalEntityTypes.NaturalPerson));

            LE.AddOrder(new Order("Surname", true));
            LE.AddOrder(new Order("FirstNames", true));

            if ((SearchCriteria.Surname != null && SearchCriteria.Surname.Length > 0)
             || (SearchCriteria.FirstNames != null && SearchCriteria.FirstNames.Length > 0))
            {
                // surname
                if (SearchCriteria.Surname != null && SearchCriteria.Surname.Length > 0)
                {
                    if (SearchCriteria.ExactMatch)
                        LE.Add(Expression.Like("Surname", SearchCriteria.Surname));
                    else
                        LE.Add(Expression.Like("Surname", SearchCriteria.Surname, MatchMode.Start));
                }

                // firstnames
                if (SearchCriteria.FirstNames != null && SearchCriteria.FirstNames.Length > 0)
                {
                    if (SearchCriteria.ExactMatch)
                        LE.Add(Expression.Like("FirstNames", SearchCriteria.FirstNames));
                    else
                        LE.Add(Expression.Like("FirstNames", SearchCriteria.FirstNames, MatchMode.Start));
                }
            }

            // set max rowcounts
            criteria.SetMaxResults(MaxRowCount);

            criteria.AddOrder(new Order("Key", true));

            // set the criteria to return distinct records
            if (SearchCriteria.Distinct)
                criteria.SetResultTransformer(CriteriaSpecification.DistinctRootEntity);

            // run the query
            return criteria.List<Account_DAO>();
        }


    }

}
