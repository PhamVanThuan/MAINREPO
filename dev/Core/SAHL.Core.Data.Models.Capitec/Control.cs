using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class ControlDataModel :  IDataModel
    {
        public ControlDataModel(int controlNumber, string controlDescription, double? controlNumeric, string controlText)
        {
            this.ControlNumber = controlNumber;
            this.ControlDescription = controlDescription;
            this.ControlNumeric = controlNumeric;
            this.ControlText = controlText;
		
        }		

        public int ControlNumber { get; set; }

        public string ControlDescription { get; set; }

        public double? ControlNumeric { get; set; }

        public string ControlText { get; set; }
    }
}