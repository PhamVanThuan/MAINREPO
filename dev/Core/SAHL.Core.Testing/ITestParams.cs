using SAHL.Core.Testing.Ioc;
using StructureMap;
using System;
namespace SAHL.Core.Testing
{
    public interface ITestParams
    {
        Type TypeUnderTest { get; }
        ITestingIocContainer TestingIocContainer { get; }
        string TestName { get; }
        Type UsedConvention { get; }
    }
}
