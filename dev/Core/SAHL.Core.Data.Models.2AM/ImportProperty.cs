using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ImportPropertyDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ImportPropertyDataModel(int offerKey, string propertyTypeKey, string titleTypeKey, string areaClassificationKey, string occupancyTypeKey, string propertyDescription1, string propertyDescription2, string propertyDescription3, double? deedsOfficeValue, DateTime? currentBondDate, string erfNumber, string erfPortionNumber, string sectionalSchemeName, string sectionalUnitNumber, string deedsPropertyTypeKey, string erfSuburbDescription, string erfMetroDescription, string titleDeedNumber)
        {
            this.OfferKey = offerKey;
            this.PropertyTypeKey = propertyTypeKey;
            this.TitleTypeKey = titleTypeKey;
            this.AreaClassificationKey = areaClassificationKey;
            this.OccupancyTypeKey = occupancyTypeKey;
            this.PropertyDescription1 = propertyDescription1;
            this.PropertyDescription2 = propertyDescription2;
            this.PropertyDescription3 = propertyDescription3;
            this.DeedsOfficeValue = deedsOfficeValue;
            this.CurrentBondDate = currentBondDate;
            this.ErfNumber = erfNumber;
            this.ErfPortionNumber = erfPortionNumber;
            this.SectionalSchemeName = sectionalSchemeName;
            this.SectionalUnitNumber = sectionalUnitNumber;
            this.DeedsPropertyTypeKey = deedsPropertyTypeKey;
            this.ErfSuburbDescription = erfSuburbDescription;
            this.ErfMetroDescription = erfMetroDescription;
            this.TitleDeedNumber = titleDeedNumber;
		
        }
		[JsonConstructor]
        public ImportPropertyDataModel(int propertyKey, int offerKey, string propertyTypeKey, string titleTypeKey, string areaClassificationKey, string occupancyTypeKey, string propertyDescription1, string propertyDescription2, string propertyDescription3, double? deedsOfficeValue, DateTime? currentBondDate, string erfNumber, string erfPortionNumber, string sectionalSchemeName, string sectionalUnitNumber, string deedsPropertyTypeKey, string erfSuburbDescription, string erfMetroDescription, string titleDeedNumber)
        {
            this.PropertyKey = propertyKey;
            this.OfferKey = offerKey;
            this.PropertyTypeKey = propertyTypeKey;
            this.TitleTypeKey = titleTypeKey;
            this.AreaClassificationKey = areaClassificationKey;
            this.OccupancyTypeKey = occupancyTypeKey;
            this.PropertyDescription1 = propertyDescription1;
            this.PropertyDescription2 = propertyDescription2;
            this.PropertyDescription3 = propertyDescription3;
            this.DeedsOfficeValue = deedsOfficeValue;
            this.CurrentBondDate = currentBondDate;
            this.ErfNumber = erfNumber;
            this.ErfPortionNumber = erfPortionNumber;
            this.SectionalSchemeName = sectionalSchemeName;
            this.SectionalUnitNumber = sectionalUnitNumber;
            this.DeedsPropertyTypeKey = deedsPropertyTypeKey;
            this.ErfSuburbDescription = erfSuburbDescription;
            this.ErfMetroDescription = erfMetroDescription;
            this.TitleDeedNumber = titleDeedNumber;
		
        }		

        public int PropertyKey { get; set; }

        public int OfferKey { get; set; }

        public string PropertyTypeKey { get; set; }

        public string TitleTypeKey { get; set; }

        public string AreaClassificationKey { get; set; }

        public string OccupancyTypeKey { get; set; }

        public string PropertyDescription1 { get; set; }

        public string PropertyDescription2 { get; set; }

        public string PropertyDescription3 { get; set; }

        public double? DeedsOfficeValue { get; set; }

        public DateTime? CurrentBondDate { get; set; }

        public string ErfNumber { get; set; }

        public string ErfPortionNumber { get; set; }

        public string SectionalSchemeName { get; set; }

        public string SectionalUnitNumber { get; set; }

        public string DeedsPropertyTypeKey { get; set; }

        public string ErfSuburbDescription { get; set; }

        public string ErfMetroDescription { get; set; }

        public string TitleDeedNumber { get; set; }

        public void SetKey(int key)
        {
            this.PropertyKey =  key;
        }
    }
}