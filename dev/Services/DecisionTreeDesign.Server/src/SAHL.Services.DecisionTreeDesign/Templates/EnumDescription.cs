using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.DecisionTreeDesign.Templates
{
    public class EnumDescription
    {
        public EnumDescription(string name, List<string> enumsLiterals)
        {
            this.Name = name;
            this.EnumLiterals = enumsLiterals;   
        }

        public string Name { get; protected set; }
        public List<string> EnumLiterals { get; protected set; }
        
        public string enumsAsString()
        {
            string merged = "";
            foreach (string en in EnumLiterals)
            {
                merged += en + ", ";
            }
            merged = merged.TrimEnd(", ".ToArray());
            return merged;
        }
    }
}
