namespace DomainService2.Specs.Workflow.Life.AcceptBenefitsCommandHandlerSpecs
{
    using System;
    using System.Collections.Generic;
    using DomainService2.Specs.DomainObjects;
    using DomainService2.Workflow.Life;
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;
    using SAHL.Common.Globals;

    [Subject(typeof(AcceptBenefitsCommandHandler))]
    public class When_legal_entities_qualify_not_main_applicant : DomainServiceSpec<AcceptBenefitsCommand, AcceptBenefitsCommandHandler>
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
            ILifeInsurableInterest lifeInsurableInterest = An<ILifeInsurableInterest>();
            IRole role = An<IRole>();
            IEventList<ILifeInsurableInterestType> lifeInsurableInterestTypes = new StubEventList<ILifeInsurableInterestType>();
            IEventList<IRoleType> roleTypes = new StubEventList<IRoleType>();
            IEventList<ILifeInsurableInterest> lifeInsurableInterests = new StubEventList<ILifeInsurableInterest>();
            IDictionary<GeneralStatuses, IGeneralStatus> generalStatuses = new Dictionary<GeneralStatuses, IGeneralStatus>();
            IEventList<IRole> roles = new StubEventList<IRole>();

            applicationRepository.WhenToldTo(x => x.GetApplicationLifeByKey(Param<int>.IsAnything)).Return(applicationLife);
            applicationLife.WhenToldTo(x => x.Account).Return(accountLifePolicy);
            accountLifePolicy.WhenToldTo(x => x.ParentMortgageLoan).Return(loanAccount);
            loanAccount.WhenToldTo(x => x.GetNaturalPersonLegalEntitiesByRoleType(Param<IDomainMessageCollection>.IsAnything, Param<int[]>.IsAnything)).Return(applicants);
            lifeRepository.WhenToldTo(x => x.CheckLegalEntityQualifies(accountLifePolicy, legalEntityNaturalPerson)).Return(true);
            lifeRepository.WhenToldTo(x => x.CreateEmptyLifeInsurableInterest()).Return(lifeInsurableInterest);
            role.WhenToldTo(x => x.RoleType.Key).Return((int)RoleTypes.Suretor);
            legalEntityNaturalPerson.WhenToldTo(x => x.GetRole(Param<int>.IsAnything)).Return(role);
            lookupRepository.WhenToldTo(x => x.LifeInsurableInterestTypes).Return(lifeInsurableInterestTypes);
            lifeInsurableInterestTypes.ObjectDictionary.Add(Convert.ToString((int)LifeInsurableInterestTypes.Surety), null);
            accountLifePolicy.WhenToldTo(x => x.LifeInsurableInterests).Return(lifeInsurableInterests);
            accountRepository.WhenToldTo(x => x.CreateEmptyRole()).Return(role);
            lookupRepository.WhenToldTo(x => x.RoleTypes).Return(roleTypes);
            roleTypes.ObjectDictionary.Add(Convert.ToString((int)RoleTypes.AssuredLife), null);
            generalStatuses.Add(GeneralStatuses.Active, null);
            lookupRepository.WhenToldTo(x => x.GeneralStatuses).Return(generalStatuses);
            accountLifePolicy.WhenToldTo(x => x.Roles).Return(roles);

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

        It should_create_empty_life_insurable_interest = () =>
        {
            lifeRepository.WasToldTo(x => x.CreateEmptyLifeInsurableInterest());
        };

        It should_save_account = () =>
        {
            accountRepository.WasToldTo(x => x.SaveAccount(Param<IAccountLifePolicy>.IsAnything));
        };
    }
}