using System;
using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.GetnWorkingDaysFromTodayCommandHandlerSpecs
{
    [Subject(typeof(GetnWorkingDaysFromTodayCommandHandler))]
    public class When_GetnWorkingDaysFromToday : DomainServiceSpec<GetnWorkingDaysFromTodayCommand, GetnWorkingDaysFromTodayCommandHandler>
    {
        static DateTime result = DateTime.Now.AddDays(12);
        static int days = 12;
        Establish context = () =>
            {
                ICommonRepository commonRepository = An<ICommonRepository>();
                commonRepository.WhenToldTo(x => x.GetnWorkingDaysFromToday(days))
                    .Return(result);

                command = new GetnWorkingDaysFromTodayCommand(days);
                handler = new GetnWorkingDaysFromTodayCommandHandler(commonRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_set_the_return_value = () =>
            {
                command.Result.ShouldEqual<DateTime>(result);
            };
    }
}