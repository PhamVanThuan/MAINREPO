using SAHL.Core.Testing.FileConventions;
using SAHL.Core.Testing.Ioc;
using StructureMap;
using StructureMap.Graph;
using System;
namespace SAHL.Core.Testing
{
    public class TestParams: ITestParams
    {
        public TestParams(Type typeUnderTest, Type usedConvention, ITestingIocContainer testingIocContainer)
        {
            this.TypeUnderTest = typeUnderTest;
            this.UsedConvention = usedConvention;
            this.TestingIocContainer = testingIocContainer;
        }
        public Type TypeUnderTest
        {
            get;
            private set;
        }

        public ITestingIocContainer TestingIocContainer
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return String.Format(" Type under test: {0}, Assembly: {1}", this.TypeUnderTest.Name, this.TypeUnderTest.Assembly.GetName().Name);
        }

        public string TestName
        {
            get
            {
                return this.TypeUnderTest.Name;
            }
        }


        public Type UsedConvention { get; private set; }
    }
}
