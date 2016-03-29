using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.DecisionTreeModelsFromJsonGenerator.Lib.Templates
{
    public class TreeVariableDescription
    {
        public int Id { get; set; }

        public string PropertyName { get; protected set; }

        public string TypeName { get; protected set; }

        public string Usage { get; protected set; }

        public TreeVariableDescription(int id, string propertyName, string typeName, string usage)
        {
            this.Id = id;
            this.PropertyName = propertyName;
            this.TypeName = typeName;
            this.Usage = usage;
        }
    }
}
