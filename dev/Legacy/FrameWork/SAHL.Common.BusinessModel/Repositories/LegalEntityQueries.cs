using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.DAO;
using NHibernate;
using NHibernate.Expression;
using Castle.ActiveRecord.Framework;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Collections;
using SAHL.Common;
using SAHL.Common.Security;

namespace SAHL.Common.BusinessModel.Repositories
{
    public class NaturalPersonSearchQuery : ActiveRecordBaseQuery
    {
        IClientSearchCriteria _searchCriteria;
        int _maxRowCount;
        public NaturalPersonSearchQuery(IClientSearchCriteria SearchCriteria, int MaxRowCount)
            : base(typeof(LegalEntityNaturalPerson_DAO))
        {
            _searchCriteria = SearchCriteria;
            _maxRowCount = MaxRowCount;
        }

        protected override NHibernate.IQuery CreateQuery(NHibernate.ISession session)
        {
            string HQL = "from LegalEntityNaturalPerson_DAO LE";
            IQuery q = session.CreateQuery(HQL);

            return q;
        }

        protected override object InternalExecute(ISession session)
        {
            // build up our criteria
            ICriteria Criteria = session.CreateCriteria(typeof(LegalEntityNaturalPerson_DAO));

            // firstname
            if (_searchCriteria.FirstNames != null && _searchCriteria.FirstNames.Length > 0)
                Criteria.Add(Expression.Like("FirstNames", _searchCriteria.FirstNames, MatchMode.End));
            // surname
            if (_searchCriteria.Surname != null && _searchCriteria.Surname.Length > 0)
                Criteria.Add(Expression.Like("Surname", _searchCriteria.Surname, MatchMode.End));
            // preferred name
            if (_searchCriteria.PreferredName != null && _searchCriteria.PreferredName.Length > 0)
                Criteria.Add(Expression.Like("PreferredName", _searchCriteria.PreferredName, MatchMode.End));
            // IDNumber
            if (_searchCriteria.IDNumber != null && _searchCriteria.IDNumber.Length > 0)
                Criteria.Add(Expression.Like("IDNumber", _searchCriteria.IDNumber, MatchMode.End));
            // PassportNumber
            if (_searchCriteria.PassportNumber != null && _searchCriteria.PassportNumber.Length > 0)
                Criteria.Add(Expression.Like("PassportNumber", _searchCriteria.PassportNumber, MatchMode.End));

            Criteria.Add(Expression.IsNotNull("Salutation"));
            // Account Number
            if (_searchCriteria.AccountNumber != null)
            {
                ICriteria RC = Criteria.CreateCriteria("Roles");
                Criteria.SetFetchMode("Roles", FetchMode.Eager);
                ICriteria AC = RC.CreateCriteria("Account");
                RC.SetFetchMode("Account", FetchMode.Eager);
                AC.Add(Expression.Like("Key", _searchCriteria.AccountNumber));
            }

            //// distinct


            // set max rowcounts
            Criteria.SetMaxResults(_maxRowCount);

            // set order bys
            Criteria.AddOrder(new Order("Surname", true));
            Criteria.AddOrder(new Order("FirstNames", true));

            return Criteria.List<LegalEntityNaturalPerson_DAO>();
        }
    }

    public class CompanySearchQuery : ActiveRecordBaseQuery
    {
        IClientSearchCriteria _searchCriteria;
        int _maxRowCount;
        public CompanySearchQuery(IClientSearchCriteria SearchCriteria, int MaxRowCount)
            : base(typeof(LegalEntityNaturalPerson_DAO))
        {
            _searchCriteria = SearchCriteria;
            _maxRowCount = MaxRowCount;
        }

        protected override NHibernate.IQuery CreateQuery(NHibernate.ISession session)
        {
            string HQL = "from LegalEntityCompany_DAO LE";
            IQuery q = session.CreateQuery(HQL);
            return q;
        }

        protected override object InternalExecute(ISession session)
        {
            // build up our criteria
            ICriteria Criteria = session.CreateCriteria(typeof(LegalEntityCompany_DAO));

            // CompanyRegisteredName
            if (_searchCriteria.CompanyRegisteredName != null && _searchCriteria.CompanyRegisteredName.Length > 0)
                Criteria.Add(Expression.Like("RegisteredName", _searchCriteria.CompanyRegisteredName, MatchMode.End));
            // CompanyTradingName
            if (_searchCriteria.CompanyTradingName != null && _searchCriteria.CompanyTradingName.Length > 0)
                Criteria.Add(Expression.Like("TradingName", _searchCriteria.CompanyTradingName, MatchMode.End));
            // CompanyNumber
            if (_searchCriteria.CompanyNumber != null && _searchCriteria.CompanyNumber.Length > 0)
                Criteria.Add(Expression.Like("RegistrationNumber", _searchCriteria.CompanyNumber, MatchMode.End));
       
            // Account Number
            if (_searchCriteria.AccountNumber != null)
            {
                ICriteria RC = Criteria.CreateCriteria("Roles");
                Criteria.SetFetchMode("Roles", FetchMode.Eager);
                ICriteria AC = RC.CreateCriteria("Account");
                RC.SetFetchMode("Account", FetchMode.Eager);
                AC.Add(Expression.Like("Key", _searchCriteria.AccountNumber));
            }

            //// distinct

            // set max rowcounts
            Criteria.SetMaxResults(_maxRowCount);

            // set order bys
            Criteria.AddOrder(new Order("RegisteredName", true));
            Criteria.AddOrder(new Order("TradingName", true));

            return Criteria.List<LegalEntityCompany_DAO>();
        }
    }

    public class LegalEntitySuperSearchQuery2 : ActiveRecordBaseQuery
    {
        int _maxRowCount;
        IClientSuperSearchCriteria _criteria;

        public LegalEntitySuperSearchQuery2(IClientSuperSearchCriteria Criteria, int MaxRowCount)
            : base(typeof(LegalEntity_DAO))
        {
            _maxRowCount = MaxRowCount;
            _criteria = Criteria;
        }

        protected override IQuery CreateQuery(ISession session)
        {
            string HQL = "from LegalEntityCompany_DAO LE";
            IQuery q = session.CreateQuery(HQL);
            return q;
        }

        protected override object InternalExecute(ISession session)
        {
            string _searchString = _criteria.SearchString;

            // first build any and/or matches
            Regex Reg = new Regex(@"\b(and|or)\b");
            MatchCollection Matches = Reg.Matches(_searchString);
            for (int i = 0; i < Matches.Count; i++)
            {
                //Matches[i].
            }

            Regex NumReg = new Regex(@"\b\d+\b");
            MatchCollection NumMatches = NumReg.Matches(_searchString);


            // build up our criteria
            ICriteria Criteria = session.CreateCriteria(typeof(LegalEntity_DAO));

            // firstname
            ICriterion FN = Expression.Like("FirstNames", _searchString, MatchMode.Anywhere);
            // surname
            ICriterion SN = Expression.Or(FN, Expression.And(Expression.IsNotNull("Surname"), Expression.Like("Surname", "prinsloo", MatchMode.Anywhere)));
            // preferred name
            ICriterion PN = Expression.Or(SN, Expression.Like("PreferredName", _searchString, MatchMode.Anywhere));
            // IDNumber
            ICriterion ID = Expression.Or(PN, Expression.Like("IDNumber", _searchString, MatchMode.Anywhere));
            // PassportNumber
            ICriterion PASS = Expression.Or(ID, Expression.Like("PassportNumber", _searchString, MatchMode.Anywhere));
            // CompanyRegisteredName
            ICriterion CRN = Expression.Or(PASS, Expression.Like("RegisteredName", _searchString, MatchMode.Anywhere));
            // CompanyTradingName
            ICriterion CTN = Expression.Or(CRN, Expression.Like("TradingName", _searchString, MatchMode.Anywhere));
            // CompanyNumber
            ICriterion RN = Expression.Or(CTN, Expression.Like("RegistrationNumber", _searchString, MatchMode.Anywhere));
            
            Criteria.Add(RN);
            
            // Account Number

            if(NumMatches.Count>0)
            {
                ICriteria RC = Criteria.CreateCriteria("Roles");
                Criteria.SetFetchMode("Roles", FetchMode.Eager);
                ICriteria AC = RC.CreateCriteria("Account");
                RC.SetFetchMode("Account", FetchMode.Eager);

                for(int i=0;i<NumMatches.Count;i++)
                {
                    AC.Add(Expression.Like("Key", Convert.ToInt32(NumMatches[i].ToString())));
                }
            }


            //// distinct

            // set max rowcounts
            Criteria.SetMaxResults(_maxRowCount);

            // set order bys
            Criteria.AddOrder(new Order("Surname", true));
            Criteria.AddOrder(new Order("RegisteredName", true));
            return Criteria.List<LegalEntity_DAO>();
        }
    }

    public class LegalEntitySuperSearchQuery : ActiveRecordBaseQuery
    {
        int _maxRowCount;
        IClientSuperSearchCriteria _criteria;

        public LegalEntitySuperSearchQuery(IClientSuperSearchCriteria Criteria, int MaxRowCount)
            : base(typeof(LegalEntity_DAO))
        {
            _maxRowCount = MaxRowCount;
            _criteria = Criteria;
        }

        protected override IQuery CreateQuery(ISession session)
        {
            string FTJoin = "inner join\n " +
            "role r\n" +
            "on\n " +
            "r.legalentitykey = LE.legalentitykey\n" +
            "inner join account a on a.accountkey = r.accountkey\n " +
            "inner join\n " +
            "Containstable(LegalEntity, (FirstNames, Surname, PreferredName, RegisteredName, TradingName, CellPhoneNumber, EmailAddress, FaxNumber, HomePhoneNumber, IDNumber, Initials, PassportNumber, RegistrationNumber, WorkPhoneNumber), '{0}') {1}\n" +
            "on\n" +
            "le.legalentitykey = {1}.[KEY]\n";
            
            string RoleWhere = " r.accountkey in( {0} )\n";
            string UserOrgWhere = " a.RRR_OriginationSourceKey in ({0})\n";
            string Order = "order by TheRank desc";

            StringCollection Ranks = new StringCollection();
            string query = "";
            string joins = "";
            string nandstart = "";
            string _searchString = _criteria.SearchString;
            string AccountSearch = "";
            string Or = "";

            // first build any and/or matches
            Regex Reg = new Regex("(?<QUOTES>\".*?\")|(?<NOTAND>\\band\\b\\s\\bnot\\b)|(?<AND>\\band\\b)|(?<OR>\\bor\\b)|(?<NUMBER>\\d+)|(?<WORD>\\b\\w+\\b)|\\w+");

            Regex RegNum = new Regex("\\d+");
            MatchCollection Matches = Reg.Matches(_searchString);
            string[] names = Reg.GetGroupNames();
            for (int i = 0; i < Matches.Count; i++)
            {
                string M = Matches[i].ToString().ToLower();
                string MatchSwitch = "";
                string MatchGroup = GetMatchGroup(names, Matches[i].Groups);
                if (M.StartsWith("\"") && M.EndsWith("\""))
                    MatchSwitch = "word";
                else
                    if ((M != "or") && (M != "and") && (M != "and not"))
                        MatchSwitch = "word";
                    else
                        MatchSwitch = M;


                switch (MatchSwitch)
                {
                    case "word":
                        // are we following  on from a and or nand
                        if (nandstart != "")
                        {
                            // add the and postfix
                            string Rank = "k" + Ranks.Count.ToString();
                            Ranks.Add(Rank);
                            joins += String.Format(FTJoin, M, Rank);
                            nandstart = "";
                        }
                        else
                        {
                            // if the next match is an and or a notand add to the nandstart
                            if (i < Matches.Count - 1)
                            {
                                string NextMatch = Matches[i + 1].ToString();
                                if (NextMatch == "and" || NextMatch == "notand")
                                {
                                    nandstart = M;
                                }
                                else
                                    Or += M + " or ";
                            }
                            else
                                Or += M + " or ";

                            // if the Matchgroup is actually a number add it to the account string
                            if (MatchGroup == "NUMBER")
                            {
                                AccountSearch += M;
                                if (i < Matches.Count - 1)
                                    AccountSearch += ", ";
                            }
                        }
                        break;
                    case "or":

                        break;
                    case "and":
                        {
                            if (i < Matches.Count - 1)
                            {
                                string NextMatchGroup = GetMatchGroup(names, Matches[i+1].Groups);
                                if (NextMatchGroup != "NUMBER")
                                {
                                    // do the and prefix
                                    string Rank = "k" + Ranks.Count.ToString();
                                    Ranks.Add(Rank);
                                    joins += String.Format(FTJoin, nandstart, Rank);
                                }
                            }
                        }
                        break;
                    case "notand":
                        {
                            if (i < Matches.Count - 1)
                            {
                                string NextMatchGroup = GetMatchGroup(names, Matches[i + 1].Groups);
                                if (NextMatchGroup != "NUMBER")
                                {
                                    // do the and prefix
                                    string Rank = "k" + Ranks.Count.ToString();
                                    Ranks.Add(Rank);
                                    joins += String.Format(FTJoin, nandstart, Rank);
                                }
                            }
                        }
                        break;
                    case "number":

                        break;
                }
            }

            // build the or join
            if (Or.Length > 0 && Or.EndsWith("or "))
            {
                Or = Or.Substring(0, Or.Length - 4);

                string Ranking = "k" + Ranks.Count.ToString();
                Ranks.Add(Ranking);
                joins += String.Format(FTJoin, Or, Ranking);
            }

            if (AccountSearch.EndsWith(", "))
            {
                AccountSearch = AccountSearch.Substring(0, AccountSearch.Length - 2);
            }

            string RankString = "(";
            for (int i = 0; i < Ranks.Count; i++)
            {
                RankString += (Ranks[i] + ".rank");
                if (i < Ranks.Count - 1)
                    RankString += " + ";
            }
            RankString += ")";

            query = "select " + RankString + " as TheRank, le.* from LegalEntity LE\n";
            query += joins;

            // get the principals origination sources to we can filter the results
            SAHLPrincipalCache SPC = SAHLPrincipal.GetSAHLPrincipalCache(); //SAHLPrincipalCache.GetPrincipalCache(_criteria.Principal);
            string UserOrgsOrigs = "";
            for(int i=0;i<SPC.UserOriginationSources.Count;i++)
            {
                UserOrgsOrigs += SPC.UserOriginationSources[i].OriginationSource.Key.ToString();
                if(i<SPC.UserOriginationSources.Count -1)
                    UserOrgsOrigs += ", ";
            }
            query += string.Format(" where " + UserOrgWhere, UserOrgsOrigs);

            // add the account filter
            if (AccountSearch.Length > 0)
                query += string.Format(" and " + RoleWhere, AccountSearch);

            // add an order clause for the ranking
            query += Order;


            IQuery q = session.CreateSQLQuery(query, "LE", typeof(LegalEntity_DAO));
            q.SetMaxResults(this._maxRowCount);
            return q;
        }

        private string GetMatchGroup(string[] Names, GroupCollection groupCollection)
        {
            for (int i = 0; i < groupCollection.Count; i++)
            {
                if (Names[i] != "0")
                {
                    if (groupCollection[i].Success)
                        return Names[i];
                }
            }
            return "";
        }

        protected override object InternalExecute(ISession session)
        {
            IQuery q = CreateQuery(session);
            return q.List < LegalEntity_DAO>();
//            return new List<LegalEntity_DAO>(list);
        }
    }
}
