using SAHL.Core.Extensions;
using SAHL.Services.Communications.ComcorpLiveRepliesService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SAHL.Services.Communications.Managers.LiveReplies
{
    public class LiveRepliesManager : ILiveRepliesManager
    {
        public string ComcorpLiveRepliesServiceVersion { get; private set; }

        public ProcessBankLiveRepliesRequestServiceHeaderBankId? ComcorpLiveRepliesServiceBankId { get; private set; }

        public string InternalEmailFromAddress { get; private set; }

        private IDictionary<int, string> ComcorpLiveRepliesReplyStatuses = new Dictionary<int, string>();

        private string comcorpLiveRepliesWebserviceURL { get; set; }

        private string comcorpLiveRepliesPublicKey { get; set; }

        private ICommunicationSettings communicationSettings;

        public LiveRepliesManager(ICommunicationSettings communicationSettings)
        {
            this.communicationSettings = communicationSettings;
        }

        public void Start()
        {
            // use core config provider
            ComcorpLiveRepliesServiceVersion = communicationSettings.ComcorpLiveRepliesServiceVersion;
            comcorpLiveRepliesWebserviceURL = communicationSettings.ComcorpLiveRepliesWebserviceURL;
            string comcorpLiveRepliesPublicKeyModulus = communicationSettings.ComcorpLiveRepliesPublicKeyModulus;
            string comcorpLiveRepliesPublicKeyExponent = communicationSettings.ComcorpLiveRepliesPublicKeyExponent;
            ComcorpLiveRepliesServiceBankId = GetProcessBankLiveRepliesRequestServiceHeaderBankIdFromBankId(communicationSettings.ComcorpLiveRepliesBankId);
            InternalEmailFromAddress = communicationSettings.InternalEmailFromAddress;

            if (ComcorpLiveRepliesServiceBankId == null ||
                string.IsNullOrWhiteSpace(comcorpLiveRepliesWebserviceURL) ||
                string.IsNullOrWhiteSpace(comcorpLiveRepliesPublicKeyModulus) ||
                string.IsNullOrWhiteSpace(comcorpLiveRepliesPublicKeyExponent) ||
                string.IsNullOrWhiteSpace(ComcorpLiveRepliesServiceVersion) ||
                string.IsNullOrWhiteSpace(InternalEmailFromAddress))
            {
                throw new Exception("Missing configuration values for comcorp live replies");
            }

            comcorpLiveRepliesPublicKey = "<RSAKeyValue><Modulus>" + comcorpLiveRepliesPublicKeyModulus + "</Modulus><Exponent>" + comcorpLiveRepliesPublicKeyExponent + "</Exponent></RSAKeyValue>";

            ComcorpLiveRepliesReplyStatuses.Add(1, "Request has been received successfully");
            ComcorpLiveRepliesReplyStatuses.Add(2, "An Unknown Error has occurred - Please contact Support");
            ComcorpLiveRepliesReplyStatuses.Add(3, "Unable to load XML request document - Please contact Support");
            ComcorpLiveRepliesReplyStatuses.Add(4, "Invalid Application.MAC detected - Please correct value and resend");
            ComcorpLiveRepliesReplyStatuses.Add(8, "Failed to authenticate request - Please contact Support");
            ComcorpLiveRepliesReplyStatuses.Add(9, "Failed to parse application according to XSD Schema - Please contact Support");
        }

        public void Stop()
        {
            ComcorpLiveRepliesServiceVersion = string.Empty;
            ComcorpLiveRepliesServiceBankId = null;
            comcorpLiveRepliesWebserviceURL = string.Empty;
            comcorpLiveRepliesPublicKey = string.Empty;
            InternalEmailFromAddress = string.Empty;
        }

        public string CreateComcorpMessageAuthenticationCodeFromXml(string xmlString)
        {
            byte[] textArray;
            byte[] plainText;
            byte[] cipherText;
            byte[] sha1Result;

            SHA1Managed sha1 = new SHA1Managed();
            StringBuilder sha1String = new StringBuilder(40);
            RSACryptoServiceProvider rsaEncrypt = new RSACryptoServiceProvider();

            textArray = Encoding.ASCII.GetBytes(xmlString);
            sha1Result = sha1.ComputeHash(textArray);

            for (int i = 0; i <= sha1Result.Length - 1; i++)
            {
                sha1String.Append(String.Format("{0:x2}", sha1Result[i]).ToUpper());
            }

            rsaEncrypt.FromXmlString(comcorpLiveRepliesPublicKey);
            plainText = Encoding.ASCII.GetBytes(sha1String.ToString());
            cipherText = rsaEncrypt.Encrypt(plainText, false);
            return Convert.ToBase64String(cipherText);
        }

        public string GenerateXmlStringFromObject<T>(T liveReplyRequest)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(liveReplyRequest.GetType());
            StringWriterWithEncoding stringWriterUTF8 = new StringWriterWithEncoding(Encoding.UTF8);

            xmlSerializer.Serialize(stringWriterUTF8, liveReplyRequest);
            return stringWriterUTF8.ToString();
        }

        public ProcessBankLiveRepliesRequestLiveReplyEventId? GetProcessBankLiveRepliesRequestLiveReplyEventIdFromEventId(int eventId)
        {
            Type enumType = typeof(ProcessBankLiveRepliesRequestLiveReplyEventId);

            foreach (Enum val in Enum.GetValues(enumType))
            {
                FieldInfo fi = enumType.GetField(val.ToString());
                XmlEnumAttribute[] attributes = (XmlEnumAttribute[])fi.GetCustomAttributes(typeof(XmlEnumAttribute), false);
                if (attributes.Length > 0)
                {
                    XmlEnumAttribute attr = attributes[0];
                    if (attr.Name == eventId.ToString())
                    {
                        return (ProcessBankLiveRepliesRequestLiveReplyEventId)val;
                    }
                }
            }
            return null;
        }

        // This needs an integration test
        public string ProcessBankLiveReplies(string requestXml)
        {
            LiveRepliesServiceSoapClient liveRepliesServiceSoapClient = new LiveRepliesServiceSoapClient("LiveRepliesServiceSoap", comcorpLiveRepliesWebserviceURL);
            return liveRepliesServiceSoapClient.ProcessBankLiveReplies(requestXml);
        }

        public Tuple<int, string> GetComcorpLiveRepliesReplyStatus(string resultXml)
        {
            try
            {
                XDocument xDocument = XDocument.Parse(resultXml);
                int statusId;
                int.TryParse(xDocument.Descendants().FirstOrDefault(x => x.Name == "Service.Result").Value, out statusId);
                string status = GetComcorpLiveRepliesReplyStatuseByStatusId((int)statusId);

                if (string.IsNullOrWhiteSpace(status))
                {
                    return null;
                }

                return Tuple.Create(statusId, status);
            }
            catch
            {
                return null;
            }
        }

        private ProcessBankLiveRepliesRequestServiceHeaderBankId? GetProcessBankLiveRepliesRequestServiceHeaderBankIdFromBankId(string bankId)
        {
            Type enumType = typeof(ProcessBankLiveRepliesRequestServiceHeaderBankId);

            foreach (Enum val in Enum.GetValues(enumType))
            {
                FieldInfo fi = enumType.GetField(val.ToString());
                XmlEnumAttribute[] attributes = (XmlEnumAttribute[])fi.GetCustomAttributes(typeof(XmlEnumAttribute), false);
                if (attributes.Length > 0)
                {
                    XmlEnumAttribute attr = attributes[0];
                    if (attr.Name == bankId)
                    {
                        return (ProcessBankLiveRepliesRequestServiceHeaderBankId)val;
                    }
                }
            }
            return null;
        }

        private string GetComcorpLiveRepliesReplyStatuseByStatusId(int statusId)
        {
            string replyStatus;
            if (ComcorpLiveRepliesReplyStatuses == null || ComcorpLiveRepliesReplyStatuses.Count == 0)
            {
                return null;
            }

            ComcorpLiveRepliesReplyStatuses.TryGetValue(statusId, out replyStatus);
            return replyStatus;
        }
    }
}