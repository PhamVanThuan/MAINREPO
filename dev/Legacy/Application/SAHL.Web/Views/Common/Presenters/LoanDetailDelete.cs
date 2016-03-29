using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;


namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class LoanDetailDelete : SAHLCommonBasePresenter<ILoanDetail>
    {
        private IReadOnlyEventList<IDetail> _lstDetail;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LoanDetailDelete(ILoanDetail view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

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

            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (cboNode != null)
            {
                SetupDisplay(Convert.ToInt32(cboNode.GenericKey));
            }
        }


        #region Private Methods


        private void SetupDisplay(int accountKey)
        {
            IAccountRepository _accRepository;
            _accRepository = RepositoryFactory.GetRepository<IAccountRepository>();
           
            _lstDetail = _accRepository.GetDetailByAccountKey(accountKey);

            _view.ShowButtons = true;
            _view.ShowLabels = true;
            _view.SubmitButtonText = "Delete";
            _view.DeleteMode = true;
            _view.DetailGridPostBackType = GridPostBackType.SingleClick;

            if (_lstDetail != null && _lstDetail.Count > 0)
            {
                _view.BindDetailGrid(_lstDetail);
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
                int selectedIndex = Convert.ToInt32(e.Key);
                if (_lstDetail != null && _lstDetail.Count > selectedIndex)
                {
                    _view.BindData(_lstDetail[selectedIndex]);
                    if (_lstDetail[selectedIndex].DetailType.AllowUpdateDelete == true && _lstDetail[selectedIndex].DetailType.AllowScreen == true)
                    {
                        _view.SubmitButtonEnabled = true;
                    }
                    else
                    {
                        _view.SubmitButtonEnabled = false;
                    }
                }
            }
        }

        void _view_OnCancelClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("LoanDetailDisplay");
        }

        void _view_OnSubmitClicked(object sender, KeyChangedEventArgs e)
        {
            if (e != null)
            {
                if (_view.Messages.ErrorMessages.Count == 0)
                {
                    int selectGridIndex = Convert.ToInt32(e.Key);
                    IDetail deleteRec = _lstDetail[selectGridIndex];
                    IAccountRepository accRepository = RepositoryFactory.GetRepository<IAccountRepository>();
                    ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                    IManualDebitOrderRepository manualDebitOrderRepo = RepositoryFactory.GetRepository<IManualDebitOrderRepository>();
                    IMemoRepository memoRepo = RepositoryFactory.GetRepository<IMemoRepository>();


                    TransactionScope txn = new TransactionScope();
                    try
                    {
                        accRepository.RemoveDetailByKey(deleteRec.Key);
                        if (deleteRec.DetailType.Key == (int)SAHL.Common.Globals.DetailTypes.EstateLateSecured || deleteRec.DetailType.Key == (int)SAHL.Common.Globals.DetailTypes.EstateLateUnsecured)
                        {
                            short estateLateDetailCount = 0;

                            CBOMenuNode _cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
                            IAccount account = null;
                            if (_cboNode != null)
                            {
                                account = accRepository.GetAccountByKey(Convert.ToInt32(_cboNode.GenericKey));
                            }

                            if (account != null)
                            {
                                foreach (IDetail detail in account.Details)
                                {
                                    if (detail.DetailType.Key == (int)SAHL.Common.Globals.DetailTypes.EstateLateSecured || detail.DetailType.Key == (int)SAHL.Common.Globals.DetailTypes.EstateLateUnsecured)
                                    {
                                        estateLateDetailCount++;
                                    }
                                }

                                if (estateLateDetailCount == 0)
                                {
                                    foreach (IFinancialService finService in account.FinancialServices)
                                    {
                                        foreach (var manualDebitOrder in finService.ManualDebitOrders)
                                        {
                                            if (manualDebitOrder.TransactionType.Key == (short)SAHL.Common.Globals.TransactionTypes.MonthlyServiceFee && !manualDebitOrder.Active)
                                            {
                                                this.ExclusionSets.Add(RuleExclusionSets.DetailTypeAddFinancialServiceRecurringTransactions);
                                                

                                                // When REINSTATING A ManualDebitOrder We have to create a new one

                                                //manualDebitOrder.Active = true;
                                                //_financialServiceRepo.SaveFinancialServiceRecurringTransaction(manualDebitOrder);

                                                // Create

                                                var memo = memoRepo.CreateMemo();
                                                memo.ADUser = manualDebitOrder.Memo.ADUser;
                                                memo.Description = manualDebitOrder.Memo.Description;
                                                memo.ExpiryDate = manualDebitOrder.Memo.ExpiryDate;
                                                memo.GeneralStatus = manualDebitOrder.Memo.GeneralStatus;
                                                memo.GenericKey = manualDebitOrder.Memo.GenericKey;
                                                memo.GenericKeyType = manualDebitOrder.Memo.GenericKeyType;
                                                memo.InsertedDate = DateTime.Now.Date;
                                                memo.ReminderDate = DateTime.Now.AddDays(1);
                                                memo.ExpiryDate = DateTime.Now.AddDays(1);

                                                //if (manualDebitOrder.Memo != null)
                                                //{
                                                //    var newMemo = _memoRepo.CreateMemo();
                                                //    newMemo.ADUser = manualDebitOrder.Memo.ADUser;
                                                //    newMemo.Description = manualDebitOrder.Memo.Description;
                                                //    newMemo.ExpiryDate = manualDebitOrder.Memo.ExpiryDate;
                                                //    newMemo.GeneralStatus = manualDebitOrder.Memo.GeneralStatus;
                                                //    newMemo.GenericKey = manualDebitOrder.Memo.GenericKey;
                                                //    newMemo.GenericKeyType = manualDebitOrder.Memo.GenericKeyType;
                                                //    newMemo.InsertedDate = DateTime.Now.Date;
                                                //    _memoRepo.SaveMemo(newMemo);

                                                //    memoKey = newMemo.Key;
                                                //}

                                                IManualDebitOrder newManualDebitOrder = manualDebitOrderRepo.GetEmptyManualDebitOrder();
                                                newManualDebitOrder.FinancialService = manualDebitOrder.FinancialService;
                                                newManualDebitOrder.ActionDate = manualDebitOrder.ActionDate;
                                                newManualDebitOrder.Amount = manualDebitOrder.Amount;
                                                newManualDebitOrder.BankAccount = manualDebitOrder.BankAccount;
                                                newManualDebitOrder.GeneralStatus = lookupRepo.GeneralStatuses[SAHL.Common.Globals.GeneralStatuses.Active];
                                                newManualDebitOrder.InsertDate = DateTime.Now;
                                                newManualDebitOrder.Memo = memo;
                                                newManualDebitOrder.Reference = manualDebitOrder.Reference;
                                                newManualDebitOrder.TransactionType = manualDebitOrder.TransactionType;
                                                newManualDebitOrder.UserID = manualDebitOrder.UserID;

                                                manualDebitOrderRepo.SaveManualDebitOrder(newManualDebitOrder);

                                                this.ExclusionSets.Remove(RuleExclusionSets.DetailTypeAddFinancialServiceRecurringTransactions);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // This repo method has the logic to raise an external activity for certain detail types to fire a flag in the Debt Counselling Workflow Map.
                        DCRepo.RaiseActiveExternalActivityForDeleteDetailType(deleteRec.Account.Key, deleteRec.DetailType.Key);

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

                    if (_view.Messages.ErrorMessages.Count == 0)
                        _view.Navigator.Navigate("LoanDetailDisplay");
                }
            }
        }

        #endregion

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

        #endregion
    }
}





