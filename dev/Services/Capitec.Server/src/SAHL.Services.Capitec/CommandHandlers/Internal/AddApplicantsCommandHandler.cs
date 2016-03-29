using System;
using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.Applicant;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Interfaces.Capitec.Commands;

namespace SAHL.Services.Capitec.CommandHandlers.Internal
{
    public class AddApplicantsCommandHandler : IServiceCommandHandler<AddApplicantsCommand>
    {
        private IApplicantManager applicantService;
        private ILookupManager lookupService;
        private IUnitOfWorkFactory unitOfWorkFactory;

        public AddApplicantsCommandHandler(IApplicantManager applicantService, ILookupManager lookupService, IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.lookupService = lookupService;
            this.applicantService = applicantService;
        }

        public ISystemMessageCollection HandleCommand(AddApplicantsCommand command, IServiceRequestMetadata metadatas)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            using (var uow = this.unitOfWorkFactory.Build())
            {
                foreach (var addedApplicant in command.Applicants)
                {
                    var applicant = addedApplicant.Value;
                    var applicantID = addedApplicant.Key;
                    bool personExists = applicantService.DoesPersonExist(applicant.Information.IdentityNumber);
                    Guid personID;

                    if (!personExists)
                    {
                        personID = lookupService.GenerateCombGuid();
                        applicantService.AddPerson(personID, applicant.Information.SalutationEnumId, applicant.Information.FirstName, applicant.Information.Surname, applicant.Information.IdentityNumber);
                    }
                    else
                    {
                        personID = applicantService.GetPersonIDFromIdentityNumber(applicant.Information.IdentityNumber);
                    }

                    applicantService.SaveApplicant(applicantID, personID, applicant.Information.MainContact);

                    applicantService.AddContactDetailsForApplicant(applicantID, applicant.Information);

                    applicantService.AddResidentialAddressForApplicant(applicantID, applicant.ResidentialAddress);

                    applicantService.AddDeclarationsForApplicant(applicantID, applicant.Declarations);

                    applicantService.AddEmploymentForApplicant(applicantID, applicant.EmploymentDetails);
                }
                uow.Complete();
            }
            return messages;
        }
    }
}