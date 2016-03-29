using DomainService2.Workflow.Origination.Valuations;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.Valuations.SendEmailToAllOpenApplicationConsultantsForValuationComplete
{
    [Subject(typeof(SendEmailToAllOpenApplicationConsultantsForValuationCompleteCommandHandler))]
    public class When_valuation_key_is_not_zero : WithFakes
    {
        protected static SendEmailToAllOpenApplicationConsultantsForValuationCompleteCommand command;
        protected static SendEmailToAllOpenApplicationConsultantsForValuationCompleteCommandHandler handler;
        protected static IDomainMessageCollection messages;

        protected static IOrganisationStructureRepository orgStructureRepository;
        protected static IPropertyRepository propertyRepository;
        protected static IMessageService messageService;

        Establish context = () =>
            {
                messageService = An<IMessageService>();

                orgStructureRepository = An<IOrganisationStructureRepository>();

                IApplicationRole branchConsultantD = An<IApplicationRole>();
                branchConsultantD.WhenToldTo(x => x.LegalEntity.EmailAddress).Return("bcd1@sahomeloans.com");
                orgStructureRepository.WhenToldTo(x => x.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(1, (int)SAHL.Common.Globals.OfferRoleTypes.BranchConsultantD, (int)SAHL.Common.Globals.GeneralStatuses.Active)).Return(branchConsultantD);

                IApplicationRole newBusinessProcessorD = An<IApplicationRole>();
                newBusinessProcessorD.WhenToldTo(x => x.LegalEntity.EmailAddress).Return("nbp1@sahomeloans.com");
                orgStructureRepository.WhenToldTo(x => x.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(1, (int)SAHL.Common.Globals.OfferRoleTypes.NewBusinessProcessorD, (int)SAHL.Common.Globals.GeneralStatuses.Active)).Return(newBusinessProcessorD);

                IApplicationRole flProcessorD = An<IApplicationRole>();
                flProcessorD.WhenToldTo(x => x.LegalEntity.EmailAddress).Return("flp1@sahomeloans.com");
                orgStructureRepository.WhenToldTo(x => x.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(1, (int)SAHL.Common.Globals.OfferRoleTypes.FLProcessorD, (int)SAHL.Common.Globals.GeneralStatuses.Active)).Return(flProcessorD);

                IApplicationRole branchAdminD = An<IApplicationRole>();
                branchAdminD.WhenToldTo(x => x.LegalEntity.EmailAddress).Return("ba1@sahomeloans.com");
                orgStructureRepository.WhenToldTo(x => x.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(1, (int)SAHL.Common.Globals.OfferRoleTypes.BranchAdminD, (int)SAHL.Common.Globals.GeneralStatuses.Active)).Return(branchAdminD);

                IValuation valuation = An<IValuation>();
                valuation.WhenToldTo(x => x.Property.Key).Return(1);

                propertyRepository = An<IPropertyRepository>();
                propertyRepository.WhenToldTo(x => x.GetValuationByKey(Param.IsAny<int>())).Return(valuation);

                IApplicationMortgageLoan appMortgageLoan = An<IApplicationMortgageLoan>();
                appMortgageLoan.WhenToldTo(x => x.Key).Return(1);
                appMortgageLoan.WhenToldTo(x => x.IsOpen).Return(true);

                IEventList<IApplication> applications = new EventList<IApplication>();
                applications.Add(null, appMortgageLoan);
                propertyRepository.WhenToldTo(x => x.GetApplicationsForProperty(Param.IsAny<int>())).Return(applications);

                messages = new DomainMessageCollection();
                command = new SendEmailToAllOpenApplicationConsultantsForValuationCompleteCommand(99999, 0);
                handler = new SendEmailToAllOpenApplicationConsultantsForValuationCompleteCommandHandler(orgStructureRepository, propertyRepository, messageService);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_retrieve_the_valuation_from_the_db = () =>
            {
                propertyRepository.WasToldTo(x => x.GetValuationByKey(99999));
            };

        It should_retrieve_all_applications_for_the_property_linked_to_the_valuation = () =>
            {
                propertyRepository.WasToldTo(x => x.GetApplicationsForProperty(Param.IsAny<int>()));
            };

        It should_send_valuation_completed_email = () =>
        {
            messageService.WasToldTo(x => x.SendEmailInternal(Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>()));
        };
    }
}