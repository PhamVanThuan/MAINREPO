using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.ApplicationDomain.Managers.AffordabilityAssessment;
using System;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.AffordabilityAssessment
{
    public class when_getting_unconfirmed_application_affordabilityassessments : WithFakes
    {
        private static IAffordabilityAssessmentManager affordabilityAssessmentManager;
        private static IAffordabilityAssessmentDataManager affordabilityAssessmentDataManager;
        private static IEnumerable<AffordabilityAssessmentDataModel> affordabilityAssessmentDataModels;
        private static List<AffordabilityAssessmentItemDataModel> items;
        private static int applicationKey, affordabilityAssessmentKey;

        private Establish context = () =>
        {
            applicationKey = 999;
            affordabilityAssessmentKey = 111;

            affordabilityAssessmentDataManager = An<IAffordabilityAssessmentDataManager>();

            affordabilityAssessmentDataModels = new List<AffordabilityAssessmentDataModel>()
            {
                new AffordabilityAssessmentDataModel(affordabilityAssessmentKey,
                                                    1,
                                                    (int)GenericKeyType.Offer,
                                                    (int)AffordabilityAssessmentStatus.Unconfirmed,
                                                    (int)GeneralStatus.Active,
                                                    1,
                                                    DateTime.Now,
                                                    1,
                                                    1,
                                                    1,
                                                    1500,
                                                    null, null)
            };
            affordabilityAssessmentDataManager.WhenToldTo(x => x.GetActiveAffordabilityAssessmentsForApplication(applicationKey)).Return(affordabilityAssessmentDataModels);

            AffordabilityAssessmentItemDataModel basicGrossSalary_Drawings = new AffordabilityAssessmentItemDataModel(
                                                                                                1,
                                                                                                affordabilityAssessmentKey,
                                                                                                (int)AffordabilityAssessmentItemType.BasicGrossSalary_Drawings,
                                                                                                DateTime.Now,
                                                                                                1,
                                                                                                1111,
                                                                                                1111,
                                                                                                null,
                                                                                                null);

            AffordabilityAssessmentItemDataModel transport = new AffordabilityAssessmentItemDataModel(
                                                                                                2,
                                                                                                affordabilityAssessmentKey,
                                                                                                (int)AffordabilityAssessmentItemType.Transport,
                                                                                                DateTime.Now,
                                                                                                1,
                                                                                                2222,
                                                                                                2222,
                                                                                                null,
                                                                                                null);

            AffordabilityAssessmentItemDataModel security = new AffordabilityAssessmentItemDataModel(
                                                                                                3,
                                                                                                affordabilityAssessmentKey,
                                                                                                (int)AffordabilityAssessmentItemType.Security,
                                                                                                DateTime.Now,
                                                                                                1,
                                                                                                3333,
                                                                                                3333,
                                                                                                null,
                                                                                                null);

            AffordabilityAssessmentItemDataModel vehicle = new AffordabilityAssessmentItemDataModel(
                                                                                                4,
                                                                                                affordabilityAssessmentKey,
                                                                                                (int)AffordabilityAssessmentItemType.Vehicle,
                                                                                                DateTime.Now,
                                                                                                1,
                                                                                                4444,
                                                                                                4444,
                                                                                                null,
                                                                                                null);

            AffordabilityAssessmentItemDataModel sahlBond = new AffordabilityAssessmentItemDataModel(
                                                                                                5,
                                                                                                affordabilityAssessmentKey,
                                                                                                (int)AffordabilityAssessmentItemType.SAHLBond,
                                                                                                DateTime.Now,
                                                                                                1,
                                                                                                5555,
                                                                                                5555,
                                                                                                null,
                                                                                                null);

            items = new List<AffordabilityAssessmentItemDataModel>() { basicGrossSalary_Drawings, transport, security, vehicle, sahlBond };

            affordabilityAssessmentDataManager.WhenToldTo(x => x.GetAffordabilityAssessmentItems(affordabilityAssessmentKey)).Return(items);

            affordabilityAssessmentDataManager.WhenToldTo(x => x.GetAffordabilityAssessmentContributors(affordabilityAssessmentKey)).Return(new List<int>() { 1, 2 });

            affordabilityAssessmentDataManager.WhenToldTo(x => x.GetAffordabilityAssessmentStressFactorByKey(1)).Return(new Tuple<decimal, string>(0.5M, "5%"));

            affordabilityAssessmentManager = new AffordabilityAssessmentManager(affordabilityAssessmentDataManager);
        };

        private Because of = () =>
        {
            affordabilityAssessmentManager.GetApplicationAffordabilityAssessments(applicationKey);
        };

        private It should_ask_the_affordability_assessment_data_manager_to_get_the_applications_unconfirmed_affordability_assessments = () =>
        {
            affordabilityAssessmentDataManager.WasToldTo(x => x.GetActiveAffordabilityAssessmentsForApplication(applicationKey));
        };
    }
}