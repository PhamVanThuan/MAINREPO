using Common.Enums;

namespace Automation.DataModels
{
    public sealed class AttorneyContacts
    {
        public Automation.DataModels.LegalEntity LegalEntity { get; set; }

        public ExternalRoleTypeEnum ExternalRoleType { get; set; }
    }
}