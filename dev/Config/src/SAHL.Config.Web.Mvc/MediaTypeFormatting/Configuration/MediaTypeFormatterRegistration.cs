using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using SAHL.Core;

namespace SAHL.Config.Web.Mvc.MediaTypeFormatting.Configuration
{
    public class MediaTypeFormatterRegistration : IRegistrable
    {
        public IEnumerable<MediaTypeFormatter> MediaTypeFormatters { get; private set; }

        private readonly ICollection<MediaTypeFormatter> mediaTypeFormatterCollectionToAddTo;

        public MediaTypeFormatterRegistration(IEnumerable<MediaTypeFormatter> mediaTypeFormatters, ICollection<MediaTypeFormatter> mediaTypeFormatterCollectionToAddTo = null)
        {
            this.MediaTypeFormatters = mediaTypeFormatters ?? Enumerable.Empty<MediaTypeFormatter>();
            this.mediaTypeFormatterCollectionToAddTo = mediaTypeFormatterCollectionToAddTo;
        }

        public void Register()
        {
            if (!this.MediaTypeFormatters.Any())
            {
                return;
            }

            var collection = mediaTypeFormatterCollectionToAddTo ?? GlobalConfiguration.Configuration.Formatters;
            collection.Clear();
            foreach (var item in this.MediaTypeFormatters)
            {
                collection.Add(item);
            }
        }
    }
}
