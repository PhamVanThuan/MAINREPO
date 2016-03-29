using DomainService2.Workflow.Origination.ApplicationCapture;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.Origination.ApplicationCapture.CreateNetLeadCommandHandlerSpecs
{
    [Subject(typeof(CreateInternetApplicationCommandHandler))]
    class when_the_internet_lead_is_valid : DomainServiceSpec<CreateInternetLeadCommand, CreateInternetLeadCommandHandler>
    {
        private static IApplicationRepository applicationRepo;

        Establish context = () =>
        {
            applicationRepo = An<IApplicationRepository>();
            ILeadInputInformation leadInput = An<ILeadInputInformation>();
            int i = Param<int>.IsAnything;
            applicationRepo.WhenToldTo(x => x.DeserializeNetLeadXML(Param<string>.IsAnything)).Return(leadInput);
            applicationRepo.WhenToldTo(x => x.GenerateLeadFromWeb(i, leadInput)).Return(true);

            command = new CreateInternetLeadCommand(Param.IsAny<int>(), Param.IsAny<string>());
            handler = new CreateInternetLeadCommandHandler(applicationRepo);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_return_true = () =>
        {
            command.Result.ShouldBeTrue();
        };
    }
}
