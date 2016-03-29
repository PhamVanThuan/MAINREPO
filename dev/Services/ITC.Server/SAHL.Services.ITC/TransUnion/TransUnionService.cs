using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.ITC.TransUnionITC;
using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SAHL.Services.ITC.TransUnion
{
    public class TransUnionService : ITransUnionService
    {
        private ITransUnionServiceConfiguration serviceConfiguration;
        private ConsumerSoapClient client = new ConsumerSoapClient();
        private BureauEnquiry41 _Request = new BureauEnquiry41();
        private BureauResponse _Response = null;

        public TransUnionService(ITransUnionServiceConfiguration serviceConfiguration)
        {
            this.serviceConfiguration = serviceConfiguration;
        }

        public XmlDocument ResponseXml
        {
            get
            {
                MemoryStream m = new MemoryStream();
                XmlSerializer ser = new XmlSerializer(typeof(BureauResponse));
                ser.Serialize(m, _Response);

                m.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(m);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sr.ReadToEnd());
                return doc;
            }
        }

        public XmlDocument RequestXml
        {
            get
            {
                MemoryStream m = new MemoryStream();
                XmlSerializer ser = new XmlSerializer(typeof(BureauEnquiry41));
                ser.Serialize(m, _Request);

                m.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(m);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sr.ReadToEnd());
                return doc;
            }
        }

        public ItcResponse PerformRequest(ItcRequest request)
        {
            SetRequest(request);
            _Response = client.ProcessRequestTrans41(_Request, (Destination)Enum.Parse(typeof(Destination), this.serviceConfiguration.Destination));
            var serviceResponseStatus = (ServiceResponseStatus)Enum.Parse(typeof(ServiceResponseStatus), ResponseStatus.Success.ToString());
            var itcResponse = new ItcResponse(_Response.ErrorCode, _Response.ErrorMessage, DateTime.Now, serviceResponseStatus, 
                XDocument.Parse(RequestXml.InnerXml), XDocument.Parse(ResponseXml.InnerXml));
            return itcResponse;
        }

        private void SetRequest(ItcRequest itcRequest)
        {

            _Request.Forename1 = itcRequest.Forename1;
            _Request.Forename2 = itcRequest.Forename2;
            _Request.Forename3 = itcRequest.Forename3;
            _Request.Surname = itcRequest.Surname;
            _Request.BirthDate = itcRequest.BirthDate;
            _Request.IdentityNo1 = itcRequest.IdentityNo1;
            if (!string.IsNullOrEmpty(itcRequest.Sex))
            {
                _Request.Sex = (Sex)Enum.Parse(typeof(Sex), itcRequest.Sex);
            }
            _Request.Title = itcRequest.Title;
            _Request.MaritalStatus = itcRequest.MaritalStatus;
            _Request.HomeTelCode = itcRequest.HomeTelCode;
            _Request.HomeTelNo = itcRequest.HomeTelNo;
            _Request.WorkTelCode = itcRequest.WorkTelCode;
            _Request.WorkTelNo = itcRequest.WorkTelNo;
            _Request.CellNo = itcRequest.CellNo;
            _Request.EmailAddress = itcRequest.EmailAddress;
            _Request.AddressLine1 = itcRequest.AddressLine1;
            _Request.AddressLine2 = itcRequest.AddressLine2;
            _Request.Suburb = itcRequest.Suburb;
            _Request.City = itcRequest.City;
            _Request.PostalCode = itcRequest.PostalCode;
            _Request.Address2Line1 = itcRequest.Address2Line1;
            _Request.Address2Line2 = itcRequest.Address2Line2;
            _Request.Address2Suburb = itcRequest.Address2Suburb;
            _Request.Address2City = itcRequest.Address2City;
            _Request.Address2PostalCode = itcRequest.Address2PostalCode;

        }
    }
}