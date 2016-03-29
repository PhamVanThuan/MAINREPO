using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class XSLTransformationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public XSLTransformationDataModel(int genericKeyTypeKey, string styleSheet, int version)
        {
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.StyleSheet = styleSheet;
            this.Version = version;
		
        }
		[JsonConstructor]
        public XSLTransformationDataModel(int xSLTransformationKey, int genericKeyTypeKey, string styleSheet, int version)
        {
            this.XSLTransformationKey = xSLTransformationKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.StyleSheet = styleSheet;
            this.Version = version;
		
        }		

        public int XSLTransformationKey { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public string StyleSheet { get; set; }

        public int Version { get; set; }

        public void SetKey(int key)
        {
            this.XSLTransformationKey =  key;
        }
    }
}