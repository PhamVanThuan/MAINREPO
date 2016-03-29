using Machine.Specifications;
using SAHL.Core.Testing.Fluent;
namespace SAHL.Core.Testing.Specs
{
    public class when_running_an_integration_test : FluentTest
    {
        private static RandomFluentTest randomFluentTest;

        Establish context = () =>
        {
            randomFluentTest = new RandomFluentTest();
        };

        Because of = () =>
        {
            randomFluentTest.RunTest();
        };

        It should_run  = () =>
        {
        };
    }
    public class RandomFluentTest: FluentTest
    {
        public void RunTest()
        {
            test.Setup<SetupB>(y =>
            {
                y.Set<string>("name", "Testing Name");
            });
            test.Setup<SetupA>(y =>
            {
                y.Set<int>("id", 1234567);
                y.Set<string>("test", "test1234");
            }).Execute<SetupA>((ctx) =>
            {
                ctx.DoSomething();
            }).Assert<SetupA>((instance) =>
            {
                instance.ShouldNotBeNull();
            });
        }
    }
    public class SetupA
    {
        public SetupA(int id, SetupB setupB,string test)
        { 
        }
        public void DoSomething()
        {
            //this function is a test function it does nothing by design
        }
    }
    public class SetupB
    {
        private string name;
        public SetupB(string name)
        {
            this.name = name;
        }
        public void DoSomethingElse()
        {
            //this function is a test function it does nothing by design
        }
    }
}
