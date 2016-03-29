using System.Collections.Generic;
using DomainService2.SharedServices;
using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace DomainService2.Specs.SharedServices.Common.UpdateParentVarsCommandHandlerSpecs
{
    [Subject(typeof(UpdateParentVarsCommandHandler))]
    public class When_valid_instance : DomainServiceSpec<UpdateParentVarsCommand, UpdateParentVarsCommandHandler>
    {
        protected static IX2Repository x2Repository;
        protected static IX2WorkflowService x2WorkflowService;

        Establish context = () =>
            {
                x2Repository = An<IX2Repository>();
                x2WorkflowService = An<IX2WorkflowService>();
                IInstance iChild = An<IInstance>();
                IInstance iParentInstance = An<IInstance>();
                iChild.WhenToldTo(x => x.ParentInstance).Return(iParentInstance);
                iParentInstance.WhenToldTo(x => x.ID).Return(Param.IsAny<long>());

                x2Repository.WhenToldTo(x => x.GetInstanceByKey(Param.IsAny<long>())).Return(iChild);

                command = new UpdateParentVarsCommand(Param.IsAny<long>(), Param.IsAny<Dictionary<string, object>>());
                handler = new UpdateParentVarsCommandHandler(x2WorkflowService, x2Repository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_set_datarow = () =>
            {
                x2WorkflowService.WasToldTo(x => x.SetX2DataRow(Param.IsAny<long>(), Param.IsAny<IDictionary<string, object>>()));
            };
    }
}