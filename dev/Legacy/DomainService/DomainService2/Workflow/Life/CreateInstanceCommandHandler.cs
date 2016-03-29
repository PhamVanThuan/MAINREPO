namespace DomainService2.Workflow.Life
{
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;
    using SAHL.Common.Globals;

    public class CreateInstanceCommandHandler : IHandlesDomainServiceCommand<CreateInstanceCommand>
    {
        private IAccountRepository AccountRepository;
        private IApplicationRepository ApplicationRepository;
        private IOrganisationStructureRepository OrganisationStructureRepository;
        private IWorkflowAssignmentRepository WorkflowAssignmentRepository;
        private ICommonRepository commonRepository;

        public CreateInstanceCommandHandler(IAccountRepository accountRepository, IApplicationRepository applicationRepository, IOrganisationStructureRepository organisationStructureRepository, IWorkflowAssignmentRepository workflowAssignmentRepository, ICommonRepository commonRepository)
        {
            this.AccountRepository = accountRepository;
            this.ApplicationRepository = applicationRepository;
            this.OrganisationStructureRepository = organisationStructureRepository;
            this.WorkflowAssignmentRepository = workflowAssignmentRepository;
            this.commonRepository = commonRepository;
        }

        public void Handle(IDomainMessageCollection messages, CreateInstanceCommand command)
        {
            // Get the Mortgage Loan
            IMortgageLoanAccount mortgageLoanAccount = AccountRepository.GetAccountByKey(command.LoanNumber) as IMortgageLoanAccount;

            // Get latest 'Open/Accepted' loan Offer with OfferInformationtype of 'Accepted'
            // this will be used to determine which mandate we use and for storing in the OfferAccountRelationship table.
            IApplication mortgageLoanApplication = ApplicationRepository.GetLastestAcceptedApplication(mortgageLoanAccount);
            string assignTo = command.AssignTo;

            if (command.AssignTo == "Token")
            {
                assignTo = OrganisationStructureRepository.GetLifeConsultantADUserNameFromMandate(mortgageLoanApplication);
            }

            // Create the Life Appplication
            IApplicationLife applicationLife = ApplicationRepository.CreateLifeApplication(mortgageLoanAccount.Key, mortgageLoanApplication.Key, assignTo);
            commonRepository.RefreshDAOObject<IApplicationLife>(applicationLife.Key);

            // Update the data record with the applicationkey
            command.ApplicationKey = applicationLife.Key;

            // set the InstanceData.Name
            command.Name = applicationLife.Account.Key.ToString();

            // check for assured lives
            IReadOnlyEventList<ILegalEntityNaturalPerson> AssuredLives = applicationLife.Account.GetNaturalPersonLegalEntitiesByRoleType(messages, new int[] { (int)RoleTypes.AssuredLife });
            if (AssuredLives.Count > 0)
            {
                command.Subject = applicationLife.Account.GetLegalName(LegalNameFormat.Full);
            }
            else
            {
                command.Subject = mortgageLoanAccount.SecuredMortgageLoan.Account.GetLegalName(LegalNameFormat.Full);
            }

            command.Priority = 9;

            IADUser aDUser = OrganisationStructureRepository.GetAdUserForAdUserName(assignTo);
            int aDUserKey = aDUser.Key;
            int OfferRoleTypeOrganisationStructureMappingKey = OrganisationStructureRepository.GetOfferRoleTypeOrganisationStructureMappingKey((int)OrganisationStructure.CCC_Consultant, (int)OfferRoleTypes.Consultant);
            WorkflowAssignmentRepository.AssignWorkflowRole(command.InstanceID, aDUserKey, OfferRoleTypeOrganisationStructureMappingKey, "Contact Client");
        }
    }
}