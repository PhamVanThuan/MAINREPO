namespace SAHL.Core.Services
{
    public interface IServiceCommandWithReturnedData : IServiceCommand
    {
    }

    public interface IServiceCommandWithReturnedData<T> : IServiceCommandWithReturnedData
    {
        T Result { get; set; }
    }
}