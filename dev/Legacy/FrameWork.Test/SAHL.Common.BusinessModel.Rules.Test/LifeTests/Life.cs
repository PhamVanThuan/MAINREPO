using System;
using SAHL.Common.Collections;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using Rhino.Mocks;
using SAHL.Common.Globals;
using System.Collections;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Rules.LifeRules;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Rules.Test.LifeTests
{
    [TestFixture]
    public class Life : RuleBase
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

        #region ValidateAssuredLifeMinimumRequired
        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ValidateAssuredLifeMinimumRequired_NoArgumentsPassed()
        {
            ValidateAssuredLifeMinimumRequired rule = new ValidateAssuredLifeMinimumRequired();

            ExecuteRule(rule, 0);
        }

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if incorrect arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ValidateAssuredLifeMinimumRequired_IncorrectArgumentsPassed()
        {
            ValidateAssuredLifeMinimumRequired rule = new ValidateAssuredLifeMinimumRequired();

            // Setup an incorrect Argument to pass -- the rule should accept an IAccountLifePolicy
            IApplication application = _mockery.StrictMock<IApplication>();
            ExecuteRule(rule, 0, application);

        }

        [NUnit.Framework.Test]
        public void ValidateAssuredLifeMinimumRequired_ClosedAccount_Success()
        {
            ValidateAssuredLifeMinimumRequired rule = new ValidateAssuredLifeMinimumRequired();

            // Setup the AccountLifePolicy
            IAccountLifePolicy accountLifePolicy = _mockery.StrictMock<IAccountLifePolicy>();
            IAccountStatus accountStatus = _mockery.StrictMock<IAccountStatus>();
            SetupResult.For(accountStatus.Key).Return((int)SAHL.Common.Globals.AccountStatuses.Closed);
            SetupResult.For(accountLifePolicy.AccountStatus).Return(accountStatus);

            ExecuteRule(rule, 0, accountLifePolicy);
        }

        [NUnit.Framework.Test]
        public void ValidateAssuredLifeMinimumRequired_Success()
        {
            ValidateAssuredLifeMinimumRequired rule = new ValidateAssuredLifeMinimumRequired();

            // Setup an correct Argument to pass 
            IAccountLifePolicy accountLifePolicy = _mockery.StrictMock<IAccountLifePolicy>();
 
            // Setup the account status
            IAccountStatus accountStatus = _mockery.StrictMock<IAccountStatus>();
            SetupResult.For(accountStatus.Key).Return((int)SAHL.Common.Globals.AccountStatuses.Open);
            SetupResult.For(accountLifePolicy.AccountStatus).Return(accountStatus);

            // Setup an Assured Life Role on the IAccountLifePolicy
            IEventList<IRole> roles = new EventList<IRole>();
            IRole role = _mockery.StrictMock<IRole>();

            IRoleType roleType = _mockery.StrictMock<IRoleType>();
            SetupResult.For(role.RoleType).Return(roleType);
            SetupResult.For(roleType.Key).Return((int)SAHL.Common.Globals.RoleTypes.AssuredLife);
            SetupResult.For(accountLifePolicy.Key).Return(1);
            SetupResult.For(accountLifePolicy.Roles).Return(roles);

            roles.Add(Messages, role);
            
            // Execute the rule 
            ExecuteRule(rule, 0, accountLifePolicy);
        }

        [NUnit.Framework.Test]
        public void ValidateAssuredLifeMinimumRequired_Failure()
        {
            ValidateAssuredLifeMinimumRequired rule = new ValidateAssuredLifeMinimumRequired();

            // Setup an correct Argument to pass along
            IAccountLifePolicy accountLifePolicy = _mockery.StrictMock<IAccountLifePolicy>();
            SetupResult.For(accountLifePolicy.Key).Return(1);

            // Setup the account status
            IAccountStatus accountStatus = _mockery.StrictMock<IAccountStatus>();
            SetupResult.For(accountStatus.Key).Return((int)SAHL.Common.Globals.AccountStatuses.Open);
            SetupResult.For(accountLifePolicy.AccountStatus).Return(accountStatus);

            IEventList<IRole> roles = new EventList<IRole>();
            IRole role = _mockery.StrictMock<IRole>();

            IRoleType roleType = _mockery.StrictMock<IRoleType>();
            SetupResult.For(role.RoleType).Return(roleType);
            SetupResult.For(roleType.Key).Return((int)SAHL.Common.Globals.RoleTypes.MainApplicant); // this will cause the rule to fail
            SetupResult.For(accountLifePolicy.Roles).Return(roles);

            roles.Add(Messages, role);

            ExecuteRule(rule, 1, accountLifePolicy);


        }
        #endregion

        [Test]
        public void LifeApplicationCheckMonthsInArrearsTestPass()
        {
            LifeApplicationCheckMonthsInArrears rule = new LifeApplicationCheckMonthsInArrears();
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();
            IAccountRepository accRepo = _mockery.StrictMock<IAccountRepository>();
            //
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            MockCache.Add(typeof(IAccountRepository).ToString(), accRepo);
            SetupResult.For(accRepo.GetCurrentMonthsInArrears(1)).IgnoreArguments().Return(0);
            //
            SetupResult.For(mortgageLoanAccount.Key).Return(1);
            ExecuteRule(rule, 0, mortgageLoanAccount);
        }

        [Test]
        public void LifeApplicationCheckMonthsInArrearsTestFail()
        {
            LifeApplicationCheckMonthsInArrears rule = new LifeApplicationCheckMonthsInArrears();
            IMortgageLoanAccount mortgageLoanAccount = _mockery.StrictMock<IMortgageLoanAccount>();
            IAccountRepository accRepo = _mockery.StrictMock<IAccountRepository>();
            //
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            MockCache.Add(typeof(IAccountRepository).ToString(), accRepo);
            SetupResult.For(accRepo.GetCurrentMonthsInArrears(1)).IgnoreArguments().Return(3.5);
            //
            SetupResult.For(mortgageLoanAccount.Key).Return(1);
            ExecuteRule(rule, 1, mortgageLoanAccount);
        }

        //[NUnit.Framework.Test]
        //public void GetCurrentConsultant_Test()
        //{
        //    string Consultant = "Consultant";
        //    //int ApplicationKey = 656795; // dev 
        //    int ApplicationKey = 682779; // systets
        //    string ADUserName = "";

        //    IOrganisationStructureRepository osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
        //    IApplicationRoleType art = osRepo.GetApplicationRoleTypeByName(Consultant);
        //    IApplicationRole appRole = osRepo.FindApplicationRoleByApplicationRoleTypeKeyAndApplicationKey(ApplicationKey, art.Key);

        //    IADUser aduser = osRepo.FindByLegalEntityKey(appRole.LegalEntity.Key);
        //    ADUserName = aduser.ADUserName;

        //    Assert.IsTrue(ADUserName == @"SAHL\CraigF");

        //}

		/// <summary>
		/// life Application Create Debt Counselling
		/// </summary>
		[Test]
		public void LifeApplicationCreateDebtCounselling()
		{
			//Life Policy Account
			LifeApplicationCreateDebtCounsellingHelper(1, LifePolicyStatuses.Prospect, 1, true, false);
			LifeApplicationCreateDebtCounsellingHelper(1, LifePolicyStatuses.Prospect, 2, true, true);
			LifeApplicationCreateDebtCounsellingHelper(1, LifePolicyStatuses.Prospect, 0, false, false);
			LifeApplicationCreateDebtCounsellingHelper(1, LifePolicyStatuses.Prospect, 1, false, true);

			//Life Policy Application
			LifeApplicationCreateDebtCounsellingHelper(2, LifePolicyStatuses.Prospect, 1, true, false);
			LifeApplicationCreateDebtCounsellingHelper(2, LifePolicyStatuses.Prospect, 2, true, true);
			LifeApplicationCreateDebtCounsellingHelper(2, LifePolicyStatuses.Prospect, 0, false, false);
			LifeApplicationCreateDebtCounsellingHelper(2, LifePolicyStatuses.Prospect, 1, false, true);

			//Parent Account
			LifeApplicationCreateDebtCounsellingHelper(3, LifePolicyStatuses.Prospect, 1, true, false);
			LifeApplicationCreateDebtCounsellingHelper(3, LifePolicyStatuses.Prospect, 2, true, true);
			LifeApplicationCreateDebtCounsellingHelper(3, LifePolicyStatuses.Prospect, 0, false, false);
			LifeApplicationCreateDebtCounsellingHelper(3, LifePolicyStatuses.Prospect, 1, false, true);

			//The rule should only run if the LifePolicyStatus is "Prospect"
			foreach(var lifePolicyStatus in Enum.GetValues(typeof(LifePolicyStatuses)))
			{
				var lifePolicyStatusEnum = (LifePolicyStatuses)Enum.Parse(typeof(LifePolicyStatuses), lifePolicyStatus.ToString());
				if (lifePolicyStatusEnum != LifePolicyStatuses.Prospect)
				{
					LifeApplicationCreateDebtCounsellingHelper(1, lifePolicyStatusEnum, 0, true, true);
				}
			}
		}

		/// <summary>
		/// Life Application Create Debt Counselling Helper
		/// </summary>
		/// <param name="context">1: Life Policy Account, 2: Life Policy Application, 3: Normal IAccount</param>
		/// <param name="expectedErrorCount"></param>
		/// <param name="accountUnderDebtCounselling"></param>
		/// <param name="legalEntityHasDebtCounsellingCases"></param>
		private void LifeApplicationCreateDebtCounsellingHelper(int context, LifePolicyStatuses lifePolicyStatusKey, int expectedErrorCount, bool accountUnderDebtCounselling, bool legalEntityHasDebtCounsellingCases)
		{
			var rule = new LifeApplicationCreateDebtCounselling();
			
			var lifeApplication = _mockery.StrictMock<IApplicationLife>();
			var accountLifePolicy = _mockery.StrictMock<IAccountLifePolicy>();
			var lifePolicy = _mockery.StrictMock<ILifePolicy>();
			var lifePolicyStatus = _mockery.StrictMock<ILifePolicyStatus>();
			var parentAccount = _mockery.StrictMock<IAccount>();
			var role = _mockery.StrictMock<IRole>();
			var roleType = _mockery.StrictMock<IRoleType>();
			var legalEntity = _mockery.StrictMock<ILegalEntity>();
			var debtCounsellingCase = _mockery.StrictMock<IDebtCounselling>();

			IEventList<IRole> roles = new EventList<IRole>(new[]{
				role
			});

			IEventList<IDebtCounselling> debtCounsellingCases = new EventList<IDebtCounselling>(new[]{
				debtCounsellingCase
			});

			SetupResult.For(accountLifePolicy.LifePolicy).Return(lifePolicy);
			SetupResult.For(lifePolicy.LifePolicyStatus).Return(lifePolicyStatus);
			SetupResult.For(lifePolicyStatus.Key).Return((int)lifePolicyStatusKey);
			SetupResult.For(lifeApplication.Account).Return(accountLifePolicy);

			SetupResult.For(parentAccount.UnderDebtCounselling).Return(accountUnderDebtCounselling);
			SetupResult.For(parentAccount.Roles).Return(roles);
			SetupResult.For(parentAccount.Key).Return(1);

            SetupResult.For(accountLifePolicy.ParentAccount).Return(parentAccount);
			SetupResult.For(accountLifePolicy.UnderDebtCounselling).Return(accountUnderDebtCounselling);
			SetupResult.For(accountLifePolicy.Key).Return(1);
			SetupResult.For(accountLifePolicy.Roles).Return(roles);
			
			SetupResult.For(role.LegalEntity).Return(legalEntity);
			SetupResult.For(role.RoleType).Return(roleType);
			SetupResult.For(roleType.Description).Return("RoleTypeDescription");
			
			SetupResult.For(legalEntity.DisplayName).Return("DisplayName");
			SetupResult.For(legalEntity.DebtCounsellingCases).Return(legalEntityHasDebtCounsellingCases ? debtCounsellingCases : null);

			if (context == 1)
			{
				ExecuteRule(rule, expectedErrorCount, accountLifePolicy);
			}
			else if (context == 2)
			{
				ExecuteRule(rule, expectedErrorCount, lifeApplication);
			}
			else if (context == 3)
			{
				ExecuteRule(rule, expectedErrorCount, parentAccount);
			}
		}
    }
}
