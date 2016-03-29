using Common.Enums;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Automation.DataModels
{
    public sealed class OfferRole
    {
        public int OfferRoleKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int OfferKey { get; set; }

        public OfferRoleTypeEnum OfferRoleTypeKey { get; set; }

        public GeneralStatusEnum GeneralStatusKey { get; set; }

        public DateTime StatusChangeDate { get; set; }

        public OfferRoleTypeGroupEnum OfferRoleTypeGroupKey { get; set; }

        public OfferRoleAttribute OfferRoleAttribute { get; set; }
    }
}