using Mono.Cecil;
using System.Collections.ObjectModel;

namespace SAHL.VSExtensions.Interfaces.Reflection
{
    public interface IScanConvention
    {
        ObservableCollection<ITypeInfo> Results { get; set; }

        void ProcessType(TypeDefinition typeToProcess);
    }
}