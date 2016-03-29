using Common.Enums;
using System;
using System.Collections.Generic;

namespace Automation.DataModels
{
    public class LegalEntityRole : IComparable<LegalEntityRole>
    {
        public int LegalEntityKey { get; set; }

        public string LegalEntityLegalName { get; set; }

        public RoleTypeEnum RoleTypeKey { get; set; }

        public GeneralStatusEnum GeneralStatusKey { get; set; }

        public string IDNumber { get; set; }

        public int LegalEntityTypeKey { get; set; }

        public string RoleDescription { get; set; }

        public int ProductKey { get; set; }

        public int AccountKey { get; set; }

        public DateTime DateOfBirth { get; set; }

        public List<LegalEntityBankAccount> LegalEntityBankAccounts { get; set; }

        public int CompareTo(LegalEntityRole other)
        {
            if (this.LegalEntityKey != other.LegalEntityKey)
                return 0;
            if (this.LegalEntityLegalName != other.LegalEntityLegalName)
                return 0;
            if (this.GeneralStatusKey != other.GeneralStatusKey)
                return 0;
            if (this.RoleTypeKey != other.RoleTypeKey)
                return 0;
            return 1;
        }
    }
}