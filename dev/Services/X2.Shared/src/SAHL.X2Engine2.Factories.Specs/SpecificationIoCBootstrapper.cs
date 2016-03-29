using StructureMap;

namespace SAHL.X2Engine2.Factories.Specs
{
    public static class SpecificationIoCBootstrapper
    {
        public static void Initialize()
        {
            ObjectFactory.Initialize(x =>
                {
                    x.Scan(scan =>
                    {
                        scan.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                        scan.WithDefaultConventions();
                        scan.LookForRegistries();
                    });
                });
        }
    }
}