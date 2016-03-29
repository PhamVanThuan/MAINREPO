using Common.Enums;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// Will find all the possible addresses in the dbo.address table, where the streetname match the given expression.
        /// </summary>
        /// <param name="streetNameExpression"></param>
        /// <returns></returns>
        public QueryResults GetAddressesByStreetName(string streetNameExpression)
        {
            string query = "select * from dbo.address where address.streetname = '" + streetNameExpression + "'";
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Will call the dbo.fGetFormattedAddressDelimited function, and return a formatted address.
        /// </summary>
        /// <param name="addressKey"></param>
        /// <returns>delimited string</returns>
        public string GetFormattedAddressByKey(int addressKey)
        {
            string query = string.Format(@"declare @AddressKey varchar(max)
                                           set @AddressKey = dbo.fGetFormattedAddressDelimited ({0}, 0)
                                           select @AddressKey", addressKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            QueryResults _results = dataContext.ExecuteSQLQuery(statement);
            return _results.Rows(0).Column(0).Value;
        }

        /// <summary>
        /// Get one or more addresses from the address table
        /// </summary>
        public IEnumerable<Automation.DataModels.Address> GetAddresses(string cityDescription = "", int addresskey = 0)
        {
            var query = "";
            if (!String.IsNullOrEmpty(cityDescription))
                query = String.Format(@"select address.*,addressformat.description as AddressFormatDescription,
                                                	[2am].dbo.fGetFormattedAddressDelimited (address.addresskey,0) as FormattedAddress
                                            from dbo.address
                                            inner join dbo.addressformat on address.addressformatkey = addressformat.addressformatkey
                                            where address.RRR_CityDescription = '{0}'", cityDescription);
            else if (addresskey != 0)
                query = String.Format(@"select a.*,af.description as AddressFormatDescription,
                                                	[2am].dbo.fGetFormattedAddressDelimited (a.addresskey,0) as FormattedAddress
                                            from dbo.address a
                                            inner join dbo.addressformat af
                                            on a.addressformatkey = af.addressformatkey
                                            where a.addresskey = {0}", addresskey);

            return dataContext.Query<Automation.DataModels.Address>(query);
        }

        /// <summary>
        /// This assertion will check if a residential address exists when provided with the details required
        /// </summary>
        /// <param name="streetNum">Street Number</param>
        /// <param name="streetName">Street Name</param>
        /// <param name="p">Province</param>
        /// <param name="suburb">Suburb</param>
        /// <param name="addressKey">Returns AddressKey if found</param>
        public int GetExistingResidentialAddress(string streetNum, string streetName, string p, string suburb)
        {
            int addressKey = 0;
            string query =
                string.Format(@"select * from Address (nolock)
                                    where StreetNumber = '{0}' and StreetName = '{1}'
                                    and RRR_ProvinceDescription = '{2}' and SuburbKey = (
                                    select SuburbKey from Suburb (nolock) where description = '{3}'
                                    ) and addressFormatKey = 1", streetNum, streetName, p, suburb);
            var statement = new SQLStatement() { StatementString = query };
            var r = dataContext.ExecuteSQLQuery(statement);
            if (!r.HasResults)
                throw new Exception("GetExistingResidentialAddress returned no rows.");
            return addressKey = r.Rows(0).Column(0).GetValueAs<int>();
        }

        /// <summary>
        /// This assertion will check if the link between the Address and the LegalEntity has been correctly stored in the
        /// LegalEntityAddress table.
        /// </summary>
        /// <param name="legalEntityKey">LegalEntityKey</param>
        /// <param name="addressKey">AddressKey</param>
        public bool IsAddressLegalEntityLinkByGeneralStatus(int legalEntityKey, int addressKey, GeneralStatusEnum gsKey)
        {
            string query =
                string.Format(@"select * from LegalEntityAddress (nolock)
                                    where AddressKey = {0} and LegalEntityKey = {1} and generalStatusKey = {2}",
                                addressKey, legalEntityKey, (int)gsKey);
            var statement = new SQLStatement() { StatementString = query };
            var r = dataContext.ExecuteSQLQuery(statement);
            return r.HasResults;
        }

        /// <summary>
        /// This assertion will check if an address record matching the 5 free text lines provided exists in the database, if it does
        /// then it will pass and return the addressKey
        /// </summary>
        /// <param name="line1">Free Text Line 1</param>
        /// <param name="line2">Free Text Line 2</param>
        /// <param name="line3">Free Text Line 3</param>
        /// <param name="line4">Free Text Line 4</param>
        /// <param name="line5">Free Text Line 5</param>
        /// <param name="addressKey"></param>
        public bool IsFreeTextAddress(string line1, string line2, string line3, string line4, string line5,
            out int addressKey)
        {
            string query =
                string.Format(@"select * from Address where freeText1 = '{0}'
                                    and freeText2 = '{1}' and freeText3 = '{2}' and freeText4 = '{3}'
                                    and freeText5 = '{4}'", line1, line2, line3, line4, line5);
            var statement = new SQLStatement() { StatementString = query };
            var r = dataContext.ExecuteSQLQuery(statement);
            addressKey = int.Parse(r.Rows(0).Column(0).Value);
            return r.HasResults;
        }

        /// <summary>
        /// Ensures that the correct address type has been saved to the LegalEntityAddress table.
        /// </summary>
        /// <param name="_legalEntityKey">legalEntityKey</param>
        /// <param name="addressKey">addressKey</param>
        public string GetAddressType(int _legalEntityKey, int addressKey)
        {
            string query =
                string.Format(@"select adt.Description from LegalEntityAddress (nolock)
                                    join addressType adt (nolock) on LegalEntityAddress.addressTypeKey = adt.addressTypeKey
                                    where AddressKey = {0} and LegalEntityKey = {1}", addressKey, _legalEntityKey);
            var statement = new SQLStatement() { StatementString = query };
            var r = dataContext.ExecuteSQLQuery(statement);
            return r.Rows(0).Column(0).Value;
        }

        /// <summary>
        /// Ensures that a PO Box address record exists
        /// </summary>
        /// <param name="boxNumber"></param>
        /// <param name="postOffice"></param>
        /// <param name="addressKey"></param>
        public bool IsExistingPOBoxAddress(string boxNumber, string postOffice, out int addressKey)
        {
            string query =
                string.Format(@"select * from address (nolock) where addressFormatKey=2
                                    and boxNumber = '{0}'
                                    and postofficeKey = (
                                    select postOfficeKey from PostOffice (nolock)
                                    where description = '{1}'
                                    )", boxNumber, postOffice);
            var statement = new SQLStatement() { StatementString = query };
            var r = dataContext.ExecuteSQLQuery(statement);
            addressKey = int.Parse(r.Rows(0).Column(0).Value);
            return r.HasResults;
        }

        /// <summary>
        /// Ensures that a Cluster Box address record exists
        /// </summary>
        /// <param name="clusterBox">Cluster Box Number</param>
        /// <param name="postOffice">Post Office</param>
        /// <param name="addressKey">AddressKey</param>
        public bool IsExistingClusterBoxAddress(string clusterBox, string postOffice, out int addressKey)
        {
            string query =
                string.Format(@"select * from address a
                                    where boxnumber = '{0}'
                                    and addressFormatKey = 6
                                    and postOfficeKey = (select postOfficeKey
                                    from postOffice where description = '{1}')", clusterBox, postOffice);

            var statement = new SQLStatement() { StatementString = query };
            var r = dataContext.ExecuteSQLQuery(statement);
            addressKey = int.Parse(r.Rows(0).Column(0).Value);
            return r.HasResults;
        }

        /// <summary>
        /// Asserts that a Cluster Box address record exists
        /// </summary>
        /// <param name="postNetSuite">PostNet Suite Number</param>
        /// <param name="privateBag">private Bag</param>
        /// <param name="postOffice">Post Office</param>
        /// <param name="addressKey">AddressKey</param>
        public bool IsExistingPostNetAddress(string postNetSuite, string privateBag, string postOffice, out int addressKey)
        {
            string query =
                string.Format(@"select * from address a where addressFormatKey=3
                                and boxNumber = '{0}' and suiteNumber = '{1}'
                                and postOfficeKey = (
                                select postOfficeKey
                                from postOffice
                                where description = '{2}')", privateBag, postNetSuite, postOffice);

            var statement = new SQLStatement() { StatementString = query };
            var r = dataContext.ExecuteSQLQuery(statement);
            addressKey = int.Parse(r.Rows(0).Column(0).Value);
            return r.HasResults;
        }

        /// <summary>
        /// Asserts that a private bag address exists
        /// </summary>
        /// <param name="privateBag">Private Bag Number</param>
        /// <param name="postOffice">Post Office</param>
        /// <param name="addressKey">out AddressKey if exists</param>
        public bool IsExistingPrivateBagAddress(string privateBag, string postOffice, out int addressKey)
        {
            string query =
                string.Format(@"select * from address a where addressFormatKey=4
                                and boxNumber = '{0}'
                                and postOfficeKey = (
                                select postOfficeKey
                                from postOffice
                                where description = '{1}')", privateBag, postOffice);
            var statement = new SQLStatement() { StatementString = query };
            var r = dataContext.ExecuteSQLQuery(statement);
            addressKey = int.Parse(r.Rows(0).Column(0).Value);
            return r.HasResults;
        }

        /// <summary>
        /// Removes all the legal entity addresses except for one on this case.
        /// </summary>
        /// <param name="_legalEntityKey"></param>
        public void SetupAddressData(int _legalEntityKey)
        {
            string sql =
                string.Format(@"declare @legalentityaddressKey int
                                select top 1 @legalentityaddressKey = legalentityaddressKey
                                from [2am].dbo.LegalEntityAddress
                                where legalentitykey={0} and generalstatuskey=1
                                order by 1 desc
                                delete from [2am].dbo.LegalEntityAddress
                                where legalEntityAddressKey <> @legalentityaddressKey
                                and legalentitykey={1}", _legalEntityKey, _legalEntityKey);
            var statement = new SQLStatement() { StatementString = sql };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Setups the only address as the Offer Mailing Address
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <param name="offerKey"></param>
        public void SetupAddressAsApplicationMailingAddress(int legalEntityKey, int offerKey)
        {
            string sql =
                string.Format(@"delete from [2am].dbo.offerMailingAddress
                                where offerKey = {0}

                                insert into [2am].dbo.OfferMailingAddress
                                select top 1 {1},addressKey,1,3,2,{2},1
                                from legalentityaddress
                                where legalentitykey={3} and generalstatuskey=1", offerKey, offerKey, legalEntityKey, legalEntityKey);

            var statement = new SQLStatement() { StatementString = sql };
            bool b = dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Sets up the legal entity address as the account mailing address.
        /// </summary>
        /// <param name="legalEntityKey">LegalEntityKey</param>
        /// <param name="accountKey">AccountKey</param>
        public void SetupAddressAsAccountMailingAddress(int legalEntityKey, int accountKey)
        {
            string sql =
                string.Format(@"delete from [2AM].dbo.mailingAddress where accountKey={0}

                                insert into [2AM].dbo.mailingAddress
                                select top 1 {0}, addressKey,1,3,2,{1},1
                                from [2AM].dbo.legalEntityAddress
                                where legalEntityKey={1}
                                and generalStatuskey=1", accountKey, legalEntityKey);

            var statement = new SQLStatement() { StatementString = sql };
            bool b = dataContext.ExecuteNonSQLQuery(statement);
        }

        public Automation.DataModels.Address InsertAddress(Automation.DataModels.Address addressToInsert)
        {
            string statement =
                string.Format(@"
                                    declare @addresskey int
                                    declare @postofficekey int
                                    set @postofficekey = (select top 1 postofficekey from [2am].dbo.PostOffice
			                                             where description ='{8}')
                                    INSERT INTO [2AM].[dbo].[Address]
                                       ([AddressFormatKey]
                                       ,[BoxNumber]
                                       ,[UnitNumber]
                                       ,[BuildingNumber]
                                       ,[BuildingName]
                                       ,[StreetNumber]
                                       ,[StreetName]
                                       ,[SuburbKey]
                                       ,[PostOfficeKey]
                                       ,[RRR_CountryDescription]
                                       ,[RRR_ProvinceDescription]
                                       ,[RRR_CityDescription]
                                       ,[RRR_SuburbDescription]
                                       ,[RRR_PostalCode]
                                       ,[UserID]
                                       ,[ChangeDate]
                                       ,[SuiteNumber]
                                       ,[FreeText1]
                                       ,[FreeText2]
                                       ,[FreeText3]
                                       ,[FreeText4]
                                       ,[FreeText5])
                                VALUES
                                       ({0},'{1}','{2}','{3}','{4}','{5}','{6}',{7}, @postofficekey,'{9}','{10}',
                                       '{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}')

                                SELECT * FROM dbo.Address
                                WHERE addresskey = SCOPE_IDENTITY()",
                                (int)addressToInsert.AddressFormatKey,
                                addressToInsert.BoxNumber,
                                addressToInsert.UnitNumber,
                                addressToInsert.BuildingNumber,
                                addressToInsert.BuildingName,
                                addressToInsert.StreetNumber,
                                addressToInsert.StreetName,
                                Helpers.IfZeroReturnNULL(addressToInsert.SuburbKey),
                                addressToInsert.PostOfficeDescription,
                                addressToInsert.RRR_CountryDescription,
                                addressToInsert.RRR_ProvinceDescription,
                                addressToInsert.RRR_CityDescription,
                                addressToInsert.RRR_SuburbDescription,
                                addressToInsert.RRR_PostalCode,
                                addressToInsert.UserID,
                                addressToInsert.ChangeDate,
                                addressToInsert.SuiteNumber,
                                addressToInsert.Line1,
                                addressToInsert.Line2,
                                addressToInsert.Line3,
                                addressToInsert.Line4,
                                addressToInsert.Line5);
            return dataContext.Query<Automation.DataModels.Address>(statement).FirstOrDefault();
        }

        public void DeleteAddress(int addresskey)
        {
            var query = String.Format("delete from dbo.address where addresskey = {0}", addresskey);
            dataContext.Execute(query);
        }
    }
}