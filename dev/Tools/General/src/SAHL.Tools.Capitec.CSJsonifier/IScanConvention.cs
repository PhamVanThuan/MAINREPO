using Mono.Cecil;

namespace SAHL.Tools.Capitec.CSJsonifier
{
    public interface IScanConvention : IScanResult
    {
        void ProcessType(TypeDefinition typeToProcess);
    }
}