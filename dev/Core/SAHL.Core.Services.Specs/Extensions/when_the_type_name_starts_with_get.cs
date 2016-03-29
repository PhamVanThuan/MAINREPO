using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services.Extensions;
using System;
using System.Linq;

namespace SAHL.Core.Services.Specs.ExtensionsSpecs
{
    public class when_the_type_name_starts_with_get : WithFakes
    {
        private class GetReadOnlyType { };

        private static bool isReadOnly;
        private static GetReadOnlyType type;

        private Establish context = () =>
            {
                type = new GetReadOnlyType();
            };

        private Because of = () =>
            {
                isReadOnly = type.GetType().IsReadOnlyCommand();
            };

        private It should_return_true = () =>
            {
                isReadOnly.ShouldBeTrue();
            };
    }
}