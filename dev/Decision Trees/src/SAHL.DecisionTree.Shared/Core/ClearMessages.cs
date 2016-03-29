using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DecisionTree.Shared.Core
{
    public class ClearMessages : Node
    {
        public string SubtreeName { get; protected set; }

        public string SubtreeVersion { get; protected set; }

        public ClearMessages(string subtreeName, string subtreeVersion, int id, string name)
            : base(id, name, NodeType.ClearMessages, "")
        {
            this.SubtreeName = subtreeName;
            this.SubtreeVersion = subtreeVersion;
            this.nodeType = NodeType.ClearMessages;
        }
    }
}
