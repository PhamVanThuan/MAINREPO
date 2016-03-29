using SAHL.Core.Testing.FileConventions;
using StructureMap.Graph;
using System.Collections.Generic;
namespace SAHL.Core.Testing.Providers
{
    public interface ITestParamsProvider<T, T2>
        where T: IFileConvention
        where T2 : IRegistrationConvention, new()
    {
        IEnumerable<ITestParams> GetTestParams();
    }
}
