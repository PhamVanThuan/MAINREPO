using SAHL.Core.Data;
using System;

namespace SAHL.Services.EventProjection.Projections.CapitecApplicationDaily.Statements
{
    public class UpdateCapitecApplicationStatement : ISqlStatement<SAHL.Core.Data.Models.Capitec.ApplicationDataModel>
    {
        public int OfferKey { get; protected set; }

        public Guid SAHLOfferStatusKey { get; protected set; }

        public Guid SAHLOfferStageKey { get; protected set; }

        public string ConsultantName { get; protected set; }

        public string ConsultantContactNumber { get; protected set; }

        public DateTime CompositeDate { get; protected set; }

        public UpdateCapitecApplicationStatement(int offerKey, Guid sahlOfferStatusKey, Guid sahlOfferStageKey, string consultantName, string consultantContactNumber, DateTime compositeDate)
        {
            this.OfferKey = offerKey;
            this.SAHLOfferStatusKey = sahlOfferStatusKey;
            this.SAHLOfferStageKey = sahlOfferStageKey;
            this.ConsultantName = consultantName;
            this.ConsultantContactNumber = consultantContactNumber;
            this.CompositeDate = compositeDate;
        }

        public string GetStatement()
        {
            return @"
SET XACT_ABORT ON

UPDATE [SAHL-CDB01].[Capitec].[dbo].[Application]
SET ApplicationStageTypeEnumId = @SAHLOfferStageKey,
    ApplicationStatusEnumId = @SAHLOfferStatusKey,
    LastStatusChangeDate = @CompositeDate,
    ConsultantName = @ConsultantName,
    ConsultantContactNumber = @ConsultantContactNumber
WHERE ApplicationNumber = @OfferKey;

SET XACT_ABORT OFF
";
        }
    }
}