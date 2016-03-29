using System.Collections.Generic;

namespace DomainService2.SharedServices.Common
{
    public class CloneExistsForParentCommand : StandardDomainServiceCommand
    {
        public CloneExistsForParentCommand(long childInstanceId, List<string> states)
        {
            this.ChildInstanceId = childInstanceId;
            this.States = states;
        }

        public long ChildInstanceId
        {
            get;
            protected set;
        }

        public List<string> States
        {
            get;
            protected set;
        }

        public bool Result { get; set; }
    }
}