using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Application;
using System;

namespace SAHL.Services.Capitec.Specs.ApplicationDataServiceSpecs
{
    public class when_getting_the_next_available_application_number_and_they_are_all_used : WithFakes
    {
        static ApplicationDataManager service;
        static ReservedApplicationNumberDataModel model;
        static Exception exception;
        private static FakeDbFactory dbFactory;

        Establish context = () =>
        {
            model = null;
            dbFactory = new FakeDbFactory();
            service = new ApplicationDataManager(dbFactory);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<GetNextApplicationNumberQuery>())).Return(model);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() =>
            {
                service.GetNextApplicationNumber();
            });
        };

        It should_throw_a_null_reference_exception = () =>
        {
            exception.ShouldBeOfExactType(typeof(NullReferenceException));
        };

        It should_throw_a_custom_error_message = () =>
        {
            exception.Message.ShouldEqual("No reserved application numbers are available.");
        };
    }
}
