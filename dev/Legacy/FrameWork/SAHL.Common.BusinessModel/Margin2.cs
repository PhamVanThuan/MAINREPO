using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Base;

namespace SAHL.Common.BusinessModel
{
    public partial class Margin : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Margin_DAO>, IMargin
    {
        public static IReadOnlyEventList<IMargin> GetForCreditMatrixCalc(IDomainMessageCollection Messages, int MortgageLoanPurposeKey, int EmploymentTypeKey, int OriginationSourceKey, int ProductKey, double TotalLoanAmount, double LTV)
        {
            //SELECT DISTINCT M.* FROM [2AM].[dbo].[CreditCriteria] CC (NOLOCK)  
            //JOIN [2AM].[dbo].[CreditMatrix] CM (NOLOCK) ON CC.CreditMatrixKey = CM.CreditMatrixKey  
            //JOIN [2AM].[dbo].[OriginationSourceProductCreditMatrix] OSPCM (NOLOCK) ON CM.CreditMatrixKey = OSPCM.CreditMatrixKey  
            //JOIN [2AM].[dbo].[OriginationSourceProduct] OSP (NOLOCK) ON OSPCM.OriginationSourceProductKey = OSP.OriginationSourceProductKey  
            //JOIN [2AM].[dbo].[Margin] M (NOLOCK) ON CC.MarginKey = M.MarginKey  
            //WHERE CC.MortgageLoanPurposeKey = @MortgageLoanPurposeKey  
            //AND CC.EmploymentTypeKey = @EmploymentTypeKey  
            //AND OSP.OriginationSourceKey = @OriginationSourceKey  
            //AND OSP.ProductKey = @ProductKey;

            string HQL = "select distinct m from CreditCriteria_DAO cc join cc.CreditMatrix as cm join cc.CreditMatrix.OriginationSourceProducts as osp join cc.Margin as m "
                + "where cc.MortgageLoanPurpose.Key = ? AND cc.EmploymentType.Key = ? AND osp.OriginationSource.Key = ? AND osp.Product.Key = ?";

            SimpleQuery<Margin_DAO> q = new SimpleQuery<Margin_DAO>(HQL, MortgageLoanPurposeKey, EmploymentTypeKey, OriginationSourceKey, ProductKey);

            Margin_DAO[] res = q.Execute();

            IEventList<IMargin> list = new DAOEventList<Margin_DAO, IMargin, Margin>(res);
            return new ReadOnlyEventList<IMargin>(list);
        }

        public static IReadOnlyEventList<IMargin> GetForCreditMatrixCalcByLTV(IDomainMessageCollection Messages, int MortgageLoanPurposeKey, int EmploymentTypeKey, int OriginationSourceKey, int ProductKey, double TotalLoanAmount, double LTV)
        {
            //SELECT DISTINCT M.* FROM [2AM].[dbo].[CreditCriteria] CC (NOLOCK)  
            //JOIN [2AM].[dbo].[CreditMatrix] CM (NOLOCK) ON CC.CreditMatrixKey = CM.CreditMatrixKey  
            //JOIN [2AM].[dbo].[OriginationSourceProductCreditMatrix] OSPCM (NOLOCK) ON CM.CreditMatrixKey = OSPCM.CreditMatrixKey  
            //JOIN [2AM].[dbo].[OriginationSourceProduct] OSP (NOLOCK) ON OSPCM.OriginationSourceProductKey = OSP.OriginationSourceProductKey  
            //JOIN [2AM].[dbo].[Margin] M (NOLOCK) ON CC.MarginKey = M.MarginKey  
            //WHERE CC.MortgageLoanPurposeKey = @MortgageLoanPurposeKey  
            //AND CC.EmploymentTypeKey = @EmploymentTypeKey  
            //AND CC.MaxLoanAmount >= @TotalLoanAmount  
            //AND CC.LTV >= @LTV  
            //AND OSP.OriginationSourceKey = @OriginationSourceKey  
            //AND OSP.ProductKey = @ProductKey;

            string HQL = "select distinct m from CreditCriteria_DAO cc join cc.CreditMatrix as cm join cc.CreditMatrix.OriginationSourceProducts as osp join cc.Margin as m "
                + "where cc.MortgageLoanPurpose.Key = ? AND cc.EmploymentType.Key = ? AND cc.MaxLoanAmount >= ? AND cc.LTV >= ? "
                + "AND osp.OriginationSource.Key = ? AND osp.Product.Key = ?";

            SimpleQuery<Margin_DAO> q = new SimpleQuery<Margin_DAO>(HQL, MortgageLoanPurposeKey, EmploymentTypeKey, TotalLoanAmount, LTV, OriginationSourceKey, ProductKey);

            Margin_DAO[] res = q.Execute();

            IEventList<IMargin> list = new DAOEventList<Margin_DAO, IMargin, Margin>(res);
            return new ReadOnlyEventList<IMargin>(list);
        }
        protected void OnMarginProducts_BeforeAdd(ICancelDomainArgs args, object Item)
        {

        }
        protected void OnMarginProducts_BeforeRemove(ICancelDomainArgs args, object Item)
        {

        }
        protected void OnProductCategories_BeforeAdd(ICancelDomainArgs args, object Item)
        {

        }
        protected void OnProductCategories_BeforeRemove(ICancelDomainArgs args, object Item)
        {

        }
        protected void OnRateConfigurations_BeforeAdd(ICancelDomainArgs args, object Item)
        {

        }
        protected void OnRateConfigurations_BeforeRemove(ICancelDomainArgs args, object Item)
        {

        }

        protected void OnMarginProducts_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }
        protected void OnMarginProducts_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }
        protected void OnProductCategories_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }
        protected void OnProductCategories_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }
        protected void OnRateConfigurations_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }
        protected void OnRateConfigurations_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }
    }
}


