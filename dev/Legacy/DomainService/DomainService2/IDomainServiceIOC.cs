namespace DomainService2
{
    using System;

    public interface IDomainServiceIOC : IDisposable
    {
        IHandlesDomainServiceCommand<T> GetCommandHandler<T>() where T : IDomainServiceCommand;

        T Get<T>();
		void Bind<T, I>() where I : T;
    }
}