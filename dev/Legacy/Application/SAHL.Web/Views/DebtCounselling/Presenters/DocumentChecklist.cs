using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Service.Interfaces;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    public class DocumentChecklist : SAHLCommonBasePresenter<IDocumentChecklist>
    {
        public CBOMenuNode _node;
        public InstanceNode _instanceNode;

        private int _genericKey;
        public int GenericKey
        {
            get { return _genericKey; }
            set { _genericKey = value; }
        }

        private int _genericKeyTypeKey;
        public int GenericKeyTypeKey
        {
            get { return _genericKeyTypeKey; }
            set { _genericKeyTypeKey = value; }
        }

        private ILookupRepository _lookupRepo;
        public ILookupRepository LookupRepo
        {
            get
            {
                if (_lookupRepo == null)
                    _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                return _lookupRepo;
            }
        }

        /// <summary>
        /// Constructor for DocumentChecklist
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public DocumentChecklist(IDocumentChecklist view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
            // Get the Node   
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            if (_node is InstanceNode)
            {
                InstanceNode instanceNode = _node as InstanceNode;
                _genericKey = instanceNode.GenericKey; // this will be the debtcounsellingkey
                _genericKeyTypeKey = instanceNode.GenericKeyTypeKey;
            }
            else
            {
                _genericKey = _node.GenericKey;
                _genericKeyTypeKey = _node.GenericKeyTypeKey;
            }

            SetupDateList();
        }

        private void SetupDateList()
        {
            #region StageDefinitionDates

            List<BindableDCItem> dates = new List<BindableDCItem>();

            List<int> sdsdg = new List<int>();
            sdsdg.Add((int)StageDefinitionStageDefinitionGroups.Received17pt1);
            sdsdg.Add((int)StageDefinitionStageDefinitionGroups.Received17pt2);
            sdsdg.Add((int)StageDefinitionStageDefinitionGroups.Received17pt3);

            IList<IStageTransition> listST = SDRepo.GetStageTransitionList(_genericKey, _genericKeyTypeKey, sdsdg);


            DateTime? dt171 = null;
            string comments171 = String.Empty;
            Int64 key171 = 0;
            DateTime? dt172 = null;
            string comments172 = String.Empty;
            Int64 key172 = 0;
            DateTime? dt173 = null;
            string comments173 = String.Empty;
            Int64 key173 = 0;

            foreach (IStageTransition st in listST)
            {
                switch (st.StageDefinitionStageDefinitionGroup.Key)
                {
                    case (int)StageDefinitionStageDefinitionGroups.Received17pt1:
                        if (!dt171.HasValue || st.Key > key171)
                        {
                            dt171 = st.EndTransitionDate;
                            comments171 = st.Comments;
                            key171 = st.Key;
                        }
                        break;
                    case (int)StageDefinitionStageDefinitionGroups.Received17pt2:
                        if (!dt172.HasValue || st.Key > key172)
                        {
                            dt172 = st.EndTransitionDate;
                            comments172 = st.Comments;
                            key172 = st.Key;
                        }
                        break;
                    case (int)StageDefinitionStageDefinitionGroups.Received17pt3:
                        if (!dt173.HasValue || st.Key > key173)
                        {
                            dt173 = st.EndTransitionDate;
                            comments173 = st.Comments;
                            key173 = st.Key;
                        }
                        break;
                    default:
                        break;
                }
            }

            dates.Add(new BindableDCItem("17.1", dt171, comments171, (int)DCItemType.dte171, true));
            dates.Add(new BindableDCItem("17.2", dt172, comments172, (int)DCItemType.dte172, true));
            dates.Add(new BindableDCItem("17.3", dt173, comments173, (int)DCItemType.dte173, false));

            #endregion

            if (_genericKeyTypeKey == (int)GenericKeyTypes.DebtCounselling2AM)
            {
                IDebtCounselling dc = DCRepo.GetDebtCounsellingByKey(_genericKey);
                if (dc != null)
                {
                    #region OtherDates

                    DateTime? dt = null;
                    string comments = String.Empty;

                    //60 Days
                    dt = DCRepo.Get60DaysDate(dc.Key);

                    dates.Add(new BindableDCItem("60 Days", dt, comments, (int)DCItemType.dte60Days, false));

                    //HearingDetails
                    dt = null;
                    comments = String.Empty;

                    if (dc.GetActiveHearingDetails != null && dc.GetActiveHearingDetails.Count > 0)
                    {
                        dt = (from hearingDetail in dc.GetActiveHearingDetails
                              orderby hearingDetail.Key descending
                              select hearingDetail.HearingDate).FirstOrDefault();
                    }

                    dates.Add(new BindableDCItem("Next Hearing", dt, comments, (int)DCItemType.dteHearing, false));

                    //ReviewDate
                    if (dc.AcceptedActiveProposal != null && dc.AcceptedActiveProposal.ReviewDate.HasValue)
                        dates.Add(new BindableDCItem("Review", dc.AcceptedActiveProposal.ReviewDate.Value, "", (int)DCItemType.dteReview, false));
                    else
                        dates.Add(new BindableDCItem("Review", null, "", (int)DCItemType.dteReview, false));

                    #endregion
                }
            }

            _view.BindDateGrid(dates);
        }

        public void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException("TODO");
        }

        public void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            TransactionScope txn = new TransactionScope();
            try
            {
                StageDefinitionStageDefinitionGroups sdsdg = StageDefinitionStageDefinitionGroups.Received17pt2;

                switch (_view.ItemType)
                {
                    case DCItemType.dte171:
                        sdsdg = StageDefinitionStageDefinitionGroups.Received17pt1;
                        break;
                    case DCItemType.dte172:
                        sdsdg = StageDefinitionStageDefinitionGroups.Received17pt2;
                        break;
                    default:
                        throw new NotImplementedException("Only 17.1 and 17.2 dates are catered for");
                        break;
                }

                SDRepo.SaveStageTransition(_genericKey, sdsdg, _view.NewDate.Value, _view.Comment, _view.CurrentPrincipal.Identity.Name);

                txn.VoteCommit();
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                txn.Dispose();
            }

            SetupDateList();
        }

        private IStageDefinitionRepository _sdRepo;
        public IStageDefinitionRepository SDRepo
        {
            get
            {
                if (_sdRepo == null)
                    _sdRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();

                return _sdRepo;
            }
        }

        private IDebtCounsellingRepository _dcRepo;
        public IDebtCounsellingRepository DCRepo
        {
            get
            {
                if (_dcRepo == null)
                    _dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                return _dcRepo;
            }
        }
    }
}