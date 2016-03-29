namespace SAHL.Core.Services.Extensions
{
    public interface ICommandValidation<C, V>
        where C : IServiceCommand
        where V : IValidation
    {
    }
}