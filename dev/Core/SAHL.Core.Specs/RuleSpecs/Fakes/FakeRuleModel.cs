namespace SAHL.Core.Specs.RuleSpecs.Fakes
{
    public class FakeRuleModel : IFakePartialRuleItem
    {
        public FakeRuleModel(string someProperty)
        {
            this.SomeProperty = someProperty;
        }

        public string SomeProperty { get; protected set; }

        public string ABC
        {
            get { return "ABC"; }
        }
    }
}