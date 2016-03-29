using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Origination.Valuations
{
    public class ManualValuationsMainDwellingDetailsAdd : ManualValuationsMainDwellingDetailsAddControls
    {
        public void MainDwellingDetailsAdd(string classification, string roofType, int extent, int rate, string cottageRoofType, int cottageExtent, int cottageRate, int valuationEscalation, ButtonTypeEnum button)
        {
            //Main Buiding
            base.MainBuildingClassification.Option(classification).Select();
            base.MainBuildingRoofType.Option(roofType).Select();
            base.BuildingExtent.TypeText(extent.ToString());
            base.MainBuildingRate.Value = rate.ToString();
            //Cottage
            if (!string.IsNullOrEmpty(cottageRoofType) && cottageRoofType != string.Empty) base.CottageRoofType.Option(cottageRoofType).Select();
            if (cottageExtent != -1) base.CottageExtent.TypeText(cottageExtent.ToString());
            if (cottageRate != -1) base.CottageRate.TypeText(cottageRate.ToString());

            if (valuationEscalation != -1) base.Escalation.TypeText(valuationEscalation.ToString());

            switch (button)
            {
                case ButtonTypeEnum.Cancel:
                    base.CancelButton.Click();
                    break;

                case ButtonTypeEnum.Next:
                    base.SubmitButton.Click();
                    break;

                default:
                    break;
            }
        }

        public void MainDwellingDetailsAdd(int classification, int roofType, int extent, int rate, int cottageRoofType, int cottageExtent, int cottageRate, int valuationEscalation, ButtonTypeEnum button)
        {
            //Main Buiding
            base.MainBuildingClassification.Options[classification].Select();
            base.MainBuildingRoofType.Options[roofType].Select();
            base.BuildingExtent.TypeText(extent.ToString());
            base.MainBuildingRate.TypeText(rate.ToString());

            //Cottage
            if (cottageRoofType != -1) base.CottageRoofType.Options[cottageRoofType].Select();
            if (cottageExtent != -1) base.CottageExtent.TypeText(cottageExtent.ToString());
            if (cottageRate != -1) base.CottageRate.TypeText(cottageRate.ToString());

            if (valuationEscalation != -1) base.Escalation.TypeText(valuationEscalation.ToString());

            switch (button)
            {
                case ButtonTypeEnum.Next:
                    base.SubmitButton.Click();
                    break;

                case ButtonTypeEnum.Cancel:
                    base.CancelButton.Click();
                    break;
            }
        }

        public void PopulateCottage(Automation.DataModels.ValuationCottage cottage)
        {
            base.CottageRoofType.Option(cottage.ValuationRoofTypeDescription).Select();
            base.CottageExtent.Value = cottage.Extent.ToString();
            base.CottageRate.Value = cottage.Rate.ToString();
        }

        public void PopulateMainBuilding(Automation.DataModels.Valuation valuation)
        {
            base.MainBuildingClassification.Option(valuation.ValuationClassificationDescription).Select();
            base.MainBuildingRoofType.Option(valuation.ValuationMainBuilding.ValuationRoofTypeDescription).Select();
            base.BuildingExtent.Value = valuation.ValuationMainBuilding.Extent.ToString();
            base.MainBuildingRate.Value = valuation.ValuationMainBuilding.Rate.ToString();
        }

        public void Cancel()
        {
            base.CancelButton.Click();
        }

        public void Next()
        {
            base.SubmitButton.Click();
        }

        public void AssertMainDwellingControls()
        {
            Assert.That(base.MainBuildingRoofType.Exists, "MainBuildingRoofType dropdown does not exist");
            Assert.That(base.MainBuildingRate.Exists, "MainBuildingRate does not exist");
            Assert.That(base.BuildingExtent.Exists, "MainBuildingBuildingExtent does not exist");
            Assert.That(base.MainBuildingReplaceValue.Exists, "MainBuildingReplaceValue does not exist");
            Assert.That(base.CottageRoofType.Exists, "CottageRoofType does not exist");
            Assert.That(base.CottageExtent.Exists, "CottageExtent does not exist");
            Assert.That(base.CottageRate.Exists, "CottageRate does not exist");
            Assert.That(base.Escalation.Exists, "Escalation% does not exist");
        }
    }
}