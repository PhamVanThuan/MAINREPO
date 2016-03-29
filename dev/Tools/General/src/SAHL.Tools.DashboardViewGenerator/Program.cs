using Mono.Cecil;
using SAHL.Tools.DashboardViewGenerator.Templates;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SAHL.Tools.DashboardViewGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            Options options = new Options();
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                IContainer container = IOCConfig.Register(options);
                string assemblyName = "";
                IEnumerable<IScanResult> conventions = container.GetInstance<ITypeScanner>().Scan(options.Input, out assemblyName);
                IFileManager fileManager = container.GetInstance<IFileManager>();
                foreach (IScanResult convention in conventions)
                {
                    foreach (TypeDefinition type in convention.Types)
                    {
                        var htmlTemplate = new DashboardHtmlTemplate(type);
                        var jsTemplate = new DashboardJSTemplate(type);

                        if(!fileManager.DoesFileExist(htmlTemplate.RelativePath))
                        {
                            fileManager.SaveNewFile(htmlTemplate.TransformText(),htmlTemplate.RelativePath);
                        }
                        if(!fileManager.DoesFileExist(jsTemplate.RelativePath))
                        {
                            fileManager.SaveNewFile(jsTemplate.TransformText(), jsTemplate.RelativePath);
                        }
                    }
                }
            }
        }
    }
}
