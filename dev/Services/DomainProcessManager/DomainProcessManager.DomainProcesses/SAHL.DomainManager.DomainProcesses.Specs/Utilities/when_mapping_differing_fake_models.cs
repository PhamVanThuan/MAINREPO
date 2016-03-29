using Machine.Fakes;
using Machine.Specifications;
using SAHL.DomainProcessManager.DomainProcesses.Specs.Utilities.Fakes;
using SAHL.DomainProcessManager.DomainProcesses.Utilities;
using System; 

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Utilities
{
    public class when_mapping_differing_fake_models : WithFakes
    {
        private static DomainModelMapper mapper;
        private static FakeDifferingModel domainProcessModel;
        private static Exception thrownException;

        private Establish context = () =>
        {
            domainProcessModel = new FakeDifferingModel(1, "Item1", "Items", "Title", 200, "Extras");
            mapper = new DomainModelMapper();
            mapper.CreateMap<FakeDifferingModel, FakeDomainName.Fakes.FakeDifferingModel>();
        };

        private Because of = () =>
        {
            thrownException = Catch.Exception(() => mapper.Map(domainProcessModel));
        };

        private It should_throw_an_exception_containing_the_mismatched_property_name = () =>
        {
            thrownException.InnerException.Message.ShouldContain("something");
        };

        private It should_throw_an_exception_containing_the_mismatched_source_type = () =>
        {
            thrownException.InnerException.Message.ShouldContain(typeof(FakeDifferingModel).FullName);
        };

        private It should_throw_an_exception_containing_the_mismatched_destination_type = () =>
        {
            thrownException.InnerException.Message.ShouldContain(typeof(FakeDomainName.Fakes.FakeDifferingModel).FullName);
        };
    }
}