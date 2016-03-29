using SAHL.Core.IoC;

namespace SAHL.X2Engine2.Node
{
    /// <summary>
    /// Abstraction of an X2 Engine Node that can be hosted in a variety of environents.
    /// </summary>
    public interface IX2EngineNodeHost : IStartableService, IStoppableService
    {
    }
}