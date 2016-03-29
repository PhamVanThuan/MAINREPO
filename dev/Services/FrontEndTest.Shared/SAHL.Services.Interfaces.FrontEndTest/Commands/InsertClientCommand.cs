using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System;
using System.ComponentModel.DataAnnotations;


namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class InsertClientCommand : ServiceCommand, IFrontEndTestCommand
    {
        [Required]
        public LegalEntityDataModel LegalEntity { get; protected set; }
        [Required]
        public int OfferKey { get; protected set; }
        [Required]
        public Guid ClientGuid { get; protected set; }
        public InsertClientCommand(LegalEntityDataModel legalEntity, int offerKey, Guid clientGuid)
        {
            this.LegalEntity = legalEntity;
            this.OfferKey = offerKey;
            this.ClientGuid = clientGuid;
        }
    }
}
