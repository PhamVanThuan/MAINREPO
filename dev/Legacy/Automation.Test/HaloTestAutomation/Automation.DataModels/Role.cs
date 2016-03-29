using System;

namespace Automation.DataModels
{
    public sealed class Role: Record
    {
        public int LegalEntityKey { get; set; }

        public int AccountKey { get; set; }

        public int RoleTypeKey { get; set; }

        public int AccountRoleKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public DateTime StatusChangeDate { get; set; }

        public LegalEntity LegalEntity { get; set; }
    }
}