namespace SAHL.Services.Interfaces.Query.Models
{
    public interface ISupportedLookup
    {
        string Lookup { get; set; }
        ILookupMetaDataModel MetaData { get; set; } 
    }
}