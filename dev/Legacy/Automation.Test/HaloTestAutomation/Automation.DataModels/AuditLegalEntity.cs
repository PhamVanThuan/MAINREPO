using System;
namespace Automation.DataModels
{
    public class AuditLegalEntity: IComparable<AuditLegalEntity>
    {
        public string FirstNames { get; set; }
        public string Surname { get; set; }
        public string HomePhoneCode { get; set; }
        public string HomePhoneNumber { get; set; }
        public string WorkPhoneCode { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public int CompareTo(AuditLegalEntity other)
        {
            if (this.FirstNames != other.FirstNames)
                return 0;
            if (this.Surname != other.Surname)
                return 0;
            if (this.HomePhoneCode != other.HomePhoneCode)
                return 0;
            if (this.HomePhoneNumber != other.HomePhoneNumber)
                return 0;
            if (this.WorkPhoneCode != other.WorkPhoneCode)
                return 0;
            if (this.WorkPhoneNumber != other.WorkPhoneNumber)
                return 0;
            if (this.EmailAddress != other.EmailAddress)
                return 0;
            return 1;
        }
    }
}
