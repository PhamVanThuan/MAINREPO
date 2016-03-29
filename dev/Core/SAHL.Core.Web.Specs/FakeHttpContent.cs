using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SAHL.Core.Web.Specs
{
    internal class FakeHttpContent : HttpContent
    {
        string testString;

        public FakeHttpContent(string fakeContent)
        {
            this.testString = fakeContent;
        }

        protected override Task SerializeToStreamAsync(System.IO.Stream stream, System.Net.TransportContext context)
        {
            return Task.Run(() =>
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(testString);
            });
        }

        protected override bool TryComputeLength(out long length)
        {
            length = testString.Length;
            return true;
        }
    }
}