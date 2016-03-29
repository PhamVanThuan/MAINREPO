using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class ExternalRole : IComparable<ExternalRole>
    {
        public int LegalEntityKey { get; set; }

        public string LegalEntityLegalName { get; set; }

        public int GenericKey { get; set; }

        public int ExternalRoleKey { get; set; }

        public ExternalRoleTypeEnum ExternalRoleTypeKey { get; set; }

        public GenericKeyTypeEnum GenericKeyTypeKey { get; set; }

        public GeneralStatusEnum GeneralStatusKey { get; set; }

        public string IDNumber { get; set; }

        public int CompareTo(ExternalRole other)
        {
            if (this.LegalEntityKey != other.LegalEntityKey)
                return 0;
            if (this.LegalEntityLegalName != other.LegalEntityLegalName)
                return 0;
            if (this.GenericKey != other.GenericKey)
                return 0;
            if (this.ExternalRoleKey != other.ExternalRoleKey)
                return 0;
            if (this.ExternalRoleTypeKey != other.ExternalRoleTypeKey)
                return 0;
            if (this.GenericKeyTypeKey != other.GenericKeyTypeKey)
                return 0;
            if (this.GeneralStatusKey != other.GeneralStatusKey)
                return 0;
            return 1;
        }
    }
}