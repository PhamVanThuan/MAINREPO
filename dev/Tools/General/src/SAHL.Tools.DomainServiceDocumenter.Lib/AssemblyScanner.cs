using Mono.Cecil;
using System.Collections.Generic;
using System.IO;

namespace SAHL.Tools.DomainServiceDocumenter.Lib
{
    public class AssemblyScanner
    {
        
        public void Scan(string assemblyFileName, IEnumerable<ITypeScanner> typeScanners)
        {
            IAssemblyResolver assemblyResolver = new ScopedAssemblyResolver(Path.GetDirectoryName(assemblyFileName));
            ReaderParameters readerParameters = new ReaderParameters()
            {
                ReadSymbols = true,
                AssemblyResolver = assemblyResolver
            };

            foreach (TypeDefinition type in AssemblyDefinition.ReadAssembly(assemblyFileName, readerParameters).MainModule.GetTypes())
            {
                foreach (var typeScanner in typeScanners)
                {
                    typeScanner.ProcessTypeDefinition(type);
                }
            }
        }

        public void ScanForAssociations(IEnumerable<IAssociationScanner> associationScanners)
        {
            foreach (var associationScanner in associationScanners)
            {
                associationScanner.ProcessAssociations();
            }
        }

    }
}