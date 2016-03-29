namespace SAHL.Services.Cuttlefish.Services
{
    public interface IDataAccessConfigurationProvider
    {
        string ConnectionString { get; }
    }
}