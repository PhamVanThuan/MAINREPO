namespace SAHL.Core.DataType
{
    /// <summary>
    /// DataType formats for use with string.Format()
    /// </summary>
    public static class Formats
    {
        /// <summary>
        /// Format strings for the DateTime type
        /// </summary>
        public static class Date
        {
            public const string Default = Iso;

            public const string Iso = "{0:yyyy-MM-dd}";
        }

        /// <summary>
        /// Format strings for the DateTime type
        /// </summary>
        public static class DateTime
        {
            public const string Default = IsoUpToMinute;

            //not exact standard, missing T delimiter between date and time
            public const string Iso = "{0:yyyy-MM-dd HH:mm:ss}";
            public const string IsoUpToMinute = "{0:yyyy-MM-dd HH:mm}";
        }

        /// <summary>
        /// Format strings for any numerical type that should be represented as a currency
        /// </summary>
        public static class Currency
        {
            public const string Default = AsRandRoundedToTwoDecimalPlaces;
            public const string AsRandRoundedToTwoDecimalPlaces = "R{0:n2}";
            public const string AsCurrency = "{0:c}";
        }

        /// <summary>
        /// Format strings for any numerical type
        /// </summary>
        public static class Number
        {
            public const string Default = RoundedToNearestUnitWithThousandSeparator;
            public const string RoundedToNearestUnitWithThousandSeparator = "{0:n0}";
        }
    }
}
