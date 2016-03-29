using BuildingBlocks.Services.Contracts;
using Common.Enums;
using NUnit.Framework;
using System;
using WatiN.Core.Logging;

namespace BuildingBlocks.Assertions
{
    /// <summary>
    /// contains assertions specific to Debt Counselling
    /// </summary>
    public static class ReasonAssertions
    {
        private static IReasonService reasonService;

        static ReasonAssertions()
        {
            reasonService = ServiceLocator.Instance.GetService<IReasonService>();
        }

        /// <summary>
        /// Ensures that a specific combination of Reason Description and Reason Type exists against a given GenericKey
        /// </summary>
        /// <param name="reasonDescription">ReasonDescription.Description</param>
        /// <param name="reasonTypeDescription">ReasonType.Description</param>
        /// <param name="genericKey">Reason.GenericKey</param>
        /// <param name="genericKeyType">ReasonDefinition.GenericKeyTypeKey</param>
        public static void AssertReason(string reasonDescription, string reasonTypeDescription, int genericKey, GenericKeyTypeEnum genericKeyType, bool reasonExists = true, string expectedComment = "")
        {
            bool exists = reasonService.ReasonExistsAgainstGenericKey(reasonDescription, reasonTypeDescription, genericKey, genericKeyType);
            //check if we are trying to find if a reason should or should not exist.
            if (reasonExists)
            {
                Logger.LogAction(@"Asserting that Reason Description: '{0}' exists against: GenericKeyTypeKey={1}, GenericKey={2}", reasonDescription, (int)genericKeyType, genericKey);
                Assert.True(exists, string.Format(@"A Reason with description {0} was not found against the given Generic Key: {1}", reasonDescription, genericKey));
                //Assert reason comment
                if (!String.IsNullOrEmpty(expectedComment))
                {
                    var reason = reasonService.GetReasonsByGenericKeyAndGenericKeyType(genericKey, genericKeyType);
                    var actualComment = reason.Rows(0).Column("Comment").Value;
                    Assert.AreEqual(expectedComment, actualComment);
                }
            }
            else
            {
                Logger.LogAction(@"Asserting that Reason Description: '{0}' does not exist against: GenericKeyTypeKey={1}, GenericKey={2}", reasonDescription, (int)genericKeyType, genericKey);
                Assert.IsFalse(exists, string.Format(@"A Reason with description {0} was found against the given Generic Key: {1}", reasonDescription, genericKey));
            }
        }
    }
}