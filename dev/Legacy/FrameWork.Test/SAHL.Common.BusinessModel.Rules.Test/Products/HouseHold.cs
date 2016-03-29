using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using Castle.ActiveRecord;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Rules.Products;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Rules.Test.Products
{
    [TestFixture]
    public class HouseHold : RuleBase
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

        #region HouseholdIncomeContributorMinimum

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void HouseholdIncomeContributorMinimumSuccess()
        {
            HouseholdIncomeContributorMinimum rule = new HouseholdIncomeContributorMinimum(RepositoryFactory.GetRepository<ICastleTransactionsService>());

            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            using (IDbConnection con = Helper.GetSQLDBConnection())
            {
                string sqlQueryFailAccount = @"select top 1 ofr.OfferKey
                    from offerrole ofr (nolock)
                    join LegalEntity le (nolock) on ofr.LegalEntityKey = le.LegalEntityKey
	                    and le.LegalEntityTypeKey = 2 --Natural Persons
                    join OfferRoleType ort (nolock) on ofr.OfferRoleTypeKey = ort.OfferRoleTypeKey
	                    and ort.OfferRoleTypeGroupKey = 3 --clients only
	                    and ofr.OfferRoleTypeKey != 13 --no life cllients
                    join offerroleattribute ofra (nolock) on ofra.OfferRoleKey = ofr.OfferRoleKey
                    where ofr.GeneralStatusKey = 1 --and ofr.OfferKey = @applicationKey
                    and ofra.OfferRoleAttributeTypeKey = 1 ";

                object obj = Helper.ExecuteScalar(con, sqlQueryFailAccount);

                if (obj != null)
                {
                    SetupResult.For(applicationMortgageLoan.Key).Return((int)obj);
                    ExecuteRule(rule, 0, applicationMortgageLoan);
                }
            }
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void HouseholdIncomeContributorMinimumFail()
        {
            HouseholdIncomeContributorMinimum rule = new HouseholdIncomeContributorMinimum(RepositoryFactory.GetRepository<ICastleTransactionsService>());

            IApplicationMortgageLoan applicationMortgageLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            using (IDbConnection con = Helper.GetSQLDBConnection())
            {
                string sqlQueryFailAccount = "select top 1 ofr.OfferKey " +
                            "from offerrole ofr (nolock) " +
                            "join OfferRoleType ort (nolock) on ofr.OfferRoleTypeKey = ort.OfferRoleTypeKey  " +
                            "    and ort.OfferRoleTypeKey = 1 ";

                object obj = Helper.ExecuteScalar(con, sqlQueryFailAccount);

                if (obj != null)
                {
                    SetupResult.For(applicationMortgageLoan.Key).Return((int)obj);
                    ExecuteRule(rule, 1, applicationMortgageLoan);
                }
            }
        }

        #endregion HouseholdIncomeContributorMinimum

        #region HouseholdIncomeAtLeastOne

        /// <summary>
        /// Expects Messages.Count = 1.
        /// </summary>
        [NUnit.Framework.Test]
        public void HouseholdIncomeAtLeastOneFail()
        {
            HouseholdIncomeAtLeastOne rule = new HouseholdIncomeAtLeastOne();

            IApplicationMortgageLoan appMortLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            SetupResult.For(appMortLoan.Key).Return(1);

            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            Stack<IApplicationRole> applicationRoleStack = new Stack<IApplicationRole>();
            applicationRoleStack.Push(appRole);
            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(applicationRoleStack);
            SetupResult.For(appMortLoan.ApplicationRoles).Return(applicationRoles);

            IApplicationRoleType appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(appRoleType.Key).Return((int)OfferRoleTypes.AssuredLife);
            SetupResult.For(appRole.ApplicationRoleType).Return(appRoleType);

            IEventList<IApplicationRoleAttribute> appRoleAttrs = new EventList<IApplicationRoleAttribute>();
            SetupResult.For(appRole.ApplicationRoleAttributes).Return(appRoleAttrs);
            IApplicationRoleAttribute appRoleAttr = _mockery.StrictMock<IApplicationRoleAttribute>();
            appRoleAttrs.Add(Messages, appRoleAttr);

            IApplicationRoleAttributeType appRoleAttrType = _mockery.StrictMock<IApplicationRoleAttributeType>();
            SetupResult.For(appRoleAttrType.Key).Return((int)OfferRoleAttributeTypes.IncomeContributor);
            SetupResult.For(appRoleAttr.OfferRoleAttributeType).Return(appRoleAttrType);

            ILegalEntity legalEnt = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(appRole.LegalEntity).Return(legalEnt);

            IEventList<IEmployment> emps = new EventList<IEmployment>();
            SetupResult.For(legalEnt.Employment).Return(emps);
            IEmployment emp = _mockery.StrictMock<IEmployment>();
            SetupResult.For(emp.IsConfirmed).Return(false);
            emps.Add(Messages, emp);

            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            SetupResult.For(empStatus.Key).Return((int)EmploymentStatuses.Current);
            SetupResult.For(emp.EmploymentStatus).Return(empStatus);

            ExecuteRule(rule, 1, appMortLoan);
        }

        /// <summary>
        /// Expects Messages.Count = 0.
        /// </summary>
        [NUnit.Framework.Test]
        public void HouseholdIncomeAtLeastOneSuccess()
        {
            HouseholdIncomeAtLeastOne rule = new HouseholdIncomeAtLeastOne();

            IApplicationMortgageLoan appMortLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            SetupResult.For(appMortLoan.Key).Return(1);

            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            Stack<IApplicationRole> applicationRoleStack = new Stack<IApplicationRole>();
            applicationRoleStack.Push(appRole);
            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(applicationRoleStack);
            SetupResult.For(appMortLoan.ApplicationRoles).Return(applicationRoles);

            IApplicationRoleType appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            SetupResult.For(appRoleType.Key).Return((int)OfferRoleTypes.MainApplicant);
            SetupResult.For(appRole.ApplicationRoleType).Return(appRoleType);

            IEventList<IApplicationRoleAttribute> appRoleAttrs = new EventList<IApplicationRoleAttribute>();
            SetupResult.For(appRole.ApplicationRoleAttributes).Return(appRoleAttrs);
            IApplicationRoleAttribute appRoleAttr = _mockery.StrictMock<IApplicationRoleAttribute>();
            appRoleAttrs.Add(Messages, appRoleAttr);

            IApplicationRoleAttributeType appRoleAttrType = _mockery.StrictMock<IApplicationRoleAttributeType>();
            SetupResult.For(appRoleAttrType.Key).Return((int)OfferRoleAttributeTypes.IncomeContributor);
            SetupResult.For(appRoleAttr.OfferRoleAttributeType).Return(appRoleAttrType);

            ILegalEntity legalEnt = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(appRole.LegalEntity).Return(legalEnt);

            IEventList<IEmployment> emps = new EventList<IEmployment>();
            SetupResult.For(legalEnt.Employment).Return(emps);
            IEmployment emp = _mockery.StrictMock<IEmployment>();
            SetupResult.For(emp.IsConfirmed).Return(true);
            emps.Add(Messages, emp);

            IEmploymentStatus empStatus = _mockery.StrictMock<IEmploymentStatus>();
            SetupResult.For(empStatus.Key).Return((int)EmploymentStatuses.Current);
            SetupResult.For(emp.EmploymentStatus).Return(empStatus);

            ExecuteRule(rule, 0, appMortLoan);
        }

        #endregion HouseholdIncomeAtLeastOne

        #region HouseholdIncomeContributorRules

        [Test]
        public void HouseholdIncomeContributorRules()
        {
            HouseholdIncomeContributorRules(OfferRoleTypes.AssuredLife, 1);
            HouseholdIncomeContributorRules(OfferRoleTypes.EstateAgent, 1);
            HouseholdIncomeContributorRules(OfferRoleTypes.Seller, 1);
            HouseholdIncomeContributorRules(OfferRoleTypes.MainApplicant, 0);
            HouseholdIncomeContributorRules(OfferRoleTypes.Suretor, 0);
        }

        private void HouseholdIncomeContributorRules(OfferRoleTypes roleType, int msgCount)
        {
            HouseholdIncomeContributorRules rule = new HouseholdIncomeContributorRules();

            IApplicationMortgageLoan appMortLoan = _mockery.StrictMock<IApplicationMortgageLoan>();
            SetupResult.For(appMortLoan.Key).Return(1);

            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            Stack<IApplicationRole> applicationRoleStack = new Stack<IApplicationRole>();
            applicationRoleStack.Push(appRole);
            IReadOnlyEventList<IApplicationRole> applicationRoles = new ReadOnlyEventList<IApplicationRole>(applicationRoleStack);
            SetupResult.For(appMortLoan.ApplicationRoles).Return(applicationRoles);

            IApplicationRoleType appRoleType = _mockery.StrictMock<IApplicationRoleType>();

            SetupResult.For(appRoleType.Key).Return((int)roleType);

            SetupResult.For(appRole.ApplicationRoleType).Return(appRoleType);

            IEventList<IApplicationRoleAttribute> appRoleAttrs = new EventList<IApplicationRoleAttribute>();
            SetupResult.For(appRole.ApplicationRoleAttributes).Return(appRoleAttrs);
            IApplicationRoleAttribute appRoleAttr = _mockery.StrictMock<IApplicationRoleAttribute>();
            appRoleAttrs.Add(Messages, appRoleAttr);

            IApplicationRoleAttributeType appRoleAttrType = _mockery.StrictMock<IApplicationRoleAttributeType>();
            SetupResult.For(appRoleAttrType.Key).Return((int)OfferRoleAttributeTypes.IncomeContributor);
            SetupResult.For(appRoleAttr.OfferRoleAttributeType).Return(appRoleAttrType);

            ExecuteRule(rule, msgCount, appMortLoan);
        }

        #endregion HouseholdIncomeContributorRules
    }
}