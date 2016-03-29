using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ControlDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ControlDataModel(string controlDescription, double? controlNumeric, string controlText, int? controlGroupKey)
        {
            this.ControlDescription = controlDescription;
            this.ControlNumeric = controlNumeric;
            this.ControlText = controlText;
            this.ControlGroupKey = controlGroupKey;
		
        }
		[JsonConstructor]
        public ControlDataModel(int controlNumber, string controlDescription, double? controlNumeric, string controlText, int? controlGroupKey)
        {
            this.ControlNumber = controlNumber;
            this.ControlDescription = controlDescription;
            this.ControlNumeric = controlNumeric;
            this.ControlText = controlText;
            this.ControlGroupKey = controlGroupKey;
		
        }		

        public int ControlNumber { get; set; }

        public string ControlDescription { get; set; }

        public double? ControlNumeric { get; set; }

        public string ControlText { get; set; }

        public int? ControlGroupKey { get; set; }

        public void SetKey(int key)
        {
            this.ControlNumber =  key;
        }
    }
}