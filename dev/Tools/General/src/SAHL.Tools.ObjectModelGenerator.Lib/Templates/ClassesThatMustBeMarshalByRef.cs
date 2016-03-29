using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.ObjectModelGenerator.Lib.Templates
{
    public static class ClassesThatMustBeMarshalByRef
    {
        public static List<string> Classes
        {
            get
            {
                return new List<string>(new string[] { "Instance" });
            }
        }
    }
}
