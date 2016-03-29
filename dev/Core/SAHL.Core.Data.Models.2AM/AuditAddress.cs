using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditAddressDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditAddressDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int addressKey, int? addressFormatKey, string boxNumber, string unitNumber, string buildingNumber, string buildingName, string streetNumber, string streetName, int? suburbKey, int? postOfficeKey, string rRR_CountryDescription, string rRR_ProvinceDescription, string rRR_CityDescription, string rRR_SuburbDescription, string rRR_PostalCode, string userID, DateTime? changeDate, string suiteNumber, string freeText1, string freeText2, string freeText3, string freeText4, string freeText5)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.AddressKey = addressKey;
            this.AddressFormatKey = addressFormatKey;
            this.BoxNumber = boxNumber;
            this.UnitNumber = unitNumber;
            this.BuildingNumber = buildingNumber;
            this.BuildingName = buildingName;
            this.StreetNumber = streetNumber;
            this.StreetName = streetName;
            this.SuburbKey = suburbKey;
            this.PostOfficeKey = postOfficeKey;
            this.RRR_CountryDescription = rRR_CountryDescription;
            this.RRR_ProvinceDescription = rRR_ProvinceDescription;
            this.RRR_CityDescription = rRR_CityDescription;
            this.RRR_SuburbDescription = rRR_SuburbDescription;
            this.RRR_PostalCode = rRR_PostalCode;
            this.UserID = userID;
            this.ChangeDate = changeDate;
            this.SuiteNumber = suiteNumber;
            this.FreeText1 = freeText1;
            this.FreeText2 = freeText2;
            this.FreeText3 = freeText3;
            this.FreeText4 = freeText4;
            this.FreeText5 = freeText5;
		
        }
		[JsonConstructor]
        public AuditAddressDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int addressKey, int? addressFormatKey, string boxNumber, string unitNumber, string buildingNumber, string buildingName, string streetNumber, string streetName, int? suburbKey, int? postOfficeKey, string rRR_CountryDescription, string rRR_ProvinceDescription, string rRR_CityDescription, string rRR_SuburbDescription, string rRR_PostalCode, string userID, DateTime? changeDate, string suiteNumber, string freeText1, string freeText2, string freeText3, string freeText4, string freeText5)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.AddressKey = addressKey;
            this.AddressFormatKey = addressFormatKey;
            this.BoxNumber = boxNumber;
            this.UnitNumber = unitNumber;
            this.BuildingNumber = buildingNumber;
            this.BuildingName = buildingName;
            this.StreetNumber = streetNumber;
            this.StreetName = streetName;
            this.SuburbKey = suburbKey;
            this.PostOfficeKey = postOfficeKey;
            this.RRR_CountryDescription = rRR_CountryDescription;
            this.RRR_ProvinceDescription = rRR_ProvinceDescription;
            this.RRR_CityDescription = rRR_CityDescription;
            this.RRR_SuburbDescription = rRR_SuburbDescription;
            this.RRR_PostalCode = rRR_PostalCode;
            this.UserID = userID;
            this.ChangeDate = changeDate;
            this.SuiteNumber = suiteNumber;
            this.FreeText1 = freeText1;
            this.FreeText2 = freeText2;
            this.FreeText3 = freeText3;
            this.FreeText4 = freeText4;
            this.FreeText5 = freeText5;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int AddressKey { get; set; }

        public int? AddressFormatKey { get; set; }

        public string BoxNumber { get; set; }

        public string UnitNumber { get; set; }

        public string BuildingNumber { get; set; }

        public string BuildingName { get; set; }

        public string StreetNumber { get; set; }

        public string StreetName { get; set; }

        public int? SuburbKey { get; set; }

        public int? PostOfficeKey { get; set; }

        public string RRR_CountryDescription { get; set; }

        public string RRR_ProvinceDescription { get; set; }

        public string RRR_CityDescription { get; set; }

        public string RRR_SuburbDescription { get; set; }

        public string RRR_PostalCode { get; set; }

        public string UserID { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string SuiteNumber { get; set; }

        public string FreeText1 { get; set; }

        public string FreeText2 { get; set; }

        public string FreeText3 { get; set; }

        public string FreeText4 { get; set; }

        public string FreeText5 { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}