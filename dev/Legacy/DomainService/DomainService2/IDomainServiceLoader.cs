using SAHL.X2.Common;

namespace DomainService2
{
    public interface IDomainServiceLoader
    {
        T Get<T>() where T : IX2WorkflowService;

        IDomainServiceIOC DomainServiceIOC { get; }
    }
}