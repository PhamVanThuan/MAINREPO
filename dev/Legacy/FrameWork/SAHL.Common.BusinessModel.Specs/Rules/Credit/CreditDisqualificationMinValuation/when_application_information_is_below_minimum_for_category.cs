using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel.Specs.Rules.Credit.CreditDisqualificationMinValuation
{
	[Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMinValuation))]
	public class when_application_information_is_below_minimum_for_category : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMinValuation>
	{
		protected static ICreditCriteriaRepository creditCriteriaRepository;
		protected static IApplicationRepository applicationRepository;

		protected static ICreditMatrix creditMatrix;
		protected static double? minimumValuationForCategory;

		protected static ISupportsVariableLoanApplicationInformation variableLoanApplicationInformation;
		protected static IApplicationInformationVariableLoan applicationInformationVariableLoan;
		protected static IApplication application;
		protected static IApplicationProductVariableLoan applicationProduct;
		protected static IApplicationType applicationType;

		protected static ICategory applicationCategory;

		Establish context = () =>
		{
			minimumValuationForCategory = 1000;
			creditCriteriaRepository = An<ICreditCriteriaRepository>();
			applicationRepository = An<IApplicationRepository>();

			applicationCategory = An<ICategory>();
			applicationCategory.WhenToldTo(x => x.Key).Return(0);

			application = An<IApplication>();
			applicationType = An<IApplicationType>();
			applicationProduct = An<IApplicationProductVariableLoan>();
			applicationInformationVariableLoan = An<IApplicationInformationVariableLoan>();
			applicationInformationVariableLoan.WhenToldTo(x => x.Category).Return(applicationCategory);


			var creditCriteria = An<ICreditCriteria>();
			creditCriteria.WhenToldTo(x => x.MinPropertyValue).Return(minimumValuationForCategory);
			creditCriteria.WhenToldTo(x => x.Category).Return(applicationCategory);
			creditCriteria.WhenToldTo(x => x.Key).Return(0);
			applicationInformationVariableLoan.WhenToldTo(x => x.CreditCriteria).Return(creditCriteria);

			creditMatrix = An<ICreditMatrix>();
			creditMatrix.WhenToldTo(x => x.CreditCriterias).Return(new EventList<ICreditCriteria>(new ICreditCriteria[] { creditCriteria }));

			creditCriteriaRepository = An<ICreditCriteriaRepository>();
			creditCriteriaRepository.WhenToldTo(x => x.GetCreditCriteriaByKey(Param.IsAny<int>())).Return(creditCriteria);

			applicationType.WhenToldTo(x => x.Key).Return((int)SAHL.Common.Globals.OfferTypes.NewPurchaseLoan);
			application.WhenToldTo(x => x.ApplicationType).Return(applicationType);
			application.WhenToldTo(x => x.CurrentProduct).Return(applicationProduct);
			applicationProduct.WhenToldTo(x => x.Application).Return(application);
			applicationProduct.WhenToldTo(x => x.VariableLoanInformation).Return(applicationInformationVariableLoan);
			applicationInformationVariableLoan.WhenToldTo(x => x.PropertyValuation).Return(minimumValuationForCategory - 1);

			businessRule = new SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMinValuation(creditCriteriaRepository, applicationRepository);
			RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.Credit.CreditDisqualificationMinValuation>.startrule.Invoke();
		};

		Because of = () =>
		{
			businessRule.ExecuteRule(messages, application);
		};

		It rule_should_fail = () =>
		{
			messages.Count.ShouldEqual(1);
		};
	}
}
