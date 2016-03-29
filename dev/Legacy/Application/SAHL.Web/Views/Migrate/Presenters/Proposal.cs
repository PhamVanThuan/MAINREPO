using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Views.Migrate.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Collections.Interfaces;
using System.Text;

namespace SAHL.Web.Views.Migrate.Presenters
{
    public class Proposal : SAHLCommonBasePresenter<SAHL.Web.Views.Migrate.Interfaces.IProposal>
    {
        private IMigrationDebtCounsellingRepository migrationDebtCounsellingRepository;
        private IAccountRepository accountRepository;

        public IMigrationDebtCounsellingRepository MigrationDebtCounsellingRepository
        {
            get
            {
                if (migrationDebtCounsellingRepository == null)
                {
                    migrationDebtCounsellingRepository = RepositoryFactory.GetRepository<IMigrationDebtCounsellingRepository>();
                }
                return migrationDebtCounsellingRepository;
            }
        }
        public IAccountRepository AccountRepository
        {
            get
            {
                if (accountRepository == null)
                {
                    accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
                }
                return accountRepository;
            }
        }
        public IMigrationDebtCounsellingProposal SelectedProposal { get; set; }
        public IMigrationDebtCounselling DebtCounsellingCase { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public Proposal(SAHL.Web.Views.Migrate.Interfaces.IProposal view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// On View Initialized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!GlobalCacheData.ContainsKey(ViewConstants.SelectedAccountKey))
            {
                _view.Messages.Add(new Error("There is no active Account to use", "There is no active Account to use"));
                return;
            }

            //Get the Account Key
            _view.AccountKey = Convert.ToInt32(GlobalCacheData[ViewConstants.SelectedAccountKey].ToString());

            //Use the Account Key to get the Debt Counselling Case
            DebtCounsellingCase = MigrationDebtCounsellingRepository.GetMigrationDebtCounsellingByAccountKey(_view.AccountKey);

            if (DebtCounsellingCase == null)
            {
                _view.Messages.Add(new Error("please ensure that a Debt Counselling Case ", "A Debt Counselling case for the Account does not exist"));
                return;
            }
            if (DebtCounsellingCase.DebtCounsellingProposals.FirstOrDefault() == null)
            {
                SelectedProposal = MigrationDebtCounsellingRepository.CreateEmptyProposal(); // add
                SelectedProposal.DebtCounselling = DebtCounsellingCase;
            }
            else
            {
                SelectedProposal = DebtCounsellingCase.DebtCounsellingProposals.First();
            }

            _view.ProposalStatusses = GetProposalStatusses();
            _view.InclusiveChoices = GetInclusiveChoices();
            _view.YesNoChoices = GetYesNoChoices();
            _view.MarketRates = GetMarketRates();
            _view.ApprovalUsers = MigrationDebtCounsellingRepository.GetApprovalUsers();

            _view.AddClick += new EventHandler<EventArgs>(OnAddClick);
            _view.RemoveClick += new EventHandler<SAHL.Common.Web.UI.Events.KeyChangedEventArgs>(OnRemoveClick);
            _view.BackClick += new EventHandler<EventArgs>(OnBackClick);
            _view.FinishClick += new EventHandler<EventArgs>(OnFinishClick);

            _view.HOCOrLifeChanged += new EventHandler<EventArgs>(OnHOCOrLifeChanged);

            IAccount account = AccountRepository.GetAccountByKey(_view.AccountKey);
            _view.TotalInstalmentAmount = (decimal)account.InstallmentSummary.TotalLoanInstallment;

            _view.BindProposal(SelectedProposal);
        }

        /// <summary>
        /// On Back Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnBackClick(object sender, EventArgs e)
        {
            //Save the Header first
            if (!SaveProposal())
            {
                return;
            }
            //Navigate to the Previous View
            Navigator.Navigate("CaseDetail");
        }

        /// <summary>
        /// On Finish Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnFinishClick(object sender, EventArgs e)
        {
            //Save the Header first
            if (!SaveProposal())
            {
                return;
            }
            //Navigate to the Next View (The Create new Migration Debt Counselling Case)
            Navigator.Navigate("CreateMigrateDCCase");
        }

        /// <summary>
        /// On Remove Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnRemoveClick(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            int proposalItemKey = Convert.ToInt32(e.Key);

            if (proposalItemKey <= 0)
            {
                string errorMessage = "Must select a proposal item";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }

            if (_view.IsValid)
            {
                try
                {
                    //Reset the input fields
                    _view.ResetInputFields();

                    // get the proposal item
                    IMigrationDebtCounsellingProposalItem pi = MigrationDebtCounsellingRepository.GetMigrationProposalItemByKey(proposalItemKey);

                    // remove the proposal item
                    SelectedProposal.DebtCounsellingProposalItems.Remove(_view.Messages, pi);

                    MigrationDebtCounsellingRepository.SaveMigrationProposal(SelectedProposal);

                    _view.BindProposal(SelectedProposal);
                }
                catch (Exception)
                {
                    if (!_view.IsValid)
                        throw;
                }

            }
        }

        /// <summary>
        /// On Add Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnAddClick(object sender, EventArgs e)
        {
            //Proposal
            if (!SaveProposal())
            {
                return;
            }

            //Proposal Items
            if (!SaveProposalItems())
            {
                return;
            }

            _view.BindProposal(SelectedProposal);
        }

        /// <summary>
        /// On HOC or Life Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHOCOrLifeChanged(object sender, EventArgs e)
        {
            IAccount account = AccountRepository.GetAccountByKey(_view.AccountKey);
            CalculateProposal(account, SelectedProposal, _view.HOCInclusive, _view.LifeInclusive);
            SaveProposal();
        }

        /// <summary>
        /// Calculate
        /// </summary>
        /// <param name="account"></param>
        /// <param name="proposal"></param>
        /// <param name="hocIncusive"></param>
        /// <param name="lifeInclusive"></param>
        private void CalculateProposal(IAccount account, IMigrationDebtCounsellingProposal proposal, bool hocIncusive, bool lifeInclusive)
        {
            foreach (IMigrationDebtCounsellingProposalItem proposalItem in proposal.DebtCounsellingProposalItems)
            {
                decimal[] lifeAndHocPremiums = GetHOCAndLifeMonthlyPremium(account);
                decimal newInstalment = proposalItem.Amount + proposalItem.AdditionalAmount;

                if (lifeInclusive)
                {
                    newInstalment = newInstalment - lifeAndHocPremiums[0];
                    _view.LifeInstalment = -lifeAndHocPremiums[0];
                }

                if (hocIncusive)
                {
                    newInstalment = newInstalment - lifeAndHocPremiums[1];
                    _view.HOCInstalment = -lifeAndHocPremiums[1];
                }

                //Only Calculate the Instalment Percentage if the new Instalment is > 0 and the Total Loan Instalment > 0
                if (account.InstallmentSummary.TotalLoanInstallment > 0)
                {
                    proposalItem.InstalmentPercentage = newInstalment / (decimal)account.InstallmentSummary.TotalLoanInstallment;
                }
            }
        }

        /// <summary>
        /// Get HOC Monthly Premium
        /// </summary>
        /// <param name="account"></param>
        /// <returns>First Parameter is the </returns>
        private static decimal[] GetHOCAndLifeMonthlyPremium(IAccount account)
        {
            double lifePolicyMonthlyPremium = 0D;
            double hocMonthlyPremium = 0D;

            foreach (IAccount ca in account.RelatedChildAccounts)
            {
                IAccountLifePolicy aLP = ca as IAccountLifePolicy;
                if (aLP != null && aLP.AccountStatus.Key == (int)GeneralStatuses.Active)
                {
                    lifePolicyMonthlyPremium = aLP.LifePolicy.MonthlyPremium;
                }

                IAccountHOC aH = ca as IAccountHOC;
                if (aH != null && aH.AccountStatus.Key == (int)GeneralStatuses.Active)
                {
                    hocMonthlyPremium = aH.HOC.HOCMonthlyPremium ?? 0;
                }
            }
            return new decimal[] { (decimal)hocMonthlyPremium, (decimal)lifePolicyMonthlyPremium };
        }

        /// <summary>
        /// Save Proposal Items
        /// </summary>
        /// <returns></returns>
        private bool SaveProposalItems()
        {
            if (SelectedProposal.Key == 0)
            {
                _view.Messages.Add(new Error("Please ensure that the Proposal Details have been captured, before capturing Proposal line items.", "Please ensure that the Proposal Details have been captured, before capturing Proposal line items."));
            }
            //Proposal Item Validation
            if (!_view.ValidateProposalItem())
                return false;

            IMigrationDebtCounsellingProposalItem proposalItemToAdd = null;

            proposalItemToAdd = MigrationDebtCounsellingRepository.CreateEmptyProposalItem();
            proposalItemToAdd.DebtCounsellingProposal = SelectedProposal;
            _view.GetProposalItemFromView(proposalItemToAdd);

            //Save the Proposal Item if it's correct
            if (!ValidateProposalItemBusinessRules(SelectedProposal.DebtCounsellingProposalItems, proposalItemToAdd))
                return false;

            try
            {
                SelectedProposal.DebtCounsellingProposalItems.Add(_view.Messages, proposalItemToAdd);

                //Calculate the Proposal (HOC and Life Calculations)
                IAccount account = AccountRepository.GetAccountByKey(_view.AccountKey);
                CalculateProposal(account, SelectedProposal, _view.HOCInclusive, _view.LifeInclusive);

                MigrationDebtCounsellingRepository.SaveMigrationProposal(SelectedProposal);

                //Reset the input fields, since we have saved the item
                _view.ResetInputFields();
                return true;
            }
            catch (Exception)
            {
                if (_view.IsValid)
                    throw;

                return false;
            }

        }

        /// <summary>
        /// Save Proposal1235894
        /// </summary>
        /// <returns></returns>
        private bool SaveProposal()
        {
            //Proposal
            //Proposal Validation
            if (!_view.ValidateProposal())
                return false;

            if (!_view.SaveProposal)
                return true;

            try
            {
                _view.GetProposalFromView(SelectedProposal);

                //Save the Header if it's correct
                if (!ValidateProposalBusinessRules(SelectedProposal))
                {
                    return false;
                }
                else
                {
                    MigrationDebtCounsellingRepository.SaveMigrationProposal(SelectedProposal);
                }
                return true;
            }
            catch (Exception)
            {
                if (_view.IsValid)
                    throw;

                return false;
            }
        }

        /// <summary>
        /// Get Proposal Statusses
        /// </summary>
        /// <returns></returns>
        private static IDictionary<int, string> GetProposalStatusses()
        {
            //Proposal Statusses
            var proposalStatusses = Enum.GetValues(typeof(ProposalStatuses)).Cast<ProposalStatuses>()
                                      .Select(proposalStatus => new KeyValuePair<int, string>((int)proposalStatus, proposalStatus.ToString()));
            IDictionary<int, string> proposalStatusDictionary = new Dictionary<int, string>();
            foreach (KeyValuePair<int, string> item in proposalStatusses)
            {
                proposalStatusDictionary.Add(item);
            }
            return proposalStatusDictionary;
        }

        /// <summary>
        /// Get Inclusive Choices
        /// </summary>
        /// <returns></returns>
        private static IDictionary<int, string> GetInclusiveChoices()
        {
            IDictionary<int, string> inclusiveChoices = new Dictionary<int, string>();
            inclusiveChoices.Add(SAHL.Common.Constants.Proposals.HOCLifeInclusiveKey, SAHL.Common.Constants.Proposals.HOCLifeInclusiveDesc);
            inclusiveChoices.Add(SAHL.Common.Constants.Proposals.HOCLifeExclusiveKey, SAHL.Common.Constants.Proposals.HOCLifeExclusiveDesc);
            return inclusiveChoices;
        }

        /// <summary>
        /// Get Yes No Choices
        /// </summary>
        /// <returns></returns>
        private static IDictionary<int, string> GetYesNoChoices()
        {
            IDictionary<int, string> yesNoChoices = new Dictionary<int, string>();
            yesNoChoices.Add(1, "Yes");
            yesNoChoices.Add(0, "No");
            return yesNoChoices;
        }

        /// <summary>
        /// Get Market Rates
        /// </summary>
        /// <returns></returns>
        private static IDictionary<int, string> GetMarketRates()
        {
            IDictionary<int, string> marketRates = new Dictionary<int, string>();
            marketRates.Add(SAHL.Common.Constants.Proposals.FixedMarketRateKey, SAHL.Common.Constants.Proposals.FixedMarketRateDesc);
            return marketRates;
        }

        /// <summary>
        /// Validate Business Rules
        /// </summary>
        /// <param name="proposalItems"></param>
        /// <param name="proposalItemToAdd"></param>
        /// <returns></returns>
        private bool ValidateProposalItemBusinessRules(IEventList<IMigrationDebtCounsellingProposalItem> proposalItems, IMigrationDebtCounsellingProposalItem proposalItemToAdd)
        {
            bool valid = true;

            //if (proposalItemToAdd.StartDate > proposalItemToAdd.EndDate)
            //{
            //    valid = false;
            //    _view.Messages.Add(new Error("The Start Date cannot be after the End Date", "The Start Date cannot be after the End Date"));
            //}

            if (!DatesOverlap(proposalItems.ToList<IMigrationDebtCounsellingProposalItem>(), proposalItemToAdd))
            {
                valid = false;
            }

            //Validate Interest Rate
            if (proposalItemToAdd.InterestRate * 100 > 100 ||
                proposalItemToAdd.InterestRate * 100 < 0)
            {
                valid = false;
                _view.Messages.Add(new Error("The Interest Rate cannot be less than 0 or more than 100", "The Interest Rate cannot be less than 0 or more than 100"));
            }

            //Validate Amount
            if (proposalItemToAdd.Amount < 0)
            {
                valid = false;
                _view.Messages.Add(new Error("The Amount cannot be negative", "The Amount cannot be negative"));
            }

            //Validate Additional Amount
            if (proposalItemToAdd.Amount < 0)
            {
                valid = false;
                _view.Messages.Add(new Error("The Additional Amount cannot be negative", "The Additional Amount cannot be negative"));
            }

            //Validate Annual Escalation
            if (proposalItemToAdd.Amount < 0)
            {
                valid = false;
                _view.Messages.Add(new Error("The Annual Escalation cannot be negative", "The Annual Escalation cannot be negative"));
            }
            return valid;
        }

        /// <summary>
        /// Validate Proposal Business Rules
        /// </summary>
        /// <param name="proposal"></param>
        /// <returns></returns>
        private bool ValidateProposalBusinessRules(IMigrationDebtCounsellingProposal proposal)
        {
            bool valid = true;

            //Validate Proposal Status
            if (proposal.ProposalStatusKey == (int)ProposalStatuses.Active ||
                proposal.DebtCounselling.ApprovalDate.HasValue)
            {
                //Make sure that the Approval Date is set
                if (!proposal.DebtCounselling.ApprovalDate.HasValue)
                {
                    valid = false;
                    _view.Messages.Add(new Error("The Approval Date is Mandatory", "The Approval Date is Mandatory"));
                }
                //Make sure that the User is set
                if (!proposal.DebtCounselling.ApprovalUserKey.HasValue)
                {
                    valid = false;
                    _view.Messages.Add(new Error("The Approval User is Mandatory", "The Approval User is Mandatory"));
                }
                //Make sure that the Amount is correct
                //if (!proposal.DebtCounselling.ApprovalAmount.HasValue)
                //{
                //    valid = false;
                //    _view.Messages.Add(new Error("The Approval Amount is Mandatory", "The Approval Amount is Mandatory"));
                //}
                //Make sure that the Amount is correct
                if (!proposal.DebtCounselling.ReviewDate.HasValue)
                {
                    valid = false;
                    _view.Messages.Add(new Error("The Review Date is Mandatory", "The Review Amount is Mandatory"));
                }
            }
            return valid;
        }

        /// <summary>
        /// Check if any of the Dates Overlap
        /// </summary>
        /// <param name="proposalItems"></param>
        /// <param name="proposalItemToAdd"></param>
        /// <returns></returns>
        private bool DatesOverlap(IList<IMigrationDebtCounsellingProposalItem> proposalItems, IMigrationDebtCounsellingProposalItem proposalItemToAdd)
        {
            bool valid = true;
            List<IMigrationDebtCounsellingProposalItem> sortedProposalItems = new List<IMigrationDebtCounsellingProposalItem>(proposalItems);
            sortedProposalItems.Add(proposalItemToAdd);
            sortedProposalItems.Sort(delegate(IMigrationDebtCounsellingProposalItem c1, IMigrationDebtCounsellingProposalItem c2) { return c1.StartDate.CompareTo(c2.StartDate); });

            DateTime endDate = DateTime.MinValue;
            if (sortedProposalItems.Count > 0)
            {
                endDate = sortedProposalItems[0].EndDate;
            }

            StringBuilder messageBuilder = new StringBuilder();

            if (sortedProposalItems.Count > 1)
            {
                //Loop through each Proposal Item
                for (int count = 1; count < sortedProposalItems.Count; count++)
                {
                    //Compare the Current Proporal Item StartDate to the Previous Proposal Item EndDate
                    IMigrationDebtCounsellingProposalItem proposalItem = sortedProposalItems[count];
                    if (proposalItem.StartDate != endDate.AddDays(1))
                    {
                        messageBuilder.Append("EndDate: " + endDate.ToShortDateString() + " StartDate: " + proposalItem.StartDate.ToShortDateString() + "<br>");
                        valid = false;
                    }
                    endDate = proposalItem.EndDate;
                }
            }

            if (!valid)
            {
                _view.Messages.Add(new Error(messageBuilder.ToString(), messageBuilder.ToString()));
            }

            return valid;
        }
    }
}