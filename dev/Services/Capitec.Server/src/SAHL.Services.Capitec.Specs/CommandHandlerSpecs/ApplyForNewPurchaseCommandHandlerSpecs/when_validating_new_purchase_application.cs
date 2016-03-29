using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Validation;
using SAHL.Services.Capitec.Managers.Application;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Capitec.Specs.Stubs;
using SAHL.Services.Interfaces.Capitec.Commands;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.ApplyForNewPurchaseCommandHandlerSpecs
{
    public class when_validating_new_purchase_application : WithFakes
    {
        private static ILookupManager lookupService;
        private static IApplicationManager applicationService;
        private static ApplyForNewPurchaseCommand command;
        private static NewPurchaseApplication application;
        private static List<Applicant> applicants;
        private static Guid applicationID;
        private static Guid generatedID;
        private static ApplicantStubs stubs;
        private static NewPurchaseLoanDetails newPurchaseLoanDetails;
        private static List<ValidationResult> results = new List<ValidationResult>();
        private static ValidateCommand validation;
        private static ITypeMetaDataLookup lookups;
        private static IValidateStrategy strategy;

        private Establish context = () =>
        {
            lookupService = An<ILookupManager>();
            applicationService = An<IApplicationManager>();
            stubs = new ApplicantStubs();
            newPurchaseLoanDetails = new NewPurchaseLoanDetails(new Guid(), new Guid(), 30000, 0, 100000, 500, 240, true);

            generatedID = Guid.NewGuid();
            applicationID = Guid.NewGuid();

            var _applicantInformation = new ApplicantInformation("112233", null, "Muggs", stubs.SalutationEnumId, "0724310696", "0724310696", "0724310696", "bob@muggs.com", DateTime.Parse("1979-01-10"), "Mr", true);

            var _applicant = new Applicant(_applicantInformation, stubs.GetApplicantResidentialAddress, stubs.GetApplicantEmploymentDetails, stubs.GetApplicantDeclarations);

            applicants = new List<Applicant>() { _applicant, stubs.GetApplicant };
            application = new NewPurchaseApplication(newPurchaseLoanDetails, applicants, new Guid(), "1184050800000-0700");
            //
            command = new ApplyForNewPurchaseCommand(applicationID, application);
            //
            lookups = new TypeMetaDataLookup();
            strategy = new ValidateStrategy(lookups);
            validation = new ValidateCommand(lookups, strategy);
            lookupService.WhenToldTo(x => x.GenerateCombGuid()).Return(generatedID);
        };

        private Because of = () =>
        {
            results = validation.Validate(command).ToList();
        };

        private It should_contain_a_validation_message_for_the_application_purchase_price = () =>
        {
            results.Where(x => x.ErrorMessage.Equals("Purchase Price must be greater than R 0 {in: NewPurchaseLoanDetails}")).FirstOrDefault().ShouldNotBeNull();
        };

        private It should_contain_a_validation_message_for_the_applicant_ID_number_field = () =>
        {
            results.Where(x => x.ErrorMessage.Equals("Please provide a valid South African ID Number. {in: Information}")).FirstOrDefault().ShouldNotBeNull();
        };

        private It should_contain_a_validation_message_for_the_applicant_first_name_field = () =>
        {
            results.Where(x => x.ErrorMessage.Equals("The FirstName field is required. {in: Information}")).FirstOrDefault().ShouldNotBeNull();
        };
    }
}