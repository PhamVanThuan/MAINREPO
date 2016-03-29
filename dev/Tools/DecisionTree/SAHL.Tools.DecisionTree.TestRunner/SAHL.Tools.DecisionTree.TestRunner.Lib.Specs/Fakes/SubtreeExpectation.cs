using SAHL.Tools.DecisionTree.TestRunner.Interfaces;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib.Specs.Fakes
{
    public class SubtreeExpectation : ISubtreeExpectation
    {
        public SubtreeExpectation(string subtreeName, string assertion)
        {
            this.Assertion = assertion;
            this.SubtreeName = subtreeName;
        }

        public string Assertion
        {
            get;

            set;
        }

        public string SubtreeName
        {
            get;

            set;
        }
    }
}