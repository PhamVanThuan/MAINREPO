using DomainService2.Workflow.PersonalLoan;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Specs.Workflow.PersonalLoan.ApplicationHasSAHLLifeAppliedCommandHandlerSpecs
{
    [Subject(typeof(ApplicationHasSAHLLifeAppliedCommandHandler))]
    public class when_application_has_sahl_life_applied : DomainServiceSpec<ApplicationHasSAHLLifeAppliedCommand, ApplicationHasSAHLLifeAppliedCommandHandler>
    {
        static IApplicationUnsecuredLendingRepository applicationUnsecuredLendingRepository;

        Establish context = () =>
        {
            applicationUnsecuredLendingRepository = An<IApplicationUnsecuredLendingRepository>();
            applicationUnsecuredLendingRepository.WhenToldTo(r => r.ApplicationHasSAHLLifeApplied(Param.IsAny<int>())).Return(true);
            command = new ApplicationHasSAHLLifeAppliedCommand(Param.IsAny<int>());
            handler = new ApplicationHasSAHLLifeAppliedCommandHandler(applicationUnsecuredLendingRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_check_if_sahl_life_is_applied = () =>
        {
            applicationUnsecuredLendingRepository.WasToldTo(x => x.ApplicationHasSAHLLifeApplied(Param.IsAny<int>()));
        };

        It should_tell_that_sahl_life_is_applied = () =>
        {
            command.Result.ShouldBeTrue();
        };

        
    }
}
