namespace SAHL.Tools.DomainServiceDocumenter.Lib
{
    public interface ITypeScanner
    {
        bool ProcessTypeDefinition(Mono.Cecil.TypeDefinition typeToProcess);
    }
}