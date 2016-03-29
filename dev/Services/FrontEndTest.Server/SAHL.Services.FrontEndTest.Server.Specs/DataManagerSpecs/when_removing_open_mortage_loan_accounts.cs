using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Fakes;
using SAHL.Services.FrontEndTest.Managers.Statements;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.FETest;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_removing_open_mortage_loan_accounts 
    {
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;
        private static OpenMortgageLoanAccountsDataModel model;
        private static int accountKey;

        private Establish context = () =>
        {
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
            accountKey = 1711511;
            model = new OpenMortgageLoanAccountsDataModel(1,accountKey,true);
        };

        private Because of = () =>
        {
            testDataManager.RemoveOpenMortgageLoanAccount(accountKey);
        };

        private It should_remove_the_correct_Mortgage_loan_account = () =>
        {
            fakeDb.FakedDb.InAppContext().
               WasToldTo(x => x.DeleteWhere<OpenMortgageLoanAccountsDataModel>
                   (string.Format("AccountKey = {0}", accountKey), null));
        };

        private It should_complete_the_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };

    }
}
