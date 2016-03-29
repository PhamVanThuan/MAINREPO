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
        /// <summary>
        /// Will get a single legalentityaddress by idnumber or legalname
        /// </summary>
        /// <param name="registeredName"></param>
        /// <param name="idnumber"></param>
        /// <param name="buildingname"></param>
        /// <param name="buildingnumber"></param>
        /// <param name="streetname"></param>
        /// <param name="streetNumber"></param>
        /// <param name="countryDescription"></param>
        /// <param name="provinceDescription"></param>
        /// <param name="suburbDescription"></param>
        /// <param name="registrationNumber"></param>
        /// <returns></returns>
        public QueryResultsRow GetLegalEntityAddress(string buildingname, string buildingnumber, string streetname, string streetNumber,
                    string countryDescription, string provinceDescription, string suburbDescription, string idnumber = "",
                    string registeredName = "", string registrationNumber = ""
            )
        {
            string query =
                String.Format(@"select lea.*, le.*, a.*
                                from dbo.legalentity le
                                inner join dbo.legalentityaddress lea on le.legalentitykey = lea.legalentitykey
                                inner join dbo.address a on lea.addresskey = a.addresskey
                                where  (le.registeredname = '{0}' or le.idnumber = '{1}'  or le.registrationnumber = '{2}')
	                            and a.BuildingName = '{3}' and a.BuildingNumber = '{4}'
	                            and a.StreetName = '{5}' and a.StreetNumber = '{6}'
	                            and a.RRR_CountryDescription = '{7}' and a.RRR_ProvinceDescription = '{8}'
	                            and a.RRR_SuburbDescription like '%{9}%'"
                            , registeredName, idnumber, registrationNumber, buildingname, buildingnumber,
                                streetname, streetNumber, countryDescription, provinceDescription, suburbDescription);
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLQuery(statement);
            if (results.RowList.Count == 0)
                return null;
            return results.Rows(0);
        }

        /// <summary>
        /// Select the top 01 (first) legalentity address that is found for the legalentity idnumber, passport, registrationnumber or legalname
        /// </summary>
        /// <param name="registeredName"></param>
        /// <param name="idnumber"></param>
        /// <param name="registrationNumber"></param>
        /// <returns></returns>
        public Automation.DataModels.LegalEntityAddress GetRandomLegalEntityAddress(string idnumber, string registeredName, string registrationNumber)
        {
            var p = new DynamicParameters();
            p.Add("@idnumber", value: idnumber, dbType: DbType.String, direction: ParameterDirection.Input);
            p.Add("@registeredName", value: registeredName, dbType: DbType.String, direction: ParameterDirection.Input);
            p.Add("@registrationNumber", value: registrationNumber, dbType: DbType.String, direction: ParameterDirection.Input);
            return dataContext.Query<Automation.DataModels.LegalEntityAddress>("[test].[GetRandomLegalEntityAddress]", parameters: p, commandtype: CommandType.StoredProcedure).FirstOrDefault();
        }

        public void DeleteLegalEntityDomiciliumAddress(int legalEntityKey)
        {
            var p = new DynamicParameters();
            p.Add("@legalEntityKey", value: legalEntityKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            dataContext.Execute("[2am].[test].[DeleteLegalEntityDomiciliumAddress]", parameters: p, commandtype: CommandType.StoredProcedure);
        }

        public Automation.DataModels.LegalEntityDomicilium InsertLegalEntityDomiciliumAddress(int legalEntityAddressKey, int legalEntityKey, GeneralStatusEnum generalStatus)
        {
            var p = new DynamicParameters();
            p.Add("@generalstatuskey", value: (int)generalStatus, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@legalentityaddresskey", value: legalEntityAddressKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            dataContext.Execute("[2am].[test].[InsertLegalEntityDomiciliumAddress]", parameters: p, commandtype: CommandType.StoredProcedure);
            return GetLegalEntityDomiciliumAddresses(legalEntityKey).Where(x => x.GeneralStatusKey == generalStatus).FirstOrDefault();
        }

        public void UpdateLegalEntityDomiciliumAddress(int legalentitydomiciliumkey, GeneralStatusEnum domiciliumStatus)
        {
            var p = new DynamicParameters();
            p.Add("@legalentitydomiciliumkey ", value: legalentitydomiciliumkey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@domiciliumStatusKey ", value: (int)domiciliumStatus, dbType: DbType.Int32, direction: ParameterDirection.Input);
            dataContext.Execute("[2am].[test].[UpdateLegalEntityDomiciliumAddress]", parameters: p, commandtype: CommandType.StoredProcedure);
        }

        public IEnumerable<Automation.DataModels.LegalEntityDomicilium> GetLegalEntityDomiciliumAddresses(int legalentitykey)
        {
            var query = String.Format(@"select led.*,lea.legalentitykey,a.addresskey, dbo.fGetFormattedAddress(a.addressKey) as FormattedAddress
                    , dbo.fGetFormattedAddressDelimited(a.addressKey, 0) as DelimitedAddress
                                                from [2am].dbo.LegalEntityDomicilium as led
 	                                            join [2am].dbo.legalentityaddress as lea on led.legalentityaddresskey=lea.legalentityaddresskey
                                                join [2am].dbo.address as a on lea.addresskey=a.addresskey
                                             where lea.legalentitykey = {0}
                                            ", legalentitykey);
            return dataContext.Query<Automation.DataModels.LegalEntityDomicilium>(query, commandtype: CommandType.Text);
        }

        public QueryResults GetLegalEntityAddressesByAccountKey(int accountkey)
        {
            string query = String.Format(@"select top 10 le.idnumber, [2am].dbo.fGetFormattedAddressDelimited (lea.addresskey,0) as address,
                                            lea.generalstatuskey
                                            from [2am].dbo.legalentity le
											inner join [2am].dbo.legalentityaddress lea on le.legalentitykey = lea.legalentitykey
											inner join [2am].dbo.role r on r.legalentitykey = le.legalentitykey
											where r.accountkey = {0}", accountkey);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        public IEnumerable<Automation.DataModels.LegalEntityAddress> GetLegalEntityAddresses(int legalEntityKey)
        {
            var query = string.Format(@"select
                                                lea.legalEntityAddressKey,
                                                dbo.fgetformattedaddressdelimited(a.addressKey,0) as DelimitedAddress,
                                                lea.AddressKey, dbo.fGetFormattedAddress(a.addressKey) as FormattedAddress,
                                                LegalEntityKey,
                                                GeneralStatusKey,
                                                at.description as AddressTypeDescription,
                                                a.AddressFormatKey,
                                                lea.AddressTypeKey,
                                                lea.LegalEntityAddressKey
                                            from [2am].dbo.legalentityaddress lea
                                            join [2am].dbo.address as a
                                                on lea.addresskey = a.addresskey
                                            join [2am].dbo.AddressType at on lea.addressTypeKey=at.addressTypeKey
                                            where legalentitykey={0} order by 1 desc", legalEntityKey);
            return dataContext.Query<Automation.DataModels.LegalEntityAddress>(query);
        }

        public void CleanupLegalEntityAddresses(int legalEntityKey, bool delete, GeneralStatusEnum status)
        {
            int del = delete ? 1 : 0;
            var p = new DynamicParameters();
            p.Add("@LegalEntityKey", value: legalEntityKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@Delete", value: del, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@GeneralStatusKey", value: (int)status, dbType: DbType.Int32, direction: ParameterDirection.Input);
            dataContext.Execute("test.CleanupLegalEntityAddresses", parameters: p, commandtype: CommandType.StoredProcedure);
        }
    }
}