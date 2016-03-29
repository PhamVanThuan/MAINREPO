namespace SAHL.Services.Interfaces.Query.Models.Core
{
    public class PrimaryKey : IPrimaryKey
    {
        public string Key { get; set; }
        public string Alias { get; set; }
    }
}