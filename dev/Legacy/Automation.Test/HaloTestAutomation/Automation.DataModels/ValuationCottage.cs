using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class ValuationCottage : IComparable<ValuationCottage>
    {
        public int ValuationCottageValuationKey { get; set; }

        public int ValuationKey { get; set; }

        public ValuationRoofTypeEnum ValuationRoofTypeKey { get; set; }

        public string ValuationRoofTypeDescription { get; set; }

        public double Extent { get; set; }

        public double Rate { get; set; }

        public int CompareTo(ValuationCottage other)
        {
            throw new NotImplementedException();
        }
    }
}