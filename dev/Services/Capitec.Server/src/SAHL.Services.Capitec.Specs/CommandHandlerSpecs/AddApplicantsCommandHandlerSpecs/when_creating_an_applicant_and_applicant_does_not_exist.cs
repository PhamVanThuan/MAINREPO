using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Capitec.CommandHandlers.Internal;
using SAHL.Services.Capitec.Managers.Applicant;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Capitec.Specs.Stubs;
using SAHL.Services.Interfaces.Capitec.Commands;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;

namespace SAHL.Services.Capitec.Specs.CommandHandlerSpecs.AddApplicantsCommandHandlerSpecs
{
    internal class when_creating_an_applicant_for_an_application : WithFakes
    {
        private static Guid applicantID;
        private static ApplicantStubs applicant;
        private static Guid personID;
        private static AddApplicantsCommand command;
        private static AddApplicantsCommandHandler handler;
        private static ILookupManager lookupService;
        private static IApplicantManager applicantService;
        private static Dictionary<Guid, Applicant> applicantsToAdd;
        private static IUnitOfWorkFactory unitOfWorkFactory;

        private Establish context = () =>
        {
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            lookupService = An<ILookupManager>();
            applicantService = An<IApplicantManager>();
            handler = new AddApplicantsCommandHandler(applicantService, lookupService, unitOfWorkFactory);

            applicant = new ApplicantStubs();
            applicantID = new Guid();
            personID = new Guid();
            applicantsToAdd = new Dictionary<Guid, Applicant> { { applicantID, applicant.GetApplicant } };
            command = new AddApplicantsCommand(applicantsToAdd);

            lookupService.WhenToldTo(x => x.GenerateCombGuid()).Return(personID);
        };

        private Because of = () =>
        {
            handler.HandleCommand(command, new ServiceRequestMetadata());
        };

        private It should_check_if_the_person_is_an_existing_client = () =>
        {
            applicantService.WasToldTo(x => x.DoesPersonExist(applicant.GetApplicant.Information.IdentityNumber));
        };

        private It should_add_the_person = () =>
        {
            applicantService.WasToldTo(x => x.AddPerson(Param.IsAny<Guid>(), Param.IsAny<Guid>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>()));
        };

        private It should_not_get_the_personID = () =>
        {
            applicantService.WasNotToldTo(x => x.GetPersonIDFromIdentityNumber(applicant.GetApplicant.Information.IdentityNumber));
        };

        private It should_add_the_applicant = () =>
        {
            applicantService.WasToldTo(x => x.SaveApplicant(applicantID, personID, true));
        };

        private It should_add_the_applicant_contact_details = () =>
        {
            applicantService.WasToldTo(x => x.AddContactDetailsForApplicant(applicantID, applicant.GetApplicant.Information));
        };

        private It should_add_the_applicant_residential_address = () =>
        {
            applicantService.WasToldTo(x => x.AddResidentialAddressForApplicant(applicantID, applicant.GetApplicant.ResidentialAddress));
        };

        private It should_add_the_applicant_declarations = () =>
        {
            applicantService.WasToldTo(x => x.AddDeclarationsForApplicant(applicantID, applicant.GetApplicant.Declarations));
        };

        private It should_add_the_applicant_employment = () =>
        {
            applicantService.WasToldTo(x => x.AddEmploymentForApplicant(applicantID, applicant.GetApplicant.EmploymentDetails));
        };
    }
}