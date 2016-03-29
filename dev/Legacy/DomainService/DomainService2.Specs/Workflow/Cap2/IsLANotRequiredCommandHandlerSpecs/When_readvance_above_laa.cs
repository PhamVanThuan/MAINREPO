using DomainService2.Workflow.Cap2;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.Cap2.IsLANotRequiredCommandHandlerSpecs
{
    [Subject(typeof(IsLANotRequiredCommandHandler))]
    public class When_readvance_above_laa : DomainServiceSpec<IsLANotRequiredCommand, IsLANotRequiredCommandHandler>
    {
        Establish context = () =>
            {
                ICapRepository capRepository = An<ICapRepository>();
                ICapApplication application = An<ICapApplication>();

                capRepository.WhenToldTo(x => x.GetCapOfferByKey(Param.IsAny<int>()))
                    .Return(application);

                capRepository.WhenToldTo(x => x.IsReAdvanceLAA(application))
                    .Return(false);

                command = new IsLANotRequiredCommand(1111);
                handler = new IsLANotRequiredCommandHandler(capRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_return_false = () =>
            {
                command.Result.ShouldBeFalse();
            };
    }
}