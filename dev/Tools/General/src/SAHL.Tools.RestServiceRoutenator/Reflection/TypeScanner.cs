using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
using Mono.Cecil;

namespace SAHL.Tools.RestServiceRoutenator.Reflection
{
    public class TypeScanner : ITypeScanner
    {
        private IEnumerable<IScanConvention> scanConventions;

        public TypeScanner(IEnumerable<IScanConvention> scanConventions)
        {
            this.scanConventions = scanConventions;
        }

        public IEnumerable<IScanResult> Scan(string desiredAssemblyPath,out string assemblyName)
        {
            var scanResults = ScanMono(desiredAssemblyPath, out assemblyName).ToArray();

            foreach (var result in scanResults)
            {
                result.AdditionalData= scanXmlDoc(desiredAssemblyPath.Replace("dll", "Routes.xml"));
            }

            return scanResults;
        }

        private string[] scanXmlDoc(string desiredXmlPath)
        {
            if (!File.Exists(desiredXmlPath))
                return new string [0];

            XDocument doc = XDocument.Load(desiredXmlPath);
            IEnumerable<string> roots = doc.XPathSelectElements("//route").Select(x => x.Value);
            return roots.ToArray();
        }

        private IEnumerable<IScanResult> ScanMono(string input, out string assemblyName)
        {
            var assemblyResolver = new DefaultAssemblyResolver();
            assemblyResolver.AddSearchDirectory(new FileInfo(input).Directory.FullName);

            var readerParameters = new ReaderParameters
            {
                AssemblyResolver = assemblyResolver,
                ReadSymbols = false,
            };

            AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(input, readerParameters);
            assemblyName = assemblyDefinition.Name.Name;
            foreach (TypeDefinition definition in assemblyDefinition.MainModule.GetTypes())
            {
                foreach (var convention in scanConventions)
                {
                    convention.ProcessType(definition);
                }
            }

            return scanConventions.Select(x => x as IScanResult);
        }
    }
}