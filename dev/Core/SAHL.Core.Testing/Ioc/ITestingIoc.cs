using SAHL.Core.Testing.FileConventions;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Pipeline;
namespace SAHL.Core.Testing.Ioc
{
    public interface ITestingIoc : IContainer
    {
        void Configure<T, T2>()
            where T : IFileConvention
            where T2 : IRegistrationConvention, new();
    }
}
