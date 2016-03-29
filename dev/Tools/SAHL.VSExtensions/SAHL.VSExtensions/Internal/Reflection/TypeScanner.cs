using Mono.Cecil;
using SAHL.VSExtensions.Interfaces.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SAHomeloans.SAHL_VSExtensions.Internal.Reflection
{
    public class TypeScanner : ITypeScanner
    {
        public TypeScanner()
        {
        }

        public void Scan(IEnumerable<IScannableAssembly> assemblies, IEnumerable<IScanConvention> scanConventions)
        {
            IScannableAssembly first = assemblies.FirstOrDefault();

            var assemblyResolver = new DefaultAssemblyResolver();
            assemblyResolver.AddSearchDirectory(new FileInfo(first.AssemblyPath).Directory.FullName);
            ReaderParameters readerParameters = new ReaderParameters
            {
                AssemblyResolver = assemblyResolver,
                ReadSymbols = false,
            };

            foreach (IScannableAssembly assembly in assemblies)
            {
                foreach (TypeDefinition definition in AssemblyDefinition.ReadAssembly(assembly.AssemblyPath, readerParameters).MainModule.GetTypes())
                {
                    foreach (IScanConvention convention in scanConventions)
                    {
                        convention.ProcessType(definition);
                    }
                }
            }
        }
    }
}