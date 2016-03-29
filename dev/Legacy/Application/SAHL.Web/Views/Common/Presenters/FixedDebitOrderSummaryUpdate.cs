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
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Events;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.Service.Interfaces;


namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// class FixedDebitOrderSummaryUpdate
    /// </summary>
    public class FixedDebitOrderSummaryUpdate : FixedDebitOrderSummary
    {
        /// <summary>
        /// Constructor for FixedDebitOrderSummaryUpdate
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public FixedDebitOrderSummaryUpdate(IFixedDebitOrderSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.selectedFirstRow = false;
            _view.SetGridPostBack();
            _view.BindFutureDatedDOGrid(_futureDatedChangeLst);
            _view.SetUpInitialDataOnView(account);
            _view.OnFutureOrderGridSelectedIndexChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnFutureOrderGridSelectedIndexChanged);
            _view.UpdateButtonClicked += new KeyChangedEventHandler(_view_UpdateButtonClicked);
            _view.CancelButtonClicked += new KeyChangedEventHandler(_view_CancelButtonClicked);
        }

        /// <summary>
        /// Set visibility of Controls on PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;
            _view.ShowButtons = true;
            _view.ShowUpdateableControl = true;
            _view.SetControlForUpdate();
        }

        void _view_OnFutureOrderGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            _view.BindUpdateableControlsData(_futureDatedChangeLst[Convert.ToInt32(e.Key)]);
        }
        void _view_CancelButtonClicked(object sender, KeyChangedEventArgs e)
        {
            Navigator.Navigate("Cancel");
        }

        void _view_UpdateButtonClicked(object sender, KeyChangedEventArgs e)
        {
            DateTime? _effectiveDate;
            IFutureDatedChange _fDatedChange;

            _effectiveDate = _view.GetEffectiveDateCaptured;

            if (Convert.ToInt32(e.Key) == -1) // If nothing was selected on the grid
            {
                if (_effectiveDate.HasValue && _effectiveDate.Value.Date == DateTime.Today) // then Update information on Account Table
                {
                    double updatedFixedDebitOrderAmount = _view.UpdatedFixedDebitOrderAmount;
                   
                    TransactionScope txn = new TransactionScope();

                    try
                    {
                        // Check that the Updated Fixed DebitOrder Amount is greater than or equal to the user captured amt
                        IRuleService ruleService = ServiceFactory.GetService<IRuleService>();
                        ruleService.ExecuteRule(_view.Messages, "FinancialServiceBankAccountUpdateDebitOrderAmount", account, updatedFixedDebitOrderAmount);
                        if (_view.IsValid)
                        {
                            _accRepo.UpdateFixedPayment(account.Key, updatedFixedDebitOrderAmount, _view.CurrentPrincipal.Identity.Name);
                        }
                        else
                        {
                            throw new Exception();
                        }
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

                }
                else // if effective date in the future then insert into FutureDatedChangeTable
                {
                    _fDatedChange = _futureDatedRepo.CreateEmptyFutureDatedChange();
                    _view.GetCapturedFutureDatedChange(_fDatedChange);

                    IFutureDatedChangeDetail _fdChangeDetail = _futureDatedRepo.CreateEmptyFutureDatedChangeDetail();
                    _view.GetCapturedFutureDatedChangeDetail(_fdChangeDetail);

                    _fDatedChange.IdentifierReferenceKey = account.Key;
                    _fdChangeDetail.ReferenceKey = account.Key;
                    _fdChangeDetail.FutureDatedChange = _fDatedChange;

                    TransactionScope txn = new TransactionScope();

                    try
                    {
                        _futureDatedRepo.SaveFutureDatedChange(_fDatedChange);
                        _futureDatedRepo.SaveFutureDatedChangeDetail(_fdChangeDetail);
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

                }
            }
            else // If something selected on Grid - updated selected item
            {
                _fDatedChange = _futureDatedChangeLst[Convert.ToInt32(e.Key)];

                _view.GetUpdatedFDChange(_fDatedChange);

               TransactionScope txn = new TransactionScope();

                try
                {
                    _futureDatedRepo.SaveFutureDatedChange(_fDatedChange);
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

            }

            if (_view.IsValid)
                Navigator.Navigate("Cancel");
        }
    }

}
