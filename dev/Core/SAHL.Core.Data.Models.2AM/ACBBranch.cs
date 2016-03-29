using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ACBBranchDataModel :  IDataModel
    {
        public ACBBranchDataModel(string aCBBranchCode, int aCBBankCode, string aCBBranchDescription, string activeIndicator)
        {
            this.ACBBranchCode = aCBBranchCode;
            this.ACBBankCode = aCBBankCode;
            this.ACBBranchDescription = aCBBranchDescription;
            this.ActiveIndicator = activeIndicator;
		
        }		

        public string ACBBranchCode { get; set; }

        public int ACBBankCode { get; set; }

        public string ACBBranchDescription { get; set; }

        public string ActiveIndicator { get; set; }
    }
}