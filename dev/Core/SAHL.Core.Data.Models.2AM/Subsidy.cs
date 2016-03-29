using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class SubsidyDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public SubsidyDataModel(int subsidyProviderKey, int employmentKey, int legalEntityKey, string salaryNumber, string paypoint, string notch, string rank, int generalStatusKey, double stopOrderAmount, bool gEPFMember)
        {
            this.SubsidyProviderKey = subsidyProviderKey;
            this.EmploymentKey = employmentKey;
            this.LegalEntityKey = legalEntityKey;
            this.SalaryNumber = salaryNumber;
            this.Paypoint = paypoint;
            this.Notch = notch;
            this.Rank = rank;
            this.GeneralStatusKey = generalStatusKey;
            this.StopOrderAmount = stopOrderAmount;
            this.GEPFMember = gEPFMember;
		
        }
		[JsonConstructor]
        public SubsidyDataModel(int subsidyKey, int subsidyProviderKey, int employmentKey, int legalEntityKey, string salaryNumber, string paypoint, string notch, string rank, int generalStatusKey, double stopOrderAmount, bool gEPFMember)
        {
            this.SubsidyKey = subsidyKey;
            this.SubsidyProviderKey = subsidyProviderKey;
            this.EmploymentKey = employmentKey;
            this.LegalEntityKey = legalEntityKey;
            this.SalaryNumber = salaryNumber;
            this.Paypoint = paypoint;
            this.Notch = notch;
            this.Rank = rank;
            this.GeneralStatusKey = generalStatusKey;
            this.StopOrderAmount = stopOrderAmount;
            this.GEPFMember = gEPFMember;
		
        }		

        public int SubsidyKey { get; set; }

        public int SubsidyProviderKey { get; set; }

        public int EmploymentKey { get; set; }

        public int LegalEntityKey { get; set; }

        public string SalaryNumber { get; set; }

        public string Paypoint { get; set; }

        public string Notch { get; set; }

        public string Rank { get; set; }

        public int GeneralStatusKey { get; set; }

        public double StopOrderAmount { get; set; }

        public bool GEPFMember { get; set; }

        public void SetKey(int key)
        {
            this.SubsidyKey =  key;
        }
    }
}