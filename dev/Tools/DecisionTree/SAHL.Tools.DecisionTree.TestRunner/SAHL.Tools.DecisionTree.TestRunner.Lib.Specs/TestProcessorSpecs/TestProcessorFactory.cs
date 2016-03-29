using Machine.Fakes;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Specs
{
    public class TestProcessorFactory : WithFakes
    {
        public TestProcessor Processor { get; set; }
        public IAssertionManager assertionManager { get;set; }
        public TestProcessorFactory()
        {
            assertionManager = An<IAssertionManager>();
            Processor = new TestProcessor(assertionManager);
        }
    }
}
