using System;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
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
    public class UnsecuredLoanDetailUpdate : SAHLCommonBasePresenter<ILoanDetail>
    {
        private IAccountRepository _accRepository;
        private IReadOnlyEventList<IDetail> _lstDetail;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public UnsecuredLoanDetailUpdate(ILoanDetail view, SAHLCommonBaseController controller)
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

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelClicked);
            _view.OnSubmitButtonClicked += new KeyChangedEventHandler(_view_OnSubmitClicked);
            _view.OnGridSelectedIndexChanged += new KeyChangedEventHandler(_view_OnGridIndexChanged);

            _view.UpdateMode = true;
            _view.DeleteMode = false;
            _view.CancellationTypeEnabled = false;

            _accRepository = RepositoryFactory.GetRepository<IAccountRepository>();

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
            _view.ShowButtons = true;
            _view.SubmitButtonText = "Update";
            _view.ShowLabels = false;
            _view.DetailGridPostBackType = GridPostBackType.SingleClick;

            _lstDetail = _accRepository.GetDetailByAccountKey(accountKey);

            if (_lstDetail != null && _lstDetail.Count > 0)
            {
                _view.BindDetailGrid(_lstDetail);

                if (!_view.IsPostBack)
                    _view.BindData(_lstDetail[0]);

                if (_lstDetail[0].DetailType.AllowUpdateDelete == true && _lstDetail[0].DetailType.AllowScreen == true)
                {
                    _view.SubmitButtonEnabled = true;
                }
                else
                {
                    _view.SubmitButtonEnabled = false;
                }
            }
            else
            {
                _view.SubmitButtonEnabled = false;
            }
        }

        #endregion

        #region Events Handlers

        void _view_OnGridIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (e != null)
            {
                if (_lstDetail != null && _lstDetail.Count > Convert.ToInt32(e.Key))
                {
                    int selectedIndex = Convert.ToInt32(e.Key);
                    _view.BindData(_lstDetail[selectedIndex]);

                    if (_lstDetail[selectedIndex].DetailType.AllowUpdate == true && _lstDetail[selectedIndex].DetailType.AllowScreen == true)
                    {
                        _view.ShowLabels = false;
                        _view.SubmitButtonEnabled = true;
                    }
                    else
                    {
                        _view.ShowLabels = true;
                        _view.SubmitButtonEnabled = false;
                    }
                }
            }
        }

        void _view_OnCancelClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("UnsecuredLoanDetailDisplay");
        }

        void _view_OnSubmitClicked(object sender, KeyChangedEventArgs e)
        {
            if (e != null )
            {
                int selectedIndex = Convert.ToInt32(e.Key);

                if (!_view.UpdatedDetailDate.HasValue)
                    _view.Messages.Add(new Error("Detail Date must be entered.", "Detail Date must be entered."));

                if (_view.IsValid)
                {
                    IDetail detail = _lstDetail[selectedIndex];

                    detail.DetailDate = _view.UpdatedDetailDate.Value;
                    detail.Amount = _view.UpdatedDetailAmount;
                    detail.Description = _view.UpdatedDetailDescription;
                    detail.UserID = _view.CurrentPrincipal.Identity.Name;
                    detail.ChangeDate = DateTime.Now;

                    using (TransactionScope txn = new TransactionScope())
                    {
                        try
                        {
                            _accRepository.SaveDetail(detail);
                            txn.VoteCommit();
                        }
                        catch (Exception)
                        {
                            txn.VoteRollBack();
                            if (_view.IsValid)
                                throw;
                        }
                    }

                    if (_view.IsValid)
                        _view.Navigator.Navigate("UnsecuredLoanDetailDisplay");
                }
                else
                {
                    if (_lstDetail[selectedIndex].DetailType.AllowUpdate == true && _lstDetail[selectedIndex].DetailType.AllowScreen == true)
                    {
                        _view.ShowLabels = false;
                        _view.SubmitButtonEnabled = true;
                        _view.BindData(_lstDetail[selectedIndex]);
                    }
                    else
                    {
                        _view.ShowLabels = true;
                        _view.SubmitButtonEnabled = false;
                        _view.BindData(_lstDetail[selectedIndex]);
                    }
                }
            }
        }

        #endregion
    }
}
