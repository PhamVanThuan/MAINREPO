using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class XMLHistoryDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public XMLHistoryDataModel(int genericKeyTypeKey, int genericKey, string xMLData, DateTime insertDate)
        {
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GenericKey = genericKey;
            this.XMLData = xMLData;
            this.InsertDate = insertDate;
		
        }
		[JsonConstructor]
        public XMLHistoryDataModel(int xMLHistoryKey, int genericKeyTypeKey, int genericKey, string xMLData, DateTime insertDate)
        {
            this.XMLHistoryKey = xMLHistoryKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GenericKey = genericKey;
            this.XMLData = xMLData;
            this.InsertDate = insertDate;
		
        }		

        public int XMLHistoryKey { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public int GenericKey { get; set; }

        public string XMLData { get; set; }

        public DateTime InsertDate { get; set; }

        public void SetKey(int key)
        {
            this.XMLHistoryKey =  key;
        }
    }
}