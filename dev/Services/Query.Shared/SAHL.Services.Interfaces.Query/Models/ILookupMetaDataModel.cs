namespace SAHL.Services.Interfaces.Query.Models
{
    public interface ILookupMetaDataModel
    {
        string LookupType { get; set; }
        string Db { get; set; }
        string Schema { get; set; }
        string LookupTable { get; set; }
        string LookupKey { get; set; }
        string LookupDescription { get; set; }
    }
}