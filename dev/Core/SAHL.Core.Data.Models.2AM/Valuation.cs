using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ValuationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ValuationDataModel(int? valuatorKey, DateTime? valuationDate, double? valuationAmount, double? valuationHOCValue, double? valuationMunicipal, string valuationUserID, int propertyKey, double? hOCThatchAmount, double? hOCConventionalAmount, double? hOCShingleAmount, DateTime? changeDate, int? valuationClassificationKey, double? valuationEscalationPercentage, int valuationStatusKey, string data, int valuationDataProviderDataServiceKey, bool isActive, int? hOCRoofKey)
        {
            this.ValuatorKey = valuatorKey;
            this.ValuationDate = valuationDate;
            this.ValuationAmount = valuationAmount;
            this.ValuationHOCValue = valuationHOCValue;
            this.ValuationMunicipal = valuationMunicipal;
            this.ValuationUserID = valuationUserID;
            this.PropertyKey = propertyKey;
            this.HOCThatchAmount = hOCThatchAmount;
            this.HOCConventionalAmount = hOCConventionalAmount;
            this.HOCShingleAmount = hOCShingleAmount;
            this.ChangeDate = changeDate;
            this.ValuationClassificationKey = valuationClassificationKey;
            this.ValuationEscalationPercentage = valuationEscalationPercentage;
            this.ValuationStatusKey = valuationStatusKey;
            this.Data = data;
            this.ValuationDataProviderDataServiceKey = valuationDataProviderDataServiceKey;
            this.IsActive = isActive;
            this.HOCRoofKey = hOCRoofKey;
		
        }
		[JsonConstructor]
        public ValuationDataModel(int valuationKey, int? valuatorKey, DateTime? valuationDate, double? valuationAmount, double? valuationHOCValue, double? valuationMunicipal, string valuationUserID, int propertyKey, double? hOCThatchAmount, double? hOCConventionalAmount, double? hOCShingleAmount, DateTime? changeDate, int? valuationClassificationKey, double? valuationEscalationPercentage, int valuationStatusKey, string data, int valuationDataProviderDataServiceKey, bool isActive, int? hOCRoofKey)
        {
            this.ValuationKey = valuationKey;
            this.ValuatorKey = valuatorKey;
            this.ValuationDate = valuationDate;
            this.ValuationAmount = valuationAmount;
            this.ValuationHOCValue = valuationHOCValue;
            this.ValuationMunicipal = valuationMunicipal;
            this.ValuationUserID = valuationUserID;
            this.PropertyKey = propertyKey;
            this.HOCThatchAmount = hOCThatchAmount;
            this.HOCConventionalAmount = hOCConventionalAmount;
            this.HOCShingleAmount = hOCShingleAmount;
            this.ChangeDate = changeDate;
            this.ValuationClassificationKey = valuationClassificationKey;
            this.ValuationEscalationPercentage = valuationEscalationPercentage;
            this.ValuationStatusKey = valuationStatusKey;
            this.Data = data;
            this.ValuationDataProviderDataServiceKey = valuationDataProviderDataServiceKey;
            this.IsActive = isActive;
            this.HOCRoofKey = hOCRoofKey;
		
        }		

        public int ValuationKey { get; set; }

        public int? ValuatorKey { get; set; }

        public DateTime? ValuationDate { get; set; }

        public double? ValuationAmount { get; set; }

        public double? ValuationHOCValue { get; set; }

        public double? ValuationMunicipal { get; set; }

        public string ValuationUserID { get; set; }

        public int PropertyKey { get; set; }

        public double? HOCThatchAmount { get; set; }

        public double? HOCConventionalAmount { get; set; }

        public double? HOCShingleAmount { get; set; }

        public DateTime? ChangeDate { get; set; }

        public int? ValuationClassificationKey { get; set; }

        public double? ValuationEscalationPercentage { get; set; }

        public int ValuationStatusKey { get; set; }

        public string Data { get; set; }

        public int ValuationDataProviderDataServiceKey { get; set; }

        public bool IsActive { get; set; }

        public int? HOCRoofKey { get; set; }

        public void SetKey(int key)
        {
            this.ValuationKey =  key;
        }
    }
}