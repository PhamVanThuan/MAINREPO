using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.FETest
{
    [Serializable]
    public partial class OpenNewBusinessApplicationsDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OpenNewBusinessApplicationsDataModel(int offerKey, double lTV, int? sPVKey, int? propertyKey, bool? hasDebitOrder, bool? hasMailingAddress, bool? hasProperty, bool? isAccepted, double? householdIncome, int? employmentTypeKey)
        {
            this.OfferKey = offerKey;
            this.LTV = lTV;
            this.SPVKey = sPVKey;
            this.PropertyKey = propertyKey;
            this.HasDebitOrder = hasDebitOrder;
            this.HasMailingAddress = hasMailingAddress;
            this.HasProperty = hasProperty;
            this.IsAccepted = isAccepted;
            this.HouseholdIncome = householdIncome;
            this.EmploymentTypeKey = employmentTypeKey;
		
        }
		[JsonConstructor]
        public OpenNewBusinessApplicationsDataModel(int id, int offerKey, double lTV, int? sPVKey, int? propertyKey, bool? hasDebitOrder, bool? hasMailingAddress, bool? hasProperty, bool? isAccepted, double? householdIncome, int? employmentTypeKey)
        {
            this.Id = id;
            this.OfferKey = offerKey;
            this.LTV = lTV;
            this.SPVKey = sPVKey;
            this.PropertyKey = propertyKey;
            this.HasDebitOrder = hasDebitOrder;
            this.HasMailingAddress = hasMailingAddress;
            this.HasProperty = hasProperty;
            this.IsAccepted = isAccepted;
            this.HouseholdIncome = householdIncome;
            this.EmploymentTypeKey = employmentTypeKey;
		
        }		

        public int Id { get; set; }

        public int OfferKey { get; set; }

        public double LTV { get; set; }

        public int? SPVKey { get; set; }

        public int? PropertyKey { get; set; }

        public bool? HasDebitOrder { get; set; }

        public bool? HasMailingAddress { get; set; }

        public bool? HasProperty { get; set; }

        public bool? IsAccepted { get; set; }

        public double? HouseholdIncome { get; set; }

        public int? EmploymentTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}