using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.Affordability;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.Affordability
{
    public class when_getting_an_affordability_assessment : WithFakes
    {
        private static FakeDbFactory fakeDbFactory;
        private static AffordabilityDataManager affordabilityDataManager;
        private static IEnumerable<LegalEntityAffordabilityDataModel> results;
        private static int clientKey;
        private static int applicationNumber;
        private static IEnumerable<LegalEntityAffordabilityDataModel> existingAssessment;
        private static string whereClause;

        private Establish context = () =>
        {
            clientKey = 1;
            applicationNumber = 2;
            existingAssessment = new LegalEntityAffordabilityDataModel[] { new LegalEntityAffordabilityDataModel(1, 1, 5000, string.Empty, 1408282) };
            fakeDbFactory = new FakeDbFactory();
            affordabilityDataManager = new AffordabilityDataManager(fakeDbFactory);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectWhere<LegalEntityAffordabilityDataModel>(Param.IsAny<string>(), null))
                .Return(existingAssessment);
            whereClause = string.Format("LegalEntityKey = {0} AND OfferKey = {1}", clientKey, applicationNumber);
        };

        private Because of = () =>
        {
            results = affordabilityDataManager.GetAffordabilityAssessment(clientKey, applicationNumber);
        };

        private It should_return_the_collection_from_the_database = () =>
        {
            results.ShouldEqual(existingAssessment);
        };

        private It should_use_the_clientKey_and_applicationNumber_in_the_where_clause = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectWhere<LegalEntityAffordabilityDataModel>(whereClause, null));
        };

    }
}