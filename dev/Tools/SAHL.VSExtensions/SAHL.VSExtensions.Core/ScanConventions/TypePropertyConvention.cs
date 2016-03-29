using SAHL.VSExtensions.Interfaces.Reflection;
using System.Collections.ObjectModel;

namespace SAHL.VSExtensions.Core.ScanConventions
{
    public class TypePropertyConvention : IScanConvention
    {
        private string typeName;

        public ObservableCollection<ITypeInfo> Results
        {
            get;
            set;
        }

        public TypePropertyConvention(string typeName)
        {
            this.typeName = ConventionHelper.ParseType(typeName);
            Results = new ObservableCollection<ITypeInfo>();
        }

        public void ProcessType(Mono.Cecil.TypeDefinition typeToProcess)
        {
            if ((typeToProcess.FullName == typeName))
            {
                foreach (Mono.Cecil.PropertyDefinition definition in typeToProcess.Properties)
                {
                    Results.Add(new TypeInfo(definition.Name, definition.FullName));
                }
            }
        }
    }
}