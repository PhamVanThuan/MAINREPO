using Automation.DataAccess;
using BuildingBlocks.Services;
using BuildingBlocks.Services.Contracts;
using Common.Extensions;
using NUnit.Framework;
using WatiN.Core.Logging;

namespace BuildingBlocks.Assertions
{
    public static class eWorkAssertions
    {
        private static IEWorkService eworkService;

        static eWorkAssertions()
        {
            eworkService = new EWorkService();
        }

        /// <summary>
        /// This assertion will check that an eWork eFolder exists at a given stage in a given map.
        /// </summary>
        /// <param name="eFolderName">eFolderName</param>
        /// <param name="eStageName">eStageName</param>
        /// <param name="eMapName">eMapName</param>
        public static void AssertEworkCaseExists(string eFolderName, string eStageName, string eMapName)
        {
            Logger.LogAction(string.Format(@"Asserting that an eWork case exists at {0} in the {1} map for {2}", eStageName, eMapName, eFolderName));
            QueryResults r = eworkService.GetEFolderID(eFolderName, eStageName, eMapName);
            Assert.IsTrue(r.HasResults, string.Format(@"Assertion failed, no eWork case found at the {0} stage in the {1} map for eFolder {2}", eStageName, eMapName, eFolderName));
        }

        /// <summary>
        /// This assertion will check that an eWork eFolder exists at a given stage in a given map.
        /// </summary>
        /// <param name="eFolderName">eFolderName</param>
        /// <param name="eStageName">eStageName</param>
        public static void AssertEworkCaseExists(string eFolderName, string eStageName)
        {
            Logger.LogAction(string.Format(@"Asserting that an eWork case exists at {0} stage for {1}", eStageName, eFolderName));
            QueryResults r = eworkService.GetEFolderID(eFolderName, eStageName);
            Assert.IsTrue(r.HasResults, string.Format(@"Assertion failed, no eWork case found at the {0} stage for eFolder {1}", eStageName, eFolderName));
        }

        /// <summary>
        /// This assertion will check that an eWork eFolder does not exists at a given stage in a given map.
        /// </summary>
        /// <param name="eFolderName">eFolderName</param>
        /// <param name="eStageName">eStageName</param>
        /// <param name="eMapName">eMapName</param>
        public static void AssertNotEworkCaseExists(string eFolderName, string eStageName, string eMapName)
        {
            Logger.LogAction(string.Format(@"Asserting that an eWork case exists at {0} in the {1} map for {2}", eStageName, eMapName, eFolderName));
            QueryResults r = eworkService.GetEFolderID(eFolderName, eStageName, eMapName);
            Assert.IsFalse(r.HasResults, string.Format(@"eWork case found at the {0} stage in the {1} map for eFolder {2}", eStageName, eMapName, eFolderName));
        }

        /// <summary>
        /// This assertion will check that the eWork Loss Control table's UserToDo column has been updated correctly.
        /// </summary>
        /// <param name="eFolderName">eFolderName</param>
        /// <param name="expectedUserToDo">The expected value of the UserToDo column</param>
        public static void AssertLossControlUserToDo(string eFolderName, string eMapName, string eStageName, string expectedUserToDo)
        {
            //remove the sahl\ prefix from the expectedUserName if it exists as the ework usertodo does not contain this prefix
            expectedUserToDo = expectedUserToDo.ToLower().RemoveDomainPrefix();
            Logger.LogAction(string.Format(@"Asserting that the UserToDo in the [e-work]..LossControl table for {0} is set to {1}",
                eFolderName, expectedUserToDo));
            QueryResults r = eworkService.GetEWorkEFolderAssignmentData(eFolderName, eMapName, eStageName);
            StringAssert.AreEqualIgnoringCase(expectedUserToDo, r.Rows(0).Column("UserToDo").Value);
            r.Dispose();
        }
    }
}