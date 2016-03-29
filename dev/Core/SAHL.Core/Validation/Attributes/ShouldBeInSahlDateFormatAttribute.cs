using System;

namespace SAHL.Core.Validation.Attributes
{
    /// <summary>
    /// Validation attribute that ensures the string representation of the value is in the format "dddd-dd-dd" on the client-side, 
    /// and is "dddd-dd-dd dd:dd:dd LL", where d is a digit and LL is either AM or PM. The format dddd-d-d is also accepted.
    /// </summary>
    public class ShouldBeInSahlDateFormatAttribute : TieredReguarExpressionAttribute
    {
        //dddd-dd-dd or dddd-d-d
        private const string MatchDateOnly = @"^\d{4}-\d{1,2}-\d{1,2}$";

        //dddd-dd-dd or dddd-dd-dd 12:00:00 AM (dddd-dd-dd can also be dddd-d-d)
        private const string MatchDateWithOptionalMidnightTime = @"^\d{4}-\d{1,2}-\d{1,2}(\s12:00:00\sAM$)?";

        public ShouldBeInSahlDateFormatAttribute()
            : base(MatchDateOnly, MatchDateWithOptionalMidnightTime)
        {
        }

        public override bool IsValid(object value)
        {
            var isDate = value is DateTime;
            if (!isDate)
            {
                return base.IsValid(value);
            }

            var date = (DateTime)value;
            return date.Hour == 0 && date.Minute == 0 && date.Second == 0 && date.Millisecond == 0;
        }
    }
}