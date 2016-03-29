using System;

namespace SAHL.Core.Validation.Attributes
{
    /// <summary>
    /// Validation attribute that ensures the string representation of the value is in the format 
    /// "dddd-dd-dd dd:dd:dd LL" on the client-side, and is "dddd-dd-dd dd:dd:dd LL", where d is a digit and LL is either AM or PM. 
    /// The format dddd-d-d is also accepted.
    /// </summary>
    public class ShouldBeInSahlDateTimeFormatAttribute : TieredReguarExpressionAttribute
    {
        //dddd-dd-dd or dddd-d-d
        private const string MatchDateTimeOnly = @"^\d{4}-\d{1,2}-\d{1,2}(\s\d{1,2}:\d{2}:\d{2}\s(A|P)M$)?";

        //dddd-dd-dd or dddd-dd-dd dd:dd:dd AM (dddd-d-d is also valid for date, and d:dd:dd is also valid for time, AM can also be PM)
        private const string MatchDateWithOptionalTime = @"^\d{4}-\d{1,2}-\d{1,2}(\s\d{1,2}:\d{2}:\d{2}\s(A|P)M$)?";

        public ShouldBeInSahlDateTimeFormatAttribute()
            : base(MatchDateTimeOnly, MatchDateWithOptionalTime)
        {
        }

        public override bool IsValid(object value)
        {
            return value is DateTime || base.IsValid(value);
        }
    }
}