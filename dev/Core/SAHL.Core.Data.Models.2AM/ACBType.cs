using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ACBTypeDataModel :  IDataModel
    {
        public ACBTypeDataModel(int aCBTypeNumber, string aCBTypeDescription)
        {
            this.ACBTypeNumber = aCBTypeNumber;
            this.ACBTypeDescription = aCBTypeDescription;
		
        }		

        public int ACBTypeNumber { get; set; }

        public string ACBTypeDescription { get; set; }
    }
}