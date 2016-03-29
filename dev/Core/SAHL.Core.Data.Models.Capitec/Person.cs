using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class PersonDataModel :  IDataModel
    {
        public PersonDataModel(Guid salutationEnumId, string firstName, string surname, string identityNumber)
        {
            this.SalutationEnumId = salutationEnumId;
            this.FirstName = firstName;
            this.Surname = surname;
            this.IdentityNumber = identityNumber;
		
        }

        public PersonDataModel(Guid id, Guid salutationEnumId, string firstName, string surname, string identityNumber)
        {
            this.Id = id;
            this.SalutationEnumId = salutationEnumId;
            this.FirstName = firstName;
            this.Surname = surname;
            this.IdentityNumber = identityNumber;
		
        }		

        public Guid Id { get; set; }

        public Guid SalutationEnumId { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string IdentityNumber { get; set; }
    }
}