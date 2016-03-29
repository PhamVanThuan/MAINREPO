using System.Collections.Generic;
using System.Net.Http.Formatting;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Config.Web.Mvc.MediaTypeFormatting;
using SAHL.Config.Web.Mvc.MediaTypeFormatting.Configuration;
using SAHL.Core;

namespace SAHL.Config.Web.Mvc.Specs.CustomMediaTypeFormatterSpecs
{
    public class when_adding_custom_media_type_formatters_for_registration : WithFakes
    {
        Establish that = () =>
        {
            container = An<IIocContainer>();

            newCustomFormatter = new BsonMediaTypeFormatter();
            customFormatters = new List<MediaTypeFormatter>
            {
                newCustomFormatter,
            };

            existingMediaTypeFormatter = new JsonMediaTypeFormatter();
            mediaTypeFormatterCollection = new MediaTypeFormatterCollection
            {
                existingMediaTypeFormatter,
            };

            mediaTypeFormatterRegistration = new MediaTypeFormatterRegistration(customFormatters, mediaTypeFormatterCollection);
        };

        private Because of = () =>
        {
            mediaTypeFormatterRegistration.Register();
        };

        private It should_have_added_the_custom_formats = () =>
        {
            mediaTypeFormatterCollection.ShouldContainOnly(newCustomFormatter);
        };

        private It should_have_cleared_the_existing_formatters_before_adding = () =>
        {
            mediaTypeFormatterCollection.ShouldNotContain(existingMediaTypeFormatter);
        };

        private static IIocContainer container;
        private static List<MediaTypeFormatter> customFormatters;
        private static MediaTypeFormatterRegistration mediaTypeFormatterRegistration;
        private static MediaTypeFormatterCollection mediaTypeFormatterCollection;
        private static JsonMediaTypeFormatter existingMediaTypeFormatter;
        private static BsonMediaTypeFormatter newCustomFormatter;
    }
}
