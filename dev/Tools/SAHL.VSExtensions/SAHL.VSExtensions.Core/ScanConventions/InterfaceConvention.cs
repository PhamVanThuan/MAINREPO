using Mono.Cecil;
using SAHL.VSExtensions.Interfaces.Reflection;
using System.Collections.ObjectModel;
using System.Linq;

namespace SAHL.VSExtensions.Core.ScanConventions
{
    public class InterfaceConvention : IScanConvention
    {
        private string typeName;

        public ObservableCollection<ITypeInfo> Results
        {
            get;
            set;
        }

        public InterfaceConvention(string typeName)
        {
            this.typeName = ConventionHelper.ParseType(typeName);
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
            bool result = type.Interfaces.Any(x => x.Name == interfaceFullName || HasInterface(x.Resolve(), interfaceFullName)) || (type.BaseType != null && HasInterface(type.BaseType.Resolve(), interfaceFullName));
            return result;
        }
    }
}