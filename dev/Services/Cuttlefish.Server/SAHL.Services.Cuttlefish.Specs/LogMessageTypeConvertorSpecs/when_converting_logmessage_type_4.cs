using Machine.Specifications;
using SAHL.Services.Cuttlefish.Workers;

namespace SAHL.Services.Cuttlefish.Specs.LogMessageTypeConvertorSpecs
{
    public class when_converting_logmessage_type_4
    {
        private static LogMessageTypeConverter converter;
        private static string expectedStringType;
        private static string result;

        private Establish context = () =>
            {
                converter = new LogMessageTypeConverter();
                expectedStringType = "Debug";
            };

        private Because of = () =>
            {
                result = converter.ConvertLogMessageTypeToString(4);
            };

        private It should_return_the_correct_string_version_of_the_logmessage_type_debug = () =>
            {
                result.ShouldEqual(expectedStringType);
            };
    }
}