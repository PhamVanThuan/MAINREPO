using System;
using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Models.Core;

namespace SAHL.Services.Query.Models.QueryDataModels
{
    public class LegalEntity //: IDataModel
    {

        public LegalEntity()
        {
            AddPrimaryKey();
            AddRelationships();
        }

        public IPrimaryKey PrimaryKey { get; set; }
        public List<IRelationshipDefinition> Relationships { get; set; } 

        public int LegalEntityKey { get; set; }
        public int? LegalEntityTypeKey { get; set; }
        public int? MaritalStatusKey { get; set; }
        public int? GenderKey { get; set; }
        public int? PopulationGroupKey { get; set; }
        public DateTime? IntroductionDate { get; set; }
        public int? Salutationkey { get; set; }
        public string FirstNames { get; set; }
        public string Initials { get; set; }
        public string Surname { get; set; }
        public string PreferredName { get; set; }
        public string IDNumber { get; set; }
        public string PassportNumber { get; set; }
        public string TaxNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string RegisteredName { get; set; }
        public string TradingName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string HomePhoneCode { get; set; }
        public string HomePhoneNumber { get; set; }
        public string WorkPhoneCode { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string CellPhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string FaxCode { get; set; }
        public string FaxNumber { get; set; }
        public string Password { get; set; }
        public int? CitizenTypeKey { get; set; }
        public int? LegalEntityStatusKey { get; set; }
        public string Comments { get; set; }
        public int? LegalEntityExceptionStatusKey { get; set; }
        public string UserID { get; set; }
        public DateTime? ChangeDate { get; set; }
        public int? EducationKey { get; set; }
        public int? HomeLanguageKey { get; set; }
        public int? DocumentLanguageKey { get; set; }
        public int? ResidenceStatusKey { get; set; }

        private void AddRelationships()
        {
            Relationships = new List<IRelationshipDefinition>();
        }

        private void AddPrimaryKey()
        {
            PrimaryKey = new PrimaryKey()
            {
                Alias = "Id",
                Key = "LegalEntityKey"
            };
        }

    }
}