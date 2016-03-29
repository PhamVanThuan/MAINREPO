namespace SAHL.Services.Interfaces.Query.Models
{
    public interface ISupportedLookupModel
    {
        string LookupKey { get; set; } 
        string LookupTable { get; set; } 
    }
}