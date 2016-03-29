namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Utilities.Fakes
{
    public class FakeDifferingModel
    {
        public int FakeModelID { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Title { get; set; }

        public decimal Value { get; set; }

        public string SomeExtraProperty { get; set; }

        public FakeDifferingModel(int fakeModelID, string name, string category, string title, decimal value, string someExtraProperty)
        {
            this.FakeModelID = fakeModelID;
            this.Name = name;
            this.Category = category;
            this.Title = title;
            this.SomeExtraProperty = someExtraProperty;
            this.Value = value;
        }
    }
}

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Utilities.FakeDomainName.Fakes
{
    public class FakeDifferingModel
    {
        public int FakeModelID { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Title { get; set; }

        public decimal Value { get; set; }

        public string Something { get; set; }

        public FakeDifferingModel(int fakeModelID, string name, string category, string title, decimal value, string something)
        {
            this.FakeModelID = fakeModelID;
            this.Name = name;
            this.Category = category;
            this.Title = title;
            this.Value = value;
            this.Something = something;
        }
    }
}