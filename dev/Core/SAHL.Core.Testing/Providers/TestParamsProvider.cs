using SAHL.Core.Testing.FileConventions;
using SAHL.Core.Testing.Ioc;
using StructureMap.Graph;
using System.Collections.Generic;
namespace SAHL.Core.Testing.Providers
{
    public class TestParamsProvider<T, T2> : ITestParamsProvider<T, T2>
        where T: IFileConvention
        where T2 : IRegistrationConvention, new()
    {
        private ITestingIocContainer testingIocContainer;
        public TestParamsProvider(ITestingIocContainer<T,T2> testingIocContainer)
        {
            this.testingIocContainer = testingIocContainer;
        }

        public IEnumerable<ITestParams> GetTestParams()
        {
            var testParams = new List<ITestParams>();
            foreach (var typeUnderTest in this.testingIocContainer.GetRegisteredTypes())
            {
                testParams.Add(new TestParams(typeUnderTest, typeof(T2), this.testingIocContainer));
            }
            return testParams;
        }
    }
}
