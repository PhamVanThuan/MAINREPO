using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class OrganisationStructureRepositoryTest : TestBase
    {
        private IOrganisationStructureRepository _orgRep = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

        [Test]
        public void GetRoleTypesForADUser()
        {
            string sql = @"select top 1 ADUserName from ADUser a
				inner join UserOrganisationStructure uos on a.ADUserKey = uos.ADUserKey
				inner join OrganisationStructure os on os.OrganisationStructureKey = uos.OrganisationStructureKey
				inner join OfferRoleTypeOrganisationStructureMapping ortosm on ortosm.OrganisationStructureKey = os.OrganisationStructureKey";

            IDataReader reader = DBHelper.ExecuteReader(sql);
            if (!reader.Read())
                Assert.Ignore("No data");
            string adUserName = reader.GetString(0);
            reader.Dispose();

            using (new SessionScope())
            {
                Dictionary<string, string> roleTypes = _orgRep.GetRoleTypesForADUser(adUserName);
                Assert.Greater(roleTypes.Count, 0);
            }
        }

        [Test]
        public void GetReassignUserApplicationRoleList()
        {
            int iApplicationKey = 0;
            int iApplicationRoleTypeKey = 0;

            string sql = @"select top 1 o.OfferKey,ofr.offerRoleTypeKey
				  from [2am].[dbo].[Offer] o (nolock)
				inner join [2am].[dbo].[OfferRole] ofr (nolock) on ofr.OfferKey = o.OfferKey
				inner join [2am].[dbo].[LegalEntity] le (nolock) on ofr.LegalEntityKey = le.LegalEntityKey
				inner join [2am].[dbo].[GeneralStatus] gs (nolock) on ofr.GeneralStatusKey = gs.GeneralStatusKey
				inner join [2am].[dbo].adUser adSub (nolock) on ofr.LegalEntityKey = adSub.LegalEntityKey
				inner join [2am].[dbo].UserOrganisationStructure uosSub (nolock) on uosSub.ADUserKey = adSub.ADUserKey
				inner join [2am].[dbo].OrganisationStructure osSub (nolock) on osSub.OrganisationStructureKey = uosSub.OrganisationStructureKey
				inner join [2am].[dbo].OfferRoleTypeOrganisationStructureMapping orstm (nolock) on osSub.OrganisationStructureKey = orstm.OrganisationStructureKey and orstm.OfferRoleTypeKey = ofr.OfferRoleTypeKey
				inner join [2am].[dbo].OrganisationStructure osBranch (nolock) on osSub.ParentKey = osBranch.OrganisationStructureKey";
            using (DBHelper dbHelper = new DBHelper(Databases.TwoAM))
            {
                using (IDbCommand cmd = dbHelper.CreateCommand(sql))
                {
                    cmd.CommandTimeout = 40;

                    using (IDataReader reader = dbHelper.ExecuteReader(cmd))
                    {
                        reader.Read();
                        iApplicationKey = reader.GetInt32(0);
                        iApplicationRoleTypeKey = reader.GetInt32(1);
                    }
                }
            }

            if (iApplicationKey > 0 && iApplicationRoleTypeKey > 0)
            {
                DataTable dtRoleList = _orgRep.GetReassignUserApplicationRoleList(iApplicationKey, iApplicationRoleTypeKey);
            }
        }

        [Test]
        public void GetOrganisationStructureConfirmationList()
        {
            string sql = @"select top 1 organisationStructureKey
							from [2am].[dbo].[vOrganisationStructure] (nolock)";

            using (new SessionScope())
            {
                List<int> SelectedNoteLst = new List<int>();
                ParameterCollection parameters = new ParameterCollection();
                Object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);

                if (o != null)
                {
                    SelectedNoteLst.Add(Convert.ToInt32(o));
                }

                DataTable dtConfirmationList = _orgRep.GetOrganisationStructureConfirmationList(SelectedNoteLst);
            }
        }

        [Test]
        public void GetUserOrganisationStructureHistory()
        {
            string sql = @"select top 1 vuosh.ADUserKey
							from [2am].[dbo].vUserOrganisationStructureHistory vuosh
							inner join [2am].[dbo].[ADUser] ad (nolock)
								on vuosh.ADUserKey = ad.ADUserKey
							inner join [2am].[dbo].[vOrganisationStructure] vos (nolock)
								on vuosh.OrganisationStructureKey = vos.OrganisationStructureKey";

            using (new SessionScope())
            {
                int ADUserKey = 0;
                ParameterCollection parameters = new ParameterCollection();
                Object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);

                if (o != null)
                {
                    ADUserKey = Convert.ToInt32(o);
                }

                DataTable dtOrgStructure = _orgRep.GetUserOrganisationStructureHistory(ADUserKey);
            }
        }

        [Test]
        public void SaveAdUser()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                IEventList<IADUser> aduList = _orgRep.GetAdUsersByPartialName("a", 3);

                foreach (IADUser adu in aduList)
                {
                    _orgRep.SaveAdUser(adu);
                }

                Assert.IsNotNull(aduList);
            }
        }

        [Test]
        public void ReassignUserTest()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                string sql = @"with Offer_CTE (InstanceID,OfferKey)
                                as
                                (
	                                SELECT TOP 1
		                                xac.InstanceID,xac.ApplicationKey
	                                FROM
		                                x2.X2DATA.Application_Capture xac (nolock)
	                                JOIN
		                                x2.x2.instance xi (nolock) on xi.id = xac.instanceID
	                                WHERE
		                                xac.isEA <> 1 AND xac.isEstateAgentApplication <> 1 AND xac.CaseOwnerName is not null
	                                ORDER BY
		                                xac.InstanceID
                                )
                                SELECT TOP 1
	                                cte.InstanceID,ofr.offerRoleKey,cte.Offerkey,ad.aduserkey, ad.adusername
                                FROM
	                                Offer_CTE CTE
                                JOIN
	                                [2am].dbo.OfferRole ofr (nolock) on ofr.OfferKey = cte.OfferKey
                                JOIN
	                                [2am].dbo.OfferRoleType ort (nolock) on ort.OfferRoleTypeKey = ofr.OfferRoleTypeKey
                                JOIN
	                                [2am].dbo.ADUser ad (nolock) on ad.legalEntityKey = ofr.legalEntityKey
                                JOIN
	                                [2am].dbo.UserOrganisationStructure uos (nolock) on ad.aduserKey = uos.aduserKey
                                JOIN
	                                [2am].dbo.OfferRoleTypeOrganisationStructureMapping ortosm (nolock) on ortosm.OrganisationStructureKey = uos.OrganisationStructureKey
                                WHERE
	                                ad.generalStatusKey = 1
                                AND
	                                ofr.generalStatusKey = 1
                                AND
	                                ort.OfferRoleTypeGroupKey  = 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                long instanceID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                int appRoleKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                int applicationkey = Convert.ToInt32(ds.Tables[0].Rows[0][2]);
                int aduserkey = Convert.ToInt32(ds.Tables[0].Rows[0][3]);
                int newAduserkey = 0;
                string newAdusername = "";
                string adsername = Convert.ToString(ds.Tables[0].Rows[0][4]);
                try
                {
                    string message = "Unit test";

                    IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                    IApplicationRole appRoleCheckPre = orgStructRepo.GetApplicationRoleForADUser(applicationkey, adsername);

                    IApplicationRole appRole = orgStructRepo.GetApplicationRoleByKey(appRoleKey);

                    //Lookup another aduserket to use in the reassign
                    newAduserkey = GetRandomAduser(aduserkey, out newAdusername);

                    //reassign to new user
                    orgStructRepo.ReassignUser(applicationkey, instanceID, newAduserkey, appRole, out message);

                    //Test the the original aduser has been set to inactive
                    Assert.IsTrue(appRoleCheckPre.GeneralStatus.Key == (int)GeneralStatuses.Inactive, "Original user should be inactive");

                    //test that the other user is now active
                    IApplicationRole appRoleCheckPost = orgStructRepo.GetApplicationRoleForADUser(applicationkey, newAdusername);
                    Assert.IsTrue(appRoleCheckPost.GeneralStatus.Key == (int)GeneralStatuses.Active, "New assigned user should be inactive");

                    //Test that we can reassign back to the original user
                    orgStructRepo.ReassignUser(applicationkey, instanceID, aduserkey, appRole, out message);
                    Assert.IsTrue(appRoleCheckPre.GeneralStatus.Key == (int)GeneralStatuses.Active, "Original user should be have been reactivated");

                    //Test that new user is now inactive
                    appRoleCheckPost = orgStructRepo.GetApplicationRoleForADUser(applicationkey, newAdusername);
                    if (appRoleCheckPost != null)
                        Assert.Fail("User should be set to inactive");
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
            }
        }

        private int GetRandomAduser(int ADUserkey, out string adusername)
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                string sql = string.Empty;
                sql = string.Format(@"select top 1 ADUserkey , adusername from aduser where aduserkey != {0}
								and legalEntityKey is not null and GeneralStatusKey =1 and ADUserName like 'SAHL\%user%'",
                                    ADUserkey);

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int aduserkey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                adusername = Convert.ToString(ds.Tables[0].Rows[0][1]);
                return aduserkey;
            }
        }

        [Test]
        public void GetADUsersPerRoleTypeAndOrgStructDictionary()
        {
            string sql = @"SELECT top 1 ad.adusername
			FROM [2AM].[DBO].[ADUser] AD (nolock)
			INNER JOIN [2AM].[DBO].[UserOrganisationStructure] UOS (nolock)
				ON UOS.ADUserKey = AD.ADUserKey
			INNER JOIN [2AM].[DBO].[OrganisationStructure] OS (nolock)
				ON UOS.OrganisationStructureKey = OS.OrganisationStructureKey
			INNER JOIN [2AM].[DBO].OfferRoleTypeOrganisationStructureMapping OSM (nolock)
				ON OSM.oRGANISATIONsTRUCTUREkEY = OS.oRGANISATIONsTRUCTUREkEY
			INNER JOIN [2AM].[DBO].[OfferRoleType] ORT (nolock)
				ON OSM.OfferRoleTypeKey = ORT.OfferRoleTypeKey
			WHERE ORT.OfferRoleTypeGroupKey = 1
				order by ad.aduserkey desc";

            using (new SessionScope())
            {
                ParameterCollection parameters = new ParameterCollection();
                Object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);
                string strADUsername = "";
                if (o != null)
                {
                    strADUsername = Convert.ToString(o);
                }

                IOrganisationStructure _orgStruct = null;

                IADUser _currentADUser = _orgRep.GetAdUserForAdUserName(strADUsername);

                IList<IOrganisationStructure> _orgStructList;

                // get dynamic role types
                _orgStructList = _orgRep.GetOrgStructsPerADUser(_currentADUser);

                IList<IApplicationRoleType> _roleTypes = _orgRep.GetAppRoleTypesForOrgStructList(_orgStructList);

                Dictionary<IADUser, int> adUserDict = _orgRep.GetADUsersPerRoleTypeAndOrgStructDictionary(_roleTypes[0], _orgStructList);
            }
        }

        [Test]
        public void GetBranchForConsultant()
        {
            string sql = @"select top 1 os.OrganisationStructureKey, ofr.OfferRoleKey
				from OfferRole ofr (nolock)
				inner join ADUser a (nolock) on a.LegalEntityKey = ofr.LegalEntityKey
				inner join UserOrganisationStructure uos (nolock) on a.ADUserKey = uos.ADUserKey
				inner join OrganisationStructure os (nolock) on os.OrganisationStructureKey = uos.OrganisationStructureKey";

            IDataReader reader = DBHelper.ExecuteReader(sql);
            if (!reader.Read())
                Assert.Ignore("No data");
            int orgStructKey = reader.GetInt32(0);
            int offerRoleKey = reader.GetInt32(1);
            reader.Dispose();

            using (new SessionScope())
            {
                // load up the IApplicationRole
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                IApplicationRole appRole = appRepo.GetApplicationRoleByKey(offerRoleKey);

                IOrganisationStructure orgStruct = _orgRep.GetBranchForConsultant(appRole);
                Assert.IsNotNull(orgStruct);
                Assert.AreEqual(orgStruct.Key, orgStructKey);
            }
        }

        /// <summary>
        /// Ensures that a recursive query for an aduser works.
        /// </summary>
        [Test]
        public void GetUsersForOrganisationStructureKeyRecursive()
        {
            int orgStructureKey = -1;
            int adUserKey = -1;

            // get an organisation structure that has children and has a parent
            using (new SessionScope())
            {
                string hql = "from OrganisationStructure_DAO os where os.Parent is not null and os.Parent.Key <> 1 and os.ADUsers.size > 0";
                SimpleQuery<OrganisationStructure_DAO> q = new SimpleQuery<OrganisationStructure_DAO>(typeof(ADUser_DAO), hql);
                q.SetQueryRange(1);
                OrganisationStructure_DAO[] results = q.Execute();
                if (results.Length == 0)
                    Assert.Ignore("Unable to find data.");

                // set the keys to an ADUser and the OrgStructure key to the highest key possible
                OrganisationStructure_DAO os = results[0];
                adUserKey = results[0].ADUsers[0].Key;
                orgStructureKey = os.Parent.Key;
            }

            using (new SessionScope())
            {
                // now we run the repository method, and make sure the ADUser appears in the results
                IEventList<IADUser> users = _orgRep.GetUsersForOrganisationStructureKey(orgStructureKey, true);

                bool found = false;
                foreach (IADUser user in users)
                {
                    if (user.Key == adUserKey)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    Assert.Fail("Recursive query did not find ADUser {0} in Org Structure {1}", adUserKey, orgStructureKey);
            }
        }

        [NUnit.Framework.Test]
        public void GetDynamicRoleTypes()
        {
            using (new SessionScope())
            {
                IList<IApplicationRoleType> roleTypes = _orgRep.GetDynamicRoleTypes(null);
                Assert.IsTrue(roleTypes.Count > 0);
            }
        }

        [NUnit.Framework.Test]
        public void GetUsersforDynamicRole()
        {
            using (new SessionScope())
            {
                IApplicationRoleType art = _orgRep.GetApplicationRoleTypeByName("Credit Supervisor D");
                IEventList<IADUser> users = _orgRep.GetUsersForDynamicRole(art);

                Assert.IsTrue(users.Count > 0);
            }
        }

        [NUnit.Framework.Test]
        public void GetUsersforDynamicRoleFull()
        {
            using (new SessionScope())
            {
                IApplicationRoleType art = _orgRep.GetApplicationRoleTypeByName("Credit Supervisor D");
                IEventList<IADUser> adUsers = _orgRep.GetUsersForDynamicRole(art);
                Assert.IsTrue(adUsers.Count > 0);
            }
        }

        [NUnit.Framework.Test]
        public void GetAllUsersforDynamicRoleFull()
        {
            using (new SessionScope())
            {
                IApplicationRoleType art = _orgRep.GetApplicationRoleTypeByName("Credit Supervisor D");
                IEventList<IADUser> adUsers = _orgRep.GetAllUsersForDynamicRole(art, false);
                Assert.IsTrue(adUsers.Count > 0);
            }
        }

        [Test]
        public void GetTopApplicationRoleForApplicationKey()
        {
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 OFR.OfferKey
				FROM [2AM].DBO.[OfferRole] OFR (nolock)
				INNER JOIN [2AM].[DBO].[OfferRoleType] ORT (nolock)
					ON ORT.OfferRoleTypeKey = OFR.OfferRoleTypeKey
				WHERE ORT.OfferRoleTypeGroupKey = 1 AND OFR.GeneralStatusKey = 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int applicationKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                IApplicationRole ar = _orgRep.GetTopApplicationRoleForApplicationKey(applicationKey);
                Assert.IsTrue(ar.Key > 0);
            }
        }

        [NUnit.Framework.Test]
        public void GetFeatureGroupsForUserRoles()
        {
            using (new SessionScope())
            {
                string userRoles = "'SAHL\\CraigF','Everyone','HelpDesk','ITStaff','LifeAdmin','ViewOnly'";

                IEventList<IFeatureGroup> fgs = _orgRep.GetFeatureGroupsForUserRoles(userRoles);

                Assert.IsTrue(fgs.Count > 0);
            }
        }

        [NUnit.Framework.Test]
        public void GetFeaturesForUserRoles()
        {
            using (new SessionScope())
            {
                string userRoles = "'SAHL\\CraigF','Everyone','HelpDesk','ITStaff','LifeAdmin','ViewOnly'";

                IEventList<IFeature> fs = _orgRep.GetFeaturesForUserRoles(userRoles);

                Assert.IsTrue(fs.Count > 0);
            }
        }

        [NUnit.Framework.Test]
        public void GetAllHelpdeskConsultants()
        {
            using (new SessionScope())
            {
                IADUser me = _orgRep.GetAdUserForAdUserName("SAHL\\ClintS");
                IList<IADUser> HDBrats = _orgRep.GetAllHelpdeskConsultants("Help Desk", me);

                Assert.IsTrue(HDBrats.Count > 0);
            }
        }

        /// <summary>
        /// Tests the GetOrgStructureOriginationSourcesPerADUser method.
        /// </summary>
        [Test]
        public void GetOrgStructureOriginationSourcesPerADUser()
        {
            string sql = @";with OSTopLevels (OrganisationStructureKey)
							as
							(
								select distinct OrganisationStructureKey from OrganisationStructureOriginationSource (nolock)
								UNION ALL
								select OS.OrganisationStructureKey
								from
									OrganisationStructure OS  (nolock)
								join OSTopLevels
									on OS.parentkey = OSTopLevels.OrganisationStructureKey
							)
							select top 1 ad.adusername
							from OSTopLevels OS
							inner join UserOrganisationStructure UOS (nolock)
								on OS.OrganisationStructureKey = UOS.OrganisationStructureKey
							inner join ADUser ad (nolock)
								on ad.aduserKey = UOS.aduserkey
							where ad.generalStatusKey = 1";

            IDataReader reader = DBHelper.ExecuteReader(sql);
            if (!reader.Read())
                Assert.Ignore("No data");
            string adUserName = reader.GetString(0);
            reader.Dispose();

            using (new SessionScope())
            {
                IList<IOrganisationStructureOriginationSource> result = _orgRep.GetOrgStructureOriginationSourcesPerADUser(adUserName);
                Assert.Greater(result.Count, 0);
            }
        }

        [Test]
        public void GetOriginationSourceKeysPerADUser()
        {
            string sql = @"select top 1 ADUserName from ADUser a
				inner join UserOrganisationStructure uos on a.ADUserKey = uos.ADUserKey
				inner join OrganisationStructure os on os.OrganisationStructureKey = uos.OrganisationStructureKey";

            IDataReader reader = DBHelper.ExecuteReader(sql);
            if (!reader.Read())
                Assert.Ignore("No data");
            string adUserName = reader.GetString(0);
            reader.Dispose();

            using (new SessionScope())
            {
                IList<int> result = _orgRep.GetOriginationSourceKeysPerADUser(adUserName);
                Assert.Greater(result.Count, 0);
            }
        }

        [NUnit.Framework.Test]
        public void FindApplicationRolesForApplicationKeyAndLEKey()
        {
            using (new SessionScope())
            {
                // get the first application role record
                ApplicationRole_DAO dao = ApplicationRole_DAO.FindFirst();

                // get application roles for the application and legalentity found above
                IOrganisationStructureRepository osr = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IEventList<IApplicationRole> applicationRoles = osr.FindApplicationRolesForApplicationKeyAndLEKey(dao.ApplicationKey, dao.LegalEntityKey);

                Assert.IsTrue(applicationRoles.Count > 0);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void GetOrganisationStructForADUser_NoCrash()
        {
            using (new SessionScope())
            {
                ApplicationRole_DAO dao = ApplicationRole_DAO.FindFirst();

                IOrganisationStructureRepository osr = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                IApplicationRole appRole = appRepo.GetApplicationRoleByKey(dao.Key);

                IOrganisationStructure organisationStructure = osr.GetOrganisationStructForADUser(appRole);

                Assert.IsTrue(1 == 1); //dummy assert here - we are just making sure the repo method doesnt crash
            }
        }

        [Test]
        public void GetOrgStructsPerADUserTest()
        {
            using (new SessionScope())
            {
                string testADUserQ = @"select top 1 ad.aduserKey from [2am].[dbo].[ADUser] ad (nolock)
				where ad.legalEntityKey is not null";
                IOrganisationStructureRepository osr = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(testADUserQ, typeof(ADUser_DAO), new ParameterCollection());
                IADUser aduser = osr.GetADUserByKey(Convert.ToInt32(o));
                IList<IOrganisationStructure> osLst = osr.GetOrgStructsPerADUser(aduser);
                Assert.IsTrue(1 == 1);
            }
        }

        [Test]
        public void GetOrgStructsPerADUserAndCompanyTest()
        {
            using (new SessionScope())
            {
                string testADUserQ = @"select top 1 ad.aduserKey from [2am].[dbo].[ADUser] ad (nolock)
				where ad.legalEntityKey is not null";
                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(testADUserQ, typeof(ADUser_DAO), new ParameterCollection());
                OrganisationStructure_DAO org = OrganisationStructure_DAO.FindFirst();
                IOrganisationStructureRepository osr = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IADUser aduser = osr.GetADUserByKey(Convert.ToInt32(o));
                IList<IOrganisationStructure> osLst = osr.GetOrgStructsPerADUserAndCompany(aduser.Key, org.Key);
                Assert.IsTrue(1 == 1);
            }
        }

        [Test]
        public void GetADUsersPerRoleTypeAndOrgStructListTest()
        {
            string sql = @"SELECT TOP 1 UOS.ORGANISATIONSTRUCTUREKEY, OSM.OFFERROLETYPEKEY
			FROM [2AM].[DBO].OFFERROLETYPEORGANISATIONSTRUCTUREMAPPING OSM (nolock)
			INNER JOIN [2AM].[DBO].[USERORGANISATIONSTRUCTURE] UOS (nolock)
				ON OSM.ORGANISATIONSTRUCTUREKEY = UOS.ORGANISATIONSTRUCTUREKEY
			INNER JOIN [2AM].[DBO].[ADUSER] AD (nolock)
				ON UOS.ADUSERKEY = AD.ADUSERKEY
			WHERE AD.GENERALSTATUSKEY = 1
			GROUP BY UOS.ORGANISATIONSTRUCTUREKEY, OSM.OFFERROLETYPEKEY
			HAVING COUNT(*) > 0";

            DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
            int organisationStructureKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            int offerRoleTypeKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);

            using (new SessionScope())
            {
                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IOrganisationStructure orgStruct = orgStructRepo.GetOrganisationStructureForKey(organisationStructureKey);
                IApplicationRoleType appRoleType = orgStructRepo.GetApplicationRoleTypeByKey(offerRoleTypeKey);
                IList<IOrganisationStructure> orgList = new List<IOrganisationStructure>();
                orgList.Add(orgStruct);
                IEventList<IADUser> aduserList = orgStructRepo.GetADUsersPerRoleTypeAndOrgStructList(appRoleType, orgList, true);
                Assert.IsTrue(aduserList.Count > 0);
            }
        }

        [Test]
        public void GetAllocationMandateSetGroupByUserOrganisationStructureKeyTest()
        {
            string sql = @"select top 1 asuo.UserOrganisationStructureKey
			from [2am].dbo.AllocationMandateSetUserOrganisationStructure asuo (nolock)
			inner join [2am].dbo.allocationMandateSet ams (nolock)
				on asuo.AllocationMandateSetKey = ams.AllocationMandateSetKey
			inner join [2am].dbo.allocationMandateSetGroup amsg (nolock)
				on amsg.allocationMandateSetGroupKey = ams.allocationMandateSetGroupKey";

            DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
            int userOrganisationStructureKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

            using (new SessionScope())
            {
                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IAllocationMandateSetGroup amsg = orgStructRepo.GetAllocationMandateSetGroupByUserOrganisationStructureKey(userOrganisationStructureKey);
                Assert.IsNotNull(amsg);
            }
        }

        [Test]
        public void GetOfferRoleTypeOrganisationStructureMappingKeyTest()
        {
            string sql = @"SELECT TOP 1 ofr.OfferRoleKey
			FROM [2AM].[DBO].[ADUser] AD (nolock)
			INNER JOIN [2AM].[DBO].[UserOrganisationStructure] UOS (nolock)
				ON UOS.ADUserKey = AD.ADUserKey
			INNER JOIN [2AM].[DBO].[OrganisationStructure] OS (nolock)
				ON UOS.OrganisationStructureKey = OS.OrganisationStructureKey
			INNER JOIN [2AM].[DBO].OfferRoleTypeOrganisationStructureMapping OSM (nolock)
				ON OSM.oRGANISATIONsTRUCTUREkEY = OS.oRGANISATIONsTRUCTUREkEY
			INNER JOIN [2AM].[DBO].OfferRole ofr (nolock)
				ON ofr.LegalEntityKey = AD.LegalEntityKey  and ofr.OfferRoleTypeKey = OSM.OfferRoleTypeKey";

            DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
            int OfferRoleKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

            using (new SessionScope())
            {
                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                int? OfferRoleTypeOrganisationStructureMappingKey = orgStructRepo.GetOfferRoleTypeOrganisationStructureMappingKey(OfferRoleKey);
                Assert.IsTrue(OfferRoleTypeOrganisationStructureMappingKey.HasValue);
            }
        }

        [Test]
        public void CreateWorkflowAssignmentTest()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                string sql = @"with Offer_CTE (InstanceID,OfferKey)
                                as
                                (
	                                SELECT TOP 1
		                                xac.InstanceID,xac.ApplicationKey
	                                FROM
		                                x2.X2DATA.Application_Capture xac (nolock)
	                                JOIN
		                                x2.x2.instance xi (nolock) on xi.id = xac.instanceID
	                                WHERE
		                                xac.isEA <> 1 AND xac.isEstateAgentApplication <> 1 AND xac.CaseOwnerName is not null
	                                ORDER BY
		                                xac.InstanceID
                                )
                                SELECT TOP 1
	                                cte.InstanceID,ofr.offerRoleKey,cte.Offerkey,ad.aduserkey, ad.adusername
                                FROM
	                                Offer_CTE CTE
                                JOIN
	                                [2am].dbo.OfferRole ofr (nolock) on ofr.OfferKey = cte.OfferKey
                                JOIN
	                                [2am].dbo.OfferRoleType ort (nolock) on ort.OfferRoleTypeKey = ofr.OfferRoleTypeKey
                                JOIN
	                                [2am].dbo.ADUser ad (nolock) on ad.legalEntityKey = ofr.legalEntityKey
                                JOIN
	                                [2am].dbo.UserOrganisationStructure uos (nolock) on ad.aduserKey = uos.aduserKey
                                JOIN
	                                [2am].dbo.OfferRoleTypeOrganisationStructureMapping ortosm (nolock) on ortosm.OrganisationStructureKey = uos.OrganisationStructureKey
                                WHERE
	                                ad.generalStatusKey = 1
                                AND
	                                ofr.generalStatusKey = 1
                                AND
	                                ort.OfferRoleTypeGroupKey  = 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int instanceID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                int appRoleKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IApplicationRole appRole = orgStructRepo.GetApplicationRoleByKey(appRoleKey);
                orgStructRepo.CreateWorkflowAssignment(appRole, instanceID, GeneralStatuses.Active);
            }
        }

        [Test]
        public void GetWorkflowAssignmentTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 xwa.InstanceID, xwa.OfferRoleTypeOrganisationStructureMappingKey,xwa.ADUserKey,xwa.GeneralStatusKey
				from X2.X2.WorkflowAssignment xwa (nolock)";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int InstanceID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                int OfferRoleTypeOrganisationStructureMappingKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                int ADUserKey = Convert.ToInt32(ds.Tables[0].Rows[0][2]);
                int GeneralStatusKey = Convert.ToInt32(ds.Tables[0].Rows[0][3]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                DataTable dt = orgStructRepo.GetWorkflowAssignment(InstanceID, OfferRoleTypeOrganisationStructureMappingKey, ADUserKey, GeneralStatusKey);
                Assert.IsNotNull(dt);
            }
        }

        [Test]
        public void GetEmptyUserOrganisationStructureHistoryTest()
        {
            using (new SessionScope())
            {
                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IUserOrganisationStructureHistory uosh = orgStructRepo.GetEmptyUserOrganisationStructureHistory();
                Assert.IsNotNull(uosh);
            }
        }

        [Test]
        public void GetEmptyUserOrganisationStructureTest()
        {
            using (new SessionScope())
            {
                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IUserOrganisationStructure uos = orgStructRepo.GetEmptyUserOrganisationStructure();
                Assert.IsNotNull(uos);
            }
        }

        [Test]
        public void HasActiveWorkflowAssignmentTest()
        {
            using (new SessionScope())
            {
                string sql = @"with Offer_CTE (InstanceID,OfferKey)
                                as
                                (
	                                SELECT TOP 1
		                                xac.InstanceID,xac.ApplicationKey
	                                FROM
		                                x2.X2DATA.Application_Capture xac (nolock)
	                                JOIN
		                                x2.x2.instance xi (nolock) on xi.id = xac.instanceID
	                                WHERE
		                                xac.isEA <> 1 AND xac.isEstateAgentApplication <> 1 AND xac.CaseOwnerName is not null
	                                ORDER BY
		                                xac.InstanceID
                                )
                                SELECT TOP 1
	                                cte.InstanceID,ofr.offerRoleKey,cte.Offerkey,ad.aduserkey, ad.adusername
                                FROM
	                                Offer_CTE CTE
                                JOIN
	                                [2am].dbo.OfferRole ofr (nolock) on ofr.OfferKey = cte.OfferKey
                                JOIN
	                                [2am].dbo.OfferRoleType ort (nolock) on ort.OfferRoleTypeKey = ofr.OfferRoleTypeKey
                                JOIN
	                                [2am].dbo.ADUser ad (nolock) on ad.legalEntityKey = ofr.legalEntityKey
                                JOIN
	                                [2am].dbo.UserOrganisationStructure uos (nolock) on ad.aduserKey = uos.aduserKey
                                JOIN
	                                [2am].dbo.OfferRoleTypeOrganisationStructureMapping ortosm (nolock) on ortosm.OrganisationStructureKey = uos.OrganisationStructureKey
                                WHERE
	                                ad.generalStatusKey = 1
                                AND
	                                ofr.generalStatusKey = 1
                                AND
	                                ort.OfferRoleTypeGroupKey  = 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int instanceID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                int appRoleKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IApplicationRole appRole = orgStructRepo.GetApplicationRoleByKey(appRoleKey);
                bool? answer = orgStructRepo.HasActiveWorkflowAssignment(instanceID, appRole);
                Assert.IsTrue(answer.HasValue);
            }
        }

        [Test]
        public void GenerateApplicationRoleTest()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                string sql = @"select top 1 ofr.offerRoleTypeKey, ofr.offerKey, ofr.legalEntityKey
				from [2am].dbo.OfferRole ofr (nolock)
				inner join [2am].dbo.OfferRoleType ort (nolock)
					on ofr.offerRoleTypeKey = ort.offerRoleTypeKey
				where ofr.generalStatusKey = 1 and ort.offerRoleTypeGroupKey = 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int ApplicationRoleTypeKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                int ApplicationKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                int LegalEntityKey = Convert.ToInt32(ds.Tables[0].Rows[0][2]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IApplicationRole appRole = orgStructRepo.GenerateApplicationRole(ApplicationRoleTypeKey, ApplicationKey, LegalEntityKey, true);
                Assert.IsNotNull(appRole);
            }
        }

        [Test]
        public void DeactivateExistingApplicationRolesTest()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                string sql = @"select top 1 ofr.offerKey, ofr.offerRoleTypeKey, ofr.OfferRoleKey
				from [2am].dbo.OfferRole ofr (nolock)
				inner join [2am].dbo.OfferRoleType ort (nolock)
					on ofr.offerRoleTypeKey = ort.offerRoleTypeKey
				where ofr.generalStatusKey = 1 and ort.offerRoleTypeGroupKey = 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int ApplicationKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                int ApplicationRoleTypeKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                int ApplicationRoleKey = Convert.ToInt32(ds.Tables[0].Rows[0][2]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                orgStructRepo.DeactivateExistingApplicationRoles(ApplicationKey, ApplicationRoleTypeKey);
            }
        }

        [Test]
        public void DeactivateExistingApplicationRolesExceptionTest()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                string sql = @"select top 1 ofr.offerKey, ofr.offerRoleTypeKey, ofr.OfferRoleKey
				from [2am].dbo.OfferRole ofr (nolock)
				inner join [2am].dbo.OfferRoleType ort (nolock)
					on ofr.offerRoleTypeKey = ort.offerRoleTypeKey
				where ofr.generalStatusKey = 1 and ort.offerRoleTypeGroupKey = 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int ApplicationKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                int ApplicationRoleTypeKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                int ApplicationRoleKey = Convert.ToInt32(ds.Tables[0].Rows[0][2]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                orgStructRepo.DeactivateExistingApplicationRoles(ApplicationKey, ApplicationRoleTypeKey, ApplicationRoleKey);
            }
        }

        [Test]
        public void DeactivateApplicationRoleTest()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                string sql = @"select top 1 ofr.offerKey, ofr.offerRoleTypeKey, ofr.OfferRoleKey
				from [2am].dbo.OfferRole ofr (nolock)
				inner join [2am].dbo.OfferRoleType ort (nolock)
					on ofr.offerRoleTypeKey = ort.offerRoleTypeKey
				where ofr.generalStatusKey = 1 and ort.offerRoleTypeGroupKey = 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int ApplicationKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                int ApplicationRoleTypeKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                int ApplicationRoleKey = Convert.ToInt32(ds.Tables[0].Rows[0][2]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                orgStructRepo.DeactivateApplicationRole(ApplicationRoleKey);
            }
        }

        [Test]
        public void GetADUserPerOrgStructAndAppRoleTest()
        {
            // Test - GetADUserPerOrgStructAndAppRole
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 UOS.OrganisationStructureKey, ofr.OfferRoleKey
				FROM [2AM].[DBO].OfferRoleTypeOrganisationStructureMapping OSM (nolock)
				INNER JOIN [2AM].[DBO].[OfferRoleType] ort (nolock)
					ON ort.OfferRoleTypeKey = OSM.OfferRoleTypeKey
				INNER JOIN [2AM].[DBO].[UserOrganisationStructure] UOS (nolock)
					ON OSM.OrganisationStructureKey = UOS.OrganisationStructureKey
				INNER JOIN [2AM].[DBO].[ADUser] AD (nolock)
					ON UOS.ADUserKey = AD.ADUserKey
				INNER JOIN [2AM].[DBO].[OfferRole] ofr (nolock)
					ON ofr.legalEntityKey = ad.legalEntityKey and ofr.OfferRoleTypeKey = ort.OfferRoleTypeKey
				WHERE AD.generalStatusKey = 1 and ofr.generalStatusKey = 1 and ort.OfferRoleTypeGroupKey = 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int OrgStructKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                int ApplicationRoleKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IOrganisationStructure orgStruct = orgStructRepo.GetOrganisationStructureForKey(OrgStructKey);
                IApplicationRole appRole = orgStructRepo.GetApplicationRoleByKey(ApplicationRoleKey);
                IEventList<IADUser> adusers = orgStructRepo.GetADUserPerOrgStructAndAppRole(orgStruct, appRole);
                Assert.IsTrue(adusers.Count > 0);
            }
        }

        [Test]
        public void GetApplicationRolesByAppKeyTest()
        {
            // Test - GetApplicationRolesByAppKey
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 OFR.OfferKey, INST.ID
                    FROM X2.X2.INSTANCE INST (nolock)
                    INNER JOIN X2.X2.STATE ST (nolock) ON INST.STATEID = ST.ID
                    INNER JOIN X2.X2.STATEWORKLIST STWL (nolock) ON ST.ID = STWL.STATEID
                    INNER JOIN X2.X2.SECURITYGROUP SG (nolock) ON STWL.SECURITYGROUPID = SG.ID
                    JOIN[2AM].[DBO].[OfferRoleType] ORT (nolock) ON ORT.Description = SG.NAME
                    JOIN [2AM].[DBO].[OfferRole] OFR (nolock) ON OFR.OfferRoleTypeKey = ORT.OfferRoleTypeKey
                    WHERE OFR.GeneralStatusKey = 1 and ORT.OfferRoleTypeGroupKey = 1 and SG.Name = 'Branch Consultant D'";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int applicationKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                int instanceID = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                string adUserName = "adUserName";

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IEventList<IApplicationRole> appRoles = orgStructRepo.GetApplicationRolesByAppKey(applicationKey, adUserName, instanceID);
                Assert.IsTrue(appRoles.Count > 0);
            }
        }

        [Test]
        public void GetApplicationRoleForADUserTest()
        {
            // Test - GetApplicationRoleForADUser
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 AD.ADUserName, OFR.OfferKey
				FROM [2AM].DBO.[OfferRole] OFR (nolock)
				INNER JOIN [2AM].[DBO].[OfferRoleType] ORT (nolock)
					ON ORT.OfferRoleTypeKey = OFR.OfferRoleTypeKey
				INNER JOIN [2AM].[DBO].OfferRoleTypeGroup ORTG (nolock)
					ON ORT.OfferRoleTypeGroupKey = ORTG.OfferRoleTypeGroupKey
				INNER JOIN [2AM].[DBO].[ADUSER] AD (nolock)
					ON OFR.LegalEntityKey = AD.LegalEntityKey
				WHERE ORTG.OfferRoleTypeGroupKey = 1 AND OFR.GeneralStatusKey = 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                string adUserName = Convert.ToString(ds.Tables[0].Rows[0][0]);
                int applicationKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IApplicationRole appRole = orgStructRepo.GetApplicationRoleForADUserAndApplication(adUserName, applicationKey);
                Assert.IsNotNull(appRole);
            }
        }

        [Test]
        public void GetBranchUsersForUserInThisBranchTest()
        {
            // Test - GetBranchUsersForUserInThisBranch
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 AD.ADUserName, OS.description
				FROM [2AM].DBO.[OfferRole] OFR (nolock)
				INNER JOIN [2AM].[DBO].[OfferRoleType] ORT (nolock)
					ON ORT.OfferRoleTypeKey = OFR.OfferRoleTypeKey
				INNER JOIN [2AM].[DBO].OfferRoleTypeOrganisationStructureMapping OSM (nolock)
					ON OSM.OfferRoleTypeKey = OSM.OfferRoleTypeKey
				INNER JOIN [2AM].[DBO].[OrganisationStructure] OS (nolock)
					ON OSM.OrganisationStructureKey = OS.OrganisationStructureKey
				INNER JOIN [2AM].[DBO].OfferRoleTypeGroup ORTG (nolock)
					ON ORT.OfferRoleTypeGroupKey = ORTG.OfferRoleTypeGroupKey
				INNER JOIN [2AM].[DBO].[ADUSER] AD (nolock)
					ON OFR.LegalEntityKey = AD.LegalEntityKey
                inner join [2am]..UserOrganisationstructure UOS (nolock)
                    on UOS.ADUserKey = AD.ADUserKey
				WHERE ORTG.OfferRoleTypeGroupKey = 1 AND OFR.GeneralStatusKey = 1 and ort.description = 'Branch Consultant D'";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                string adUserName = Convert.ToString(ds.Tables[0].Rows[0][0]);
                string OrganisationStructureGroup = Convert.ToString(ds.Tables[0].Rows[0][1]);
                string prefix = "Branch";

                SAHL.Common.BusinessModel.Interfaces.Repositories.OrganisationStructureGroup osg = (SAHL.Common.BusinessModel.Interfaces.Repositories.OrganisationStructureGroup)Enum.Parse(typeof(SAHL.Common.BusinessModel.Interfaces.Repositories.OrganisationStructureGroup), OrganisationStructureGroup, true);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IADUser aduser = orgStructRepo.GetAdUserForAdUserName(adUserName);
                IEventList<IADUser> adusers = orgStructRepo.GetBranchUsersForUserInThisBranch(aduser, osg, prefix);
                Assert.IsTrue(adusers.Count > 0);
            }
        }

        [Test]
        public void GetAttorneyByLegalEntityKeyTest()
        {
            // Test - GetAttorneyByLegalEntityKey
            using (new SessionScope())
            {
                string sql = @"select top 1 le.legalEntityKey
				from [2AM].DBO.[Attorney] att (nolock)
				inner join [2AM].DBO.[LegalEntity] le (nolock)
					on att.legalEntityKey = le.legalEntityKey
				where att.generalStatusKey = 1 ";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int LegalEntityKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IAttorney att = orgStructRepo.GetAttorneyByLegalEntityKey(LegalEntityKey);
                Assert.IsNotNull(att);
            }
        }

        [Test]
        public void GetUsersForRoleTypeAndOrgStructTest()
        {
            // Test - GetUsersForRoleTypeAndOrgStruct
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 OS.OrganisationStructureKey, OSM.OfferRoleTypeKey
				FROM [2AM].[DBO].[ADUser] AD (nolock)
				INNER JOIN [2AM].[DBO].[UserOrganisationStructure] UOS (nolock)
					ON UOS.ADUserKey = AD.ADUserKey
				INNER JOIN [2AM].[DBO].[OrganisationStructure] OS (nolock)
					ON UOS.OrganisationStructureKey = OS.OrganisationStructureKey
				INNER JOIN [2AM].[DBO].OfferRoleTypeOrganisationStructureMapping OSM (nolock)
					ON OSM.oRGANISATIONsTRUCTUREkEY = OS.oRGANISATIONsTRUCTUREkEY
				INNER JOIN [2AM].[DBO].OfferRoleType ORT (nolock)
					ON ORT.OfferRoleTypeKey = ORT.OfferRoleTypeKey
				WHERE ad.generalStatusKey = 1 and ORT.OfferRoleTypeGroupKey = 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int OrganisationStructureKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                int ApplicationRoleTypeKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IOrganisationStructure orgStruct = orgStructRepo.GetOrganisationStructureForKey(OrganisationStructureKey);
                IApplicationRoleType art = orgStructRepo.GetApplicationRoleTypeByKey(ApplicationRoleTypeKey);
                IEventList<IADUser> adusers = orgStructRepo.GetUsersForRoleTypeAndOrgStruct(art, orgStruct);
                Assert.IsTrue(adusers.Count > 0);
            }
        }

        [Test]
        public void GetApplicationRoleForADUserAndApplicationTest()
        {
            // Test - GetApplicationRoleForADUserAndApplication
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 OFR.OfferKey, AD.ADUserName
				FROM [2AM].DBO.[OfferRole] OFR (nolock)
				INNER JOIN [2AM].[DBO].[OfferRoleType] ORT (nolock)
					ON ORT.OfferRoleTypeKey = OFR.OfferRoleTypeKey
				INNER JOIN [2AM].[DBO].OfferRoleTypeGroup ORTG (nolock)
					ON ORT.OfferRoleTypeGroupKey = ORTG.OfferRoleTypeGroupKey
				INNER JOIN [2AM].[DBO].[ADUSER] AD (nolock)
					ON OFR.LegalEntityKey = AD.LegalEntityKey
				WHERE ORTG.OfferRoleTypeGroupKey = 1 AND OFR.GeneralStatusKey = 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int applicationKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                string adUserName = Convert.ToString(ds.Tables[0].Rows[0][1]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IApplicationRole appRole = orgStructRepo.GetApplicationRoleForADUserAndApplication(adUserName, applicationKey);
                Assert.IsNotNull(appRole);
            }
        }

        [Test]
        public void GetLatestApplicationRoleByApplicationKeyAndRoleTypeKeyAndLegalEntityKeyTest()
        {
            // Test - GetLatestApplicationRoleByApplicationKeyAndRoleTypeKeyAndLegalEntityKey
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 OFR.OfferKey, ORT.OfferRoleTypeKey, OFR.GeneralStatusKey, LE.legalEntityKey
				FROM [2AM].DBO.[OfferRole] OFR (nolock)
				INNER JOIN [2AM].[DBO].[OfferRoleType] ORT (nolock)
					ON ORT.OfferRoleTypeKey = OFR.OfferRoleTypeKey
				INNER JOIN [2AM].[DBO].[LegalEntity] LE (nolock)
					ON LE.legalEntityKey = OFR.legalEntityKey
				WHERE ORT.OfferRoleTypeGroupKey = 1 AND OFR.GeneralStatusKey = 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int appKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                int appRoleTypeKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                int generalStatusKey = Convert.ToInt32(ds.Tables[0].Rows[0][2]);
                int legalEntityKey = Convert.ToInt32(ds.Tables[0].Rows[0][3]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IApplicationRole appRole = orgStructRepo.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKeyAndLegalEntityKey(appKey, appRoleTypeKey, legalEntityKey, generalStatusKey);
                Assert.IsNotNull(appRole);
            }
        }

        [Test]
        public void GetLatestApplicationRoleByApplicationKeyAndRoleTypeKeyTest()
        {
            // Test - GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 OFR.OfferKey, ORT.OfferRoleTypeKey, OFR.GeneralStatusKey
				FROM [2AM].DBO.[OfferRole] OFR (nolock)
				INNER JOIN [2AM].[DBO].[OfferRoleType] ORT (nolock)
					ON ORT.OfferRoleTypeKey = OFR.OfferRoleTypeKey
				INNER JOIN [2AM].[DBO].[LegalEntity] LE (nolock)
					ON LE.legalEntityKey = OFR.legalEntityKey
				WHERE ORT.OfferRoleTypeGroupKey = 1 AND OFR.GeneralStatusKey = 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int appKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                int appRoleTypeKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                int generalStatusKey = Convert.ToInt32(ds.Tables[0].Rows[0][2]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IApplicationRole appRole = orgStructRepo.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(appKey, appRoleTypeKey, generalStatusKey);
                Assert.IsNotNull(appRole);
            }
        }

        [Test]
        public void CreateNewApplicationRoleTest()
        {
            // Test - CreateNewApplicationRole
            using (new SessionScope())
            {
                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IApplicationRole appRole = orgStructRepo.CreateNewApplicationRole();
                Assert.IsNotNull(appRole);
            }
        }

        [Test]
        public void GetApplicationRoleTypeByKeyTest()
        {
            // Test - GetApplicationRoleTypeByKey
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 ORT.OfferRoleTypeKey
				FROM [2AM].[DBO].[OfferRoleType] ORT (nolock)
				WHERE ORT.OfferRoleTypeGroupKey = 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int appRoleTypeKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IApplicationRoleType appRoleType = orgStructRepo.GetApplicationRoleTypeByKey(appRoleTypeKey);
                Assert.IsNotNull(appRoleType);
            }
        }

        [Test]
        public void GetApplicationRoleByKey()
        {
            // Test - GetApplicationRoleByKey
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 OFR.OfferRoleKey
				FROM [2AM].DBO.[OfferRole] OFR (nolock)";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int appRoleKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IApplicationRole appRole = orgStructRepo.GetApplicationRoleByKey(appRoleKey);
                Assert.IsNotNull(appRole);
            }
        }

        [Test]
        public void GetApplicationRoleTypeByName()
        {
            // Test - GetApplicationRoleTypeByName
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 ORT.Description
				FROM [2AM].[DBO].[OfferRoleType] ORT (nolock)
				WHERE ORT.OfferRoleTypeGroupKey = 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                string appRoleTypeDesc = Convert.ToString(ds.Tables[0].Rows[0][0]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IApplicationRoleType appRoleType = orgStructRepo.GetApplicationRoleTypeByName(appRoleTypeDesc);
                Assert.IsNotNull(appRoleType);
            }
        }

        /*
         Oustanding Tests
            - OrgStructure and Features
            - ADUsers
         */

        [Test]
        public void GetContextMenuByKeyTest()
        {
            // Test - GetContextMenuByKey
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 CM.ContextKey
				FROM [2AM].[DBO].[ContextMenu] CM (nolock)";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int contextKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IContextMenu contextMenu = orgStructRepo.GetContextMenuByKey(contextKey);
                Assert.IsNotNull(contextMenu);
            }
        }

        [Test]
        public void CreateEmptyContextMenuTest()
        {
            // Test - CreateEmptyContextMenu
            using (new SessionScope())
            {
                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IContextMenu contextMenu = orgStructRepo.CreateEmptyContextMenu();
                Assert.IsNotNull(contextMenu);
            }
        }

        [Test]
        public void GetTopLevelContextMenuNodesTest()
        {
            // Test - GetTopLevelContextMenuNodes
            using (new SessionScope())
            {
                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IEventList<IContextMenu> contextMenus = orgStructRepo.GetTopLevelContextMenuNodes();
            }
        }

        [Test]
        public void GetCBOByKeyTest()
        {
            // Test - GetCBOByKey
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 CBO.CoreBusinessObjectKey
				FROM [2AM].[DBO].[CoreBusinessObjectMenu] CBO (nolock)";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int coreBusinessObjectKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                ICBOMenu cboMenu = orgStructRepo.GetCBOByKey(coreBusinessObjectKey);
                Assert.IsNotNull(cboMenu);
            }
        }

        [Test]
        public void GetFeatureByKeyTest()
        {
            // Test - GetFeatureByKey
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 f.FeatureKey
				FROM [2AM].[DBO].[Feature] f (nolock)";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int featureKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IFeature feature = orgStructRepo.GetFeatureByKey(featureKey);
                Assert.IsNotNull(feature);
            }
        }

        [Test]
        public void CreateEmptyCBOTest()
        {
            // Test - CreateEmptyCBO
            using (new SessionScope())
            {
                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                ICBOMenu cboMenu = orgStructRepo.CreateEmptyCBO();
                Assert.IsNotNull(cboMenu);
            }
        }

        [Test]
        public void GetTopLevelCBONodesTest()
        {
            // Test - GetTopLevelCBONodes
            using (new SessionScope())
            {
                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IEventList<ICBOMenu> cboMenus = orgStructRepo.GetTopLevelCBONodes();
            }
        }

        [Test]
        public void GetCompleteFeatureGroupListTest()
        {
            // Test - GetCompleteFeatureGroupList
            using (new SessionScope())
            {
                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IEventList<IFeatureGroup> featureGroups = orgStructRepo.GetCompleteFeatureGroupList();
            }
        }

        [Test]
        public void GetCompleteFeatureListTest()
        {
            // Test - GetCompleteFeatureList
            using (new SessionScope())
            {
                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IEventList<IFeature> features = orgStructRepo.GetCompleteFeatureList();
            }
        }

        [Test]
        public void CreateEmptyOrganisationStructureTest()
        {
            // Test - CreateEmptyOrganisationStructure
            using (new SessionScope())
            {
                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IOrganisationStructure organisationStructure = orgStructRepo.CreateEmptyOrganisationStructure();
                Assert.IsNotNull(organisationStructure);
            }
        }

        [Test]
        public void GetOrganisationStructureForDescriptionTest()
        {
            // Test - GetOrganisationStructureForDescription
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 ORG.Description
				FROM [2AM].[DBO].[OrganisationStructure] ORG (nolock)";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                string description = Convert.ToString(ds.Tables[0].Rows[0][0]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IOrganisationStructure organisationStructure = orgStructRepo.GetOrganisationStructureForDescription(description);
                Assert.IsNotNull(organisationStructure);
            }
        }

        [Test]
        public void GetRootOrganisationStructureForDescriptionTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 os.Description from [2am].[dbo].[OrganisationStructure] os (nolock) where os.ParentKey is null";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                string description = Convert.ToString(ds.Tables[0].Rows[0][0]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IOrganisationStructure organisationStructure = orgStructRepo.GetRootOrganisationStructureForDescription(description);
                Assert.IsNotNull(organisationStructure);
                Assert.IsTrue(organisationStructure.Description == description);
            }
        }

        [Test]
        public void GetOrganisationStructureForKeyTest()
        {
            // Test - GetOrganisationStructureForKey
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 ORG.OrganisationStructureKey
				FROM [2AM].[DBO].[OrganisationStructure] ORG (nolock)";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int organisationStructureKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IOrganisationStructure organisationStructure = orgStructRepo.GetOrganisationStructureForKey(organisationStructureKey);
                Assert.IsNotNull(organisationStructure);
            }
        }

        [Test]
        public void GetCompanyListTest()
        {
            // Test - GetCompanyList
            using (new SessionScope())
            {
                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IEventList<IOrganisationStructure> organisationStructures = orgStructRepo.GetCompanyList();
            }
        }

        [Test]
        public void GetTopLevelOrganisationStructureForOriginationSourceTest()
        {
            // Test - GetTopLevelOrganisationStructureForOriginationSource
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 ORG.ParentKey
				FROM [2AM].[DBO].[OrganisationStructure] ORG (nolock)
				WHERE ORG.ParentKey IS NOT NULL
				GROUP BY ORG.ParentKey
				HAVING COUNT(*) > 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int parentKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IEventList<IOrganisationStructure> organisationStructures = orgStructRepo.GetTopLevelOrganisationStructureForOriginationSource(parentKey);
                Assert.IsTrue(organisationStructures != null && organisationStructures.Count > 0);
            }
        }

        [Test]
        public void CreateEmptyFeatureTest()
        {
            // Test - CreateEmptyFeature
            using (new SessionScope())
            {
                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IFeature feature = orgStructRepo.CreateEmptyFeature();
                Assert.IsNotNull(feature);
            }
        }

        [Test]
        public void GetTopLevelFeatureListTest()
        {
            // Test - GetTopLevelFeatureList
            using (new SessionScope())
            {
                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IEventList<IFeature> features = orgStructRepo.GetTopLevelFeatureList();
            }
        }

        [Test]
        public void GetCompleteAdUserListTest()
        {
            // Test - GetCompleteAdUserList
            using (new SessionScope())
            {
                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IEventList<IADUser> adusers = orgStructRepo.GetCompleteAdUserList();
            }
        }

        [Test]
        public void CreateEmptyAdUserTest()
        {
            // Test - CreateEmptyAdUser
            using (new SessionScope())
            {
                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IADUser aduser = orgStructRepo.CreateEmptyAdUser();
                Assert.IsNotNull(aduser);
            }
        }

        [Test]
        public void GetAdUserForAdUserNameTest()
        {
            // Test - GetAdUserForAdUserName
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 AD.adusername
				FROM [2AM].[DBO].[ADUser] AD (nolock)
				WHERE AD.GeneralStatusKey = 1 and AD.ADUserName IS NOT NULL AND AD.ADUserName <> ''";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                string adusername = Convert.ToString(ds.Tables[0].Rows[0][0]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IADUser aduser = orgStructRepo.GetAdUserForAdUserName(adusername);
                Assert.IsNotNull(aduser);
            }
        }

        [Test]
        public void GetAdUserByLegalEntityKeyTest()
        {
            // Test - GetAdUserByLegalEntityKey
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 LE.LegalEntityKey
				FROM [2AM].[DBO].[ADUser] AD (nolock)
				INNER JOIN [2AM].[DBO].[LegalEntity] LE (nolock)
					ON LE.LegalEntityKey = AD.LegalEntityKey
				WHERE AD.GeneralStatusKey = 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int legalEntityKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IADUser aduser = orgStructRepo.GetAdUserByLegalEntityKey(legalEntityKey);
                Assert.IsNotNull(aduser);
            }
        }

        [Test]
        public void GetAdUsersByPartialNameTest()
        {
            // Test - GetAdUsersByPartialName
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 AD.adusername
				FROM [2AM].[DBO].[ADUser] AD (nolock)
				WHERE AD.GeneralStatusKey = 1 and AD.ADUserName IS NOT NULL AND AD.ADUserName <> ''";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                string adusername = Convert.ToString(ds.Tables[0].Rows[0][0]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IEventList<IADUser> adusers = orgStructRepo.GetAdUsersByPartialName(adusername, 10);
                Assert.IsTrue(adusers != null && adusers.Count > 0);
            }
        }

        [Test]
        public void GetADUserByKeyTest()
        {
            // Test - GetADUserByKey
            using (new SessionScope())
            {
                string sql = @"SELECT TOP 1 AD.ADUserKey
				FROM [2AM].[DBO].[ADUser] AD (nolock)
				WHERE AD.GeneralStatusKey = 1 and AD.ADUserName IS NOT NULL AND AD.ADUserName <> ''";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int aduserKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IADUser aduser = orgStructRepo.GetADUserByKey(aduserKey);
                Assert.IsNotNull(aduser);
            }
        }

        [Test]
        public void GetCreditRoleTypes()
        {
            using (new SessionScope())
            {
                string sql = @"Select OrganisationStructureKey
								From OrganisationStructure
								Where Description = 'Credit'";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());

                if (ds != null)
                {
                    int orgstructurekey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                    IEventList<IApplicationRoleType> roleTypes = orgStructRepo.GetCreditRoleTypes(orgstructurekey);
                    Assert.IsTrue(roleTypes != null);
                }
                else
                {
                    Assert.Fail("No valid keys for this test");
                }
            }
        }

        [Test]
        public void GetUsersPerOrgStruct()
        {
            using (new SessionScope())
            {
                string sql = @"Select OrganisationStructureKey
								From OrganisationStructure
								Where Description = 'Credit'";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());

                if (ds != null)
                {
                    int orgstructurekey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                    IEventList<IADUser> users = orgStructRepo.GetUsersPerOrgStruct(orgstructurekey);
                    Assert.IsTrue(users != null);
                }
                else
                {
                    Assert.Fail("No valid keys for this test");
                }
            }
        }

        [Test]
        public void GetADUsersByWorkflowRoleTypeAndOrgStructListTest()
        {
            using (new SessionScope())
            {
                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                string sql = @"
						SELECT TOP 1 osm.organisationstructurekey,
						OSM.WorkflowRoleTypeKey
						FROM   [2AM].[DBO].workflowroletypeorganisationstructuremapping osm (nolock)
						INNER JOIN [2AM].[DBO].[USERORGANISATIONSTRUCTURE] uos (nolock)
						ON osm.organisationstructurekey = uos.organisationstructurekey
						INNER JOIN [2AM].[DBO].[UserOrganisationStructureRoundRobinStatus] uosrrs( nolock)
						ON uosrrs.userorganisationstructurekey =
						uos.userorganisationstructurekey
						INNER JOIN [2AM].[DBO].generalstatus rrs (nolock)
						ON rrs.generalstatuskey = uosrrs.generalstatuskey
						INNER JOIN [2AM].[DBO].[ADUSER] ad (nolock)
						ON uos.aduserkey = ad.aduserkey
						INNER JOIN [2AM].[DBO].generalstatus ads (nolock)
						ON ads.generalstatuskey = ad.generalstatuskey
						INNER JOIN [2AM].[DBO].legalentity le (nolock)
						ON ad.legalentitykey = le.legalentitykey
						ORDER  BY ad.adusername";

                int OrganisationStructureKey = 1;
                int OfferRoleTypeKey = 1;

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    OfferRoleTypeKey = Convert.ToInt32(ds.Tables[0].Rows[0]["WorkflowRoleTypeKey"]);
                    OrganisationStructureKey = Convert.ToInt32(ds.Tables[0].Rows[0]["OrganisationStructureKey"]);
                }
                IOrganisationStructure orgStruct = orgStructRepo.GetOrganisationStructureForKey(OrganisationStructureKey);
                IList<IOrganisationStructure> orgList = new List<IOrganisationStructure>();
                orgList.Add(orgStruct);
                DataTable dt = orgStructRepo.GetADUsersByWorkflowRoleTypeAndOrgStructList(OfferRoleTypeKey, orgList);
                Assert.IsTrue(dt != null);
            }
        }

        [Test]
        public void GetADUsersPerRoleTypeAndOrgStructListDTTest()
        {
            using (new SessionScope())
            {
                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                string sql = @"SELECT TOP 1 OSM.OfferRoleTypeKey,UOS.OrganisationStructureKey
				FROM [2AM].[DBO].OFFERROLETYPEORGANISATIONSTRUCTUREMAPPING OSM (nolock)
				INNER JOIN [2AM].[DBO].[USERORGANISATIONSTRUCTURE] UOS (nolock)
					ON OSM.ORGANISATIONSTRUCTUREKEY = UOS.ORGANISATIONSTRUCTUREKEY
				INNER JOIN [2AM].[DBO].[UserOrganisationStructureRoundRobinStatus] UOSRRS
					ON UOSRRS.UserOrganisationStructureKey = UOS.UserOrganisationStructureKey
				INNER JOIN [2AM].[DBO].GeneralStatus RRS (nolock)
					ON RRS.GeneralStatusKey = UOSRRS.GeneralStatusKey
				INNER JOIN [2AM].[DBO].[ADUSER] AD (nolock)
					ON UOS.ADUSERKEY = AD.ADUSERKEY
				INNER JOIN [2AM].[DBO].GeneralStatus ADS (nolock)
					ON ADS.GeneralStatusKey = AD.GeneralStatusKey
				INNER JOIN [2AM].[DBO].LegalEntity LE (nolock)
					ON AD.LegalEntityKey  = LE.LegalEntityKey";

                int OrganisationStructureKey = 1;
                int OfferRoleTypeKey = 1;

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    OfferRoleTypeKey = Convert.ToInt32(ds.Tables[0].Rows[0]["OfferRoleTypeKey"]);
                    OrganisationStructureKey = Convert.ToInt32(ds.Tables[0].Rows[0]["OrganisationStructureKey"]);
                }
                IOrganisationStructure orgStruct = orgStructRepo.GetOrganisationStructureForKey(OrganisationStructureKey);
                IList<IOrganisationStructure> orgList = new List<IOrganisationStructure>();
                orgList.Add(orgStruct);
                DataTable dt = orgStructRepo.GetADUsersPerRoleTypeAndOrgStructListDT(OfferRoleTypeKey, orgList);
                Assert.IsTrue(dt != null);
            }
        }

        [Test]
        public void GetUserOrganisationStructureRoundRobinStatusTest()
        {
            using (new SessionScope())
            {
                IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                int uosrrsKey = UserOrganisationStructureRoundRobinStatus_DAO.FindFirst().Key;
                IUserOrganisationStructureRoundRobinStatus userOrganisationStructureRoundRobinStatus = orgStructRepo.GetUserOrganisationStructureRoundRobinStatus(uosrrsKey);
                Assert.IsNotNull(userOrganisationStructureRoundRobinStatus);
            }
        }

        [Test]
        public void SaveUserOrganisationStructureFutureTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                IUserOrganisationStructure userOrganisationStructure = SaveUserOrganisationStructureHelper(DateTime.Now.AddDays(1));
                Assert.IsNotNull(userOrganisationStructure.UserOrganisationStructureRoundRobinStatus);
                Assert.IsTrue(userOrganisationStructure.UserOrganisationStructureRoundRobinStatus[0].GeneralStatus.Key == (int)GeneralStatuses.Inactive);
                Assert.IsTrue(userOrganisationStructure.UserOrganisationStructureRoundRobinStatus[0].CapitecGeneralStatus.Key == (int)GeneralStatuses.Inactive);
            }
        }

        [Test]
        public void SaveUserOrganisationStructurePastTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                IUserOrganisationStructure userOrganisationStructure = SaveUserOrganisationStructureHelper(DateTime.Now.AddDays(-1));
                Assert.IsNotNull(userOrganisationStructure.UserOrganisationStructureRoundRobinStatus);
                Assert.IsTrue(userOrganisationStructure.UserOrganisationStructureRoundRobinStatus[0].GeneralStatus.Key == (int)GeneralStatuses.Active);
                Assert.IsTrue(userOrganisationStructure.UserOrganisationStructureRoundRobinStatus[0].CapitecGeneralStatus.Key == (int)GeneralStatuses.Inactive);
            }
        }

        [Test]
        public void SaveUserOrganisationStructureTodayTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                IUserOrganisationStructure userOrganisationStructure = SaveUserOrganisationStructureHelper(DateTime.Now);
                Assert.IsNotNull(userOrganisationStructure.UserOrganisationStructureRoundRobinStatus);
                Assert.IsTrue(userOrganisationStructure.UserOrganisationStructureRoundRobinStatus[0].GeneralStatus.Key == (int)GeneralStatuses.Active);
                Assert.IsTrue(userOrganisationStructure.UserOrganisationStructureRoundRobinStatus[0].CapitecGeneralStatus.Key == (int)GeneralStatuses.Inactive);
            }
        }

        [Test]
        public void GetUsersForWorkflowRoleType()
        {
            using (new SessionScope())
            {
                IList<WorkflowRoleTypes> workflowRoleTypes = new List<WorkflowRoleTypes>();
                workflowRoleTypes.Add(SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingAdminD);
                workflowRoleTypes.Add(SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingConsultantD);

                DataTable dt = _orgRep.GetUsersForWorkflowRoleType(workflowRoleTypes);
                Assert.Greater(dt.Rows.Count, 0);
            }
        }

        [Test]
        public void GetRoleTypesByOrganisationStructureKeyTest()
        {
            using (new SessionScope())
            {
                string sql = @"	select top 1 os.OrganisationStructureKey
	                            from [2am]..OfferRoleType ort (nolock)
	                            inner join [2am]..OfferRoleTypeOrganisationStructureMapping ortosm (nolock)
		                            on ortosm.OfferRoleTypeKey = ort.OfferRoleTypeKey
	                            inner join [2am]..OrganisationStructure os (nolock)
		                            on ortosm.OrganisationStructureKey = os.OrganisationStructureKey";
                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                if (o != null)
                {
                    Dictionary<string, string> dict = _orgRep.GetRoleTypesByOrganisationStructureKey(new List<int> { Convert.ToInt32(o) });
                    Assert.IsTrue(dict.Count > 0);
                }
                else
                    Assert.Fail("No data");
            }
        }

        private static IUserOrganisationStructure SaveUserOrganisationStructureHelper(DateTime startDate)
        {
            string sql = @"
					declare @ADUserKey int
					declare @OrganisationStructureKey int

					select top 1 @ADUserKey = uos.ADUserKey, @OrganisationStructureKey = uos.OrganisationStructureKey
					from [2am].dbo.OfferRoleTypeOrganisationStructureMapping ortosm
					inner join [2am].dbo.UserOrganisationStructure uos
						on uos.OrganisationStructureKey = ortosm.OrganisationStructureKey
					inner join [2am].dbo.ADUser ad
						on ad.aduserKey = uos.aduserKey
					where ad.generalStatusKey = 1

					select top 1 @ADUserKey as ADUserKey,uos.OrganisationStructureKey
					from [2am].dbo.OfferRoleTypeOrganisationStructureMapping ortosm
					inner join [2am].dbo.UserOrganisationStructure uos
						on uos.OrganisationStructureKey = ortosm.OrganisationStructureKey
					where uos.ADUserKey <> @ADUserKey and uos.OrganisationStructureKey <> @OrganisationStructureKey";

            DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
            int adUserKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            int organisationStructureKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);

            IOrganisationStructureRepository orgRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IADUser aduser = orgRepo.GetADUserByKey(adUserKey);
            IOrganisationStructure organisationStructure = orgRepo.GetOrganisationStructureForKey(organisationStructureKey);

            IGeneralStatus generalStatus = RepositoryFactory.GetRepository<ILookupRepository>().GeneralStatuses[GeneralStatuses.Active];

            IUserOrganisationStructure userOrganisationStructure = orgRepo.GetEmptyUserOrganisationStructure();
            userOrganisationStructure.ADUser = aduser;
            userOrganisationStructure.OrganisationStructure = organisationStructure;
            userOrganisationStructure.GeneralStatus = generalStatus;
            orgRepo.SaveUserOrganisationStructure(userOrganisationStructure, startDate, null);
            return userOrganisationStructure;
        }
    }
}