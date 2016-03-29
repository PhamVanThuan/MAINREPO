using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AccountBaselIIDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AccountBaselIIDataModel(int accountKey, DateTime? accountingDate, DateTime? processDate, double? lGD, double? eAD, double? behaviouralScore, double? pD, int? eL)
        {
            this.AccountKey = accountKey;
            this.AccountingDate = accountingDate;
            this.ProcessDate = processDate;
            this.LGD = lGD;
            this.EAD = eAD;
            this.BehaviouralScore = behaviouralScore;
            this.PD = pD;
            this.EL = eL;
		
        }
		[JsonConstructor]
        public AccountBaselIIDataModel(int accountBaselIIKey, int accountKey, DateTime? accountingDate, DateTime? processDate, double? lGD, double? eAD, double? behaviouralScore, double? pD, int? eL)
        {
            this.AccountBaselIIKey = accountBaselIIKey;
            this.AccountKey = accountKey;
            this.AccountingDate = accountingDate;
            this.ProcessDate = processDate;
            this.LGD = lGD;
            this.EAD = eAD;
            this.BehaviouralScore = behaviouralScore;
            this.PD = pD;
            this.EL = eL;
		
        }		

        public int AccountBaselIIKey { get; set; }

        public int AccountKey { get; set; }

        public DateTime? AccountingDate { get; set; }

        public DateTime? ProcessDate { get; set; }

        public double? LGD { get; set; }

        public double? EAD { get; set; }

        public double? BehaviouralScore { get; set; }

        public double? PD { get; set; }

        public int? EL { get; set; }

        public void SetKey(int key)
        {
            this.AccountBaselIIKey =  key;
        }
    }
}