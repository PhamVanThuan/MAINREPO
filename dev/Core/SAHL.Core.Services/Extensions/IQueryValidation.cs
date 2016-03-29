namespace SAHL.Core.Services.Extensions
{
    public interface IQueryValidation<Q, V>
        where Q : IServiceQuery
        where V : IValidation
    {
    }
}