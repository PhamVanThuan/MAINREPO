using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;

namespace SAHL.Common.BusinessModel
{
    public abstract partial class Account : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Account_DAO>, IAccount
    {
        private IAccountInstallmentSummary _installmentSummary;

        public IApplication GetLatestApplicationByType(OfferTypes OfferType)
        {
            string HQL = "FROM Application_DAO a WHERE a.Account.Key = ? AND a.ApplicationType.Key = ? ORDER BY a.Key desc";

            SimpleQuery<Application_DAO> q = new SimpleQuery<Application_DAO>(HQL, this.Key, (int)OfferType);
            Application_DAO[] list = q.Execute();

            if (list.Length > 0)
            {
                IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return bmtm.GetMappedType<IApplication>(list[0]);
            }

            return null;
        }

        public virtual IAccountInstallmentSummary InstallmentSummary
        {
            get
            {
                if (_installmentSummary == null)
                    _installmentSummary = new AccountInstallmentSummary(this);
                return _installmentSummary;

                // return MortgageLoanAccountHelper.GetInstallmentSummary(this as Account);
            }
        }

        public IAccountBaselII GetLatestBehaviouralScore()
        {
            if (this.AccountBaselII == null || this.AccountBaselII.Count == 0)
                return null;

            DateTime latestDate = new DateTime(1, 1, 1);
            IAccountBaselII actBasel = this.AccountBaselII[0];

            for (int i = 0; i < this.AccountBaselII.Count; i++)
            {
                DateTime? date = this.AccountBaselII[i].AccountingDate;
                if (date != null && date > latestDate)
                {
                    latestDate = (DateTime)date;
                    actBasel = this.AccountBaselII[i];
                }
            }
            return actBasel;
        }

        /// <summary>
        /// Return a Table list of Account LegalEntities with an indication of whether the
        /// the ReasonDefinition (from ReasonType and ReasonDescription) provided exists
        /// this is to confirm whether there are any LegalEntities that are not marked as
        /// Dead or sequestrated, and to provide detail for the emails
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="rd"></param>
        /// <returns></returns>
        public DataTable GetAccountRoleNotificationByTypeAndDecription(ReasonTypes rt, ReasonDescriptions rd)
        {
            string query = UIStatementRepository.GetStatement("Account", "AccountLegalEntitiesWithoutNotification");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@AccountKey", this.Key));
            prms.Add(new SqlParameter("@ReasonTypeKey", (int)rt));
            prms.Add(new SqlParameter("@ReasonDescriptionKey", (int)rd));

            DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), prms);

            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];

            return new DataTable();
        }

        public string GetLegalName(LegalNameFormat Format)
        {
            int roleType = (int)SAHL.Common.Globals.RoleTypes.MainApplicant;

            if (this.Product.Key == (int)SAHL.Common.Globals.Products.LifePolicy)
                roleType = (int)SAHL.Common.Globals.RoleTypes.AssuredLife;

            IEventList<IRole> roles = Roles;
            string LegalName = "";
            for (int i = 0; i < roles.Count; i++)
            {
                if (roles[i].RoleType.Key == roleType)
                {
                    if (roles[i].LegalEntity != null)
                        LegalName += roles[i].LegalEntity.GetLegalName(Format);
                    if (i < roles.Count - 1)
                        LegalName += " & ";
                }
            }
            if (LegalName.EndsWith(" & "))
                LegalName = LegalName.Substring(0, LegalName.Length - 3);
            return LegalName;
        }

        public int GetEmploymentTypeKey()
        {
            /*
                SELECT
                    SUM(emp.ConfirmedBasicIncome) as HouseholdIncome,
                    emp.EmploymentTypeKey, r.AccountKey
                FROM Employment as emp
                inner join [Role] r on r.LegalEntityKey = emp.LegalEntityKey
                WHERE --r.AccountKey = ? AND
                    r.RoleTypeKey in (2,3)
                    AND r.GeneralStatusKey = 1 AND emp.EmploymentStatusKey = 1
                GROUP BY  emp.EmploymentTypeKey, r.AccountKey
                ORDER BY r.AccountKey, SUM(emp.ConfirmedBasicIncome) DESC
            */

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT emp.EmploymentType.Key FROM Employment_DAO as emp join emp.LegalEntity.Roles as role WHERE role.Account.Key = ? "); //, SUM(emp.ConfirmedBasicIncome) as HouseholdIncome
            sb.AppendLine("AND role.RoleType.Key in (2,3) AND role.GeneralStatus.Key = 1 AND emp.EmploymentStatus.Key = 1 ");

            //sb.AppendLine("AND emp.EmploymentType is not null "); //This can never be null
            sb.AppendLine("GROUP BY  emp.EmploymentType.Key");
            sb.AppendLine("ORDER BY SUM(emp.ConfirmedBasicIncome) DESC");

            ScalarQuery<int> q = new ScalarQuery<int>(typeof(Employment_DAO), sb.ToString(), this.Key);
            q.SetQueryRange(1);
            return q.Execute();
        }

        /// <summary>
        /// Returns the household income for the account.  Note that this relies on a computed database column so
        /// ensure the object has been persisted (and if necessary refreshed) before using this value.
        /// </summary>
        /// <returns></returns>
        public double GetHouseholdIncome()
        {
            double householdIncome = 0;

            string query = UIStatementRepository.GetStatement("Account", "GetHouseholdIncome");

            // Create a collection
            ParameterCollection parameters = new ParameterCollection();

            //Add the required parameters
            parameters.Add(new SqlParameter("@AccountKey", this.Key));

            // execute
            object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);

            // Get the Return Values
            householdIncome = o == null ? 0 : Convert.ToDouble(o);

            return householdIncome;
        }

        /// <summary>
        /// The PTI for the Account
        /// This is always based on the Amortising Instalment
        /// </summary>
        public double CalcAccountPTI
        {
            get
            {
                return MortgageLoanAccountHelper.CalcAccountPTI(this);
            }
        }

        /// <summary>
        /// Checks whether there has been an ArrearTransactionNewBalance greater than (Minimum) within the last (Months) months
        /// </summary>
        /// <param name="monthsInArrears"></param>
        /// <param name="minimumAmount"></param>
        /// <returns></returns>
        public bool HasBeenInArrears(int monthsInArrears, float minimumAmount)
        {
            string HQL = "from ArrearTransaction_DAO at where at.FinancialService.Account.Key = ? and at.Balance > ?";
            SimpleQuery<ArrearTransaction_DAO> q = new SimpleQuery<ArrearTransaction_DAO>(HQL, this.Key, minimumAmount);
            ArrearTransaction_DAO[] arrears = q.Execute();

            DateTime temp = DateTime.Now.AddMonths(-monthsInArrears);

            for (int i = 0; i < arrears.Length; i++)
            {
                if (arrears[i].EffectiveDate >= temp)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines if an account is open. The following Account statuses are considered open.
        /// <list type="bullet">
        ///     <item>Open</item>
        ///     <item>Locked</item>
        /// </list>
        /// </summary>
        public bool IsActive
        {
            get
            {
                return (this.AccountStatus.Key == (int)AccountStatuses.Open || this.AccountStatus.Key == (int)AccountStatuses.Dormant);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool UnderDebtCounselling
        {
            get
            {
                bool rslt = false;

                string query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "UnderDebtCounsellingByAccountKey");

                // Create a collection
                ParameterCollection parameters = new ParameterCollection();

                //Add the required parameters
                parameters.Add(new SqlParameter("@AccountKey", this.Key));

                // execute
                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);

                // Get the Return Values
                int count = Convert.ToInt16(o);
                if (count > 0)
                    rslt = true;

                return rslt;
            }
        }

        [Obsolete]
        public IFinancialService GetFinancialServiceByType(SAHL.Common.Globals.FinancialServiceTypes FinancialServiceType)
        {
            for (int i = 0; i < FinancialServices.Count; i++)
            {
                if (FinancialServices[i].FinancialServiceType.Key == (int)FinancialServiceType && FinancialServices[i].AccountStatus.Key == (int)AccountStatuses.Open)
                    return FinancialServices[i];
            }
            return null;
        }

        /// <summary>
        /// Gets all FinancialServices of the given FinancialServiceType and AccountStatus for this account
        /// </summary>
        /// <param name="FinancialServiceType"></param>
        /// <param name="Statuses"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IFinancialService> GetFinancialServicesByType(SAHL.Common.Globals.FinancialServiceTypes FinancialServiceType, AccountStatuses[] Statuses)
        {
            List<IFinancialService> list = new List<IFinancialService>();
            ArrayList AL = new ArrayList();
            for (int k = 0; k < Statuses.Length; k++)
            {
                AL.Add((int)Statuses[k]);
            }

            for (int i = 0; i < FinancialServices.Count; i++)
            {
                if (FinancialServices[i].FinancialServiceType.Key == (int)FinancialServiceType && AL.Contains(FinancialServices[i].AccountStatus.Key))
                    list.Add(FinancialServices[i]);
            }

            return new ReadOnlyEventList<IFinancialService>(list);
        }

        public IEventList<IAccount> GetNonProspectRelatedAccounts()
        {
            DomainMessageCollection messages = new DomainMessageCollection();
            EventList<IAccount> list = new EventList<IAccount>();

            for (int i = 0; i < this.RelatedChildAccounts.Count; i++)
            {
                if (this.RelatedChildAccounts[i].AccountStatus.Key != (int)AccountStatuses.ApplicationpriortoInstructAttorney)
                {
                    list.Add(messages, this.RelatedChildAccounts[i]);
                }
            }

            return list;
        }

        public IAccount GetRelatedAccountByType(SAHL.Common.Globals.AccountTypes AccountType, IEventList<IAccount> RelatedAccounts)
        {
            for (int i = 0; i < RelatedAccounts.Count; i++)
            {
                if (RelatedAccounts[i].AccountType == AccountType)
                    return RelatedAccounts[i];
            }
            return null;
        }

        public IReadOnlyEventList<ILegalEntity> GetLegalEntitiesByRoleType(IDomainMessageCollection Messages, int[] RoleTypes)
        {
            return GetLegalEntitiesByRoleType(Messages, RoleTypes, GeneralStatusKey.All);
        }

        public IReadOnlyEventList<ILegalEntity> GetLegalEntitiesByRoleType(IDomainMessageCollection Messages, int[] RoleTypes, GeneralStatusKey StatusKey)
        {
            ArrayList RTS = new ArrayList(RoleTypes);
            EventList<ILegalEntity> results = new EventList<ILegalEntity>();
            DomainMessageCollection DMC = new DomainMessageCollection();

            for (int i = 0; i < this.Roles.Count; i++)
            {
                if (RTS.Contains(Roles[i].RoleType.Key))
                {
                    if (StatusKey == GeneralStatusKey.All)
                        results.Add(DMC, Roles[i].LegalEntity);
                    else if (Roles[i].GeneralStatus.Key == Convert.ToInt32(StatusKey))
                        results.Add(DMC, Roles[i].LegalEntity);
                }
            }
            return new ReadOnlyEventList<ILegalEntity>(results);
        }

        public IReadOnlyEventList<ILegalEntityNaturalPerson> GetNaturalPersonLegalEntitiesByRoleType(IDomainMessageCollection Messages, int[] RoleTypes)
        {
            return GetNaturalPersonLegalEntitiesByRoleType(Messages, RoleTypes, GeneralStatusKey.All);
        }

        public IReadOnlyEventList<ILegalEntityNaturalPerson> GetNaturalPersonLegalEntitiesByRoleType(IDomainMessageCollection Messages, int[] RoleTypes, GeneralStatusKey StatusKey)
        {
            ArrayList RTS = new ArrayList(RoleTypes);
            EventList<ILegalEntityNaturalPerson> results = new EventList<ILegalEntityNaturalPerson>();
            DomainMessageCollection DMC = new DomainMessageCollection();

            for (int i = 0; i < this.Roles.Count; i++)
            {
                if (RTS.Contains(Roles[i].RoleType.Key))
                {
                    if (Roles[i].LegalEntity is ILegalEntityNaturalPerson)
                    {
                        if (StatusKey == GeneralStatusKey.All)
                            results.Add(DMC, Roles[i].LegalEntity as ILegalEntityNaturalPerson);
                        else if (Roles[i].GeneralStatus.Key == Convert.ToInt32(StatusKey))
                            results.Add(DMC, Roles[i].LegalEntity as ILegalEntityNaturalPerson);
                    }
                }
            }
            return new ReadOnlyEventList<ILegalEntityNaturalPerson>(results);
        }

        public void RemoveRolesForLegalEntity(IDomainMessageCollection Messages, ILegalEntity LegalEntity, int[] RoleTypes)
        {
            ArrayList RTS = new ArrayList(RoleTypes);

            // remove the roles from the account
            foreach (IRole role in this.Roles)
            {
                if (RTS.Contains(role.RoleType.Key) && role.LegalEntity.Key == LegalEntity.Key)
                    Roles.Remove(Messages, role);
            }

            // remove the roles from the legalentity
            foreach (IRole role in LegalEntity.Roles)
            {
                if (RTS.Contains(role.RoleType.Key) && role.Account.Key == this.Key)
                    LegalEntity.Roles.Remove(Messages, role);
            }
        }

        public abstract SAHL.Common.Globals.AccountTypes AccountType
        {
            get;
        }

        virtual protected void OnRoles_BeforeAdd(ICancelDomainArgs args, object Item)
        {
            IRole role = Item as IRole;

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            // check that a role for the legal entity does not already exist
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            int rulePassed = svc.ExecuteRule(spc.DomainMessages, "ValidateUniqueAccountRole", role);
            if (rulePassed == 0)
                args.Cancel = true;

            // warning validation : check that a legalentity does not play a role on another account/application belong to a different origination source
            rulePassed = svc.ExecuteRule(spc.DomainMessages, "LegalEntityOriginationSource", role.LegalEntity);
            if (rulePassed == 0)
                args.Cancel = true;

            // The following rules apply when an aplication is accepted (account roles are created)
            if (role.LegalEntity is ILegalEntityNaturalPerson)
            {
                ILegalEntityNaturalPerson legalEntityNaturalPerson = role.LegalEntity as ILegalEntityNaturalPerson;

                rulePassed = svc.ExecuteRule(spc.DomainMessages, "LegalEntityNaturalPersonMandatorySaluation", legalEntityNaturalPerson);
                if (rulePassed == 0)
                    args.Cancel = true;

                rulePassed = svc.ExecuteRule(spc.DomainMessages, "LegalEntityNaturalPersonMandatoryInitials", legalEntityNaturalPerson);
                if (rulePassed == 0)
                    args.Cancel = true;

                rulePassed = svc.ExecuteRule(spc.DomainMessages, "LegalEntityNaturalPersonMandatoryFirstName", legalEntityNaturalPerson);
                if (rulePassed == 0)
                    args.Cancel = true;

                rulePassed = svc.ExecuteRule(spc.DomainMessages, "LegalEntityNaturalPersonMandatorySurname", legalEntityNaturalPerson);
                if (rulePassed == 0)
                    args.Cancel = true;

                rulePassed = svc.ExecuteRule(spc.DomainMessages, "LegalEntityNaturalPersonUpdateProfilePreferedName", legalEntityNaturalPerson);
                if (rulePassed == 0)
                    args.Cancel = true;

                rulePassed = svc.ExecuteRule(spc.DomainMessages, "LegalEntityNaturalPersonMandatoryGender", legalEntityNaturalPerson);
                if (rulePassed == 0)
                    args.Cancel = true;

                rulePassed = svc.ExecuteRule(spc.DomainMessages, "LegalEntityNaturalPersonMandatoryMaritalStatus", legalEntityNaturalPerson);
                if (rulePassed == 0)
                    args.Cancel = true;

                rulePassed = svc.ExecuteRule(spc.DomainMessages, "LegalEntityNaturalPersonMandatoryPopulationGroup", legalEntityNaturalPerson);
                if (rulePassed == 0)
                    args.Cancel = true;

                rulePassed = svc.ExecuteRule(spc.DomainMessages, "LegalEntityNaturalPersonMandatoryEducation", legalEntityNaturalPerson);
                if (rulePassed == 0)
                    args.Cancel = true;

                rulePassed = svc.ExecuteRule(spc.DomainMessages, "LegalEntityNaturalPersonMandatoryCitizenType", legalEntityNaturalPerson);
                if (rulePassed == 0)
                    args.Cancel = true;

                rulePassed = svc.ExecuteRule(spc.DomainMessages, "LegalEntityNaturalPersonMandatoryIDNumber", legalEntityNaturalPerson);
                if (rulePassed == 0)
                    args.Cancel = true;

                rulePassed = svc.ExecuteRule(spc.DomainMessages, "LegalEntityNaturalPersonMandatoryDateOfBirth", legalEntityNaturalPerson);
                if (rulePassed == 0)
                    args.Cancel = true;

                rulePassed = svc.ExecuteRule(spc.DomainMessages, "LegalEntityNaturalPersonMandatoryPassportNumber", legalEntityNaturalPerson);
                if (rulePassed == 0)
                    args.Cancel = true;
            }

            //If we are cancelling a request, throw an error so that we know it has been canccelled.
            //This needs to be done after all items have been validated so that multiple/all messages are
            //reported to the UI
            if (args.Cancel)
                throw new SAHL.Common.Exceptions.DomainValidationException();
        }

        virtual protected void OnRoles_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnRelatedChildAccounts_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnRelatedChildAccounts_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnRelatedParentAccounts_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnRelatedParentAccounts_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnFinancialServices_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnFinancialServices_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnSubsidies_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnSubsidies_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnApplications_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected virtual void OnApplications_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnMailingAddresses_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnMailingAddresses_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnAccountProperties_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnAccountProperties_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnAccountInformations_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnAccountInformations_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnRoles_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        virtual protected void OnRoles_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnRelatedChildAccounts_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnRelatedChildAccounts_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnRelatedParentAccounts_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnRelatedParentAccounts_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnFinancialServices_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnFinancialServices_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnSubsidies_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnSubsidies_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplications_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnApplications_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnMailingAddresses_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnMailingAddresses_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnAccountProperties_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnAccountProperties_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnAccountInformations_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnAccountInformations_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnDetails_BeforeAdd(ICancelDomainArgs args, object Item)
        {
            IDetail detail = Item as IDetail;
            IDetailType detailType = detail.DetailType;
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IDomainMessageCollection dmc = spc.DomainMessages;

            // do a null check for detail type before running rules - if it is null the NotNull check will prevent the
            // user from continuing
            if (detailType != null)
            {
                IMortgageLoanAccount mortgageLoanAccount = this as IMortgageLoanAccount;
                if (mortgageLoanAccount != null)
                {
                    // for mortgage loan accounts, the detail type Readvance in Progress cannot be added if there
                    // is a Super Lo application in progress
                    if (Applications != null && detailType.Key == (int)DetailTypes.ReadvanceInProgress)
                    {
                        foreach (IApplication application in this.Applications)
                        {
                            if (application.CurrentProduct is IApplicationProductSuperLoLoan && application.ApplicationStatus.Key == (int)OfferStatuses.Open)
                            {
                                string msg = String.Format("The detail type {0} cannot be attached to accounts with Super Lo applications in progress.", detailType.Description);
                                dmc.Add(new Error(msg, msg));
                                args.Cancel = true;
                            }
                        }
                    }

                    // if the account balance is greater than 0, cannot add details with detail type of LoanClosed
                    // or PaidUpWithNoHOC
                    if (mortgageLoanAccount.LoanCurrentBalance > 0 && (detailType.Key == (int)DetailTypes.LoanClosed || detailType.Key == (int)DetailTypes.PaidUpWithNoHOC))
                    {
                        string msg = String.Format("The detail type {0} cannot be attached to accounts with a current balance greater than 0.", detailType.Description);
                        dmc.Add(new Error(msg, msg));
                        args.Cancel = true;
                    }

                    // if the debit order date is today, prevent them from adding detail types Bank Details Incorrect (217)
                    // or Debit Order Suspended (150)
                    if (mortgageLoanAccount.SecuredMortgageLoan != null && (detailType.Key == (int)DetailTypes.BankDetailsIncorrect || detailType.Key == (int)DetailTypes.DebitOrderSuspended))
                    {
                        foreach (IFinancialServiceBankAccount bankAccount in mortgageLoanAccount.SecuredMortgageLoan.FinancialServiceBankAccounts)
                        {
                            if (bankAccount.DebitOrderDay == DateTime.Now.Day)
                            {
                                string msg = String.Format("The detail type {0} cannot be attached on the account's debit order day.", detailType.Description);
                                dmc.Add(new Error(msg, msg));
                                args.Cancel = true;
                            }
                        }
                    }

                    // if (mortgageLoanAccount.SecuredMortgageLoan.FinancialServiceBankAccounts[0].DebitOrderDay
                }
            }

            //If we are cancelling a request, throw an error so that we know it has been canccelled.
            //This needs to be done after all items have been validated so that multiple/all messages are
            //reported to the UI
            if (args.Cancel)
                throw new SAHL.Common.Exceptions.DomainValidationException();
        }

        public void OnDetails_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnDetails_BeforeRemove(ICancelDomainArgs args, object Item)
        {
            IDetail detail = Item as IDetail;

            // new details can be removed at any stage
            if (detail.Key <= 0)
                return;

            IDetailType detailType = detail.DetailType;
            int detailTypeKey = detailType.Key;
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IDomainMessageCollection dmc = spc.DomainMessages;

            // the following rules apply to mortgage loan accounts
            if (this is IMortgageLoanAccount)
            {
                // if the detail type does not allow updates, we cannot allow the delete
                if (!detailType.AllowUpdate || !detailType.AllowUpdateDelete)
                {
                    string msg = String.Format("The detail type {0} attached to the detail does not allow updates, so the detail cannot be removed.", detailType.Description);
                    dmc.Add(new Error(msg, msg));
                    args.Cancel = true;
                }

                // cannot delete a detail with detail type of LoanClosed
                if (detailTypeKey == (int)DetailTypes.LoanClosed)
                {
                    string msg = String.Format("The detail type {0} cannot be removed.", detailType.Description);
                    dmc.Add(new Error(msg, msg));
                    args.Cancel = true;
                }

                // cannot delete detail with detail types of Paid Up with No HOC (100)
                // if there is a detail type with a detail class of Registration Process
                if (detailTypeKey == (int)DetailTypes.PaidUpWithNoHOC)
                {
                    foreach (IDetail d in this.Details)
                    {
                        if (d.DetailType != null && d.DetailType.DetailClass != null && d.DetailType.DetailClass.Key == (int)DetailClasses.RegistrationProcess)
                        {
                            string msg = "You cannot delete details with detail type of 'Paid Up with No HOC' if the loan is pending registration";
                            dmc.Add(new Error(msg, msg));
                            args.Cancel = true;
                        }
                    }
                }
            }

            //If we are cancelling a request, throw an error so that we know it has been canccelled.
            //This needs to be done after all items have been validated so that multiple/all messages are
            //reported to the UI
            if (args.Cancel)
                throw new SAHL.Common.Exceptions.DomainValidationException();
        }

        public void OnDetails_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnAccountBaselII_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnAccountBaselII_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnAccountBaselII_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnAccountBaselII_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("FinancialServiceBankAccountUpdateDebitOrderAmount");
        }

        #region Debt

        public void OnDebtCounsellings_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnDebtCounsellings_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnDebtCounsellings_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnDebtCounsellings_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        #endregion Debt

        /// <summary>
        /// Committed Loan Value
        /// </summary>
        public double CommittedLoanValue
        {
            get
            {
                double clv = 0;
                string sql = UIStatementRepository.GetStatement("Repositories.AccountRepository", "AccountCommittedLoanValue");
                ParameterCollection prms = new ParameterCollection();
                Helper.AddIntParameter(prms, "@AccountKey", this.Key);
                object ds = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), prms);

                if (ds != null)
                {
                    double.TryParse(ds.ToString(), out clv);
                }

                return clv;
            }
        }

        public bool HasActiveSubsidy
        {
            get
            {
                bool hasActiveSubsidy = false;

                foreach (ISubsidy subsidy in this.Subsidies)
                {
                    if (subsidy.GeneralStatus.Key == (int)GeneralStatuses.Active)
                    {
                        hasActiveSubsidy = true;
                        break;
                    }
                }

                return hasActiveSubsidy;
            }
        }

        /// <summary>
        /// Find the New Business application for this account (there should only be one) and check if it has the ThirtyYearTerm OfferAttribute
        /// </summary>
        public bool IsThirtyYearTerm
        {
            get 
            { 
                var newBusinessApplication = this.Applications.Where(x => x.ApplicationStatus.Key == (int)OfferStatuses.Accepted &&
                    (  x.ApplicationType.Key == (int)OfferTypes.NewPurchaseLoan 
                    || x.ApplicationType.Key == (int)OfferTypes.SwitchLoan 
                    || x.ApplicationType.Key == (int)OfferTypes.RefinanceLoan)).FirstOrDefault();

                return (newBusinessApplication != null && newBusinessApplication.HasAttribute(OfferAttributeTypes.Loanwith30YearTerm));
            }
        }

        public bool HasAccountInformationType(AccountInformationTypes accountInformationType)
        {
            return this.AccountInformations.Any(x => x.AccountInformationType.Key == (int)accountInformationType);
        }
    }
}