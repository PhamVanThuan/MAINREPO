using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Events
{
    public class ActiveNaturalPersonClientUpdatedEvent : Event
    {
        public ActiveNaturalPersonClientUpdatedEvent(DateTime date, string preferredName, SalutationType salutation, Education? education,
            Language homeLanguage, CorrespondenceLanguage correspondenceLanguage, string homePhoneCode, string homePhone, string workPhoneCode,
            string workPhone, string faxCode, string faxNumber, string cellphone, string emailAddress)
            : base(date)
        {
            this.Salutation = salutation;
            this.Education = education;
            this.HomeLanguage = homeLanguage;
            this.CorrespondenceLanguage = correspondenceLanguage;
            this.HomePhoneCode = homePhoneCode;
            this.HomePhone = homePhone;
            this.WorkPhoneCode = workPhoneCode;
            this.WorkPhone = workPhone;
            this.FaxCode = faxCode;
            this.FaxNumber = faxNumber;
            this.Cellphone = cellphone;
            this.EmailAddress = emailAddress;
            this.PreferredName = preferredName;
        }

        public string Cellphone { get; protected set; }

        public CorrespondenceLanguage CorrespondenceLanguage { get; protected set; }

        public Education? Education { get; protected set; }

        public string EmailAddress { get; protected set; }

        public string FaxCode { get; protected set; }

        public string FaxNumber { get; protected set; }

        public Language HomeLanguage { get; protected set; }

        public string HomePhone { get; protected set; }

        public string HomePhoneCode { get; protected set; }

        public string PreferredName { get; protected set; }

        public SalutationType Salutation { get; protected set; }

        public string WorkPhone { get; protected set; }

        public string WorkPhoneCode { get; protected set; }
    }
}