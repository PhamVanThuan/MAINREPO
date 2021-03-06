﻿using DomainService2.SharedServices.Common;
using DomainService2.Specs.DomainObjects;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Specs.SharedServices.Common.AddReasonsCommandHandlerSpecs
{
    [Subject(typeof(AddReasonsCommandHandler))]
    public class When_reasondefinition_exists : DomainServiceSpec<AddReasonsCommand, AddReasonsCommandHandler>
    {
        protected static IReasonRepository reasonRepository;

        Establish context = () =>
            {
                reasonRepository = An<IReasonRepository>();

                IReasonDefinition reasonDefinition = An<IReasonDefinition>();
                IReadOnlyEventList<IReasonDefinition> reasonDefinitionList = new StubReadOnlyEventList<IReasonDefinition>(new IReasonDefinition[] { reasonDefinition });
                reasonRepository.WhenToldTo(x => x.GetReasonDefinitionsByReasonDescriptionKey(Param.IsAny<ReasonTypes>(), Param.IsAny<int>())).Return(reasonDefinitionList);

                IReason reason = An<IReason>();
                reasonRepository.WhenToldTo(x => x.CreateEmptyReason()).Return(reason);

                command = new AddReasonsCommand(Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<int>());
                handler = new AddReasonsCommandHandler(reasonRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_save_reason = () =>
            {
                reasonRepository.WasToldTo(x => x.SaveReason(Param.IsAny<IReason>()));
            };
    }
}