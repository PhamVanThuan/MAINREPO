using System.Collections.Generic;
using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.ArchiveChildInstancesCommandHandlerSpecs
{
    [Subject(typeof(ArchiveChildInstancesCommandHandler))]
    public class When_no_child_instances_exist_for_instance : WithFakes
    {
        protected static ArchiveChildInstancesCommand command;
        protected static ArchiveChildInstancesCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static IX2Repository x2Repository;

        // Arrange
        Establish context = () =>
        {
            messages = An<IDomainMessageCollection>();
            x2Repository = An<IX2Repository>();
            List<IInstance> instanceList = new List<IInstance>();
            IEventList<IInstance> instances = new EventList<IInstance>(instanceList);

            x2Repository.WhenToldTo(x => x.GetChildInstances(Param<long>.IsAnything)).Return(instances);

            command = new ArchiveChildInstancesCommand(Param<long>.IsAnything, Param<string>.IsAnything);
            handler = new ArchiveChildInstancesCommandHandler(x2Repository);
        };

        // Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_not_archive_any_instances = () =>
        {
            x2Repository.WasNotToldTo(x => x.CreateAndSaveActiveExternalActivity(Param<string>.IsAnything, Param<long>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything));
        };
    }
}