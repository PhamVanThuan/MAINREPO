using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ValidatorDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ValidatorDataModel(string domainFieldKey, int validatorTypeKey, int errorRepositoryKey, string initialValue, string regularExpression, double? minimumValue, double? maximumValue)
        {
            this.DomainFieldKey = domainFieldKey;
            this.ValidatorTypeKey = validatorTypeKey;
            this.ErrorRepositoryKey = errorRepositoryKey;
            this.InitialValue = initialValue;
            this.RegularExpression = regularExpression;
            this.MinimumValue = minimumValue;
            this.MaximumValue = maximumValue;
		
        }
		[JsonConstructor]
        public ValidatorDataModel(int validatorKey, string domainFieldKey, int validatorTypeKey, int errorRepositoryKey, string initialValue, string regularExpression, double? minimumValue, double? maximumValue)
        {
            this.ValidatorKey = validatorKey;
            this.DomainFieldKey = domainFieldKey;
            this.ValidatorTypeKey = validatorTypeKey;
            this.ErrorRepositoryKey = errorRepositoryKey;
            this.InitialValue = initialValue;
            this.RegularExpression = regularExpression;
            this.MinimumValue = minimumValue;
            this.MaximumValue = maximumValue;
		
        }		

        public int ValidatorKey { get; set; }

        public string DomainFieldKey { get; set; }

        public int ValidatorTypeKey { get; set; }

        public int ErrorRepositoryKey { get; set; }

        public string InitialValue { get; set; }

        public string RegularExpression { get; set; }

        public double? MinimumValue { get; set; }

        public double? MaximumValue { get; set; }

        public void SetKey(int key)
        {
            this.ValidatorKey =  key;
        }
    }
}