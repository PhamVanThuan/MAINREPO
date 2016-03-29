using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel;
using SAHL.Common.UI;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using System.Collections;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Controls;


namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class DisbursementHistoryBase : SAHLCommonBasePresenter<IDisbursementHistory>
    {
        private IList<IDisbursementStatus> _historyStatuses;
        private IAccount _account;
        private int _genericKey;
        private ILookupRepository _lookupRepo;

        public int GenericKey
        {
            get { return _genericKey; }
            set { _genericKey = value; }
        }

        public ILookupRepository LookupRepo
        {
            get { return _lookupRepo; }
            set { _lookupRepo = value; }
        }

        public IAccount Account
        {
            get { return _account; }
            set { _account = value; }
        }
	


        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public DisbursementHistoryBase(IDisbursementHistory view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (cboNode != null)
                _genericKey = cboNode == null ? -1 : cboNode.GenericKey;

            _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            BindDisbursementStatuses();
            if (!_view.IsPostBack)
                _view.SelectedStatus = Convert.ToInt32(DisbursementStatuses.Disbursed);

            BindGridData();
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindGridData()
        {
            if (_account == null)
                return;

            IDisbursementRepository disbursementRepo = RepositoryFactory.GetRepository<IDisbursementRepository>();
            IReadOnlyEventList<IDisbursement> disbursementList = null;
            if (!_view.IsPostBack)
                disbursementList = disbursementRepo.GetDisbursmentsByParentAccountKeyAndStatus(_account.Key, Convert.ToInt32(DisbursementStatuses.Disbursed));
            else
                disbursementList = disbursementRepo.GetDisbursmentsByParentAccountKeyAndStatus(_account.Key, _view.SelectedStatus);

            _view.BindGrid(disbursementList);

            double totalDisbursements = 0;
            if (disbursementList != null)
            {
                for (int i = 0; i < disbursementList.Count; i++)
                {
                    if (disbursementList[i].Amount.HasValue)
                        totalDisbursements += disbursementList[i].Amount.Value;
                }
            }
            _view.TotalDisbursementsValue = totalDisbursements;
        }


        /// <summary>
        /// 
        /// </summary>
        private void BindDisbursementStatuses()
        {

            IEventList<IDisbursementStatus> disbursementStatusList = _lookupRepo.DisbursementStatuses;
            _historyStatuses = new List<IDisbursementStatus>();
            for (int i = 0; i < disbursementStatusList.Count; i++)
            {
                if (disbursementStatusList[i].Key == Convert.ToInt32(DisbursementStatuses.Disbursed) ||
                    disbursementStatusList[i].Key == Convert.ToInt32(DisbursementStatuses.ReadyForDisbursement) ||
                    disbursementStatusList[i].Key == Convert.ToInt32(DisbursementStatuses.RolledBack))
                {
                    _historyStatuses.Add(disbursementStatusList[i]);
                }
            }

            _view.BindDisbursementStatuses(_historyStatuses);
        }
    }
}
