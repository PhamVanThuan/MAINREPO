using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAHL.Web.Views.FurtherLending.DTO
{
    public class RecipientCorrespondenceSelection
    {
        private int legalEntityKey;
        private int correspondenceMediumKey;
        private string contactInfo;
        private bool isUsingAlternateContactInfo;

        public RecipientCorrespondenceSelection(int legalEntityKey, int correspondenceMediumKey, string contactInfo, bool isUsingAlternateContactInfo)
        {
            this.legalEntityKey = legalEntityKey;
            this.correspondenceMediumKey = correspondenceMediumKey;
            this.contactInfo = contactInfo;
            this.isUsingAlternateContactInfo = isUsingAlternateContactInfo;
        }

        public int LegalEntityKey{get { return legalEntityKey; }}

        public int CorrespondenceMediumKey { get { return correspondenceMediumKey; } }

        public string ContactInfo { get { return contactInfo; } }

        public bool IsUsingAlternateContactInfo { get { return isUsingAlternateContactInfo; } }
    }
}