namespace SAHL.Core.Services
{
    public class SortQueryParameter : IQueryParameter
    {
        public SortQueryParameter()
        {
        }

        public string OrderBy { get; set; }

        public SortDirectionOptions SortDirection { get; set; }

        public enum SortDirectionOptions { Ascending, Descending }
    }
}