namespace SAHL.Common.BusinessModel.DataTransferObjects
{
    public static class BooleanExtensions
    {
        public static string ToYesNoString(this bool value)
        {
            return value ? "Yes" : "No";
        }
    }
}