using DomainService2.Specs.DomainObjects;
using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using System;
using System.Collections.Generic;

namespace DomainService2.Specs.SharedServices.Common.ConfirmApplicationAffordabilityAssessmentsCommandHandlerSpecs
{
    [Subject(typeof(ConfirmApplicationAffordabilityAssessmentsCommandHandler))]
    public class When_confirming_an_unconfirmed_affordability_assessment : WithFakes
    {
        protected static IDomainMessageCollection messages;
        protected static ConfirmApplicationAffordabilityAssessmentsCommand command;
        protected static ConfirmApplicationAffordabilityAssessmentsCommandHandler handler;
        protected static IAffordabilityAssessmentRepository affordabilityAssessmentRepository;
        protected static ILookupRepository lookupRepository;
        protected static IAffordabilityAssessment affordabilityAssessment;
        protected static IList<IAffordabilityAssessment> affordabilityAssessments;
        protected static IEventList<IAffordabilityAssessmentStatus> affordabilityAssessmentStatuses = new StubEventList<IAffordabilityAssessmentStatus>();
        protected static int applicationKey = 1;

        // Arrange
        private Establish context = () =>
        {
            affordabilityAssessmentRepository = An<IAffordabilityAssessmentRepository>();
            lookupRepository = An<ILookupRepository>();

            affordabilityAssessment = An<IAffordabilityAssessment>();
            affordabilityAssessment.WhenToldTo(x => x.AffordabilityAssessmentStatus.Key).Return((int)SAHL.Common.Globals.AffordabilityAssessmentStatuses.Unconfirmed);

            affordabilityAssessments = new List<IAffordabilityAssessment>();
            affordabilityAssessments.Add(affordabilityAssessment);

            affordabilityAssessmentRepository.WhenToldTo(x => x.GetActiveApplicationAffordabilityAssessments(Param.IsAny<int>())).Return(affordabilityAssessments);

            lookupRepository.WhenToldTo(x => x.AffordabilityAssessmentStatuses).Return(affordabilityAssessmentStatuses);
            affordabilityAssessmentStatuses.ObjectDictionary.Add(Convert.ToString((int)AffordabilityAssessmentStatuses.Confirmed), null);

            command = new ConfirmApplicationAffordabilityAssessmentsCommand(applicationKey);
            handler = new ConfirmApplicationAffordabilityAssessmentsCommandHandler(affordabilityAssessmentRepository, lookupRepository);
        };

        // Act
        private Because of = () =>
        {
            handler.Handle(messages, command);
        };

        private It should_get_the_affordability_assessments_for_the_application = () =>
        {
            affordabilityAssessmentRepository.WasToldTo(x => x.GetActiveApplicationAffordabilityAssessments(Param.IsAny<int>()));
        };

        private It should_update_the_status_of_the_affordability_assessment_to_confirmed = () =>
        {
            affordabilityAssessmentRepository.WasToldTo(x => x.SaveAffordabilityAssessment(affordabilityAssessment));
        };
    }
}