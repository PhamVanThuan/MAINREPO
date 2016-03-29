using System.Collections.Generic;
using DomainService2.SharedServices;
using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace DomainService2.Specs.SharedServices.Common.GetApplicationKeyFromSourceInstanceIDHandlerSpecs
{
    [Subject(typeof(GetApplicationKeyFromSourceInstanceIDCommandHandler))]
    public class When_GetApplicationIDFromSourceInstanceID : DomainServiceSpec<GetApplicationKeyFromSourceInstanceIDCommand, GetApplicationKeyFromSourceInstanceIDCommandHandler>
    {
        protected static IX2WorkflowService x2WorkflowService;
        protected static IX2Repository x2Repository;

        Establish context = () =>
        {
            x2WorkflowService = An<IX2WorkflowService>();
            x2Repository = An<IX2Repository>();

            IInstance instance = An<IInstance>();
            x2Repository.WhenToldTo(x => x.GetInstanceByKey(Param.IsAny<long>())).Return(instance);
            instance.WhenToldTo(x => x.SourceInstanceID).Return(1);
            instance.WhenToldTo(x => x.ID).Return(1);

            IDictionary<string, object> data = new Dictionary<string, object>();
            data.Add("ApplicationKey", "1");
            x2WorkflowService.WhenToldTo(x => x.GetX2DataRow(Param.IsAny<long>())).Return(data);

            command = new GetApplicationKeyFromSourceInstanceIDCommand(Param.IsAny<long>());
            handler = new GetApplicationKeyFromSourceInstanceIDCommandHandler(x2Repository, x2WorkflowService);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_return_not_null_application_key = () =>
            {
                command.ApplicationKeyResult.ShouldNotBeNull();
            };
    }
}