using Automation.DataAccess;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using ObjectMaps.Pages;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using WatiN.Core;
using WatiN.Core.Exceptions;
using WatiN.Core.UtilityClasses;

namespace BuildingBlocks.Presenters.LegalEntity
{
    /// <summary>
    ///
    /// </summary>
    public class LegalEntityDetailsSubsidyAddLegalEntityEmploymentDetails : LegalEntitySubsidyDetailsAddControls
    {
        private readonly IApplicationService applicationService;

        public LegalEntityDetailsSubsidyAddLegalEntityEmploymentDetails()
        {
            applicationService = ServiceLocator.Instance.GetService<IApplicationService>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="offerkey"></param>
        /// <param name="subsidyProvider"></param>
        /// <param name="salaryPersalNo"></param>
        /// <param name="paypoint"></param>
        /// <param name="rank"></param>
        /// <param name="notch"></param>
        /// <param name="stopOrderAmount"></param>
        /// <param name="button"></param>
        public void AddSubsidyDetails(int offerkey, string subsidyProvider, string salaryPersalNo, string paypoint, string rank,
            string notch, int stopOrderAmount, ButtonTypeEnum button)
        {
            if (offerkey > 0)
            {
                QueryResults results = applicationService.GetOfferData(offerkey);
                string accountNumber = results.Rows(0).Column("ReservedAccountKey").Value;
                base.ddlAccountKey.Option(new Regex("^" + accountNumber + "[\x20-\x7E]*$")).Select();
            }
            if (!string.IsNullOrEmpty(subsidyProvider))
            {
                base.txtSubsidyProvider.TypeText(subsidyProvider);

                SimpleTimer timer = new SimpleTimer(TimeSpan.FromSeconds(30));
                while (true)
                {
                    if (timer.Elapsed)
                    {
                        new WatiNException();
                    }
                    else if (base.divsSAHLAutoCompleteItems.Count > 0)
                    {
                        base.divsSAHLAutoCompleteItems[0].MouseDown();
                        break;
                    }
                    Thread.Sleep(Settings.SleepTime);
                }
            }
            if (!string.IsNullOrEmpty(salaryPersalNo)) base.txtSalaryNumber.Value = (salaryPersalNo);
            if (!string.IsNullOrEmpty(paypoint)) base.txtPaypoint.Value = (paypoint);
            if (!string.IsNullOrEmpty(rank)) base.txtRank.Value = (rank);
            if (!string.IsNullOrEmpty(notch)) base.txtNotch.Value = (notch);
            if (stopOrderAmount != -1) base.currStopOrder_txtRands.Value = (stopOrderAmount.ToString());

            switch (button)
            {
                case ButtonTypeEnum.Back:
                    base.btnBack.Click();
                    break;

                case ButtonTypeEnum.Cancel:
                    base.btnCancel.Click();
                    break;

                case ButtonTypeEnum.Save:
                    base.btnSave.Click();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="TestBrowser"></param>
        /// <param name="accountNumber"></param>
        /// <param name="subsidyProvider"></param>
        /// <param name="salaryPersalNo"></param>
        /// <param name="paypoint"></param>
        /// <param name="rank"></param>
        /// <param name="notch"></param>
        /// <param name="stopOrderAmount"></param>
        /// <param name="button"></param>
        public void AddSubsidyDetailsForAccount(int accountNumber, string subsidyProvider, string salaryPersalNo, string paypoint,
            string rank, string notch, int stopOrderAmount, ButtonTypeEnum button)
        {
            if (accountNumber != -1) base.ddlAccountKey.Option(new Regex("^" + accountNumber + "[\x20-\x7E]*$")).Select();
            if (!string.IsNullOrEmpty(subsidyProvider) && base.txtSubsidyProvider.Value != subsidyProvider)
            {
                base.txtSubsidyProvider.TypeText(subsidyProvider);
                base.divSAHLAutoCompleteDiv.WaitUntilExists();
                Thread.Sleep(3000);
                base.divSAHLAutoCompleteItem(subsidyProvider).MouseDown();
            }
            //Thread.Sleep(3000);
            if (!string.IsNullOrEmpty(salaryPersalNo) && base.txtSalaryNumber.Value != salaryPersalNo)
                base.txtSalaryNumber.Value = (salaryPersalNo);
            if (!string.IsNullOrEmpty(paypoint) && base.txtPaypoint.Value != paypoint)
                base.txtPaypoint.Value = (paypoint);
            if (!string.IsNullOrEmpty(rank) && base.txtRank.Value != rank)
                base.txtRank.Value = (rank);
            if (!string.IsNullOrEmpty(notch) && base.txtNotch.Value != notch)
                base.txtNotch.Value = (notch);
            if (stopOrderAmount != -1 && base.currStopOrder_txtRands.Value != stopOrderAmount.ToString())
                base.currStopOrder_txtRands.TypeText(stopOrderAmount.ToString());
            base.currStopOrder_txtCents.TypeText("00");

            switch (button)
            {
                case ButtonTypeEnum.Back:
                    base.btnBack.Click();
                    break;

                case ButtonTypeEnum.Cancel:
                    base.btnCancel.Click();
                    break;

                case ButtonTypeEnum.Save:
                    base.btnSave.Click();
                    break;
            }
        }

        /// <summary>
        /// Adds subsidy details using our employment model
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="accountKey"></param>
        /// <param name="employment"></param>
        /// <param name="button"></param>
        public void AddSubsidyDetailsForAccount(int accountKey, Automation.DataModels.Employment employment, ButtonTypeEnum button)
        {
            AddSubsidyDetailsForAccount(accountKey, employment.SubsidyProvider, employment.SalaryNumber, employment.PayPoint,
                employment.Rank, employment.Notch, employment.StopOrderAmount, button);
        }

        /// <summary>
        /// Clicks the cancel button on the subsidy details screen
        /// </summary>
        public void ClickCancel()
        {
            base.btnCancel.Click();
        }

        /// <summary>
        /// Clicks the cancel button on the subsidy details screen
        /// </summary>
        public void ClickBack()
        {
            base.btnBack.Click();
        }
    }
}