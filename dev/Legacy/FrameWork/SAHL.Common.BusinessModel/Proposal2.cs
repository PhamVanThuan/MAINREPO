using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Utils;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Proposal_DAO
    /// </summary>
    public partial class Proposal : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Proposal_DAO>, IProposal
    {
        private IMemoRepository _memoRepository;
        private IMemo _memo;
        private int _totalTerm;
        IReasonRepository _ReasonRepository;
        IStageDefinitionRepository _StageDefinitionRepository;

        public IMemoRepository MemoRepository
        {
            get
            {
                if (_memoRepository == null)
                {
                    _memoRepository = RepositoryFactory.GetRepository<IMemoRepository>();
                }
                return _memoRepository;
            }
        }

        public IReasonRepository ReasonRepository
        {
            get
            {
                if (_ReasonRepository == null)
                {
                    _ReasonRepository = RepositoryFactory.GetRepository<IReasonRepository>();
                }
                return _ReasonRepository;
            }
        }

        public IStageDefinitionRepository StageDefinitionRepository
        {
            get
            {
                if (_StageDefinitionRepository == null)
                {
                    _StageDefinitionRepository = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
                }
                return _StageDefinitionRepository;
            }
        }

        public int TotalTerm
        {
            get
            {
                //Calculate the total term
                //Sort the Proposal Items
                List<IProposalItem> sortedProposalItems = new List<IProposalItem>(this.ProposalItems);
                sortedProposalItems.Sort(delegate(IProposalItem c1, IProposalItem c2) { return c2.EndDate.CompareTo(c1.EndDate); });

                DateTime endDate = DateTime.MinValue;
                if (sortedProposalItems.Count > 0)
                {
                    endDate = sortedProposalItems[0].EndDate;
                }

                IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
                IControl ctrl = ctrlRepo.GetControlByDescription("MaxProposalTerm");

                _totalTerm = endDate.MonthDifference(this.DebtCounselling.Account.OpenDate.Value, 1);

                return _totalTerm;
            }
        }

        public IMemo Memo
        {
            get
            {
                IEventList<IMemo> memos = MemoRepository.GetMemoByGenericKey(this.Key, (int)GenericKeyTypes.Proposal);
                if (memos.Count > 0)
                {
                    _memo = memos[0];
                }
                if (_memo == null)
                {
                    _memo = MemoRepository.CreateMemo();
                }
                return _memo;
            }
            set
            {
                _memo = value;
            }
        }

        public DateTime? AcceptedDate
        {
            get
            {
                IList<IStageTransition> list = StageDefinitionRepository.GetStageTransitionList(this.DebtCounselling.Key, (int)GenericKeyTypes.DebtCounselling2AM, new List<int> { (int)StageDefinitionStageDefinitionGroups.DebtCounsellingProposalAccepted });

                if (list == null || list.Count() <= 0)
                    return null;

                IStageTransition st = list.OrderByDescending(x => x.TransitionDate).FirstOrDefault();
                return st.TransitionDate;
            }
        }

        public IADUser AcceptedUser
        {
            get
            {
                IList<IStageTransition> list = StageDefinitionRepository.GetStageTransitionList(this.DebtCounselling.Key, (int)GenericKeyTypes.DebtCounselling2AM, new List<int> { (int)StageDefinitionStageDefinitionGroups.DebtCounsellingProposalAccepted });

                if (list == null || list.Count() <= 0)
                    return null;

                IStageTransition st = list.OrderByDescending(x => x.TransitionDate).FirstOrDefault();
                return st.ADUser;
            }
        }

        public IReason AcceptedReason
        {
            get
            {
                if (this.Accepted.HasValue && this.Accepted.Value == true)
                    return ReasonRepository.GetLatestReasonByGenericKeyAndReasonTypeKey(this.Key, (int)ReasonTypes.ProposalAccepted);
                return null;
            }
        }

        public IReason ActiveReason
        {
            get
            {
                return ReasonRepository.GetLatestReasonByGenericKeyAndReasonTypeKey(this.Key, (int)ReasonTypes.CounterProposals);
            }
        }

        public double? AcceptedRate
        {
            get
            {
                IProposalItem item = this.ProposalItems.OrderBy(x => x.StartDate).Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now).FirstOrDefault();

                if (item == null)
                    item = this.ProposalItems.OrderBy(x => x.StartDate).FirstOrDefault();

                if (item != null)
                    return item.InterestRate;

                return null;
            }
        }

        private void ProposalItems_AfterRemove(ICancelDomainArgs args, object Item)
        {
            throw new NotImplementedException();
        }

        private void ProposalItems_AfterAdd(ICancelDomainArgs args, object Item)
        {
            throw new NotImplementedException();
        }

        private void ProposalItems_BeforeRemove(ICancelDomainArgs args, object Item)
        {
            throw new NotImplementedException();
        }

        private void ProposalItems_BeforeAdd(ICancelDomainArgs args, object Item)
        {
            throw new NotImplementedException();
        }

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("CounterProposalReasonMemo");
            Rules.Add("DebtCounsellingDuplicateDraftProposal");
        }
    }
}