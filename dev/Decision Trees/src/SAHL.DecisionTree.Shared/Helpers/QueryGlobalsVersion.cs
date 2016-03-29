using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DecisionTree.Shared.Helpers
{
    public class QueryGlobalsVersion
    {
        public int VariablesVersion
        {
            get;protected set;
        }

        public int MessagesVersion
        {
            get;protected set;
        }

        public int EnumerationsVersion
        {
            get;protected set;
        }

        public QueryGlobalsVersion(int VariablesVersion, int MessagesVersion, int EnumerationsVersion)
        {
            this.VariablesVersion = VariablesVersion;
            this.MessagesVersion = MessagesVersion;
            this.EnumerationsVersion = EnumerationsVersion;
        }
    }
}
