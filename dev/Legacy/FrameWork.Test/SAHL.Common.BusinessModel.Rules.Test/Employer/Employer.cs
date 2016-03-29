using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Rules.Employer;

namespace SAHL.Common.BusinessModel.Rules.Test.Employer
{
    [TestFixture]
    public class Employer : RuleBase
    {

        private IEmployer _employer;

        [NUnit.Framework.SetUp]
        public new void Setup()
        {
            base.Setup();
            _employer = _mockery.StrictMock<IEmployer>();
        }

        [TearDown]
        public override void TearDown()
        {
        }

        [NUnit.Framework.Test]
        public void EmployerContactEmailValidationTest()
        {

            EmployerContactEmailValidation rule = new EmployerContactEmailValidation();

            Expect.Call(_employer.ContactEmail).Return("asasa");
            ExecuteRule(rule, 1, _employer);

            Expect.Call(_employer.ContactEmail).Return("test@test");
            ExecuteRule(rule, 1, _employer);

            Expect.Call(_employer.ContactEmail).Return("test@test@test.com");
            ExecuteRule(rule, 1, _employer);

            Expect.Call(_employer.ContactEmail).Return("test.test.com");
            ExecuteRule(rule, 1, _employer);

            Expect.Call(_employer.ContactEmail).Return("test@test.com");
            ExecuteRule(rule, 0, _employer);

        }

        [NUnit.Framework.Test]
        public void EmployerUniqueNameTest()
        {

            EmployerUniqueName rule = new EmployerUniqueName();

            // run rule with NO name - should be fine as the rule doesn't really run
            SetupResult.For(_employer.Name).Return("");
            ExecuteRule(rule, 0, _employer);

            // run rule with arb name that should be fine
            SetupResult.For(_employer.Name).Return("*!$!!&^%&^%&^");
            SetupResult.For(_employer.Key).Return(0);
            ExecuteRule(rule, 0, _employer);

            // run rule with the name of an existing employment record
            Employer_DAO emp = Employer_DAO.FindFirst();
            SetupResult.For(_employer.Name).Return(emp.Name);
            SetupResult.For(_employer.Key).Return(0);
            ExecuteRule(rule, 1, _employer);

        }

        [NUnit.Framework.Test]
        public void EmployerAccountantEmailValidationTest()
        {
            EmployerAccountantEmailValidation rule = new EmployerAccountantEmailValidation();

            Expect.Call(_employer.AccountantEmail).Return("asasa");
            ExecuteRule(rule, 1, _employer);

            Expect.Call(_employer.AccountantEmail).Return("test@test");
            ExecuteRule(rule, 1, _employer);

            Expect.Call(_employer.AccountantEmail).Return("test@test@test.com");
            ExecuteRule(rule, 1, _employer);

            Expect.Call(_employer.AccountantEmail).Return("test.test.com");
            ExecuteRule(rule, 1, _employer);

            Expect.Call(_employer.AccountantEmail).Return("test@test.com");
            ExecuteRule(rule, 0, _employer);
        }

        [NUnit.Framework.Test]
        public void EmployerAccountantPhoneNumberAndCodeValidationTest()
        {
            EmployerAccountantPhoneNumberAndCodeValidation rule = new EmployerAccountantPhoneNumberAndCodeValidation();

            Expect.Call(_employer.AccountantTelephoneCode).Return("").Repeat.Any();
            Expect.Call(_employer.AccountantTelephoneNumber).Return("").Repeat.Any();
            ExecuteRule(rule, 0, _employer);

            Expect.Call(_employer.AccountantTelephoneCode).Return("031").Repeat.Any();
            Expect.Call(_employer.AccountantTelephoneNumber).Return("").Repeat.Any();
            ExecuteRule(rule, 1, _employer);

            Expect.Call(_employer.AccountantTelephoneCode).Return("").Repeat.Any();
            Expect.Call(_employer.AccountantTelephoneNumber).Return("123456").Repeat.Any();
            ExecuteRule(rule, 1, _employer);

            Expect.Call(_employer.AccountantTelephoneCode).Return("031").Repeat.Any();
            Expect.Call(_employer.AccountantTelephoneNumber).Return("123456").Repeat.Any();
            ExecuteRule(rule, 0, _employer);
        }

    }
}
