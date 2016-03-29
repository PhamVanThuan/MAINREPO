using Automation.DataAccess.DataHelper;
using Automation.DataModels;
using BuildingBlocks.Services.Contracts;
using System.Collections.Generic;
namespace BuildingBlocks.Services
{
    public sealed class AuditService:_2AMDataHelper, IAuditService
    {
        public void EnableLegalEntityUpdateTrigger()
        {
            base.EnableTrigger("[tu_LegalEntity]");
        }
        public void EnableLegalEntityInsertTrigger()
        {
            base.EnableTrigger("[ti_LegalEntity]");
        }
    }
}
