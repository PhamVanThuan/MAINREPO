namespace SAHL.Tools.DomainServiceDocumenter.Lib.TypeScanners
{
    public class QueryTypeScanner : ITypeScanner
    {
        public bool ProcessTypeDefinition(Mono.Cecil.TypeDefinition typeToProcess)
        {
            if (typeToProcess.IsInterface == false && typeToProcess.Name.EndsWith("Query"))
            {
                return true;
            }

            return false;
        }
    }
}