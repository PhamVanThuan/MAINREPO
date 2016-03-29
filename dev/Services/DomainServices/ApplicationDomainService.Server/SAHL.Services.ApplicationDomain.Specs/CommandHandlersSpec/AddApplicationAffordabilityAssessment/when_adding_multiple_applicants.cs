using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM.Managers.ADUser;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplicationAffordabilityAssessment
{
    public class when_adding_multiple_applicants : WithCoreFakes
    {
        private static AddApplicationAffordabilityAssessmentCommand command;
        private static AddApplicationAffordabilityAssessmentCommandHandler handler;

        private static AffordabilityAssessmentModel affordabilityAssessmentModel;

        private static int applicationNumber;
        private static List<int> contributingApplicants;
        private static int noOfHouseHoldDependants;
        private static int noOfContributingApplicants;
        private static int affordabilityAssessmentKey;
        private static string adUser;
        private static int userLastAmendedID;

        private static IAffordabilityAssessmentManager affordabilityAssessmentManager;
        private static IADUserManager aduserManager;
        private static IDomainRuleManager<AffordabilityAssessmentModel> domainRuleManager;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<AffordabilityAssessmentModel>>();

            applicationNumber = 123456;

            contributingApplicants = new List<int> { 142233, 153344, 145566 };

            affordabilityAssessmentKey = 1123;

            noOfHouseHoldDependants = 2;
            noOfContributingApplicants = 3;
            adUser = "SAHL\\BCUser3";
            userLastAmendedID = 90;

            serviceRequestMetaData = new ServiceRequestMetadata();
            serviceRequestMetaData.Add(ServiceRequestMetadata.HEADER_USERNAME, adUser);

            affordabilityAssessmentModel = new AffordabilityAssessmentModel(0,
                                                                            applicationNumber,
                                                                            AffordabilityAssessmentStatus.Unconfirmed,
                                                                            DateTime.Now,
                                                                            noOfHouseHoldDependants,
                                                                            noOfContributingApplicants,
                                                                            0,
                                                                            contributingApplicants,
                                                                            null, null);

            aduserManager = An<IADUserManager>();
            aduserManager.WhenToldTo(x => x.GetAdUserKeyByUserName(adUser)).Return(userLastAmendedID);

            affordabilityAssessmentManager = An<IAffordabilityAssessmentManager>();
            affordabilityAssessmentManager.WhenToldTo(x => x.CreateAffordabilityAssessment(Param.IsAny<AffordabilityAssessmentModel>(), Param.IsAny<int>()))
                                          .Return(affordabilityAssessmentKey);

            command = new AddApplicationAffordabilityAssessmentCommand(affordabilityAssessmentModel);
            handler = new AddApplicationAffordabilityAssessmentCommandHandler(domainRuleManager, aduserManager, affordabilityAssessmentManager, eventRaiser);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_create_the_affordability_assessment_against_the_application = () =>
        {
            affordabilityAssessmentManager.WasToldTo(x => x.CreateAffordabilityAssessment(affordabilityAssessmentModel, userLastAmendedID));
        };

        private It should_link_each_legal_entity_to_the_affordability_assessment = () =>
        {
            foreach (var applicantLegalEntityKey in contributingApplicants)
            {
                affordabilityAssessmentManager.WasToldTo(x => x.LinkLegalEntityToAffordabilityAssessment(affordabilityAssessmentKey, applicantLegalEntityKey));
            }
        };
    }
}