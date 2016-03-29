namespace DomainService2
{
    public interface IHandlesDomainServiceCommandDecorator<T> : IHandlesDomainServiceCommand<T> where T : IDomainServiceCommand
    {
        IHandlesDomainServiceCommand<T> InnerHandler { get; }
    }
}