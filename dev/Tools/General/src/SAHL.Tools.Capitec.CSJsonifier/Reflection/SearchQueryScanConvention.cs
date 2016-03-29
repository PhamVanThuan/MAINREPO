using System.Collections.Generic;
using System.Linq;

namespace SAHL.Tools.Capitec.CSJsonifier.Reflection
{
    public class SearchQueryScanConvention : TypeScanConventionBase, IScanConvention
    {
        public bool UseContainer
        {
            get { return false; }
        }

        public string Type
        {
            get { return "SearchQueries"; }
        }

        public string FakeType
        {
            get { return "searchQuery"; }
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
            string typeName = ParseType("IFullTextServiceQuery");
            if (HasInterface(typeToProcess, typeName) && !typeToProcess.IsInterface && !typeToProcess.IsAbstract && typeToProcess.IsPublic)
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