using System;

namespace Automation.DataModels
{
    public class ValuationCombinedThatch : IComparable<ValuationCombinedThatch>
    {
        public int CombinedThatchValuationKey { get; set; }

        public int ValuationKey { get; set; }

        public double Value { get; set; }

        public int CompareTo(ValuationCombinedThatch other)
        {
            throw new NotImplementedException();
        }
    }
}