using DomainService2.Workflow.Origination.Valuations;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;

namespace DomainService2.Specs.Workflow.Origination.Valuations.GetValuationData
{
    [Subject(typeof(GetValuationDataCommandHandler))]
    public class When_the_application_is_not_a_mortgageloan_application : GetValuationDataCommandHandlerBase
    {
        Establish context = () =>
            {
                messages = new DomainMessageCollection();

                IApplicationLife applicationLife = An<IApplicationLife>();

                applicationRepository = An<IApplicationReadOnlyRepository>();
                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(applicationLife);

                command = new GetValuationDataCommand(0);
                handler = new GetValuationDataCommandHandler(applicationRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_return_an_empty_valuation_dataset = () =>
            {
                command.ValuationDataResult.ShouldBeEmpty();
            };
    }
}