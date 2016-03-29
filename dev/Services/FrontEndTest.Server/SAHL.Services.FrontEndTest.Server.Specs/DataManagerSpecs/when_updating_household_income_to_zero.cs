using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.FrontEndTest.Managers.Statements;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_updating_household_income_to_zero : WithFakes
    {
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;
        private static int applicationNumber;
        private static int householdIncome;

        private Establish context = () =>
        {
            applicationNumber = 123456789;
            householdIncome = 0;
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
        };

        private Because of = () =>
        {
            testDataManager.UpdateHouseholdIncomeToZero(applicationNumber);
        };

        private It should_update_using_the_correct_application_number_and_householdIncome = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Update<OfferInformationVariableLoanDataModel>
                (Arg.Is<UpdateHouseholdIncomeStatement>(y => y.ApplicationNumber == applicationNumber && y.HouseholdIncome == householdIncome)));
        };
        
        It should_complete_the_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x=>x.Complete());   
        };
		

    }
}