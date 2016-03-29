namespace SAHL.Core.Data
{
    public interface IDataModelWithPrimaryKeyId : IDataModel
    {
        int Id { get; set; }
    }
}