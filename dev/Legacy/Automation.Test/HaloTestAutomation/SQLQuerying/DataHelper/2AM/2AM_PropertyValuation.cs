using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        public QueryResults GetProperty(string propertyKey)
        {
            string query =
                string.Format(@"select
                                p.propertyDescription1,p.propertyDescription2,p.propertyDescription3,
                                pt.description as propertyType,tt.description as titleType,ot.description as occupancyType,
                                pad.contact1 as inspectionContact1,pad.contact1phone as inspectionTel1,
                                unitNumber,buildingNumber,buildingName,streetNumber,streetName
                                ,rrr_ProvinceDescription,rrr_SuburbDescription,dbo.fGetFormattedAddressDelimited(a.addressKey,0) as formattedAddress
                                from [2am].dbo.property p with (nolock)
                                join [2am].dbo.titletype tt  with (nolock) on p.titletypekey = tt.titletypekey
                                join [2am].dbo.propertyType pt with (nolock) on p.propertyTypeKey = pt.propertyTypeKey
                                join [2am].dbo.occupancyType ot with (nolock) on p.occupancyTypeKey = ot.occupancyTypeKey
                                join [2am].dbo.propertyaccessdetails pad with (nolock )on p.propertykey = pad.propertykey
                                join [2am].dbo.address a with (nolock) on p.addressKey=a.addressKey
                                where p.PropertyKey = {0}", propertyKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        public Automation.DataModels.Property GetProperty(int propertyKey = 0, int accountkey = 0, int offerkey = 0)
        {
            var param = new DynamicParameters();
            param.Add("@PropertyKey", value: propertyKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@OfferKey", value: offerkey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@AccountKey", value: accountkey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            return dataContext.Query<Automation.DataModels.Property>("[test].[GetProperties]", parameters: param, commandtype: CommandType.StoredProcedure).FirstOrDefault();
        }

        /// <summary>
        /// Get the Address details of a property with an active Valuation less than 12 months old.
        /// To try ensure a satisfactory LTV when submitting an application a second constraint has been added so that only properties with a Valuation greater than a given Market Rate are returned.
        /// </summary>
        /// <returns>ValuationKey, ValuationDate, PropertyKey, StreetNumber, StreetName, ProvinceDescription, SuburbDescription, FormattedAddressDelimited, ValuationAmount</returns>
        public QueryResults GetAddressDetailsForPropertyValuation(int marketValue, bool greaterThan12Months, bool equalTo)
        {
            string query = string.Format(@"SELECT TOP (1) v.ValuationKey, v.ValuationDate, p.PropertyKey, a.StreetNumber, a.StreetName,
                a.RRR_ProvinceDescription as 'ProvinceDescription', a.RRR_SuburbDescription as 'SuburbDescription',
                [2am].[dbo].[fGetFormattedAddressDelimited] (a.addresskey,0) as 'FormattedAddressDelimited', v.ValuationAmount
                FROM [2am].dbo.Address AS a with (nolock)
                INNER JOIN [2am].dbo.Property AS p with (nolock) ON a.AddressKey = p.AddressKey
                INNER JOIN [2am].dbo.Valuation AS v with (nolock) ON p.PropertyKey = v.PropertyKey
                INNER JOIN [2am].dbo.PropertyAccessDetails AS pad with (nolock) ON p.PropertyKey = pad.PropertyKey
                WHERE
                a.Streetnumber is not null and
                a.Streetnumber <> '' and
                a.Streetname is not null and
                a.Streetname <> '' and
                v.Isactive = 1 and
                v.ValuationStatusKey = 2 and
                [token]
                v.ValuationAmount > {0}", marketValue);

            string greaterThanOrLessThan = greaterThan12Months == false ? "DateDiff(month,v.ValuationDate,getdate()) < 12 and" :
                "DateDiff(month,v.ValuationDate,getdate()) > 12 and";

            if (equalTo)
                greaterThanOrLessThan = "DateDiff(month,v.ValuationDate,getdate()) = 12 and ";
            query = query.Replace("[token]", greaterThanOrLessThan);

            var statement = new SQLStatement { StatementString = query };

            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// This method will set the propertykey on the offermortgageloan table.
        /// </summary>
        /// <param name="propertyKey"></param>
        /// <param name="offerKey"></param>
        /// <returns></returns>
        public bool UpdateOfferMortgageLoanPropertyKey(int propertyKey, int offerKey)
        {
            string query = @"update dbo.offermortgageloan
                             set offermortgageloan.propertykey = " + propertyKey +
                             "where offermortgageloan.offerkey = " + offerKey;
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Retrieves the property detail for the property linked to an Offer
        /// </summary>
        /// <param name="OfferKey"></param>
        /// <returns></returns>
        public QueryResults GetPropertyByOfferKey(int OfferKey)
        {
            string query = string.Format(@"
                            SELECT oml.PropertyKey
                            FROM [2am].[dbo].[offermortgageloan] oml (nolock)
                            WHERE
                                oml.OfferKey = {0}", OfferKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Get the Address details of a property with an active Valuation less than 12 months old.
        /// To try ensure a satisfactory LTV when submitting an application a second constraint has been added so that only properties with a Valuation greater than a given Market Rate are returned.
        /// </summary>
        /// <returns>ValuationKey, ValuationDate, PropertyKey, StreetNumber, StreetName, ProvinceDescription, SuburbDescription, FormattedAddressDelimited, ValuationAmount</returns>
        public QueryResults GetAddressDetailsForPropertyValuationLessThan12MonthsOld(int marketValue)
        {
            string query = @"SELECT     TOP (1) v.ValuationKey, v.ValuationDate, p.PropertyKey, a.StreetNumber, a.StreetName, a.RRR_ProvinceDescription as 'ProvinceDescription', a.RRR_SuburbDescription as 'SuburbDescription'
                                ,[2am].[dbo].[fGetFormattedAddressDelimited] (
                                   a.addresskey
                                  ,0) as 'FormattedAddressDelimited'
                                , v.ValuationAmount
                                FROM         Address AS a INNER JOIN
                                                      Property AS p ON a.AddressKey = p.AddressKey INNER JOIN
                                                      Valuation AS v ON p.PropertyKey = v.PropertyKey INNER JOIN
                                                      PropertyAccessDetails AS pad ON p.PropertyKey = pad.PropertyKey
                                WHERE
                                a.Streetnumber is not null and
                                a.Streetnumber <> '' and
                                a.Streetname is not null and
                                a.Streetname <> '' and
                                v.Isactive = 1 and
                                v.ValuationStatusKey = 2 and
                                DateDiff (month,v.ValuationDate,getdate()) < 12 and
                                v.ValuationAmount > " + marketValue;

            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Get the Address details of a property with an active Valuation greater than 12 months old.
        /// To try ensure a satisfactory LTV when submitting an application a second constraint has been added so that only properties with a Valuation greater than a given Market Rate are returned.
        /// </summary>
        /// <returns>ValuationKey, ValuationDate, PropertyKey, StreetNumber, StreetName, ProvinceDescription, SuburbDescription, FormattedAddressDelimited, ValuationAmount</returns>
        public QueryResults GetAddressDetailsForPropertyValuationGreaterThan12MonthsOld(int marketValue)
        {
            string query = string.Format(@"SELECT TOP (1) v.ValuationKey, v.ValuationDate, p.PropertyKey, a.StreetNumber, a.StreetName, a.RRR_ProvinceDescription as 'ProvinceDescription', a.RRR_SuburbDescription as 'SuburbDescription'
                                ,[2am].[dbo].[fGetFormattedAddressDelimited](a.addresskey,0) as 'FormattedAddressDelimited', v.ValuationAmount
                                FROM [2am].dbo.Address AS a
                                JOIN [2am].dbo.Property AS p ON a.AddressKey = p.AddressKey
                                JOIN [2am].dbo.Valuation AS v ON p.PropertyKey = v.PropertyKey
                                JOIN [2am].dbo.PropertyAccessDetails AS pad ON p.PropertyKey = pad.PropertyKey
                                WHERE  a.Streetnumber is not null
                                and a.Streetnumber <> ''
                                and a.Streetname is not null
                                and a.Streetname <> ''
                                and v.Isactive = 1
                                and v.ValuationStatusKey = 2
                                and DateDiff (month,v.ValuationDate,getdate()) > 12
                                and v.ValuationAmount > {0}
                                order by newid()", marketValue);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Get the Address details of a property with an active Valuation equal to 12 months old.
        /// To try ensure a satisfactory LTV when submitting an application a second constraint has been added so that only properties with a Valuation greater than a given Market Rate are returned.
        /// </summary>
        /// <returns>ValuationKey, ValuationDate, PropertyKey, StreetNumber, StreetName, ProvinceDescription, SuburbDescription, FormattedAddressDelimited, ValuationAmount</returns>
        public QueryResults GetAddressDetailsForPropertyValuation12MonthsOld(int marketValue)
        {
            string query = @"SELECT     TOP (1) v.ValuationKey, v.ValuationDate, p.PropertyKey, a.StreetNumber, a.StreetName, a.RRR_ProvinceDescription as 'ProvinceDescription', a.RRR_SuburbDescription as 'SuburbDescription'
                                ,[2am].[dbo].[fGetFormattedAddressDelimited] (
                                   a.addresskey
                                  ,0) as 'FormattedAddressDelimited'
                                , v.ValuationAmount
                                FROM         Address AS a INNER JOIN
                                                      Property AS p ON a.AddressKey = p.AddressKey INNER JOIN
                                                      Valuation AS v ON p.PropertyKey = v.PropertyKey INNER JOIN
                                                      PropertyAccessDetails AS pad ON p.PropertyKey = pad.PropertyKey
                                WHERE
                                a.Streetnumber is not null and
                                a.Streetnumber <> '' and
                                a.Streetname is not null and
                                a.Streetname <> '' and
                                v.Isactive = 1 and
                                v.ValuationStatusKey = 2 and
                                DateDiff (month,v.ValuationDate,getdate()) = 12 and
                                v.ValuationAmount > " + marketValue;

            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        public IEnumerable<Automation.DataModels.XmlHistory> GetValuationXmlHistory(int generickey)
        {
            string query = String.Format(@"SELECT * FROM dbo.XmlHistory WHERE generickey={0}", generickey);
            return dataContext.Query<Automation.DataModels.XmlHistory>(query);
        }

        public void UpdateDeedsOfficeDetails(int offerKey)
        {
            SQLStoredProcedure proc = new SQLStoredProcedure { Name = "test.UpdateDeedsOfficeDetails" };
            proc.AddParameter(new SqlParameter("@offerkey", offerKey));
            dataContext.ExecuteStoredProcedure(proc);
        }

        public Automation.DataModels.Property InsertProperty(Automation.DataModels.Property property)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@PropertyTypeKey", value: property.PropertyTypeKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@TitleTypeKey", value: property.TitleTypeKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@AreaClassificationKey", value: property.AreaClassificationKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@OccupancyTypeKey", value: property.OccupancyTypeKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@AddressKey", value: property.AddressKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@PropertyDescription1", value: property.PropertyDescription1, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@PropertyDescription2", value: property.PropertyDescription2, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@PropertyDescription3", value: property.PropertyDescription3, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@DeedsOfficeValue", value: property.DeedsOfficeValue, dbType: DbType.Double, direction: ParameterDirection.Input);
            parameters.Add("@CurrentBondDate", value: property.CurrentBondDate, dbType: DbType.DateTime, direction: ParameterDirection.Input);
            parameters.Add("@ErfNumber", value: property.ErfNumber, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@ErfPortionNumber", value: property.ErfPortionNumber, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@SectionalSchemeName", value: property.SectionalSchemeName, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@SectionalUnitNumber", value: property.SectionalUnitNumber, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@DeedsPropertyTypeKey", value: property.DeedsPropertyTypeKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@ErfSuburbDescription", value: property.ErfSuburbDescription, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@ErfMetroDescription", value: property.ErfMetroDescription, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@DataProviderKey", value: property.DataProviderKey, dbType: DbType.Int32, direction: ParameterDirection.Input);

            return dataContext.Query<Automation.DataModels.Property>("test.InsertProperty", parameters: parameters, commandtype: CommandType.StoredProcedure).FirstOrDefault();
        }

        public Automation.DataModels.Property UpdatePropertyAddress(int propertyKey, int addressKey)
        {
            var query = String.Format(@"update dbo.property
                                          set addressKey = {0}
                                          where propertyKey = {1}", addressKey, propertyKey);
            dataContext.Execute(query);
            return this.GetProperty(propertyKey: propertyKey);
        }

        public void CreatePropertyAccessDetails(int propertykey, string contact1, string contact1MobilePhone, string contact1Phone, string contact1WorkPhone, string contact2, string contact2Phone)
        {
            var query = String.Format(@"INSERT INTO [2AM].[dbo].[PropertyAccessDetails]
                                                       ([PropertyKey]
                                                       ,[Contact1]
                                                       ,[Contact1Phone]
                                                       ,[Contact1WorkPhone]
                                                       ,[Contact1MobilePhone]
                                                       ,[Contact2]
                                                       ,[Contact2Phone])
                                                 VALUES
                                                       ({0},
                                                        '{1}',
                                                        '{2}',
                                                        '{3}',
                                                        '{4}',
                                                        '{5}',
                                                        '{6}')
                                            GO", propertykey, contact1, contact1Phone, contact1WorkPhone, contact1MobilePhone, contact2, contact2Phone);
            dataContext.Execute(query);
        }

        public IEnumerable<DataModels.Property> GetMortgageLoanPropertiesForLegalEntity(int legalEntityKey)
        {
            var query = String.Format(@"SELECT p.*
                FROM [2AM].dbo.LegalEntity le (NOLOCK)
	                JOIN [2AM].dbo.Role r (NOLOCK) ON r.LegalEntityKey = le.LegalEntityKey
	                JOIN [2AM].dbo.Account acc (NOLOCK) ON acc.AccountKey = r.AccountKey AND acc.RRR_ProductKey in (1,2,9,11)
	                JOIN [2AM].dbo.FinancialService fs (NOLOCK) ON acc.AccountKey = fs.AccountKey
	                JOIN [2AM].fin.MortgageLoan ml (NOLOCK) ON fs.FinancialServiceKey = ml.FinancialServiceKey
	                JOIN [2AM].dbo.Property p (NOLOCK) ON ml.PropertyKey = p.PropertyKey
                WHERE le.LegalEntityKey = {0}", legalEntityKey);
            return dataContext.Query<Automation.DataModels.Property>(query);
        }
    }
}