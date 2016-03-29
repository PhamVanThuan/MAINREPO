using System.Collections.Generic;
using Mono.Cecil;

namespace SAHL.Tools.RestServiceRoutenator.Reflection
{
    public class MonoCecilRepresentationRestQueryScan : TypeScanConventionBase, IScanConvention
    {
        public bool UseContainer { get; private set; }
        public string Type { get { return "Rest"; } }
        public string FakeType { get { return "rest"; } }
        public IList<TypeDefinition> FoundTypes { get { return base.FoundTypes; } }
        public IList<TypeDefinition> FoundResultTypes { get { return base.FoundReturnTypes; } }

        public void ProcessType(TypeDefinition typeToProcess)
        {
            string[] typeNames = { ParseType("ApiController"), ParseType("QueryServiceBaseController") };
            foreach (string typeName in typeNames)
            {
                if (IsValid(typeToProcess, typeName))
                {
                    AddIfDoesntExist(typeToProcess);
                }
            }
        }

        private bool IsValid(TypeDefinition typeToProcess, string typeName)
        {
            bool runnableClass = (!typeToProcess.IsAbstract && typeToProcess.IsPublic);
            bool childClassOfGivenType = (typeToProcess.BaseType != null &&  typeToProcess.BaseType.Name == typeName);
            return runnableClass && childClassOfGivenType;
        }

        private void AddIfDoesntExist(TypeDefinition typeToProcess)
        {
            if (!base.FoundTypes.Contains(typeToProcess))
            {
                base.FoundTypes.Add(typeToProcess);
            }
        }
    }
}