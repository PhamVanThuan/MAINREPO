using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Affordability;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplicantAffordability
{
    public class when_adding_affordabilities : WithCoreFakes
    {
        private static AddApplicantAffordabilitiesCommandHandler commandHandler;
        private static AddApplicantAffordabilitiesCommand command;
        private static IApplicationDataManager applicationDataManager;
        private static IApplicantDataManager applicantDataManager;
        private static IAffordabilityDataManager affordabilityDataManager;
        private static ApplicantAffordabilityModel applicantAffordabilityModel;
        private static IEnumerable<AffordabilityTypeModel> clientAffordabilityAssessment;
        private static IDomainRuleManager<ApplicantAffordabilityModel> affordabilityDomainRuleManager;
        private static int clientKey;
        private static int applicationNumber;

        private Establish context = () =>
        {
            clientKey = 2323;
            applicationNumber = 1211;

            clientAffordabilityAssessment = new AffordabilityTypeModel[] {
                  new AffordabilityTypeModel(AffordabilityType.Childsupport,3000000, ""),
                  new AffordabilityTypeModel(AffordabilityType.BondPayments,50000000, "This is a description")
            };
            applicantAffordabilityModel = new ApplicantAffordabilityModel(clientAffordabilityAssessment, clientKey, applicationNumber);
            affordabilityDomainRuleManager = An<IDomainRuleManager<ApplicantAffordabilityModel>>();
            applicationDataManager = An<IApplicationDataManager>();
            affordabilityDataManager = An<IAffordabilityDataManager>();
            applicantDataManager = An<IApplicantDataManager>();
            command = new AddApplicantAffordabilitiesCommand(applicantAffordabilityModel);
            commandHandler = new AddApplicantAffordabilitiesCommandHandler(affordabilityDomainRuleManager, applicantDataManager, eventRaiser, unitOfWorkFactory, affordabilityDataManager);

            applicantDataManager.WhenToldTo(x => x.CheckClientIsAnApplicantOnTheApplication(clientKey, applicationNumber)).Return(true);
            affordabilityDomainRuleManager.WhenToldTo(x => x.ExecuteRules(messages, command.ApplicantAffordabilityModel)).
                Callback<ISystemMessageCollection>(y => { SystemMessageCollection.Empty(); });
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, new ServiceRequestMetadata());
        };

        private It should_check_that_the_applicant_plays_a_role_on_the_application = () =>
        {
            applicantDataManager
                .WasToldTo(x => x.CheckClientIsAnApplicantOnTheApplication(command.ApplicantAffordabilityModel.ClientKey, command.ApplicantAffordabilityModel.ApplicationNumber))
                .OnlyOnce();
        };

        private It should_check_that_the_affordability_descriptions_are_required = () =>
        {
            affordabilityDomainRuleManager.WasToldTo(x => x.ExecuteRules(messages, command.ApplicantAffordabilityModel));
        };

        private It should_save_the_applicant_affordabilities = () =>
        {
            affordabilityDataManager
                .WasToldTo(x => x.SaveAffordability(Param.IsAny<AffordabilityTypeModel>(), clientKey, applicationNumber))
                .Times(command.ApplicantAffordabilityModel.ClientAffordabilityAssessment.Count());
        };

        private It should_raise_applicant_affordabilities_added_event = () =>
        {
            eventRaiser
                .WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), 
                                             Param.IsAny<ApplicantAffordabilitiesAddedEvent>(), 
                                             Param.IsAny<int>(), 
                                             Param.IsAny<int>(), 
                                             Param.IsAny<IServiceRequestMetadata>()))
                .OnlyOnce();
        };
    }
}