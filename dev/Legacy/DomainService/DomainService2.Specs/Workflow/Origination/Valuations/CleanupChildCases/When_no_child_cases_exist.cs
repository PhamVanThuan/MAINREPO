using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Machine.Fakes;
using DomainService2.Workflow.Origination.Valuations;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.Valuations.CleanupChildCases
{
    [Subject(typeof(CleanupChildCasesCommandHandler))]
    public class When_no_child_cases_exist : CleanupChildCasesCommandHandlerBase
    {
        private static IX2Repository x2Repository;

        Establish context = () =>
        {
            IEventList<IInstance> childInstances = new EventList<IInstance>();

            x2Repository = An<IX2Repository>();
            x2Repository.WhenToldTo(x => x.GetChildInstances(Param.IsAny<long>())).Return(childInstances);

            messages = new DomainMessageCollection();
            command = new CleanupChildCasesCommand(0);
            handler = new CleanupChildCasesCommandHandler(x2Repository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_not_create_and_save_external_activities_to_perform_the_archive_of_anything = () =>
        {
            x2Repository.WasNotToldTo(x => x.CreateAndSaveActiveExternalActivity(Param.IsAny<string>(), Param.IsAny<long>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>()));
        };

    }
}
