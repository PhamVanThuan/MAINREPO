using Automation.DataModels;
using System.Linq;
using BuildingBlocks.Services.Contracts;
using NUnit.Framework;
using BuildingBlocks.Timers;
namespace BuildingBlocks.Assertions
{
    public class AuditAssertions
    {
        private static IAuditService auditsService = ServiceLocator.Instance.GetService<IAuditService>();
        private static ILegalEntityService legalEntityService = ServiceLocator.Instance.GetService<ILegalEntityService>();
        public static void AssertLegalEntity(int legalEntityKey, AuditLegalEntity expectedAuditLegalEntity)
        {
            var actualAuditLegalEntity = auditsService.GetLegalEntityAudits(legalEntityKey).FirstOrDefault();
            //Auditing takes a while
            GeneralTimer.BlockWaitFor(1000, 10, () =>
            {
                actualAuditLegalEntity = auditsService.GetLegalEntityAudits(legalEntityKey).FirstOrDefault();
                return actualAuditLegalEntity != null;
            });
            Assert.NotNull(actualAuditLegalEntity, "No audit data available for legal entity {0}", legalEntityKey);
            Assert.True(expectedAuditLegalEntity.CompareTo(actualAuditLegalEntity) == 1,"Auditing for legal entity is incorrect: LegalEntityKey:{0}",legalEntityKey);
        }
    }
}
