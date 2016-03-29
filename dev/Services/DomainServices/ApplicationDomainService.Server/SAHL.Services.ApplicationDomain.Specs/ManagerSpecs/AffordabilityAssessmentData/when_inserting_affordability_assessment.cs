﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using System;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessmentData
{
    public class when_inserting_affordability_assessment : WithFakes
    {
        private static IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;
        private static FakeDbFactory fakedDb;

        private static AffordabilityAssessmentDataModel affordabilityAssessmentDataModel;

        private Establish context = () =>
        {
            affordabilityAssessmentDataModel = new AffordabilityAssessmentDataModel(1, 2, 1, 1, 1, DateTime.Now, 1, 1, 1, 1, null, null);

            fakedDb = new FakeDbFactory();
            affordabilityAssessmentDataManager = new AffordabilityAssessmentDataManager(fakedDb);
        };

        private Because of = () =>
        {
            affordabilityAssessmentDataManager.InsertAffordabilityAssessment(affordabilityAssessmentDataModel);
        };

        private It should_insert_the_affordability_assessment = () =>
        {
            fakedDb.FakedDb.DbContext.WasToldTo(x => x.Insert<AffordabilityAssessmentDataModel>(affordabilityAssessmentDataModel));
        };
    }
}