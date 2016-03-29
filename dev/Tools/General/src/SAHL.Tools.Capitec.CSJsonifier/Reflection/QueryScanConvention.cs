using System.Collections.Generic;
using System.Linq;

namespace SAHL.Tools.Capitec.CSJsonifier.Reflection
{
    public class QueryScanConvention : TypeScanConventionBase, IScanConvention
    {
        public bool UseContainer
        {
            get { return false; }
        }

        public string Type
        {
            get { return "Queries"; }
        }

        public string FakeType
        {
            get { return "query"; }
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
            string[] typeNames = new string[] { ParseType("ISqlServiceQuery<>"), ParseType("IServiceQuery<>") };
            string typeNameToExclude = ParseType("IFullTextServiceQuery");
            foreach (string typeName in typeNames)
            {
                if (HasInterface(typeToProcess, typeName) && !typeToProcess.IsInterface && !typeToProcess.IsAbstract && typeToProcess.IsPublic && !HasInterface(typeToProcess, typeNameToExclude))
                {
                    if (!base.commandTypes.Contains(typeToProcess))
                    {
                        base.commandTypes.Add(typeToProcess);
                    }

                    Mono.Cecil.TypeReference serviceQueryResult = ((Mono.Cecil.GenericInstanceType)typeToProcess.BaseType).GenericArguments.FirstOrDefault();
                    Mono.Cecil.TypeDefinition resultType = serviceQueryResult == null ? null : serviceQueryResult.Resolve();
                    if (resultType != null)
                    {
                        if (!base.commandResultTypes.Contains(resultType))
                        {
                            base.commandResultTypes.Add(resultType);
                        }
                    }
                }
            }
        }
    }
}