using Machine.Fakes;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Assertions;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.AssertionManagerSpecs
{
    public class AssertionManagerTestFactory : WithFakes
    {
        public IAssertionFactory AssertionFactory{get;set;}
        public AssertionManager AssertionManager { get; set; }
        public AssertionManagerTestFactory()
        {
            this.AssertionFactory = An<IAssertionFactory>();
            this.AssertionManager = new AssertionManager(AssertionFactory);
        }
    }
}
