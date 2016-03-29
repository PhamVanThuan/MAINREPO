using SAHL.Common.Collections.Interfaces;

namespace DomainService2
{
    public interface IHandlesDomainServiceCommand<T> where T : IDomainServiceCommand
    {
        void Handle(IDomainMessageCollection messages, T command);
    }
}