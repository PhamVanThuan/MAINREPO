using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.LifeDomain.Commands;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;

namespace SAHL.Services.Interfaces.LifeDomain.Specs.Commands
{
    public class when_updating_a_approved_disability_claim : WithFakes
    {
        private static AmendApprovedDisabilityClaimCommand command;
        private static int disabilityClaimKey;
        private static string claimantOccupation;
        private static int disabilityTypeKey;
        private static string otherDisabilityComments;
        private static DateTime expectedReturnToWorkDate;
        private static DateTime lastDateWorked;
        

        private Establish context = () =>
        {
            disabilityClaimKey = 1111;
            claimantOccupation = EmploymentSector.ITandElectronics.ToString();
            disabilityTypeKey = 4444;
            otherDisabilityComments = string.Empty;
            expectedReturnToWorkDate = DateTime.Now.AddDays(4d);
            lastDateWorked = DateTime.Now; 
        };

        private Because of = () =>
        {
            command = new AmendApprovedDisabilityClaimCommand(disabilityClaimKey, disabilityTypeKey, otherDisabilityComments, claimantOccupation, lastDateWorked, expectedReturnToWorkDate);
        };

        private It should_be_assignable_to_ILifeDomainCommand = () =>
        {
            command.ShouldBeAssignableTo(typeof(ILifeDomainCommand));
        };
    }
}