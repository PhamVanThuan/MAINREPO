using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CATSPaymentBatchTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CATSPaymentBatchTypeDataModel(string description, string cATSProfile, string cATSFileNamePrefix, int cATSEnvironment, int nextCATSFileSequenceNo)
        {
            this.Description = description;
            this.CATSProfile = cATSProfile;
            this.CATSFileNamePrefix = cATSFileNamePrefix;
            this.CATSEnvironment = cATSEnvironment;
            this.NextCATSFileSequenceNo = nextCATSFileSequenceNo;
		
        }
		[JsonConstructor]
        public CATSPaymentBatchTypeDataModel(int cATSPaymentBatchTypeKey, string description, string cATSProfile, string cATSFileNamePrefix, int cATSEnvironment, int nextCATSFileSequenceNo)
        {
            this.CATSPaymentBatchTypeKey = cATSPaymentBatchTypeKey;
            this.Description = description;
            this.CATSProfile = cATSProfile;
            this.CATSFileNamePrefix = cATSFileNamePrefix;
            this.CATSEnvironment = cATSEnvironment;
            this.NextCATSFileSequenceNo = nextCATSFileSequenceNo;
		
        }		

        public int CATSPaymentBatchTypeKey { get; set; }

        public string Description { get; set; }

        public string CATSProfile { get; set; }

        public string CATSFileNamePrefix { get; set; }

        public int CATSEnvironment { get; set; }

        public int NextCATSFileSequenceNo { get; set; }

        public void SetKey(int key)
        {
            this.CATSPaymentBatchTypeKey =  key;
        }
    }
}