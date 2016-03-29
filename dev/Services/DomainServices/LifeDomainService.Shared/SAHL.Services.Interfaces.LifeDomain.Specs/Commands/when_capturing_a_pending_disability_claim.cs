using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;

namespace SAHL.Services.Interfaces.LifeDomain.Specs.Commands
{
    public class when_capturing_a_pending_disability_claim : WithFakes
    {
        private static CapturePendingDisabilityClaimCommand command;
        private static int disabilityClaimKey;
        
        private static DateTime lastDateWorked;
        private static DateTime dateOfDiagnosis;
        private static string claimantOccupation;
        private static int disabilityTypeKey;
        private static string otherDisabilityComments;
        private static DateTime expectedReturnToWorkDate;
        

        private Establish context = () =>
        {
            disabilityClaimKey = 1111;
            lastDateWorked = DateTime.Now;
            dateOfDiagnosis = DateTime.Now;
            claimantOccupation = EmploymentSector.ITandElectronics.ToString();
            disabilityTypeKey = 4444;
            otherDisabilityComments = string.Empty;
            expectedReturnToWorkDate = DateTime.Now.AddDays(4d);    
        };

        private Because of = () =>
        {
            command = new CapturePendingDisabilityClaimCommand(disabilityClaimKey, 
                                                                dateOfDiagnosis, 
                                                                disabilityTypeKey, 
                                                                otherDisabilityComments, 
                                                                claimantOccupation, 
                                                                lastDateWorked, 
                                                                expectedReturnToWorkDate);
        };

        private It should_be_assignable_to_ILifeDomainCommand = () =>
        {
            command.ShouldBeAssignableTo(typeof(ILifeDomainCommand));
        };

        private It should_be_assignable_to_IDisabilityClaimRuleModel = () =>
        {
            command.ShouldBeAssignableTo(typeof(IDisabilityClaimRuleModel));
        };

        private It should_implement_the_requires_pending_disability_claim_check = () =>
        {
            command.ShouldBeAssignableTo(typeof(IRequiresPendingDisabilityClaim));
        };
    }
}