using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services.Extensions;
using System;
using System.Linq;

namespace SAHL.Core.Services.Specs.ExtensionsSpecs
{
    public class when_the_type_name_does_not_start_with_get : WithFakes
    {
        private class ReadOnlyType { };

        private static bool isReadOnly;
        private static ReadOnlyType type;

        private Establish context = () =>
        {
            type = new ReadOnlyType();
        };

        private Because of = () =>
        {
            isReadOnly = type.GetType().IsReadOnlyCommand();
        };

        private It should_return_false = () =>
        {
            isReadOnly.ShouldBeFalse();
        };
    }
}