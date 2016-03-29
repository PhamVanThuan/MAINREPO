using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using NUnit.Framework;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.FrontEndTest.Managers.Statements;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_getting_financial_transactions : WithFakes
    {
        private static IEnumerable<GetFinancialTransactionsQueryResult> expectedResult;
        private static GetFinancialTransactionsStatement financialTransactionsStatement;
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;
        private static int FinancialServiceKey;

        private Establish context = () =>
        {
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
            expectedResult = An<IEnumerable<GetFinancialTransactionsQueryResult>>();
            FinancialServiceKey = 123456789;
            fakeDb.FakedDb.InReadOnlyAppContext()
              .WhenToldTo
                 (x => x.SelectOne
                   (Arg.Is<GetFinancialTransactionsStatement>(y => y.FinancialServiceKey == FinancialServiceKey)));

        };

        private Because of = () =>
        {
            expectedResult = testDataManager.GetFinancialTransactions(FinancialServiceKey);
        };

        private It should_return_the_correct_statement = () =>
        {
            Assert.AreEqual(expectedResult, testDataManager.GetFinancialTransactions(FinancialServiceKey));
        };
    }
}
