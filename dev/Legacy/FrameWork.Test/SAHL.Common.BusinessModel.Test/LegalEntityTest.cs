using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Data;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class LegalEntityTest : TestBase
    {
        [Test]
        public void GetInsurableInterest()
        {
            using (new SessionScope())
            {
                IDomainMessageCollection messages = new DomainMessageCollection();

                ParameterCollection parameters = new ParameterCollection();
                IDbConnection con = Helper.GetSQLDBConnection();
                DataTable DT = new DataTable();
                string query = "Select top 1 a.AccountKey, lii.LegalEntityKey from [2AM].[dbo].[Account] a (nolock) "
                    + "join [2AM].[dbo].[FinancialService] fs (nolock) on fs.AccountKey = a.AccountKey "
                    + "join [2AM].[dbo].[LifeInsurableInterest] lii (nolock) on lii.AccountKey = fs.AccountKey "
                    + "join [2AM].[dbo].[LegalEntity] le (nolock) on le.LegalEntityKey = lii.LegalEntityKey "
                    + "where fs.AccountStatusKey = 1 and a.AccountStatusKey = 1";
                Helper.FillFromQuery(DT, query, con, parameters);

                Assert.That(DT.Rows.Count == 1, "No Data found matching the query");
                DataRow row = DT.Rows[0];
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                ILegalEntity le = BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(LegalEntity_DAO.Find((int)row[1]));
                ILifeInsurableInterest lii = le.GetInsurableInterest((int)row[0]);

                Assert.That(lii != null);
                Assert.That(lii.Account.Key == (int)row[0]);
                Assert.That(lii.LegalEntity.Key == (int)row[1]);
            }
        }

        [Test]
        public void GetRole()
        {
            using (new SessionScope())
            {
                int _legalEntityKey = 55836;
                int _accountKey = 1399049;

                IDomainMessageCollection messages = new DomainMessageCollection();

                ParameterCollection parameters = new ParameterCollection();
                IDbConnection con = Helper.GetSQLDBConnection();
                DataTable DT = new DataTable();
                string query = string.Format("Select * from [2AM].[dbo].[Role] r (nolock) "
                    + "join [2AM].[dbo].[Account] a (nolock) on a.AccountKey = r.AccountKey "
                    + "where r.LegalEntityKey = {0}", _legalEntityKey);
                Helper.FillFromQuery(DT, query, con, parameters);

                Assert.IsTrue(DT.Rows.Count > 0, "No Data found matching the query");
                DataRow row = DT.Rows[0];

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                ILegalEntity le = BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(LegalEntity_DAO.Find(_legalEntityKey));
                IRole _role = le.GetRole(_accountKey);

                Assert.That(_role != null);
                Assert.That(_role.Account.Key == _accountKey);
                Assert.That(_role.LegalEntity.Key == _legalEntityKey);
            }
        }

        [Test]
        public void ClosedCorporation()
        {
            using (new SessionScope())
            {
                int key = Convert.ToInt32(base.GetPrimaryKey("LegalEntity", "LegalEntityKey", "LegalEntityTypeKey = 3"));

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                LegalEntity_DAO a = LegalEntity_DAO.Find(key);
                ILegalEntity le = BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(a);
            }
        }

        /// <summary>
        /// Tests the GetApplicationRolesByRoleTypeGroups method.
        /// </summary>
        [Test]
        public void GetApplicationRolesByRoleTypeGroups()
        {
            using (new SessionScope())
            {
                // get a legal entity that has more than one offer role type group
                string sql = @"select top 1 ofr.LegalEntityKey
                from OfferRoleType ort (nolock)
                inner join OfferRole ofr (nolock) on ofr.OfferRoleTypeKey = ort.OfferRoleTypeKey
                group by ofr.LegalEntityKey
                having count(distinct OfferRoleTypeGroupKey) > 1";
                DataTable dt = GetQueryResults(sql);
                if (dt.Rows.Count == 0)
                    Assert.Ignore("No data");
                IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                LegalEntity_DAO leDao = LegalEntity_DAO.Find(Convert.ToInt32(dt.Rows[0]["LegalEntityKey"]));
                ILegalEntity legalEntity = bmtm.GetMappedType<ILegalEntity, LegalEntity_DAO>(leDao);
                dt.Dispose();

                List<OfferRoleTypeGroups> offerRoleTypeGroupKeys = new List<OfferRoleTypeGroups>();
                int totalRoleCount = 0;

                // get the list of offer role type groups for the legal entity
                sql = String.Format(@"select count(OfferRoleKey) as RoleCount, OfferRoleTypeGroupKey
                from OfferRole ofr (nolock)
                inner join OfferRoleType ort (nolock) on ofr.OfferRoleTypeKey = ort.OfferRoleTypeKey
                where ofr.LegalEntityKey = {0}
                group by ort.OfferRoleTypeGroupKey", legalEntity.Key);
                dt = GetQueryResults(sql);

                // run through each of the groups individually and validate the count
                foreach (DataRow row in dt.Rows)
                {
                    int roleCount = Convert.ToInt32(row["RoleCount"]);
                    int ortgKey = Convert.ToInt32(row["OfferRoleTypeGroupKey"]);

                    // make the call to the method and ensure they come back with the same count
                    IReadOnlyEventList<IApplicationRole> roles = legalEntity.GetApplicationRolesByRoleTypeGroups((OfferRoleTypeGroups)ortgKey);
                    Assert.AreEqual(roleCount, roles.Count, "GetApplicationRolesByRoleTypeGroups failed for legal entity {0}", legalEntity.Key);

                    // add to the list, and add to the toal for the final check
                    offerRoleTypeGroupKeys.Add((OfferRoleTypeGroups)ortgKey);
                    totalRoleCount += roleCount;
                }
                dt.Dispose();

                // now do a final comparison with all the role keys
                IReadOnlyEventList<IApplicationRole> allRoles = legalEntity.GetApplicationRolesByRoleTypeGroups(offerRoleTypeGroupKeys.ToArray());
                Assert.AreEqual(totalRoleCount, allRoles.Count, "GetApplicationRolesByRoleTypeGroups failed for legal entity {0}", legalEntity.Key);
            }
        }

        /// <summary>
        /// Tests the GetApplicationRolesByApplicationKey method.
        /// </summary>
        [Test]
        public void GetApplicationRolesByApplicationKey()
        {
            string sql = @"select top 1 count(*) as NumRoles, OfferKey, LegalEntityKey
                from OfferRole ofr (nolock)
                group by OfferKey, LegalEntityKey
                having count(*) > 1";

            DataTable dt = GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data");
            int numRoles = Convert.ToInt32(dt.Rows[0]["NumRoles"]);
            int offerKey = Convert.ToInt32(dt.Rows[0]["OfferKey"]);
            int leKey = Convert.ToInt32(dt.Rows[0]["LegalEntityKey"]);
            dt.Dispose();

            using (new SessionScope())
            {
                LegalEntity_DAO leDao = LegalEntity_DAO.Find(leKey);
                IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                ILegalEntity le = bmtm.GetMappedType<ILegalEntity, LegalEntity_DAO>(leDao);

                IReadOnlyEventList<IApplicationRole> roles = le.GetApplicationRolesByApplicationKey(offerKey);
                Assert.AreEqual(numRoles, roles.Count, "GetApplicationRolesByApplicationKey failed for legal entity {0}", le.Key);
            }
        }

        /// <summary>
        /// Tests the GetApplicationRolesByRoleTypes method.
        /// </summary>
        [Test]
        public void GetApplicationRolesByRoleTypes()
        {
            using (new SessionScope())
            {
                // get a legal entity that has more than one offer role type group
                string sql = @"select top 1 ofr.LegalEntityKey
                    from OfferRole ofr (nolock)
                    group by ofr.LegalEntityKey
                    having count(distinct OfferRoleTypeKey) > 1";
                DataTable dt = GetQueryResults(sql);
                if (dt.Rows.Count == 0)
                    Assert.Ignore("No data");
                IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                LegalEntity_DAO leDao = LegalEntity_DAO.Find(Convert.ToInt32(dt.Rows[0]["LegalEntityKey"]));
                ILegalEntity legalEntity = bmtm.GetMappedType<ILegalEntity, LegalEntity_DAO>(leDao);
                dt.Dispose();

                List<OfferRoleTypes> offerRoleTypeKeys = new List<OfferRoleTypes>();
                int totalRoleCount = 0;

                // get the list of offer role type groups for the legal entity
                sql = String.Format(@"select count(OfferRoleKey) as RoleCount, OfferRoleTypeKey
                    from OfferRole ofr (nolock)
                    where ofr.LegalEntityKey = {0}
                    group by ofr.OfferRoleTypeKey", legalEntity.Key);
                dt = GetQueryResults(sql);

                // run through each of the groups individually and validate the count
                foreach (DataRow row in dt.Rows)
                {
                    int roleCount = Convert.ToInt32(row["RoleCount"]);
                    int ortKey = Convert.ToInt32(row["OfferRoleTypeKey"]);

                    // make the call to the method and ensure they come back with the same count
                    IReadOnlyEventList<IApplicationRole> roles = legalEntity.GetApplicationRolesByRoleTypes((OfferRoleTypes)ortKey);
                    Assert.AreEqual(roleCount, roles.Count, "GetApplicationRolesByRoleTypes failed for legal entity {0}", legalEntity.Key);

                    // add to the list, and add to the toal for the final check
                    offerRoleTypeKeys.Add((OfferRoleTypes)ortKey);
                    totalRoleCount += roleCount;
                }
                dt.Dispose();

                // now do a final comparison with all the role keys
                IReadOnlyEventList<IApplicationRole> allRoles = legalEntity.GetApplicationRolesByRoleTypes(offerRoleTypeKeys.ToArray());
                Assert.AreEqual(totalRoleCount, allRoles.Count, "GetApplicationRolesByRoleTypes failed for legal entity {0}", legalEntity.Key);
            }
        }

        /// <summary>
        /// Tests the GetApplicationRoleClient method.
        /// </summary>
        [Test]
        public void GetApplicationRoleClient()
        {
            // find a legal entity role that occurs more than once for a single offer and legal entity, and also has
            // a role in the "Client" group
            string sql = String.Format(@"select top 1 LegalEntityKey, OfferKey
                from OfferRole ofr (nolock)
                inner join OfferRoleType ort (nolock) on ofr.OfferRoleTypeKey = ort.OfferRoleTypeKey
                where ort.OfferRoleTypeGroupKey = {0}
                group by OfferKey, LegalEntityKey
                having count(*) > 1", (int)OfferRoleTypeGroups.Client);

            DataTable dt = GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data");
            int leKey = Convert.ToInt32(dt.Rows[0]["LegalEntityKey"]);
            int offerKey = Convert.ToInt32(dt.Rows[0]["OfferKey"]);
            dt.Dispose();

            using (new SessionScope())
            {
                LegalEntity_DAO leDao = LegalEntity_DAO.Find(leKey);
                IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                ILegalEntity le = bmtm.GetMappedType<ILegalEntity, LegalEntity_DAO>(leDao);

                IApplicationRole role = le.GetApplicationRoleClient(offerKey);
                Assert.IsNotNull(role);
                Assert.AreEqual(role.ApplicationRoleType.ApplicationRoleTypeGroup.Key, (int)OfferRoleTypeGroups.Client);
            }
        }

        /// <summary>
        /// Tests the GetExternalRoleClient method.
        /// </summary>
        [Test]
        public void GetExternalRoleClient()
        {
            string sql = String.Format(@"select top 1 er.GenericKey, er.LegalEntityKey
                from [2am].dbo.ExternalRole er (nolock)
                inner join [2am].dbo.ExternalRoleType ert (nolock) on ert.ExternalRoleTypeKey = er.ExternalRoleTypeKey
                    and ert.ExternalRoleTypeGroupKey = {0}
                where er.GeneralStatusKey = 1
                and er.GenericKeyTypeKey = {1} ", (int)ExternalRoleTypeGroups.Client, (int)GenericKeyTypes.Offer);

            DataTable dt = GetQueryResults(sql);

            if (dt.Rows.Count == 0)
                Assert.Ignore("No data");

            int leKey = Convert.ToInt32(dt.Rows[0]["LegalEntityKey"]);
            int offerKey = Convert.ToInt32(dt.Rows[0]["GenericKey"]);
            dt.Dispose();

            using (new SessionScope(FlushAction.Never))
            {
                LegalEntity_DAO leDao = LegalEntity_DAO.Find(leKey);
                IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                ILegalEntity le = bmtm.GetMappedType<ILegalEntity, LegalEntity_DAO>(leDao);

                IExternalRole role = le.GetActiveClientExternalRoleForOffer(offerKey);

                Assert.IsNotNull(role);
                Assert.AreEqual(role.ExternalRoleType.ExternalRoleTypeGroup.Key, (int)ExternalRoleTypeGroups.Client);
            }
        }
    }
}