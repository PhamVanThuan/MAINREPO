using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.GetApplicationStatusCommandHandlerSpecs
{
    [Subject(typeof(GetApplicationTypeCommandHandler))]
    public class When_application_not_found_for_applicationkey : DomainServiceSpec<GetApplicationStatusCommand, GetApplicationStatusCommandHandler>
    {
        Establish context = () =>
            {
                var applicationRepository = An<IApplicationRepository>();
                IApplication application = null;

                command = new GetApplicationStatusCommand(Param<int>.IsAnything);
                handler = new GetApplicationStatusCommandHandler(applicationRepository);

                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);
            };


        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_return_zero = () =>
            {
                int defaultApplicationStatusKey = 0;
                command.ApplicationStatusKeyResult.Equals(defaultApplicationStatusKey);
            };
    }
}