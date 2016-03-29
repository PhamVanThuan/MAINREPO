using DomainService2.Workflow.Cap2;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.Cap2.IsCreditCheckRequiredCommandHandlerSpecs
{
    [Subject(typeof(IsCreditCheckRequiredCommandHandler))]
    public class When_above_ltvthreshold : DomainServiceSpec<IsCreditCheckRequiredCommand, IsCreditCheckRequiredCommandHandler>
    {
        Establish context = () =>
            {
                ICapRepository capRepository = An<ICapRepository>();
                ICapApplication application = An<ICapApplication>();

                capRepository.WhenToldTo(x => x.GetCapOfferByKey(Param.IsAny<int>()))
                    .Return(application);

                capRepository.WhenToldTo(x => x.CheckLTVThreshold(application)).
                    Return(false);

                command = new IsCreditCheckRequiredCommand(1111);
                handler = new IsCreditCheckRequiredCommandHandler(capRepository);
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