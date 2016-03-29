using Common.Enums;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        public void UpdateValuationDateToDateLessThan12MonthsAgo(int offerKey)
        {
            string query =
                string.Format(@"update v
                                set valuationdate = dateadd(mm, -6, getdate())
                                from [2am].dbo.offermortgageloan oml
	                                join [2am].dbo.property p on oml.propertyKey = p.propertyKey
	                                join [2am].dbo.valuation v on p.propertyKey = v.propertyKey and isActive = 1
                                where oml.offerkey = {0}
	                                and v.valuationdate < dateadd(mm, -12, getdate())", offerKey);
            var statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        public Automation.DataModels.Valuation GetRandomValuationForAccountWithHOC(AccountStatusEnum accountStatus, HOCInsurerEnum hocInsurer, OriginationSourceEnum originationSource, HOCRoofEnum roofType)
        {
            var query = String.Format(@"select top 01 acc.accountkey, p.*,v.* from dbo.account as acc
                                                join dbo.financialservice as fs
                                                    on acc.accountkey=fs.accountkey
                                                join dbo.financialservice as childfs
                                                    on fs.financialservicekey=childfs.parentfinancialservicekey
                                                join dbo.hoc as h
                                                    on childfs.financialservicekey=h.financialservicekey
                                                join fin.mortgageloan as ml
                                                    on fs.financialservicekey=ml.financialservicekey
                                                join dbo.property as p
                                                    on p.propertykey=ml.propertykey
                                                join dbo.valuation as v
                                                    on v.propertykey=ml.propertykey
                                            where acc.accountstatuskey = {0} and h.hocinsurerkey = {1} and acc.rrr_originationsourcekey = {2}  and h.hocroofkey ={3}", (int)accountStatus, (int)hocInsurer, (int)originationSource, (int)roofType);
            return dataContext.Query<Automation.DataModels.Valuation>(query, commandtype: CommandType.Text).FirstOrDefault();
        }

        public IEnumerable<Automation.DataModels.Valuation> GetValuations(int propertyKey)
        {
            return dataContext.Query<Automation.DataModels.Valuation>(
                String.Format("select v.*,hf.description as HOCRoofDescription from dbo.valuation as v join dbo.hocroof as hf on v.hocroofkey=hf.hocroofkey where propertykey = {0}",
                propertyKey), commandtype: CommandType.Text);
        }

        /// <summary>
        ///   Get data for the latest valuation record
        /// </summary>
        /// <param name = "OfferKey">offer.offerkey</param>
        /// <returns>valuation.valuationkey, valuation.valuationdate, year(valuation.valuationdate) as 'Year', month(valuation.valuationdate) as 'Month', day(valuation.valuationdate) as 'Day', valuation.IsActive, valuationdataproviderdataservice.valuationdataproviderdataservicekey, dataprovider.dataproviderkey,  dataprovider.description as 'DataProvider', dataservice.dataservicekey, dataservice.description as 'DataService'</returns>
        public QueryResults GetLatestPropertyValuationData(int OfferKey)
        {
            string query =
                @"Select v.valuationkey, v.propertykey, v.valuationdate, year(v.valuationdate) as 'Year', month(v.valuationdate) as 'Month', day(v.valuationdate) as 'Day',
                v.IsActive, vdpd.valuationdataproviderdataservicekey, dp.dataproviderkey,  dp.description as 'DataProvider', ds.dataservicekey,
                ds.description as 'DataService'
                from [2am].[dbo].[valuation] v (nolock)
				join [2am].[dbo].[valuationdataproviderdataservice] vdpd on v.valuationdataproviderdataservicekey = vdpd.valuationdataproviderdataservicekey
				join [2am].[dbo].[dataproviderdataservice] dpd on vdpd.dataproviderdataservicekey = dpd.dataproviderdataservicekey
				join [2am].[dbo].[dataprovider] dp on dpd.dataproviderkey = dp.dataproviderkey
				join [2am].[dbo].[dataservice] ds on dpd.dataservicekey = ds.dataservicekey
                where v.valuationkey = (
                select max(v.valuationkey)
                from [2am].[dbo].[offer] o (nolock)
				join [2am].[dbo].[offermortgageloan] oml (nolock) on o.offerkey = oml.offerkey
				join [2am].[dbo].[property] p (nolock) on oml.propertykey = p.propertykey
				join [2am].[dbo].[valuation] v (nolock) on  p.propertykey = v.propertykey
                where o.offerkey = " +
                OfferKey + @"
                                group by o.offerkey)";

            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        public Automation.DataModels.Valuation InsertValuation(Automation.DataModels.Valuation valuation)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ValuatorKey", value: valuation.ValuatorKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@ValuationDate", value: valuation.ValuationDate, dbType: DbType.DateTime, direction: ParameterDirection.Input);
            parameters.Add("@ValuationAmount", value: valuation.ValuationAmount, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@ValuationHOCValue", value: valuation.ValuationHOCValue, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@ValuationMunicipal", value: valuation.ValuationMunicipal, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@ValuationUserID", value: valuation.ValuationUserID, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@PropertyKey", value: valuation.PropertyKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@HOCThatchAmount", value: valuation.HOCThatchAmount, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@HOCConventionalAmount", value: valuation.HOCConventionalAmount, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@HOCShingleAmount", value: valuation.HOCShingleAmount, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@ChangeDate", value: valuation.ChangeDate, dbType: DbType.DateTime, direction: ParameterDirection.Input);
            parameters.Add("@ValuationClassificationKey", value: valuation.ValuationClassificationKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@ValuationEscalationPercentage", value: valuation.ValuationEscalationPercentage, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@ValuationStatusKey", value: valuation.ValuationStatusKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@Data", value: valuation.Data, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@ValuationDataProviderDataServiceKey", value: valuation.ValuationDataProviderDataServiceKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@IsActive", value: valuation.IsActive, dbType: DbType.Boolean, direction: ParameterDirection.Input);
            parameters.Add("@HOCRoofKey", value: valuation.HOCRoofKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            return dataContext.Query<Automation.DataModels.Valuation>("test.InsertValuationRecord", parameters: parameters, commandtype: CommandType.StoredProcedure).FirstOrDefault();
        }

        public void DeleteValuationRecord(Automation.DataModels.Valuation valuation)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ValuationUserID", value: valuation.ValuationUserID, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@PropertyKey", value: valuation.PropertyKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@ChangeDate", value: valuation.ChangeDate, dbType: DbType.DateTime, direction: ParameterDirection.Input);
            parameters.Add("@IsActive", value: valuation.IsActive, dbType: DbType.Int32, direction: ParameterDirection.Input);
            dataContext.Execute("test.DeleteValuationRecord", parameters: parameters, commandtype: CommandType.StoredProcedure);
        }

        public void UpdateValuationStatus(Common.Enums.ValuationStatusEnum valuationStatus, int valuationkey)
        {
            dataContext.ExecuteNonSQLQuery(new SQLStatement
            {
                StatementString = String.Format(@"update dbo.valuation
                                                  set valuationstatuskey = {0}
                                                  where valuationkey = {1}", (int)valuationStatus, valuationkey)
            });
        }

        public void UpdateAllValuationStatuses(Common.Enums.ValuationStatusEnum valuationStatus, int propertyKey)
        {
            dataContext.ExecuteNonSQLQuery(new SQLStatement
            {
                StatementString = String.Format(@"update dbo.valuation
                                                  set valuationstatuskey = {0}
                                                  where propertykey = {1}", (int)valuationStatus, propertyKey)
            });
        }

        public IEnumerable<Automation.DataModels.Valuer> GetActiveValuators()
        {
            return dataContext.Query<Automation.DataModels.Valuer>("select * from dbo.valuator");
        }

        public void InsertXmlHistory(int xmlHistoryKey, string xmlData)
        {
            var query = String.Format(@"SET IDENTITY_INSERT xmlhistory ON
                                            INSERT INTO dbo.xmlhistory (XMLHistoryKey,GenericKeyTypeKey,GenericKey,XMLData,InsertDate)
                                            VALUES ({0},1,0,'{1}',GETDATE())", xmlHistoryKey, xmlData);
            dataContext.Execute(query);
        }

        public void DeleteXmlHistory(int xmlHistoryKey)
        {
            var query = String.Format(@"delete from xmlhistory where xmlhistorykey = {0}", xmlHistoryKey);
            dataContext.Execute(query);
        }

        public int GetMaxXmlHistoryKey()
        {
            return dataContext.Query<int>("select max(xmlhistorykey) from dbo.xmlhistory").FirstOrDefault();
        }

        public void UpdateValuationIsActive(int activeValKey, bool isActive)
        {
            var query = String.Format(@"update dbo.valuation set isactive={0} where valuationkey = {1}", Convert.ToInt32(isActive), activeValKey);
            dataContext.Execute(query);
        }

        public int InsertXmlHistory(string xml, int genericKey, GenericKeyTypeEnum genericKeyTypeKey)
        {
            string query =
                String.Format(@"INSERT INTO [2AM].[dbo].[XMLHistory] ([GenericKeyTypeKey],[GenericKey],[XMLData],[InsertDate])
                                    VALUES ( {0}, {1}, '{2}','{3}') select convert(int,SCOPE_IDENTITY())", (int)genericKeyTypeKey, genericKey, xml, DateTime.Now);
            return dataContext.Query<int>(query).FirstOrDefault();
        }

        public int UpdateXmlHistory(string xml, int xmlHistoryKey, GenericKeyTypeEnum genericKeyTypeKey)
        {
            string query =
                String.Format(@"UPDATE [2AM].[dbo].[XMLHistory]
                                    SET [XMLData] = '{0}', [InsertDate] = '{1}'
                                    WHERE XMLHistoryKey = {2}", xml, DateTime.Now, xmlHistoryKey);
            return dataContext.Query<int>(query).FirstOrDefault();
        }

        public int InsertValuationForOffer(int offerKey, bool isEzVal)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OfferKey", value: offerKey, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@IsEzVal", isEzVal, dbType: DbType.Boolean, direction: ParameterDirection.Input);
            parameters.Add("@ValuationKey", dbType: DbType.Int32, direction: ParameterDirection.Output);
            dataContext.Execute("test.InsertValuationForOffer", parameters: parameters, commandtype: CommandType.StoredProcedure);
            return parameters.Get<int>("@ValuationKey");
        }

        public void CreateXmlHistoryForOffer(int offerKey)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OfferKey", value: offerKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            dataContext.Execute("test.CreateXmlHistoryForOffer", parameters: parameters, commandtype: CommandType.StoredProcedure);
        }
    }
}