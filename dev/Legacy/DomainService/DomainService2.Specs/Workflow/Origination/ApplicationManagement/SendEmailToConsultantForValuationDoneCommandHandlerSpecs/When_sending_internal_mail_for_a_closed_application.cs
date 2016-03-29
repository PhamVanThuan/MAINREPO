using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.SendEmailToConsultantForValuationDoneCommandHandlerSpecs
{
    [Subject(typeof(SendEmailToConsultantForValuationDoneCommandHandler))]
    public partial class When_sending_internal_mail_for_a_closed_application : WithFakes
    {
        protected static SendEmailToConsultantForValuationDoneCommand command;
        protected static SendEmailToConsultantForValuationDoneCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static IApplicationRepository applicationRepository;
        protected static IPropertyRepository propertyRepository;
        protected static IMessageService messageService;

        Establish context = () =>
        {
            messageService = An<IMessageService>();
            propertyRepository = An<IPropertyRepository>();
            applicationRepository = An<IApplicationRepository>();
            IApplicationMortgageLoan applicationMortgageLoan = An<IApplicationMortgageLoan>();
            IProperty property = An<IProperty>();
            IApplication application = An<IApplication>();
            IApplicationRole applicationRole = An<IApplicationRole>();
            IApplicationRoleType applicationRoleType = An<IApplicationRoleType>();
            ILegalEntity legalEntity = An<ILegalEntity>();
            IReadOnlyEventList<IApplicationRole> applicationRoles = null;
            IEventList<IApplication> applications = null;
            IGeneralStatus generalStatus = An<IGeneralStatus>();

            legalEntity.WhenToldTo(x => x.EmailAddress).Return("test");
            applicationRoleType.WhenToldTo(x => x.Key).Return((int)OfferRoleTypes.NewBusinessProcessorD);
            generalStatus.WhenToldTo(x => x.Key).Return((int)GeneralStatuses.Active);
            applicationRole.WhenToldTo(x => x.ApplicationRoleType).Return(applicationRoleType);
            applicationRole.WhenToldTo(x => x.GeneralStatus).Return(generalStatus);
            applicationRole.WhenToldTo(x => x.LegalEntity).Return(legalEntity);
            applicationRoles = new ReadOnlyEventList<IApplicationRole>(new List<IApplicationRole> { applicationRole });
            application.WhenToldTo(x => x.ApplicationRoles).Return(applicationRoles);
            application.WhenToldTo(x => x.IsOpen).Return(false);
            applications = new EventList<IApplication>(new List<IApplication> { application });

            propertyRepository.WhenToldTo(x => x.GetApplicationsForProperty(Param<int>.IsAnything)).Return(applications);

            property.WhenToldTo(x => x.Key).Return(1);

            applicationMortgageLoan.WhenToldTo(x => x.Property).Return(property);
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(applicationMortgageLoan);

            messageService.WhenToldTo(x => x.SendEmailInternal(Param<string>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything)).Return(true);

            command = new SendEmailToConsultantForValuationDoneCommand(Param<int>.IsAnything);
            handler = new SendEmailToConsultantForValuationDoneCommandHandler(applicationRepository, propertyRepository, messageService);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_send_an_internal_mail = () =>
        {
            messageService.WasNotToldTo(x => x.SendEmailInternal(Param<string>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything, Param<string>.IsAnything));
        };
    }
}