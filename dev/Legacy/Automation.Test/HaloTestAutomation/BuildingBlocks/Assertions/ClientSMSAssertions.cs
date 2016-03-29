using BuildingBlocks.Services.Contracts;
using NUnit.Framework;
using System.Linq;

namespace BuildingBlocks.Assertions
{
    public static class ClientSMSAssertions
    {
        public static void AssertSMSSentToClients(string expectedSmsText, int generickey)
        {
            var applicantRoles = ServiceLocator.Instance.GetService<IApplicationService>().GetActiveOfferRolesByOfferKey(generickey, Common.Enums.OfferRoleTypeGroupEnum.Client)
                                                                                            .Where(x => x.OfferRoleTypeKey == Common.Enums.OfferRoleTypeEnum.MainApplicant);
            foreach (var mainAppRole in applicantRoles)
            {
                var le = ServiceLocator.Instance.GetService<ILegalEntityService>().GetLegalEntity(legalentitykey: mainAppRole.LegalEntityKey);
                var queryResults = ServiceLocator.Instance.GetService<IClientEmailService>().GetClientEmailSMS(expectedSmsText, le.CellPhoneNumber, generickey);
                Assert.True(queryResults.HasResults, "Sms was not sent to client: {0} for offer: {1}",
                            ServiceLocator.Instance.GetService<ILegalEntityService>().GetLegalEntityLegalName(le.LegalEntityKey), generickey);
            }
        }
    }
}