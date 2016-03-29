using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Rules.FinancialService;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using Rhino.Mocks;

namespace SAHL.Common.BusinessModel.Rules.Test.FinancialService
{
    [TestFixture]
    public class FinancialServiceBankAccount : RuleBase
    {

        private IFinancialServiceBankAccount _fsBankAccount;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _fsBankAccount = _mockery.StrictMock<IFinancialServiceBankAccount>();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void FinancialServiceBankAccountDebitOrderDayAllowedValuesTest()
        {
            FinancialServiceBankAccountDebitOrderDayAllowedValues rule = new FinancialServiceBankAccountDebitOrderDayAllowedValues();

            IApplicationDebitOrder _appDebitOrder = _mockery.StrictMock<IApplicationDebitOrder>();
            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

            List<int> allowedDays = new List<int>();
            // establish list of allowed values
            foreach (IDebitOrderDay debitOrderDay in lookupRepo.DebitOrderDays.ObjectDictionary.Values)
            {
                allowedDays.Add(debitOrderDay.Day);
            }
            
            // now run through a range of values, ensuring rule only passes for allowed days
            for (int i = 0; i <= 31; i++)
            {
                int messageCount = (allowedDays.Contains(i) ? 0 : 1);

                SetupResult.For(_fsBankAccount.DebitOrderDay).Return(i);
                ExecuteRule(rule, messageCount, _fsBankAccount);

                SetupResult.For(_appDebitOrder.DebitOrderDay).Return(i);
                ExecuteRule(rule, messageCount, _appDebitOrder);

            }
        }
    }
}
