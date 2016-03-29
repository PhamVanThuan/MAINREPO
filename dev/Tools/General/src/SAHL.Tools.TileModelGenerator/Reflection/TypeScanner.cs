using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.TileModelGenerator.Reflection
{
    public class TypeScanner : ITypeScanner
    {
        IScanConvention[] scanConventions;

        public TypeScanner(IScanConvention[] scanConventions)
        {
            this.scanConventions = scanConventions;
        }

        public IEnumerable<IResult> Scan(string inputLocation)
        {
            var assemblyResolver = new DefaultAssemblyResolver();
            assemblyResolver.AddSearchDirectory(new FileInfo(inputLocation).Directory.FullName);

            ReaderParameters readerParameters = new ReaderParameters
            {
                AssemblyResolver = assemblyResolver,
                ReadSymbols = false,
            };

            string assemblyName = AssemblyDefinition.ReadAssembly(inputLocation, readerParameters).Name.Name;
            foreach (TypeDefinition definition in AssemblyDefinition.ReadAssembly(inputLocation, readerParameters).MainModule.GetTypes())
            {
                foreach (IScanConvention convention in scanConventions)
                {
                    convention.ProcessType(definition);
                }
            }
            return scanConventions;
        }
    }
}
