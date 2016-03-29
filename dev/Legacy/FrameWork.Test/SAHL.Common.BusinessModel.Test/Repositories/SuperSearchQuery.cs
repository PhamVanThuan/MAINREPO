using NUnit.Framework;
using SAHL.Test;
using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class SuperSearchQuery : TestBase
    {
        private int AliasCnt = 0;

        private string FTJoin = "inner join\n " +
        "Containstable(LegalEntity, (CellPhoneNumber, EmailAddress, FaxNumber, FirstNames, HomePhoneNumber, IDNumber, Initials, PassportNumber, PreferredName, RegisteredName, RegistrationNumber, Surname, TradingName, WorkPhoneNumber), '{0}') {1}\n" +
        "on\n" +
        "le.legalentitykey = {1}.[KEY]\n";

        private string ACCJoin = "inner join\n " +
        "role r\n" +
        "on\n " +
        "r.legalentitykey = LE.legalentitykey\n";

        private string ACCWhere = "where r.accountkey in( {0} )\n";
        private string Order = "order by TheRank desc";

        [Test]
        public void BuildQuery()
        {
            StringCollection Ranks = new StringCollection();
            string query = "";
            string joins = "";
            string nandstart = "";
            string _searchString = "mommy daddy koos january and \"hello and not goodbye\" OR today and not 123456";
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
                            // do the and prefix
                            string Rank = "k" + Ranks.Count.ToString();
                            Ranks.Add(Rank);
                            joins += String.Format(FTJoin, nandstart, Rank);
                        }
                        break;

                    case "notand":
                        {
                            // do the and prefix
                            string Rank = "k" + Ranks.Count.ToString();
                            Ranks.Add(Rank);
                            joins += String.Format(FTJoin, nandstart, Rank);
                        }
                        break;

                    case "number":

                        break;
                }
            }

            // build the or join
            if (Or.EndsWith("or "))
            {
                Or = Or.Substring(0, Or.Length - 3);
            }
            if (AccountSearch.EndsWith(", "))
            {
                AccountSearch = AccountSearch.Substring(0, AccountSearch.Length - 2);
            }
            string Ranking = "k" + Ranks.Count.ToString();
            Ranks.Add(Ranking);
            joins += String.Format(FTJoin, Or, Ranking);

            string RankString = "(";
            for (int i = 0; i < Ranks.Count; i++)
            {
                RankString += (Ranks[i] + ".rank");
                if (i < Ranks.Count - 1)
                    RankString += " + ";
            }
            RankString += ")";

            query = "select " + RankString + " as TheRank, le.* from LegalEntity LE\n";
            if (AccountSearch.Length > 0)
                query += ACCJoin;
            query += joins;
            if (AccountSearch.Length > 0)
                query += string.Format(ACCWhere, AccountSearch);
            query += Order;
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
    }
}