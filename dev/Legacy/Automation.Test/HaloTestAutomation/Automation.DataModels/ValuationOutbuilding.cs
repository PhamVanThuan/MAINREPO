using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class ValuationOutbuilding : IComparable<ValuationOutbuilding>
    {
        public int ValuationOutbuildingValuationKey { get; set; }

        public int ValuationKey { get; set; }

        public ValuationRoofTypeEnum ValuationRoofTypeKey { get; set; }

        public string ValuationRoofTypeDescription { get; set; }

        public double Extent { get; set; }

        public double Rate { get; set; }

        public int CompareTo(ValuationOutbuilding other)
        {
            throw new NotImplementedException();
        }
    }
}