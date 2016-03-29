using SAHL.VSExtensions.Interfaces.Reflection;
using System.Collections.ObjectModel;

namespace SAHL.VSExtensions.Core.ScanConventions
{
    public class TypeConstantsConvention : IScanConvention
    {
        private string typeName;

        public ObservableCollection<ITypeInfo> Results
        {
            get;
            set;
        }

        public TypeConstantsConvention(string typeName)
        {
            this.typeName = ConventionHelper.ParseType(typeName);
            Results = new ObservableCollection<ITypeInfo>();
        }

        public void ProcessType(Mono.Cecil.TypeDefinition typeToProcess)
        {
            if (typeToProcess.Name == typeName)
            {
                foreach (var x in typeToProcess.Fields)
                {
                    Results.Add(new TypeInfo(x.Name, string.Format("{0}.{1}", typeToProcess.FullName, x.Name))); // = //typeToProcess.Fields.Select(x => new TypeInfo(x.Name, string.Format("{0}.{1}", typeToProcess.FullName, x.Name))).ToArray();
                }
            }
        }
    }
}