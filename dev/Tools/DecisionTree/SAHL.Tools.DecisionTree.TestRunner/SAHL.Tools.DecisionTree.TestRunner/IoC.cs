using SAHL.Tools.DecisionTree.Coverage.Lib;
using SAHL.Tools.DecisionTree.Coverage.Lib.CoverageResultWriters;
using SAHL.Tools.DecisionTree.TestRunner.Lib;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Interfaces;
using StructureMap;

namespace SAHL.Tools.DecisionTree.TestRunner
{
    public class IoC
    {
        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL.Tools.DecisionTree.TestRunner"));
                    scan.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL.Tools.DecisionTree.Coverage"));
                    scan.WithDefaultConventions();
                    scan.LookForRegistries();
                });
                x.For<ITreeExecutionManager>().Singleton().Use<TreeExecutionManager>();
                x.For<ICoverageResultWriter>().Use<CoverageResultXmlWriter>().Named("XML");
                x.For<ICoverageResultWriter>().Use<CoverageResultHtmlWriter>().Named("HTML");
            });
            return ObjectFactory.Container;
        }
    }
}