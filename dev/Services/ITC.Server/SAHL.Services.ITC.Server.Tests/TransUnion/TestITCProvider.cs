using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using SAHL.Services.Interfaces.ITC.Models;
using TransUnionMocks;

namespace SAHL.Services.ITC.Tests.TransUnion
{
    public static class TestITCProvider
    {
        public static ItcResponse GetTestITC(string fileName = "default_response.xml")
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TransUnion/TestITCs", fileName);
            var reader = XmlReader.Create(path);
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(BureauResponse), "https://secure.transunion.co.za/TUBureau");
            var response = (BureauResponse)serializer.Deserialize(reader);
            reader.Close();
            var enquiry = new BureauEnquiry41() { IdentityNo1 = "8211045229080" };
            response.UniqueRefGuid = Guid.NewGuid();
            if (response.ConsumerInfoNO04 != null)
            {
                response.ConsumerInfoNO04.IdentityNo1 = enquiry.IdentityNo1;
            }
            var date = DateTime.Now;
            switch (fileName)
            {
                case "Judgements 1 and value 10 000 ITC.xml":
                    response.JudgementsNJ07[0].JudgmentDate = date.AddDays(1).AddYears(-3).ToString("yyyyMMdd");
                    break;

                case "Judgements 2 and value 10 000 ITC.xml":
                    response.JudgementsNJ07[0].JudgmentDate = date.ToString("yyyyMMdd");
                    response.JudgementsNJ07[1].JudgmentDate = date.AddDays(1).AddYears(-3).ToString("yyyyMMdd");
                    break;

                case "Unsettled defaults of 3 within two years ITC.xml":
                    response.DefaultsD701Part1[0].WrittenOffDate = date.ToString("yyyyMMdd");
                    response.DefaultsD701Part1[1].WrittenOffDate = date.AddDays(1).AddYears(-1).ToString("yyyyMMdd");
                    response.DefaultsD701Part1[2].WrittenOffDate = date.AddDays(1).AddYears(-2).ToString("yyyyMMdd");
                    break;

                default:
                    break;
            }
            var itcRequest = GetDefaultRequest();
            var responseXml = GetResponseXml(response);
            var itcResponse = new ItcResponse(response.ErrorCode,
                response.ErrorMessage,
                DateTime.Now,
                ServiceResponseStatus.Success,
                itcRequest,
                XDocument.Parse(responseXml.InnerXml));
            return itcResponse;
        }

        /// <summary>
        ///     Returns the response from ITC in Xml.
        /// </summary>
        private static XmlDocument GetResponseXml(BureauResponse response)
        {
            MemoryStream m = new MemoryStream();
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(BureauResponse));
            ser.Serialize(m, response);

            m.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(m);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sr.ReadToEnd());
            return doc;
        }

        private static XDocument GetDefaultRequest()
        {
            const string request =
                "<BureauEnquiry41 xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                    "<ValidationError xmlns=\"https://secure.transunion.co.za/TUBureau\">false</ValidationError>" +
                    "<SubscriberCode xmlns=\"https://secure.transunion.co.za/TUBureau\">35792</SubscriberCode>" +
                    "<ClientReference xmlns=\"https://secure.transunion.co.za/TUBureau\">Home Loan Enquiry</ClientReference>" +
                    "<SecurityCode xmlns=\"https://secure.transunion.co.za/TUBureau\">SHL92</SecurityCode>" +
                    "<EnquirerContactName xmlns=\"https://secure.transunion.co.za/TUBureau\">SA Home Loans</EnquirerContactName>" +
                    "<EnquirerContactPhoneNo xmlns=\"https://secure.transunion.co.za/TUBureau\">031 560 5300</EnquirerContactPhoneNo>" +
                    "<Surname xmlns=\"https://secure.transunion.co.za/TUBureau\">Surname</Surname>" +
                    "<Forename1 xmlns=\"https://secure.transunion.co.za/TUBureau\">FirstName</Forename1>" +
                    "<BirthDate xmlns=\"https://secure.transunion.co.za/TUBureau\">19780218</BirthDate>" +
                    "<IdentityNo1 xmlns=\"https://secure.transunion.co.za/TUBureau\">7802185089080</IdentityNo1>" +
                    "<Sex xmlns=\"https://secure.transunion.co.za/TUBureau\">M</Sex>" +
                    "<Title xmlns=\"https://secure.transunion.co.za/TUBureau\">Mr</Title>" +
                    "<AddressLine2 xmlns=\"https://secure.transunion.co.za/TUBureau\">38 Kensington Drive</AddressLine2>" +
                    "<Suburb xmlns=\"https://secure.transunion.co.za/TUBureau\">Durban North (Kwazulu-natal)</Suburb>" +
                    "<City xmlns=\"https://secure.transunion.co.za/TUBureau\">Durban (Kwazulu-natal)</City>" +
                    "<PostalCode xmlns=\"https://secure.transunion.co.za/TUBureau\">4051</PostalCode>" +
                    "<WorkTelNo xmlns=\"https://secure.transunion.co.za/TUBureau\">0315765000</WorkTelNo>" +
                    "<Address2PostalCode xmlns=\"https://secure.transunion.co.za/TUBureau\" />" +
                    "<CellNo xmlns=\"https://secure.transunion.co.za/TUBureau\">0782003001</CellNo>" +
                    "<EmailAddress xmlns=\"https://secure.transunion.co.za/TUBureau\">test@sahomeloans.com</EmailAddress>" +
                    "</BureauEnquiry41>";
            XDocument xdoc = XDocument.Parse(request);
            return xdoc;
        }
    }
}
