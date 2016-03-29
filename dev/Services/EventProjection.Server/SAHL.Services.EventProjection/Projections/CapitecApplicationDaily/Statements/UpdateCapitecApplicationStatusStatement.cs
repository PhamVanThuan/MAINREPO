using SAHL.Core.Data;
using System;

namespace SAHL.Services.EventProjection.Projections.CapitecApplicationDaily.Statements
{
    public class UpdateCapitecApplicationStatusStatement : ISqlStatement<SAHL.Core.Data.Models.Capitec.ApplicationDataModel>
    {
        public int OfferKey { get; protected set; }

        public Guid SAHLOfferStatusKey { get; protected set; }

        public DateTime CompositeDate { get; protected set; }

        public UpdateCapitecApplicationStatusStatement(int offerKey, Guid sahlOfferStatusKey, DateTime compositeDate)
        {
            this.OfferKey = offerKey;
            this.SAHLOfferStatusKey = sahlOfferStatusKey;
            this.CompositeDate = compositeDate;
        }

        public string GetStatement()
        {
            return @"
SET XACT_ABORT ON

UPDATE [SAHL-CDB01].[Capitec].[dbo].[Application]
SET
    ApplicationStatusEnumId = @SAHLOfferStatusKey,
    LastStatusChangeDate = @CompositeDate
WHERE ApplicationNumber = @OfferKey

SET XACT_ABORT OFF
";
        }
    }
}