using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ACBBankDataModel :  IDataModel
    {
        public ACBBankDataModel(int aCBBankCode, string aCBBankDescription)
        {
            this.ACBBankCode = aCBBankCode;
            this.ACBBankDescription = aCBBankDescription;
		
        }		

        public int ACBBankCode { get; set; }

        public string ACBBankDescription { get; set; }
    }
}