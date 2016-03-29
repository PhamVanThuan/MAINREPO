using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Machine.Fakes;
using DomainService2.Workflow.Origination.Valuations;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.Valuations.CleanupChildCases
{
    [Subject(typeof(CleanupChildCasesCommandHandler))]
    public class When_child_cases_exist : CleanupChildCasesCommandHandlerBase
    {
        private static IX2Repository x2Repository;

        Establish context = () =>
            {
                
                IInstance case1 = An<IInstance>();
                case1.WhenToldTo(x => x.ID).Return(100010001L);

                IInstance case2 = An<IInstance>();
                case2.WhenToldTo(x => x.ID).Return(100020002L);

                IInstance case3 = An<IInstance>();
                case3.WhenToldTo(x => x.ID).Return(100030003L);

                IEventList<IInstance> childInstances = new EventList<IInstance>();
                childInstances.Add(null, case1);
                childInstances.Add(null, case2);
                childInstances.Add(null, case3);
  
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

        It should_create_and_save_external_activities_to_perform_the_archive_of_each_case = () =>
            {
                x2Repository.WasToldTo(x => x.CreateAndSaveActiveExternalActivity(SAHL.Common.Constants.WorkFlowExternalActivity.CloneCleanupArchive, 100010001L, SAHL.Common.Constants.WorkFlowName.Valuations, SAHL.Common.Constants.WorkFlowProcessName.Origination, null));
                x2Repository.WasToldTo(x => x.CreateAndSaveActiveExternalActivity(SAHL.Common.Constants.WorkFlowExternalActivity.CloneCleanupArchive, 100020002L, SAHL.Common.Constants.WorkFlowName.Valuations, SAHL.Common.Constants.WorkFlowProcessName.Origination, null));
                x2Repository.WasToldTo(x => x.CreateAndSaveActiveExternalActivity(SAHL.Common.Constants.WorkFlowExternalActivity.CloneCleanupArchive, 100030003L, SAHL.Common.Constants.WorkFlowName.Valuations, SAHL.Common.Constants.WorkFlowProcessName.Origination, null));
            };
    }
}
