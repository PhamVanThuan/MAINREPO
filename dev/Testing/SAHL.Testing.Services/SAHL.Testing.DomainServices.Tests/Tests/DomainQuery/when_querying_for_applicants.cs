using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Testing.Services.Tests.DomainQuery
{
    [TestFixture]
    public class when_querying_for_applicants : ServiceTestBase<IDomainQueryServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var query = new GetRandomOpenApplicationQuery();
            base.PerformQuery(query);
            int applicationNumber = query.Result.Results.FirstOrDefault().ApplicationNumber;
            var applicationRoles = TestApiClient.Get<OfferRoleDataModel>(new { offerkey = applicationNumber, generalstatuskey = (int)GeneralStatus.Active })
                .Where(x => x.OfferRoleTypeKey == (int)OfferRoleType.MainApplicant || x.OfferRoleTypeKey == (int)OfferRoleType.Lead_MainApplicant)
                .First();
            var legalEntity = TestApiClient.GetByKey<LegalEntityDataModel>(applicationRoles.LegalEntityKey);
            var getApplicantDetailsForOfferQuery = new GetApplicantDetailsForOfferQuery(applicationNumber);
            base.Execute<GetApplicantDetailsForOfferQuery>(getApplicantDetailsForOfferQuery);
            var applicantExists = getApplicantDetailsForOfferQuery.Result.Results.Where(x => x.IdentityNumber == legalEntity.IDNumber).First();
            Assert.That(applicantExists != null);
        }

        [Test]
        public void when_unsuccessful()
        {
            var query = new GetApplicantDetailsForOfferQuery(Int32.MaxValue);
            base.Execute<GetApplicantDetailsForOfferQuery>(query);
            Assert.That(query.Result.Results.Count().Equals(0));
        }
    }
}