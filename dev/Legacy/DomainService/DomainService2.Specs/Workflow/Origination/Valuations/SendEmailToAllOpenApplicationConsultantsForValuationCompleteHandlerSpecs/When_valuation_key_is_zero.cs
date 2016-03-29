using DomainService2.Workflow.Origination.Valuations;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.Valuations.SendEmailToAllOpenApplicationConsultantsForValuationComplete
{
    [Subject(typeof(SendEmailToAllOpenApplicationConsultantsForValuationCompleteCommandHandler))]
    public class When_valuation_key_is_zero : WithFakes
    {
        protected static SendEmailToAllOpenApplicationConsultantsForValuationCompleteCommand command;
        protected static SendEmailToAllOpenApplicationConsultantsForValuationCompleteCommandHandler handler;
        protected static IDomainMessageCollection messages;

        protected static IOrganisationStructureRepository orgStructureRepository;
        protected static IPropertyRepository propertyRepository;
        protected static IMessageService messageService;

        Establish context = () =>
            {
                orgStructureRepository = An<IOrganisationStructureRepository>();
                propertyRepository = An<IPropertyRepository>();
                messageService = An<IMessageService>();


                var legalEntity = An<ILegalEntity>();
                legalEntity.WhenToldTo(x => x.EmailAddress).Return("bcuser@sahomeloans.com");
                var appRole = An<IApplicationRole>();
                appRole.WhenToldTo(x => x.LegalEntity).Return(legalEntity);

                orgStructureRepository.WhenToldTo(x=>x.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<int>()))
                                                .Return(appRole);

                messages = new DomainMessageCollection();
                command = new SendEmailToAllOpenApplicationConsultantsForValuationCompleteCommand(0, 99999);
                handler = new SendEmailToAllOpenApplicationConsultantsForValuationCompleteCommandHandler(orgStructureRepository, propertyRepository, messageService);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_use_the_application_key = () =>
            {
                orgStructureRepository.WasToldTo(x => x.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(99999, (int)SAHL.Common.Globals.OfferRoleTypes.BranchConsultantD, (int)SAHL.Common.Globals.GeneralStatuses.Active));
            };

        It should_send_valuation_completed_email = () =>
            {
                messageService.WasToldTo(x => x.SendEmailInternal(Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>()));
            };
    }
}