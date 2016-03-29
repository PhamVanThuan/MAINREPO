using StructureMap;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SAHL.Tools.Capitec.CSJsonifier
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Options options = new Options();
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                IOCConfig.Register(options);
                IContainer container = ObjectFactory.GetInstance<IContainer>();
                ITypeScanner scanner = container.GetInstance<ITypeScanner>();
                ITemplateManager templateManager = container.GetInstance<ITemplateManager>();
                IFileManagement fileManagement = container.GetInstance<IFileManagement>();
                ISharedModelManager sharedModelManager = container.GetInstance<ISharedModelManager>();
                string assemblyName = string.Empty;
                IEnumerable<IScanResult> conventionResults = scanner.Scan(options.Input,out assemblyName);
                IEnumerable<IScanResult> filteredResults = conventionResults.Where(x => x.CommandTypes.Count > 0);
                foreach (IScanResult result in filteredResults)
                {
                    IBaseTemplate template = container.GetInstance<IBaseTemplate>();
                    IFakeBaseTemplate fakeTemplate = container.GetInstance<IFakeBaseTemplate>();

                    fileManagement.Save(options.Output, options.libFolder, assemblyName, result.Type, template.Process(result));
                    fileManagement.Save(options.Output, options.libFolder, assemblyName + ".Fake", result.Type, fakeTemplate.Process(result));
                }

                IScanResult sharedResult = sharedModelManager.GetComplexModels(filteredResults);
                if (sharedResult.CommandTypes.Count > 0)
                {
                    IBaseTemplate sharedTemplate = container.GetInstance<IBaseTemplate>();
                    fileManagement.Save(options.Output, options.libFolder, assemblyName, sharedResult.Type, sharedTemplate.Process(sharedResult));
                }
            }
        }
    }
}