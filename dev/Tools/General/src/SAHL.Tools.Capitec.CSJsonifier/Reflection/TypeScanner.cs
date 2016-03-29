using Mono.Cecil;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SAHL.Tools.Capitec.CSJsonifier.Reflection
{
    public class TypeScanner : ITypeScanner
    {
        private IEnumerable<IScanConvention> scanConventions;

        public TypeScanner(IEnumerable<IScanConvention> scanConventions)
        {
            this.scanConventions = scanConventions;
        }

        public IEnumerable<IScanResult> Scan(string input,out string assemblyName)
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
                bool ignore = false;
                foreach (CustomAttribute customAttributes in definition.CustomAttributes)
                {
                    if (customAttributes.AttributeType.FullName == "SAHL.Core.Services.Attributes.CSJsonifierIgnore")
                    {
                        ignore = true;
                        break;
                    }
                }

                if (!ignore)
                {
                    foreach (IScanConvention convention in scanConventions)
                    {
                        convention.ProcessType(definition);
                    }
                }
            }

            return scanConventions.Select(x => x as IScanResult);
        }
    }
}