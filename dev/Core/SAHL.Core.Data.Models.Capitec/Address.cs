using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class AddressDataModel :  IDataModel
    {
        public AddressDataModel(Guid addressFormatEnumId, string boxNumber, string unitNumber, string buildingNumber, string buildingName, string streetNumber, string streetName, Guid? suburbId, Guid? postOfficeId, string suiteNumber, string freeText1, string freeText2, string freeText3, string freeText4, string freeText5)
        {
            this.AddressFormatEnumId = addressFormatEnumId;
            this.BoxNumber = boxNumber;
            this.UnitNumber = unitNumber;
            this.BuildingNumber = buildingNumber;
            this.BuildingName = buildingName;
            this.StreetNumber = streetNumber;
            this.StreetName = streetName;
            this.SuburbId = suburbId;
            this.PostOfficeId = postOfficeId;
            this.SuiteNumber = suiteNumber;
            this.FreeText1 = freeText1;
            this.FreeText2 = freeText2;
            this.FreeText3 = freeText3;
            this.FreeText4 = freeText4;
            this.FreeText5 = freeText5;
		
        }

        public AddressDataModel(Guid id, Guid addressFormatEnumId, string boxNumber, string unitNumber, string buildingNumber, string buildingName, string streetNumber, string streetName, Guid? suburbId, Guid? postOfficeId, string suiteNumber, string freeText1, string freeText2, string freeText3, string freeText4, string freeText5)
        {
            this.Id = id;
            this.AddressFormatEnumId = addressFormatEnumId;
            this.BoxNumber = boxNumber;
            this.UnitNumber = unitNumber;
            this.BuildingNumber = buildingNumber;
            this.BuildingName = buildingName;
            this.StreetNumber = streetNumber;
            this.StreetName = streetName;
            this.SuburbId = suburbId;
            this.PostOfficeId = postOfficeId;
            this.SuiteNumber = suiteNumber;
            this.FreeText1 = freeText1;
            this.FreeText2 = freeText2;
            this.FreeText3 = freeText3;
            this.FreeText4 = freeText4;
            this.FreeText5 = freeText5;
		
        }		

        public Guid Id { get; set; }

        public Guid AddressFormatEnumId { get; set; }

        public string BoxNumber { get; set; }

        public string UnitNumber { get; set; }

        public string BuildingNumber { get; set; }

        public string BuildingName { get; set; }

        public string StreetNumber { get; set; }

        public string StreetName { get; set; }

        public Guid? SuburbId { get; set; }

        public Guid? PostOfficeId { get; set; }

        public string SuiteNumber { get; set; }

        public string FreeText1 { get; set; }

        public string FreeText2 { get; set; }

        public string FreeText3 { get; set; }

        public string FreeText4 { get; set; }

        public string FreeText5 { get; set; }
    }
}