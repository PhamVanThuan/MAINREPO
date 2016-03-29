using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Rules.AtLeastOneClientContactDetailShouldBeProvidedSpecs
{
    public class ClientContactDetailsTestModel : IClientContactDetails
    {
        public ClientContactDetailsTestModel(string homePhoneCode, string homePhone, string workPhoneCode, string workPhone, string faxCode, string faxNumber, string cellphone, string emailAddress)
        {
            this.HomePhoneCode = homePhoneCode;
            this.HomePhone = homePhone;
            this.WorkPhoneCode = workPhoneCode;
            this.WorkPhone = workPhone;
            this.FaxCode = faxCode;
            this.FaxNumber = faxNumber;
            this.Cellphone = cellphone;
            this.EmailAddress = emailAddress;
        }

        public string HomePhoneCode { get; protected set; }

        public string HomePhone { get; protected set; }

        public string WorkPhoneCode { get; protected set; }

        public string WorkPhone { get; protected set; }

        public string FaxCode { get; protected set; }

        public string FaxNumber { get; protected set; }

        public string Cellphone { get; protected set; }

        public string EmailAddress { get; protected set; }
    }
}