using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;

namespace SAHL.Services.Interfaces.LifeDomain.Specs.Models.DisabilityClaimDetailModelSpecs
{
    public class when_all_model_properties_are_provided : WithFakes
    {
        private static DisabilityClaimDetailModel model;
        private static Exception ex;
        private static int disabilityClaimKey;
        private static int accountKey;
        private static int loanNumber;
        private static int legalEntityKey;
        private static string claimantLegalEntityDisplayName;
        private static DateTime dateClaimReceived;
        private static DateTime lastDateWorked;
        private static DateTime dateOfDiagnosis;
        private static string claimantOccupation;
        private static int disabilityTypeKey;
        private static string disabilityType;
        private static string otherDisabilityComments;
        private static DateTime expectedReturnToWorkDate;
        private static int disabilityClaimStatusKey;
        private static string disabilityClaimStatus;
        private static DateTime paymentStartDate;
        private static int numberOfInstalmentsAuthorized;
        private static DateTime paymentEndDate;

        private Establish context = () =>
        {
            disabilityClaimKey = 111;
            accountKey = 222;
            loanNumber = 333;
            legalEntityKey = 333;
            claimantLegalEntityDisplayName = "Joe Soap";
            dateClaimReceived = DateTime.Today;
            lastDateWorked = DateTime.Today.AddDays(1);
            dateOfDiagnosis = DateTime.Today;
            claimantOccupation = EmploymentSector.ITandElectronics.ToString();
            disabilityTypeKey = 444;
            disabilityType = "A Disability";
            otherDisabilityComments = string.Empty;
            expectedReturnToWorkDate = DateTime.Today.AddMonths(12);
            disabilityClaimStatusKey = 555;
            disabilityClaimStatus = string.Empty;
            paymentStartDate = DateTime.Today.AddMonths(1);
            numberOfInstalmentsAuthorized = 12;
            paymentEndDate = DateTime.Today.AddMonths(12);
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new DisabilityClaimDetailModel(disabilityClaimKey, 
                                                        accountKey, 
                                                        loanNumber, 
                                                        legalEntityKey, 
                                                        claimantLegalEntityDisplayName, 
                                                        dateClaimReceived, 
                                                        lastDateWorked, 
                                                        dateOfDiagnosis, 
                                                        claimantOccupation, 
                                                        disabilityTypeKey, 
                                                        disabilityType, 
                                                        otherDisabilityComments, 
                                                        expectedReturnToWorkDate, 
                                                        disabilityClaimStatusKey, 
                                                        disabilityClaimStatus, 
                                                        paymentStartDate, 
                                                        numberOfInstalmentsAuthorized, 
                                                        paymentEndDate);
            });
        };

        private It should_not_throw_a_validation_exception = () =>
        {
            ex.ShouldBeNull();
        };

        private It should_construct_the_model = () =>
        {
            model.ShouldNotBeNull();
        };
    }
}