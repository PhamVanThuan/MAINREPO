namespace SAHL.Services.Interfaces.Search.Models
{
    public class SearchFilter
    {
        public SearchFilter(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; protected set; }

        public string Value { get; protected set; }
    }
}