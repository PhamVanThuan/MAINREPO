using SAHL.Tools.DecisionTree.TestRunner.Interfaces;

namespace SAHL.DecisionTree.TreeTests.Models
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