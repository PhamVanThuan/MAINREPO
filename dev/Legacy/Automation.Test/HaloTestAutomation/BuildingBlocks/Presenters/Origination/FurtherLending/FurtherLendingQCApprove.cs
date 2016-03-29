using BuildingBlocks.Presenters.CommonPresenters;
using Common.Constants;
using Common.Extensions;
using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.Origination.FurtherLending
{
    public class FurtherLendingQCApprove : FurtherLendingQuickCashControls
    {
        /// <summary>
        /// Approves QC on an further lending application. If the max QC amount is less than R10,000 then the QC will be declined.
        /// </summary>
        /// <param name="b">TestBrowser</param>
        internal void ApproveQC()
        {
            //find the max QC amount
            string maxQC = base.lblMaximumQC.Text.CleanCurrencyString(true);
            if (Convert.ToInt32(maxQC) > 10000)
            {
                //calc the QC amounts to approve
                double maxTotal = 0.8 * (Convert.ToDouble(maxQC));
                double maxUpfront = 0.5 * (Convert.ToInt32(maxQC));
                //set the total amt approved
                base.txtTotalApprovedRands.Value = maxTotal.ToString();
                //set the upfront amt approved
                base.txtTotalUpfrontApprovedRands.Value = maxUpfront.ToString();
                //save
                base.btnSave.Click();
            }
            else
            {
                DeclineQC();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="b"></param>
        internal void DeclineQC()
        {
            //click the QC decline button
            base.btnQCDeclineReasons.Click();
            base.Document.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.QuickCashDecline);
            //save
            base.btnSave.Click();
        }
    }
}