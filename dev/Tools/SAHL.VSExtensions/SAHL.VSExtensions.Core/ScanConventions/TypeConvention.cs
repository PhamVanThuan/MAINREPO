using SAHL.VSExtensions.Interfaces.Reflection;
using System.Collections.ObjectModel;

namespace SAHL.VSExtensions.Core.ScanConventions
{
    public class TypeConvention : IScanConvention
    {
        private string typeName;

        public ObservableCollection<ITypeInfo> Results
        {
            get;
            set;
        }

        public TypeConvention(string typeName)
        {
            this.typeName = ConventionHelper.ParseType(typeName);
            Results = new ObservableCollection<ITypeInfo>();
        }

        public void ProcessType(Mono.Cecil.TypeDefinition typeToProcess)
        {
            if ((typeToProcess.BaseType != null && typeToProcess.BaseType.Name == typeName))
            {
                Results.Add(new TypeInfo(typeToProcess.Name, typeToProcess.FullName));
            }
        }
    }
}