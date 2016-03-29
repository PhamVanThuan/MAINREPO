using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.ObjectFromJsonGenerator.Lib.Models
{
    public class Variable
    {
        public string Name { get; protected set; }
        public string Type { get; protected set; }
        public string Value { get; protected set; }
        public bool IsEnum { get; protected set; }

        public Variable(string name,string type,string value,bool isEnum)
        {
            this.Name = name;
            this.Type = type;
            this.Value = value;
            this.IsEnum = isEnum;
        }
    }
}
