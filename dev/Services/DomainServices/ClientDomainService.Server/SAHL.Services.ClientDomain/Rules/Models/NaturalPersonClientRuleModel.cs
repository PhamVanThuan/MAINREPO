using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.ClientDomain.Rules.Models
{
    public class NaturalPersonClientRuleModel : NaturalPersonClientModel
    {
        public NaturalPersonClientRuleModel(int clientKey, string idNumber, string passportNumber, SalutationType? salutation, string firstName, string surname, string initials, string preferredName,
            Gender? gender, MaritalStatus? maritalStatus, PopulationGroup? populationGroup, CitizenType? citizenshipType, DateTime? dateOfBirth, Language? homeLanguage,
            CorrespondenceLanguage? correspondenceLanguage, Education? education, string homePhoneCode, string homePhone, string workPhoneCode, string workPhone, string faxCode,
            string faxNumber, string cellphone, string emailAddress)
            : base(idNumber, passportNumber, salutation, firstName, surname, initials, preferredName, gender, maritalStatus, populationGroup, citizenshipType, dateOfBirth, homeLanguage,
            correspondenceLanguage, education, homePhoneCode, homePhone, workPhoneCode, workPhone, faxCode, faxNumber, cellphone, emailAddress)
        {
            this.ClientKey = clientKey;
        }

        public NaturalPersonClientRuleModel(int clientKey, NaturalPersonClientModel naturalPersonClient)
            : this(clientKey, naturalPersonClient.IDNumber, naturalPersonClient.PassportNumber, naturalPersonClient.Salutation, naturalPersonClient.FirstName,
            naturalPersonClient.Surname, naturalPersonClient.Initials, naturalPersonClient.PreferredName, naturalPersonClient.Gender, naturalPersonClient.MaritalStatus,
            naturalPersonClient.PopulationGroup, naturalPersonClient.CitizenshipType, naturalPersonClient.DateOfBirth, naturalPersonClient.HomeLanguage, naturalPersonClient.CorrespondenceLanguage,
            naturalPersonClient.Education, naturalPersonClient.HomePhoneCode, naturalPersonClient.HomePhone, naturalPersonClient.WorkPhoneCode, naturalPersonClient.WorkPhone,
            naturalPersonClient.FaxCode, naturalPersonClient.FaxNumber, naturalPersonClient.Cellphone, naturalPersonClient.EmailAddress)
        {
        }

        public int ClientKey { get; protected set; }
    }
}