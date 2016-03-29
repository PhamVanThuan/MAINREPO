using System;
using System.IO;
using System.Web.Services.Protocols;

namespace SAHL.Common.WebServices
{
    public class SAHLSoapExtension : SoapExtension
    {
        public delegate void SerializeEvent(object sender, string xml);
        public static event SerializeEvent OnAfterSerialize;
        public static event SerializeEvent OnBeforeDeserialize;

        private Stream wireStream;
        private Stream applicationStream;

        // Save the Stream representing the SOAP request or SOAP response into
        // a local memory buffer.
        public override Stream ChainStream(Stream stream)
        {
            wireStream = stream;
            applicationStream = new MemoryStream();
            return applicationStream;
        }

        public override object GetInitializer(Type serviceType)
        {
            return null;
        }

        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return null;
        }

        public override void Initialize(object initializer)
        {
        }

        public override void ProcessMessage(SoapMessage message)
        {
            switch (message.Stage)
            {
                case SoapMessageStage.BeforeSerialize:
                    break;

                case SoapMessageStage.AfterSerialize:
                    string Request = WriteToWire(message);

                    if (OnAfterSerialize != null)
                        OnAfterSerialize(null, Request);

                    //StackTrace st = new StackTrace(false);
                    //StackFrame[] frames = st.GetFrames();
                    //MethodBase mb = st.GetFrame(7).GetMethod();
                    //ParameterInfo[] pi = mb.GetParameters();

                    //write the sql tables here!

                    //using (StreamWriter sw = File.CreateText("e:\\Request.txt"))
                    //{
                    //    sw.WriteLine(DateTime.Now.ToString("dd-MM-yy hh:mm"));
                    //    sw.WriteLine(Request);
                    //    sw.WriteLine();
                    //}
                    break;

                case SoapMessageStage.BeforeDeserialize:
                    string Response = ReadFromWire(message);

                    if (OnBeforeDeserialize != null)
                        OnBeforeDeserialize(null, Response);

                    //write the sql tables here!

                    //using (StreamWriter sw = File.CreateText("e:\\Response.txt"))
                    //{
                    //    sw.WriteLine(DateTime.Now.ToString("dd-MM-yy hh:mm"));
                    //    sw.WriteLine(Response);
                    //    sw.WriteLine();
                    //}
                    break;

                case SoapMessageStage.AfterDeserialize:
                    break;

                default:
                    throw new Exception("invalid stage");
            }
        }

        private string ReadFromWire(SoapMessage message)
        {
            try
            {
                applicationStream.Position = 0;
                return Copy(wireStream, applicationStream);
            }
            finally
            {
                applicationStream.Position = 0;
            }
        }

        private string WriteToWire(SoapMessage message)
        {
            applicationStream.Position = 0;
            return Copy(applicationStream, wireStream);
        }

        private string Copy(Stream from, Stream to)
        {
            TextReader reader = new StreamReader(from);
            TextWriter writer = new StreamWriter(to);
            string ret = reader.ReadToEnd();
            writer.WriteLine(ret);
            writer.Flush();
            return ret;
        }
    }

    // Create a SoapExtensionAttribute for a SOAP extension that can be
    // applied to an XML Web service method.
    [AttributeUsage(AttributeTargets.Method)]
    public class SAHLSoapExtensionAttribute : SoapExtensionAttribute
    {
        private int _priority;

        public override int Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = value;
            }
        }

        public override Type ExtensionType
        {
            get
            {
                return typeof(SAHLSoapExtension);
            }
        }

        public SAHLSoapExtensionAttribute()
            : base()
        {
        }
    }
}