using System;
using System.Xml.Linq;

namespace SAHL.Services.Interfaces.ITC.Models
{
    public class ItcResponse
    {
        public ItcResponse(string errorCode, string errorMessage, DateTime itcDate, ServiceResponseStatus responseStatus, XDocument request, XDocument response)
        {
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;
            this.ITCDate = itcDate;
            this.ResponseStatus = responseStatus;
            this.Request = request;
            this.Response = response;
        }

        public string ErrorCode { get; protected set; }

        public string ErrorMessage { get; protected set; }

        public DateTime ITCDate { get; protected set; }

        public XDocument Request { get; protected set; }

        public XDocument Response { get; protected set; }

        public ServiceResponseStatus ResponseStatus { get; protected set; }
    }
}