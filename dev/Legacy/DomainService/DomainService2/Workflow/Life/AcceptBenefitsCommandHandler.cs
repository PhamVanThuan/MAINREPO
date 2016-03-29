namespace DomainService2.Workflow.Life
{
    using System;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;
    using SAHL.Common.Globals;

    public class AcceptBenefitsCommandHandler : IHandlesDomainServiceCommand<AcceptBenefitsCommand>
    {
        private IApplicationRepository ApplicationRepository;
        private ILifeRepository LifeRepository;
        private IAccountRepository AccountRepository;
        private ILookupRepository LookupRepository;

        public AcceptBenefitsCommandHandler(IApplicationRepository applicationRepository, ILifeRepository lifeRepository, ILookupRepository lookupRepository, IAccountRepository accountRepository)
        {
            this.ApplicationRepository = applicationRepository;
            this.LifeRepository = lifeRepository;
            this.LookupRepository = lookupRepository;
            this.AccountRepository = accountRepository;
        }

        public void Handle(IDomainMessageCollection messages, AcceptBenefitsCommand command)
        {
            // Get the Life Policy Application
            IApplicationLife applicationLife = ApplicationRepository.GetApplicationLifeByKey(command.ApplicationKey);

            // Get the Life Account
            IAccountLifePolicy accountLifePolicy = applicationLife.Account as IAccountLifePolicy;

            // Get the Loan Account Object
            IAccount _loanAccount = accountLifePolicy.ParentMortgageLoan;

            // Get the Loan Applicants (main applicants and suretors)
            IReadOnlyEventList<ILegalEntityNaturalPerson> _lstApplicants = _loanAccount.GetNaturalPersonLegalEntitiesByRoleType(messages, new int[] { (int)RoleTypes.MainApplicant, (int)RoleTypes.Suretor });

            bool bFirstAssuredLife = true;
            foreach (ILegalEntityNaturalPerson legalEntityNaturalPerson in _lstApplicants)
            {
                // validate the legal entity to see if they qualify
                if (LifeRepository.CheckLegalEntityQualifies(accountLifePolicy, legalEntityNaturalPerson) == false)
                    continue;

                // Populate the LifeInsurableInterest object
                ILifeInsurableInterest _lifeInsurableInterest = LifeRepository.CreateEmptyLifeInsurableInterest();
                _lifeInsurableInterest.Account = applicationLife.Account;
                _lifeInsurableInterest.LegalEntity = legalEntityNaturalPerson;

                IRole _leRole = legalEntityNaturalPerson.GetRole(_loanAccount.Key);
                if (_leRole.RoleType.Key == (int)RoleTypes.MainApplicant)
                    _lifeInsurableInterest.LifeInsurableInterestType = LookupRepository.LifeInsurableInterestTypes.ObjectDictionary[Convert.ToString((int)LifeInsurableInterestTypes.Mortgagor)];
                else
                    _lifeInsurableInterest.LifeInsurableInterestType = LookupRepository.LifeInsurableInterestTypes.ObjectDictionary[Convert.ToString((int)LifeInsurableInterestTypes.Surety)];

                // Add the LifeInsurableInterest to the accountLifePolicy object
                accountLifePolicy.LifeInsurableInterests.Add(messages, _lifeInsurableInterest);

                IRole _role = AccountRepository.CreateEmptyRole();
                _role.Account = applicationLife.Account;
                _role.LegalEntity = legalEntityNaturalPerson;
                _role.RoleType = LookupRepository.RoleTypes.ObjectDictionary[Convert.ToString((int)RoleTypes.AssuredLife)];
                _role.GeneralStatus = LookupRepository.GeneralStatuses[GeneralStatuses.Active];
                _role.StatusChangeDate = System.DateTime.Now;

                // Add the role to the accountLifePolicy object
                accountLifePolicy.Roles.Add(messages, _role);

                // Set the Applications Contact Person to the first assured life
                if (bFirstAssuredLife == true)
                {
                    bFirstAssuredLife = false;
                    applicationLife.PolicyHolderLegalEntity = legalEntityNaturalPerson as ILegalEntity;
                }
            }

            // Save the life policy account (roles & insturable interest types)
            AccountRepository.SaveAccount(accountLifePolicy);
        }
    }
}