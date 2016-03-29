using System;
using System.Runtime.Serialization;

using SAHL.Core.Data;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class PropertyModel : IDataModel
    {
        public PropertyModel(PropertyType propertyType, TitleType titleType, int areaClassificationKey, OccupancyType occupancyType, ResidentialAddressModel address,
                             string propertyDescription1, string propertyDescription2, string propertyDescription3, double? deedsOfficeValue, DateTime? currentBondDate,
                             string erfNumber, string erfPortionNumber, string sectionalSchemeName, string sectionalUnitNumber, DeedsPropertyType? deedsPropertyType,
                             string erfSuburbDescription, string erfMetroDescription, DataProvider? dataProvider)
        {
            this.PropertyType          = propertyType;
            this.TitleType             = titleType;
            this.AreaClassificationKey = areaClassificationKey;
            this.OccupancyType         = occupancyType;
            this.Address               = address;

            this.PropertyDescription1 = propertyDescription1;
            this.PropertyDescription2 = propertyDescription2;
            this.PropertyDescription3 = propertyDescription3;

            this.DeedsOfficeValue    = deedsOfficeValue;
            this.CurrentBondDate     = currentBondDate;
            this.ErfNumber           = erfNumber;
            this.ErfPortionNumber    = erfPortionNumber;
            this.SectionalSchemeName = sectionalSchemeName;
            this.SectionalUnitNumber = sectionalUnitNumber;

            this.DeedsPropertyType    = deedsPropertyType;
            this.ErfSuburbDescription = erfSuburbDescription;
            this.ErfMetroDescription  = erfMetroDescription;
            this.DataProvider         = dataProvider;
        }

        [DataMember]
        public PropertyType PropertyType { get; set; }

        [DataMember]
        public TitleType TitleType { get; set; }

        [DataMember]
        public int AreaClassificationKey { get; set; }

        [DataMember]
        public OccupancyType OccupancyType { get; set; }

        [DataMember]
        public ResidentialAddressModel Address { get; set; }

        [DataMember]
        public string PropertyDescription1 { get; set; }

        [DataMember]
        public string PropertyDescription2 { get; set; }

        [DataMember]
        public string PropertyDescription3 { get; set; }

        [DataMember]
        public double? DeedsOfficeValue { get; set; }

        [DataMember]
        public DateTime? CurrentBondDate { get; set; }

        [DataMember]
        public string ErfNumber { get; set; }

        [DataMember]
        public string ErfPortionNumber { get; set; }

        [DataMember]
        public string SectionalSchemeName { get; set; }

        [DataMember]
        public string SectionalUnitNumber { get; set; }

        [DataMember]
        public DeedsPropertyType? DeedsPropertyType { get; set; }

        [DataMember]
        public string ErfSuburbDescription { get; set; }

        [DataMember]
        public string ErfMetroDescription { get; set; }

        [DataMember]
        public DataProvider? DataProvider { get; set; }
    }
}
