﻿using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.ClientDomain.Events
{
    public class NaturalPersonClientAddedEvent : Event
    {
        public NaturalPersonClientAddedEvent(DateTime date, int clientKey, MaritalStatus? maritalStatus, Gender? gender, PopulationGroup? populationGroup,
            DateTime introductionDate, SalutationType? salutation, string firstName, string surname, string prefferedName, string idNumber, string passportNumber,
            DateTime? dateOfBirth, string homePhoneCode, string homePhone, string workPhoneCode, string workPhone, string cellphone, string emailAddress, string faxCode, string faxNumber
            , CitizenType? citizenshipType, Education education)
            : base(date)
        {
            this.ClientKey = clientKey;

            this.MaritalStatus = maritalStatus;
            this.Gender = gender;
            this.PopulationGroup = populationGroup;
            this.IntroductionDate = introductionDate;
            this.Salutation = salutation;
            this.FirstName = firstName;
            this.Surname = surname;
            this.PrefferedName = prefferedName;
            this.IDNumber = idNumber;
            this.PassportNumber = passportNumber;
            this.DateOfBirth = dateOfBirth;
            this.HomePhoneCode = homePhoneCode;
            this.HomePhone = HomePhone;
            this.WorkPhoneCode = workPhoneCode;
            this.WorkPhone = WorkPhone;
            this.Cellphone = cellphone;
            this.EmailAddress = emailAddress;
            this.FaxCode = faxCode;
            this.FaxNumber = FaxNumber;

            this.CitizenshipType = citizenshipType;
            this.Education = education;
        }

        public int ClientKey { get; protected set; }

        public MaritalStatus? MaritalStatus { get; protected set; }

        public Gender? Gender { get; set; }

        public PopulationGroup? PopulationGroup { get; set; }

        public DateTime IntroductionDate { get; set; }

        public SalutationType? Salutation { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string PrefferedName { get; set; }

        public string IDNumber { get; set; }

        public string PassportNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string HomePhoneCode { get; set; }

        public string HomePhone { get; set; }

        public string WorkPhoneCode { get; set; }

        public string WorkPhone { get; set; }

        public string Cellphone { get; set; }

        public string EmailAddress { get; set; }

        public string FaxCode { get; set; }

        public string FaxNumber { get; set; }

        public CitizenType? CitizenshipType { get; set; }

        public Education Education { get; set; }
    }
}