﻿using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.AssetsLiabilitiesModelManagerSpecs
{
    public class when_populating_a_student_loan_liability_loan : WithCoreFakes
    {
        private static AssetsLiabilitiesModelManager modelManager;
        private static List<LiabilityItem> comcorpLiabilityItems;
        private static ApplicantAssetLiabilityModel result;
        private static LiabilityItem studentLoan;
        private static IValidationUtils validationUtils;
        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            comcorpLiabilityItems = IntegrationServiceTestHelper.PopulateLiabilityItems();
            studentLoan = comcorpLiabilityItems.Where(x => x.SAHLLiabilityDesc == "Liability Loan" && x.LiabilityLoanType == "Student Loan").First();
            modelManager = new AssetsLiabilitiesModelManager(validationUtils);
        };

        private Because of = () =>
        {
            result = modelManager.PopulateLiabilities(comcorpLiabilityItems)
                .Where(x => x.AssetLiabilityType == AssetLiabilityType.LiabilityLoan && x.AssetLiabilitySubType == AssetLiabilitySubType.StudentLoan)
                .FirstOrDefault();
        };

        private It should_map_the_comcorp_item_as_a_liability_loan = () =>
        {
            result.AssetLiabilityType.ShouldEqual(AssetLiabilityType.LiabilityLoan);
        };

        private It should_set_the_sub_type_to_personal_loan = () =>
        {
            result.AssetLiabilitySubType.ShouldEqual(AssetLiabilitySubType.StudentLoan);
        };

        It should_set_the_liability_value_to_the_SAHLLiabilityValue_field = () =>
        {
            result.LiabilityValue.ShouldEqual(Convert.ToDouble(studentLoan.SAHLLiabilityValue));
        };

        It should_set_the_date_repayable_to_the_LiabilityDateRepayable_field = () =>
        {
            result.Date.ShouldEqual(studentLoan.LiabilityDateRepayable);
        };

        It should_set_the_cost_of_the_loan_to_the_SAHLLiabilityCost_field = () =>
        {
            result.Cost.ShouldEqual(Convert.ToDouble(studentLoan.SAHLLiabilityCost));
        };

        It should_set_the_company_name_field = () =>
        {
            result.CompanyName.ShouldEqual(studentLoan.LiabilityCompanyName);
        };
    }
}
