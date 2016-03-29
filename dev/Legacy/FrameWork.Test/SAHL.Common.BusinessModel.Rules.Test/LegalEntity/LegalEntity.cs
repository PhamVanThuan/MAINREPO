using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Rules.LegalEntity;
using SAHL.Common.BusinessModel.Rules.LegalEntityLogin;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.Test.LegalEntity
{
    [TestFixture]
    public class LegalEntity : LegalEntityBase
    {
        [SetUp]
        public new void Setup()
        {
            base.Setup();
            Messages = new DomainMessageCollection();
        }

        [TearDown]
        public new void TearDown()
        {
            base.TearDown();
        }

        #region LegalEntityMandatoryName

        [NUnit.Framework.Test]
        public void LegalEntityNaturalPersonMandatoryNamePassed()
        {
            LegalEntityMandatoryName rule = new LegalEntityMandatoryName();
            ILegalEntityNaturalPerson le = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            SetupResult.For(le.FirstNames).Return("Test");
            SetupResult.For(le.Surname).Return("Test");
            ExecuteRule(rule, 0, le);
        }

        [NUnit.Framework.Test]
        public void LegalEntityNaturalPersonMandatoryNameFailed()
        {
            LegalEntityMandatoryName rule = new LegalEntityMandatoryName();
            ILegalEntityNaturalPerson le = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            SetupResult.For(le.FirstNames).Return("");
            SetupResult.For(le.Surname).Return("");
            ExecuteRule(rule, 2, le);
        }

        #endregion LegalEntityMandatoryName

        #region LegalEntityNaturalPersonCOP

        /// <summary>
        /// Test expects the rule to throw an exception when an unexpected argument is passed.
        /// </summary>
        [NUnit.Framework.Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "The LegalEntityNaturalPersonCOP rule expects the following objects to be passed: ILegalEntityNaturalPerson, IMortgageLoanAccount, IApplicationMortgageLoan.")]
        public void LegalEntityNaturalPersonCOPCorrectArgumentsPassed()
        {
            LegalEntityNaturalPersonCOP rule = new LegalEntityNaturalPersonCOP();

            ILegalEntityCompany legalEntityCompany = _mockery.StrictMock<ILegalEntityCompany>();
            if (!_ruleService.Enabled)
                Assert.Fail("_ruleService is off: _ruleService.Enabled");
            ExecuteRule(rule, 1, legalEntityCompany);
        }

        [NUnit.Framework.Test]
        public void LegalEntityNaturalPersonCOPApplicationMortgageLoanNoSpouseTestFail()
        {
            LegalEntityNaturalPersonCOP rule = new LegalEntityNaturalPersonCOP();

            IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();
            ILegalEntityNaturalPerson hubby = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            IMaritalStatus marStatus = _mockery.StrictMock<IMaritalStatus>();
            SetupResult.For(hubby.MaritalStatus).Return(marStatus);
            SetupResult.For(marStatus.Key).Return((int)MaritalStatuses.MarriedCommunityofProperty);

            Stack<ILegalEntityNaturalPerson> naturalPersons = new Stack<ILegalEntityNaturalPerson>();
            naturalPersons.Push(hubby);
            IReadOnlyEventList<ILegalEntityNaturalPerson> leNaturalPersons = new ReadOnlyEventList<ILegalEntityNaturalPerson>(naturalPersons);
            SetupResult.For(appML.GetNaturalPersonLegalEntitiesByRoleType(null, null, GeneralStatusKey.Active)).IgnoreArguments().Return(leNaturalPersons);

            ExecuteRule(rule, 1, appML);
        }

        [NUnit.Framework.Test]
        public void LegalEntityNaturalPersonCOPApplicationMortgageLoanSpouseNotCOPTestFail()
        {
            LegalEntityNaturalPersonCOP rule = new LegalEntityNaturalPersonCOP();

            IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();

            ILegalEntityNaturalPerson hubby = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            IMaritalStatus marStatus = _mockery.StrictMock<IMaritalStatus>();
            SetupResult.For(hubby.MaritalStatus).Return(marStatus);
            SetupResult.For(marStatus.Key).Return((int)MaritalStatuses.MarriedCommunityofProperty);

            ILegalEntityNaturalPerson wife = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            IMaritalStatus marStatusWife = _mockery.StrictMock<IMaritalStatus>();
            SetupResult.For(wife.MaritalStatus).Return(marStatusWife);
            SetupResult.For(marStatusWife.Key).Return((int)MaritalStatuses.MarriedAnteNuptualContract);

            Stack<ILegalEntityNaturalPerson> naturalPersons = new Stack<ILegalEntityNaturalPerson>();
            naturalPersons.Push(hubby);
            naturalPersons.Push(wife);

            IReadOnlyEventList<ILegalEntityNaturalPerson> leNaturalPersons = new ReadOnlyEventList<ILegalEntityNaturalPerson>(naturalPersons);
            SetupResult.For(appML.GetNaturalPersonLegalEntitiesByRoleType(null, null, GeneralStatusKey.Active)).IgnoreArguments().Return(leNaturalPersons);
            ExecuteRule(rule, 1, appML);
        }

        [NUnit.Framework.Test]
        public void LegalEntityNaturalPersonCOPApplicationMortgageLoanSpousesCOPTestPass()
        {
            LegalEntityNaturalPersonCOP rule = new LegalEntityNaturalPersonCOP();

            IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();

            ILegalEntityNaturalPerson hubby = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            IMaritalStatus marStatus = _mockery.StrictMock<IMaritalStatus>();
            SetupResult.For(hubby.MaritalStatus).Return(marStatus);
            SetupResult.For(marStatus.Key).Return((int)MaritalStatuses.MarriedCommunityofProperty);

            ILegalEntityNaturalPerson wife = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            IMaritalStatus marStatusWife = _mockery.StrictMock<IMaritalStatus>();
            SetupResult.For(wife.MaritalStatus).Return(marStatusWife);
            SetupResult.For(marStatusWife.Key).Return((int)MaritalStatuses.MarriedCommunityofProperty);

            Stack<ILegalEntityNaturalPerson> naturalPersons = new Stack<ILegalEntityNaturalPerson>();
            naturalPersons.Push(hubby);
            naturalPersons.Push(wife);

            IReadOnlyEventList<ILegalEntityNaturalPerson> leNaturalPersons = new ReadOnlyEventList<ILegalEntityNaturalPerson>(naturalPersons);
            SetupResult.For(appML.GetNaturalPersonLegalEntitiesByRoleType(null, null, GeneralStatusKey.Active)).IgnoreArguments().Return(leNaturalPersons);
            ExecuteRule(rule, 0, appML);
        }

        /// <summary>
        ///
        /// </summary>
        [NUnit.Framework.Test]
        public void LegalEntityNaturalPersonCOPCOPPartnerExists()
        {
            LegalEntityNaturalPersonCOP rule = new LegalEntityNaturalPersonCOP();
            IEventList<IRole> roles = new EventList<IRole>();

            ILegalEntityNaturalPerson legalEntityNaturalPerson = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            ILegalEntityNaturalPerson childLegalEntityNaturalPerson = _mockery.StrictMock<ILegalEntityNaturalPerson>();

            // Setup LegalEntity.Key(s) (These need to be different)
            SetupResult.For(legalEntityNaturalPerson.Key).Return(1);
            SetupResult.For(childLegalEntityNaturalPerson.Key).Return(2);

            // Setup legalEntityNaturalPerson.MaritalStatus.Key
            IMaritalStatus maritalStatus = _mockery.StrictMock<IMaritalStatus>();
            SetupResult.For(maritalStatus.Key).Return(((int)MaritalStatuses.MarriedCommunityofProperty));
            SetupResult.For(legalEntityNaturalPerson.MaritalStatus).Return(maritalStatus);

            // Setup roles[0].Account
            IRole role = _mockery.StrictMock<IRole>();
            IMortgageLoanAccount account = _mockery.StrictMock<IMortgageLoanAccount>();
            SetupResult.For(role.Account).Return(account);
            IRoleType roleType = _mockery.StrictMock<IRoleType>();
            SetupResult.For(roleType.Key).Return((int)RoleTypes.MainApplicant);
            SetupResult.For(role.RoleType).Return(roleType);

            // Setup Account.Roles.LegalEntity and MaritalStatus
            SetupResult.For(role.LegalEntity).Return(childLegalEntityNaturalPerson);
            SetupResult.For(childLegalEntityNaturalPerson.MaritalStatus).Return(maritalStatus);

            // Setup Account.AccountStatus
            IAccountStatus accountStatus = _mockery.StrictMock<IAccountStatus>();
            SetupResult.For(accountStatus.Key).Return((int)AccountStatuses.Open);
            SetupResult.For(account.AccountStatus).Return(accountStatus);

            // Setup Account.Roles
            SetupResult.For(account.Roles).Return(roles);

            // Setup legalEntityNaturalPerson.Roles
            roles.Add(Messages, role);
            SetupResult.For(legalEntityNaturalPerson.Roles).Return(roles);

            ExecuteRule(rule, 0, legalEntityNaturalPerson);
        }

        #endregion LegalEntityNaturalPersonCOP

        #region LegalEntityNaturalPersonMinAge

        /// <summary>
        /// Test expects the rule to throw an exception when an unexpected argument is passed.
        /// </summary>
        [NUnit.Framework.Test]
        [ExpectedException(typeof(ArgumentException))]
        public void LegalEntityNaturalPersonMinAgeCorrectArgumentsPassed()
        {
            LegalEntityNaturalPersonMinAge rule = new LegalEntityNaturalPersonMinAge();

            ILegalEntityCompany legalEntityCompany = _mockery.StrictMock<ILegalEntityCompany>();

            ExecuteRule(rule, 0, legalEntityCompany);
        }

        /// <summary>
        /// Test expects the rule to throw an exception when an unexpected argument is passed.
        /// </summary>
        [NUnit.Framework.Test]
        public void LegalEntityNaturalPersonMinAgeSuccess()
        {
            LegalEntityNaturalPersonMinAge rule = new LegalEntityNaturalPersonMinAge();

            ILegalEntityNaturalPerson legalEntityNaturalPerson = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            SetupResult.For(legalEntityNaturalPerson.AgeNextBirthday).Return(32);

            ExecuteRule(rule, 0, legalEntityNaturalPerson);
        }

        /// <summary>
        /// Test expects the rule to throw an exception when an unexpected argument is passed.
        /// </summary>
        [NUnit.Framework.Test]
        public void LegalEntityNaturalPersonMinAgeFailure()
        {
            LegalEntityNaturalPersonMinAge rule = new LegalEntityNaturalPersonMinAge();

            ILegalEntityNaturalPerson legalEntityNaturalPerson = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            SetupResult.For(legalEntityNaturalPerson.AgeNextBirthday).Return(18);

            ExecuteRule(rule, 0, legalEntityNaturalPerson);
        }

        #endregion LegalEntityNaturalPersonMinAge

        #region LegalEntityNaturalPersonIDNumberDOB

        /// <summary>
        /// Test expects the rule to throw an exception when no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test]
        [ExpectedException(typeof(ArgumentException))]
        public void LegalEntityNaturalPersonIDNumberDOBFailureNoArgumentsPassed()
        {
            LegalEntityNaturalPersonIDNumberDOB rule = new LegalEntityNaturalPersonIDNumberDOB();

            ExecuteRule(rule, 0);
        }

        /// <summary>
        /// Test expects the rule to throw an exception when an unexpected argument is passed.
        /// </summary>
        [NUnit.Framework.Test]
        [ExpectedException(typeof(ArgumentException))]
        public void LegalEntityNaturalPersonIDNumberDOBFailureWrongArgumentsPassed()
        {
            LegalEntityNaturalPersonIDNumberDOB rule = new LegalEntityNaturalPersonIDNumberDOB();

            ILegalEntityCompany legalEntityCompany = _mockery.StrictMock<ILegalEntityCompany>();

            ExecuteRule(rule, 1, legalEntityCompany);
        }

        /// <summary>
        /// Test expects the rule to throw an exception when an unexpected argument is passed.
        /// </summary>
        [NUnit.Framework.Test]
        public void LegalEntityNaturalPersonIDNumberDOBSuccess()
        {
            LegalEntityNaturalPersonIDNumberDOB rule = new LegalEntityNaturalPersonIDNumberDOB();

            ILegalEntityNaturalPerson legalEntityNaturalPerson = _mockery.StrictMock<ILegalEntityNaturalPerson>();

            // Setup legalEntityNaturalPerson.CitizenType
            ICitizenType citizenType = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(citizenType.Key).Return((int)CitizenTypes.SACitizen);
            SetupResult.For(legalEntityNaturalPerson.CitizenType).Return(citizenType);

            // Setup Date of Birth
            SetupResult.For(legalEntityNaturalPerson.DateOfBirth).Return(new DateTime(1978, 08, 09));

            // Setup ID Number
            SetupResult.For(legalEntityNaturalPerson.IDNumber).Return("7808095297084");

            ExecuteRule(rule, 0, legalEntityNaturalPerson);
        }

        /// <summary>
        /// Test expects a .
        /// </summary>
        [NUnit.Framework.Test]
        public void LegalEntityNaturalPersonIDNumberDOBFailureMismatch()
        {
            LegalEntityNaturalPersonIDNumberDOB rule = new LegalEntityNaturalPersonIDNumberDOB();

            ILegalEntityNaturalPerson legalEntityNaturalPerson = _mockery.StrictMock<ILegalEntityNaturalPerson>();

            // Setup legalEntityNaturalPerson.CitizenType
            ICitizenType citizenType = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(citizenType.Key).Return((int)CitizenTypes.SACitizen);
            SetupResult.For(legalEntityNaturalPerson.CitizenType).Return(citizenType);

            // Setup Date of Birth
            SetupResult.For(legalEntityNaturalPerson.DateOfBirth).Return(new DateTime(1976, 08, 09));

            // Setup ID Number so that it doesn't match the DOB
            SetupResult.For(legalEntityNaturalPerson.IDNumber).Return("7808095297084");

            ExecuteRule(rule, 1, legalEntityNaturalPerson);
        }

        #endregion LegalEntityNaturalPersonIDNumberDOB

        #region DetermineLegalEntityUpdateOpenAccount

        /// <summary>
        /// Test expects the rule to throw an exception when no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test]
        [ExpectedException(typeof(ArgumentException))]
        public void DetermineLegalEntityUpdateOpenAccountFailureNoArgumentsPassed()
        {
            DetermineLegalEntityUpdateOpenAccount rule = new DetermineLegalEntityUpdateOpenAccount();

            ExecuteRule(rule, 0);
        }

        /// <summary>
        /// Test expects the rule to throw an exception when the wrong arguments are passed.
        /// </summary>
        [NUnit.Framework.Test]
        [ExpectedException(typeof(ArgumentException))]
        public void DetermineLegalEntityUpdateOpenAccountFailureWrongArgumentsPassed()
        {
            DetermineLegalEntityUpdateOpenAccount rule = new DetermineLegalEntityUpdateOpenAccount();

            // Create some arb un expected object and pass it to the function.
            IAccount account = _mockery.StrictMock<IAccount>();
            ExecuteRule(rule, 0, account, account);
        }

        /// <summary>
        /// Test expects the rule to throw an exception when arguments passed do not represent the same Legal Entity.
        /// </summary>
        [NUnit.Framework.Test]
        [ExpectedException(typeof(ArgumentException))]
        public void DetermineLegalEntityUpdateOpenAccountFailureMustPassSameLEData()
        {
            DetermineLegalEntityUpdateOpenAccount rule = new DetermineLegalEntityUpdateOpenAccount();

            // Setup two Legal Entities
            ILegalEntityNaturalPerson legalEntityBefore = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            ILegalEntityNaturalPerson legalEntityAfter = _mockery.StrictMock<ILegalEntityNaturalPerson>();

            // Setup legalEntityAfter.Key and legalEntityBefore.Key
            SetupResult.For(legalEntityBefore.Key).Return(1);
            SetupResult.For(legalEntityAfter.Key).Return(2);

            ExecuteRule(rule, 0);
        }

        /// <summary>
        /// Test expects the rule to fail if changes are made on Legal Entities with open accounts.
        /// </summary>
        [NUnit.Framework.Test]
        public void DetermineLegalEntityUpdateOpenAccountDisallowedNaturalPersonChanges()
        {
            DetermineLegalEntityUpdateOpenAccount rule = new DetermineLegalEntityUpdateOpenAccount();

            // Setup two Legal Entities
            ILegalEntityNaturalPerson legalEntityBefore = _mockery.StrictMock<ILegalEntityNaturalPerson>();
            ILegalEntityNaturalPerson legalEntityAfter = _mockery.StrictMock<ILegalEntityNaturalPerson>();

            // Setup legalEntityAfter.Key and legalEntityBefore.Key
            SetupResult.For(legalEntityBefore.Key).Return(1);
            SetupResult.For(legalEntityAfter.Key).Return(1);

            // Setup LegalEntityExceptionStatus
            SetupResult.For(legalEntityBefore.LegalEntityExceptionStatus).Return(null);
            SetupResult.For(legalEntityAfter.LegalEntityExceptionStatus).Return(null);

            // Setup a role with an active account
            IAccount account = _mockery.StrictMock<IAccount>();
            SetupResult.For(account.IsActive).Return(true);

            IRole role = _mockery.StrictMock<IRole>();
            SetupResult.For(role.Account).Return(account);

            IEventList<IRole> roles = new EventList<IRole>();
            roles.Add(Messages, role);

            SetupResult.For(legalEntityBefore.Roles).Return(roles);
            SetupResult.For(legalEntityAfter.Roles).Return(roles);

            // Setup all fields might be blocked
            ISalutation salutation = _mockery.StrictMock<ISalutation>();
            SetupResult.For(salutation.Key).Return(1);
            SetupResult.For(legalEntityBefore.Salutation).Return(salutation);
            SetupResult.For(legalEntityAfter.Salutation).Return(salutation);

            SetupResult.For(legalEntityBefore.Initials).Return("SR");
            SetupResult.For(legalEntityAfter.Initials).Return("SRM");

            SetupResult.For(legalEntityBefore.FirstNames).Return("Joe");
            SetupResult.For(legalEntityAfter.FirstNames).Return("Joe");

            SetupResult.For(legalEntityBefore.Surname).Return("Blog");
            SetupResult.For(legalEntityAfter.Surname).Return("Blog");

            SetupResult.For(legalEntityBefore.IDNumber).Return(String.Empty);
            SetupResult.For(legalEntityAfter.IDNumber).Return(String.Empty);

            SetupResult.For(legalEntityBefore.PassportNumber).Return(String.Empty);
            SetupResult.For(legalEntityAfter.PassportNumber).Return(String.Empty);

            IGender gender = _mockery.StrictMock<IGender>();
            SetupResult.For(gender.Key).Return(1);
            SetupResult.For(legalEntityBefore.Gender).Return(gender);
            SetupResult.For(legalEntityAfter.Gender).Return(gender);

            IMaritalStatus maritalStatus = _mockery.StrictMock<IMaritalStatus>();
            SetupResult.For(maritalStatus.Key).Return(1);
            SetupResult.For(legalEntityBefore.MaritalStatus).Return(maritalStatus);
            SetupResult.For(legalEntityAfter.MaritalStatus).Return(maritalStatus);

            ICitizenType citizenType = _mockery.StrictMock<ICitizenType>();
            SetupResult.For(citizenType.Key).Return(1);
            SetupResult.For(legalEntityBefore.CitizenType).Return(citizenType);
            SetupResult.For(legalEntityAfter.CitizenType).Return(citizenType);

            ExecuteRule(rule, 1, legalEntityBefore, legalEntityAfter);
        }

        #endregion DetermineLegalEntityUpdateOpenAccount

        #region MortgageLoanAccountLegalEntityRoleMainApplicant

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountLegalEntityRoleMainApplicantAccountSuccess()
        {
            MortgageLoanAccountLegalEntityRoleMainApplicant rule = new MortgageLoanAccountLegalEntityRoleMainApplicant();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.Roles
            IEventList<IRole> roles = new EventList<IRole>();
            SetupResult.For(mortgageLoanAccount.Roles).Return(roles);
            IRole role = _mockery.StrictMock<IRole>();
            roles.Add(Messages, role);

            // Setup role.RoleType.Key
            IRoleType roleType = _mockery.StrictMock<IRoleType>();
            SetupResult.For(role.RoleType).Return(roleType);
            SetupResult.For(roleType.Key).Return((int)RoleTypes.MainApplicant);

            // Setup role.GeneralStatus.Key
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(role.GeneralStatus).Return(generalStatus);
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);

            ExecuteRule(rule, 0, mortgageLoanAccount);
        }

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountLegalEntityRoleMainApplicantAccountFail()
        {
            MortgageLoanAccountLegalEntityRoleMainApplicant rule = new MortgageLoanAccountLegalEntityRoleMainApplicant();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.Roles
            IEventList<IRole> roles = new EventList<IRole>();
            SetupResult.For(mortgageLoanAccount.Roles).Return(roles);
            IRole role = _mockery.StrictMock<IRole>();
            roles.Add(Messages, role);

            // Setup role.RoleType.Key
            IRoleType roleType = _mockery.StrictMock<IRoleType>();
            SetupResult.For(role.RoleType).Return(roleType);
            SetupResult.For(roleType.Key).Return((int)RoleTypes.MainApplicant);

            // Setup role.GeneralStatus.Key
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(role.GeneralStatus).Return(generalStatus);
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Inactive);

            ExecuteRule(rule, 1, mortgageLoanAccount);
        }

        #endregion MortgageLoanAccountLegalEntityRoleMainApplicant

        #region MortgageLoanAccountLegalEntityRoleCompanyMainApplicant

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountLegalEntityRoleCompanyMainApplicantAccountSuccess()
        {
            MortgageLoanAccountLegalEntityRoleCompanyMainApplicant rule = new MortgageLoanAccountLegalEntityRoleCompanyMainApplicant();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.Roles
            IEventList<IRole> roles = new EventList<IRole>();
            SetupResult.For(mortgageLoanAccount.Roles).Return(roles);
            IRole role = _mockery.StrictMock<IRole>();
            roles.Add(Messages, role);

            // Setup role.LegalEntity.LegalEntityType.Key
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(role.LegalEntity).Return(legalEntity);
            ILegalEntityType legalEntityType = _mockery.StrictMock<ILegalEntityType>();
            SetupResult.For(legalEntity.LegalEntityType).Return(legalEntityType);
            SetupResult.For(legalEntityType.Key).Return((int)LegalEntityTypes.Trust);

            // Setup role.RoleType.Key
            IRoleType roleType = _mockery.StrictMock<IRoleType>();
            SetupResult.For(role.RoleType).Return(roleType);
            SetupResult.For(roleType.Key).Return((int)RoleTypes.MainApplicant);

            // Setup role.GeneralStatus.Key
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(role.GeneralStatus).Return(generalStatus);
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);

            ExecuteRule(rule, 0, mortgageLoanAccount);
        }

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountLegalEntityRoleCompanyMainApplicantAccountFail()
        {
            MortgageLoanAccountLegalEntityRoleCompanyMainApplicant rule = new MortgageLoanAccountLegalEntityRoleCompanyMainApplicant();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.Roles
            IEventList<IRole> roles = new EventList<IRole>();
            SetupResult.For(mortgageLoanAccount.Roles).Return(roles);
            IRole role = _mockery.StrictMock<IRole>();
            roles.Add(Messages, role);

            // Setup role.LegalEntity.LegalEntityType.Key
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(role.LegalEntity).Return(legalEntity);
            ILegalEntityType legalEntityType = _mockery.StrictMock<ILegalEntityType>();
            SetupResult.For(legalEntity.LegalEntityType).Return(legalEntityType);
            SetupResult.For(legalEntityType.Key).Return((int)LegalEntityTypes.Trust);

            // Setup role.RoleType.Key
            IRoleType roleType = _mockery.StrictMock<IRoleType>();
            SetupResult.For(role.RoleType).Return(roleType);
            SetupResult.For(roleType.Key).Return((int)RoleTypes.Suretor);

            // Setup role.GeneralStatus.Key
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(role.GeneralStatus).Return(generalStatus);
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);

            ExecuteRule(rule, 1, mortgageLoanAccount);
        }

        [NUnit.Framework.Test]
        public void MortgageLoanAccountLegalEntityRoleCompanyMainApplicantAppMLCoSuretorTestFail()
        {
            MortgageLoanAccountLegalEntityRoleCompanyMainApplicant rule = new MortgageLoanAccountLegalEntityRoleCompanyMainApplicant();

            IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();

            // Setup applicationMortgageLoan.ApplicationRoles
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            IGeneralStatus gs = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(applicationRole.GeneralStatus).Return(gs);
            SetupResult.For(gs.Key).Return((int)GeneralStatuses.Active);

            SetupResult.For(applicationRole.ApplicationRoleType).Return(appRoleType);
            SetupResult.For(appRoleType.Key).Return((int)OfferRoleTypes.LeadSuretor);

            ILegalEntity le = _mockery.StrictMock<ILegalEntity>();
            ILegalEntityType leType = _mockery.StrictMock<ILegalEntityType>();

            SetupResult.For(applicationRole.LegalEntity).Return(le);
            SetupResult.For(le.LegalEntityType).Return(leType);
            SetupResult.For(leType.Key).Return((int)LegalEntityTypes.Company);

            Stack<IApplicationRole> appRoles = new Stack<IApplicationRole>();
            appRoles.Push(applicationRole);

            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(appRoles);
            SetupResult.For(appML.ApplicationRoles).Return(applicationRoles);

            ExecuteRule(rule, 1, appML);
        }

        [NUnit.Framework.Test]
        public void MortgageLoanAccountLegalEntityRoleCompanyMainApplicantAppMLCoMainAppTestPass()
        {
            MortgageLoanAccountLegalEntityRoleCompanyMainApplicant rule = new MortgageLoanAccountLegalEntityRoleCompanyMainApplicant();

            IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();

            // Setup applicationMortgageLoan.ApplicationRoles
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            IGeneralStatus gs = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(applicationRole.GeneralStatus).Return(gs);
            SetupResult.For(gs.Key).Return((int)GeneralStatuses.Active);

            SetupResult.For(applicationRole.ApplicationRoleType).Return(appRoleType);
            SetupResult.For(appRoleType.Key).Return((int)OfferRoleTypes.MainApplicant);

            ILegalEntity le = _mockery.StrictMock<ILegalEntity>();
            ILegalEntityType leType = _mockery.StrictMock<ILegalEntityType>();

            SetupResult.For(applicationRole.LegalEntity).Return(le);
            SetupResult.For(le.LegalEntityType).Return(leType);
            SetupResult.For(leType.Key).Return((int)LegalEntityTypes.Company);

            Stack<IApplicationRole> appRoles = new Stack<IApplicationRole>();
            appRoles.Push(applicationRole);

            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(appRoles);
            SetupResult.For(appML.ApplicationRoles).Return(applicationRoles);

            ExecuteRule(rule, 0, appML);
        }

        [NUnit.Framework.Test]
        public void MortgageLoanAccountLegalEntityRoleCompanyMainApplicantAppMLNatPersonMainAppTestPass()
        {
            MortgageLoanAccountLegalEntityRoleCompanyMainApplicant rule = new MortgageLoanAccountLegalEntityRoleCompanyMainApplicant();

            IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();

            // Setup applicationMortgageLoan.ApplicationRoles
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            IGeneralStatus gs = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(applicationRole.GeneralStatus).Return(gs);
            SetupResult.For(gs.Key).Return((int)GeneralStatuses.Active);

            SetupResult.For(applicationRole.ApplicationRoleType).Return(appRoleType);
            SetupResult.For(appRoleType.Key).Return((int)OfferRoleTypes.Suretor);

            ILegalEntity le = _mockery.StrictMock<ILegalEntity>();
            ILegalEntityType leType = _mockery.StrictMock<ILegalEntityType>();

            SetupResult.For(applicationRole.LegalEntity).Return(le);
            SetupResult.For(le.LegalEntityType).Return(leType);
            SetupResult.For(leType.Key).Return((int)LegalEntityTypes.NaturalPerson);

            Stack<IApplicationRole> appRoles = new Stack<IApplicationRole>();
            appRoles.Push(applicationRole);

            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(appRoles);
            SetupResult.For(appML.ApplicationRoles).Return(applicationRoles);

            ExecuteRule(rule, 0, appML);
        }

        #endregion MortgageLoanAccountLegalEntityRoleCompanyMainApplicant

        #region MortgageLoanAccountLegalEntityRoleCompanyMainApplicantQuantity

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountLegalEntityRoleCompanyMainApplicantQuantityAccountSuccess()
        {
            MortgageLoanAccountLegalEntityRoleCompanyMainApplicantQuantity rule = new MortgageLoanAccountLegalEntityRoleCompanyMainApplicantQuantity();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.Roles
            IEventList<IRole> roles = new EventList<IRole>();
            SetupResult.For(mortgageLoanAccount.Roles).Return(roles);
            IRole role = _mockery.StrictMock<IRole>();
            roles.Add(Messages, role);

            // Setup role.LegalEntity.LegalEntityType.Key
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(role.LegalEntity).Return(legalEntity);
            ILegalEntityType legalEntityType = _mockery.StrictMock<ILegalEntityType>();
            SetupResult.For(legalEntity.LegalEntityType).Return(legalEntityType);
            SetupResult.For(legalEntityType.Key).Return((int)LegalEntityTypes.Trust);

            // Setup role.RoleType.Key
            IRoleType roleType = _mockery.StrictMock<IRoleType>();
            SetupResult.For(role.RoleType).Return(roleType);
            SetupResult.For(roleType.Key).Return((int)RoleTypes.MainApplicant);

            // Setup role.GeneralStatus.Key
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(role.GeneralStatus).Return(generalStatus);
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);

            ExecuteRule(rule, 0, mortgageLoanAccount);
        }

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountLegalEntityRoleCompanyMainApplicantQuantityAccountFail()
        {
            MortgageLoanAccountLegalEntityRoleCompanyMainApplicantQuantity rule = new MortgageLoanAccountLegalEntityRoleCompanyMainApplicantQuantity();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.Roles
            IEventList<IRole> roles = new EventList<IRole>();
            SetupResult.For(mortgageLoanAccount.Roles).Return(roles);
            IRole role = _mockery.StrictMock<IRole>();
            roles.Add(Messages, role);
            roles.Add(Messages, role); // Inserting two roles of Company Main Applicant

            // Setup role.LegalEntity.LegalEntityType.Key
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(role.LegalEntity).Return(legalEntity);
            ILegalEntityType legalEntityType = _mockery.StrictMock<ILegalEntityType>();
            SetupResult.For(legalEntity.LegalEntityType).Return(legalEntityType);
            SetupResult.For(legalEntityType.Key).Return((int)LegalEntityTypes.Trust);

            // Setup role.RoleType.Key
            IRoleType roleType = _mockery.StrictMock<IRoleType>();
            SetupResult.For(role.RoleType).Return(roleType);
            SetupResult.For(roleType.Key).Return((int)RoleTypes.MainApplicant);

            // Setup role.GeneralStatus.Key
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(role.GeneralStatus).Return(generalStatus);
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);

            ExecuteRule(rule, 1, mortgageLoanAccount);
        }

        #endregion MortgageLoanAccountLegalEntityRoleCompanyMainApplicantQuantity

        #region LegalEntityOriginationSource

        /// <summary>
        /// All LegalEntityOriginationSource tests
        /// </summary>
        [NUnit.Framework.Test]
        public void LegalEntityOriginationSource()
        {
            LegalEntityOriginationSource rule = new LegalEntityOriginationSource(RepositoryFactory.GetRepository<ICastleTransactionsService>());

            using (new TransactionScope())
            {
                IDbConnection con = Helper.GetSQLDBConnection();
                try
                {
                    //Fail on an RCS Account LE

                    #region RCSAccFail

                    #region OldMockTest

                    //// Setup the parameter objects to pass along
                    //ILegalEntityNaturalPerson legalEntity = _mockery.StrictMock<ILegalEntityNaturalPerson>();

                    //// Setup originationsource of account roles
                    //IOriginationSource originationSource = _mockery.StrictMock<IOriginationSource>();
                    //SetupResult.For(originationSource.Key).Return((int)SAHL.Common.Globals.OriginationSources.RCS);
                    //SetupResult.For(originationSource.Description).Return("RCS");

                    //// Setup legalEntity properties
                    //ILegalEntityType legalEntityType = _mockery.StrictMock<ILegalEntityType>();
                    //SetupResult.For(legalEntity.LegalEntityType).Return(legalEntityType);
                    //SetupResult.For(legalEntityType.Key).Return((int)LegalEntityTypes.NaturalPerson);
                    //SetupResult.For(legalEntity.GetLegalName(LegalNameFormat.Full)).Return("Mr. Craig Graham Fraser");
                    //SetupResult.For(legalEntity.IDNumber).Return("6812095219087");

                    //// Setup LegalEntity Account Roles
                    //IEventList<IRole> roles = new EventList<IRole>();
                    //SetupResult.For(legalEntity.Roles).Return(roles);
                    //IRole role = _mockery.StrictMock<IRole>();
                    //roles.Add(Messages, role);

                    //// Setup account
                    //IAccount account = _mockery.StrictMock<IAccount>();
                    //SetupResult.For(role.Account).Return(account);
                    //// Setup origination source of account
                    //SetupResult.For(account.OriginationSource).Return(originationSource);
                    //// Setup status of account
                    //IAccountStatus accountStatus = _mockery.StrictMock<IAccountStatus>();
                    //SetupResult.For(account.AccountStatus).Return(accountStatus);
                    //SetupResult.For(accountStatus.Key).Return((int)SAHL.Common.Globals.AccountStatuses.Open);
                    //// Setup product of the account
                    //IProduct product = _mockery.StrictMock<IProduct>();
                    //SetupResult.For(account.Product).Return(product);
                    //SetupResult.For(product.Description).Return("Variable Loan");

                    #endregion OldMockTest

                    string sqlQueryFailAccount = "select top 1 r.LegalEntityKey from Account a (nolock) " +
                        "join [Role] r (nolock) on a.AccountKey = r.AccountKey " +
                        "where a.RRR_OriginationSourceKey = 4 and a.AccountStatusKey = 1";

                    object obj = Helper.ExecuteScalar(con, sqlQueryFailAccount);

                    if (obj != null)
                    {
                        ILegalEntity le = LERepo.GetLegalEntityByKey((int)obj);

                        ExecuteRule(rule, 1, le);
                    }

                    #endregion RCSAccFail

                    //Fail on an RCS Application LE

                    #region RCSAppFail

                    #region OldMockTest

                    // Setup the parameter objects to pass along
                    //ILegalEntityNaturalPerson legalEntity = _mockery.StrictMock<ILegalEntityNaturalPerson>();

                    //// Setup originationsource of account roles
                    //IOriginationSource accountOriginationSource = _mockery.StrictMock<IOriginationSource>();
                    //SetupResult.For(accountOriginationSource.Key).Return((int)SAHL.Common.Globals.OriginationSources.SAHomeLoans);
                    //SetupResult.For(accountOriginationSource.Description).Return("SA Homeloans");

                    //// Setup originationsource of application roles
                    //IOriginationSource originationSource = _mockery.StrictMock<IOriginationSource>();
                    //SetupResult.For(originationSource.Key).Return((int)SAHL.Common.Globals.OriginationSources.RCS);
                    //SetupResult.For(originationSource.Description).Return("RCS");

                    //// Setup legalEntity properties
                    //ILegalEntityType legalEntityType = _mockery.StrictMock<ILegalEntityType>();
                    //SetupResult.For(legalEntity.LegalEntityType).Return(legalEntityType);
                    //SetupResult.For(legalEntityType.Key).Return((int)LegalEntityTypes.NaturalPerson);
                    //SetupResult.For(legalEntity.GetLegalName(LegalNameFormat.Full)).Return("Mr. Craig Graham Fraser");
                    //SetupResult.For(legalEntity.IDNumber).Return("6812095219087");

                    //// Setup LegalEntity Account Roles
                    //IEventList<IRole> roles = new EventList<IRole>();
                    //SetupResult.For(legalEntity.Roles).Return(roles);
                    //IRole role = _mockery.StrictMock<IRole>();
                    //roles.Add(Messages, role);

                    //// Setup account
                    //IAccount account = _mockery.StrictMock<IAccount>();
                    //SetupResult.For(role.Account).Return(account);
                    //// Setup origination source of account
                    //SetupResult.For(account.OriginationSource).Return(accountOriginationSource);
                    //// Setup status of account
                    //IAccountStatus accountStatus = _mockery.StrictMock<IAccountStatus>();
                    //SetupResult.For(account.AccountStatus).Return(accountStatus);
                    //SetupResult.For(accountStatus.Key).Return((int)SAHL.Common.Globals.AccountStatuses.Open);

                    //// Setup LegalEntity Application Roles
                    //IEventList<IApplicationRole> applicationRoles = new EventList<IApplicationRole>();
                    //SetupResult.For(legalEntity.ApplicationRoles).Return(applicationRoles);
                    //IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();

                    //IApplicationRoleType applicationRoleType = _mockery.StrictMock<IApplicationRoleType>();
                    //IApplicationRoleTypeGroup applicationRoleTypeGroup = _mockery.StrictMock<IApplicationRoleTypeGroup>();

                    //SetupResult.For(applicationRoleTypeGroup.Key).Return((int)SAHL.Common.Globals.OfferRoleTypeGroups.Client);
                    //SetupResult.For(applicationRoleType.ApplicationRoleTypeGroup).Return(applicationRoleTypeGroup);
                    //SetupResult.For(applicationRole.ApplicationRoleType).Return(applicationRoleType);

                    //applicationRoles.Add(Messages, applicationRole);

                    //// Setup application
                    //IApplication application = _mockery.StrictMock<IApplication>();
                    //SetupResult.For(applicationRole.Application).Return(application);
                    //// Setup origination source of application
                    //SetupResult.For(application.OriginationSource).Return(originationSource);
                    //// Setup applicationInformation of application
                    //IApplicationInformation applicationInformation = _mockery.StrictMock<IApplicationInformation>();
                    //SetupResult.For(application.GetLatestApplicationInformation()).Return(applicationInformation);
                    //IApplicationInformationType applicationInformationType = _mockery.StrictMock<IApplicationInformationType>();
                    //SetupResult.For(applicationInformation.ApplicationInformationType).Return(applicationInformationType);
                    //SetupResult.For(applicationInformationType.Key).Return((int)SAHL.Common.Globals.OfferInformationTypes.AcceptedOffer);
                    //// Setup applicationType of the application
                    //IApplicationType applicationType = _mockery.StrictMock<IApplicationType>();
                    //SetupResult.For(application.ApplicationType).Return(applicationType);
                    //SetupResult.For(applicationType.Key).Return((int)SAHL.Common.Globals.ApplicationTypes.NewPurchaseLoan);
                    //SetupResult.For(applicationType.Description).Return("New Purchase Loan");

                    //ExecuteRule(rule, 1, legalEntity);

                    #endregion OldMockTest

                    //NB: RCS offer roles are linked through OfferAccountRelationship
                    //      in the Role table
                    string sqlQueryFailApplication = "select top 1 r.LegalEntityKey " +
                        "from Offer o (nolock) " +
                        "join OfferAccountRelationship oar(nolock)on o.OfferKey = oar.OfferKey " +
                        "join [Role] r on oar.AccountKey = r.AccountKey " +
                        "where o.OriginationSourceKey = 4 and o.OfferStatusKey = 1";

                    obj = Helper.ExecuteScalar(con, sqlQueryFailApplication);

                    if (obj != null)
                    {
                        ILegalEntity le = LERepo.GetLegalEntityByKey((int)obj);

                        ExecuteRule(rule, 1, le);
                    }

                    #endregion RCSAppFail

                    //Pass on SAHL LE

                    #region SAHLPass

                    #region OldMockTest

                    //// Setup the parameter objects to pass along
                    //ILegalEntityNaturalPerson legalEntity = _mockery.StrictMock<ILegalEntityNaturalPerson>();
                    ////int primaryOriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans;

                    //// Setup origination source of account/application roles
                    //IOriginationSource originationSource = _mockery.StrictMock<IOriginationSource>();
                    //SetupResult.For(originationSource.Key).Return((int)SAHL.Common.Globals.OriginationSources.SAHomeLoans);

                    //// Setup LegalEntity Account Roles
                    //IEventList<IRole> roles = new EventList<IRole>();
                    //SetupResult.For(legalEntity.Roles).Return(roles);
                    //IRole role = _mockery.StrictMock<IRole>();
                    //roles.Add(Messages, role);

                    //// Setup account
                    //IAccount account = _mockery.StrictMock<IAccount>();
                    //SetupResult.For(role.Account).Return(account);
                    //// Setup origination source of account
                    //SetupResult.For(account.OriginationSource).Return(originationSource);
                    //// Setup status of account
                    //IAccountStatus accountStatus = _mockery.StrictMock<IAccountStatus>();
                    //SetupResult.For(account.AccountStatus).Return(accountStatus);
                    //SetupResult.For(accountStatus.Key).Return((int)SAHL.Common.Globals.AccountStatuses.Open);

                    //// Setup LegalEntity Application Roles
                    //IEventList<IApplicationRole> applicationRoles = new EventList<IApplicationRole>();
                    //SetupResult.For(legalEntity.ApplicationRoles).Return(applicationRoles);
                    //IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();

                    //IApplicationRoleType applicationRoleType = _mockery.StrictMock<IApplicationRoleType>();
                    //IApplicationRoleTypeGroup applicationRoleTypeGroup = _mockery.StrictMock<IApplicationRoleTypeGroup>();

                    //SetupResult.For(applicationRoleTypeGroup.Key).Return((int)SAHL.Common.Globals.OfferRoleTypeGroups.Client);
                    //SetupResult.For(applicationRoleType.ApplicationRoleTypeGroup).Return(applicationRoleTypeGroup);
                    //SetupResult.For(applicationRole.ApplicationRoleType).Return(applicationRoleType);

                    //applicationRoles.Add(Messages, applicationRole);

                    ////ApplicationRoleType.ApplicationRoleTypeGroup.Key

                    //// Setup application
                    //IApplication application = _mockery.StrictMock<IApplication>();
                    //SetupResult.For(applicationRole.Application).Return(application);
                    //// Setup origination source of application
                    //SetupResult.For(application.OriginationSource).Return(originationSource);
                    //// Setup applicationInformation of application
                    //IApplicationInformation applicationInformation = _mockery.StrictMock<IApplicationInformation>();
                    //SetupResult.For(application.GetLatestApplicationInformation()).Return(applicationInformation);
                    //IApplicationInformationType applicationInformationType = _mockery.StrictMock<IApplicationInformationType>();
                    //SetupResult.For(applicationInformation.ApplicationInformationType).Return(applicationInformationType);
                    //SetupResult.For(applicationInformationType.Key).Return((int)SAHL.Common.Globals.OfferInformationTypes.AcceptedOffer);

                    //ExecuteRule(rule, 0, legalEntity);

                    #endregion OldMockTest

                    string sqlQueryPass = "select top 1 le.LegalEntityKey from LegalEntity le (nolock) " +
                        "where le.LegalEntityKey not in  " +
                        "( " +
                        "    select r.LegalEntityKey  " +
                        "    from Account a (nolock) " +
                        "    join [Role] r (nolock) on r.AccountKey = a.AccountKey " +
                        "    where a.RRR_OriginationSourceKey = 4 " +
                        ")";

                    obj = Helper.ExecuteScalar(con, sqlQueryPass);

                    if (obj != null)
                    {
                        ILegalEntity le = LERepo.GetLegalEntityByKey((int)obj);

                        ExecuteRule(rule, 0, le);
                    }

                    #endregion SAHLPass
                }
                finally
                {
                    if (con != null)
                        con.Dispose();
                }
            }
        }

        #endregion LegalEntityOriginationSource

        #region Repositories

        private ILegalEntityRepository leRepo;

        public ILegalEntityRepository LERepo
        {
            get
            {
                if (leRepo == null)
                    leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

                return leRepo;
            }
        }

        #endregion Repositories

        #region MortgageLoanAccountLegalEntityRoleLeadMainApplicant

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountLegalEntityRoleLeadMainApplicantAccountSuccess()
        {
            MortgageLoanAccountLegalEntityRoleLeadMainApplicant rule = new MortgageLoanAccountLegalEntityRoleLeadMainApplicant();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.Roles
            IEventList<IRole> roles = new EventList<IRole>();
            SetupResult.For(mortgageLoanAccount.Roles).Return(roles);
            IRole role = _mockery.StrictMock<IRole>();
            roles.Add(Messages, role);

            // Setup role.RoleType.Key
            IRoleType roleType = _mockery.StrictMock<IRoleType>();
            SetupResult.For(role.RoleType).Return(roleType);
            SetupResult.For(roleType.Key).Return((int)RoleTypes.MainApplicant);

            // Setup role.GeneralStatus.Key
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(role.GeneralStatus).Return(generalStatus);
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);

            ExecuteRule(rule, 0, mortgageLoanAccount);
        }

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountLegalEntityRoleLeadMainApplicantAccountFail()
        {
            MortgageLoanAccountLegalEntityRoleLeadMainApplicant rule = new MortgageLoanAccountLegalEntityRoleLeadMainApplicant();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.Roles
            IEventList<IRole> roles = new EventList<IRole>();
            SetupResult.For(mortgageLoanAccount.Roles).Return(roles);
            IRole role = _mockery.StrictMock<IRole>();
            roles.Add(Messages, role);

            // Setup role.RoleType.Key
            IRoleType roleType = _mockery.StrictMock<IRoleType>();
            SetupResult.For(role.RoleType).Return(roleType);
            SetupResult.For(roleType.Key).Return((int)RoleTypes.MainApplicant);

            // Setup role.GeneralStatus.Key
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(role.GeneralStatus).Return(generalStatus);
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Inactive);

            ExecuteRule(rule, 1, mortgageLoanAccount);
        }

        #endregion MortgageLoanAccountLegalEntityRoleLeadMainApplicant

        [NUnit.Framework.Test]
        public void MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicantTestPass()
        {
            MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicant rule = new MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicant();

            IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();

            // Setup applicationMortgageLoan.ApplicationRoles
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            IGeneralStatus gs = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(applicationRole.GeneralStatus).Return(gs);
            SetupResult.For(gs.Key).Return((int)GeneralStatuses.Active);

            SetupResult.For(applicationRole.ApplicationRoleType).Return(appRoleType);
            SetupResult.For(appRoleType.Key).Return((int)OfferRoleTypes.MainApplicant);

            ILegalEntity le = _mockery.StrictMock<ILegalEntity>();
            ILegalEntityType leType = _mockery.StrictMock<ILegalEntityType>();

            SetupResult.For(applicationRole.LegalEntity).Return(le);
            SetupResult.For(le.LegalEntityType).Return(leType);
            SetupResult.For(leType.Key).Return((int)LegalEntityTypes.Company);

            Stack<IApplicationRole> appRoles = new Stack<IApplicationRole>();
            appRoles.Push(applicationRole);

            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(appRoles);
            SetupResult.For(appML.ApplicationRoles).Return(applicationRoles);

            ExecuteRule(rule, 0, appML);
        }

        [NUnit.Framework.Test]
        public void MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicantSuretorTestFail()
        {
            MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicant rule = new MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicant();

            IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();

            // Setup applicationMortgageLoan.ApplicationRoles
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            IGeneralStatus gs = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(applicationRole.GeneralStatus).Return(gs);
            SetupResult.For(gs.Key).Return((int)GeneralStatuses.Active);

            SetupResult.For(applicationRole.ApplicationRoleType).Return(appRoleType);
            SetupResult.For(appRoleType.Key).Return((int)OfferRoleTypes.LeadSuretor);

            ILegalEntity le = _mockery.StrictMock<ILegalEntity>();
            ILegalEntityType leType = _mockery.StrictMock<ILegalEntityType>();

            SetupResult.For(applicationRole.LegalEntity).Return(le);
            SetupResult.For(le.LegalEntityType).Return(leType);
            SetupResult.For(leType.Key).Return((int)LegalEntityTypes.Company);

            Stack<IApplicationRole> appRoles = new Stack<IApplicationRole>();
            appRoles.Push(applicationRole);

            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(appRoles);
            SetupResult.For(appML.ApplicationRoles).Return(applicationRoles);

            ExecuteRule(rule, 1, appML);
        }

        #region MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicantQuantity

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicantQuantityAccountSuccess()
        {
            MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicantQuantity rule = new MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicantQuantity();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.Roles
            IEventList<IRole> roles = new EventList<IRole>();
            SetupResult.For(mortgageLoanAccount.Roles).Return(roles);
            IRole role = _mockery.StrictMock<IRole>();
            roles.Add(Messages, role);

            // Setup role.LegalEntity.LegalEntityType.Key
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(role.LegalEntity).Return(legalEntity);
            ILegalEntityType legalEntityType = _mockery.StrictMock<ILegalEntityType>();
            SetupResult.For(legalEntity.LegalEntityType).Return(legalEntityType);
            SetupResult.For(legalEntityType.Key).Return((int)LegalEntityTypes.Trust);

            // Setup role.RoleType.Key
            IRoleType roleType = _mockery.StrictMock<IRoleType>();
            SetupResult.For(role.RoleType).Return(roleType);
            SetupResult.For(roleType.Key).Return((int)RoleTypes.MainApplicant);

            // Setup role.GeneralStatus.Key
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(role.GeneralStatus).Return(generalStatus);
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);

            ExecuteRule(rule, 0, mortgageLoanAccount);
        }

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicantQuantityAccountFail()
        {
            MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicantQuantity rule = new MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicantQuantity();

            // Setup the correct object to pass along
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();

            // Setup mortgageLoanAccount.Roles
            IEventList<IRole> roles = new EventList<IRole>();
            SetupResult.For(mortgageLoanAccount.Roles).Return(roles);
            IRole role = _mockery.StrictMock<IRole>();
            roles.Add(Messages, role);
            roles.Add(Messages, role); // Inserting two roles of Company Main Applicant

            // Setup role.LegalEntity.LegalEntityType.Key
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(role.LegalEntity).Return(legalEntity);
            ILegalEntityType legalEntityType = _mockery.StrictMock<ILegalEntityType>();
            SetupResult.For(legalEntity.LegalEntityType).Return(legalEntityType);
            SetupResult.For(legalEntityType.Key).Return((int)LegalEntityTypes.Trust);

            // Setup role.RoleType.Key
            IRoleType roleType = _mockery.StrictMock<IRoleType>();
            SetupResult.For(role.RoleType).Return(roleType);
            SetupResult.For(roleType.Key).Return((int)RoleTypes.MainApplicant);

            // Setup role.GeneralStatus.Key
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(role.GeneralStatus).Return(generalStatus);
            SetupResult.For(generalStatus.Key).Return((int)GeneralStatuses.Active);

            ExecuteRule(rule, 1, mortgageLoanAccount);
        }

        #endregion MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicantQuantity

        /// <summary>
        /// LegalEntityContactDetailsMandatoryTest
        /// </summary>
        [NUnit.Framework.Test]
        public void LegalEntityContactDetailsMandatoryTest()
        {
            //Only pass one detail and it should be valid
            LegalEntityContactDetailsMandatoryHelper("test@test.com", String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, 0);
            LegalEntityContactDetailsMandatoryHelper(String.Empty, "0837401111", String.Empty, String.Empty, String.Empty, String.Empty, 0);

            LegalEntityContactDetailsMandatoryHelper(String.Empty, String.Empty, "031", String.Empty, String.Empty, String.Empty, 1);
            LegalEntityContactDetailsMandatoryHelper(String.Empty, String.Empty, String.Empty, "2013333", String.Empty, String.Empty, 1);
            LegalEntityContactDetailsMandatoryHelper(String.Empty, String.Empty, String.Empty, String.Empty, "035", String.Empty, 1);

            //This one should fail as no detail is supplied
            LegalEntityContactDetailsMandatoryHelper(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, 1);
        }

        /// <summary>
        /// Legal Entity Contact Details Mandatory Helper
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="cellphoneNumber"></param>
        /// <param name="homePhoneCode"></param>
        /// <param name="homePhoneNumber"></param>
        /// <param name="workPhoneCode"></param>
        /// <param name="workPhoneNumber"></param>
        private void LegalEntityContactDetailsMandatoryHelper(string emailAddress,
                                                           string cellphoneNumber,
                                                           string homePhoneCode,
                                                           string homePhoneNumber,
                                                           string workPhoneCode,
                                                           string workPhoneNumber,
                                                           int expectedErrorCount)
        {
            LegalEntityContactDetailsMandatory rule = new LegalEntityContactDetailsMandatory();

            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(legalEntity.EmailAddress).Return(emailAddress);
            SetupResult.For(legalEntity.CellPhoneNumber).Return(cellphoneNumber);

            SetupResult.For(legalEntity.HomePhoneCode).Return(homePhoneCode);
            SetupResult.For(legalEntity.HomePhoneNumber).Return(homePhoneNumber);

            SetupResult.For(legalEntity.WorkPhoneCode).Return(workPhoneCode);
            SetupResult.For(legalEntity.WorkPhoneNumber).Return(workPhoneNumber);

            ExecuteRule(rule, expectedErrorCount, legalEntity);
        }

        [NUnit.Framework.Test]
        public void MortgageLoanAccountLegalEntityRoleCompanyMainApplicantNaturalPersonSuretorPass()
        {
            MortgageLoanAccountLegalEntityRoleCompanyMainApplicantNaturalPersonSuretor rule = new MortgageLoanAccountLegalEntityRoleCompanyMainApplicantNaturalPersonSuretor();

            IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();

            // Setup applicationMortgageLoan.ApplicationRoles
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            IGeneralStatus gs = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(applicationRole.GeneralStatus).Return(gs);
            SetupResult.For(gs.Key).Return((int)GeneralStatuses.Active);

            SetupResult.For(applicationRole.ApplicationRoleType).Return(appRoleType);
            SetupResult.For(appRoleType.Key).Return((int)OfferRoleTypes.MainApplicant);

            ILegalEntity le = _mockery.StrictMock<ILegalEntity>();
            ILegalEntityType leType = _mockery.StrictMock<ILegalEntityType>();

            SetupResult.For(applicationRole.LegalEntity).Return(le);
            SetupResult.For(le.LegalEntityType).Return(leType);
            SetupResult.For(leType.Key).Return((int)LegalEntityTypes.NaturalPerson);

            Stack<IApplicationRole> appRoles = new Stack<IApplicationRole>();
            appRoles.Push(applicationRole);

            //second application role
            // Setup applicationMortgageLoan.ApplicationRoles
            IApplicationRole applicationRole2 = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType appRoleType2 = _mockery.StrictMock<IApplicationRoleType>();
            IGeneralStatus gs2 = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(applicationRole2.GeneralStatus).Return(gs2);
            SetupResult.For(gs2.Key).Return((int)GeneralStatuses.Active);

            SetupResult.For(applicationRole2.ApplicationRoleType).Return(appRoleType2);
            SetupResult.For(appRoleType2.Key).Return((int)OfferRoleTypes.Suretor);

            ILegalEntity le2 = _mockery.StrictMock<ILegalEntity>();
            ILegalEntityType leType2 = _mockery.StrictMock<ILegalEntityType>();

            SetupResult.For(applicationRole2.LegalEntity).Return(le2);
            SetupResult.For(le2.LegalEntityType).Return(leType2);
            SetupResult.For(leType2.Key).Return((int)LegalEntityTypes.NaturalPerson);
            appRoles.Push(applicationRole2);

            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(appRoles);
            SetupResult.For(appML.ApplicationRoles).Return(applicationRoles);

            ExecuteRule(rule, 0, appML);
        }

        [NUnit.Framework.Test]
        public void MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicantNaturalPersonSuretorPass()
        {
            MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicantNaturalPersonSuretor rule = new MortgageLoanAccountLegalEntityRoleCompanyLeadMainApplicantNaturalPersonSuretor();

            IApplicationMortgageLoan appML = _mockery.StrictMock<IApplicationMortgageLoan>();

            // Setup applicationMortgageLoan.ApplicationRoles
            IApplicationRole applicationRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            IGeneralStatus gs = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(applicationRole.GeneralStatus).Return(gs);
            SetupResult.For(gs.Key).Return((int)GeneralStatuses.Active);

            SetupResult.For(applicationRole.ApplicationRoleType).Return(appRoleType);
            SetupResult.For(appRoleType.Key).Return((int)OfferRoleTypes.LeadMainApplicant);

            ILegalEntity le = _mockery.StrictMock<ILegalEntity>();
            ILegalEntityType leType = _mockery.StrictMock<ILegalEntityType>();

            SetupResult.For(applicationRole.LegalEntity).Return(le);
            SetupResult.For(le.LegalEntityType).Return(leType);
            SetupResult.For(leType.Key).Return((int)LegalEntityTypes.NaturalPerson);

            Stack<IApplicationRole> appRoles = new Stack<IApplicationRole>();
            appRoles.Push(applicationRole);

            //second application role
            // Setup applicationMortgageLoan.ApplicationRoles
            IApplicationRole applicationRole2 = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType appRoleType2 = _mockery.StrictMock<IApplicationRoleType>();
            IGeneralStatus gs2 = _mockery.StrictMock<IGeneralStatus>();
            SetupResult.For(applicationRole2.GeneralStatus).Return(gs2);
            SetupResult.For(gs2.Key).Return((int)GeneralStatuses.Active);

            SetupResult.For(applicationRole2.ApplicationRoleType).Return(appRoleType2);
            SetupResult.For(appRoleType2.Key).Return((int)OfferRoleTypes.Suretor);

            ILegalEntity le2 = _mockery.StrictMock<ILegalEntity>();
            ILegalEntityType leType2 = _mockery.StrictMock<ILegalEntityType>();

            SetupResult.For(applicationRole2.LegalEntity).Return(le2);
            SetupResult.For(le2.LegalEntityType).Return(leType2);
            SetupResult.For(leType2.Key).Return((int)LegalEntityTypes.NaturalPerson);
            appRoles.Push(applicationRole2);

            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(appRoles);
            SetupResult.For(appML.ApplicationRoles).Return(applicationRoles);

            ExecuteRule(rule, 0, appML);
        }

        [Test]
        public void LegalEntityLoginPass()
        {
            using (new SessionScope())
            {
                var rule = new LegalEntityLoginDuplicateUsername();
                ILegalEntityLogin lel = _mockery.StrictMock<ILegalEntityLogin>();
                ILegalEntityRepository repo = _mockery.StrictMock<ILegalEntityRepository>();
                string username = "test";

                //
                SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
                MockCache.Add(typeof(ILegalEntityRepository).ToString(), repo);

                //
                SetupResult.For(lel.Username).Return("test");
                SetupResult.For(repo.GetLegalEntityLogin(username)).IgnoreArguments().Return(null);

                ExecuteRule(rule, 0, lel);
            }
        }

        [Test]
        public void LegalEntityLoginFail()
        {
            using (new SessionScope())
            {
                var rule = new LegalEntityLoginDuplicateUsername();
                ILegalEntityLogin lel = _mockery.StrictMock<ILegalEntityLogin>();
                ILegalEntityRepository repo = _mockery.StrictMock<ILegalEntityRepository>();
                string username = "test";

                //
                SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
                MockCache.Add(typeof(ILegalEntityRepository).ToString(), repo);

                //
                SetupResult.For(lel.Username).Return("test");
                SetupResult.For(repo.GetLegalEntityLogin(username)).IgnoreArguments().Return(lel);

                ExecuteRule(rule, 1, lel);
            }
        }
    }
}