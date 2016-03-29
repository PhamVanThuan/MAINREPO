using Mono.Cecil;
using SAHL.VSExtensions.Interfaces.Reflection;
using System.Collections.ObjectModel;
using System.Linq;

namespace SAHL.VSExtensions.Core.ScanConventions
{
    public class InterfaceWithGenericsConvention : IScanConvention
    {
        private string typeName;
        private string[] genericTypes;

        public ObservableCollection<ITypeInfo> Results
        {
            get;
            set;
        }

        public InterfaceWithGenericsConvention(string typeName,params string[] genericTypes)
        {
            this.typeName = ConventionHelper.ParseType(typeName);
            this.genericTypes = genericTypes;
            Results = new ObservableCollection<ITypeInfo>();
        }

        public void ProcessType(Mono.Cecil.TypeDefinition typeToProcess)
        {
            if (HasInterface(typeToProcess, typeName) && !typeToProcess.IsInterface && !typeToProcess.IsAbstract && this.Results.SingleOrDefault(x => x.Name == typeToProcess.Name) == null)
            {
                this.Results.Add(new TypeInfo(typeToProcess.Name, typeToProcess.FullName));
            }
        }

        private bool HasInterface(TypeDefinition type, string interfaceFullName)
        {
            bool result = type.Interfaces.Any(x => (x.Name == interfaceFullName && genericTypes.All(z=>x.FullName.Contains(z)))|| HasInterface(x.Resolve(), interfaceFullName)) || (type.BaseType != null && HasInterface(type.BaseType.Resolve(), interfaceFullName));
            return result;
        }
    }
}