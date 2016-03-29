using Automation.DataModels;
using System;
using System.Collections.Generic;
namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        public IEnumerable<AuditLegalEntity> GetLegalEntityAudits(int legalEntityKey)
        {
            var query = String.Format(@"select * from [2am].dbo.[AuditLegalEntity] 
                                        where legalentitykey={0} 
                                        order by auditdate desc", legalEntityKey);
            return dataContext.Query<AuditLegalEntity>(query);
        }
        public void EnableTrigger(string triggerName)
        {
            triggerName = triggerName.Replace("[", "").Replace("]", "");
            var query = string.Format("ENABLE Trigger [{0}] ON [2am].[dbo].[LegalEntity]", triggerName);
            dataContext.Execute(query);
        }
    }
}
