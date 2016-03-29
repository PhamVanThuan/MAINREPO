using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Utils;

namespace SAHL.Common.BusinessModel.Rules.DebtCounselling
{
    #region Proposal

    [RuleDBTag("DebtCounsellingDuplicateDraftProposal",
    "Draft Proposal/Counter Proposal must only exist once for a DebtCounselling case.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingDuplicateDraftProposal")]
    [RuleInfo]
    public class DebtCounsellingDuplicateDraftProposal : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellingDuplicateDraftProposal rule expects a Domain object to be passed.");

            IProposal p = (IProposal)Parameters[0];
            if (p == null)
                throw new ArgumentException("The DebtCounsellingDuplicateDraftProposal rule expects the following objects to be passed: IProposal.");

            if (p.ProposalStatus.Key == (int)ProposalStatuses.Draft)
            {
                // look for a draft proposal of my type, that is not me
                IDebtCounsellingRepository debtCounsellingRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
                List<IProposal> proposals = debtCounsellingRepo.GetProposalsByTypeAndStatus(p.DebtCounselling.Key, (ProposalTypes)p.ProposalType.Key, ProposalStatuses.Draft);
                foreach (IProposal dp in proposals)
                {
                    if (dp.Key != p.Key)//found a draft that is not me
                    {
                        string msg = String.Format(@"A Draft {0} exists for this Debt Counselling case.", p.ProposalType.Description);
                        AddMessage(msg, msg, Messages);
                        return 0;
                    }
                }
            }

            return 1;
        }
    }

    [RuleDBTag("DebtCounsellingRelatedActiveProposalExists",
    "Related Debt Counselling Cases must not have an active proposal.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingRelatedActiveProposalExists")]
    [RuleInfo]
    public class DebtCounsellingRelatedActiveProposalExists : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellingRelatedActiveProposalExists rule expects a Domain object to be passed.");

            IDebtCounselling debtcounselling = (IDebtCounselling)Parameters[0];
            if (debtcounselling == null)
                throw new ArgumentException("The DebtCounsellingRelatedActiveProposalExists rule expects the following objects to be passed: IDebtCounselling.");

            IDebtCounsellingRepository debtCounsellingRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

            //Get Related Debt Counselling Cases for this account
            if (debtcounselling.Account != null)
            {
                List<IDebtCounselling> debtCounsellingCases = debtCounsellingRepo.GetDebtCounsellingByAccountKey(debtcounselling.Account.Key, DebtCounsellingStatuses.Open);
                foreach (IDebtCounselling debtCounsellingCase in debtCounsellingCases)
                {
                    //Don't check existing debt counselling case for accepted proposal but only related ones
                    if (debtCounsellingCase.Key != debtcounselling.Key)
                    {
                        // look for an active proposal
                        List<IProposal> proposals = debtCounsellingRepo.GetProposalsByTypeAndStatus(debtCounsellingCase.Key, ProposalTypes.Proposal, ProposalStatuses.Active);
                        if (proposals != null)
                        {
                            //Check if there is an accepted proposal
                            foreach (IProposal proposal in proposals)
                            {
                                if (proposal.Accepted.HasValue && proposal.Accepted.Value == true)
                                {
                                    string msg = "There are related Accepted Proposal/s for this Account.";
                                    AddMessage(msg, msg, Messages);
                                    return 0;
                                }
                            }
                        }
                    }
                }
            }

            return 1;
        }
    }

    [RuleDBTag("DebtCounsellingActiveProposalExists",
"Active Proposal must exist for a DebtCounselling case.",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingActiveProposalExists")]
    [RuleInfo]
    public class DebtCounsellingActiveProposalExists : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellingActiveProposalExists rule expects a Domain object to be passed.");

            IDebtCounselling debtcounselling = (IDebtCounselling)Parameters[0];
            if (debtcounselling == null)
                throw new ArgumentException("The DebtCounsellingActiveProposalExists rule expects the following objects to be passed: IDebtCounselling.");

            // look for an active proposal
            IDebtCounsellingRepository debtCounsellingRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
            List<IProposal> proposals = debtCounsellingRepo.GetProposalsByTypeAndStatus(debtcounselling.Key, ProposalTypes.Proposal, ProposalStatuses.Active);
            if (proposals == null || proposals.Count <= 0)
            {
                string msg = "No Active Proposal exists for this Debt Counselling case.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("DebtCounsellingActiveCounterProposalExists",
"Active Counter Proposal must exist for a DebtCounselling case, and be the latest Counter Proposal.",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingActiveCounterProposalExists")]
    [RuleInfo]
    public class DebtCounsellingActiveCounterProposalExists : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellingActiveCounterProposalExists rule expects a Domain object to be passed.");

            IDebtCounselling debtcounselling = (IDebtCounselling)Parameters[0];
            if (debtcounselling == null)
                throw new ArgumentException("The DebtCounsellingActiveCounterProposalExists rule expects the following objects to be passed: IDebtCounselling.");

            // look for an active counter proposal
            IDebtCounsellingRepository debtCounsellingRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
            List<IProposal> proposals = debtCounsellingRepo.GetProposalsByType(debtcounselling.Key, ProposalTypes.CounterProposal);
            int activeCPKey = 0;
            int maxCPKey = 0;
            foreach (IProposal p in proposals)
            {
                //get the max key as the latest item
                if (p.Key > maxCPKey)
                    maxCPKey = p.Key;

                //get the active key if it exists
                if (p.ProposalStatus.Key == (int)ProposalStatuses.Active)
                    activeCPKey = p.Key;
            }
            string msg = String.Empty;

            if (proposals == null || proposals.Count <= 0)
                msg = "There are no Counter Proposals for this case.";

            if (activeCPKey == 0)
                msg = "The is no Active Counter Proposal for this case.";

            if (maxCPKey > activeCPKey)
                msg = "The latest Counter Proposal must be active for this case.";

            if (!String.IsNullOrEmpty(msg))
            {
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("CounterProposalReasonMemo",
    "Active Counter Proposals must have a Memo captured that describes the reason for the counter.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.CounterProposalReasonMemo")]
    [RuleInfo]
    public class CounterProposalReasonMemo : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The CounterProposalReasonMemo rule expects a Domain object to be passed.");

            IProposal proposal = (IProposal)Parameters[0];
            if (proposal == null)
                throw new ArgumentException("The CounterProposalReasonMemo rule expects the following objects to be passed: IProposal.");

            if (proposal.ProposalStatus.Key == (int)ProposalStatuses.Active
                && proposal.ProposalType.Key == (int)ProposalTypes.CounterProposal
                && (
                    proposal.Memo == null
                    || String.IsNullOrEmpty(proposal.Memo.Description)
                    )
                )
            {
                string msg = "A Counter Proposal note is required before you can set the status to active.";
                AddMessage(msg, msg, Messages);
                return 1;
            }

            return 0;
        }
    }

    [RuleDBTag("DebtCounsellingProposalTermLimit",
    "Active Proposal Term Limit.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingProposalTermLimit")]
    [RuleInfo]
    public class DebtCounsellingProposalTermLimit : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length != 2)
                throw new ArgumentException("The DebtCounsellingProposalTermLimit rule expects a two parameters to be passed.");

            IDebtCounselling debtcounselling = (IDebtCounselling)Parameters[0];
            if (debtcounselling == null)
                throw new ArgumentException("The DebtCounsellingProposalTermLimit rule expects the first parameter to be: IDebtCounselling.");

            IReasonDefinition reasonDefinition = (IReasonDefinition)Parameters[1];
            if (reasonDefinition == null)
                throw new ArgumentException("The DebtCounsellingProposalTermLimit rule expects the second parameter to be: IReasonDefinition.");

            // look for an active proposal
            IProposal proposal = debtcounselling.GetActiveProposal(ProposalTypes.Proposal);

            if (proposal != null)
            {
                IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
                IControl ctrl = ctrlRepo.GetControlByDescription("MaxProposalTerm");

                if (proposal.TotalTerm > Convert.ToInt32(ctrl.ControlNumeric))
                {
                    string msg = "The total term of the loan cannot exceed " + Convert.ToInt32(ctrl.ControlNumeric) + " months from the Registration Date.";
                    AddMessage(msg, msg, Messages);
                    return 0;
                }
            }
            return 1;
        }
    }

    [RuleDBTag("DebtCounsellingProposalRemainingTerm",
  "Remaining Proposal Term.",
  "SAHL.Rules.DLL",
  "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingProposalRemainingTerm")]
    [RuleInfo]
    public class DebtCounsellingProposalRemainingTerm : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellingProposalRemainingTerm rule expects a Domain object to be passed.");

            IDebtCounselling debtcounselling = (IDebtCounselling)Parameters[0];
            if (debtcounselling == null)
                throw new ArgumentException("The DebtCounsellingProposalRemainingTerm rule expects the following objects to be passed: IDebtCounselling.");

            // look for an active proposal
            IProposal proposal = debtcounselling.GetActiveProposal(ProposalTypes.Proposal);

            if (proposal != null)
            {
                IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
                IControl ctrl = ctrlRepo.GetControlByDescription("MaxProposalTerm");

                if (proposal.TotalTerm > Convert.ToInt32(ctrl.ControlNumeric))
                {
                    string msg = "The total term of the loan cannot exceed " + Convert.ToInt32(ctrl.ControlNumeric) + " months from the Registration Date.";
                    AddMessage(msg, msg, Messages);
                    return 0;
                }
            }

            return 1;
        }
    }

    [RuleDBTag("DebtCounsellingOpenCasesExist",
"Open cases linked to a Debt Counsellor Exist.",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingOpenCasesExist")]
    [RuleInfo]
    public class DebtCounsellingOpenCasesExist : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellingOpenCasesExist rule expects a Domain object to be passed.");

            ILegalEntity le = (ILegalEntity)Parameters[0];
            if (le == null)
                throw new ArgumentException("The DebtCounsellingOpenCasesExist rule expects the following objects to be passed: ILegalEntity.");

            ILegalEntityRepository leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            IReadOnlyEventList<IExternalRole> exRoles = leRepo.GetExternalRoles(GenericKeyTypes.DebtCounselling2AM, ExternalRoleTypes.DebtCounsellor, le.Key);

            var er = (from r in exRoles where r.GeneralStatus.Key == (int)GeneralStatuses.Active select r);

            if (er.ToList().Count > 0)
            {
                string msg = "Debt Counsellor is currently linked to active cases.";
                AddMessage(msg, msg, Messages);
                return 1;
            }

            return 0;
        }
    }

    [RuleDBTag("DebtCounsellingActiveCounterProposalRequiresReason",
"For a Debt Counselling Counter Proposal to be Active, a Reason must be added.",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingActiveCounterProposalRequiresReason")]
    [RuleInfo]
    public class DebtCounsellingActiveCounterProposalRequiresReason : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellingActiveCounterProposalRequiresReason rule expects a Proposal object to be passed.");

            IProposal proposal = (IProposal)Parameters[0];
            if (proposal == null)
                throw new ArgumentException("The DebtCounsellingActiveCounterProposalRequiresReason rule expects the following objects to be passed: IProposal.");

            if (proposal.ProposalType.Key != (int)ProposalTypes.CounterProposal)
                throw new ArgumentException("The DebtCounsellingActiveCounterProposalRequiresReason rule expects a Counter Proposal to be passed.");

            if (proposal.ActiveReason == null)
            {
                string msg = "A Reason must exist for the Counter Proposal.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    #endregion Proposal

    #region Proposal Item

    [RuleDBTag("ProposalItemsDatesOverlap",
    "Proposal Items Dates must not overlap and there must be no gaps.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.ProposalItemsDatesOverlap")]
    [RuleInfo]
    public class ProposalItemsDatesOverlap : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ProposalItemsDatesOverlap rule expects a Domain object to be passed.");

            IProposal proposal = (IProposal)Parameters[0];
            if (proposal == null)
                throw new ArgumentException("The ProposalItemsDatesOverlap rule expects the following objects to be passed: IProposal.");

            List<IProposalItem> sortedProposalItems = new List<IProposalItem>(proposal.ProposalItems);
            sortedProposalItems.Sort(delegate(IProposalItem c1, IProposalItem c2) { return c1.StartDate.CompareTo(c2.StartDate); });

            //Dictionary<int, object> _proposalItems = new Dictionary<int, object>();

            StringBuilder msg = new StringBuilder();
            msg.Append("Overlapping dates or gaps exist - " + "<br>");
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            if (sortedProposalItems.Count > 0)
            {
                startDate = sortedProposalItems[0].StartDate;
                endDate = sortedProposalItems[0].EndDate;
            }

            bool bOverLap = false;
            if (sortedProposalItems.Count > 1)
            {
                //Loop through each Proposal Item
                for (int count = 1; count < sortedProposalItems.Count; count++)
                {
                    //Compare the Current Proporal Item StartDate to the Previous Proposal Item EndDate
                    IProposalItem proposalItem = sortedProposalItems[count];
                    if (proposalItem.StartDate != endDate.AddDays(1))
                    {
                        bOverLap = true;
                        msg.Append("EndDate: " + endDate.ToShortDateString() + " StartDate: " + proposalItem.StartDate.ToShortDateString() + "<br>");
                    }
                    startDate = proposalItem.StartDate;
                    endDate = proposalItem.EndDate;
                }
            }

            if (bOverLap)
            {
                AddMessage(msg.ToString(), msg.ToString(), Messages);
                return 1;
            }

            return 0;
        }
    }

    [RuleDBTag("ProposalItemStartDateMandatory",
    "Proposal Item Start Date is madatory.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.ProposalItemStartDateMandatory")]
    [RuleInfo]
    public class ProposalItemStartDateMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ProposalItemStartDateMandatory rule expects a Domain object to be passed.");

            IProposalItem proposalItem = (IProposalItem)Parameters[0];
            if (proposalItem == null)
                throw new ArgumentException("The ProposalItemStartDateMandatory rule expects the following objects to be passed: IProposalItem.");

            if (proposalItem.StartDate == DateTime.MinValue)
            {
                string msg = "Start Date is required.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("ProposalItemEndDateMandatory",
    "Proposal Item Start Date is mandatory.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.ProposalItemEndDateMandatory")]
    [RuleInfo]
    public class ProposalItemEndDateMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ProposalItemEndDateMandatory rule expects a Domain object to be passed.");

            IProposalItem proposalItem = (IProposalItem)Parameters[0];
            if (proposalItem == null)
                throw new ArgumentException("The ProposalItemEndDateMandatory rule expects the following objects to be passed: IProposalItem.");

            if (proposalItem.EndDate == DateTime.MinValue)
            {
                string msg = "End Date is required.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("ProposalItemDates",
    "Checks that the Start Date is not greater than the End Date.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.ProposalItemDates")]
    [RuleInfo]
    public class ProposalItemDates : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ProposalItemDates rule expects a Domain object to be passed.");

            IProposalItem proposalItem = (IProposalItem)Parameters[0];
            if (proposalItem == null)
                throw new ArgumentException("The ProposalItemDates rule expects the following objects to be passed: IProposalItem.");

            if (proposalItem.StartDate > proposalItem.EndDate)
            {
                string msg = "Start Date cannot be greater than End Date.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("ProposalItemInterestRateMandatory",
    "Proposal Item Interest Rate is mandatory.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.ProposalItemInterestRateMandatory")]
    [RuleInfo]
    public class ProposalItemInterestRateMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ProposalItemInterestRateMandatory rule expects a Domain object to be passed.");

            IProposalItem proposalItem = (IProposalItem)Parameters[0];
            if (proposalItem == null)
                throw new ArgumentException("The ProposalItemInterestRateMandatory rule expects the following objects to be passed: IProposalItem.");

            if (proposalItem.InterestRate * 100 <= 0)
            {
                string msg = "A Positive Interest Rate is required.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("ProposalMaxInterestRate",
    "This rule checks that the interest rate set is within the desired range.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.ProposalMaxInterestRate")]
    [RuleParameterTag(new string[] { "@MaxInterestRate,99.99,7", "@MaxLinkRate,9.99,7" })]
    [RuleInfo]
    public class ProposalMaxInterestRate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The ProposalMaxInterestRate rule expects a Domain object to be passed.");

            IProposalItem proposalItem = (IProposalItem)Parameters[0];
            if (proposalItem == null)
                throw new ArgumentException("The ProposalMaxInterestRate rule expects the following objects to be passed: IProposalItem.");

            double maxInterestRate = 0;
            double maxLinkRate = 0;
            double rate = 0;
            foreach (IRuleParameter prm in RuleItem.RuleParameters)
            {
                if (prm.Name == "@MaxInterestRate")
                    maxInterestRate = Convert.ToDouble(prm.Value);
                if (prm.Name == "@MaxLinkRate")
                    maxLinkRate = Convert.ToDouble(prm.Value);
            }

            #endregion Check for allowed object type(s)

            if (proposalItem.MarketRate == null)
                rate = maxInterestRate;
            else
            {
                switch (proposalItem.MarketRate.Key)
                {
                    case (int)MarketRates.ThreeMonthJIBAR:
                    case (int)MarketRates.RepoRate:
                    case (int)MarketRates.PrimeLendingRate:
                        rate = maxLinkRate;
                        break;
                    default:
                        rate = maxInterestRate;
                        break;
                }
            }

            if (proposalItem.InterestRate * 100 > rate)
            {
                string msg = string.Format(@"The Interest Rate exceeds the maximum expected of {0}%", rate);
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("ProposalItemAmountMandatory",
    "Proposal Item Amount is mandatory.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.ProposalItemAmountMandatory")]
    [RuleInfo]
    public class ProposalItemAmountMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ProposalItemAmountMandatory rule expects a Domain object to be passed.");

            IProposalItem proposalItem = (IProposalItem)Parameters[0];
            if (proposalItem == null)
                throw new ArgumentException("The ProposalItemAmountMandatory rule expects the following objects to be passed: IProposalItem.");

            if (proposalItem.Amount < 0)
            {
                string msg = "Amount must be Zero or greater.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("ProposalItemAmountZeroValue",
   "Proposal Item Amount is mandatory.",
   "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.DebtCounselling.ProposalItemAmountZeroValue")]
    [RuleInfo]
    public class ProposalItemAmountZeroValue : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The ProposalItemAmountZeroValue rule expects a Domain object to be passed.");

            IProposalItem proposalItem = (IProposalItem)Parameters[0];
            if (proposalItem == null)
                throw new ArgumentException("The ProposalItemAmountZeroValue rule expects the following objects to be passed: IProposalItem.");

            if (proposalItem.Amount == 0)
            {
                string msg = "Are you sure you want to save a zero amount?";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("DeleteDraftProposal",
   "Deleting a draft proposal.",
   "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.DebtCounsellingDeleteDraftProposal")]
    [RuleInfo]
    public class DeleteDraftProposal : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DeleteDraftProposal rule expects a Domain object to be passed.");

            IProposal proposal = (IProposal)Parameters[0];
            if (proposal == null)
                throw new ArgumentException("The DeleteDraftProposal rule expects the following objects to be passed: IProposal.");

            if (proposal.ProposalStatus.Key == (int)ProposalStatuses.Draft)
            {
                string msg = "Are you sure you want to delete proposal?";
                AddMessage(msg, msg, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("DeleteProposal",
   "Deleting a draft proposal.",
   "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.DebtCounsellingDeleteProposal")]
    [RuleInfo]
    public class DeleteProposal : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DeleteProposal rule expects a Domain object to be passed.");

            IProposal proposal = (IProposal)Parameters[0];
            if (proposal == null)
                throw new ArgumentException("The DeleteProposal rule expects the following objects to be passed: IProposal.");

            if (proposal.ProposalStatus.Key != (int)ProposalStatuses.Draft)
            {
                string msg = "Only draft proposals can be deleted";
                AddMessage(msg, msg, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("DebtCounsellingEWorkDetails",
   "Validate the details of the e-work loss control case.",
   "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingEWorkDetails")]
    [RuleInfo]
    public class DebtCounsellingEWorkDetails : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellingEWorkDetails rule expects a Domain object to be passed.");

            IDebtCounselling debtcounselling = (IDebtCounselling)Parameters[0];
            if (debtcounselling == null)
                throw new ArgumentException("The DebtCounsellingEWorkDetails rule expects the following objects to be passed: IDebtCounselling.");

            var accountkey = debtcounselling.Account.Key;
            var estagename = "";
            var eFolderId = "";
            IADUser aduser = null;

            var dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
            dcRepo.GetEworkDataForLossControlCase(accountkey, out estagename, out eFolderId, out aduser);

            // eFolderid null empty
            if (string.IsNullOrEmpty(eFolderId))
            {
                string msg = "There is no active case in the E-Work loss control map.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            // Check if there is an ADUser
            if (aduser == null)
            {
                string msg = "There is no E-Work User assigned to this case.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            // Check if the ADUser is active
            if (aduser.GeneralStatusKey.Key == (int)GeneralStatuses.Inactive)
            {
                string msg = "The E-Work User assigned to this case is inactive.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("DebtCounsellingHasLitigationAttorney",
    "A litigation Attorney has to be selected to continue further in the context the rule is being used.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingHasLitigationAttorney")]
    [RuleInfo]
    public class DebtCounsellingHasLitigationAttorney : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellingActiveProposalExists rule expects a Domain object to be passed.");

            IDebtCounselling debtcounselling = (IDebtCounselling)Parameters[0];
            if (debtcounselling == null)
                throw new ArgumentException("The DebtCounsellingHasLitigationAttorney rule expects the following objects to be passed: IDebtCounselling.");

            // look for an active Litigation Consultant
            if (debtcounselling.LitigationAttorney == null)
            {
                string msg = "Please select a Litigation Attorney before continuing this action.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            // look for an active Litigation Attorney contacts
            IAttorney attorney = debtcounselling.LitigationAttorney;

            IList<ILegalEntity> le = attorney.GetContacts(ExternalRoleTypes.DebtCounselling, GeneralStatuses.Active);
            if (le.Count == 0)
            {
                string msg = "The Litigation Attorney does not contain any contacts.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("DebtCounselling5DayTerminationReminderHearingAppearanceTypesCheck",
    "The 5 days timer must NOT fire if these HearingAppearanceTypes exist - CourtApplication, TribunalCourtApplication, OrderGranted, TribunalOrderGranted",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounselling5DayTerminationReminderHearingAppearanceTypesCheck")]
    [RuleInfo]
    public class DebtCounselling5DayTerminationReminderHearingAppearanceTypesCheck : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounselling5DayTerminationReminderCheck rule expects a Domain object to be passed.");

            int DebtCounsellingKey = Convert.ToInt32(Parameters[0]);

            List<int> HearingAppearanceTypeKeys = new List<int>();
            HearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.CourtApplication);
            HearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.TribunalCourtApplication);
            HearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.OrderGranted);
            HearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.TribunalOrderGranted);

            IDebtCounsellingRepository dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
            if (dcRepo.CheckHearingDetailExistsForDebtCounsellingKey(DebtCounsellingKey, (int)HearingTypes.Court, HearingAppearanceTypeKeys) ||
                dcRepo.CheckHearingDetailExistsForDebtCounsellingKey(DebtCounsellingKey, (int)HearingTypes.Tribunal, HearingAppearanceTypeKeys))
            {
                string msg = "The 5 Days timer cannot fire as the applicable Hearing Appearance Types exists on the Debt Counselling Case.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("DebtCounselling60DayTerminationReminderHearingAppearanceTypesCheck",
    "The 60 days timer must NOT fire if these HearingAppearanceTypes exist - CourtApplication, TribunalCourtApplication, OrderGranted, TribunalOrderGranted",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounselling60DayTerminationReminderHearingAppearanceTypesCheck")]
    [RuleInfo]
    public class DebtCounselling60DayTerminationReminderHearingAppearanceTypesCheck : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounselling60DayTerminationReminderCheck rule expects a Domain object to be passed.");

            int DebtCounsellingKey = Convert.ToInt32(Parameters[0]);

            List<int> HearingAppearanceTypeKeys = new List<int>();
            HearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.CourtApplication);
            HearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.TribunalCourtApplication);

            IDebtCounsellingRepository dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

            if (dcRepo.CheckHearingDetailExistsForDebtCounsellingKey(DebtCounsellingKey, (int)HearingTypes.Court, HearingAppearanceTypeKeys) ||
                dcRepo.CheckHearingDetailExistsForDebtCounsellingKey(DebtCounsellingKey, (int)HearingTypes.Tribunal, HearingAppearanceTypeKeys))
            {
                string msg = "Court Hearing Appearance Types Exist.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("DebtCounsellingDepositCheck",
   "DebtCounsellingDepositCheck looks for a deposit paid after the 17.1 has been sent",
   "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingDepositCheck")]
    [RuleInfo]
    public class DebtCounsellingDepositCheck : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellingDepositCheck rule expects a DebtCounselling Key to be passed.");

            int DebtCounsellingKey = Convert.ToInt32(Parameters[0]);

            IDebtCounsellingRepository dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
            IStageDefinitionRepository sdRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
            ILoanTransactionRepository ltRepo = RepositoryFactory.GetRepository<ILoanTransactionRepository>();

            //get DC Case
            IDebtCounselling dc = dcRepo.GetDebtCounsellingByKey(DebtCounsellingKey);
            if (dc == null) return 1;

            //get LT
            IFinancialTransaction lt = ltRepo.GetLastLoanTransactionByTransactionTypeAndAccountKey(TransactionTypes.InstalmentPaymentDeposit, dc.Account.Key, false);
            if (lt == null) return 1;

            //get ST
            List<int> sdsdgList = new List<int>();
            sdsdgList.Add((int)StageDefinitionStageDefinitionGroups.Received17pt1);

            IList<IStageTransition> stList = sdRepo.GetStageTransitionList(DebtCounsellingKey, (int)GenericKeyTypes.DebtCounselling2AM, sdsdgList);

            if (stList.Count > 0)
            {
                // get latest Received17pt1 StageTransition by key
                SAHL.Common.BusinessModel.Interfaces.IStageTransition st = stList.OrderByDescending(x => x.Key).FirstOrDefault();

                //we should only ever have one of these
                if (st.EndTransitionDate.HasValue && (st.EndTransitionDate < lt.EffectiveDate))
                {
                    string msg = "Deposits have been received.";
                    AddMessage(msg, msg, Messages);
                    return 0;
                }
            }
            return 1;
        }
    }

    [RuleDBTag("DebtCounselling45DayReminderHearingAppearanceTypesCheck",
    "The 45 days timer must NOT fire if these HearingAppearanceTypes exist - CourtApplication, TribunalCourtApplication, OrderGranted, TribunalOrderGranted",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounselling45DayReminderHearingAppearanceTypesCheck")]
    [RuleInfo]
    public class DebtCounselling45DayReminderHearingAppearanceTypesCheck : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounselling45DayReminderHearingAppearanceTypesCheck rule expects a Domain object to be passed.");

            int DebtCounsellingKey = Convert.ToInt32(Parameters[0]);

            List<int> hearingAppearanceTypeKeys = new List<int>();
            hearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.CourtApplication);
            hearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.TribunalCourtApplication);
            hearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.OrderGranted);
            hearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.TribunalOrderGranted);

            IDebtCounsellingRepository dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

            if (dcRepo.CheckHearingDetailExistsForDebtCounsellingKey(DebtCounsellingKey, (int)HearingTypes.Court, hearingAppearanceTypeKeys) ||
                dcRepo.CheckHearingDetailExistsForDebtCounsellingKey(DebtCounsellingKey, (int)HearingTypes.Tribunal, hearingAppearanceTypeKeys))
            {
                string msg = "The 45 Day timer cannot fire as the applicable Hearing Appearance Types exists on the Debt Counselling Case.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("DebtCounsellingHasPaymentDistributionAgent",
    "A Payment Distribution Agent has to be selected to continue further in the context the rule is being used.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingHasPaymentDistributionAgent")]
    [RuleInfo]
    public class DebtCounsellingHasPaymentDistributionAgent : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellingHasPaymentDistributionAgent rule expects a Domain object to be passed.");

            IDebtCounselling debtcounselling = (IDebtCounselling)Parameters[0];
            if (debtcounselling == null)
                throw new ArgumentException("The DebtCounsellingHasPaymentDistributionAgent rule expects the following objects to be passed: IDebtCounselling.");

            // look for an active Payment Distribution Agent
            if (debtcounselling.PaymentDistributionAgent == null)
            {
                string msg = "Please select a Payment Distribution Agent before continuing this action.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("DebtCounsellingHasPaymentDistributionAgentOrDebitOrder",
    "Please select a Payment Distribution Agent or add an active Debit Order Payment before continuing this action.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingHasPaymentDistributionAgentOrDebitOrder")]
    [RuleInfo]
    public class DebtCounsellingHasPaymentDistributionAgentOrDebitOrder : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellingHasPaymentDistributionAgentOrDebitOrder rule expects a Domain object to be passed.");

            IDebtCounselling debtcounselling = (IDebtCounselling)Parameters[0];
            if (debtcounselling == null)
                throw new ArgumentException("The DebtCounsellingHasPaymentDistributionAgentOrDebitOrder rule expects the following objects to be passed: IDebtCounselling.");

            // look for an active Payment Distribution Agent
            if (debtcounselling.PaymentDistributionAgent == null)
            {
                string msg = "Please select a Payment Distribution Agent or add an active Debit Order Payment, Subsidy Payment or Direct Payment before continuing this action.";
                IAccount account = debtcounselling.Account;
                if (account.Details != null)
                {
                    IList<IDetail> details = account.Details.Where(x => x.DetailType.Key == (int)DetailTypes.DebitOrderSuspended || x.DetailType.Key == (int)DetailTypes.BankDetailsIncorrect).ToList();
                    if (details == null || details.Count == 0)
                    {
                        IFinancialService financialService = account.GetFinancialServiceByType(FinancialServiceTypes.VariableLoan);
                        if (financialService != null)
                        {
                            if (financialService.CurrentBankAccount != null && (financialService.CurrentBankAccount.FinancialServicePaymentType.Key == (int)FinancialServicePaymentTypes.DebitOrderPayment
                                                                                || financialService.CurrentBankAccount.FinancialServicePaymentType.Key == (int)FinancialServicePaymentTypes.SubsidyPayment
                                                                                || financialService.CurrentBankAccount.FinancialServicePaymentType.Key == (int)FinancialServicePaymentTypes.DirectPayment))
                            {
                                return 1;
                            }
                            else
                            {
                                AddMessage(msg, msg, Messages);
                                return 0;
                            }
                        }
                        else
                        {
                            AddMessage(msg, msg, Messages);
                            return 0;
                        }
                    }
                    else
                    {
                        AddMessage(msg, msg, Messages);
                        return 0;
                    }
                }
                else
                {
                    IFinancialService financialService = account.GetFinancialServiceByType(FinancialServiceTypes.VariableLoan);
                    if (financialService != null)
                    {
                        if (financialService.CurrentBankAccount != null && (financialService.CurrentBankAccount.FinancialServicePaymentType.Key == (int)FinancialServicePaymentTypes.DebitOrderPayment
                                                                                || financialService.CurrentBankAccount.FinancialServicePaymentType.Key == (int)FinancialServicePaymentTypes.SubsidyPayment
                                                                                || financialService.CurrentBankAccount.FinancialServicePaymentType.Key == (int)FinancialServicePaymentTypes.DirectPayment))
                        {
                            return 1;
                        }
                        else
                        {
                            AddMessage(msg, msg, Messages);
                            return 0;
                        }
                    }
                    else
                    {
                        AddMessage(msg, msg, Messages);
                        return 0;
                    }
                }
            }

            return 1;
        }
    }

    #endregion Proposal Item

    #region Duplicate Cases

    [RuleDBTag("MultipleDebtCounsellingCasesForAccount",
    "More than one case exists for this account.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.MultipleDebtCounsellingCasesForAccount")]
    [RuleInfo]
    public class MultipleDebtCounsellingCasesForAccount : BusinessRuleBase
    {
        public MultipleDebtCounsellingCasesForAccount(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The MultipleDebtCounsellingCasesForAccount rule expects an object to be passed in.");

            int accountKey = Convert.ToInt32(Parameters[0]);
            if (accountKey < 1)
                throw new ArgumentException("The MultipleDebtCounsellingCasesForAccount rule expects the following objects to be passed: AccountKey.");

            string query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "UnderDebtCounsellingByAccountKey");

            // Create a collection
            ParameterCollection parameters = new ParameterCollection();

            //Add the required parameters
            parameters.Add(new SqlParameter("@AccountKey", accountKey));

            // execute
            object o = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);

            // Get the Return Values
            int count = Convert.ToInt16(o);
            if (count > 1)
            {
                string msg = "More than one active Case exists for this Account. Please investigate and resolve.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("MultipleDebtCounsellingGroupsForLegalEntity",
    "More than one DC Group exists for Legal Entities on this case.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.MultipleDebtCounsellingGroupsForLegalEntity")]
    [RuleInfo]
    public class MultipleDebtCounsellingGroupsForLegalEntity : BusinessRuleBase
    {
        public MultipleDebtCounsellingGroupsForLegalEntity(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The MultipleDebtCounsellingGroupsForLegalEntity rule expects an object to be passed in.");

            int accountKey = Convert.ToInt32(Parameters[0]);
            if (accountKey < 1)
                throw new ArgumentException("The MultipleDebtCounsellingGroupsForLegalEntity rule expects the following objects to be passed: AccountKey.");

            string query = UIStatementRepository.GetStatement("Repositories.DebtCounsellingRepository", "MultipleDebtCounsellingGroupsForLegalEntity");

            // Create a collection
            ParameterCollection parameters = new ParameterCollection();

            //Add the required parameters
            parameters.Add(new SqlParameter("@AccountKey", accountKey));

            // execute
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);

            //if we have more than 1 distinct record back, then add an error msg
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 1)
            {
                string msg = "More than one DC Group exists for Legal Entities on this case.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    #endregion Duplicate Cases

    #region Debt Counselling

    /// <summary>
    /// Debt Counselling Term Review Date Mandatory
    /// </summary>
    [RuleDBTag("DebtCounsellingTermReviewDateMandatory",
    "The Term Review Date on an Accepted Proposal is Mandatory",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingTermReviewDateMandatory")]
    [RuleInfo]
    public class DebtCounsellingTermReviewDateMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellingTermReviewDateMandatory rule expects a Domain object to be passed.");

            IDebtCounselling debtcounselling = (IDebtCounselling)Parameters[0];
            if (debtcounselling == null)
                throw new ArgumentException("The DebtCounsellingTermReviewDateMandatory rule expects the following objects to be passed: IDebtCounselling.");

            if (debtcounselling.AcceptedActiveProposal == null || debtcounselling.AcceptedActiveProposal.ReviewDate == null)
            {
                string msg = "The Review Date is Mandatory";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    /// <summary>
    /// Debt Counselling Payment Received Date Mandatory
    /// </summary>
    [RuleDBTag("DebtCounsellingPaymentReceivedDateMandatory",
    "The Payment Received Date on a Debt Counselling case is Mandatory",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingPaymentReceivedDateMandatory")]
    [RuleInfo]
    public class DebtCounsellingPaymentReceivedDateMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellingPaymentReceivedDateMandatory rule expects a Domain object to be passed.");

            IDebtCounselling debtcounselling = (IDebtCounselling)Parameters[0];
            if (debtcounselling == null)
                throw new ArgumentException("The DebtCounsellingPaymentReceivedDateMandatory rule expects the following objects to be passed: IDebtCounselling.");

            if (debtcounselling.PaymentReceivedDate == null)
            {
                string msg = "The Payment Received Date is Mandatory";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            if (debtcounselling.PaymentReceivedDate != null &&
                debtcounselling.PaymentReceivedDate > DateTime.Now)
            {
                string msg = "The Payment Received Date cannot be in the future";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    /// <summary>
    /// Debt Counselling Payment Received Amount Mandatory
    /// </summary>
    [RuleDBTag("DebtCounsellingPaymentReceivedAmountMandatory",
    "The Payment Received Amount on a Debt Counselling case is Mandatory",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingPaymentReceivedAmountMandatory")]
    [RuleInfo]
    public class DebtCounsellingPaymentReceivedAmountMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellingPaymentReceivedAmountMandatory rule expects a Domain object to be passed.");

            IDebtCounselling debtcounselling = (IDebtCounselling)Parameters[0];
            if (debtcounselling == null)
                throw new ArgumentException("The DebtCounsellingPaymentReceivedAmountMandatory rule expects the following objects to be passed: IDebtCounselling.");

            if (debtcounselling.PaymentReceivedAmount.GetValueOrDefault() <= 0)
            {
                string msg = "The Payment Received Amount is mandatory and must be greater than 0";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("DebtCounsellingMaximumReviewDateInMonths",
    "Ensure that the Maximum Review Date is not greater than x number of Months from Today",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingMaximumReviewDateInMonths")]
    [RuleInfo]
    [RuleParameterTag(new string[] { "@MaxReviewDateInMonths,18,9" })]
    public class DebtCounsellingMaximumReviewDateInMonths : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (RuleItem.RuleParameters.Count < 1)
                throw new Exception(String.Format("Missing rule parameter configuration for the rule {0}.", RuleItem.Name));

            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellingMaximumReviewDateInMonths rule expects a Domain object to be passed.");

            IDebtCounselling debtcounselling = (IDebtCounselling)Parameters[0];
            if (debtcounselling == null)
                throw new ArgumentException("The DebtCounsellingMaximumReviewDateInMonths rule expects the following objects to be passed: IDebtCounselling.");

            int maximumReviewDateInMonths = Convert.ToInt32(RuleItem.RuleParameters[0].Value);

            // look for an accepted proposal
            if (debtcounselling.AcceptedActiveProposal != null &&
                debtcounselling.AcceptedActiveProposal.ReviewDate != null &&
                debtcounselling.AcceptedActiveProposal.ReviewDate > DateTime.Now.AddMonths(maximumReviewDateInMonths))
            {
                string msg = String.Format("The Review Date cannot be more than {0} months from today. The latest possible date is {1}.",
                                            maximumReviewDateInMonths,
                                            DateTime.Now.AddMonths(maximumReviewDateInMonths).ToShortDateString());
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("DebtCounsellingMinimumReviewDate",
    "Ensure that the Term Review Date is not in the past",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingMinimumReviewDate")]
    [RuleInfo]
    public class DebtCounsellingMinimumReviewDate : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellingMinimumReviewDate rule expects a Domain object to be passed.");

            IDebtCounselling debtcounselling = (IDebtCounselling)Parameters[0];
            if (debtcounselling == null)
                throw new ArgumentException("The DebtCounsellingMinimumReviewDate rule expects the following objects to be passed: IDebtCounselling.");

            // look for an accepted proposal
            if (debtcounselling.AcceptedActiveProposal != null &&
                debtcounselling.AcceptedActiveProposal.ReviewDate != null &&
                debtcounselling.AcceptedActiveProposal.ReviewDate <= DateTime.Now)
            {
                string msg = "The Review Date may not be a date in the past.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("DebtCounsellingPaymentReceivedInstalmentExpectancyDateSameDay",
    "Ensure that the Payment Received Date and the Reset Instalment Expectancy Date fall on the same day.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingPaymentReceivedInstalmentExpectancyDateSameDay")]
    [RuleInfo]
    public class DebtCounsellingPaymentReceivedInstalmentExpectancyDateSameDay : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellingPaymentReceivedInstalmentExpectancyDateSameDay rule expects a Domain object to be passed.");

            if (Parameters.Length != 2)
                throw new ArgumentException("The DebtCounsellingPaymentReceivedInstalmentExpectancyDateSameDay rule expects 2 Dates to be passed.");

            DateTime? firstDate = Parameters[0] as DateTime?;
            DateTime? secondDate = Parameters[1] as DateTime?;

            // look for an accepted proposal
            if (firstDate != null &&
                secondDate != null &&
                firstDate.Value.Day != secondDate.Value.Day)
            {
                firstDate = firstDate ?? DateTime.MinValue;
                secondDate = secondDate ?? DateTime.MinValue;
                string msg = "The Payment Received Date and the Reset Instalment Expectancy Date do not have matching day values. Please ensure that the Debit Order has been adjusted correctly.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("DebtCounsellingDeleteDebitOrder",
    "Ensure that theDebit Order can not be deleted by a non Debt Couselling User.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingDeleteDebitOrder")]
    [RuleInfo]
    public class DebtCounsellingDeleteDebitOrder : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellingDeleteDebitOrder rule expects a Domain object to be passed.");

            if (Parameters.Length != 2)
                throw new ArgumentException("The DebtCounsellingDeleteDebitOrder rule expects a IFinancialServiceRecurringTransaction and SAHLPrincipal.");

            IManualDebitOrder manDebitOrder = Parameters[0] as IManualDebitOrder;
            SAHLPrincipal sp = Parameters[1] as SAHLPrincipal;
            IAccount act = manDebitOrder.FinancialService.Account as IAccount;

            if (!act.UnderDebtCounselling)
            {
                return 1;
            }

            if (sp.IsInRole("Debt Counselling Admin") ||
                sp.IsInRole("Debt Counselling Consultant") ||
                sp.IsInRole("Debt Counselling Court Consultant") ||
                sp.IsInRole("Debt Counselling Supervisor"))
            {
                return 1;
            }

            string msg = String.Format(@"Account is under debt counselling. This manual debit order was created for Debt Counselling,
                                        please contact {0} to update this manual debit order.", manDebitOrder.UserID);
            AddMessage(msg, msg, Messages);
            return 0;
        }
    }

    [RuleDBTag("IsDebtCounsellingUser",
    "Ensure that user is Debt Counselling User.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.IsDebtCounsellingUser")]
    [RuleInfo]
    public class IsDebtCounsellingUser : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The IsDebtCounsellingUser rule expects a Domain object to be passed.");

            SAHLPrincipal sp = Parameters[0] as SAHLPrincipal;

            if (sp.IsInRole("Debt Counselling Admin") ||
                sp.IsInRole("Debt Counselling Consultant") ||
                sp.IsInRole("Debt Counselling Court Consultant") ||
                sp.IsInRole("Debt Counselling Supervisor"))
            {
                return 1;
            }

            return 0;
        }
    }

    [RuleDBTag("RemoveLegalEntityFromDebtCounsellingCheck",
   "RemoveLegalEntityFromDebtCounsellingCheck checks if a Legal Entity can be removed from the DC case.",
   "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.DebtCounselling.RemoveLegalEntityFromDebtCounsellingCheck")]
    [RuleInfo]
    public class RemoveLegalEntityFromDebtCounsellingCheck : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The RemoveLegalEntityFromDebtCounsellingCheck rule expects a DebtCounselling & LegalEntity Key to be passed.");

            if (Parameters.Length != 2)
                throw new ArgumentException("The RemoveLegalEntityFromDebtCounsellingCheck rule expects a DebtCounselling & LegalEntity Key to be passed.");

            int debtCounsellingKey = Convert.ToInt32(Parameters[0]);
            int legalEntityKey = Convert.ToInt32(Parameters[1]);
            int legalEntityCount = 0;

            ILegalEntityRepository legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            IReadOnlyEventList<IExternalRole> externalRoles = legalEntityRepo.GetExternalRoles(debtCounsellingKey, GenericKeyTypes.DebtCounselling2AM, ExternalRoleTypes.Client, GeneralStatuses.Active);

            foreach (IExternalRole externalRole in externalRoles)
            {
                if (externalRole.GenericKeyType.Key == (int)GenericKeyTypes.DebtCounselling2AM
                    && externalRole.GenericKey == debtCounsellingKey
                    && externalRole.LegalEntity.Key != legalEntityKey
                    && externalRole.GeneralStatus.Key == (int)GeneralStatuses.Active
                    && externalRole.ExternalRoleType.Key == (int)ExternalRoleTypes.Client)
                {
                    legalEntityCount++;
                }
            }

            if (legalEntityCount > 0)
                return 1;
            else
            {
                ILegalEntity le = legalEntityRepo.GetLegalEntityByKey(legalEntityKey);
                string msg = le.DisplayName + " is the only legal entity linked to this debt counselling case and cannot be removed.";
                AddMessage(msg, msg, Messages);
                return 0;
            }
        }
    }

    /// <summary>
    /// Check to see if the Declined proposal has reasons captured against it.
    /// </summary>
    [RuleDBTag("DebtCounsellingProposalDeclineWithReasons",
    "The Declined Proposal Must have reasons",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingProposalDeclineWithReasons")]
    [RuleInfo]
    public class DebtCounsellingProposalDeclineWithReasons : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellingProposalDeclineWithReasons rule expects a Debt Counselling Case.");

            IDebtCounselling debtCounsellingCase = Parameters[0] as IDebtCounselling;

            IStageDefinitionRepository stageDefinitionRepository = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
            List<int> declineTypes = new List<int>();

            declineTypes.Add((int)StageDefinitionStageDefinitionGroups.ConsultantDeclineReasons);
            declineTypes.Add((int)StageDefinitionStageDefinitionGroups.DeclineProposal);

            IList<IStageTransition> tranlist = stageDefinitionRepository.GetStageTransitionList(debtCounsellingCase.Key, (int)GenericKeyTypes.DebtCounselling2AM, declineTypes);

            if (tranlist.Count > 0)
                return 1;

            string msg = "Decline Reasons must be captured in order for this document to be sent.";
            AddMessage(msg, msg, Messages);
            return 0;
        }
    }

    /// <summary>
    /// Debt Counselling Court Order With Appeal Exists
    /// </summary>
    [RuleDBTag("DebtCounsellingCourtOrderWithAppealExist",
    "Debt Counselling Court Order With Appeal Exists",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingCourtOrderWithAppealExist")]
    [RuleInfo]
    public class DebtCounsellingCourtOrderWithAppealExist : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellingCourtOrderWithAppealExist rule expects a Debt Counselling Case.");

            //Get the Debt Counselling Case
            IDebtCounselling debtCounsellingCase = Parameters[0] as IDebtCounselling;

            IStageDefinitionRepository stageDefinitionRepository = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
            IReasonRepository reasonRepository = RepositoryFactory.GetRepository<IReasonRepository>();

            List<int> declineTypes = new List<int>();

            //The stage transitions to look for
            declineTypes.Add((int)StageDefinitionStageDefinitionGroups.ConsultantDeclineReasons);
            declineTypes.Add((int)StageDefinitionStageDefinitionGroups.DeclineProposal);

            //Get the Stage Transitions
            IList<IStageTransition> stageTransitions = stageDefinitionRepository.GetStageTransitionList(debtCounsellingCase.Key, (int)GenericKeyTypes.DebtCounselling2AM, declineTypes);

            //Get the Latest Stage Transition
            IStageTransition latestStageTransition = (from stageTransition in stageTransitions
                                                      orderby stageTransition.TransitionDate descending
                                                      select stageTransition).FirstOrDefault();

            if (latestStageTransition != null)
            {
                //get the reasons by the Stage Transition Key (Reason.StageTransitionKey)
                var reasons = reasonRepository.GetReasonsByStageTransitionKeys(new int[] { latestStageTransition.Key });

                //Check to see if the Court Order With Appeal reason exists
                var courtOrderWithAppealReason = (from reason in reasons
                                                  where reason.ReasonDefinition.ReasonDescription.Key == (int)ReasonDescriptions.CourtOrderWithAppeal
                                                  select reason).FirstOrDefault();

                if (courtOrderWithAppealReason != null)
                {
                    string msg = "The Debt Counselling Case has a Court Order with Appeal decline reason.";
                    AddMessage(msg, msg, Messages);
                    return 0;
                }
            }

            return 1;
        }
    }

    /// <summary>
    /// Debt Counselling Latest Transition Is Court Order With Appeal
    /// </summary>
    [RuleDBTag("DebtCounsellingLatestTransitionIsCourtOrderWithAppeal",
    "Debt Counselling Latest Transition Is Court Order With Appeal",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingLatestTransitionIsCourtOrderWithAppeal")]
    [RuleInfo]
    public class DebtCounsellingLatestTransitionIsCourtOrderWithAppeal : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellingCourtOrderWithAppealExist rule expects a Debt Counselling Case.");

            //Get the Debt Counselling Case
            IDebtCounselling debtCounsellingCase = Parameters[0] as IDebtCounselling;

            IStageDefinitionRepository stageDefinitionRepository = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
            IReasonRepository reasonRepository = RepositoryFactory.GetRepository<IReasonRepository>();

            //Get the Stage Transitions
            IList<IStageTransition> stageTransitions = stageDefinitionRepository.GetStageTransitionsByGenericKey(debtCounsellingCase.Key);

            //Get the Latest Stage Transition
            IStageTransition latestStageTransition = (from stageTransition in stageTransitions
                                                      orderby stageTransition.TransitionDate descending
                                                      select stageTransition).FirstOrDefault();

            if (latestStageTransition != null)
            {
                //get the reasons by the Stage Transition Key (Reason.StageTransitionKey)
                var reasons = reasonRepository.GetReasonsByStageTransitionKeys(new int[] { latestStageTransition.Key });

                //Check to see if the Court Order With Appeal reason exists
                var courtOrderWithAppealReason = (from reason in reasons
                                                  where reason.ReasonDefinition.ReasonDescription.Key == (int)ReasonDescriptions.CourtOrderWithAppeal
                                                  select reason).FirstOrDefault();

                if (courtOrderWithAppealReason != null)
                {
                    string msg = "Proposal has been declined with the Court Order with Appeal reason.";
                    AddMessage(msg, msg, Messages);
                    return 0;
                }
            }

            return 1;
        }
    }

    [RuleDBTag("LegalEntitiesUnderDebtCounsellingForAccount",
    "Checks if the legal entities against an account are under debt counselling",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.LegalEntitiesUnderDebtCounsellingForAccount")]
    [RuleInfo]
    public class LegalEntitiesUnderDebtCounsellingForAccount : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellingCourtOrderWithAppealExist rule expects a Debt Counselling Case.");

            IAccount account = null;
            IApplication application = null;
            string msg = string.Empty;

            IEventList<IRole> accountRoles = null;
            IReadOnlyEventList<IApplicationRole> offerRoles = null;

            //Account
            if (Parameters[0] is IAccount)
            {
                account = Parameters[0] as IAccount;
                accountRoles = new EventList<IRole>(account.Roles.Where(x => x.GeneralStatus.Key == (int)GeneralStatuses.Active));
            }

            //Application
            if (Parameters[0] is IApplication)
            {
                application = Parameters[0] as IApplication;
                offerRoles = new ReadOnlyEventList<IApplicationRole>(application.ApplicationRoles.Where(x => x.GeneralStatus.Key == (int)GeneralStatuses.Active));
            }

            //Account Roles
            if (accountRoles != null)
            {
                foreach (IRole role in accountRoles)
                {
                    if (role.LegalEntity.DebtCounsellingCases != null)
                    {
                        foreach (IDebtCounselling dc in role.LegalEntity.DebtCounsellingCases)
                        {
                            msg = string.Format("{0} ({1}) on account ({2}) is under debt counselling.", role.LegalEntity.DisplayName, role.RoleType.Description, dc.Account.Key);
                            AddMessage(msg, msg, Messages);
                        }
                    }
                }
            }

            //Offer Roles
            else if (offerRoles != null)
            {
                foreach (IApplicationRole role in offerRoles)
                {
                    if (role.LegalEntity.DebtCounsellingCases != null)
                    {
                        foreach (IDebtCounselling dc in role.LegalEntity.DebtCounsellingCases)
                        {
                            msg = string.Format("{0} ({1}) on application ({2}) is under debt counselling.", role.LegalEntity.DisplayName, role.ApplicationRoleType.Description, application.Key);
                            AddMessage(msg, msg, Messages);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(msg))
                return 1;

            return 0;
        }
    }

    [RuleDBTag("LegalEntityUnderDebtCounselling",
    "Checks if a legal entity is under debt counselling.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.LegalEntityUnderDebtCounselling")]
    [RuleInfo]
    public class LegalEntityUnderDebtCounselling : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The LegalEntityUnderDebtCounselling rule expects a Legal Entity.");

            var legalEntity = Parameters[0] as ILegalEntity;

            if (legalEntity.DebtCounsellingCases == null)
            {
                return 1;
            }

            var dcAccounts = legalEntity.DebtCounsellingCases.Select(x => x.Account.Key.ToString());

            if (dcAccounts != null)
            {
                var msg = string.Format("{0} is under Debt Counselling on Account No's: {1}.", legalEntity.DisplayName, String.Join(", ", dcAccounts));
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    [RuleDBTag("DebtCounselling10DayTerminationReminderHearingAppearanceTypesCheck",
    "The 10 days timer must NOT fire if these HearingAppearanceTypes exist - CourtApplication, TribunalCourtApplication, OrderGranted, TribunalOrderGranted",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounselling10DayTerminationReminderHearingAppearanceTypesCheck")]
    [RuleInfo]
    public class DebtCounselling10DayTerminationReminderHearingAppearanceTypesCheck : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounselling10DayTerminationReminderCheck rule expects a Domain object to be passed.");

            IDebtCounselling debtCounsellingCase = Parameters[0] as IDebtCounselling;

            List<int> HearingAppearanceTypeKeys = new List<int>();
            HearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.CourtApplication);
            HearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.TribunalCourtApplication);
            HearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.OrderGranted);
            HearingAppearanceTypeKeys.Add((int)HearingAppearanceTypes.TribunalOrderGranted);

            //This query will get hearing details off a debt counselling case where
            //HearingType is of Court or Tribunal and the HearingAppearanceType is contained in the list above.
            var query = from hearingDetail in debtCounsellingCase.HearingDetails
                        where (hearingDetail.HearingType.Key == (int)HearingTypes.Court ||
                              hearingDetail.HearingType.Key == (int)HearingTypes.Tribunal) &&
                              hearingDetail.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                              HearingAppearanceTypeKeys.FindAll((hearingAppearanceTypeKey) =>
                              {
                                  return hearingAppearanceTypeKey == hearingDetail.HearingAppearanceType.Key;
                              }).Count > 0
                        select hearingDetail;

            if (query.FirstOrDefault() != null)
            {
                string msg = "Court Hearing Appearance Types Exist.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    #endregion Debt Counselling

    #region Hearing Detail

    [RuleDBTag("HearingDetailCommentMandatory",
   "Ensure that the Hearing Detail Comment is entered.",
   "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.DebtCounselling.HearingDetailCommentMandatory")]
    [RuleInfo]
    public class HearingDetailCommentMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The HearingDetailCommentMandatory rule expects a Domain object to be passed.");

            IDebtCounselling debtcounselling = (IDebtCounselling)Parameters[0];
            if (debtcounselling == null)
                throw new ArgumentException("The HearingDetailCommentMandatory rule expects the following objects to be passed: IDebtCounselling.");

            //Sort the Hearing Details by key in descending order
            List<IHearingDetail> sortedlstHearingDetails = new List<IHearingDetail>(debtcounselling.GetActiveHearingDetails);
            sortedlstHearingDetails.Sort(delegate(IHearingDetail c1, IHearingDetail c2) { return c2.Key.CompareTo(c1.Key); });

            if (sortedlstHearingDetails.Count == 0 || String.IsNullOrEmpty(sortedlstHearingDetails[0].Comment))
            {
                string msg = "The Hearing Detail Comment is Mandatory";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }

    #endregion Hearing Detail

    #region DebtCounsellors

    [RuleDBTag("DebtCounsellorActiveAddressRequired",
   "Ensure that an active Address exists for a Debt Counsellor or their company.",
   "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellorActiveAddressRequired")]
    [RuleInfo]
    public class DebtCounsellorActiveAddressRequired : BusinessRuleBase
    {
        public DebtCounsellorActiveAddressRequired(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The DebtCounsellorActiveAddressRequired rule expects a parameter to be passed.");

            IDebtCounselling debtCounselling = (IDebtCounselling)Parameters[0];

            var sqlQuery = UIStatementRepository.GetStatement("LegalEntity", "GetDebtCounsellorDetails");
            var parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@DebtCounsellingKey", debtCounselling.Key));


            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(sqlQuery, typeof(Account_DAO), parameters);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows[0]["DebtCounsellorAddress"] != DBNull.Value)
            {
                return 0;
            }

            var errorMessage = "Every Debt Counsellor, Designation or Company requires an active address.";
            AddMessage(errorMessage, errorMessage, Messages);
            return 1;
        }
    }

    #endregion
}