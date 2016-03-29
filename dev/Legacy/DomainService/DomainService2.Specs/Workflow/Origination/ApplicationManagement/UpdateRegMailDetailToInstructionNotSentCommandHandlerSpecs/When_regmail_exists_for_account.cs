using System;
using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.UpdateRegMailDetailToInstructionNotSentCommandHandlerSpecs
{
    [Subject(typeof(UpdateRegMailDetailToInstructionNotSentCommandHandler))]
    public class When_regmail_exists_for_account : WithFakes
    {
        protected static UpdateRegMailDetailToInstructionNotSentCommand command;
        protected static UpdateRegMailDetailToInstructionNotSentCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static IApplicationRepository applicationRepository;
        protected static IRegistrationRepository registrationRepository;

        Establish context = () =>
            {
                messages = An<IDomainMessageCollection>();
                applicationRepository = An<IApplicationRepository>();
                registrationRepository = An<IRegistrationRepository>();

                IApplication application = An<IApplication>();
                IAccountSequence accountSequence = An<IAccountSequence>();
                accountSequence.WhenToldTo(x => x.Key).Return(1);
                application.WhenToldTo(x => x.ReservedAccount).Return(accountSequence);

                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

                IRegMail regmail = An<IRegMail>();
                regmail.WhenToldTo(x => x.DetailTypeNumber).Return(1);
                regmail.WhenToldTo(x => x.RegMailDateTime).Return(new DateTime());

                registrationRepository.WhenToldTo(x => x.GetRegmailByAccountKey(Param<int>.IsAnything)).Return(regmail);
                registrationRepository.WhenToldTo(x => x.SaveRegmail(Param<IRegMail>.IsAnything));

                command = new UpdateRegMailDetailToInstructionNotSentCommand(Param<int>.IsAnything);
                handler = new UpdateRegMailDetailToInstructionNotSentCommandHandler(applicationRepository, registrationRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_update_and_save_the_regmail_record = () =>
            {
                registrationRepository.WasToldTo(x => x.SaveRegmail(Param<IRegMail>.IsAnything));
            };
    }
}