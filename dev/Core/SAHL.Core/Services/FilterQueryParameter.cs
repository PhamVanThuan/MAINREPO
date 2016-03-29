namespace SAHL.Core.Services
{
    public class FilterQueryParameter : IQueryParameter
    {
        public FilterQueryParameter()
        {
        }

        public string FilterOn { get; set; }

        public string FilterValue { get; set; }
    }
}