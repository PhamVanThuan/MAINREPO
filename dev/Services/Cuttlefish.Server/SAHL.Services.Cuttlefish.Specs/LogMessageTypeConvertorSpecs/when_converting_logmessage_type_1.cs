using Machine.Specifications;
using SAHL.Services.Cuttlefish.Workers;

namespace SAHL.Services.Cuttlefish.Specs.LogMessageTypeConvertorSpecs
{
    public class when_converting_logmessage_type_1
    {
        private static LogMessageTypeConverter converter;
        private static string expectedStringType;
        private static string result;

        private Establish context = () =>
            {
                converter = new LogMessageTypeConverter();
                expectedStringType = "Error";
            };

        private Because of = () =>
            {
                result = converter.ConvertLogMessageTypeToString(1);
            };

        private It should_return_the_correct_string_version_of_the_logmessage_type_error = () =>
            {
                result.ShouldEqual(expectedStringType);
            };
    }
}