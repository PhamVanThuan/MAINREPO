using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.FETest
{
    [Serializable]
    public partial class LossControlAttorneyInvoiceDocumentsDataModel :  IDataModel
    {
        public LossControlAttorneyInvoiceDocumentsDataModel(decimal sTOR, string gUID)
        {
            this.STOR = sTOR;
            this.GUID = gUID;
		
        }
		[JsonConstructor]
        public LossControlAttorneyInvoiceDocumentsDataModel(decimal iD, decimal sTOR, string gUID)
        {
            this.ID = iD;
            this.STOR = sTOR;
            this.GUID = gUID;
		
        }		

        public decimal ID { get; set; }

        public decimal STOR { get; set; }

        public string GUID { get; set; }
    }
}