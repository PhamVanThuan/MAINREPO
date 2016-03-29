using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Origination.Valuations
{
    /// <summary>
    /// The building block methods for the Main Building Valuation Add details
    /// </summary>
    public class ManualValuationsMainDwellingExtendedDetailsAdd : ManualValuationsMainDwellingExtendedDetailsAddControls
    {
        /// <summary>
        /// Adds the details of the main building valuation details
        /// </summary>
        /// <param name="BuildingType"></param>
        /// <param name="RoofType"></param>
        /// <param name="Extent"></param>
        /// <param name="Rate"></param>
        /// <param name="button"></param>
        public void MainDwellingExtendedDetailsAdd(string BuildingType, string RoofType, int Extent, int Rate, ButtonTypeEnum button)
        {
            if (!string.IsNullOrEmpty(BuildingType) && BuildingType != string.Empty) base.MainBuildingTypes.Option(BuildingType).Select();
            if (!string.IsNullOrEmpty(RoofType) && RoofType != string.Empty) base.OutbuildingsRoofTypes.Option(RoofType).Select();
            if (Extent != -1) base.OutbuildingsExtent.TypeText(Extent.ToString());
            if (Rate != -1) base.OutbuildingsRate.TypeText(Rate.ToString());

            switch (button)
            {
                case ButtonTypeEnum.Next:
                    base.SubmitButton.Click();
                    break;

                case ButtonTypeEnum.Cancel:
                    base.CancelButton.Click();
                    break;

                case ButtonTypeEnum.Back:
                    base.BackButton.Click();
                    break;

                case ButtonTypeEnum.AddEntry:
                    base.AddButton.Click();
                    break;

                case ButtonTypeEnum.RemoveEntry:
                    base.RemoveButton.Click();
                    break;
            }
        }

        public void PopulateImprovement(Automation.DataModels.ValuationImprovement valuationImprovement)
        {
            base.MainBuildingTypes.Options[1].Select();
            base.ImprovementType.Option(valuationImprovement.ValuationImprovementTypeDescription).Select();
            if (base.ImprovementDate.Value != null)
                base.ImprovementDate.Value = valuationImprovement.ImprovementDate.Value.ToString(Formats.DateFormat);
            if (valuationImprovement.ImprovementValue > 0)
                base.ImprovementReplaceValue.Value = valuationImprovement.ImprovementValue.ToString();
        }

        public void PopulateOutbuilding(Automation.DataModels.ValuationOutbuilding valuationOutbuilding)
        {
            base.MainBuildingTypes.Options[0].Select();
            base.OutbuildingsRoofTypes.Option(valuationOutbuilding.ValuationRoofTypeDescription).Select();
            if (valuationOutbuilding.Extent > 0)
                base.OutbuildingsExtent.Value = valuationOutbuilding.Extent.ToString();
            if (valuationOutbuilding.Rate > 0)
                base.OutbuildingsRate.Value = valuationOutbuilding.Rate.ToString();
        }

        public void Cancel()
        {
            base.CancelButton.Click();
        }

        public void Add()
        {
            base.AddButton.Click();
        }

        public void Back()
        {
            base.BackButton.Click();
        }

        public void Next()
        {
            base.Next.Click();
        }

        public void AssertMainDwellingExtendedImprovementControls()
        {
            Assert.That(base.MainBuildingTypes.Exists, "MainBuildingTypes doesn't exist");
            Assert.That(base.ImprovementType.Exists, "ImprovementType doesn't exist");
            Assert.That(base.ImprovementDate.Exists, "ImprovementDate doesn't exist");
            Assert.That(base.ImprovementReplaceValue.Exists, "ImprovementReplaceValue doesn't exist");
        }

        public void AssertMainDwellingExtendedOutbuildingControls()
        {
            Assert.That(base.MainBuildingTypes.Exists, "MainBuildingTypes doesn't exist");
            Assert.That(base.OutbuildingsRoofTypes.Exists, "OutbuildingsRoofTypes doesn't exist");
            Assert.That(base.OutbuildingsExtent.Exists, "OutbuildingsExtent doesn't exist");
            Assert.That(base.OutbuildingsRate.Exists, "OutbuildingsRate doesn't exist");
            Assert.That(base.OutbuildingsReplaceValue.Exists, "OutbuildingsReplaceValue doesn't exist");
        }
    }
}