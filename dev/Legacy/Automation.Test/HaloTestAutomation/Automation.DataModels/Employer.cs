using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class Employer : IComparable<Employer>
    {
        public int EmployerKey { get; set; }

        public string Name { get; set; }

        public string TelephoneNumber { get; set; }

        public string TelephoneCode { get; set; }

        public string ContactPerson { get; set; }

        public string ContactEmail { get; set; }

        public string AccountantName { get; set; }

        public string AccountantContactPerson { get; set; }

        public string AccountantTelephoneCode { get; set; }

        public string AccountantTelephoneNumber { get; set; }

        public string AccountantEmail { get; set; }

        public EmployerBusinessTypeEnum EmployerBusinessTypeKey { get; set; }

        public string UserID { get; set; }

        public DateTime ChangeDate { get; set; }

        public EmploymentSectorEnum EmploymentSectorKey { get; set; }

        public string EmployerBusinessTypeDescription { get; set; }

        public string EmploymentSectorDescription { get; set; }

        public int CompareTo(Employer other)
        {
            if (!this.TelephoneNumber.Equals(other.TelephoneNumber))
                return 0;
            if (!this.TelephoneCode.Equals(other.TelephoneCode))
                return 0;
            if (!this.ContactPerson.Equals(other.ContactPerson))
                return 0;
            if (!this.ContactEmail.Equals(other.ContactEmail))
                return 0;
            if (!this.AccountantName.Equals(other.AccountantName))
                return 0;
            if (!this.AccountantContactPerson.Equals(other.AccountantContactPerson))
                return 0;
            if (!this.AccountantTelephoneCode.Equals(other.AccountantTelephoneCode))
                return 0;
            if (!this.AccountantTelephoneNumber.Equals(other.AccountantTelephoneNumber))
                return 0;
            if (!this.AccountantEmail.Equals(other.AccountantEmail))
                return 0;
            if (!this.EmployerBusinessTypeKey.Equals(other.EmployerBusinessTypeKey))
                return 0;
            if (!this.UserID.Equals(other.UserID))
                return 0;
            if (!this.EmploymentSectorKey.Equals(other.EmploymentSectorKey))
                return 0;
            return 1;
        }
    }
}