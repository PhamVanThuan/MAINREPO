using System.Collections.Generic;

namespace DomainService2.SharedServices.Common
{
    public class UpdateParentVarsCommand : StandardDomainServiceCommand
    {
        public UpdateParentVarsCommand(long childInstanceID, Dictionary<string, object> dict)
        {
            this.ChildInstanceID = childInstanceID;
            this.Dict = dict;
        }

        public long ChildInstanceID
        {
            get;
            protected set;
        }

        public Dictionary<string, object> Dict
        {
            get;
            protected set;
        }
    }
}