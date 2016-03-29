using Mono.Cecil;

namespace SAHL.Tools.RestServiceRoutenator
{
    public interface IScanConvention : IScanResult
    {
        void ProcessType(TypeDefinition typeToProcess);
    }
}