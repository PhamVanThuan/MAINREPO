using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class TextStatementTypeDataModel :  IDataModel
    {
        public TextStatementTypeDataModel(int textStatementTypeKey, int originationSourceProductKey, string description)
        {
            this.TextStatementTypeKey = textStatementTypeKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.Description = description;
		
        }		

        public int TextStatementTypeKey { get; set; }

        public int OriginationSourceProductKey { get; set; }

        public string Description { get; set; }
    }
}