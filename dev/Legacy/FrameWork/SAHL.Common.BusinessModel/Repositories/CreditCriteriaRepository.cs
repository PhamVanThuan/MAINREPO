using Castle.ActiveRecord.Queries;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(ICreditCriteriaRepository))]
    public class CreditCriteriaRepository : ICreditCriteriaRepository
    {
        private ICastleTransactionsService castleTransactionService;

        public CreditCriteriaRepository()
        {
            if (castleTransactionService == null)
            {
                castleTransactionService = new CastleTransactionsService();
            }
        }

        public CreditCriteriaRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        public IReadOnlyEventList<ICreditCriteria> GetCreditCriteria(IDomainMessageCollection Messages, int OriginationSourceKey, int ProductKey, int MortgageLoanPurposeKey, int EmploymentTypeKey, double TotalLoanAmount)
        {
            string HQL = "select cc from CreditCriteria_DAO cc join cc.CreditMatrix as cm join cc.CreditMatrix.OriginationSourceProducts as osp "
            + "where cc.MortgageLoanPurpose.Key = ? AND cc.EmploymentType.Key = ? AND cc.MaxLoanAmount >= ? AND cm.NewBusinessIndicator = 'Y' AND osp.OriginationSource.Key = ? AND osp.Product.Key = ? AND cc.ExceptionCriteria = 0 "
            + "ORDER BY cc.MaxLoanAmount asc, cc.Margin.Value asc, cc.LTV asc";

            SimpleQuery<CreditCriteria_DAO> q = new SimpleQuery<CreditCriteria_DAO>(HQL, MortgageLoanPurposeKey, EmploymentTypeKey, TotalLoanAmount, OriginationSourceKey, ProductKey);
            CreditCriteria_DAO[] res = q.Execute();

            IEventList<ICreditCriteria> list = new DAOEventList<CreditCriteria_DAO, ICreditCriteria, CreditCriteria>(res);
            return new ReadOnlyEventList<ICreditCriteria>(list);
        }

        public IReadOnlyEventList<ICreditCriteria> GetCreditCriteria(IDomainMessageCollection messages, int originationSourceKey, int productKey, int mortgageLoanPurposeKey, int employmentTypeKey, double totalAmount, int creditCriteriaAttributeTypeKey)
        {
            var creditCriterias = castleTransactionService.Many<ICreditCriteria>(SAHL.Common.Globals.QueryLanguages.Sql, String.Format(@"select
										cc.*
									from
											[2am].dbo.CreditCriteria cc
									join	[2am].dbo.CreditMatrix cm on cm.CreditMatrixKey = cc.CreditMatrixKey
									join	[2am].dbo.OriginationSourceProductCreditMatrix ospcm on ospcm.CreditMatrixKey = cm.CreditMatrixKey
									join	[2am].dbo.OriginationSourceProduct osp on ospcm.OriginationSourceProductKey = osp.OriginationSourceProductKey
									join	[2am].dbo.CreditCriteriaAttribute cca on cc.CreditCriteriaKey = cca.CreditCriteriaKey
									join	[2am].dbo.Margin m on cc.MarginKey = m.MarginKey
									where   cca.CreditCriteriaAttributeTypeKey = {0}
									and cc.MortgageLoanPurposeKey = {1}
									and cc.EmploymentTypeKey = {2}
									and cc.MaxLoanAmount >= {3}
									and cm.NewBusinessIndicator = 'Y'
									and osp.OriginationSourceKey = {4}
									and osp.ProductKey = {5}
									and cc.ExceptionCriteria = 0
									order by cc.MaxLoanAmount asc, m.Value asc, cc.LTV asc", creditCriteriaAttributeTypeKey, mortgageLoanPurposeKey, employmentTypeKey, totalAmount, originationSourceKey, productKey), "cc", SAHL.Common.Globals.Databases.TwoAM);

            return new ReadOnlyEventList<ICreditCriteria>(creditCriterias);
        }

        public double GetMaxPTI(IDomainMessageCollection Messages, int OriginationSourceKey, int ProductKey, int MortgageLoanPurposeKey, int EmploymentTypeKey, double TotalLoanAmount)
        {
            string HQL = "select cc from CreditCriteria_DAO cc join cc.CreditMatrix as cm join cc.CreditMatrix.OriginationSourceProducts as osp "
            + "where cc.MortgageLoanPurpose.Key = ? AND cc.EmploymentType.Key = ? AND cc.MaxLoanAmount >= ? AND cm.NewBusinessIndicator = 'Y' AND osp.OriginationSource.Key = ? AND osp.Product.Key = ? AND cc.ExceptionCriteria = 0 "
            + "ORDER BY cc.MaxLoanAmount asc, cc.Margin.Value asc, cc.PTI desc";

            SimpleQuery<CreditCriteria_DAO> q = new SimpleQuery<CreditCriteria_DAO>(HQL, MortgageLoanPurposeKey, EmploymentTypeKey, TotalLoanAmount, OriginationSourceKey, ProductKey);
            q.SetQueryRange(1);
            CreditCriteria_DAO[] res = q.Execute();

            if (res.Length > 0
                && res[0].PTI.HasValue)
                return res[0].PTI.Value;
            else
                return 0.0;
        }

        public ICreditCriteria GetCreditCriteriaException(IDomainMessageCollection Messages, int OriginationSourceKey, int ProductKey, int MortgageLoanPurposeKey, int EmploymentTypeKey, double TotalLoanAmount)
        {
            //SELECT CC.* FROM [2AM].[dbo].[CreditCriteria] CC (NOLOCK)
            //JOIN [2AM].[dbo].[CreditMatrix] CM (NOLOCK) ON CC.CreditMatrixKey = CM.CreditMatrixKey
            //JOIN [2AM].[dbo].[OriginationSourceProductCreditMatrix] OSPCM (NOLOCK) ON CM.CreditMatrixKey = OSPCM.CreditMatrixKey
            //JOIN [2AM].[dbo].[OriginationSourceProduct] OSP (NOLOCK) ON OSPCM.OriginationSourceProductKey = OSP.OriginationSourceProductKey
            //WHERE   CC.MortgageLoanPurposeKey = @MortgageLoanPurposeKey
            //AND CC.EmploymentTypeKey = @EmploymentTypeKey
            //AND CC.MaxLoanAmount >= @TotalLoanAmount
            //AND OSP.OriginationSourceKey = @OriginationSourceKey
            //AND OSP.ProductKey = @ProductKey
            //AND CM.NewBusinessIndicator = 'Y';
            //AND CC.ExceptionCriteria = 1

            string HQL = "select cc from CreditCriteria_DAO cc join cc.CreditMatrix as cm join cc.CreditMatrix.OriginationSourceProducts as osp "
                + "where cc.MortgageLoanPurpose.Key = ? AND cc.EmploymentType.Key = ? AND cc.MaxLoanAmount >= ? AND cm.NewBusinessIndicator = 'Y' AND osp.OriginationSource.Key = ? AND osp.Product.Key = ? AND cc.ExceptionCriteria = 1 "
                + "ORDER BY cc.MaxLoanAmount desc, cc.Margin.Value asc, cc.LTV asc";

            SimpleQuery<CreditCriteria_DAO> q = new SimpleQuery<CreditCriteria_DAO>(HQL, MortgageLoanPurposeKey, EmploymentTypeKey, TotalLoanAmount, OriginationSourceKey, ProductKey);
            q.SetQueryRange(1);
            CreditCriteria_DAO[] res = q.Execute();

            if (res != null && res.Length > 0)
                return new CreditCriteria(res[0]);

            return null;
        }

        public ICreditCriteria GetCreditCriteriaByLTVAndIncome(IDomainMessageCollection messages, int originationSourceKey, int productKey, int mortgageLoanPurposeKey, int employmentTypeKey, double totalLoanAmount, double ltv, double income, CreditCriteriaAttributeTypes creditCriteriaAttributeType)
        {
            double validatedLtv = CheckLTVForCategory1(ltv);

            string sql = UIStatementRepository.GetStatement("COMMON", "GetCreditCriteria");

            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@MortgageLoanPurposeKey", mortgageLoanPurposeKey));
            prms.Add(new SqlParameter("@EmploymentTypeKey", employmentTypeKey));
            prms.Add(new SqlParameter("@TotalLoanAmount", totalLoanAmount));
            prms.Add(new SqlParameter("@LTV", (validatedLtv * 100)));
            prms.Add(new SqlParameter("@OriginationSourceKey", originationSourceKey));
            prms.Add(new SqlParameter("@ProductKey", productKey));
            prms.Add(new SqlParameter("@UnspecifiedMaxIncome", Int32.MaxValue));
            prms.Add(new SqlParameter("@Income", income));
            prms.Add(new SqlParameter("@CreditCriteriaAttributeTypeKey", (int)creditCriteriaAttributeType));

            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), prms);

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int key = Convert.ToInt32(row["CreditCriteriaKey"]);
                    CreditCriteria_DAO creditCriteriaDAO = CreditCriteria_DAO.Find(key);
                    return new CreditCriteria(creditCriteriaDAO);
                }
            }

            return null;
        }

        public IReadOnlyEventList<ICreditCriteria> GetCreditCriteriaByCategory(IDomainMessageCollection Messages, int OriginationSourceKey, int ProductKey, int MortgageLoanPurposeKey, int EmploymentTypeKey, double TotalLoanAmount, double LTV, int CategoryKey)
        {
            //SELECT CC.* FROM [2AM].[dbo].[CreditCriteria] CC (NOLOCK)
            //JOIN [2AM].[dbo].[CreditMatrix] CM (NOLOCK) ON CC.CreditMatrixKey = CM.CreditMatrixKey
            //JOIN [2AM].[dbo].[OriginationSourceProductCreditMatrix] OSPCM (NOLOCK) ON CM.CreditMatrixKey = OSPCM.CreditMatrixKey
            //JOIN [2AM].[dbo].[OriginationSourceProduct] OSP (NOLOCK) ON OSPCM.OriginationSourceProductKey = OSP.OriginationSourceProductKey
            //WHERE   CC.MortgageLoanPurposeKey = @MortgageLoanPurposeKey
            //AND CC.EmploymentTypeKey = @EmploymentTypeKey
            //AND CC.MaxLoanAmount >= @TotalLoanAmount
            //AND CC.LTV >= @LTV
            //AND OSP.OriginationSourceKey = @OriginationSourceKey
            //AND OSP.ProductKey = @ProductKey
            //AND CM.NewBusinessIndicator = 'Y'
            //AND CM.CategoryKey = @CategoryKey;

            double ltv = CheckLTVForCategory1(LTV);

            string HQL = "select cc from CreditCriteria_DAO cc join cc.CreditMatrix as cm join cc.CreditMatrix.OriginationSourceProducts as osp where cc.MortgageLoanPurpose.Key = ? "
                + "AND cc.EmploymentType.Key = ? AND cc.MaxLoanAmount >= ? AND cc.LTV >= ? AND cm.NewBusinessIndicator = 'Y' AND osp.OriginationSource.Key = ? AND osp.Product.Key = ? "
                + "AND cc.Category.Key = ? "
                + "ORDER BY cc.MaxLoanAmount asc, cc.Margin.Value asc, cc.LTV asc";

            SimpleQuery<CreditCriteria_DAO> q = new SimpleQuery<CreditCriteria_DAO>(HQL, MortgageLoanPurposeKey, EmploymentTypeKey, TotalLoanAmount, (ltv * 100), OriginationSourceKey, ProductKey, CategoryKey);

            CreditCriteria_DAO[] res = q.Execute();

            IEventList<ICreditCriteria> list = new DAOEventList<CreditCriteria_DAO, ICreditCriteria, CreditCriteria>(res);
            return new ReadOnlyEventList<ICreditCriteria>(list);
        }

        public ICreditCriteria GetCreditCriteriaByKey(int creditCriteriaKey)
        {
            CreditCriteria_DAO creditCriteria = CreditCriteria_DAO.TryFind(creditCriteriaKey);
            if (creditCriteria != null)
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<ICreditCriteria, CreditCriteria_DAO>(creditCriteria);
            }
            return null;
        }

        public ICreditCriteria GetCreditCriteriaForLatestAcceptedApplicationOnAccount(IAccount account)
        {
            var applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
            var latestAcceptedApplication = applicationRepository.GetLastestAcceptedApplication(account);
            ISupportsVariableLoanApplicationInformation svli = latestAcceptedApplication.CurrentProduct as ISupportsVariableLoanApplicationInformation;
            IApplicationInformationVariableLoan vli = svli.VariableLoanInformation;
            return vli.CreditCriteria;
        }

        private double CheckLTVForCategory1(double LTV)
        {
            double retLTV = LTV;

            IControl ctrl = CtrlRepo.GetControlByDescription("Maximum Category1 LTV");
            IControl ctrl1 = CtrlRepo.GetControlByDescription("Current Category1 LTV");

            if (ctrl != null && ctrl.ControlNumeric.HasValue)
            {
                if (LTV < ctrl.ControlNumeric.Value)
                {
                    if (ctrl1 != null && ctrl1.ControlNumeric.HasValue)
                        retLTV = ctrl1.ControlNumeric.Value;
                }
            }

            //return the lower of the 2 LTV's
            if (LTV < retLTV)
                return LTV;

            return retLTV;
        }

        private IControlRepository _ctrlRepo;

        private IControlRepository CtrlRepo
        {
            get
            {
                if (_ctrlRepo == null)
                    _ctrlRepo = SAHL.Common.Factories.RepositoryFactory.GetRepository<IControlRepository>();
                return _ctrlRepo;
            }
        }
    }
}