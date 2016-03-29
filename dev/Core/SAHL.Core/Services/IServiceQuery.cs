namespace SAHL.Core.Services
{
    public interface IServiceQuery : IServiceCommand
    {
    }

    public interface IServiceQuery<U> : IServiceQuery where U : IServiceQueryResult
    {
        U Result { get; set; }
    }
}