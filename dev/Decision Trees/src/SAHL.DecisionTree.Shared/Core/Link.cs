using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DecisionTree.Shared.Core
{
    public class Link
    {
        public int ID { get; protected set; }
        public int FromNodeID { get; protected set; }
        public int ToNodeID { get; protected set; }
        public LinkType Type { get; protected set; }

        public Link(int Id, int fromNodeID, int toNodeID, LinkType type)
        {
            this.ID = Id;
            this.FromNodeID = fromNodeID;
            this.ToNodeID = toNodeID;
            this.Type = type;
        }
    }

    public enum LinkType 
    {
        DecisionYes,
        DecisionNo,
        Standard
    }
}
