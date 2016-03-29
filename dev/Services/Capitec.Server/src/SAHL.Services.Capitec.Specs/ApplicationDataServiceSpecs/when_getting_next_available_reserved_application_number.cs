using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Application;


namespace SAHL.Services.Capitec.Specs.ApplicationDataServiceSpecs
{
    public class when_getting_next_available_reserved_application_number : WithFakes
    {
        private static ApplicationDataManager service;
        private static ReservedApplicationNumberDataModel model;
        private static int applicationNumber;
        private static FakeDbFactory dbFactory;

        Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            model = new ReservedApplicationNumberDataModel(1234567, false);
            service = new ApplicationDataManager(dbFactory);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<GetNextApplicationNumberQuery>())).Return(model);
        };

        Because of = () =>
        {
            applicationNumber = service.GetNextApplicationNumber();
        };

        It should_return_the_application_number = () =>
        {
            applicationNumber.ShouldEqual(model.ApplicationNumber);
        };
    }
}