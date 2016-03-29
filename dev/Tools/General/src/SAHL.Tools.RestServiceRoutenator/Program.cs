using System;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SAHL.Tools.RestServiceRoutenator.Reflection;
using StructureMap;

namespace SAHL.Tools.RestServiceRoutenator
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new CommandOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                IOCConfig.Register(options);
                var container = ObjectFactory.GetInstance<IContainer>();
                var scanner = container.GetInstance<ITypeScanner>();
                var templateManager = container.GetInstance<ITemplateManager>();
                var fileManagement = container.GetInstance<IFileManagement>();
                string assemblyName;
                var conventionResults = scanner.Scan(options.Input, out assemblyName).Where(x => x.FoundTypes.Any());
                foreach (var result in conventionResults)
                {
                    var template = container.GetInstance<IRestBaseTemplate>();

                    fileManagement.Save(options.Output, options.libFolder, assemblyName, result.Type, template.Process(result));
                }
            }

        }
    }
}
