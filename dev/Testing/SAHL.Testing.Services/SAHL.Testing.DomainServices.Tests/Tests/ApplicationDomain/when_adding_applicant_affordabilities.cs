using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ApplicationDomain
{
    public class when_adding_applicant_affordabilities : ServiceTestBase<IApplicationDomainServiceClient>
    {
        private GetNewBusinessApplicantQueryResult offerRoleData;

        [TearDown]
        new public void OnTestTearDown()
        {
            if (offerRoleData != null)
            {
                var command = new RemoveApplicantFromActiveNewBusinessApplicantsCommand(offerRoleData.OfferRoleKey);
                base.PerformCommand(command);
                offerRoleData = null;
            }
            base.OnTestTearDown();
        }

        [Test]
        public void when_successful()
        {
            var query = new GetNewBusinessApplicantQuery(isIncomeContributor: true, hasDeclarations: true, hasAffordabilityAssessment: false,
                hasAssetsLiabilities: false, hasBankAccount: false, hasEmployment: true, hasDomicilium: false, hasResidentialAddress: true, hasPostalAdddress: false);
            base.PerformQuery(query);
            offerRoleData = query.Result.Results.FirstOrDefault();
            var clientAffordabilityAssessment = new List<AffordabilityTypeModel> {
                new AffordabilityTypeModel(AffordabilityType.BasicSalary, 65000, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.CommissionandOvertime, 25000, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.Rental, 2000, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.IncomefromInvestments, 2000, "Description"),
                new AffordabilityTypeModel(AffordabilityType.OtherIncome1, 2000, "Description"),
                new AffordabilityTypeModel(AffordabilityType.OtherIncome2, 2000, "Description"),
                new AffordabilityTypeModel(AffordabilityType.SalaryDeductions, 80000, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.Foodandgroceries, 500, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.BondPayments, 6500, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.AllCarRepayments, 500, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.CreditCard, 500, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.Overdraft, 500, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.RetailAccounts, 100, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.Creditlinerepayment, 500, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.PlannedSavings, 500, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.OtherInstalments, 500, "Description"),
                new AffordabilityTypeModel(AffordabilityType.Other, 500, "Description"),
                new AffordabilityTypeModel(AffordabilityType.Medicalexpenses, 500, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.Clothing, 500, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.Water_lights_refuseremoval, 500, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.Ratesandtaxes, 500, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.Transport_petrolcosts, 500, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.Insurance_funeralpolicies, 500, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.Domesticworkerwage_gardenservices, 1000, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.Telephone, 500, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.Educationfees, 500, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.Childsupport, 500, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.Rentalrepayment, 500, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.Personalloans, 500, string.Empty),
                new AffordabilityTypeModel(AffordabilityType.Otherdebtrepayment, 1000, "Description")
            };
            var model = new ApplicantAffordabilityModel(clientAffordabilityAssessment, offerRoleData.LegalEntityKey, offerRoleData.OfferKey);
            var command = new AddApplicantAffordabilitiesCommand(model);
            base.Execute<AddApplicantAffordabilitiesCommand>(command).WithoutErrors();
            var affordabilityAssessment = TestApiClient.Get<LegalEntityAffordabilityDataModel>(new { offerkey = model.ApplicationNumber, legalentitykey = model.ClientKey });
            foreach (var item in clientAffordabilityAssessment)
            {
                Assert.That(affordabilityAssessment.Where(x => x.AffordabilityTypeKey == (int)item.AffordabilityType && x.Amount == item.Amount && x.Description == item.Description).First() != null);
            }
        }

        [Test]
        public void when_unsuccessful()
        {
            var query = new GetNewBusinessApplicantQuery(isIncomeContributor: true, hasDeclarations: true, hasAffordabilityAssessment: true,
                hasAssetsLiabilities: false, hasBankAccount: false, hasEmployment: true, hasDomicilium: false, hasResidentialAddress: true, hasPostalAdddress: false);
            base.PerformQuery(query);
            offerRoleData = query.Result.Results.FirstOrDefault();
            var clientAffordabilityAssessment = new List<AffordabilityTypeModel> {
                new AffordabilityTypeModel(AffordabilityType.BasicSalary, 30000, ""),
                new AffordabilityTypeModel(AffordabilityType.IncomefromInvestments, 2000, ""),
                new AffordabilityTypeModel(AffordabilityType.BasicSalary, 2000, "")};
            var model = new ApplicantAffordabilityModel(clientAffordabilityAssessment, offerRoleData.LegalEntityKey, offerRoleData.OfferKey);
            var command = new AddApplicantAffordabilitiesCommand(model);
            base.Execute<AddApplicantAffordabilitiesCommand>(command)
                .AndExpectThatErrorMessagesContain(
                    "An Affordability description is required for the 'IncomefromInvestments' affordability type.",
                    string.Format("An affordability assessment already exists for Client: {0} on ApplicationNumber: {1}", model.ClientKey, model.ApplicationNumber),
                    "An applicant's affordability assessment can only contain one of each affordability type.");
        }
    }
}