using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.DAO;
using NHibernate;
using NHibernate.Criterion;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AddressSearchQuery : ActiveRecordBaseQuery
    {
        private int _maxRowCount;
        /// <summary>
        /// 
        /// </summary>
        private IAddressSearchCriteria _searchCriteria;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="searchCriteria"></param>
        /// <param name="maxRowCount"></param>
        public AddressSearchQuery(Type type, IAddressSearchCriteria searchCriteria, int maxRowCount)
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
        protected IAddressSearchCriteria SearchCriteria
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
    public class AddressBoxSearchQuery : AddressSearchQuery
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="maxRowCount"></param>
        public AddressBoxSearchQuery(IAddressSearchCriteria searchCriteria, int maxRowCount)
            : base(typeof(AddressBox_DAO), searchCriteria, maxRowCount)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        protected override NHibernate.IQuery CreateQuery(NHibernate.ISession session)
        {
            string HQL = "from AddressBox_DAO a";
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
            ICriteria criteria = session.CreateCriteria(typeof(AddressBox_DAO));

            // box number
            if (SearchCriteria.ExactMatch)
            {
                if (SearchCriteria.BoxNumber == null)
                    criteria.Add(Expression.IsNull("BoxNumber"));
                else
                    criteria.Add(Expression.Eq("BoxNumber", SearchCriteria.BoxNumber));
            }
            else if (SearchCriteria.BoxNumber != null && SearchCriteria.BoxNumber.Length > 0)
                criteria.Add(Expression.Like("BoxNumber", SearchCriteria.BoxNumber, MatchMode.Start));

            // post office
            if (SearchCriteria.PostOfficeKey.HasValue)
                criteria.CreateCriteria("PostOffice").Add(Expression.Eq("Key", SearchCriteria.PostOfficeKey.Value));

            // set max rowcounts
            criteria.SetMaxResults(MaxRowCount);
            criteria.AddOrder(new Order("Key", true));

            return criteria.List<AddressBox_DAO>();
        }


    }

    /// <summary>
    /// 
    /// </summary>
    public class AddressClusterBoxSearchQuery : AddressSearchQuery
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="maxRowCount"></param>
        public AddressClusterBoxSearchQuery(IAddressSearchCriteria searchCriteria, int maxRowCount)
            : base(typeof(AddressClusterBox_DAO), searchCriteria, maxRowCount)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        protected override NHibernate.IQuery CreateQuery(NHibernate.ISession session)
        {
            string HQL = "from AddressClusterBox_DAO a";
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
            ICriteria criteria = session.CreateCriteria(typeof(AddressClusterBox_DAO));

            // cluster box number
            if (SearchCriteria.ExactMatch)
            {
                if (SearchCriteria.ClusterBoxNumber == null)
                    criteria.Add(Expression.IsNull("ClusterBoxNumber"));
                else
                    criteria.Add(Expression.Eq("ClusterBoxNumber", SearchCriteria.ClusterBoxNumber));
            }
            else if (SearchCriteria.ClusterBoxNumber != null && SearchCriteria.ClusterBoxNumber.Length > 0)
                criteria.Add(Expression.Like("ClusterBoxNumber", SearchCriteria.ClusterBoxNumber, MatchMode.Start));

            // post office
            if (SearchCriteria.PostOfficeKey.HasValue)
                criteria.CreateCriteria("PostOffice").Add(Expression.Eq("Key", SearchCriteria.PostOfficeKey.Value));

            // set max rowcounts
            criteria.SetMaxResults(MaxRowCount);
            criteria.AddOrder(new Order("Key", true));

            return criteria.List<AddressClusterBox_DAO>();
        }


    }

    /// <summary>
    /// 
    /// </summary>
    public class AddressFreeTextSearchQuery : AddressSearchQuery
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="maxRowCount"></param>
        public AddressFreeTextSearchQuery(IAddressSearchCriteria searchCriteria, int maxRowCount)
            : base(typeof(AddressFreeText_DAO), searchCriteria, maxRowCount)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        protected override NHibernate.IQuery CreateQuery(NHibernate.ISession session)
        {
            string HQL = "from AddressFreeText_DAO a";
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
            ICriteria criteria = session.CreateCriteria(typeof(AddressFreeText_DAO));

            // free text line 1
            if (SearchCriteria.ExactMatch)
            {
                if (SearchCriteria.FreeTextLine1 == null)
                    criteria.Add(Expression.IsNull("FreeText1"));
                else
                    criteria.Add(Expression.Eq("FreeText1", SearchCriteria.FreeTextLine1));
            }
            else if (SearchCriteria.FreeTextLine1 != null && SearchCriteria.FreeTextLine1.Length > 0)
                criteria.Add(Expression.Like("FreeText1", SearchCriteria.FreeTextLine1, MatchMode.Start));

            // free text line 2
            if (SearchCriteria.ExactMatch)
            {
                if (SearchCriteria.FreeTextLine2 == null)
                    criteria.Add(Expression.IsNull("FreeText2"));
                else
                    criteria.Add(Expression.Eq("FreeText2", SearchCriteria.FreeTextLine2));
            }
            else if (SearchCriteria.FreeTextLine2 != null && SearchCriteria.FreeTextLine2.Length > 0)
                criteria.Add(Expression.Like("FreeText2", SearchCriteria.FreeTextLine2, MatchMode.Start));

            // free text line 3
            if (SearchCriteria.ExactMatch)
            {
                if (SearchCriteria.FreeTextLine3 == null)
                    criteria.Add(Expression.IsNull("FreeText3"));
                else
                    criteria.Add(Expression.Eq("FreeText3", SearchCriteria.FreeTextLine3));
            }
            else if (SearchCriteria.FreeTextLine3 != null && SearchCriteria.FreeTextLine3.Length > 0)
                criteria.Add(Expression.Like("FreeText3", SearchCriteria.FreeTextLine3, MatchMode.Start));

            // free text line 4
            if (SearchCriteria.ExactMatch)
            {
                if (SearchCriteria.FreeTextLine4 == null)
                    criteria.Add(Expression.IsNull("FreeText4"));
                else
                    criteria.Add(Expression.Eq("FreeText4", SearchCriteria.FreeTextLine4));
            }
            else if (SearchCriteria.FreeTextLine4 != null && SearchCriteria.FreeTextLine4.Length > 0)
                criteria.Add(Expression.Like("FreeText4", SearchCriteria.FreeTextLine4, MatchMode.Start));

            // free text line 5
            if (SearchCriteria.ExactMatch)
            {
                if (SearchCriteria.FreeTextLine5 == null)
                    criteria.Add(Expression.IsNull("FreeText5"));
                else
                    criteria.Add(Expression.Eq("FreeText5", SearchCriteria.FreeTextLine5));
            }
            else if (SearchCriteria.FreeTextLine5 != null && SearchCriteria.FreeTextLine5.Length > 0)
                criteria.Add(Expression.Like("FreeText5", SearchCriteria.FreeTextLine5, MatchMode.Start));

            // post office
            if (SearchCriteria.PostOfficeKey.HasValue)
                criteria.CreateCriteria("PostOffice").Add(Expression.Eq("Key", SearchCriteria.PostOfficeKey.Value));

            // set max rowcounts
            criteria.SetMaxResults(MaxRowCount);
            criteria.AddOrder(new Order("Key", true));

            return criteria.List<AddressFreeText_DAO>();
        }


    }

    /// <summary>
    /// 
    /// </summary>
    public class AddressPostnetSuiteSearchQuery : AddressSearchQuery
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="maxRowCount"></param>
        public AddressPostnetSuiteSearchQuery(IAddressSearchCriteria searchCriteria, int maxRowCount)
            : base(typeof(AddressPostnetSuite_DAO), searchCriteria, maxRowCount)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        protected override NHibernate.IQuery CreateQuery(NHibernate.ISession session)
        {
            string HQL = "from AddressPostnetSuite_DAO a";
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
            ICriteria criteria = session.CreateCriteria(typeof(AddressPostnetSuite_DAO));

            // postnet suite number
            if (SearchCriteria.ExactMatch)
            {
                if (SearchCriteria.PostnetSuiteNumber == null)
                    criteria.Add(Expression.IsNull("SuiteNumber"));
                else
                    criteria.Add(Expression.Eq("SuiteNumber", SearchCriteria.PostnetSuiteNumber));
            }
            else if (SearchCriteria.PostnetSuiteNumber != null && SearchCriteria.PostnetSuiteNumber.Length > 0)
                criteria.Add(Expression.Like("SuiteNumber", SearchCriteria.PostnetSuiteNumber, MatchMode.Start));

            // private bag number
            if (SearchCriteria.ExactMatch)
            {
                if (SearchCriteria.PrivateBagNumber == null)
                    criteria.Add(Expression.IsNull("PrivateBagNumber"));
                else
                    criteria.Add(Expression.Eq("PrivateBagNumber", SearchCriteria.PrivateBagNumber));
            }
            else if (SearchCriteria.PrivateBagNumber != null && SearchCriteria.PrivateBagNumber.Length > 0)
                criteria.Add(Expression.Like("PrivateBagNumber", SearchCriteria.PrivateBagNumber, MatchMode.Start));

            // post office
            if (SearchCriteria.PostOfficeKey.HasValue)
                criteria.CreateCriteria("PostOffice").Add(Expression.Eq("Key", SearchCriteria.PostOfficeKey.Value));

            // set max rowcounts
            criteria.SetMaxResults(MaxRowCount);
            criteria.AddOrder(new Order("Key", true));

            return criteria.List<AddressPostnetSuite_DAO>();
        }


    }

    /// <summary>
    /// 
    /// </summary>
    public class AddressPrivateBagSearchQuery : AddressSearchQuery
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="maxRowCount"></param>
        public AddressPrivateBagSearchQuery(IAddressSearchCriteria searchCriteria, int maxRowCount)
            : base(typeof(AddressPrivateBag_DAO), searchCriteria, maxRowCount)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        protected override NHibernate.IQuery CreateQuery(NHibernate.ISession session)
        {
            string HQL = "from AddressPrivateBag_DAO a";
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
            ICriteria criteria = session.CreateCriteria(typeof(AddressPrivateBag_DAO));

            // private number
            if (SearchCriteria.ExactMatch)
            {
                if (SearchCriteria.PrivateBagNumber == null)
                    criteria.Add(Expression.IsNull("PrivateBagNumber"));
                else
                    criteria.Add(Expression.Eq("PrivateBagNumber", SearchCriteria.PrivateBagNumber));
            }
            else if (SearchCriteria.PrivateBagNumber != null && SearchCriteria.PrivateBagNumber.Length > 0)
                criteria.Add(Expression.Like("PrivateBagNumber", SearchCriteria.PrivateBagNumber, MatchMode.Start));

            // post office
            if (SearchCriteria.PostOfficeKey.HasValue)
                criteria.CreateCriteria("PostOffice").Add(Expression.Eq("Key", SearchCriteria.PostOfficeKey.Value));

            // set max rowcounts
            criteria.SetMaxResults(MaxRowCount);
            criteria.AddOrder(new Order("Key", true));

            return criteria.List<AddressPrivateBag_DAO>();
        }


    }

    /// <summary>
    /// 
    /// </summary>
    public class AddressStreetSearchQuery : AddressSearchQuery
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="maxRowCount"></param>
        public AddressStreetSearchQuery(IAddressSearchCriteria searchCriteria, int maxRowCount)
            : base(typeof(AddressStreet_DAO), searchCriteria, maxRowCount)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        protected override NHibernate.IQuery CreateQuery(NHibernate.ISession session)
        {
            string HQL = "from AddressBox_DAO a";
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
            ICriteria criteria = session.CreateCriteria(typeof(AddressStreet_DAO));

            // building name
            if (SearchCriteria.ExactMatch)
            {
                if (SearchCriteria.BuildingName == null)
                    criteria.Add(Expression.IsNull("BuildingName"));
                else
                    criteria.Add(Expression.Eq("BuildingName", SearchCriteria.BuildingName));
            }
            else if (SearchCriteria.BuildingName != null && SearchCriteria.BuildingName.Length > 0)
                criteria.Add(Expression.Like("BuildingName", SearchCriteria.BuildingName, MatchMode.Start));

            // building number
            if (SearchCriteria.ExactMatch)
            {
                if (SearchCriteria.BuildingNumber == null)
                    criteria.Add(Expression.IsNull("BuildingNumber"));
                else
                    criteria.Add(Expression.Eq("BuildingNumber", SearchCriteria.BuildingNumber));
            }
            else if (SearchCriteria.BuildingNumber != null && SearchCriteria.BuildingNumber.Length > 0)
                criteria.Add(Expression.Like("BuildingNumber", SearchCriteria.BuildingNumber, MatchMode.Start));

            // country
            if (SearchCriteria.Country != null && SearchCriteria.Country.Length > 0)
            {
                if (SearchCriteria.ExactMatch)
                    criteria.Add(Expression.Eq("RRR_CountryDescription", SearchCriteria.Country));
                else
                    criteria.Add(Expression.Like("RRR_CountryDescription", SearchCriteria.Country, MatchMode.Start));
            }

            // province
            if (SearchCriteria.Province != null && SearchCriteria.Province.Length > 0)
            {
                if (SearchCriteria.ExactMatch)
                    criteria.Add(Expression.Eq("RRR_ProvinceDescription", SearchCriteria.Province));
                else
                    criteria.Add(Expression.Like("RRR_ProvinceDescription", SearchCriteria.Province, MatchMode.Start));
            }

            // street name
            if (SearchCriteria.ExactMatch)
            {
                if (SearchCriteria.StreetName == null)
                    criteria.Add(Expression.IsNull("StreetName"));
                else
                    criteria.Add(Expression.Eq("StreetName", SearchCriteria.StreetName));
            }
            else if (SearchCriteria.StreetName != null && SearchCriteria.StreetName.Length > 0)
                criteria.Add(Expression.Like("StreetName", SearchCriteria.StreetName, MatchMode.Start));

            // street number
            if (SearchCriteria.ExactMatch)
            {
                if (SearchCriteria.StreetNumber == null)
                    criteria.Add(Expression.IsNull("StreetNumber"));
                else
                    criteria.Add(Expression.Eq("StreetNumber", SearchCriteria.StreetNumber));
            }
            else if (SearchCriteria.StreetNumber != null && SearchCriteria.StreetNumber.Length > 0)
                criteria.Add(Expression.Like("StreetNumber", SearchCriteria.StreetNumber, MatchMode.Start));

            // suburb
            if (SearchCriteria.SuburbKey.HasValue)
                criteria.CreateCriteria("Suburb").Add(Expression.Eq("Key", SearchCriteria.SuburbKey.Value));

            // unit number
            if (SearchCriteria.ExactMatch)
            {
                if (SearchCriteria.UnitNumber == null)
                    criteria.Add(Expression.IsNull("UnitNumber"));
                else
                    criteria.Add(Expression.Eq("UnitNumber", SearchCriteria.UnitNumber));
            }
            else if (SearchCriteria.UnitNumber != null && SearchCriteria.UnitNumber.Length > 0)
                criteria.Add(Expression.Like("UnitNumber", SearchCriteria.UnitNumber, MatchMode.Start));

            // set max rowcounts
            criteria.SetMaxResults(MaxRowCount);
            criteria.AddOrder(new Order("Key", true));

            return criteria.List<AddressStreet_DAO>();
        }


    }

}
