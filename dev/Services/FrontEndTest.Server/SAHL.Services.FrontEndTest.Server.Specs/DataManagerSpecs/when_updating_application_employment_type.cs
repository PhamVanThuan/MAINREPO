using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.FrontEndTest.Managers.Statements;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_updating_application_employment_type : WithFakes
    {
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;
        private static int ApplicationNumber;
        private static EmploymentType employmentType;

        private Establish context = () =>
        {
            fakeDb = new FakeDbFactory();
            ApplicationNumber = 1492075;
            employmentType = EmploymentType.Salaried;
            testDataManager = new TestDataManager(fakeDb);
        };

        private Because of = () =>
        {
            testDataManager.UpdateApplicationEmploymentType(ApplicationNumber, employmentType);
        };

        private It should_use_the_correct_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };

        private It should_perform_the_update_using_the_correct_data = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo
              (x => x.Update<OfferInformationVariableLoanDataModel>
                 (Arg.Is<UpdateApplicationEmploymentTypeStatement>(y => y.ApplicationNumber == ApplicationNumber
                      && y.EmploymentTypeKey == (int)employmentType)));
        };
    }
}