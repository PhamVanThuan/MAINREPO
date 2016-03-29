namespace SAHL.Services.Interfaces.Query.Models.Core
{
    public interface IRelatedField
    {
        string LocalKey { get; set; }
        string RelatedKey { get; set; }
        string Value { get; set; } 
    }
}