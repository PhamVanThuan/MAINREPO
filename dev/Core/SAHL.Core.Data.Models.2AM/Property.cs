using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class PropertyDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public PropertyDataModel(int? propertyTypeKey, int? titleTypeKey, int? areaClassificationKey, int? occupancyTypeKey, int? addressKey, string propertyDescription1, string propertyDescription2, string propertyDescription3, double? deedsOfficeValue, DateTime? currentBondDate, string erfNumber, string erfPortionNumber, string sectionalSchemeName, string sectionalUnitNumber, int? deedsPropertyTypeKey, string erfSuburbDescription, string erfMetroDescription, int? dataProviderKey)
        {
            this.PropertyTypeKey = propertyTypeKey;
            this.TitleTypeKey = titleTypeKey;
            this.AreaClassificationKey = areaClassificationKey;
            this.OccupancyTypeKey = occupancyTypeKey;
            this.AddressKey = addressKey;
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
            this.DataProviderKey = dataProviderKey;
		
        }
		[JsonConstructor]
        public PropertyDataModel(int propertyKey, int? propertyTypeKey, int? titleTypeKey, int? areaClassificationKey, int? occupancyTypeKey, int? addressKey, string propertyDescription1, string propertyDescription2, string propertyDescription3, double? deedsOfficeValue, DateTime? currentBondDate, string erfNumber, string erfPortionNumber, string sectionalSchemeName, string sectionalUnitNumber, int? deedsPropertyTypeKey, string erfSuburbDescription, string erfMetroDescription, int? dataProviderKey)
        {
            this.PropertyKey = propertyKey;
            this.PropertyTypeKey = propertyTypeKey;
            this.TitleTypeKey = titleTypeKey;
            this.AreaClassificationKey = areaClassificationKey;
            this.OccupancyTypeKey = occupancyTypeKey;
            this.AddressKey = addressKey;
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
            this.DataProviderKey = dataProviderKey;
		
        }		

        public int PropertyKey { get; set; }

        public int? PropertyTypeKey { get; set; }

        public int? TitleTypeKey { get; set; }

        public int? AreaClassificationKey { get; set; }

        public int? OccupancyTypeKey { get; set; }

        public int? AddressKey { get; set; }

        public string PropertyDescription1 { get; set; }

        public string PropertyDescription2 { get; set; }

        public string PropertyDescription3 { get; set; }

        public double? DeedsOfficeValue { get; set; }

        public DateTime? CurrentBondDate { get; set; }

        public string ErfNumber { get; set; }

        public string ErfPortionNumber { get; set; }

        public string SectionalSchemeName { get; set; }

        public string SectionalUnitNumber { get; set; }

        public int? DeedsPropertyTypeKey { get; set; }

        public string ErfSuburbDescription { get; set; }

        public string ErfMetroDescription { get; set; }

        public int? DataProviderKey { get; set; }

        public void SetKey(int key)
        {
            this.PropertyKey =  key;
        }
    }
}