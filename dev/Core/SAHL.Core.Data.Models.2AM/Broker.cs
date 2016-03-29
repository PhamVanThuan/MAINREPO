using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class BrokerDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public BrokerDataModel(string aDUserName, string fullName, string initials, string telephoneNumber, string faxNumber, string emailAddress, string password, string passwordQuestion, string passwordAnswer, int generalStatusKey, int? aDUserKey, int? brokerStatusNumber, int? brokerTypeNumber, int? brokerCommissionTrigger, double? brokerMinimumSAHL, double? brokerMinimumSCMB, decimal? brokerPercentageSAHL, decimal? brokerPercentageSCMB, double? brokerTarget)
        {
            this.ADUserName = aDUserName;
            this.FullName = fullName;
            this.Initials = initials;
            this.TelephoneNumber = telephoneNumber;
            this.FaxNumber = faxNumber;
            this.EmailAddress = emailAddress;
            this.Password = password;
            this.PasswordQuestion = passwordQuestion;
            this.PasswordAnswer = passwordAnswer;
            this.GeneralStatusKey = generalStatusKey;
            this.ADUserKey = aDUserKey;
            this.BrokerStatusNumber = brokerStatusNumber;
            this.BrokerTypeNumber = brokerTypeNumber;
            this.BrokerCommissionTrigger = brokerCommissionTrigger;
            this.BrokerMinimumSAHL = brokerMinimumSAHL;
            this.BrokerMinimumSCMB = brokerMinimumSCMB;
            this.BrokerPercentageSAHL = brokerPercentageSAHL;
            this.BrokerPercentageSCMB = brokerPercentageSCMB;
            this.BrokerTarget = brokerTarget;
		
        }
		[JsonConstructor]
        public BrokerDataModel(int brokerKey, string aDUserName, string fullName, string initials, string telephoneNumber, string faxNumber, string emailAddress, string password, string passwordQuestion, string passwordAnswer, int generalStatusKey, int? aDUserKey, int? brokerStatusNumber, int? brokerTypeNumber, int? brokerCommissionTrigger, double? brokerMinimumSAHL, double? brokerMinimumSCMB, decimal? brokerPercentageSAHL, decimal? brokerPercentageSCMB, double? brokerTarget)
        {
            this.BrokerKey = brokerKey;
            this.ADUserName = aDUserName;
            this.FullName = fullName;
            this.Initials = initials;
            this.TelephoneNumber = telephoneNumber;
            this.FaxNumber = faxNumber;
            this.EmailAddress = emailAddress;
            this.Password = password;
            this.PasswordQuestion = passwordQuestion;
            this.PasswordAnswer = passwordAnswer;
            this.GeneralStatusKey = generalStatusKey;
            this.ADUserKey = aDUserKey;
            this.BrokerStatusNumber = brokerStatusNumber;
            this.BrokerTypeNumber = brokerTypeNumber;
            this.BrokerCommissionTrigger = brokerCommissionTrigger;
            this.BrokerMinimumSAHL = brokerMinimumSAHL;
            this.BrokerMinimumSCMB = brokerMinimumSCMB;
            this.BrokerPercentageSAHL = brokerPercentageSAHL;
            this.BrokerPercentageSCMB = brokerPercentageSCMB;
            this.BrokerTarget = brokerTarget;
		
        }		

        public int BrokerKey { get; set; }

        public string ADUserName { get; set; }

        public string FullName { get; set; }

        public string Initials { get; set; }

        public string TelephoneNumber { get; set; }

        public string FaxNumber { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public string PasswordQuestion { get; set; }

        public string PasswordAnswer { get; set; }

        public int GeneralStatusKey { get; set; }

        public int? ADUserKey { get; set; }

        public int? BrokerStatusNumber { get; set; }

        public int? BrokerTypeNumber { get; set; }

        public int? BrokerCommissionTrigger { get; set; }

        public double? BrokerMinimumSAHL { get; set; }

        public double? BrokerMinimumSCMB { get; set; }

        public decimal? BrokerPercentageSAHL { get; set; }

        public decimal? BrokerPercentageSCMB { get; set; }

        public double? BrokerTarget { get; set; }

        public void SetKey(int key)
        {
            this.BrokerKey =  key;
        }
    }
}