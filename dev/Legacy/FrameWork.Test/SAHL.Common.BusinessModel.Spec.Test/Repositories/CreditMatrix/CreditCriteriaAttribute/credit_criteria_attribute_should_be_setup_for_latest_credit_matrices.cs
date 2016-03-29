using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord.Queries;
using Machine.Specifications;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Spec.Test.Repositories.CreditMatrix.CreditCriteriaAttribute
{
	public class credit_criteria_attribute_should_be_setup_for_latest_credit_matrices : TestBase
	{
		public class new_business_credit_criteria_attribute_should_exist_for_all_categories
		{
			protected static IList<ICategory> categoriesForNewBusiness;
			protected static string query = String.Empty;
			protected static IList<int> allowedCategories;
			Establish context = () =>
			{
				query = String.Format(@"declare @lastKnownCreditMatrixKey int
										select @lastKnownCreditMatrixKey = CreditMatrixKey from [2am].dbo.CreditMatrix where NewBusinessIndicator = 'Y'
										order by CreditMatrixKey desc

										select distinct category.* from 
										[2am].dbo.CreditMatrix cm
										join [2am].dbo.CreditCriteria cc on cc.CreditMatrixKey = cm.CreditMatrixKey
										join [2am].dbo.Category category on cc.CategoryKey = category.CategoryKey
										join [2am].dbo.creditCriteriaAttribute cca on cca.CreditCriteriaKey = cc.CreditCriteriaKey
										join [2am].dbo.CreditCriteriaAttributeType ccat on ccat.CreditCriteriaAttributeTypeKey = cca.CreditCriteriaAttributeTypeKey
										where 
										cm.CreditMatrixKey = @lastKnownCreditMatrixKey
										and ccat.CreditCriteriaAttributeTypeKey = {0}", (int)CreditCriteriaAttributeTypes.NewBusiness);
				allowedCategories = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 99 };

			};
			Because of = () =>
			{
				var transactionsService = new CastleTransactionsService();
				categoriesForNewBusiness = transactionsService.Many<ICategory>(Globals.QueryLanguages.Sql, query, "cc", Globals.Databases.TwoAM);
			};
			It should = () =>
			{
				foreach (var newBusinessCreditCriteria in categoriesForNewBusiness)
				{
					allowedCategories.Contains(newBusinessCreditCriteria.Key).ShouldBeTrue();
				}
			};
		}
		public class further_lending_non_alpha_housing_credit_criteria_attribute_should_exist_for_all_categories
		{
			protected static IList<ICategory> categoriesForNewBusiness;
			protected static string query = String.Empty;
			protected static IList<int> allowedCategories;
			Establish context = () =>
			{
				query = String.Format(@"declare @lastKnownCreditMatrixKey int
										select @lastKnownCreditMatrixKey = CreditMatrixKey from [2am].dbo.CreditMatrix where NewBusinessIndicator = 'Y'
										order by CreditMatrixKey desc

										select distinct category.* from 
										[2am].dbo.CreditMatrix cm
										join [2am].dbo.CreditCriteria cc on cc.CreditMatrixKey = cm.CreditMatrixKey
										join [2am].dbo.Category category on cc.CategoryKey = category.CategoryKey
										join [2am].dbo.creditCriteriaAttribute cca on cca.CreditCriteriaKey = cc.CreditCriteriaKey
										join [2am].dbo.CreditCriteriaAttributeType ccat on ccat.CreditCriteriaAttributeTypeKey = cca.CreditCriteriaAttributeTypeKey
										where 
										cm.CreditMatrixKey = @lastKnownCreditMatrixKey
										and ccat.CreditCriteriaAttributeTypeKey = {0}", (int)CreditCriteriaAttributeTypes.FurtherLendingNonAlphaHousing);
				allowedCategories = new List<int> { 0, 1, 2, 3, 4, 5 };

			};
			Because of = () =>
			{
				var transactionsService = new CastleTransactionsService();
				categoriesForNewBusiness = transactionsService.Many<ICategory>(Globals.QueryLanguages.Sql, query, "cc", Globals.Databases.TwoAM);
			};
			It should = () =>
			{
				foreach (var newBusinessCreditCriteria in categoriesForNewBusiness)
				{
					allowedCategories.Contains(newBusinessCreditCriteria.Key).ShouldBeTrue();
				}
			};
		}
		public class further_lending_alpha_housing_credit_criteria_attribute_should_exist_for_alpha_housing_categories_only
		{
			protected static IList<ICategory> categoriesForNewBusiness;
			protected static string query = String.Empty;
			protected static IList<int> allowedCategories;
			Establish context = () =>
			{
				query = String.Format(@"declare @lastKnownCreditMatrixKey int
										select @lastKnownCreditMatrixKey = CreditMatrixKey from [2am].dbo.CreditMatrix where NewBusinessIndicator = 'Y'
										order by CreditMatrixKey desc

										select distinct category.* from 
										[2am].dbo.CreditMatrix cm
										join [2am].dbo.CreditCriteria cc on cc.CreditMatrixKey = cm.CreditMatrixKey
										join [2am].dbo.Category category on cc.CategoryKey = category.CategoryKey
										join [2am].dbo.creditCriteriaAttribute cca on cca.CreditCriteriaKey = cc.CreditCriteriaKey
										join [2am].dbo.CreditCriteriaAttributeType ccat on ccat.CreditCriteriaAttributeTypeKey = cca.CreditCriteriaAttributeTypeKey
										where 
										cm.CreditMatrixKey = @lastKnownCreditMatrixKey
										and ccat.CreditCriteriaAttributeTypeKey = {0}", (int)CreditCriteriaAttributeTypes.FurtherLendingAlphaHousing);
				allowedCategories = new List<int> { 6, 7, 8, 9 };

			};
			Because of = () =>
			{
				var transactionsService = new CastleTransactionsService();
				categoriesForNewBusiness = transactionsService.Many<ICategory>(Globals.QueryLanguages.Sql, query, "cc", Globals.Databases.TwoAM);
			};
			It should = () =>
			{
				foreach (var newBusinessCreditCriteria in categoriesForNewBusiness)
				{
					allowedCategories.Contains(newBusinessCreditCriteria.Key).ShouldBeTrue();
				}
			};
		}
	}
}
