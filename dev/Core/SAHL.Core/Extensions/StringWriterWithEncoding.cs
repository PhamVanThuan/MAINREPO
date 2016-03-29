using System.IO;
using System.Text;

namespace SAHL.Core.Extensions
{
    public class StringWriterWithEncoding : StringWriter
    {
        private Encoding encoding;

        public StringWriterWithEncoding(Encoding encoding)
        {
            this.encoding = encoding;
        }

        public override Encoding Encoding
        {
            get { return encoding; }
        }
    }
}