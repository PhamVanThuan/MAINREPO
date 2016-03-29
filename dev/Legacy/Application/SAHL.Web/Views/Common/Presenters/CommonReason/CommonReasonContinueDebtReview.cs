using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common.Presenters.CommonReason
{
    public class CommonReasonContinueDebtReview : CommonReasonBase
    {
        private List<SelectedReason> _selectedReasons;
        private IAccountRepository _accRepo;
        private IDebtCounsellingRepository _dcRepo;

        /// <summary>
        /// Constructpor for CommonReasonQAComplete
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CommonReasonContinueDebtReview(ICommonReason view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }
        /// <summary>
        /// OnViewInitialised event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) 
                return;

            _accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            _dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

            _view.SetHiddenIndText = "1";
            _view.CancelButtonVisible = true;

            // limit the selection to one reason
            _view.OnlyOneReasonCanBeSelected = true;
        }

        /// <summary>
        /// Overrides the base OnSubmitButtonClicked event so that specific credit decline actions can be performed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void _view_OnSubmitButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            _selectedReasons = (List<SelectedReason>)e.Key;

            if (_selectedReasons.Count <= 0)
            {
                string errorMessage = "Must select at least one reason";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
            }

            if (_view.IsValid && _selectedReasons.Count > 0) // only save if reason has been selected
            {      
                base._view_OnSubmitButtonClicked(sender, e);

                #region check for and insert 'Legal Action Stopped' detail type if reason selected is 'Reallocate to DC'

                TransactionScope txn = new TransactionScope();
                
                try
                {
                   // get the selected reason definition record
                    IReasonDefinition selectedReasonDefinition = base._reasonRepo.GetReasonDefinitionByKey(_selectedReasons[0].ReasonDefinitionKey);

                    if (selectedReasonDefinition.ReasonDescription.Key == (int)SAHL.Common.Globals.ReasonDescriptions.ReallocateToDC)
                    {
                        // get the debtcounselling record
                        IDebtCounselling dc = _dcRepo.GetDebtCounsellingByKey(base.GenericKey);

                        int detailTypeKey = (int)SAHL.Common.Globals.DetailTypes.LegalActionStopped;
                        // get latest 'Legal Action Stopped' detail
                        IDetail detailLegalActionStopped = _accRepo.GetLatestDetailByAccountKeyAndDetailType(dc.Account.Key, detailTypeKey);
                        // get latest 'Foreclosure Underway' detail
                        IDetail detailForeclosureUnderway = _accRepo.GetLatestDetailByAccountKeyAndDetailType(dc.Account.Key, (int)SAHL.Common.Globals.DetailTypes.ForeclosureUnderway);

                        // insert a new 'Legal Action Stopped' detail if:
                        // 1. 'Foreclosure Underway' detail record exists and
                        // 1. No 'Legal Action Stopped' detail record exists  or
                        // 2. 'Legal Action Stopped' detail record exists but there is a more recent 'Foreclosure Underway' detail record

                        if (detailForeclosureUnderway != null)
                        {
                            if (detailLegalActionStopped == null || (detailForeclosureUnderway.Key > detailLegalActionStopped.Key))
                            {
                                SAHL.Common.BusinessModel.Interfaces.IDetail detail = _accRepo.CreateEmptyDetail();
                                detail.Account = dc.Account;
                                detail.ChangeDate = DateTime.Now;
                                detail.Description = "Continue Debt Review";
                                detail.DetailDate = DateTime.Now;
                                detail.DetailType = base._lookUpRepo.DetailTypes.ObjectDictionary[detailTypeKey.ToString()];
                                detail.UserID = "X2";
                                //detail.LinkID = dc.Key;
                                _accRepo.SaveDetail(detail);
                                
                                txn.VoteCommit();
                            }
                        }

                    }
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

                #endregion


                TransactionScope tx = new TransactionScope();
                try
                {
                    CompleteActivityAndNavigate();
                }
                catch (Exception)
                {
                    tx.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }
                finally
                {
                    tx.Dispose();
                }

            }
        }

        public override void CancelActivity()
        {
            base.X2Service.CancelActivity(_view.CurrentPrincipal);
            base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }


        public override void CompleteActivityAndNavigate()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            X2ServiceResponse rsp = base.X2Service.CompleteActivity(_view.CurrentPrincipal, null, spc.IgnoreWarnings);
            if (base.sdsdgKeys.Count > 0)
            {
                UpdateReasonsWithStageTransitionKey();
            }
            base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }
    }
}