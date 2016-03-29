using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.SetApplicationEmploymentType
{
    public class when_an_application_has_no_application_information : WithCoreFakes
    {
        private static SetApplicationEmploymentTypeCommand command;
        private static SetApplicationEmploymentTypeCommandHandler commandHandler;
        private static int applicationNumber;
        private static int applicationInformationNumber;
        private static IApplicationDataManager applicationDataManager;
        private static IApplicationManager applicationManager;
        private static IApplicantDataManager applicantDataManager;
        private static OfferInformationDataModel offerInformationDataModel;
        private static OfferInformationVariableLoanDataModel applicationInformationVariableLoan;
        private static IDomainRuleManager<OfferInformationDataModel> domainRuleManager;

        private Establish context = () =>
            {
                applicationInformationVariableLoan = null;
                applicationDataManager = An<IApplicationDataManager>();
                applicationManager = An<IApplicationManager>();
                applicantDataManager = An<IApplicantDataManager>();
                domainRuleManager = An<IDomainRuleManager<OfferInformationDataModel>>();
                applicationNumber = 1234;
                applicationInformationNumber = 4321;

                offerInformationDataModel = new OfferInformationDataModel(applicationInformationNumber, DateTime.Today.AddDays(-1), applicationNumber, (int)OfferInformationType.AcceptedOffer, 
                    "bob", null, null);
                applicationDataManager.WhenToldTo(x => x.GetLatestApplicationOfferInformation(applicationNumber)).Return(offerInformationDataModel);
                applicationDataManager.WhenToldTo(x => x.GetApplicationInformationVariableLoan(offerInformationDataModel.OfferInformationKey)).Return(applicationInformationVariableLoan);

                command = new SetApplicationEmploymentTypeCommand(applicationNumber);
                commandHandler = new SetApplicationEmploymentTypeCommandHandler(unitOfWorkFactory, applicationDataManager, applicationManager, applicantDataManager, eventRaiser, domainRuleManager);

            };

        private Because of = () =>
            {
                messages = commandHandler.HandleCommand(command, serviceRequestMetaData);
            };

        private It should_not_determine_the_application_employmentType = () =>
            {
                applicationManager.WasNotToldTo(x => x.DetermineEmploymentTypeForApplication(Param.IsAny<IEnumerable<EmploymentDataModel>>()));
            };

        private It should_not_update_the_application = () =>
            {
                applicationDataManager.WasNotToldTo(x => x.UpdateApplicationInformationVariableLoan(Param.IsAny<OfferInformationVariableLoanDataModel>()));
            };

        private It should_return_a_message = () =>
            {
                messages.ErrorMessages().First().Message.ShouldContain("No OfferInformationVariableLoan was found");
            };

        private It should_not_raise_the_employment_type_set_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(
                 Param.IsAny<DateTime>()
               , Param.IsAny<ApplicationEmploymentTypeSetEvent>()
               , Param.IsAny<int>()
               , Param.IsAny<int>()
               , Param.IsAny<IServiceRequestMetadata>()
           ));
        };
    }
}