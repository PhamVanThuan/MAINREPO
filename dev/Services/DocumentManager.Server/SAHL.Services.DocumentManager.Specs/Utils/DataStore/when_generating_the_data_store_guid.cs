using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.DocumentManager.Utils.DataStore;
using System;
using System.Linq;

namespace SAHL.Services.DocumentManager.Specs.Utils.DataStore
{
    public class when_generating_the_data_store_guid : WithFakes
    {
        private static string actualGuid;
        private static DataStoreUtils dataStorUtils;

        private Establish context = () =>
            {
                dataStorUtils = new DataStoreUtils();
            };

        private Because of = () =>
            {
                actualGuid = dataStorUtils.GenerateDataStoreGuid();
            };

        private It should_be_in_lower_case = () =>
            {
                actualGuid.Select(c => char.IsUpper(c)).First().ShouldBeFalse();
            };

        private It should_start_with_a_brace = () =>
        {
            actualGuid.ElementAt(0).ShouldEqual('{');
        };

        private It should_end_with_a_brace = () =>
        {
            actualGuid.ElementAt(actualGuid.Length - 1).ShouldEqual('}');
        };

        private It should_be_separated_with_hyphens = () =>
        {
            actualGuid.Count(x => x == '-').ShouldBeGreaterThan(0);
        };
    }
}