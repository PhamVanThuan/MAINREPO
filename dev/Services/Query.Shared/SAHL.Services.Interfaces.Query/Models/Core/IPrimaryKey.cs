namespace SAHL.Services.Interfaces.Query.Models.Core
{
    public interface IPrimaryKey
    {
        string Key { get; set; }
        string Alias { get; set; }
    }
}