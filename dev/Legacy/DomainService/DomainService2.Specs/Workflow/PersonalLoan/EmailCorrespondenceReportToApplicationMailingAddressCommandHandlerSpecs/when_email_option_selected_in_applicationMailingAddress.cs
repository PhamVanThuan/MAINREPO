using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using Machine.Specifications;
using DomainService2.Workflow.PersonalLoan;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.Globals;

namespace DomainService2.Specs.Workflow.PersonalLoan.EmailCorrespondenceReportToApplicationMailingAddressCommandHandlerSpecs
{
    [Subject(typeof(EmailCorrespondenceReportToApplicationMailingAddressCommandHandler))]
    public class when_email_option_selected_in_applicationMailingAddress : DomainServiceSpec<EmailCorrespondenceReportToApplicationMailingAddressCommand,EmailCorrespondenceReportToApplicationMailingAddressCommandHandler>
    {
        private static ICorrespondenceRepository correspondenceRepository;
        private static IApplicationRepository applicationRepository;

        Establish context = () =>
            {
                correspondenceRepository= An<ICorrespondenceRepository>();
                applicationRepository = An<IApplicationRepository>();

                IApplication application = An<IApplication>();
                IAccountSequence accSequence = An<IAccountSequence>();
                IApplicationMailingAddress appMailingAddress = An<IApplicationMailingAddress>();
                IEventList<IApplicationMailingAddress> appMailingAddresses = new EventList<IApplicationMailingAddress>();
                ILegalEntity legalEntity = An<ILegalEntity>();
                ICorrespondenceMedium cm = An<ICorrespondenceMedium>();

                accSequence.WhenToldTo(x => x.Key).Return(1);
                application.WhenToldTo(x => x.ReservedAccount).Return(accSequence);
                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

                legalEntity.WhenToldTo(x => x.Key).Return(1);
                legalEntity.WhenToldTo(x => x.EmailAddress).Return("test@test.com");

                cm.WhenToldTo(x => x.Key).Return((int)SAHL.Common.Globals.CorrespondenceMediums.Email);

                appMailingAddress.WhenToldTo(x => x.LegalEntity).Return(legalEntity);
                appMailingAddress.WhenToldTo(x => x.CorrespondenceMedium).Return(cm);
                appMailingAddresses.Add(messages,appMailingAddress);
                


                application.WhenToldTo(x => x.ApplicationMailingAddresses).Return(appMailingAddresses);
                correspondenceRepository.WhenToldTo(x => x.SendCorrespondenceReportToLegalEntity(Param<ISendCorrespondenceRequest>.IsAnything));

                command = new EmailCorrespondenceReportToApplicationMailingAddressCommand(Param<int>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything, Param<CorrespondenceTemplates>.IsAnything);
                handler = new EmailCorrespondenceReportToApplicationMailingAddressCommandHandler(correspondenceRepository, applicationRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages,command);
            };

        It should_send_the_correspondence_report = () =>
            {
                correspondenceRepository.WasToldTo(x => x.SendCorrespondenceReportToLegalEntity(Param<ISendCorrespondenceRequest>.IsAnything));
            };
    }
}
