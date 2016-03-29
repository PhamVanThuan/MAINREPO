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
    public class When_furtherlending : WithFakes
    {
        static LTVForFLCaseCommand command;
        static LTVForFLCaseCommandHandler handler;
        static IDomainMessageCollection messages;

        Establish context = () =>
            {
                IApplicationRepository applicationRepository = An<IApplicationRepository>();

                IApplicationFurtherLending app = An<IApplicationFurtherLending>();

                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(app);
                app.WhenToldTo(x => x.EstimatedDisbursedLTV).Return(1);

                messages = new DomainMessageCollection();
                command = new LTVForFLCaseCommand(Param.IsAny<int>());
                handler = new LTVForFLCaseCommandHandler(applicationRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_return_greater_than_minus_one = () =>
            {
                command.LTV.ShouldEqual<double>(1);
            };
    }
}