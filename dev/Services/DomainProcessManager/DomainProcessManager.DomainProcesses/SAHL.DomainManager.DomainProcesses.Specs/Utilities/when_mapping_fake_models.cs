using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainProcessManager.DomainProcesses.Specs.Utilities.Fakes;
using SAHL.DomainProcessManager.DomainProcesses.Utilities;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Utilities
{
    public class when_mapping_fake_models : WithFakes
    {
        private static DomainModelMapper mapper;
        private static FakeModel domainProcessModel;
        private static FakeDomainName.Fakes.FakeModel resultingDomainModel;

        private Establish context = () =>
        {
            fakeModelID = 13;
            name = "FakeName";
            category = "Category 1";
            title = "Fakes";
            value = 3000;
            domainProcessModel = new FakeModel(fakeModelID, name, category, title, value);
            mapper = new DomainModelMapper();
            mapper.CreateMap<FakeModel, FakeDomainName.Fakes.FakeModel>();
        };

        private Because of = () =>
        {
            resultingDomainModel = mapper.Map(domainProcessModel);
        };

        private It should_return_an_object_with_the_same_properties = () =>
        {
            resultingDomainModel.ShouldMatch(m =>
                m.FakeModelID == fakeModelID &&
                m.Name == name &&
                m.Category == category &&
                m.Title == title &&
                m.Value == value
            );
        };

        private static decimal value;
        private static string title;
        private static string category;
        private static string name;
        private static int fakeModelID;
    }
}