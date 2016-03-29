using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Automation.DataModels;

namespace BuildingBlocks.Services.Contracts
{
    public interface IAuditService
    {
        IEnumerable<AuditLegalEntity> GetLegalEntityAudits(int legalEntityKey);
        void EnableLegalEntityUpdateTrigger();
        void EnableLegalEntityInsertTrigger();
    }
}
