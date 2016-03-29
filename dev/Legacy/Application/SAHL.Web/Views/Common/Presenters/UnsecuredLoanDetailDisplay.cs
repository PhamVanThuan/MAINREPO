using System;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class UnsecuredLoanDetailDisplay : SAHLCommonBasePresenter<ILoanDetail>
    {
        private IReadOnlyEventList<IDetail> _lstDetail;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public UnsecuredLoanDetailDisplay(ILoanDetail view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            _view.OnCancelButtonClicked += null;
            _view.OnSubmitButtonClicked += null;
            _view.OnGridSelectedIndexChanged += new KeyChangedEventHandler(_view_OnGridIndexChanged); ;

            _view.UpdateMode = false;
            _view.DeleteMode = false;
            _view.CancellationTypeEnabled = false;

            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (cboNode != null)
            {
                int accountKey = 0;
                switch (cboNode.GenericKeyTypeKey)
                {
                    case (int)SAHL.Common.Globals.GenericKeyTypes.ParentAccount:
                        accountKey = cboNode.GenericKey;
                        break;
                    default:
                        break;
                }

                if (accountKey > 0)
                    SetupDisplay(accountKey);
            }
        }

        #region Private Methods

        private void SetupDisplay(int accountKey)
        {
            _view.ShowButtons = false;
            _view.ShowLabels = true;
            _view.DetailGridPostBackType = GridPostBackType.SingleClick;

            IAccountRepository _accRepository = RepositoryFactory.GetRepository<IAccountRepository>();
            _lstDetail = _accRepository.GetDetailByAccountKey(accountKey);

            if (_lstDetail != null && _lstDetail.Count > 0)
            {
                _view.BindDetailGrid(_lstDetail);

                if (!_view.IsPostBack)
                    _view.BindData(_lstDetail[0]);
            }
        }

        #endregion

        #region Events Handlers

        void _view_OnGridIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (e != null)
            {
                if (_lstDetail != null && _lstDetail.Count > Convert.ToInt32(e.Key))
                    _view.BindData(_lstDetail[Convert.ToInt32(e.Key)]);
            }
        }

        #endregion
    }
}
