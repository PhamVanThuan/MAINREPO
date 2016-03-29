using System.Linq;
using System.IO;
using System.IO.Abstractions;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using System.Collections.Generic;

namespace SAHL_Tools.Common
{
    public class WebApiGenerator
    {
        private IFileSystem fileSystem;

        public WebApiGenerator(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void GenerateObjects(string inputFile, string outputFile)
        {
            SyntaxTree tree = SyntaxTree.ParseFile(inputFile);

            var findClients = tree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().First();
            var findClientsIdentifier = findClients.Identifier;
            var parameter1 = findClients.ParameterList.Parameters[0];

        }
    }
}