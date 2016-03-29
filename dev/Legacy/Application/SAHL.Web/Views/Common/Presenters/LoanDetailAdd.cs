using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
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
    public class LoanDetailAdd : SAHLCommonBasePresenter<ILoanDetail>
    {
        private IReadOnlyEventList<IDetail> _lstDetail;
        private ILookupRepository _lookupRepository;
        private CBOMenuNode _cboNode;
        IAccountRepository _accRepository;

        private string _cachedDataDetailClassKey = "SelectedDetailClass";
        private string _cachedDataDetailTypeKey = "SelectedDetailType";

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LoanDetailAdd(ILoanDetail view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;

            if (PrivateCacheData.ContainsKey(_cachedDataDetailTypeKey) &&
               Convert.ToInt32(PrivateCacheData[_cachedDataDetailTypeKey]) == Convert.ToInt32(SAHL.Common.Globals.DetailTypes.UnderCancellation)) // detailtypekey 11
                _view.CancellationTypeEnabled = true;
            else
                _view.CancellationTypeEnabled = false;
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelClicked);
            _view.OnSubmitButtonClicked += new KeyChangedEventHandler(_view_OnSubmitClicked);
            _view.OnGridSelectedIndexChanged += new KeyChangedEventHandler(_view_OnGridIndexChanged);

            _accRepository = RepositoryFactory.GetRepository<IAccountRepository>();
            _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

            _cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_cboNode != null)
            {
                SetupDisplay(Convert.ToInt32(_cboNode.GenericKey));
            }
        }

        #region Private Methods

        private void BindDetailTypeByClass(int detailClassKey)
        {
            List<IDetailType> detailTypeList = new List<IDetailType>();
            IEventList<IDetailType> lstEventDetailTypes = _lookupRepository.DetailTypes;
            List<IDetailType> lstDetailTypes = new List<IDetailType>();
            foreach (IDetailType t in lstEventDetailTypes)
            {
                lstDetailTypes.Add(t);
            }
            lstDetailTypes.Sort();
            for (int detailTypeIndex = 0; detailTypeIndex < lstDetailTypes.Count; detailTypeIndex++)
            {
                if (lstDetailTypes[detailTypeIndex].DetailClass.Key == detailClassKey &&
                    lstDetailTypes[detailTypeIndex].GeneralStatus.Key == Convert.ToInt32(GeneralStatuses.Active) &&
                    lstDetailTypes[detailTypeIndex].AllowScreen == true)
                {
                    detailTypeList.Add(lstDetailTypes[detailTypeIndex]);
                }
            }

            _view.BindDetailTypeDropDown(detailTypeList);
        }

        private void SetupDisplay(int accountKey)
        {
            _lstDetail = _accRepository.GetDetailByAccountKey(accountKey);

            _view.ShowButtons = true;
            _view.SubmitButtonText = "Add";
            _view.DetailGridPostBackType = GridPostBackType.None;

            _view.BindDetailGrid(_lstDetail);
            _view.BindDetailClassDropDown(_lookupRepository.DetailClasses);
            _view.BindDetailCancellationTypeDropDown(_lookupRepository.CancellationTypes);

            if (_view.UpdatedDetailClass != -1)
            {
                BindDetailTypeByClass(_view.UpdatedDetailClass);
                if (PrivateCacheData.ContainsKey(_cachedDataDetailClassKey) &&
                    Convert.ToInt32(PrivateCacheData[_cachedDataDetailClassKey]) != _view.UpdatedDetailClass)
                {
                    PrivateCacheData[_cachedDataDetailTypeKey] = -1;
                }
                else
                {
                    if (_view.UpdatedDetailType != -1 &&
                        PrivateCacheData.ContainsKey(_cachedDataDetailTypeKey) &&
                        Convert.ToInt32(PrivateCacheData[_cachedDataDetailTypeKey]) != _view.UpdatedDetailType)
                    {
                        PrivateCacheData[_cachedDataDetailTypeKey] = _view.UpdatedDetailType;
                    }
                }
                PrivateCacheData[_cachedDataDetailClassKey] = _view.UpdatedDetailClass;
                if (!PrivateCacheData.ContainsKey(_cachedDataDetailTypeKey))
                {
                    PrivateCacheData[_cachedDataDetailTypeKey] = -1;
                }
            }

            _view.ShowLabels = false;
            _view.SubmitButtonEnabled = true;
        }

        #endregion Private Methods

        #region Events Handlers

        private void _view_OnGridIndexChanged(object sender, KeyChangedEventArgs e)
        {
        }

        private void _view_OnCancelClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("LoanDetailDisplay");
        }

        private void _view_OnSubmitClicked(object sender, KeyChangedEventArgs e)
        {
            IManualDebitOrderRepository manualDebitOrderRepo = RepositoryFactory.GetRepository<IManualDebitOrderRepository>();
            IAccount account = _accRepository.GetAccountByKey(Convert.ToInt32(_cboNode.GenericKey));
            if (account != null)
            {
                //validate input
                if (_view.UpdatedDetailClass == -1)
                    _view.Messages.Add(new Error("Detail Class must be selected.", "Detail Class must be selected."));
                if (_view.UpdatedDetailType == -1)
                    _view.Messages.Add(new Error("Detail Type must be selected.", "Detail Type must be selected."));
                if (!_view.UpdatedDetailDate.HasValue)
                    _view.Messages.Add(new Error("Detail Date must be entered.", "Detail Date must be entered."));

                if (_view.UpdatedDetailType == (int)SAHL.Common.Globals.DetailTypes.UnderCancellation && _view.UpdatedCancellationType == -1)
                    _view.Messages.Add(new Error("Cancellation Type must be selected.", "Cancellation Type must be selected."));

                if (_view.UpdatedDetailAmount > 999999999.99)
                    _view.Messages.Add(new Error("Amount cannot be greater than R999,999,999.99", "Amount cannot be greater than R999,999,999.99"));

                if (_view.IsValid)
                {
                    IDetail addRec = _accRepository.CreateEmptyDetail();
                    addRec.DetailType = _lookupRepository.DetailTypes.ObjectDictionary[_view.UpdatedDetailType.ToString()];
                    addRec.DetailDate = _view.UpdatedDetailDate.Value;
                    addRec.Description = _view.UpdatedDetailDescription;
                    addRec.Amount = _view.UpdatedDetailAmount;
                    addRec.Account = account;
                    if (_view.UpdatedCancellationType != -1)
                        addRec.LinkID = _view.UpdatedCancellationType;
                    addRec.UserID = _view.CurrentPrincipal.Identity.Name;
                    addRec.ChangeDate = DateTime.Now;

                    IRuleService svc = ServiceFactory.GetService<IRuleService>();

                    if (addRec.DetailType.Key == (int)SAHL.Common.Globals.DetailTypes.LoanClosed)
                    {
                        svc.ExecuteRule(_view.Messages, "DetailCannotBeClosedWithCurrentBalanceNotZero", addRec);
                        svc.ExecuteRule(_view.Messages, "DetailNonPerformingLoanLitigation", addRec);
                    }

                    if (_view.IsValid)
                    {
                        using (TransactionScope txn = new TransactionScope())
                        {
                            try
                            {
                                if (addRec.DetailType.Key == (int)SAHL.Common.Globals.DetailTypes.EstateLateSecured || addRec.DetailType.Key == (int)SAHL.Common.Globals.DetailTypes.EstateLateUnsecured)
                                {
                                    foreach (IFinancialService finService in account.FinancialServices)
                                    {
                                        foreach (var manualDebitOrder in finService.ManualDebitOrders)
                                        {
                                            if (manualDebitOrder.TransactionType.Key == (short)SAHL.Common.Globals.TransactionTypes.MonthlyServiceFee && manualDebitOrder.Active)
                                            {
                                                this.ExclusionSets.Add(RuleExclusionSets.DetailTypeAddFinancialServiceRecurringTransactions);
                                                //recurringTransaction.Active = false;
                                                //_financialServiceRepo.SaveFinancialServiceRecurringTransaction(recurringTransaction);

                                                manualDebitOrderRepo.CancelManualDebitOrder(manualDebitOrder);

                                                this.ExclusionSets.Remove(RuleExclusionSets.DetailTypeAddFinancialServiceRecurringTransactions);
                                            }
                                        }
                                    }
                                }

                                if (addRec.DetailType.Key == (int)SAHL.Common.Globals.DetailTypes.LoanClosed)
                                {
                                    // If we are adding a detail type to close a loan, rather call a HALO API to close the loan correctly.
                                    // The detail type will be added by the proc once everything is correctly closed.
                                    _accRepository.CloseLoanAccount(account.Key, _view.CurrentPrincipal.Identity.Name);
                                }
                                else
                                {
                                    _accRepository.SaveDetail(addRec);
                                }

                                // This repo method has the logic to raise an external activity for certain detail types to fire a flag in the Debt Counselling Workflow Map.
                                DCRepo.RaiseActiveExternalActivityForAddDetailType(addRec.Account.Key, addRec.DetailType.Key);

                                txn.VoteCommit();
                            }
                            catch (Exception)
                            {
                                txn.VoteRollBack();
                                if (_view.IsValid)
                                    throw;
                            }
                        }
                    }

                    if (_view.Messages.Count == 0)
                        _view.Navigator.Navigate("LoanDetailDisplay");
                }
            }
        }

        #endregion Events Handlers

        #region Helpers

        IDebtCounsellingRepository _dcRepo;

        public IDebtCounsellingRepository DCRepo
        {
            get
            {
                if (_dcRepo == null)
                {
                    _dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
                }
                return _dcRepo;
            }
        }

        #endregion Helpers
    }
}