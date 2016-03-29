using System;
using System.Collections.Generic;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class AccountLifePolicy : Account, IAccountLifePolicy
    {
        #region Properties

        public ILifePolicy LifePolicy
        {
            get
            {
                //The Financial Services count might include Arrears.
                //Check only for Life Policy cont
                ILifeRepository lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();
                return lifeRepo.GetLifePolicyByAccountKey(this.Key);
            }
        }

        public IMortgageLoanAccount MortgageLoanAccount
        {
            get
            {
                return this.ParentAccount as IMortgageLoanAccount;
            }
        }

        public override SAHL.Common.Globals.AccountTypes AccountType
        {
            get { return SAHL.Common.Globals.AccountTypes.Life; }
        }

        public IApplicationLife CurrentLifeApplication
        {
            get
            {
                string HQL = "FROM ApplicationLife_DAO a WHERE a.Account.Key = ? ORDER BY a.Key desc";

                SimpleQuery<ApplicationLife_DAO> q = new SimpleQuery<ApplicationLife_DAO>(HQL, this.Key);
                q.SetQueryRange(1);
                ApplicationLife_DAO[] list = q.Execute();

                if (list.Length > 0)
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationLife, ApplicationLife_DAO>(list[0]);
                }
                return null;
            }
        }

        #endregion Properties

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnLifeInsurableInterests_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnLifeInsurableInterests_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnLifeInsurableInterests_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item">object</param>
        protected void OnLifeInsurableInterests_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        override protected void OnRoles_BeforeAdd(ICancelDomainArgs args, object Item)
        {
            IRole role = Item as IRole;
            IAccountLifePolicy accountLifePolicy = role.Account as IAccountLifePolicy;

            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            // 1. check the legal entity is a natural person
            if (!(role.LegalEntity is ILegalEntityNaturalPerson))
            {
                spc.DomainMessages.Add(new Error("Assured Life must be a Natural Person", "Assured Life must be a Natural Person"));
                args.Cancel = true;
            }

            ILegalEntityNaturalPerson legalEntityNaturalPerson = role.LegalEntity as ILegalEntityNaturalPerson;

            // 2. check the person is alive
            if (legalEntityNaturalPerson.LegalEntityStatus.Key == (int)SAHL.Common.Globals.LegalEntityStatuses.Deceased)
            {
                spc.DomainMessages.Add(new Error("Assured Life cannot be Deceased", "Assured Life cannot be Deceased"));
                args.Cancel = true;
            }

            // 3. check the age between 18 & 65
            int minAge = Convert.ToInt32(lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.Life.AssuredLifeMinAge].ControlNumeric);
            int maxAge = Convert.ToInt32(lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.Life.AssuredLifeMaxAge].ControlNumeric);
            if (legalEntityNaturalPerson.AgeNextBirthday < minAge || legalEntityNaturalPerson.AgeNextBirthday > maxAge)
            {
                spc.DomainMessages.Add(new Error("Assured Life must be between the ages of " + minAge + " and " + maxAge, "Assured Life must be between the ages of " + minAge + " and " + maxAge));
                args.Cancel = true;
            }

            // 4. check the group exposure (cannot have more than 2 life policies)
            int maxPolicies = Convert.ToInt32(lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.Life.GroupExposureMaxPolicies].ControlNumeric);
            ILifeRepository lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();
            bool overExposed = lifeRepo.IsLifeOverExposed(legalEntityNaturalPerson);

            if (overExposed)
            {
                spc.DomainMessages.Add(new Error("The selected Legal Entity is already covered on " + maxPolicies + " Life Policies.", "The selected Legal Entity is already covered on  " + maxPolicies + " Life Policies."));
                args.Cancel = true;
            }

            // 5. check the person doesnt already play a role on the account
            foreach (IRole r in legalEntityNaturalPerson.Roles)
            {
                if (r.Account.Key == role.Account.Key)
                {
                    spc.DomainMessages.Add(new Error("The selected Legal Entity is already an Assured Life on this Policy.", "The selected Legal Entity is already an Assured Life on this Policy."));
                    args.Cancel = true;
                    break;
                }
            }

            // 6. check that the insurable interest has been set
            bool insurableInterestExists = false;
            foreach (ILifeInsurableInterest insurableInterest in accountLifePolicy.LifeInsurableInterests)
            {
                if (insurableInterest.LegalEntity.Key == legalEntityNaturalPerson.Key)
                {
                    insurableInterestExists = true;
                    break;
                }
            }
            if (insurableInterestExists == false)
            {
                spc.DomainMessages.Add(new Error("The selected Legal Entity does not have its Insurable Interest specified.", "The selected Legal Entity does not have its Insurable Interest specified."));
                args.Cancel = true;
            }

            //If we are cancelling a request, throw an error so that we know it has been canccelled.
            //This needs to be done after all items have been validated so that multiple/all messages are
            //reported to the UI
            if (args.Cancel)
                throw new SAHL.Common.Exceptions.DomainValidationException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        /// <param name="Item"></param>
        override protected void OnRoles_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        /// <param name="Item"></param>
        override protected void OnRoles_AfterRemove(ICancelDomainArgs args, object Item)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IRuleService svc = ServiceFactory.GetService<IRuleService>();

            // check that at least one assured life exists
            // this must only fire after the role has been removed so that the rule can check if the role count is <= 0
            int rulePassed = svc.ExecuteRule(spc.DomainMessages, "ValidateAssuredLifeMinimumRequired", this);
            if (rulePassed == 0)
                args.Cancel = true;

            //If we are cancelling a request, throw an error so that we know it has been canccelled.
            //This needs to be done after all items have been validated so that multiple/all messages are
            //reported to the UI
            if (args.Cancel)
                throw new SAHL.Common.Exceptions.DomainValidationException();
        }

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);

            Rules.Add("ValidateAssuredLifeMinimumRequired");
            Rules.Add("LifeApplicationCreateDebtCounselling");
        }
    }
}