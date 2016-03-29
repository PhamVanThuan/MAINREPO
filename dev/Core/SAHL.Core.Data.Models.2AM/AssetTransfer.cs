using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AssetTransferDataModel :  IDataModel
    {
        public AssetTransferDataModel(int accountKey, string clientSurname, int? sPVKey, double? loanTotalBondAmount, double? loanCurrentBalance, string userName, string transferedYN)
        {
            this.AccountKey = accountKey;
            this.ClientSurname = clientSurname;
            this.SPVKey = sPVKey;
            this.LoanTotalBondAmount = loanTotalBondAmount;
            this.LoanCurrentBalance = loanCurrentBalance;
            this.UserName = userName;
            this.TransferedYN = transferedYN;
		
        }		

        public int AccountKey { get; set; }

        public string ClientSurname { get; set; }

        public int? SPVKey { get; set; }

        public double? LoanTotalBondAmount { get; set; }

        public double? LoanCurrentBalance { get; set; }

        public string UserName { get; set; }

        public string TransferedYN { get; set; }
    }
}