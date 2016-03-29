using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ComcorpRequestDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ComcorpRequestDataModel(string xmlRequest, DateTime requestDate, string xmlResponse, DateTime? responseDate)
        {
            this.XmlRequest = xmlRequest;
            this.RequestDate = requestDate;
            this.XmlResponse = xmlResponse;
            this.ResponseDate = responseDate;
		
        }
		[JsonConstructor]
        public ComcorpRequestDataModel(int comcorpRequestKey, string xmlRequest, DateTime requestDate, string xmlResponse, DateTime? responseDate)
        {
            this.ComcorpRequestKey = comcorpRequestKey;
            this.XmlRequest = xmlRequest;
            this.RequestDate = requestDate;
            this.XmlResponse = xmlResponse;
            this.ResponseDate = responseDate;
		
        }		

        public int ComcorpRequestKey { get; set; }

        public string XmlRequest { get; set; }

        public DateTime RequestDate { get; set; }

        public string XmlResponse { get; set; }

        public DateTime? ResponseDate { get; set; }

        public void SetKey(int key)
        {
            this.ComcorpRequestKey =  key;
        }
    }
}