using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;
using WatiN.Core.UtilityClasses;

namespace BuildingBlocks.Presenters.Origination.Valuations
{
    public class ManualValuationAdd : ValuationDetailsAddControls
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="Valuer"></param>
        /// <param name="ValuationAmount"></param>
        /// <param name="ValuationDate"></param>
        /// <param name="MunicipalValuation"></param>
        /// <param name="RoofDescription"></param>
        /// <param name="ConventionalValuation"></param>
        /// <param name="HOCThatchAmount"></param>
        /// <param name="button"></param>
        public void ValuationDetailsAdd(string Valuer, int ValuationAmount, string ValuationDate, int MunicipalValuation, string RoofDescription,
            int ConventionalValuation, int HOCThatchAmount, ButtonTypeEnum button)
        {
            base.Valuers.Option(Valuer).Select();

            //Controls.ctl00MaintxtValuationAmount.Value = ValuationAmount.ToString();
            base.ValuationRandsAmount.Click();
            base.ValuationRandsAmount.TypeText(ValuationAmount.ToString());

            //Date needs to be passed with exact format "00/00/0000"
            base.ValuationDate.Value = ValuationDate;
            if (MunicipalValuation != -1)
            {
                //Controls.ctl00MaintxtMunicipalValuation.TypeText(MunicipalValuation.ToString());
                base.MunicipalValuation.Click();
                base.MunicipalValuation.TypeText(MunicipalValuation.ToString());
            }

            //HOC Details
            base.HOCRoofDescription.Option(RoofDescription).Select();
            SimpleTimer timer = new SimpleTimer(TimeSpan.FromSeconds(30));
            while (base.HOCConventionalAmount.GetAttributeValue("disabled") == "disabled")
            {
                if (timer.Elapsed)
                {
                    throw new WatiN.Core.Exceptions.TimeoutException("while waiting for the 'Conventional Amount' field to become active");
                }
                System.Threading.Thread.Sleep(1000);
            }
            if (ConventionalValuation != -1)
            {
                //Controls.ctl00MaintxtHOCConventionalAmount.Value = ConventionalValuation.ToString();
                base.HOCConventionalAmount.Click();
                base.HOCConventionalAmount.TypeText(ConventionalValuation.ToString());
            }
            if (HOCThatchAmount != -1)
            {
                base.HOCThatchAmount.TypeText(HOCThatchAmount.ToString());
                base.HOCThatchAmount.Click();
                base.HOCThatchAmount.TypeText(HOCThatchAmount.ToString());
            }

            switch (button)
            {
                case ButtonTypeEnum.Next:
                    base.SubmitButton.Click();
                    break;

                case ButtonTypeEnum.Cancel:
                    base.CancelButton.Click();
                    break;

                case ButtonTypeEnum.Back:
                    base.MainBackButton.Click();
                    break;
            }
            base.Document.DomContainer.WaitForComplete();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Valuer"></param>
        /// <param name="ValuationAmount"></param>
        /// <param name="ValuationDate"></param>
        /// <param name="MunicipalValuation"></param>
        /// <param name="RoofDescription"></param>
        /// <param name="ConventionalValuation"></param>
        /// <param name="HOCThatchAmount"></param>
        /// <param name="Button"></param>
        public void ValuationDetailsAdd(int Valuer, int ValuationAmount, string ValuationDate, int MunicipalValuation,
            int RoofDescription, int ConventionalValuation, int HOCThatchAmount, ButtonTypeEnum Button)
        {
            if (Valuer != -1) base.Valuers.Options[Valuer].Select();
            if (ValuationAmount != -1) base.ValuationRandsAmount.TypeText(ValuationAmount.ToString());
            //Date needs to be passed with exact format "00/00/0000"
            if (!string.IsNullOrEmpty(ValuationDate) && ValuationDate != string.Empty) base.ValuationDate.Value = ValuationDate;
            if (MunicipalValuation != -1) base.MunicipalValuation.TypeText(MunicipalValuation.ToString());

            //HOC Details
            if (RoofDescription != -1) base.HOCRoofDescription.Options[RoofDescription].Select();
            if (ConventionalValuation != -1) base.HOCConventionalAmount.TypeText(ConventionalValuation.ToString());
            if (HOCThatchAmount != -1) base.HOCThatchAmount.TypeText(HOCThatchAmount.ToString());

            switch (Button)
            {
                case ButtonTypeEnum.Next:
                    base.SubmitButton.Click();
                    break;

                case ButtonTypeEnum.Cancel:
                    base.CancelButton.Click();
                    break;

                case ButtonTypeEnum.Back:
                    base.MainBackButton.Click();
                    break;
            }
        }

        public void PopulateValuationDetail(Automation.DataModels.Valuation valuation)
        {
            base.Valuers.Option(valuation.Valuator.LegalEntity.RegisteredName).Select();
            if (valuation.ValuationAmount > 0)
            {
                base.ValuationRandsAmount.TypeText(valuation.ValuationAmount.ToString());
                base.ValuationCentsAmount.TypeText("00");
            }
            if (valuation.ValuationDate != null)
                base.ValuationDate.Value = valuation.ValuationDate.Value.ToString(Formats.DateFormat);
            if (valuation.ValuationMunicipal > 0)
                base.MunicipalValuation.Value = valuation.ValuationMunicipal.ToString();
            base.HOCRoofDescription.Option(valuation.HOCRoofDescription).Select();
            switch (valuation.HOCRoofKey)
            {
                case HOCRoofEnum.Conventional:
                    if (valuation.HOCConventionalAmount > 0)
                    {
                        base.HOCConventionalAmount.TypeText(valuation.HOCConventionalAmount.ToString());
                        base.HOCConventionalAmountCents.TypeText("00");
                    }
                    break;

                case HOCRoofEnum.Thatch:
                    if (valuation.HOCThatchAmount > 0)
                    {
                        base.HOCThatchAmount.TypeText(valuation.HOCThatchAmount.ToString());
                        base.HOCThatchAmountCents.TypeText("00");
                    }
                    break;

                case HOCRoofEnum.Partial:
                    if (valuation.HOCConventionalAmount > 0)
                    {
                        base.HOCConventionalAmount.TypeText(valuation.HOCConventionalAmount.ToString());
                        base.HOCConventionalAmountCents.TypeText("00");
                        base.HOCThatchAmount.TypeText(valuation.HOCThatchAmount.ToString());
                        base.HOCThatchAmountCents.TypeText("00");
                    }
                    break;
            }
        }

        public void Cancel()
        {
            base.CancelButton.Click();
        }

        public void Back()
        {
            base.MainBackButton.Click();
        }

        public void Add()
        {
            base.Add.Click();
        }

        public void AssertManualValuationControls()
        {
            Assert.That(base.ValuationDate.Exists, "ValuationDate doesn't exist");
            Assert.That(base.ValuationRandsAmount.Exists, "ValuationRandsAmount doesn't exist");
            Assert.That(base.ValuationCentsAmount.Exists, "ValuationCentsAmount doesn't exist");
            Assert.That(base.ValuationDate.Exists, "ValuationDate doesn't exist");
            Assert.That(base.MunicipalValuation.Exists, "ValuationDate doesn't exist");
            Assert.That(base.HOCRoofDescription.Exists, "HOCRoofDescription doesn't exist");
            Assert.That(base.HOCThatchAmount.Exists, "HOCThatchAmount doesn't exist");
            Assert.That(base.HOCConventionalAmount.Exists, "HOCThatchAmount doesn't exist");
        }
    }
}