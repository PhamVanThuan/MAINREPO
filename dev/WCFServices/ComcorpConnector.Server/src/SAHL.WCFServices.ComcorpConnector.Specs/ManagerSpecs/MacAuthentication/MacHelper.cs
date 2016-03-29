using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.MacAuthentication
{
    public class MacHelper
    {
        private readonly string xsdPrefix = "xsd";
        private readonly string xsdNamespace = "http://www.w3.org/2001/XMLSchema";
        private readonly string xsiPrefix = "xsi";
        private readonly string xsiNamespace = "http://www.w3.org/2001/XMLSchema-instance";
        private string privateKeyPath;
        private string publicKeyPath;

        public MacHelper()
        {
            var assemblyDirectory = AppDomain.CurrentDomain.BaseDirectory;
            publicKeyPath = ".\\PublicKey.xml";
            privateKeyPath = ".\\PrivateKey.xml";
        }

        public string CreateMessageMac<T>(T requestMessage)
        {
            var serializedMessage = SerializeMessage(requestMessage);
            var hashedMessage = HashMessage(serializedMessage);
            var encryptedHash = Encrypt(hashedMessage);
            return encryptedHash;
        }

        private string SerializeMessage<T>(T requestMessage)
        {
            string objectString = String.Empty;
            using (var stringWriter = new StringWriter())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add(xsdPrefix, xsdNamespace);
                ns.Add(xsiPrefix, xsiNamespace);
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stringWriter, requestMessage, ns);
                objectString = stringWriter.ToString();
                objectString = objectString.Replace("utf-16", "utf-8");
                objectString = SwapXmlNamespaces(objectString);
            }
            return objectString;
        }

        private string SwapXmlNamespaces(string objectString)
        {
            var xsdNamespaceString = String.Format("xmlns:{0}=\"{1}\"", xsdPrefix, xsdNamespace);
            var xsiNamespaceString = String.Format("xmlns:{0}=\"{1}\"", xsiPrefix, xsiNamespace);
            return objectString.Replace(String.Format("{0} {1}", xsdNamespaceString, xsiNamespaceString),
                 String.Format("{0} {1}", xsiNamespaceString, xsdNamespaceString));
        }

        private string HashMessage(string message)
        {
            SHA1Managed Sha1 = new SHA1Managed();
            var textArray = Encoding.ASCII.GetBytes(message);
            byte[] SHA1Result = Sha1.ComputeHash(textArray);
            return Convert.ToBase64String(SHA1Result);
        }

        private string Encrypt(string dataToEncrypt)
        {
            var dataBytes = Encoding.ASCII.GetBytes(dataToEncrypt);

            var publicKey = LoadPublicKey();
            var cryptoProvider = new RSACryptoServiceProvider();
            cryptoProvider.FromXmlString(publicKey);

            var encryptedData = cryptoProvider.Encrypt(dataBytes, false);
            var result = Convert.ToBase64String(encryptedData);
            return result;
        }

        private string LoadPublicKey()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(publicKeyPath);
            return xmlDocument.InnerXml;
        }
    }
}