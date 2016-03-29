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
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    public class CourtDetailsBase : SAHLCommonBasePresenter<ICourtDetails>
    {
        public CBOMenuNode _node;
        public InstanceNode _instanceNode;
        public List<IHearingDetail> _hearingDetails;
        private IDebtCounselling _debtCounselling;

        public IDebtCounselling DebtCounselling 
        {
            get { return _debtCounselling; }
            set { _debtCounselling = value; }
        }

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

        private IDebtCounsellingRepository _debtCounsellingRepo;
        public IDebtCounsellingRepository DebtCounsellingRepo
        {
            get
            {
                if (_debtCounsellingRepo == null)
                    _debtCounsellingRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                return _debtCounsellingRepo;
            }
        }

        /// <summary>
        /// Constructor for CourtDetails
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CourtDetailsBase(ICourtDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.OnCommentClick += new KeyChangedEventHandler(_view_OnCommentClick);

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

            // get the debtcounsellingrecord
            _debtCounselling = DebtCounsellingRepo.GetDebtCounsellingByKey(_genericKey);

            _hearingDetails = _debtCounselling.HearingDetails.ToList();
            // bind the hearing details
            _view.BindHearingDetails(_debtCounselling.HearingDetails);
        }


        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;
        }

        protected void _view_OnCommentClick(object sender, KeyChangedEventArgs e)
        {
            if (sender == null || string.IsNullOrEmpty(sender.ToString()))
                return;

            string comments = e.Key == null ? null : e.Key.ToString();

            int hearingDetailKey;

            if (int.TryParse(sender.ToString(), out hearingDetailKey))
            {
                _view.HearingDetailKey = hearingDetailKey;
                _view.CommentEditor = comments;
            }
        }
    }
}