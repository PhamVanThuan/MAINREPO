using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.Capitec.CSJsonifier.Reflection
{
    public class DecisionTreeGlobalsConvention : TypeScanConventionBase, IScanConvention
    {
        public bool UseContainer
        {
            get { return false; }
        }

        public string Type
        {
            get { return "Globals"; }
        }

        public string FakeType
        {
            get { return "Globals"; }
        }

        public IList<Mono.Cecil.TypeDefinition> CommandTypes
        {
            get { return base.commandTypes; }
        }

        public IList<Mono.Cecil.TypeDefinition> CommandResultTypes
        {
            get { return base.commandResultTypes; }
        }

        public void ProcessType(Mono.Cecil.TypeDefinition typeToProcess)
        {
            string[] globals = new string[] { "Enumerations", "Messages", "Variables" };
            if (typeToProcess.Module.Assembly.Name.Name.Contains("Decision") && !typeToProcess.IsAbstract && typeToProcess.IsClass && typeToProcess.IsPublic && globals.Any(x => x == typeToProcess.Name))
            {
                if(!commandTypes.Contains(typeToProcess))
                {
                    commandTypes.Add(typeToProcess);
                }
            }
        }
    }
}
