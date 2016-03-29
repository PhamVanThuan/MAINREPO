using System;
using DomainService2.SharedServices;
using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace DomainService2.Specs.SharedServices.Common.GetApplicationKeyFromSourceInstanceIDHandlerSpecs
{
    [Subject(typeof(GetApplicationKeyFromSourceInstanceIDCommandHandler))]
    public class When_invalid_instance : DomainServiceSpec<GetApplicationKeyFromSourceInstanceIDCommand, GetApplicationKeyFromSourceInstanceIDCommandHandler>
    {
        protected static IX2WorkflowService x2WorkflowService;
        protected static IX2Repository x2Repository;
        protected static Exception exception;

        Establish context = () =>
        {
            x2WorkflowService = An<IX2WorkflowService>();
            x2Repository = An<IX2Repository>();

            IInstance instance = An<IInstance>();
            x2Repository.WhenToldTo(x => x.GetInstanceByKey(Param.IsAny<long>())).Return(instance);

            command = new GetApplicationKeyFromSourceInstanceIDCommand(Param.IsAny<long>());
            handler = new GetApplicationKeyFromSourceInstanceIDCommandHandler(x2Repository, x2WorkflowService);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => handler.Handle(messages, command));
        };

        It should_return_exception = () =>
        {
            exception.ShouldNotBeNull();
        };
    }
}