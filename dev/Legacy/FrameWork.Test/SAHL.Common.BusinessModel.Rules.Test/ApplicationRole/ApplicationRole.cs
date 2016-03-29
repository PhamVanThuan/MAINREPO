using System;
using System.Collections.Generic;
using System.Data;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Rules.ApplicationRole;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Rules.Test.ApplicationRole
{
    [TestFixture]
    public class ApplicationRole : RuleBase
    {
        [SetUp]
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
        public void ReassignUserValidateLoggedInUserTestPass()
        {
            using (new SessionScope())
            {
                ReassignUserValidateLoggedInUser rule = new ReassignUserValidateLoggedInUser(RepositoryFactory.GetRepository<ICastleTransactionsService>());
                string testQuery = @";with res (AduserKey, ParentKey) as
                (select ad.AduserKey, os.ParentKey
                from [2am].[dbo].UserOrganisationStructure uos (nolock)
                inner join [2am].[dbo].OrganisationStructure os (nolock)
                    on uos.OrganisationStructureKey = os.OrganisationStructureKey
                inner join [2am].[dbo].ADUser ad (nolock)
                    on uos.ADUserKey = ad.ADUserKey)
                select top 1 res.aduserKey,ofr.OfferRoleKey
                from res
                inner join [2am].[dbo].OrganisationStructure os (nolock)
                    on os.ParentKey = res.ParentKey
                inner join [2am].[dbo].UserOrganisationStructure uos (nolock)
                    on uos.OrganisationStructureKey = os.OrganisationStructureKey
                inner join [2am].[dbo].ADUser ad (nolock)
                    on uos.ADUserKey = ad.ADUserKey
                inner join [2am].[dbo].OfferRole ofr (nolock)
                    on ad.LegalEntityKey = ofr.LegalEntityKey and ofr.GeneralStatusKey = 1
                inner join dbo.OfferRoleTypeOrganisationStructureMapping ortosm
	                on ortosm.OrganisationStructureKey = uos.OrganisationStructureKey
                        and
                    ortosm.OfferRoleTypeKey = ofr.OfferRoleTypeKey
                where ofr.offerRoleTypeKey in (101)";
                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(testQuery, typeof(OrganisationStructure_DAO), new ParameterCollection());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int AduserKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    int OfferRoleKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);

                    IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                    IOrganisationStructureRepository osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                    IApplicationRole appRole = appRepo.GetApplicationRoleByKey(OfferRoleKey);
                    IOrganisationStructure os = osRepo.GetOrganisationStructForADUser(appRole);
                    IADUser adUser = osRepo.GetADUserByKey(AduserKey);

                    // Pass
                    ExecuteRule(rule, 0, appRole, adUser);
                }
            }
        }

        [Test]
        public void ReassignUserValidateLoggedInUserTestFail()
        {
            using (new SessionScope())
            {
                ReassignUserValidateLoggedInUser rule = new ReassignUserValidateLoggedInUser(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                string testQuery = @";with res (AduserKey, ParentKey) as
                (select ad.AduserKey, os.ParentKey
                from [2am].[dbo].UserOrganisationStructure uos (nolock)
                inner join [2am].[dbo].OrganisationStructure os (nolock)
	                on uos.OrganisationStructureKey = os.OrganisationStructureKey
                inner join [2am].[dbo].ADUser ad (nolock)
	                on uos.ADUserKey = ad.ADUserKey)
                select top 1 res.aduserKey,ofr.OfferRoleKey
                from res
                inner join [2am].[dbo].OrganisationStructure os (nolock)
	                on os.ParentKey <> res.ParentKey
                inner join [2am].[dbo].UserOrganisationStructure uos (nolock)
	                on uos.OrganisationStructureKey = os.OrganisationStructureKey
                inner join [2am].[dbo].ADUser ad (nolock)
	                on uos.ADUserKey = ad.ADUserKey
                inner join [2am].[dbo].OfferRole ofr (nolock)
	                on ad.LegalEntityKey = ofr.LegalEntityKey and ofr.GeneralStatusKey = 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(testQuery, typeof(OrganisationStructure_DAO), new ParameterCollection());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int AduserKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    int OfferRoleKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);

                    IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                    IOrganisationStructureRepository osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                    IApplicationRole appRole = appRepo.GetApplicationRoleByKey(OfferRoleKey);
                    IOrganisationStructure os = osRepo.GetOrganisationStructForADUser(appRole);
                    IADUser adUser = osRepo.GetADUserByKey(AduserKey);

                    // Fail
                    ExecuteRule(rule, 1, appRole, adUser);
                }
            }
        }

        [Test]
        public void OfferroleMatchAccountroleandLEKeySuretyCheckTestPass()
        {
            using (new SessionScope())
            {
                OfferroleMatchAccountroleandLEKeySuretyCheck rule = new OfferroleMatchAccountroleandLEKeySuretyCheck();
                int applicationKey = -1;
                IApplication application = _mockery.StrictMock<IApplication>();

                string sql = @"select top 1 ofr.OfferKey
                from [2am].[dbo].offer o (nolock)
                inner join [2am].[dbo].offerRole ofr (nolock)
	                on ofr.offerKey = o.offerKey
                inner join [2am].[dbo].offerRoleType ort (nolock)
	                on ofr.OfferRoleTypeKey = ort.OfferRoleTypeKey
                inner join [2am].[dbo].RoleType rt (nolock)
	                on ort.description = rt.description
                left join [2am].[dbo].role r (nolock)
	                on ofr.legalEntityKey = r.legalEntityKey
                    and o.accountKey = r.accountKey and rt.RoleTypeKey = r.RoleTypeKey
                where ort.OfferRoleTypeGroupKey = 3 and r.LegalEntityKey is not null";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                if (o != null)
                    applicationKey = Convert.ToInt32(o);

                SetupResult.For(application.Key).Return(applicationKey);

                ExecuteRule(rule, 0, application);
            }
        }

        [Test]
        public void OfferroleMatchAccountroleandLEKeySuretyCheckTestFail()
        {
            using (new SessionScope())
            {
                OfferroleMatchAccountroleandLEKeySuretyCheck rule = new OfferroleMatchAccountroleandLEKeySuretyCheck();
                int applicationKey = -1;
                IApplication application = _mockery.StrictMock<IApplication>();

                string sql = @"select top 1 ofr.OfferKey
                from [2am].[dbo].offer o (nolock)
                inner join [2am].[dbo].offerRole ofr (nolock)
	                on ofr.offerKey = o.offerKey
                inner join [2am].[dbo].offerRoleType ort (nolock)
	                on ofr.OfferRoleTypeKey = ort.OfferRoleTypeKey
                inner join [2am].[dbo].RoleType rt (nolock)
	                on ort.description = rt.description
                left join [2am].[dbo].role r (nolock)
	                on ofr.legalEntityKey = r.legalEntityKey
                    and o.accountKey = r.accountKey and rt.RoleTypeKey = r.RoleTypeKey
                where ort.OfferRoleTypeGroupKey = 3 and r.LegalEntityKey is null";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                if (o != null)
                    applicationKey = Convert.ToInt32(o);

                SetupResult.For(application.Key).Return(applicationKey);

                ExecuteRule(rule, 1, application);
            }
        }

        [Test]
        public void CheckFurtherLendingApplicationRoleBeforeUpdateTestPass()
        {
            CheckFurtherLendingApplicationRoleBeforeUpdate rule = new CheckFurtherLendingApplicationRoleBeforeUpdate();
            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            IApplication application = _mockery.StrictMock<IApplication>();
            IAccount account = _mockery.StrictMock<IAccount>();
            IRole role = _mockery.StrictMock<IRole>();
            IRoleType roleType = _mockery.StrictMock<IRoleType>();
            ILegalEntity leAppRole = _mockery.StrictMock<ILegalEntity>();
            ILegalEntity leAccRole = _mockery.StrictMock<ILegalEntity>();
            IEventList<IRole> roles = new EventList<IRole>();

            //
            SetupResult.For(leAppRole.Key).Return(1);
            SetupResult.For(leAccRole.Key).Return(2);

            //
            SetupResult.For(appRoleType.Description).Return(OfferRoleTypes.MainApplicant.ToString());
            SetupResult.For(appRole.Key).Return(1);
            SetupResult.For(appRole.ApplicationRoleType).Return(appRoleType);
            SetupResult.For(appRole.LegalEntity).Return(leAppRole);
            SetupResult.For(appRole.Application).Return(application);
            SetupResult.For(application.Account).Return(account);
            SetupResult.For(appType.Key).Return((int)OfferTypes.ReAdvance);
            SetupResult.For(application.ApplicationType).Return(appType);
            SetupResult.For(application.Key).Return(1);

            //
            IGeneralStatus status = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(status.Key).Return(1);
            SetupResult.For(appRole.GeneralStatus).Return(status);
            SetupResult.For(role.GeneralStatus).Return(status);

            //
            SetupResult.For(roleType.Description).Return(RoleTypes.Suretor.ToString());
            SetupResult.For(role.RoleType).Return(roleType);
            SetupResult.For(role.LegalEntity).Return(leAccRole);
            roles.Add(null, role);
            SetupResult.For(account.Roles).Return(roles);
            ExecuteRule(rule, 0, appRole);
        }

        [Test]
        public void CheckFurtherLendingApplicationRoleBeforeUpdateTestFail()
        {
            CheckFurtherLendingApplicationRoleBeforeUpdate rule = new CheckFurtherLendingApplicationRoleBeforeUpdate();
            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            IApplication application = _mockery.StrictMock<IApplication>();
            IAccount account = _mockery.StrictMock<IAccount>();
            IRole role = _mockery.StrictMock<IRole>();
            IRoleType roleType = _mockery.StrictMock<IRoleType>();
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            IEventList<IRole> roles = new EventList<IRole>();

            //
            SetupResult.For(legalEntity.Key).Return(1);
            SetupResult.For(appRoleType.Description).Return(OfferRoleTypes.MainApplicant.ToString());
            SetupResult.For(appRole.Key).Return(1);
            SetupResult.For(appRole.ApplicationRoleType).Return(appRoleType);
            SetupResult.For(appRole.LegalEntity).Return(legalEntity);
            SetupResult.For(appRole.Application).Return(application);
            SetupResult.For(application.Account).Return(account);
            SetupResult.For(appType.Key).Return((int)OfferTypes.ReAdvance);
            SetupResult.For(application.ApplicationType).Return(appType);
            SetupResult.For(application.Key).Return(1);

            //
            IGeneralStatus status = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(status.Key).Return(1);
            SetupResult.For(appRole.GeneralStatus).Return(status);
            SetupResult.For(role.GeneralStatus).Return(status);

            //
            SetupResult.For(roleType.Description).Return(RoleTypes.MainApplicant.ToString());
            SetupResult.For(role.RoleType).Return(roleType);
            SetupResult.For(role.LegalEntity).Return(legalEntity);
            roles.Add(null, role);
            SetupResult.For(account.Roles).Return(roles);
            ExecuteRule(rule, 1, appRole);
        }

        [Test]
        public void CheckFurtherLendingApplicationRoleBeforeDeleteTestPass()
        {
            CheckFurtherLendingApplicationRoleBeforeDelete rule = new CheckFurtherLendingApplicationRoleBeforeDelete();
            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            IApplication application = _mockery.StrictMock<IApplication>();
            IAccount account = _mockery.StrictMock<IAccount>();
            IRole role = _mockery.StrictMock<IRole>();
            IRoleType roleType = _mockery.StrictMock<IRoleType>();
            ILegalEntity leAppRole = _mockery.StrictMock<ILegalEntity>();
            ILegalEntity leAccRole = _mockery.StrictMock<ILegalEntity>();
            IEventList<IRole> roles = new EventList<IRole>();

            //
            SetupResult.For(leAppRole.Key).Return(1);
            SetupResult.For(leAccRole.Key).Return(2);
            SetupResult.For(appRoleType.Description).Return(OfferRoleTypes.MainApplicant.ToString());
            SetupResult.For(appRole.Key).Return(1);
            SetupResult.For(appRole.ApplicationRoleType).Return(appRoleType);
            SetupResult.For(appRole.LegalEntity).Return(leAppRole);
            SetupResult.For(appRole.Application).Return(application);
            SetupResult.For(application.Account).Return(account);
            SetupResult.For(appType.Key).Return((int)OfferTypes.ReAdvance);
            SetupResult.For(application.ApplicationType).Return(appType);
            SetupResult.For(application.Key).Return(1);

            //
            IGeneralStatus status = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(status.Key).Return(1);
            SetupResult.For(appRole.GeneralStatus).Return(status);
            SetupResult.For(role.GeneralStatus).Return(status);

            //
            SetupResult.For(roleType.Description).Return(RoleTypes.Suretor.ToString());
            SetupResult.For(role.RoleType).Return(roleType);
            SetupResult.For(role.LegalEntity).Return(leAccRole);
            roles.Add(null, role);
            SetupResult.For(account.Roles).Return(roles);
            ExecuteRule(rule, 0, appRole);
        }

        [Test]
        public void CheckFurtherLendingApplicationRoleBeforeDeleteTestFail()
        {
            CheckFurtherLendingApplicationRoleBeforeDelete rule = new CheckFurtherLendingApplicationRoleBeforeDelete();
            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            IApplication application = _mockery.StrictMock<IApplication>();
            IAccount account = _mockery.StrictMock<IAccount>();
            IRole role = _mockery.StrictMock<IRole>();
            IRoleType roleType = _mockery.StrictMock<IRoleType>();
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            IEventList<IRole> roles = new EventList<IRole>();

            //
            SetupResult.For(legalEntity.Key).Return(1);
            SetupResult.For(appRoleType.Description).Return(OfferRoleTypes.MainApplicant.ToString());
            SetupResult.For(appRole.Key).Return(1);
            SetupResult.For(appRole.ApplicationRoleType).Return(appRoleType);
            SetupResult.For(appRole.LegalEntity).Return(legalEntity);
            SetupResult.For(appRole.Application).Return(application);
            SetupResult.For(application.Account).Return(account);
            SetupResult.For(appType.Key).Return((int)OfferTypes.ReAdvance);
            SetupResult.For(application.ApplicationType).Return(appType);
            SetupResult.For(application.Key).Return(1);

            //
            IGeneralStatus status = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(status.Key).Return(1);
            SetupResult.For(appRole.GeneralStatus).Return(status);
            SetupResult.For(role.GeneralStatus).Return(status);

            //
            SetupResult.For(roleType.Description).Return(RoleTypes.MainApplicant.ToString());
            SetupResult.For(role.RoleType).Return(roleType);
            SetupResult.For(role.LegalEntity).Return(legalEntity);
            roles.Add(null, role);
            SetupResult.For(account.Roles).Return(roles);
            ExecuteRule(rule, 1, appRole);
        }

        /// <summary>
        /// Validate that the legal entity to be removed does not have a debit order in use
        /// </summary>
        [NUnit.Framework.Test]
        public void ValidateRemovedRoleDebitOrderTest()
        {
            int applicantKey = 1;
            int applicantBankAccountKey = 2;
            int otherLegalEntityKey = 3;
            int otherBankAccountKey = 4;

            //Pass : Legal Entity and Bank Account for Debit Order does not exist
            ValidateRemovedRoleDebitOrderHelper(applicantKey, applicantBankAccountKey, otherLegalEntityKey, otherBankAccountKey, 0);

            applicantKey = 1;
            applicantBankAccountKey = 2;
            otherLegalEntityKey = 1;
            otherBankAccountKey = 2;

            //Fail : Entity Keys are the same as well as the Bank Account Keys
            ValidateRemovedRoleDebitOrderHelper(applicantKey, applicantBankAccountKey, otherLegalEntityKey, otherBankAccountKey, 1);
        }

        /// <summary>
        /// Ensure that the entity to be removed does not have a debit order in use
        /// </summary>
        /// <param name="applicantKey"></param>
        /// <param name="applicantBankAccountKey"></param>
        /// <param name="otherKey"></param>
        /// <param name="otherBankAccountKey"></param>
        /// <param name="expectedErrorCount"></param>
        public void ValidateRemovedRoleDebitOrderHelper(int applicantKey, int applicantBankAccountKey, int otherKey, int otherBankAccountKey, int expectedErrorCount)
        {
            //Rule
            ValidateRemovedRoleDebitOrder rule = new ValidateRemovedRoleDebitOrder();

            //Basic Scenario Setup
            IApplication application = _mockery.StrictMock<IApplication>();
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();

            #region Applicant

            IApplicationRole applicantApplicationRole = _mockery.StrictMock<IApplicationRole>();
            ILegalEntity applicant = _mockery.StrictMock<ILegalEntity>();

            //Applicant Bank Account
            IBankAccount applicantBankAccount = _mockery.StrictMock<IBankAccount>();
            ILegalEntityBankAccount applicantLegalEntityBankAccount = _mockery.StrictMock<ILegalEntityBankAccount>();

            //Applicant Role Type
            IApplicationRoleType applicantApplicationRoleType = _mockery.StrictMock<IApplicationRoleType>();
            IApplicationRoleTypeGroup applicantApplicationRoleTypeGroup = _mockery.StrictMock<IApplicationRoleTypeGroup>();
            IApplicationDebitOrder applicantApplicantDebitOrder = _mockery.StrictMock<IApplicationDebitOrder>();

            #endregion Applicant

            #region Other

            //Other Legal Entity
            IApplicationRole otherApplicationRole = _mockery.StrictMock<IApplicationRole>();
            ILegalEntity other = _mockery.StrictMock<ILegalEntity>();

            //Other Bank Account
            IBankAccount otherBankAccount = _mockery.StrictMock<IBankAccount>();
            ILegalEntityBankAccount otherLegalEntityBankAccount = _mockery.StrictMock<ILegalEntityBankAccount>();

            //Other Role Type
            IApplicationRoleType otherApplicationRoleType = _mockery.StrictMock<IApplicationRoleType>();
            IApplicationRoleTypeGroup otherApplicationRoleTypeGroup = _mockery.StrictMock<IApplicationRoleTypeGroup>();
            IApplicationDebitOrder otherApplicationDebitOrder = _mockery.StrictMock<IApplicationDebitOrder>();

            #endregion Other

            //Setup the Other Application Role
            Stack<IApplicationRole> applicationRoles = new Stack<IApplicationRole>();
            applicationRoles.Push(otherApplicationRole);
            IReadOnlyEventList<IApplicationRole> applicationRolesList = new ReadOnlyEventList<IApplicationRole>(applicationRoles);

            //Setup Values

            //ApplicationRoleTypeGroup
            //Applicant
            SetupResult.For(applicantApplicationRoleTypeGroup.Key).Return((int)Globals.OfferRoleTypeGroups.Client);

            //Other
            SetupResult.For(otherApplicationRoleTypeGroup.Key).Return((int)Globals.OfferRoleTypeGroups.Client);

            //BankAccountKey
            //Applicant
            SetupResult.For(applicantBankAccount.Key).Return(applicantBankAccountKey);

            //Other
            SetupResult.For(otherBankAccount.Key).Return(otherBankAccountKey);

            //LegalEntityKey
            //Applicant
            SetupResult.For(applicant.Key).Return(applicantKey);

            //Other
            SetupResult.For(other.Key).Return(otherKey);

            //General Status
            SetupResult.For(generalStatus.Key).Return((int)Globals.GeneralStatuses.Active);

            //Linking
            //ApplicationRole  -->  Application
            SetupResult.For(applicantApplicationRole.Application).Return(application);

            //Application  -->  Application.ApplicationRoles
            SetupResult.For(application.ApplicationRoles).Return(applicationRolesList);

            //ApplicationRole  -->  Application  -->  ApplicationDebitOrders
            IEventList<IApplicationDebitOrder> applicationDebitOrders = new EventList<IApplicationDebitOrder>();
            applicationDebitOrders.Add(Messages, applicantApplicantDebitOrder);
            applicationDebitOrders.Add(Messages, otherApplicationDebitOrder);

            SetupResult.For(application.ApplicationDebitOrders).Return(applicationDebitOrders);

            //ApplicationDebitOrder  -->  BankAccount
            SetupResult.For(applicantApplicantDebitOrder.BankAccount).Return(applicantBankAccount);
            SetupResult.For(otherApplicationDebitOrder.BankAccount).Return(otherBankAccount);

            //ApplicationRole  -->  ApplicationRoleType
            SetupResult.For(applicantApplicationRole.ApplicationRoleType).Return(applicantApplicationRoleType);
            SetupResult.For(otherApplicationRole.ApplicationRoleType).Return(otherApplicationRoleType);

            //ApplicationRoleType  -->  ApplicationRoleTypeGroup
            SetupResult.For(applicantApplicationRoleType.ApplicationRoleTypeGroup).Return(applicantApplicationRoleTypeGroup);
            SetupResult.For(otherApplicationRoleType.ApplicationRoleTypeGroup).Return(applicantApplicationRoleTypeGroup);

            //LegalEntityBankAccount  -->  BankAccount
            SetupResult.For(applicantLegalEntityBankAccount.BankAccount).Return(applicantBankAccount);
            SetupResult.For(otherLegalEntityBankAccount.BankAccount).Return(otherBankAccount);

            //LegalEntity  -->  LegalEntityBankAccount
            IEventList<ILegalEntityBankAccount> applicantLegalEntityBankAccounts = new EventList<ILegalEntityBankAccount>();
            applicantLegalEntityBankAccounts.Add(Messages, applicantLegalEntityBankAccount);
            SetupResult.For(applicant.LegalEntityBankAccounts).Return(applicantLegalEntityBankAccounts);

            IEventList<ILegalEntityBankAccount> otherLegalEntityBankAccounts = new EventList<ILegalEntityBankAccount>();
            otherLegalEntityBankAccounts.Add(Messages, otherLegalEntityBankAccount);
            SetupResult.For(other.LegalEntityBankAccounts).Return(otherLegalEntityBankAccounts);

            //ApplicationRole  -->  LegalEntity
            SetupResult.For(applicantApplicationRole.LegalEntity).Return(applicant);
            SetupResult.For(otherApplicationRole.LegalEntity).Return(other);

            //General Status
            SetupResult.For(applicantApplicationRole.GeneralStatus).Return(generalStatus);
            SetupResult.For(otherApplicationRole.GeneralStatus).Return(generalStatus);

            ExecuteRule(rule, expectedErrorCount, applicantApplicationRole);
        }

        /// <summary>
        /// Pass : The application will not contain a duplicate role type group
        /// </summary>
        [NUnit.Framework.Test]
        public void ValidateUniqueClientRolePass()
        {
            ValidateUniqueClientRole rule = new ValidateUniqueClientRole();

            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);

            //New Application Role
            ILegalEntity newLegalEntity = _mockery.StrictMock<ILegalEntity>();
            IApplicationRole newApplicationRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType newApplicationRoleType = _mockery.StrictMock<IApplicationRoleType>();
            IApplicationRoleTypeGroup newApplicationRoleTypeGroup = _mockery.StrictMock<IApplicationRoleTypeGroup>();

            //Existing Application Role
            ILegalEntity existingLegalEntity = _mockery.StrictMock<ILegalEntity>();
            IApplicationRole existingApplicationRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType existingApplicationRoleType = _mockery.StrictMock<IApplicationRoleType>();
            IApplicationRoleTypeGroup existingApplicationRoleTypeGroup = _mockery.StrictMock<IApplicationRoleTypeGroup>();

            IApplication application = _mockery.StrictMock<IApplication>();

            //Legal Entity
            SetupResult.For(newLegalEntity.Key).Return(1);

            SetupResult.For(existingLegalEntity.Key).Return(2);

            //New Application Role Type Group
            SetupResult.For(newApplicationRoleTypeGroup.Key).Return((int)OfferRoleTypeGroups.Client);

            //Existing Application Role Type Group
            SetupResult.For(existingApplicationRoleTypeGroup.Key).Return((int)OfferRoleTypeGroups.Client);

            //Linking

            //GeneralStatus  -->  ApplicationRole
            SetupResult.For(newApplicationRole.GeneralStatus).Return(generalStatus);
            SetupResult.For(existingApplicationRole.GeneralStatus).Return(generalStatus);

            //LegalEntity  -->  ApplicationRole
            SetupResult.For(newApplicationRole.LegalEntity).Return(newLegalEntity);
            SetupResult.For(existingApplicationRole.LegalEntity).Return(existingLegalEntity);

            //ApplicationRoleTypeGroup  -->  ApplicationRoleType
            //Existing
            SetupResult.For(existingApplicationRoleType.ApplicationRoleTypeGroup).Return(existingApplicationRoleTypeGroup);

            //New
            SetupResult.For(newApplicationRoleType.ApplicationRoleTypeGroup).Return(newApplicationRoleTypeGroup);

            //ApplicationRoleType  -->  ApplicationRole
            //Existing
            SetupResult.For(existingApplicationRole.ApplicationRoleType).Return(existingApplicationRoleType);

            //New
            SetupResult.For(newApplicationRole.ApplicationRoleType).Return(newApplicationRoleType);

            //Application  -->  ApplicationRole
            Stack<IApplicationRole> applicationRoles = new Stack<IApplicationRole>();
            applicationRoles.Push(existingApplicationRole);
            IReadOnlyEventList<IApplicationRole> applicationRolesList = new ReadOnlyEventList<IApplicationRole>(applicationRoles);
            SetupResult.For(application.ApplicationRoles).Return(applicationRolesList);

            SetupResult.For(newApplicationRole.Application).Return(application);

            ExecuteRule(rule, 0, newApplicationRole);
        }

        /// <summary>
        /// Fail : The application will contain a duplicate role type group
        /// </summary>
        [NUnit.Framework.Test]
        public void ValidateUniqueClientRoleFail()
        {
            ValidateUniqueClientRole rule = new ValidateUniqueClientRole();

            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);

            //New Application Role
            ILegalEntity newLegalEntity = _mockery.StrictMock<ILegalEntity>();
            IApplicationRole newApplicationRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType newApplicationRoleType = _mockery.StrictMock<IApplicationRoleType>();
            IApplicationRoleTypeGroup newApplicationRoleTypeGroup = _mockery.StrictMock<IApplicationRoleTypeGroup>();

            //Existing Application Role
            ILegalEntity existingLegalEntity = _mockery.StrictMock<ILegalEntity>();
            IApplicationRole existingApplicationRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType existingApplicationRoleType = _mockery.StrictMock<IApplicationRoleType>();
            IApplicationRoleTypeGroup existingApplicationRoleTypeGroup = _mockery.StrictMock<IApplicationRoleTypeGroup>();

            IApplication application = _mockery.StrictMock<IApplication>();

            //Legal Entity
            SetupResult.For(newLegalEntity.Key).Return(1);

            SetupResult.For(existingLegalEntity.Key).Return(1);

            //New Application Role Type Group
            SetupResult.For(newApplicationRoleTypeGroup.Key).Return((int)OfferRoleTypeGroups.Client);

            //Existing Application Role Type Group
            SetupResult.For(existingApplicationRoleTypeGroup.Key).Return((int)OfferRoleTypeGroups.Client);

            //Linking

            //GeneralStatus  -->  ApplicationRole
            SetupResult.For(newApplicationRole.GeneralStatus).Return(generalStatus);
            SetupResult.For(existingApplicationRole.GeneralStatus).Return(generalStatus);

            //LegalEntity  -->  ApplicationRole
            SetupResult.For(newApplicationRole.LegalEntity).Return(newLegalEntity);
            SetupResult.For(existingApplicationRole.LegalEntity).Return(existingLegalEntity);

            //ApplicationRoleTypeGroup  -->  ApplicationRoleType
            //Existing
            SetupResult.For(existingApplicationRoleType.ApplicationRoleTypeGroup).Return(existingApplicationRoleTypeGroup);

            //New
            SetupResult.For(newApplicationRoleType.ApplicationRoleTypeGroup).Return(newApplicationRoleTypeGroup);

            //ApplicationRoleType  -->  ApplicationRole
            //Existing
            SetupResult.For(existingApplicationRole.ApplicationRoleType).Return(existingApplicationRoleType);

            //New
            SetupResult.For(newApplicationRole.ApplicationRoleType).Return(newApplicationRoleType);

            //Application  -->  ApplicationRole
            Stack<IApplicationRole> applicationRoles = new Stack<IApplicationRole>();
            applicationRoles.Push(existingApplicationRole);
            IReadOnlyEventList<IApplicationRole> applicationRolesList = new ReadOnlyEventList<IApplicationRole>(applicationRoles);
            SetupResult.For(application.ApplicationRoles).Return(applicationRolesList);

            SetupResult.For(newApplicationRole.Application).Return(application);

            ExecuteRule(rule, 1, newApplicationRole);
        }
    }
}