using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement
{
    public class SerialisationFriendlyStateMachine<TState, TTrigger> : Stateless.StateMachine<TState, TTrigger>
    {
        public SerialisationFriendlyStateMachine(TState initialState)
            : base(initialState)
        {
        }

        public new List<TTrigger> PermittedTriggers { get { return base.PermittedTriggers.ToList(); } }
    }
}