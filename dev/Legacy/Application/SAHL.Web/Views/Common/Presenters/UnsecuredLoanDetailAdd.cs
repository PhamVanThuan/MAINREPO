using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.MobileControls;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
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
    public class UnsecuredLoanDetailAdd : SAHLCommonBasePresenter<ILoanDetail>
    {
        private IAccountRepository _accRepository;
        private IDetailType _debitOrderSuspended;
        private int _accountKey = 0;

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public UnsecuredLoanDetailAdd(ILoanDetail view, SAHLCommonBaseController controller)
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

            _view.UpdateMode = false;
            _view.DeleteMode = false;
            _view.CancellationTypeEnabled = false;

            _accRepository = RepositoryFactory.GetRepository<IAccountRepository>();

            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (cboNode != null)
            {
                switch (cboNode.GenericKeyTypeKey)
                {
                    case (int)SAHL.Common.Globals.GenericKeyTypes.ParentAccount:
                        _accountKey = cboNode.GenericKey;
                        break;
                    default:
                        break;
                }

                if (_accountKey > 0)
                    SetupDisplay(_accountKey);
            }
        }

        #region Private Methods

        private void SetupDisplay(int accountKey)
        {
            _view.ShowButtons = true;
            _view.SubmitButtonText = "Add";
            _view.ShowLabels = false;
            _view.DetailGridPostBackType = GridPostBackType.None;

            IReadOnlyEventList<IDetail> _lstDetail = _accRepository.GetDetailByAccountKey(accountKey);

            if (_lstDetail != null && _lstDetail.Count > 0)
                _view.BindDetailGrid(_lstDetail);

            ILookupRepository _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
            _debitOrderSuspended = _lookupRepository.DetailTypes.Where(x => x.Key == (int)DetailTypes.DebitOrderSuspended).Single<IDetailType>();

            if (_debitOrderSuspended != null)
            {
                IList<IDetailClass> lstDetailClasses = _lookupRepository.DetailClasses.Where(x => x.Key == _debitOrderSuspended.DetailClass.Key).ToList();

                if (lstDetailClasses != null && lstDetailClasses.Count == 1)
                {
                    _view.BindDetailClassDropDown(new EventList<IDetailClass>(lstDetailClasses));

                    List<IDetailType> detailTypeList = new List<IDetailType>(1);
                    detailTypeList.Add(_debitOrderSuspended);
                    _view.BindDetailTypeDropDown(detailTypeList);
                }
            }

            _view.ShowLabels = false;
            _view.SubmitButtonEnabled = true;
        }

        #endregion Private Methods

        #region Events Handlers

        private void _view_OnCancelClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("UnsecuredLoanDetailDisplay");
        }

        private void _view_OnSubmitClicked(object sender, KeyChangedEventArgs e)
        {
            IAccount account = null;
            if (_accountKey > 0)
                account = _accRepository.GetAccountByKey(_accountKey);

            if (account != null)
            {
                //validate input
                if (_view.UpdatedDetailClass == -1)
                    _view.Messages.Add(new Error("Detail Class must be selected.", "Detail Class must be selected."));

                if (_view.UpdatedDetailType == -1)
                    _view.Messages.Add(new Error("Detail Type must be selected.", "Detail Type must be selected."));

                if (!_view.UpdatedDetailDate.HasValue)
                    _view.Messages.Add(new Error("Detail Date must be entered.", "Detail Date must be entered."));

                if (_view.IsValid && _debitOrderSuspended != null)
                {
                    IDetail detail = _accRepository.CreateEmptyDetail();

                    detail.DetailType = _debitOrderSuspended;
                    detail.DetailDate = _view.UpdatedDetailDate.Value;
                    detail.Amount = _view.UpdatedDetailAmount;
                    detail.Description = _view.UpdatedDetailDescription;
                    detail.Account = account;
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
            }
        }

        #endregion Events Handlers
    }
}