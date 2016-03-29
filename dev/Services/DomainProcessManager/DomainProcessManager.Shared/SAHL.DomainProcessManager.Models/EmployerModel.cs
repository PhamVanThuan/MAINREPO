using System;
using System.Runtime.Serialization;

using SAHL.Core.Data;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class EmployerModel : IDataModel
    {
        public EmployerModel(int? employerKey, string employerName, string telephoneCode, string telephoneNumber, string contactPerson, string contactEmail, 
                             EmployerBusinessType employerBusinessType, EmploymentSector employmentSector)
        {
            this.EmployerKey          = employerKey;
            this.EmployerName         = employerName;
            this.TelephoneCode        = telephoneCode;
            this.TelephoneNumber      = telephoneNumber;
            this.ContactPerson        = contactPerson;
            this.ContactEmail         = contactEmail;
            this.EmployerBusinessType = employerBusinessType;
            this.EmploymentSector     = employmentSector;
        }

        [DataMember]
        public int? EmployerKey { get; set; }

        [DataMember]
        public string EmployerName { get; set; }

        [DataMember]
        public string TelephoneNumber { get; set; }

        [DataMember]
        public string TelephoneCode { get; set; }

        [DataMember]
        public string ContactPerson { get; set; }

        [DataMember]
        public string ContactEmail { get; set; }

        [DataMember]
        public EmployerBusinessType EmployerBusinessType { get; set; }

        [DataMember]
        public EmploymentSector EmploymentSector { get; set; }
    }
}
