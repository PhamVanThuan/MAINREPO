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
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Rules.OrganisationStructure;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Rules.Test.OrganisationStructure
{
    [TestFixture]
    public class OrganisationStructure : RuleBase
    {
        [SetUp()]
        public override void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void UserOrganisationStructureDistinctCheckTestPass()
        {
            using (new SessionScope())
            {
                IUserOrganisationStructure userOrganisationStructure = _mockery.StrictMock<IUserOrganisationStructure>();
                IOrganisationStructure organisationStructure = _mockery.StrictMock<IOrganisationStructure>();
                IOrganisationType organisationType = _mockery.StrictMock<IOrganisationType>();
                IADUser aduser = _mockery.StrictMock<IADUser>();

                UserOrganisationStructureDistinctCheck rule = new UserOrganisationStructureDistinctCheck(RepositoryFactory.GetRepository<ICastleTransactionsService>());
                string query = @"select top 1 uos.*
                            from UserOrganisationStructure uos
                            inner join OrganisationStructure os
                                on uos.OrganisationStructureKey = os.OrganisationStructureKey
                            inner join OrganisationType ot
                                on os.OrganisationTypeKey = ot.OrganisationTypeKey
                            where os.Description = 'Admin'
                                order by uos.UserOrganisationStructureKey desc";
                SimpleQuery<UserOrganisationStructure_DAO> q = new SimpleQuery<UserOrganisationStructure_DAO>(QueryLanguage.Sql, query);
                q.AddSqlReturnDefinition(typeof(UserOrganisationStructure_DAO), "uos");
                UserOrganisationStructure_DAO[] res = q.Execute();

                if (res != null && res.Length > 0)
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    IUserOrganisationStructure uos = BMTM.GetMappedType<IUserOrganisationStructure, UserOrganisationStructure_DAO>(res[0]);

                    SetupResult.For(aduser.Key).Return(uos.ADUser.Key);
                    SetupResult.For(organisationStructure.Description).Return(uos.OrganisationStructure.Description);
                    SetupResult.For(organisationType.Key).Return(uos.OrganisationStructure.OrganisationType.Key);
                    SetupResult.For(organisationType.Description).Return(uos.OrganisationStructure.OrganisationType.Description);
                    SetupResult.For(organisationStructure.OrganisationType).Return(organisationType);
                    SetupResult.For(userOrganisationStructure.ADUser).Return(aduser);
                    SetupResult.For(userOrganisationStructure.OrganisationStructure).Return(organisationStructure);
                    SetupResult.For(userOrganisationStructure.Key).Return(0);
                    ExecuteRule(rule, 0, userOrganisationStructure);
                }
                else
                    Assert.Fail("No data for test");
            }
        }

        [Test]
        public void UserOrganisationStructureDistinctCheckTestFail()
        {
            using (new SessionScope())
            {
                IUserOrganisationStructure userOrganisationStructure = _mockery.StrictMock<IUserOrganisationStructure>();
                IOrganisationStructure organisationStructure = _mockery.StrictMock<IOrganisationStructure>();
                IOrganisationType organisationType = _mockery.StrictMock<IOrganisationType>();
                IADUser aduser = _mockery.StrictMock<IADUser>();

                UserOrganisationStructureDistinctCheck rule = new UserOrganisationStructureDistinctCheck(RepositoryFactory.GetRepository<ICastleTransactionsService>());
                string query = @"select top 1 uos.*
                            from UserOrganisationStructure uos
                            inner join OrganisationStructure os
                                on uos.OrganisationStructureKey = os.OrganisationStructureKey
                            inner join OrganisationType ot
                                on os.OrganisationTypeKey = ot.OrganisationTypeKey
                            where os.Description = 'Consultant'
                                order by uos.UserOrganisationStructureKey desc";
                SimpleQuery<UserOrganisationStructure_DAO> q = new SimpleQuery<UserOrganisationStructure_DAO>(QueryLanguage.Sql, query);
                q.AddSqlReturnDefinition(typeof(UserOrganisationStructure_DAO), "uos");
                UserOrganisationStructure_DAO[] res = q.Execute();

                if (res != null && res.Length > 0)
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    IUserOrganisationStructure uos = BMTM.GetMappedType<IUserOrganisationStructure, UserOrganisationStructure_DAO>(res[0]);

                    SetupResult.For(aduser.Key).Return(uos.ADUser.Key);
                    SetupResult.For(organisationStructure.Description).Return(uos.OrganisationStructure.Description);
                    SetupResult.For(organisationType.Key).Return(uos.OrganisationStructure.OrganisationType.Key);
                    SetupResult.For(organisationType.Description).Return(uos.OrganisationStructure.OrganisationType.Description);
                    SetupResult.For(organisationStructure.OrganisationType).Return(organisationType);
                    SetupResult.For(userOrganisationStructure.ADUser).Return(aduser);
                    SetupResult.For(userOrganisationStructure.OrganisationStructure).Return(organisationStructure);
                    SetupResult.For(userOrganisationStructure.Key).Return(0);
                    ExecuteRule(rule, 1, userOrganisationStructure);
                }
                else
                    Assert.Fail("No data for test");
            }
        }

        [Test]
        public void UserOrganisationStructureLinkedToApplicationCheckTestPass()
        {
            using (new SessionScope())
            {
                UserOrganisationStructureLinkedToApplicationCheck rule = new UserOrganisationStructureLinkedToApplicationCheck(RepositoryFactory.GetRepository<ICastleTransactionsService>());
                string query = @"select top 1 uos.*
                                from UserOrganisationStructure uos
                                inner join vOrganisationStructure vos
	                                on uos.OrganisationStructureKey = vos.OrganisationStructureKey
                                inner join vUserOrganisationStructureHistory vuosh
	                                on uos.OrganisationStructureKey = vuosh.OrganisationStructureKey
                                        and uos.ADUserKey = vuosh.ADUserKey and vuosh.EndDate is null
                                inner join ADUser ad
	                                on uos.ADUserKey = ad.ADUserKey
                                left join OfferRole ofr
	                                on ofr.LegalEntityKey = ad.LegalEntityKey
                                where ofr.OfferRoleKey  is null";

                SimpleQuery<UserOrganisationStructure_DAO> q = new SimpleQuery<UserOrganisationStructure_DAO>(QueryLanguage.Sql, query);
                q.AddSqlReturnDefinition(typeof(UserOrganisationStructure_DAO), "uos");
                UserOrganisationStructure_DAO[] res = q.Execute();

                if (res != null && res.Length > 0)
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    IUserOrganisationStructure uos = BMTM.GetMappedType<IUserOrganisationStructure, UserOrganisationStructure_DAO>(res[0]);

                    ExecuteRule(rule, 0, uos, DateTime.Today);
                }
                else
                    Assert.Fail("No data for test");
            }
        }

        [Test]
        public void UserOrganisationStructureLinkedToApplicationCheckTestFail()
        {
            using (new SessionScope())
            {
                UserOrganisationStructureLinkedToApplicationCheck rule = new UserOrganisationStructureLinkedToApplicationCheck(RepositoryFactory.GetRepository<ICastleTransactionsService>());
                string query = @"select top 1 uos.*
                                from UserOrganisationStructure uos
                                inner join vOrganisationStructure vos
                                    on uos.OrganisationStructureKey = vos.OrganisationStructureKey
                                inner join vUserOrganisationStructureHistory vuosh
                                    on uos.OrganisationStructureKey = vuosh.OrganisationStructureKey and uos.ADUserKey = vuosh.ADUserKey and vuosh.EndDate is null
                                inner join ADUser ad
                                    on uos.ADUserKey = ad.ADUserKey
                                inner join OfferRole ofr
                                    on ofr.LegalEntityKey = ad.LegalEntityKey
                                inner join Offer o
                                    on ofr.OfferKey = o.OfferKey
                                where  ofr.GeneralStatusKey = 1 and o.OfferStatusKey not in (2,3,4,5) and
                                (ofr.StatusChangeDate >= vuosh.StartDate and ofr.StatusChangeDate <= getdate())
                                and ofr.OfferRoleTypeKey not in (100)
								and uos.UserOrganisationStructureKey != 6355";

                SimpleQuery<UserOrganisationStructure_DAO> q = new SimpleQuery<UserOrganisationStructure_DAO>(QueryLanguage.Sql, query);
                q.AddSqlReturnDefinition(typeof(UserOrganisationStructure_DAO), "uos");
                UserOrganisationStructure_DAO[] res = q.Execute();

                if (res != null && res.Length > 0)
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    IUserOrganisationStructure uos = BMTM.GetMappedType<IUserOrganisationStructure, UserOrganisationStructure_DAO>(res[0]);

                    ExecuteRule(rule, 1, uos, DateTime.Now);
                }
                else
                    Assert.Fail("No data for test");
            }
        }

        [Test]
        public void UserOrganisationStructureLinkedToActiveWorkflowRole()
        {
            using (new SessionScope(FlushAction.Never))
            {
                UserOrganisationStructureLinkedToActiveWorkflowRole rule = new UserOrganisationStructureLinkedToActiveWorkflowRole(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                //FAIL!
                string query = @"select top 1 uos.*
                from UserOrganisationStructure uos (nolock)
                inner join vOrganisationStructure vos (nolock)
	                on uos.OrganisationStructureKey = vos.OrganisationStructureKey
                inner join ADUser ad (nolock)
	                on uos.ADUserKey = ad.ADUserKey
                inner join WorkflowRole wfr  (nolock)
	                on wfr.LegalEntityKey = ad.LegalEntityKey
	                and wfr.GeneralStatusKey = 1
	                and uos.GenericKey = wfr.WorkflowRoleTypeKey
                inner join WorkflowRoleType wrt (nolock)
	                on wrt.WorkflowRoleTypeKey = wfr.WorkflowRoleTypeKey
                where
	                wrt.WorkflowRoleTypeGroupKey = 1";

                SimpleQuery<UserOrganisationStructure_DAO> q = new SimpleQuery<UserOrganisationStructure_DAO>(QueryLanguage.Sql, query);
                q.AddSqlReturnDefinition(typeof(UserOrganisationStructure_DAO), "uos");
                UserOrganisationStructure_DAO[] res = q.Execute();

                if (res != null && res.Length > 0)
                {
                    int wrtKey = res[0].GenericKey;
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    IUserOrganisationStructure uos = BMTM.GetMappedType<IUserOrganisationStructure, UserOrganisationStructure_DAO>(res[0]);
                    ExecuteRule(rule, 1, uos, wrtKey);
                }
                else
                {
                    Assert.Fail("No data for test");
                }

                //PASS
                query = @"select top 1 uos.*
                from UserOrganisationStructure uos (nolock)
                inner join vOrganisationStructure vos (nolock)
	                on uos.OrganisationStructureKey = vos.OrganisationStructureKey
                inner join ADUser ad (nolock)
	                on uos.ADUserKey = ad.ADUserKey
                inner join WorkflowRole wfr  (nolock)
	                on wfr.LegalEntityKey = ad.LegalEntityKey
	                and wfr.GeneralStatusKey = 2
	                and uos.GenericKey = wfr.WorkflowRoleTypeKey
                left join WorkflowRole wfr1  (nolock)
	                on wfr.LegalEntityKey = ad.LegalEntityKey
	                and wfr.GeneralStatusKey = 1
	                and uos.GenericKey = wfr.WorkflowRoleTypeKey
                inner join WorkflowRoleType wrt (nolock)
	                on wrt.WorkflowRoleTypeKey = wfr.WorkflowRoleTypeKey
                where
	                wrt.WorkflowRoleTypeGroupKey = 1
		                and
	                wfr1.workflowRoleKey is null";

                q = new SimpleQuery<UserOrganisationStructure_DAO>(QueryLanguage.Sql, query);
                q.AddSqlReturnDefinition(typeof(UserOrganisationStructure_DAO), "uos");
                res = q.Execute();

                if (res != null && res.Length > 0)
                {
                    int wrtKey = 0;
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    IUserOrganisationStructure uos = BMTM.GetMappedType<IUserOrganisationStructure, UserOrganisationStructure_DAO>(res[0]);
                    ExecuteRule(rule, 0, uos, wrtKey);
                }
                else
                {
                    //TODO: NODATATEST
                    //Assert.Fail("No data for test");
                }
            }
        }

        [Test]
        public void UserOrganisationStructureStartDateCheckTestFail()
        {
            using (new SessionScope())
            {
                UserOrganisationStructureStartDateCheck rule = new UserOrganisationStructureStartDateCheck(RepositoryFactory.GetRepository<ICastleTransactionsService>());
                string query = @"select top 1 ad.*
                                from [2am].[dbo].[Offer] o
                                inner join [2am].[dbo].[OfferRole] ofr
	                                on o.offerKey = ofr.offerKey
                                inner join [2am].[dbo].[legalEntity] le
	                                on le.legalEntityKey = ofr.legalEntityKey
                                inner join [2am].[dbo].[aduser] ad
	                                on ad.legalEntitykey = le.LegalEntityKey
                                where o.OfferStartDate > getdate()-100 and ofr.GeneralStatusKey = 1";

                SimpleQuery<ADUser_DAO> q = new SimpleQuery<ADUser_DAO>(QueryLanguage.Sql, query);
                q.AddSqlReturnDefinition(typeof(ADUser_DAO), "ad");
                ADUser_DAO[] res = q.Execute();

                if (res != null && res.Length > 0)
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    IADUser ad = BMTM.GetMappedType<IADUser, ADUser_DAO>(res[0]);

                    ExecuteRule(rule, 1, ad, DateTime.Today.AddDays(-100));
                }
                else
                    Assert.Ignore("No data for test");
            }
        }

        [Test]
        public void UserOrganisationStructureLegalEntityAlreadyExists()
        {
            using (new SessionScope())
            {
                UserOrganisationStructureAlreadyHasLegalEntity rule = new UserOrganisationStructureAlreadyHasLegalEntity(RepositoryFactory.GetRepository<ICastleTransactionsService>());
                string query = @"select top 1 legalentitykey,organisationstructurekey from dbo.LegalEntityOrganisationStructure order by 1 desc";

                ParameterCollection parameters = new ParameterCollection();
                DataTable DT = new DataTable();

                int lekey = -1;
                int orgkey = -1;

                using (IDbConnection con = Helper.GetSQLDBConnection())
                {
                    Helper.FillFromQuery(DT, query, con, parameters);
                }

                if (DT != null && DT.Rows.Count > 0)
                {
                    lekey = Convert.ToInt32(DT.Rows[0][0]);
                    orgkey = Convert.ToInt32(DT.Rows[0][1]);
                }
                else
                    Assert.Fail("No data available");

                ExecuteRule(rule, 1, lekey, orgkey);
            }
        }

        [Test]
        public void UserOrganisationStructureStartDateCheckTestPass()
        {
            using (new SessionScope())
            {
                UserOrganisationStructureStartDateCheck rule = new UserOrganisationStructureStartDateCheck(RepositoryFactory.GetRepository<ICastleTransactionsService>());
                string query = @"select top 1 ad.*
                                from [2am].[dbo].[Offer] o
                                inner join [2am].[dbo].[OfferRole] ofr
	                                on o.offerKey = ofr.offerKey
                                inner join [2am].[dbo].[legalEntity] le
	                                on le.legalEntityKey = ofr.legalEntityKey
                                inner join [2am].[dbo].[aduser] ad
	                                on ad.legalEntitykey = le.LegalEntityKey
                                where o.OfferStartDate < getdate() and ofr.GeneralStatusKey = 1";

                SimpleQuery<ADUser_DAO> q = new SimpleQuery<ADUser_DAO>(QueryLanguage.Sql, query);
                q.AddSqlReturnDefinition(typeof(ADUser_DAO), "ad");
                ADUser_DAO[] res = q.Execute();

                if (res != null && res.Length > 0)
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    IADUser ad = BMTM.GetMappedType<IADUser, ADUser_DAO>(res[0]);

                    ExecuteRule(rule, 0, ad, DateTime.Today);
                }
                else
                    Assert.Fail("No data for test");
            }
        }

        [Test]
        public void UserOrganisationStructureEndDateCheckTestFail()
        {
            using (new SessionScope())
            {
                UserOrganisationStructureEndDateCheck rule = new UserOrganisationStructureEndDateCheck(RepositoryFactory.GetRepository<ICastleTransactionsService>());
                string query = @"select top 1 StartDate - 10 , aduserKey, OrganisationStructureKey from [2am].[dbo].[vUserOrganisationStructureHistory]
                                where EndDate is null
                                order by UserOrganisationStructureHistoryKey desc";

                ParameterCollection parameters = new ParameterCollection();
                DataTable DT = new DataTable();
                DateTime startDate = new DateTime();
                int adUserKey = -1;
                int organisationStructureKey = -1;
                char userOrgStructHistDelete = 'D';

                using (IDbConnection con = Helper.GetSQLDBConnection())
                {
                    Helper.FillFromQuery(DT, query, con, parameters);
                }

                if (DT != null && DT.Rows.Count > 0)
                {
                    startDate = DateTime.Parse(DT.Rows[0][0].ToString());
                    adUserKey = Convert.ToInt32(DT.Rows[0][1]);
                    organisationStructureKey = Convert.ToInt32(DT.Rows[0][2]);
                }
                else
                    Assert.Fail("No data available");

                IUserOrganisationStructureHistory uosh = _mockery.StrictMock<IUserOrganisationStructureHistory>();
                IADUser ad = _mockery.StrictMock<IADUser>();
                IOrganisationStructure os = _mockery.StrictMock<IOrganisationStructure>();

                SetupResult.For(ad.Key).Return(adUserKey);
                SetupResult.For(os.Key).Return(organisationStructureKey);
                SetupResult.For(uosh.ChangeDate).Return(startDate);
                SetupResult.For(uosh.Action).Return(userOrgStructHistDelete);
                SetupResult.For(uosh.ADUser).Return(ad);
                SetupResult.For(uosh.OrganisationStructureKey).Return(os);

                ExecuteRule(rule, 1, uosh);
            }
        }

        [Test]
        public void UserOrganisationStructureEndDateCheckTestPass()
        {
            using (new SessionScope())
            {
                UserOrganisationStructureEndDateCheck rule = new UserOrganisationStructureEndDateCheck(RepositoryFactory.GetRepository<ICastleTransactionsService>());
                string query = @"select top 1 StartDate + 10 , aduserKey, OrganisationStructureKey from [2am].[dbo].[vUserOrganisationStructureHistory]
                                where EndDate is null
                                order by UserOrganisationStructureHistoryKey desc";

                ParameterCollection parameters = new ParameterCollection();
                DataTable DT = new DataTable();
                DateTime startDate = new DateTime();
                int adUserKey = -1;
                int organisationStructureKey = -1;
                char userOrgStructHistDelete = 'D';

                using (IDbConnection con = Helper.GetSQLDBConnection())
                {
                    Helper.FillFromQuery(DT, query, con, parameters);
                }

                if (DT != null && DT.Rows.Count > 0)
                {
                    startDate = DateTime.Parse(DT.Rows[0][0].ToString());
                    adUserKey = Convert.ToInt32(DT.Rows[0][1]);
                    organisationStructureKey = Convert.ToInt32(DT.Rows[0][2]);
                }
                else
                    Assert.Fail("No data available");

                IUserOrganisationStructureHistory uosh = _mockery.StrictMock<IUserOrganisationStructureHistory>();
                IADUser ad = _mockery.StrictMock<IADUser>();
                IOrganisationStructure os = _mockery.StrictMock<IOrganisationStructure>();

                SetupResult.For(ad.Key).Return(adUserKey);
                SetupResult.For(os.Key).Return(organisationStructureKey);
                SetupResult.For(uosh.ChangeDate).Return(startDate);
                SetupResult.For(uosh.Action).Return(userOrgStructHistDelete);
                SetupResult.For(uosh.ADUser).Return(ad);
                SetupResult.For(uosh.OrganisationStructureKey).Return(os);

                ExecuteRule(rule, 0, uosh);
            }
        }

        [Test]
        public void UserStatusMaintenanceAtLeastOneActiveUserTest()
        {
            UserStatusMaintenanceAtLeastOneActiveUser rule = new UserStatusMaintenanceAtLeastOneActiveUser(RepositoryFactory.GetRepository<ICastleTransactionsService>());
            object obj = null;
            using (DBHelper dbHelper = new DBHelper(Databases.TwoAM))
            {
                //Get an offerkey where the empirica score is below 0.
                string sqlPassRALess80 = "select top 1 ort.OfferRoleTypeKey " +
                                "from OfferRoleType ort (nolock) " +
                                "join OfferRoleTypeOrganisationStructureMapping ortosm (nolock) on ort.OfferRoleTypeKey = ortosm.OfferRoleTypeKey " +
                                "join OrganisationStructure os (nolock) on ortosm.OrganisationStructureKey = os.OrganisationStructureKey " +
                                "join UserOrganisationStructure uos (nolock) on os.OrganisationStructureKey = uos.OrganisationStructureKey " +
                                "join ADUser ad (nolock) on uos.ADUserKey = ad.ADUserKey " +
                                "where  " +
                                "ad.GeneralStatusKey = 1";

                using (IDbCommand cmd = dbHelper.CreateCommand(sqlPassRALess80))
                {
                    cmd.CommandTimeout = 180;

                    obj = dbHelper.ExecuteScalar(cmd);

                    if (obj != null)
                    {
                        using (new SessionScope())
                        {
                            ExecuteRule(rule, 0, (int)obj);
                        }
                    }
                }
            }
        }

        [Test]
        public void UserOrganisationStructureAllocationMandateCheckTestFail()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            UserOrganisationStructureAllocationMandateCheck rule = new UserOrganisationStructureAllocationMandateCheck();

            IUserOrganisationStructure uos = _mockery.StrictMock<IUserOrganisationStructure>();
            IADUser aduser = _mockery.StrictMock<IADUser>();
            IAllocationMandateSetGroup amsg = _mockery.StrictMock<IAllocationMandateSetGroup>();

            //
            IOrganisationStructureRepository orgRepo = _mockery.StrictMock<IOrganisationStructureRepository>();
            MockCache.Add(typeof(IOrganisationStructureRepository).ToString(), orgRepo);

            //
            SetupResult.For(amsg.AllocationGroupName).Return("AllocationGroupName");

            //
            SetupResult.For(aduser.ADUserName).Return("ADUserName");

            //
            SetupResult.For(uos.Key).Return(1);
            SetupResult.For(uos.ADUser).Return(aduser);

            //
            SetupResult.For(orgRepo.GetAllocationMandateSetGroupByUserOrganisationStructureKey(1)).IgnoreArguments().Return(amsg);

            //
            ExecuteRule(rule, 1, uos);
        }

        [Test]
        public void UserOrganisationStructureAllocationMandateCheckTestPass()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            UserOrganisationStructureAllocationMandateCheck rule = new UserOrganisationStructureAllocationMandateCheck();

            IUserOrganisationStructure uos = _mockery.StrictMock<IUserOrganisationStructure>();
            IADUser aduser = _mockery.StrictMock<IADUser>();
            IAllocationMandateSetGroup amsg = null;

            //
            IOrganisationStructureRepository orgRepo = _mockery.StrictMock<IOrganisationStructureRepository>();
            MockCache.Add(typeof(IOrganisationStructureRepository).ToString(), orgRepo);

            //
            SetupResult.For(aduser.ADUserName).Return("ADUserName");

            //
            SetupResult.For(uos.Key).Return(1);
            SetupResult.For(uos.ADUser).Return(aduser);

            //
            SetupResult.For(orgRepo.GetAllocationMandateSetGroupByUserOrganisationStructureKey(1)).IgnoreArguments().Return(amsg);

            //
            ExecuteRule(rule, 0, uos);
        }

        [Test]
        public void BranchUserSameBranchAsCurrentBranchConsultantTestPass()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 ofr.OfferKey, ad.ADUserKey
                from [2am].[dbo].offerRole ofr (nolock)
                join [2am].[dbo].legalEntity le (nolock)
	                on le.legalEntityKey = ofr.legalEntityKey
                join [2am].[dbo].aduser ad (nolock)
	                on ofr.LegalEntityKey = ad.LegalEntityKey
                join [2am].[dbo].UserOrganisationStructure uos (nolock)
	                on ad.ADUserKey = uos.ADUserKey
                join [2am].[dbo].OfferRoleTypeOrganisationStructureMapping ortosm (nolock)
	                on uos.OrganisationStructureKey = ortosm.OrganisationStructureKey
                where ofr.GeneralStatusKey = 1
	                and ofr.offerRoleTypeKey in (101,102,103)
	                and ortosm.offerRoleTypeKey in (101,102,103)";

                BranchUserSameBranchAsCurrentBranchConsultant rule = new BranchUserSameBranchAsCurrentBranchConsultant(RepositoryFactory.GetRepository<ICastleTransactionsService>());
                IOrganisationStructureRepository orgRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int offerKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                int aduserkey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                IApplication application = appRepo.GetApplicationByKey(offerKey);
                IADUser aduser = orgRepo.GetADUserByKey(aduserkey);
                ExecuteRule(rule, 0, application, aduser);
            }
        }

        [Test]
        public void BranchUserSameBranchAsCurrentBranchConsultantTestFail()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 ofr.OfferKey, uos1.ADUserKey
                from [2am].[dbo].offerRole ofr (nolock)
                join [2am].[dbo].legalEntity le (nolock)
	                on le.legalEntityKey = ofr.legalEntityKey
                join [2am].[dbo].aduser ad (nolock)
	                on ofr.LegalEntityKey = ad.LegalEntityKey
                join [2am].[dbo].UserOrganisationStructure uos (nolock)
	                on ad.ADUserKey = uos.ADUserKey
                join [2am].[dbo].OrganisationStructure os (nolock)
	                on os.OrganisationStructureKey = uos.OrganisationStructureKey
                join [2am].[dbo].OfferRoleTypeOrganisationStructureMapping ortosm (nolock)
	                on uos.OrganisationStructureKey = ortosm.OrganisationStructureKey
                join [2am].[dbo].OfferRoleTypeOrganisationStructureMapping ortosm1 (nolock)
	                on ortosm1.OfferRoleTypeKey = ortosm.OfferRoleTypeKey
	                and ortosm.OrganisationStructureKey <> ortosm1.OrganisationStructureKey
                join [2am].[dbo].UserOrganisationStructure uos1 (nolock)
	                on ortosm1.OrganisationStructureKey = uos1.OrganisationStructureKey
                join [2am].[dbo].OrganisationStructure os1 (nolock)
	                on os1.OrganisationStructureKey = uos1.OrganisationStructureKey and os1.ParentKey <> os.ParentKey
                where ofr.GeneralStatusKey = 1
	                and ofr.offerRoleTypeKey in (101,102,103)
	                and ortosm.offerRoleTypeKey in (101,102,103)";

                BranchUserSameBranchAsCurrentBranchConsultant rule = new BranchUserSameBranchAsCurrentBranchConsultant(RepositoryFactory.GetRepository<ICastleTransactionsService>());
                IOrganisationStructureRepository orgRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int offerKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                int aduserkey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                IApplication application = appRepo.GetApplicationByKey(offerKey);
                IADUser aduser = orgRepo.GetADUserByKey(aduserkey);
                ExecuteRule(rule, 1, application, aduser);
            }
        }
    }
}