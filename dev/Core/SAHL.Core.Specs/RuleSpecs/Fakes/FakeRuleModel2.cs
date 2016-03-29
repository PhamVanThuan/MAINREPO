namespace SAHL.Core.Specs.RuleSpecs.Fakes
{
    public class FakeRuleModel2
    {
        public FakeRuleModel2(string someProperty)
        {
            this.SomeProperty = someProperty;
        }

        public string SomeProperty { get; protected set; }
    }
}