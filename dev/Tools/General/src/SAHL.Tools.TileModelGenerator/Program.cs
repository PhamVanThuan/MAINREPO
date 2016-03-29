using SAHL.Tools.TileModelGenerator.Reflection;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.TileModelGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            Options options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                IContainer container = IOCConfig.Register();
                ITypeScanner scanner = container.GetInstance<ITypeScanner>();
                IEnumerable<IResult> results = scanner.Scan(options.Input);
                ResultProcessor processor = new ResultProcessor(container.GetInstance<IFileManager>(), options.Output);
                
                processor.GenerateViews(results.OfType<TileModelConvention>().Single());
                processor.GenerateEditorViews(results.OfType<TileEditorConvention>().Single(), results.OfType<TileModelConvention>().Single());
                processor.GeneratePages(results.OfType<TileToPageConvention>().Single());
            }
        }
    }
}
