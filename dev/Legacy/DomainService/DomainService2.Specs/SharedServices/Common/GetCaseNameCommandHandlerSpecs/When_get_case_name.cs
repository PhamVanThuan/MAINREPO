using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.GetCaseNameCommandHandlerSpecs
{
    [Subject(typeof(GetCaseNameCommandHandler))]
    public class When_get_case_name : DomainServiceSpec<GetCaseNameCommand, GetCaseNameCommandHandler>
    {
        Establish context = () =>
        {
            IApplicationReadOnlyRepository appRepository = An<IApplicationReadOnlyRepository>();
            IApplication application = An<IApplication>();

            appRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>()))
                .Return(application);

            application.WhenToldTo(x => x.GetLegalName(LegalNameFormat.Full)).Return("Case Name");

            command = new GetCaseNameCommand(1);
            handler = new GetCaseNameCommandHandler(appRepository);
        };
        Because of = () =>
        {
            handler.Handle(messages, command);
        };
        It should_return_case_name = () =>
        {
            command.CaseNameResult.ShouldNotBeEmpty();
        };
    }
}