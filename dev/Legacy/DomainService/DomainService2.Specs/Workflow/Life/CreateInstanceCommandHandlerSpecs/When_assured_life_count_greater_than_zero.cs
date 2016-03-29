namespace DomainService2.Specs.Workflow.Life.CreateInstanceCommandHandlerSpecs
{
    using DomainService2.Specs.DomainObjects;
    using DomainService2.Workflow.Life;
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;

    [Subject(typeof(CreateInstanceCommandHandler))]
    public class When_assured_life_count_greater_than_zero : DomainServiceSpec<CreateInstanceCommand, CreateInstanceCommandHandler>
    {
        static IApplicationLife applicationLife;
        private static ICommonRepository commonRepository;


        Establish context = () =>
        {
            commonRepository = An<ICommonRepository>();
            IMortgageLoanAccount mortgageLoanAccount = An<IMortgageLoanAccount>();
            IApplication mortgageLoanApplication = An<IApplication>();
            applicationLife = An<IApplicationLife>();
            IAccount account = An<IAccount>();
            IADUser aDUser = An<IADUser>();
            IMortgageLoan securedMortgageLoan = An<IMortgageLoan>();
            IAccountRepository accountRepository = An<IAccountRepository>();
            IApplicationRepository applicationRepository = An<IApplicationRepository>();
            IOrganisationStructureRepository organisationStructureRepository = An<IOrganisationStructureRepository>();
            IWorkflowAssignmentRepository workflowAssignmentRepository = An<IWorkflowAssignmentRepository>();
            ILegalEntityNaturalPerson legalEntityNaturalPerson = An<ILegalEntityNaturalPerson>();
            IReadOnlyEventList<ILegalEntityNaturalPerson> assuredLives = new StubReadOnlyEventList<ILegalEntityNaturalPerson>(new ILegalEntityNaturalPerson[] { legalEntityNaturalPerson });
            accountRepository.WhenToldTo(x => x.GetAccountByKey(Param<int>.IsAnything)).Return(mortgageLoanAccount);
            applicationRepository.WhenToldTo(x => x.GetLastestAcceptedApplication(Param<IAccount>.IsAnything)).Return(mortgageLoanApplication);
            organisationStructureRepository.WhenToldTo(x => x.GetLifeConsultantADUserNameFromMandate(Param<IApplication>.IsAnything)).Return("not an empty string");
            account.WhenToldTo(x => x.GetLegalName(Param<LegalNameFormat>.IsAnything)).Return(Param<string>.IsAnything);
            securedMortgageLoan.WhenToldTo(x => x.Account).Return(account);
            mortgageLoanAccount.WhenToldTo(x => x.SecuredMortgageLoan).Return(securedMortgageLoan);
            account.WhenToldTo(x => x.GetNaturalPersonLegalEntitiesByRoleType(Param<IDomainMessageCollection>.IsAnything, Param<int[]>.IsAnything)).Return(assuredLives);
            applicationLife.WhenToldTo(x => x.Account).Return(account);
            applicationRepository.WhenToldTo(x => x.CreateLifeApplication(Param<int>.IsAnything, Param<int>.IsAnything, Param<string>.IsAnything)).Return(applicationLife);
            organisationStructureRepository.WhenToldTo(x => x.GetAdUserForAdUserName(Param<string>.IsAnything)).Return(aDUser);

            command = new CreateInstanceCommand(Param<int>.IsAnything, Param<int>.IsAnything, "Token");
            handler = new CreateInstanceCommandHandler(accountRepository, applicationRepository, organisationStructureRepository, workflowAssignmentRepository, commonRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_get_get_legal_name = () =>
        {
            applicationLife.Account.WasToldTo(x => x.GetLegalName(Param<LegalNameFormat>.IsAnything));
        };
    }
}