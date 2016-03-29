using SAHL.WCFServices.ComcorpConnector.Interfaces;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Managers.MacAuthentication
{
    public class MacAuthenticationManager : IMacAuthenticationManager
    {
        private IComcorpConnectorSettings comcorpConnectorSettings;

        public MacAuthenticationManager(IComcorpConnectorSettings comcorpConnectorSettings)
        {
            this.comcorpConnectorSettings = comcorpConnectorSettings;
        }

        public bool AuthenticateMessage<T>(T requestMessage, string requestMac)
        {
            var objectString = SerializeMessage(requestMessage);
            var hashedMessage = HashMessage(objectString);
            var decryptedRequestMac = GetDecryptedRequestMac(requestMac);

            return hashedMessage.Equals(decryptedRequestMac);
        }

        internal string SerializeMessage<T>(T requestMessage)
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
            // Sometimes the namespaces are serialized in the wrong order and the object isn't hashed correctly
            var xsdNamespaceString = String.Format("xmlns:{0}=\"{1}\"", xsdPrefix, xsdNamespace);
            var xsiNamespaceString = String.Format("xmlns:{0}=\"{1}\"", xsiPrefix, xsiNamespace);
            return objectString.Replace(String.Format("{0} {1}", xsdNamespaceString, xsiNamespaceString),
                 String.Format("{0} {1}", xsiNamespaceString, xsdNamespaceString));
        }

        public string GetDecryptedRequestMac(string requestMac)
        {
            byte[] requestMacBytes = Convert.FromBase64String(requestMac);

            var privateKey = LoadPrivateKey(comcorpConnectorSettings.DocumentsPrivateKeyPath);
            RSACryptoServiceProvider rsaEncrypt = new RSACryptoServiceProvider();
            rsaEncrypt.FromXmlString(privateKey);
            var cipherText = rsaEncrypt.Decrypt(requestMacBytes, false);
            var decryptedRequestMac = Encoding.ASCII.GetString(cipherText);

            return decryptedRequestMac;
        }

        private string LoadPrivateKey(string privateKeyPath)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(privateKeyPath);
            return xmlDocument.InnerXml;
        }

        public string HashMessage(string message)
        {
            SHA1Managed Sha1 = new SHA1Managed();
            var textArray = Encoding.ASCII.GetBytes(message);
            byte[] SHA1Result = Sha1.ComputeHash(textArray);
            return Convert.ToBase64String(SHA1Result);
        }

        private readonly string xsdPrefix = "xsd";
        private readonly string xsdNamespace = "http://www.w3.org/2001/XMLSchema";
        private readonly string xsiPrefix = "xsi";
        private readonly string xsiNamespace = "http://www.w3.org/2001/XMLSchema-instance";
    }
}