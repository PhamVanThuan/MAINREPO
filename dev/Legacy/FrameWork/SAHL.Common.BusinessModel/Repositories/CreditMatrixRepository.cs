using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.DomainMessages;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.Globals;
using System.Collections;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Security;
using SAHL.Common.BusinessModel.SearchCriteria;
using SAHL.Common.Factories;
using System.Security.Principal;
using SAHL.Common;
using SAHL.Common.Exceptions;
using System.Data;
using SAHL.Common.DataAccess;
using System.Collections.Specialized;
using SAHL.Common.CacheData;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using System.Linq;

namespace SAHL.Common.BusinessModel.Repositories
{
	[FactoryType(typeof(ICreditMatrixRepository))]
	public class CreditMatrixRepository : AbstractRepositoryBase, ICreditMatrixRepository
	{
		private ICastleTransactionsService castleTransactionsService;
		public CreditMatrixRepository(ICastleTransactionsService castleTransactionsService)
		{
			this.castleTransactionsService = castleTransactionsService;
		}

		public CreditMatrixRepository()
		{
			this.castleTransactionsService = new CastleTransactionsService();
		}

		public ICreditMatrix GetCreditMatrixByKey(int key)
		{
			return base.GetByKey<ICreditMatrix, CreditMatrix_DAO>(key);
		}

		public ICategory GetCategoryByKey(int key)
		{
			return base.GetByKey<ICategory, Category_DAO>(key);
		}

		public IRateConfiguration GetRateConfigurationByMarginKeyAndMarketRateKey(int MarginKey, int MarketRateKey)
		{
			SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
			IRateConfiguration rc = RateConfiguration.GetByMarginKeyAndMarketRateKey(MarginKey, MarketRateKey);

			if (rc == null)
				spc.DomainMessages.Add(new Error("No RateConfiguration found matching the given keys.", ""));

			return rc;
		}

		public IMarginProduct GetMarginProductByRateConfigAndOSP(int RateConfigurationKey, int OriginationSourceKey, int ProductKey)
		{
			//  GetDiscountBy_RateConfig_OS_Product_Keys
			//select 
			//      mp.Discount
			//from [2AM]..OriginationSourceProductConfiguration OSPC with (nolock)
			//inner join [2AM]..marketrate mr with (nolock) on OSPC.MarketRateKey = mr.MarketRateKey
			//inner join [2AM]..marginproduct mp with (nolock) on OSPC.OriginationSourceProductKey = mp.OriginationSourceProductKey
			//inner join [2AM]..RateConfiguration rc with (nolock) on OSPC.MarketRateKey = rc.MarketRateKey   //            and mp.MarginKey = rc.MarginKey
			//inner join [2AM]..OriginationSourceProduct osp on OSPC.OriginationSourceProductKey = osp.OriginationSourceProductKey
			//where osp.ProductKey = 5 
			//and osp.OriginationSourceKey = 1
			//and rc.RateConfigurationKey = 1
			//RateConfiguration rc;
			//rc.Key;

			//MarginProduct_DAO mp;
			//mp.OriginationSourceProduct.OriginationSource.Key = 1;
			//mp.OriginationSourceProduct.Product.Key = 5;
			//mp.Margin.RateConfigurations;

			string HQL = "select mp from MarginProduct_DAO mp join mp.Margin.RateConfigurations rc where rc.Key = ? and mp.OriginationSourceProduct.OriginationSource.Key = ? and mp.OriginationSourceProduct.Product.Key = ?";

			SimpleQuery<MarginProduct_DAO> q = new SimpleQuery<MarginProduct_DAO>(HQL, RateConfigurationKey, OriginationSourceKey, ProductKey);
			q.SetResultTransformer(new NHibernate.Transform.DistinctRootEntityResultTransformer());
			MarginProduct_DAO[] res = q.Execute();

			if (res.Length > 0)
				return new MarginProduct(res[0]);

			return null;
		}

		public DataSet GetCreditCriteriaByOSP(int originationSourceKey, int productKey)
		{
			DataSet dsCreditCriteria = new DataSet();

			using (IDbConnection con = Helper.GetSQLDBConnection())
			{

				// get uiStatement
				string query = UIStatementRepository.GetStatement("COMMON", "CreditCriteriaByOSPGet");

				// setup parameters
				ParameterCollection parameters = new ParameterCollection();
				Helper.AddIntParameter(parameters, "@OSKey", originationSourceKey);
				Helper.AddIntParameter(parameters, "@ProductKey", productKey);

				// setup tableMapping 
				StringCollection TableMapping = new StringCollection();
				TableMapping.Add("CreditCriteria");
				TableMapping.Add("LinkedOSP");

				Helper.FillFromQuery(dsCreditCriteria, TableMapping, query, con, parameters);
			}

			return dsCreditCriteria;
		}

		public ICreditMatrix GetCreditMatrix(OriginationSources originationSource)
		{
			string sql = @"select
								cm.*
							from
									[2am].dbo.CreditMatrix cm
							join	[2am].dbo.OriginationSourceProductCreditMatrix ospcm on cm.CreditMatrixKey = ospcm.CreditMatrixKey 
							join	[2am].dbo.OriginationSourceProduct osp on osp.OriginationSourceProductKey = ospcm.OriginationSourceProductKey and OriginationSourceKey = ?";
			return castleTransactionsService.Single<ICreditMatrix>(QueryLanguages.Sql, sql, "cm", Databases.TwoAM, (int)originationSource);
		}
	}
}
