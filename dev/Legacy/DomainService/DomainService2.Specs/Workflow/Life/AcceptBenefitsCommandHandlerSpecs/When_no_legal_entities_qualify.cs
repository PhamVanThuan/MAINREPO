namespace DomainService2.Specs.Workflow.Life.AcceptBenefitsCommandHandlerSpecs
{
    using DomainService2.Specs.DomainObjects;
    using DomainService2.Workflow.Life;
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;

    [Subject(typeof(AcceptBenefitsCommandHandler))]
    public class When_no_legal_entities_qualify : DomainServiceSpec<AcceptBenefitsCommand, AcceptBenefitsCommandHandler>
    {
        static ILifeRepository lifeRepository;
        static IAccountRepository accountRepository;

        Establish context = () =>
        {
            accountRepository = An<IAccountRepository>();
            lifeRepository = An<ILifeRepository>();
            ILookupRepository lookupRepository = An<ILookupRepository>();
            IApplicationRepository applicationRepository = An<IApplicationRepository>();
            IApplicationLife applicationLife = An<IApplicationLife>();
            IAccountLifePolicy accountLifePolicy = An<IAccountLifePolicy>();
            IAccount loanAccount = An<IAccount>();
            ILegalEntityNaturalPerson legalEntityNaturalPerson = An<ILegalEntityNaturalPerson>();
            IReadOnlyEventList<ILegalEntityNaturalPerson> applicants = new StubReadOnlyEventList<ILegalEntityNaturalPerson>(new ILegalEntityNaturalPerson[] { legalEntityNaturalPerson });

            applicationRepository.WhenToldTo(x => x.GetApplicationLifeByKey(Param<int>.IsAnything)).Return(applicationLife);
            applicationLife.WhenToldTo(x => x.Account).Return(accountLifePolicy);
            accountLifePolicy.WhenToldTo(x => x.ParentMortgageLoan).Return(loanAccount);
            loanAccount.WhenToldTo(x => x.GetNaturalPersonLegalEntitiesByRoleType(Param<IDomainMessageCollection>.IsAnything, Param<int[]>.IsAnything)).Return(applicants);
            lifeRepository.WhenToldTo(x => x.CheckLegalEntityQualifies(accountLifePolicy, legalEntityNaturalPerson)).Return(false);

            command = new AcceptBenefitsCommand(Param<int>.IsAnything);
            handler = new AcceptBenefitsCommandHandler(applicationRepository, lifeRepository, lookupRepository, accountRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_check_legal_entity_qualify = () =>
        {
            lifeRepository.WasToldTo(x => x.CheckLegalEntityQualifies(Param<IAccountLifePolicy>.IsAnything, Param<ILegalEntityNaturalPerson>.IsAnything));
        };

        It should_not_create_empty_life_insurable_interest = () =>
        {
            lifeRepository.WasNotToldTo(x => x.CreateEmptyLifeInsurableInterest());
        };

        It should_save_account = () =>
        {
            accountRepository.WasToldTo(x => x.SaveAccount(Param<IAccountLifePolicy>.IsAnything));
        };
    }
}