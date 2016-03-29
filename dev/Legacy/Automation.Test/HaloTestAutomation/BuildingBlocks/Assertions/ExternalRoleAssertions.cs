using BuildingBlocks.Services;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using NUnit.Framework;
using WatiN.Core.Logging;

namespace BuildingBlocks.Assertions
{
    /// <summary>
    /// contains assertions for the ExternalRole
    /// </summary>
    public static class ExternalRoleAssertions
    {
        private static IExternalRoleService externalRoleService;

        static ExternalRoleAssertions()
        {
            externalRoleService = new ExternalRoleService();
        }

        /// <summary>
        /// Ensures only one active external role exists for a legal entity
        /// </summary>
        /// <param name="genericKey">GenericKey</param>
        /// <param name="genericKeyType">GenericKeyTypeKey the record is stored against in the ExternalRole table</param>
        /// <param name="externalRoleType">External Role Type</param>
        /// <param name="expectedLegalentitykey">Legal Entity expected to hold the role.</param>
        public static void AssertActiveExternalRoleExistsForLegalEntity(int genericKey, GenericKeyTypeEnum genericKeyType, ExternalRoleTypeEnum externalRoleType, int expectedLegalentitykey)
        {
            var activeExternalRoleCount
                = externalRoleService.GetActiveExternalRoleCount(genericKeyType, externalRoleType, expectedLegalentitykey, genericKey);

            //if there are more than one active external role then fail
            if (activeExternalRoleCount > 1)
            {
                Logger.LogAction(string.Format("Asserting that only one active WorkflowRole exists of type {0} on {1}",
                    externalRoleType, genericKey));
                Assert.Fail("More then one active WorkflowRoleExists");
            }
            else if (activeExternalRoleCount == 1)
            {
                var externalRole = externalRoleService.GetActiveExternalRoleByLegalEntity(genericKey, genericKeyType, externalRoleType, expectedLegalentitykey, true);
                Logger.LogAction(string.Format("Asserting that an active workflow role of type {0} exists for {1} against LegalEntity {2}", externalRoleType.ToString(),
                    genericKey, expectedLegalentitykey));
                Assert.AreEqual(expectedLegalentitykey, externalRole.LegalEntityKey, "Active External Role is not against the expected legal entity");
            }
        }

        /// <summary>
        /// Ensures that an active external role does not exist for given legalentity
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyType"></param>
        /// <param name="externalRoleType"></param>
        /// <param name="legalEntityKey"></param>
        public static void AssertActiveExternalRoleDoesNotExistForLegalEntity(int genericKey, GenericKeyTypeEnum genericKeyType, ExternalRoleTypeEnum externalRoleType,
            int expectedLegalentitykey)
        {
            var activeExternalRoleCount
              = externalRoleService.GetActiveExternalRoleCount(genericKeyType, externalRoleType, expectedLegalentitykey, genericKey);
            if (activeExternalRoleCount > 0)
                Assert.Fail("External Role was found when none was expected");
        }
    }
}