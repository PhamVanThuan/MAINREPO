using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using WebApi.Hal;
using Westwind.Utilities.Dynamic;

namespace SAHL.Services.Query.Controllers.Test
{
    [ServiceGenerationToolExclude]
    public class TestDynamicController : ApiController
    {
        private readonly ILinkResolver linkResolver;

        public TestDynamicController(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        [RepresentationRoute("/api/TestDynamic", typeof(object))]
        public JToken Get()
        {
            var items = Enumerable.Range(0, 3)
                .Select(a =>
                {
                    dynamic item = new ExpandoObject();
                    item.Id = a;
                    item.Name = "Item " + a;

                    return item;
                })
                .ToList();

            var representations = items.Select(a =>
            {
                var testRepresentation = new TestRepresentation(linkResolver);
                testRepresentation.Id = a.Id;
                return testRepresentation;
            })
            .Cast<Representation>()
            .ToList();
            
            var result = new QueryResult
            {
                Representation = new TestListRepresentation(linkResolver, representations),
                Content = items,
            };

            string serialisedRepresentation;
            string serialisedContent;

            var halFormatter = GlobalConfiguration.Configuration.Formatters.Single(a => a.GetType().TypeHandle.Equals(typeof (JsonHalMediaTypeFormatter).TypeHandle));

            using (var stream = new MemoryStream())
            {
                var stringContent = new StringContent(string.Empty);
                halFormatter.WriteToStreamAsync(result.Representation.GetType(), result.Representation, stream, stringContent, null).Wait();
                stream.Seek(0L, SeekOrigin.Begin);
                using (var reader = new StreamReader(stream))
                {
                    serialisedRepresentation = reader.ReadToEnd();
                }
            }

            var jsonFormatter = new JsonMediaTypeFormatter();

            using (var stream = new MemoryStream())
            {
                var stringContent = new StringContent(string.Empty);
                jsonFormatter.WriteToStreamAsync(result.Content.GetType(), result.Content, stream, stringContent, null).Wait();
                stream.Seek(0L, SeekOrigin.Begin);
                using (var reader = new StreamReader(stream))
                {
                    serialisedContent = reader.ReadToEnd();
                }
            }

            var content = @"{
    representation: " + serialisedRepresentation + @",
    content: " + serialisedContent + @"
}";

            return JToken.Parse(content);
        }

        [RepresentationRoute("/api/TestDynamic/{id}", typeof(object))]
        public QueryResult Get(int id)
        {
            var representation = new TestRepresentation(linkResolver);
            representation.Id = 1;
            representation.Value = "Value" + representation.Id;
            return new QueryResult
            {
                Representation = representation,
                Content = new Expando(new
                {
                    Id = 10,
                    Name = "Banana",
                }),
            };
        }
    }
}