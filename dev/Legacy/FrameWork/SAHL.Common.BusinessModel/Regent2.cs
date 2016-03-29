using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using System;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class Regent : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Regent_DAO>, IRegent, IAccount
    {
        public double FixedPayment
        {
            get { return this.RegentPremium; }
        }

        public IAccountStatus AccountStatus
        {
            get
            {
                IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                return accRepo.GetAccountStatusByKey(AccountStatuses.Open);
            }
        }

        public DateTime InsertedDate
        {
            get { return this.RegentInceptionDate.Value; }
        }

        public IOriginationSourceProduct OriginationSourceProduct
        {
            get { return null; }
        }

        public DateTime? OpenDate
        {
            get { return this.RegentNewBusinessDate.HasValue ? this.RegentNewBusinessDate : null; }
        }

        public DateTime? CloseDate
        {
            get { return null; }
        }

        public IProduct Product
        {
            get { return null; }
        }

        public IOriginationSource OriginationSource
        {
            get { return null; }
        }

        public string UserID
        {
            get { return null; }
        }

        public DateTime? ChangeDate
        {
            get { return this.RegentLastUpdateDate.HasValue ? this.RegentLastUpdateDate : null; }
        }

        int IAccount.Key
        {
            get { return Convert.ToInt32(this.Key); }
        }

        public IEventList<IRole> Roles
        {
            get { return null; }
        }

        public IEventList<IFinancialService> FinancialServices
        {
            get { return null; }
        }

        public IEventList<IApplication> Applications
        {
            get { return null; }
        }

        public IEventList<IAccount> RelatedChildAccounts
        {
            get { return null; }
        }

        public IAccount ParentAccount
        {
            get { return null; }
        }

        public IEventList<ISubsidy> Subsidies
        {
            get { return null; }
        }

        public IEventList<IMailingAddress> MailingAddresses
        {
            get { return null; }
        }

        public IEventList<IAccountInformation> AccountInformations
        {
            get { return null; }
        }

        public IEventList<IDetail> Details
        {
            get { return null; }
        }

        public IEventList<IAccountBaselII> AccountBaselII
        {
            get { return null; }
        }

        public ISPV SPV
        {
            get { return null; }
        }

        public IApplication GetLatestApplicationByType(Globals.OfferTypes OfferType)
        {
            return null;
        }

        public double GetHouseholdIncome()
        {
            return 0;
        }

        public IAccountBaselII GetLatestBehaviouralScore()
        {
            return null;
        }

        public double CalcAccountPTI
        {
            get { return 0; }
        }

        public System.Data.DataTable GetAccountRoleNotificationByTypeAndDecription(Globals.ReasonTypes rt, Globals.ReasonDescriptions rd)
        {
            return null;
        }

        public IReadOnlyEventList<ILegalEntity> GetLegalEntitiesByRoleType(IDomainMessageCollection Messages, int[] RoleTypes)
        {
            return null;
        }

        public IReadOnlyEventList<ILegalEntity> GetLegalEntitiesByRoleType(IDomainMessageCollection Messages, int[] RoleTypes, GeneralStatusKey StatusKey)
        {
            return null;
        }

        public IReadOnlyEventList<ILegalEntityNaturalPerson> GetNaturalPersonLegalEntitiesByRoleType(IDomainMessageCollection Messages, int[] RoleTypes)
        {
            return null;
        }

        public IReadOnlyEventList<ILegalEntityNaturalPerson> GetNaturalPersonLegalEntitiesByRoleType(IDomainMessageCollection Messages, int[] RoleTypes, GeneralStatusKey StatusKey)
        {
            return null;
        }

        public void RemoveRolesForLegalEntity(IDomainMessageCollection Messages, ILegalEntity LegalEntity, int[] RoleTypes)
        {
            throw new NotImplementedException();
        }

        public string GetLegalName(LegalNameFormat Format)
        {
            return null;
        }

        public Globals.AccountTypes AccountType
        {
            get { return Globals.AccountTypes.Regent; }
        }

        public IFinancialService GetFinancialServiceByType(Globals.FinancialServiceTypes FinancialServiceType)
        {
            return null;
        }

        public IReadOnlyEventList<IFinancialService> GetFinancialServicesByType(Globals.FinancialServiceTypes FinancialServiceType, Globals.AccountStatuses[] Statuses)
        {
            return null;
        }

        public IAccount GetRelatedAccountByType(Globals.AccountTypes AccountType, IEventList<IAccount> RelatedAccounts)
        {
            return null;
        }

        public int GetEmploymentTypeKey()
        {
            return 0;
        }

        public IEventList<IAccount> GetNonProspectRelatedAccounts()
        {
            return null;
        }

        public bool HasBeenInArrears(int Months, float Minimum)
        {
            return false;
        }

        public IAccountInstallmentSummary InstallmentSummary
        {
            get { return null; }
        }

        public bool IsActive
        {
            get { return false; }
        }

        public bool UnderDebtCounselling
        {
            get { return false; }
        }

        public double CommittedLoanValue
        {
            get { return 0; }
        }

        public bool HasActiveSubsidy
        {
            get { return false; }
        }
    }
}