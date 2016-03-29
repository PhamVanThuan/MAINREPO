using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class SubsidyProvider : IComparable<SubsidyProvider>
    {
        public string ContactPerson { get; set; }

        public SubsidyProviderTypeEnum SubsidyProviderTypeKey { get; set; }

        public string SubsidyProviderTypeDescription { get; set; }

        public int CompareTo(SubsidyProvider other)
        {
            if (this.ContactPerson != other.ContactPerson)
                return 0;
            if (this.SubsidyProviderTypeKey != other.SubsidyProviderTypeKey)
                return 0;
            if (this.SubsidyProviderTypeDescription != other.SubsidyProviderTypeDescription)
                return 0;
            return 1;
        }

        public Automation.DataModels.LegalEntity LegalEntity { get; set; }
    }
}