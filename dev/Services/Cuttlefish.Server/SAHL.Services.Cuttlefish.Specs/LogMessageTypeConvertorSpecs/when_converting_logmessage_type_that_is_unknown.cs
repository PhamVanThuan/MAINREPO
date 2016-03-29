using Machine.Specifications;
using SAHL.Services.Cuttlefish.Workers;

namespace SAHL.Services.Cuttlefish.Specs.LogMessageTypeConvertorSpecs
{
    public class when_converting_logmessage_type_that_is_unknown
    {
        private static LogMessageTypeConverter converter;
        private static string expectedStringType;
        private static string result;

        private Establish context = () =>
            {
                converter = new LogMessageTypeConverter();
                expectedStringType = "Unknown";
            };

        private Because of = () =>
            {
                result = converter.ConvertLogMessageTypeToString(int.MaxValue);
            };

        private It should_return_that_the_logmessage_type_is_unknown_ = () =>
            {
                result.ShouldEqual(expectedStringType);
            };
    }
}