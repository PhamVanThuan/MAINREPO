using Common.Enums;
using System;
using System.Collections.Generic;

namespace Automation.DataModels
{
    public class FutureDatedChange
    {
        public int FutureDatedChangeKey { get; set; }

        public FutureDatedChangeTypeEnum FutureDatedChangeType { get; set; }

        public int IdentifierReferenceKey { get; set; }

        public DateTime EffectiveDate { get; set; }

        public bool NotificationRequired { get; set; }

        public string UserID { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime ChangeDate { get; set; }

        public List<FutureDatedChangeDetail> FutureDatedChangeDetails { get; set; }
    }
}