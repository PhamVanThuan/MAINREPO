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
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.Exceptions;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace SAHL.Web.Views.Common.Presenters
{
    public class UpdateFinancialAdjustmentsUpdate : UpdateFinancialAdjustmentsBase
    {
        /// <summary>
        /// Constructor for UpdateRatesOverridesUpdate
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public UpdateFinancialAdjustmentsUpdate(IFinancialAdjustments view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// Initialise event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.OnUpdateButtonClicked += new KeyChangedEventHandler(_view_OnUpdateButtonClicked);
            _view.OnCancelButtonClicked += new KeyChangedEventHandler(_view_OnCancelButtonClicked);

            _view.FinancialAdjustmentsGridRowUpdating += new KeyChangedEventHandler(_view_FinancialAdjustmentsGridRowUpdating);

        }

        protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        protected void _view_OnUpdateButtonClicked(object sender, EventArgs e)
        {
            TransactionScope tx = new TransactionScope();
            try
            {
                IFinancialAdjustmentStatus ActiveStatus = _lookups.FinancialAdjustmentStatuses.ObjectDictionary[((int)FinancialAdjustmentStatuses.Active).ToString()];
                IFinancialAdjustmentStatus CanceledStatus = _lookups.FinancialAdjustmentStatuses.ObjectDictionary[((int)FinancialAdjustmentStatuses.Canceled).ToString()];

                DataTable currentDT = this.GlobalCacheData["FinancialAdjustmentData"] as DataTable;

                foreach (DataRow dr in currentDT.Rows)
                {
                    foreach (IFinancialAdjustment financialAdjustment in _financialAdjustments)
                    {

                        if (financialAdjustment.Key.ToString() == Convert.ToString(dr["FinancialAdjustmentKey"]))
                        {
                            // Cancelling a FinancialAdjustment
                            if (financialAdjustment.FinancialAdjustmentStatus.Description != Convert.ToString(dr["Status"]))
                            {
                                //Status updated from active or inactive to cancelled
                                if (//financialAdjustment.FinancialAdjustmentStatus.Key == ActiveStatus.Key &&
                                     Convert.ToString(dr["Status"]) == CanceledStatus.Description)
                                {
                                    // write the stage transition  record
                                    IStageDefinitionRepository stageDefinitionRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
                                    stageDefinitionRepo.SaveStageTransition(financialAdjustment.Key, Convert.ToInt32(SAHL.Common.Globals.StageDefinitionGroups.LoanServicing),
                                        SAHL.Common.Constants.StageDefinitionConstants.UpdateFinancialAdjustment, "UpdateFinancialAdjustment", CurrentADUser);


                                    if (financialAdjustment.FinancialAdjustmentTypeSource.Key == Convert.ToInt32(FinancialAdjustmentTypeSources.DiscountedLinkrate))
                                    {
                                        // require an API from the back-end for Opting out of a DiscountedLinkrate
                                        _fadjRepo.DiscountedLinkRateOptOut(financialAdjustment, _view.CurrentPrincipal.Identity.Name);
                                    }

                                    // A client cannot and should NOT be opted into DefendingCancellations
                                    // We will service this going forward in the term of allowing it to be Cancelled but never re-Activated
                                    else if (financialAdjustment.FinancialAdjustmentTypeSource.Key == Convert.ToInt32(FinancialAdjustmentTypeSources.DefendingCancellations))
                                    {
                                        _fadjRepo.DefendingDiscountOptOut(financialAdjustment.FinancialService.Account.Key, _view.CurrentPrincipal.Identity.Name);
                                    }
                                }
                                // Opting into a FinancialAdjustment
                                //Status updated from inactive to active
                                else if (financialAdjustment.FinancialAdjustmentStatus.Key == CanceledStatus.Key &&
                                     Convert.ToString(dr["Status"]) == ActiveStatus.Description)
                                {
                                    if (financialAdjustment.FinancialAdjustmentTypeSource.Key == Convert.ToInt32(FinancialAdjustmentTypeSources.DefendingCancellations))
                                    {
                                        // opting into DefendingCancellations is no longer serviced
                                        string msg = "Cannot opt into Defending Cancellations.";
                                        _view.Messages.Add(new Error(msg, msg));
                                        throw new DomainValidationException();
                                    }
                                    else if (financialAdjustment.FinancialAdjustmentTypeSource.Key == Convert.ToInt32(FinancialAdjustmentTypeSources.DiscountedLinkrate))
                                    {
                                        // require an API from the back-end for Opting into a DiscountedLinkrate
                                        _fadjRepo.DiscountedLinkRateOptIn(financialAdjustment, _view.CurrentPrincipal.Identity.Name);
                                    }
                                }
                            }
                        }
                    }
                }
                tx.VoteCommit();
            }
            catch (Exception)
            {
                tx.VoteRollBack();
                if (_view.IsValid)
                    throw new Exception();
            }
            finally
            {
                tx.Dispose();
            }

            if (_view.IsValid)
            {
                _cRepo.ClearFromNHibernateSession(_account);

                //Rebind the data
                if (this.GlobalCacheData.ContainsKey("FinancialAdjustmentData"))
                {
                    this.GlobalCacheData.Remove("FinancialAdjustmentData");
                    GetFinancialAdjustments(true);
                }
            }
        }

        protected void _view_FinancialAdjustmentsGridRowUpdating(object sender, KeyChangedEventArgs e)
        {
            int dtIndex = Convert.ToInt32(e.Key);

            DataTable currentDT = this.GlobalCacheData["FinancialAdjustmentData"] as DataTable;

            Dictionary<string, string> dict = sender as Dictionary<string, string>;

            foreach (KeyValuePair<string, string> kv in dict)
            {
                currentDT.Rows[dtIndex][kv.Key] = kv.Value;
            }

            this.GlobalCacheData.Add("FinancialAdjustmentData", currentDT, LifeTimes);
            BindFinancialAdjustmentGrid(currentDT, true);
        }

    }
}
