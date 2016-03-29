using Common.Constants;
using ObjectMaps.Pages;
using System.Collections.Generic;
using WatiN.Core;

namespace BuildingBlocks.Presenters.LoanServicing.Correspondence
{
    public sealed class PropertyDetailsUpdate : PropertyDetailsControls
    {
        public void PopulateView(Automation.DataModels.Property property)
        {
            base.ctl00MainddlPropertyType.Option(property.PropertyTypeDescription).Select();
            base.ctl00MainddlTitleType.Option(property.TitleTypeDescription).Select();
            base.ctl00MainddlOccupancyType.Option(property.OccupancyTypeDescription).Select();
            base.ctl00MainddlAreaClassification.Option(property.AreaClassificationDescription).Select();
            base.ctl00MaintxtPropertyDescription1.Value = property.PropertyDescription1;
            base.ctl00MaintxtPropertyDescription2.Value = property.PropertyDescription2;
            base.ctl00MaintxtPropertyDescription3.Value = property.PropertyDescription3;
            base.ctl00MainddlDeedsPropertyType.Option(property.DeedsPropertyTypeDescription).Select();

            if (base.ctl00txtMainTitleDeedNumber.Exists)
                base.ctl00txtMainTitleDeedNumber.Value = property.TitleDeedNumber;
            if (base.ctl00MaintxtBondAccountNumber.Exists)
                base.ctl00MaintxtBondAccountNumber.Value = property.BondAccountNumber.ToString();
            if (base.ctl00MainddlDeedsOffice.Exists)
                base.ctl00MainddlDeedsOffice.Option(property.DeedsOfficeName).Select();
            if (base.ctl00MaintxtErfNumber.Exists)
                base.ctl00MaintxtErfNumber.Value = property.ErfNumber;
            if (base.ctl00MaintxtPortionNumber.Exists)
                base.ctl00MaintxtPortionNumber.Value = property.ErfPortionNumber;
            if (base.ctl00MaintxtErfSuburb.Exists)
                base.ctl00MaintxtErfSuburb.Value = property.ErfSuburbDescription;
            if (base.ctl00MaintxtErfNumber.Exists)
                base.ctl00MaintxtErfNumber.Value = property.ErfSuburbDescription;
            if (base.ctl00MaintxtErfMetroDescription.Exists)
                base.ctl00MaintxtErfMetroDescription.Value = property.ErfSuburbDescription;
            if (base.ctl00MaintxtSectionalSchemeName.Exists)
                base.ctl00MaintxtSectionalSchemeName.Value = property.SectionalSchemeName;
            if (base.ctl00MaintxtSectionalUnitNumber.Exists)
                base.ctl00MaintxtSectionalUnitNumber.Value = property.SectionalUnitNumber;
        }

        public void ClickUpdate()
        {
            base.ctl00MainbtnUpdate.Click();
        }

        public void ClickCancel()
        {
            base.ctl00MainbtnCancel.Click();
        }

        public void AssertPropertyControlsExists()
        {
            Assertions.WatiNAssertions.AssertSelectListContents(
                    base.ctl00MainddlPropertyType, new List<string>{
                            PropertyType.ClusterHome,
                            PropertyType.Duplex,
                            PropertyType.Flat,
                            PropertyType.House,
                            PropertyType.Maisonette,
                            PropertyType.Simplex,
                            PropertyType.Unknown}
                );

            Assertions.WatiNAssertions.AssertSelectListContents(
                   base.ctl00MainddlTitleType, new List<string>{
                            TitleType.FreeholdEstate,
                            TitleType.House,
                            TitleType.Leasehold,
                            TitleType.SectionalTitle,
                            TitleType.SectionalTitleWithHOC,
                            TitleType.Shareblock,
                            TitleType.Unknown}
               );

            Assertions.WatiNAssertions.AssertSelectListContents(
                 base.ctl00MainddlOccupancyType, new List<string>{
                            OccupancyType.InvestmentProperty,
                            OccupancyType.HolidayHome,
                            OccupancyType.Other,
                            OccupancyType.OwnerOccupied,
                            OccupancyType.Rental}
             );

            Assertions.WatiNAssertions.AssertSelectListContents(
               base.ctl00MainddlAreaClassification, new List<string>{
                            AreaClassification.Unknown,
                            AreaClassification._1Class,
                            AreaClassification._2Class,
                            AreaClassification._3Class,
                            AreaClassification._4Class,
                            AreaClassification._5Class,
                            AreaClassification._6Class}
            );

            Assertions.WatiNAssertions.AssertSelectListContents(
              base.ctl00MainddlDeedsPropertyType, new List<string>{
                            DeedsPropertyType.Erf,
                            DeedsPropertyType.Unit}
            );

            Assertions.WatiNAssertions.AssertFieldsExist(
                  new List<Element> {
                           base.ctl00MainddlPropertyType,
                           base.ctl00MainddlAreaClassification,
                           base.ctl00MaintxtPropertyDescription1,
                           base.ctl00MaintxtPropertyDescription2,
                           base.ctl00MaintxtPropertyDescription3,
                           base.ctl00MainTitleDeedNumber,
                           base.ctl00MainddlDeedsPropertyType,
                           base.ctl00MainBondAccountNumber,
                           base.ctl00MainDeedsOffice,
                           base.ctl00MainbtnUpdate,
                           base.ctl00MainbtnCancel}
               );
            Assertions.WatiNAssertions.AssertFieldsDoNotExist(
                new List<Element>{
                        base.ctl00txtMainTitleDeedNumber,
                        base.ctl00MaintxtErfMetroDescription,
                        base.ctl00MaintxtErfNumber,
                        base.ctl00MaintxtErfSuburb,
                        base.ctl00MaintxtPortionNumber,
                        base.ctl00MaintxtSectionalSchemeName,
                        base.ctl00MaintxtSectionalUnitNumber,
                        base.ctl00MaintxtBondAccountNumber,
                        base.ctl00MainddlDeedsOffice,
                    });

            //Inspection Section
            Assertions.WatiNAssertions.AssertFieldsExist(
                 new List<Element> {
                            base.ctl00_Main_InspectionContact,
                            base.ctl00_Main_InspectionNumber,
                            base.ctl00_Main_InspectionContact2,
                            base.ctl00_Main_InspectionNumber2,
                            base.ctl00_Main_lbCurrentDataProvider,
                            base.ctl00_Main_LightStonePropertyID,
                            base.ctl00_Main_AdCheckPropertyID
                     }
              );
        }

        public void AssertPropertyDeedsOfficeControlsExists()
        {
            Assertions.WatiNAssertions.AssertSelectListContents(
                    base.ctl00MainddlPropertyType, new List<string>{
                            PropertyType.ClusterHome,
                            PropertyType.Duplex,
                            PropertyType.Flat,
                            PropertyType.House,
                            PropertyType.Maisonette,
                            PropertyType.Simplex,
                            PropertyType.Unknown}
                );

            Assertions.WatiNAssertions.AssertSelectListContents(
                   base.ctl00MainddlTitleType, new List<string>{
                            TitleType.FreeholdEstate,
                            TitleType.House,
                            TitleType.Leasehold,
                            TitleType.SectionalTitle,
                            TitleType.SectionalTitleWithHOC,
                            TitleType.Shareblock,
                            TitleType.Unknown}
               );

            Assertions.WatiNAssertions.AssertSelectListContents(
                 base.ctl00MainddlOccupancyType, new List<string>{
                            OccupancyType.InvestmentProperty,
                            OccupancyType.HolidayHome,
                            OccupancyType.Other,
                            OccupancyType.OwnerOccupied,
                            OccupancyType.Rental}
             );

            Assertions.WatiNAssertions.AssertSelectListContents(
               base.ctl00MainddlAreaClassification, new List<string>{
                            AreaClassification.Unknown,
                            AreaClassification._1Class,
                            AreaClassification._2Class,
                            AreaClassification._3Class,
                            AreaClassification._4Class,
                            AreaClassification._5Class,
                            AreaClassification._6Class}
            );

            Assertions.WatiNAssertions.AssertSelectListContents(
              base.ctl00MainddlDeedsPropertyType, new List<string>{
                            DeedsPropertyType.Erf,
                            DeedsPropertyType.Unit}
            );

            Assertions.WatiNAssertions.AssertFieldsExist
            (
               new List<Element> {
                           base.ctl00MainddlPropertyType,
                           base.ctl00MainddlAreaClassification,
                           base.ctl00MaintxtPropertyDescription1,
                           base.ctl00MaintxtPropertyDescription2,
                           base.ctl00MaintxtPropertyDescription3,
                           base.ctl00txtMainTitleDeedNumber,
                           base.ctl00MainddlDeedsPropertyType,
                           base.ctl00MaintxtBondAccountNumber,
                           base.ctl00MainddlDeedsOffice,
                           base.ctl00MaintxtErfNumber,
                           base.ctl00MaintxtPortionNumber,
                           base.ctl00MaintxtErfSuburb,
                           base.ctl00MaintxtErfMetroDescription,
                           base.ctl00MaintxtSectionalUnitNumber,
                           base.ctl00MainbtnUpdate,
                           base.ctl00MainbtnCancel}
            );
            Assertions.WatiNAssertions.AssertFieldsDoNotExist(
             new List<Element>{
                           base.ctl00MainTitleDeedNumber,
                           base.ctl00MainErfNumber,
                           base.ctl00MainPortionNumber,
                           base.ctl00MainErfSuburb,
                           base.ctl00MainErfMetroDescription,
                           base.ctl00_Main_SectionalUnitNumber,
                           base.ctl00MainDeedsOffice
                    });

            //Inspection Section
            Assertions.WatiNAssertions.AssertFieldsExist(
                 new List<Element> {
                            base.ctl00_Main_InspectionContact,
                            base.ctl00_Main_InspectionNumber,
                            base.ctl00_Main_InspectionContact2,
                            base.ctl00_Main_InspectionNumber2,
                            base.ctl00_Main_lbCurrentDataProvider,
                            base.ctl00_Main_LightStonePropertyID,
                            base.ctl00_Main_AdCheckPropertyID
                     }
              );
        }
    }
}