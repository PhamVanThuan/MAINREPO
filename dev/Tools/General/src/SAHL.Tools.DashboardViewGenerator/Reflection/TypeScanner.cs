using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.DashboardViewGenerator.Reflection
{
    public class TypeScanner : ITypeScanner
    {
        IScanConvention[] conventions;

        public TypeScanner(IScanConvention[] conventions)
        {
            this.conventions = conventions;
        }

        public IEnumerable<IScanResult> Scan(string input, out string assemblyName)
        {
            var assemblyResolver = new DefaultAssemblyResolver();
            assemblyResolver.AddSearchDirectory(new FileInfo(input).Directory.FullName);

            ReaderParameters readerParameters = new ReaderParameters
            {
                AssemblyResolver = assemblyResolver,
                ReadSymbols = false,
            };

            assemblyName = AssemblyDefinition.ReadAssembly(input, readerParameters).Name.Name;
            foreach (TypeDefinition definition in AssemblyDefinition.ReadAssembly(input, readerParameters).MainModule.GetTypes())
            {
                foreach (IScanConvention convention in this.conventions)
                {
                    convention.ProcessType(definition);
                }
            }
            return conventions;
        }
    }
}
