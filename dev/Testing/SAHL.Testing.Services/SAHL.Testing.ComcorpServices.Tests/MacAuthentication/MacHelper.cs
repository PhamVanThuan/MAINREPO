using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SAHL.Testing.ComcorpServices.Tests
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
            publicKeyPath = Path.Combine(assemblyDirectory, "MacAuthentication\\PublicKey.xml");
            privateKeyPath = Path.Combine(assemblyDirectory, "MacAuthentication\\PrivateKey.xml");
        }

        public string CreateMessageMac<T>(T requestMessage)
        {
            RemoveMacFromRequestMessage(requestMessage);
            var serializedMessage = SerializeMessage(requestMessage);
            var hashedMessage = HashMessage(serializedMessage);
            var encryptedHash = Encrypt(hashedMessage);
            return encryptedHash;
        }

        public bool AuthenticateMessage<T>(T requestMessage, string requestMac)
        {
            RemoveMacFromRequestMessage(requestMessage);
            var objectString = SerializeMessage(requestMessage);
            var hashedMessage = HashMessage(objectString);
            var decryptedRequestMac = Decrypt(requestMac);

            return hashedMessage.Equals(decryptedRequestMac);
        }

        private void RemoveMacFromRequestMessage<T>(T requestMessage)
        {
            object source = requestMessage;
            string[] bits = "RequestHeader.RequestMac".Split('.');
            for (int i = 0; i < bits.Length - 1; i++)
            {
                var prop = source.GetType().GetProperty(bits[i]);
                source = prop.GetValue(source, null);
            }
            var propertyToSet = source.GetType().GetProperty(bits[bits.Length - 1]);
            propertyToSet.SetValue(source, "", null);
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
            // Sometimes the namespaces are serialized in the wrong order and the object isn't hashed correctly
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

        private string Decrypt(string dataToDecrypt)
        {
            byte[] dataBytes = Convert.FromBase64String(dataToDecrypt);

            var privateKey = LoadPrivateKey();
            RSACryptoServiceProvider rsaEncrypt = new RSACryptoServiceProvider();
            rsaEncrypt.FromXmlString(privateKey);
            var decryptedData = rsaEncrypt.Decrypt(dataBytes, false);
            var result = Encoding.ASCII.GetString(decryptedData);

            return result;
        }

        private string LoadPrivateKey()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(privateKeyPath);
            return xmlDocument.InnerXml;
        }

        private string LoadPublicKey()
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(publicKeyPath);
            return xmlDocument.InnerXml;
        }
    }
}