using System.Collections.Generic;

namespace SAHL.Tools.Capitec.CSJsonifier.Reflection
{
    public class CommandScanConvention : TypeScanConventionBase, IScanConvention
    {
        public bool UseContainer
        {
            get { return false; }
        }

        public string Type
        {
            get { return "Commands"; }
        }

        public string FakeType
        {
            get { return "command"; }
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
            string[] typeNames = new string[]{ParseType("ICapitecServiceCommand"),ParseType("ServiceCommand")};
            foreach(string typeName in typeNames)
            {
                if ((HasInterface(typeToProcess, typeName) && !typeToProcess.IsInterface && !typeToProcess.IsAbstract && typeToProcess.IsPublic) || (typeToProcess.BaseType != null && typeToProcess.BaseType.Resolve().Name == typeName))
                {
                    if (!base.commandTypes.Contains(typeToProcess))
                    {
                        base.commandTypes.Add(typeToProcess);
                    }
                }
            }
        }
    }
}