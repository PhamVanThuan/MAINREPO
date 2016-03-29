namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Utilities.Fakes
{
    public class FakeModel
    {
        public int FakeModelID { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Title { get; set; }

        public decimal Value { get; set; }

        public FakeModel(int fakeModelID, string name, string category, string title, decimal value)
        {
            this.FakeModelID = fakeModelID;
            this.Name = name;
            this.Category = category;
            this.Title = title;
            this.Value = value;
        }
    }
}

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Utilities.FakeDomainName.Fakes
{
    public class FakeModel
    {
        public int FakeModelID { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Title { get; set; }

        public decimal Value { get; set; }

        public FakeModel(int fakeModelID, string name, string category, string title, decimal value)
        {
            this.FakeModelID = fakeModelID;
            this.Name = name;
            this.Category = category;
            this.Title = title;
            this.Value = value;
        }
    }
}