using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// Gets the details of the HOC account
        /// </summary>
        /// <param name = "hocAccountKey">accountKey</param>
        /// <returns></returns>
        public Automation.DataModels.HOCAccount GetHOCAccount(int hocAccountKey)
        {
            //reavamp query
            string query = string.Format(
                        @"if object_id('tempdb..#data') is not null
                                begin
	                                drop table #data
                                end
                            select distinct
	                            isnull(ml.propertykey,oml.propertykey) as propertykey,
	                            hocAcc.accountkey,
	                            HOCfs.financialservicekey,
	                            HOCfs.payment,
	                            HOCAcc.AccountStatusKey
                            into #data
                            from [2am].dbo.account hocAcc
                                join [2am].dbo.financialService HOCfs on hocAcc.accountKey=HOCfs.accountKey and hocAcc.rrr_productkey = 3
                                left join [2am].dbo.financialservice as mlfs on HOCfs.parentfinancialservicekey = mlfs.financialservicekey and mlfs.financialServiceTypeKey = 1 and mlfs.parentFinancialServiceKey is null
	                            left join [2am].fin.mortgageLoan ml on mlfs.financialservicekey=ml.financialservicekey
	                            left join [2AM].dbo.OfferAccountRelationship oar on hocAcc.accountkey=oar.accountkey
	                            left join [2am].dbo.offer o on oar.offerkey=o.offerkey
	                            left join [2am].dbo.offermortgageLoan oml on oar.offerkey = oml.offerkey
                            where o.AccountKey = {0} or hocAcc.AccountKey = {0}

                                select
	                                d.AccountKey,
	                                d.FinancialServiceKey,
	                                d.Payment,
	                                d.AccountStatusKey as GeneralStatus,
	                                HOC.HOCInsurerKey,
	                                ins.Description as InsurerDescription,
	                                CommencementDate,
	                                AnniversaryDate,
	                                HOCStatusKey,
	                                SAHLPolicyNumber,
	                                HOCTotalSumInsured,
	                                hr.description as HOCRoof,
	                                hc.description as HOCConstruction,
	                                hs.description as HOCSubsidence,
	                                HOC.HOCThatchAmount,
	                                HOC.HOCConventionalAmount,
	                                HOC.HOCShingleAmount,
	                                HOC.Ceded,
	                                v.ValuationDate,
	                                d.PropertyKey
                                from #data d
	                                join [2am].dbo.Valuation v on d.propertyKey = v.propertyKey and  v.isactive = 1
	                                join [2am].dbo.HOC on d.financialServiceKey=HOC.financialServiceKey
	                                join [2am].dbo.HOCInsurer ins on HOC.HOCInsurerKey = ins.HOCInsurerKey
	                                left join [2am].dbo.HOCRoof as hr on HOC.HOCRoofKey = hr.HOCRoofKey
	                                left join [2am].dbo.HOCConstruction as hc on HOC.HOCConstructionKey = hc.HOCConstructionKey
	                                left join [2am].dbo.HOCSubsidence as hs on HOC.HOCSubsidenceKey = hs.HOCSubsidenceKey", hocAccountKey);
            var HOC = dataContext.Query<Automation.DataModels.HOCAccount>(query).FirstOrDefault();
            HOC.PropertyDetails = this.GetProperty(propertyKey: HOC.PropertyKey);
            return HOC;
        }

        public int GetHOCForOffer(int mortgageLoanOfferKey)
        {
            return dataContext.Query<int>(String.Format("select accountkey from dbo.OfferAccountRelationship where offerkey = {0}", mortgageLoanOfferKey)).FirstOrDefault();
        }

        public void CreateSAHLHOCAccount(int mortgageLoanOfferKey)
        {
            var p = new DynamicParameters();

            p.Add("@mortgageLoanOfferKey", value: mortgageLoanOfferKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@UserID", value: @"SAHL\HaloUser", dbType: DbType.String, direction: ParameterDirection.Input);

            dataContext.Execute("[2am].[test].[CreateSAHLHOC]", parameters: p, commandtype: CommandType.StoredProcedure);
        }
    }
}