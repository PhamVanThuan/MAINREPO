using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using System;

namespace DomainService2.SharedServices.Common
{
    public class ConfirmApplicationAffordabilityAssessmentsCommandHandler : IHandlesDomainServiceCommand<ConfirmApplicationAffordabilityAssessmentsCommand>
    {
        private IAffordabilityAssessmentRepository affordabilityAssessmentRepository;
        private ILookupRepository lookupRepository;

        public ConfirmApplicationAffordabilityAssessmentsCommandHandler(IAffordabilityAssessmentRepository affordabilityAssessmentRepository, ILookupRepository lookupRepository)
        {
            this.affordabilityAssessmentRepository = affordabilityAssessmentRepository;
            this.lookupRepository = lookupRepository;
        }

        public void Handle(IDomainMessageCollection messages, ConfirmApplicationAffordabilityAssessmentsCommand command)
        {
            var applicationsAssessments = affordabilityAssessmentRepository.GetActiveApplicationAffordabilityAssessments(command.ApplicationKey);

            foreach (var affordabilityAssessment in applicationsAssessments)
            {
                if (affordabilityAssessment.AffordabilityAssessmentStatus.Key == (int)SAHL.Common.Globals.AffordabilityAssessmentStatuses.Unconfirmed)
                {
                    affordabilityAssessment.AffordabilityAssessmentStatus = lookupRepository.AffordabilityAssessmentStatuses.ObjectDictionary[((int)AffordabilityAssessmentStatuses.Confirmed).ToString()];
                    affordabilityAssessment.ConfirmedDate = DateTime.Now;
                    affordabilityAssessmentRepository.SaveAffordabilityAssessment(affordabilityAssessment);
                }
            }
        }
    }
}