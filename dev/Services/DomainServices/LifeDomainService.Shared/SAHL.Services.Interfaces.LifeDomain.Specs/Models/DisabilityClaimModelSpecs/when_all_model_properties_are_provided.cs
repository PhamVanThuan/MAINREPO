using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System;

namespace SAHL.Services.Interfaces.LifeDomain.Specs.Models.DisabilityClaimModelSpecs
{
    public class when_all_model_properties_are_provided : WithFakes
    {
        private static DisabilityClaimModel model;
        private static Exception ex;
        private static int disabilityClaimKey;
        private static int accountKey;
        private static int legalEntityKey;
        private static DateTime dateClaimReceived;
        private static DateTime? lastDateWorked;
        private static DateTime? dateOfDiagnosis;
        private static string claimantOccupation;
        private static int? disabilityTypeKey;
        private static string otherDisabilityComments;
        private static DateTime? expectedReturnToWorkDate;
        private static int disabilityClaimStatusKey;
        private static DateTime? paymentStartDate;
        private static int? numberOfInstalmentsAuthorized;
        private static DateTime? paymentEndDate;        

        private Establish context = () =>
        {
            disabilityClaimKey = 1111;
            accountKey = 22222;
            legalEntityKey = 3333;
            dateClaimReceived = DateTime.Now;
            lastDateWorked = DateTime.Today.AddDays(1);
            dateOfDiagnosis = DateTime.Today.AddDays(2);
            claimantOccupation = EmploymentSector.ITandElectronics.ToString();
            disabilityTypeKey = 4444;
            otherDisabilityComments = string.Empty;
            expectedReturnToWorkDate = DateTime.Now.AddDays(3);
            disabilityClaimStatusKey = 5555;
            paymentStartDate = DateTime.Now.AddDays(5);
            numberOfInstalmentsAuthorized = 10;
            paymentEndDate = DateTime.Now.AddDays(45d);           
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                model = new DisabilityClaimModel(disabilityClaimKey, 
                    accountKey, 
                    legalEntityKey, 
                    dateClaimReceived, 
                    lastDateWorked, 
                    dateOfDiagnosis, 
                    claimantOccupation, 
                    disabilityTypeKey, 
                    otherDisabilityComments, 
                    expectedReturnToWorkDate, 
                    disabilityClaimStatusKey, 
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