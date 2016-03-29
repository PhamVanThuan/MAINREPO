using DomainService2.Workflow.Origination.FurtherLending;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.FurtherLending.LTVForFLCaseCommandHandlerSpecs
{
    [Subject(typeof(LTVForFLCaseCommandHandler))]
    public class When_non_furtherlending : WithFakes
    {
        static LTVForFLCaseCommand command;
        static LTVForFLCaseCommandHandler handler;
        static IDomainMessageCollection messages;

        Establish context = () =>
        {
            IApplicationRepository applicationRepository = An<IApplicationRepository>();

            IApplication app = An<IApplication>();

            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(app);

            messages = new DomainMessageCollection();
            command = new LTVForFLCaseCommand(Param.IsAny<int>());
            handler = new LTVForFLCaseCommandHandler(applicationRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_return_minus_one = () =>
        {
            command.LTV.ShouldEqual<double>(-1);
        };
    }
}