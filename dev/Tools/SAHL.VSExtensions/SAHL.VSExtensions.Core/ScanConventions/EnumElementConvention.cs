using SAHL.VSExtensions.Interfaces.Reflection;
using System.Collections.ObjectModel;
using System.Linq;

namespace SAHL.VSExtensions.Core.ScanConventions
{
    public class EnumElementConvention : IScanConvention
    {
        private string typeName;

        public ObservableCollection<ITypeInfo> Results
        {
            get;
            set;
        }

        public EnumElementConvention(string typeName)
        {
            this.typeName = typeName;
            Results = new ObservableCollection<ITypeInfo>();
        }

        public void ProcessType(Mono.Cecil.TypeDefinition typeToProcess)
        {
            if (typeToProcess.IsEnum && typeToProcess.Name == typeName)
            {
                foreach (var x in typeToProcess.Fields.Where(x => x.IsStatic == true))
                {
                    Results.Add(new TypeInfo(x.Name, string.Format("{0}.{1}", typeToProcess.FullName, x.Name)));
                }
            }
        }
    }
}