using Automation.DataModels;
using Common.Constants;
using Common.Enums;
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
        /// <summary>
        /// Returns the LegalEntityLegalName when provided with a LegalEntityKey
        /// </summary>
        /// <param name="legalEntityKey">legalEntityKey</param>
        /// <returns></returns>
        public QueryResults GetLegalEntityLegalNameByLegalEntityKey(int legalEntityKey)
        {
            string query = string.Format(@"select [2am].dbo.LegalEntityLegalName({0}, 0)", legalEntityKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLScalar(statement);
        }

        /// <summary>
        /// Returns the Legal Entity Legal Names and the LegalEntityKeys for the applicants linked to an application.
        /// </summary>
        /// <param name="offerKey">offerKey</param>
        /// <returns></returns>
        public QueryResults GetLegalEntityLegalNamesForOffer(int offerKey)
        {
            string query =
                string.Format(@"
                select [2AM].[dbo].[LegalEntityLegalName] (ol.LegalEntityKey, 0) as LegalEntityLegalName,
                ol.OfferRoleTypeKey, ol.LegalEntityKey, le.LegalEntityTypeKey, le.IDNumber, le.registrationNumber,
                le.emailaddress
                from [2AM].[dbo].OfferRole ol with (nolock) inner join
                [2AM].[dbo].LegalEntity le with (nolock)  on ol.LegalEntityKey = le.LegalEntityKey
                where ol.OfferKey = {0}
                and ol.OfferRoleTypeKey in (8,10,11,12) and ol.GeneralStatusKey = 1 order by le.legalentitykey",
                offerKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Returns the
        /// </summary>
        /// <param name="offerKey"></param>
        /// <returns></returns>
        public QueryResults GetLegalEntityIDNumberForOffer(int offerKey)
        {
            string query =
                string.Format(@"SELECT TOP 1
                                    le.IDNumber
                                FROM [2AM].[dbo].LegalEntity le (NOLOCK)
                                    INNER JOIN [2AM].[dbo].OfferRole ofr (NOLOCK) ON ofr.LegalEntityKey = le.LegalEntityKey
                                WHERE ofr.OfferKey = {0}
                                AND ofr.OfferRoleTypeKey = {1}", offerKey, (int)OfferRoleTypeEnum.LeadMainApplicant);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Returns true if the id number passed to the method exist.
        /// </summary>
        /// <param name="idnumber">ID Number</param>
        /// <returns>bool</returns>
        public bool LegalEntitiesWithIDNumberExist(string idnumber)
        {
            string checkIfLegalEntityExistsSQL = String.Format("select legalentity.idnumber from [2AM].[dbo].legalentity with (nolock)  where legalentity.idnumber = '{0}'", idnumber);
            string checkIfIdNumberExistsInTestTableSQL = String.Format("select idnumber from [2am].test.idNumbers with (nolock) where idNumber = '{0}'", idnumber);
            var statement = new SQLStatement { StatementString = checkIfLegalEntityExistsSQL };
            var statement2 = new SQLStatement { StatementString = checkIfIdNumberExistsInTestTableSQL };
            var legalEntityTable = dataContext.ExecuteSQLQuery(statement);
            var testTable = dataContext.ExecuteSQLQuery(statement2);
            return legalEntityTable.HasResults || testTable.HasResults;
        }

        /// <summary>
        ///   Gets the Legal Entity Legal Name for the Seller linked to an Application
        /// </summary>
        /// <param name = "offerKey">OfferKey</param>
        /// <returns>QueryResults</returns>
        public QueryResults GetSellerLegalName(int offerKey)
        {
            string query =
                string.Format(@"select dbo.LegalEntityLegalName(ofr.LegalEntityKey,0) as Seller
                                from [2AM].[dbo].offer o with (nolock)
                                join [2AM].[dbo].offerrole ofr with (nolock)  on o.offerkey=ofr.offerkey
                                and offerroletypekey in (9)
                                where o.offerkey={0}", offerKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Returns the Legal Entity full names and their roles that they play on the account. This used by the Client Super
        ///   Search in order to determine a main applicant to select from the search results
        /// </summary>
        /// <param name = "accountKey">AccountKey</param>
        /// <returns>Returns dbo.LegalEntityLegalName and RoleType.Description</returns>
        public QueryResults GetLegalEntityNamesAndRoleByAccountKey(int accountKey)
        {
            string query =
                string.Format(
                    @"select CASE le.LegalEntityTypeKey WHEN 2 THEN dbo.LegalEntityLegalName(r.LegalEntityKey,0)
                    ELSE le.RegisteredName END as Name,rt.Description as Role, le.LegalEntityKey
                    from [2AM].[dbo].Account a with (nolock)
                    join [2AM].[dbo].Role r with (nolock) on a.AccountKey=r.AccountKey
                    join [2AM].[dbo].RoleType rt with (nolock) on r.RoleTypeKey=rt.RoleTypeKey
                    join [2AM].[dbo].LegalEntity le with (nolock) on r.LegalEntityKey=le.LegalEntityKey
                    where a.AccountKey={0}",
                    accountKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Inserts a new Legal Entity
        /// </summary>
        /// <returns></returns>
        public Int32 CreateNewLegalEntity(string emailAddress, string idnumber)
        {
            string query = string.Format(@"insert into [2am].dbo.LegalEntity
                        (LegalEntityTypeKey, MaritalStatusKey, GenderKey, PopulationGroupKey, IntroductionDate, Salutationkey, FirstNames,
                        Initials, Surname, PreferredName, DateOfBirth, HomePhoneCode, HomePhoneNumber, WorkPhoneCode, WorkPhoneNumber, CellPhoneNumber,
                        EmailAddress, CitizenTypeKey, LegalEntityStatusKey, DocumentLanguageKey, IDNumber, educationKey, homelanguagekey)
                        VALUES
                        (2, 2, 2, 2, GetDate()-5, 1, 'Test' + convert(varchar(2), datepart(dd, GetDate()))
                                                                    + convert(varchar(2), datepart(hh, GetDate()))
                                                                    + convert(varchar(2), datepart(mm, GetDate()))
                                                                    + convert(varchar(2), datepart(ss, GetDate())) ,
                            'T', 'Case', 'Test Case', '1973/10/10', '021', '5555555','021', '5555555', '0555555555', '{0}', 1, 1, 2, '{1}', 5, 2)

                        select Scope_Identity()", emailAddress, idnumber);
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLScalar(statement);
            //This could be more robust, lets see how we go....
            return Convert.ToInt32(results.SQLScalarValue);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="registeredName"></param>
        /// <returns></returns>
        public int CreateNewLegalEntityAsDebtCounsellor(string registeredName)
        {
            var query = String.Format(@"
                BEGIN TRAN
                INSERT INTO [2AM].dbo.LegalEntity
                (
                    LegalEntityTypeKey,
                    IntroductionDate,
                    RegisteredName,
                    DocumentLanguageKey
                )
                VALUES
                (
                    3,
                    GETDATE(),
                    '{0}',
                    2
                )
                select Scope_Identity()
                COMMIT TRAN
            ", registeredName);

            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLScalar(statement);
            return Convert.ToInt32(results.SQLScalarValue);
        }

        /// <summary>
        /// Deletes a Legal Entity
        /// </summary>
        /// <param name="legalEntityKey">LegalEntityKey</param>
        public void DeleteLegalEntity(Int32 legalEntityKey)
        {
            string query = string.Format(@"Delete from [2am].dbo.LegalEntity where LegalEntitykey = {0}", legalEntityKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Adds an application mailing address to an Application by first inserting an address record against the LE
        /// and then inserting this address as the Application Mailing Address
        /// </summary>
        /// <param name="legalEntityKey">LegalEntityKey</param>
        /// <param name="offerKey">OfferKey</param>
        /// <returns>the original mailing adress to reset/cleanup later if required</returns>
        public int AddAppMailingAddressToLE(int legalEntityKey, int offerKey)
        {
            //get the old mailing address to cleanup later
            string query = String.Format(@"select AddressKey from [2AM].[dbo].OfferMailingAddress where OfferKey = {0}", offerKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLQuery(statement);
            int maAddressKey = !results.HasResults ? 0 : results.Rows(0).Column(0).GetValueAs<int>();

            //setup an address against the new legal entity
            query = String.Format(@"
                        insert into [2AM].[dbo].LegalEntityAddress
                        select top 1 {0}, a.AddressKey, 1, GetDate(), 1
                        from [2AM].[dbo].Address a
                        where a.AddressKey not in
                        (
                            select AddressKey
                            from [2AM].[dbo].LegalEntityAddress lea
                            join [2AM].[dbo].OfferRole ofr on lea.LegalEntityKey = ofr.LegalentityKey
                            where ofr.OfferKey = {1}
                        );

                        select top 1 AddressKey
                        from [2AM].[dbo].LegalEntityAddress
                        where LegalEntityKey = {0}
                        order by EffectiveDate desc;
                        ", legalEntityKey, offerKey);

            statement = null;
            statement = new SQLStatement { StatementString = query };

            int addKey = Convert.ToInt32(dataContext.ExecuteSQLScalar(statement).SQLScalarValue);
            //set the address as the mailing address
            SetMailingAddress(addKey, offerKey);

            //we will need to return the old mailing adress to cleanup later
            return maAddressKey;
        }

        /// <summary>
        /// Updatest the OfferMailingAddress to set the AddressKey for the Offer Provided
        /// </summary>
        /// <param name="addressKey">AddressKey to use</param>
        /// <param name="offerKey">OfferKey to use</param>
        public void SetMailingAddress(int addressKey, int offerKey)
        {
            string query = String.Format(@"update [2AM].[dbo].OfferMailingAddress
                            set AddressKey = {0}
                            where OfferKey = {1}", addressKey, offerKey);

            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Deletes all the LegalEntityAddress records for a given legal entity
        /// </summary>
        /// <param name="legalEntityKey">LegalEntityKey</param>
        public void DeleteLEAddress(Int32 legalEntityKey)
        {
            string query = String.Format(@"Delete from [2am].dbo.LegalEntityAddress where LegalEntitykey = {0}", legalEntityKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Get a Legal Entity's ID Number that is not linked to the Offer
        /// </summary>
        /// <param name="offerKey"></param>
        /// <returns>QueryResults that contains an ID Number</returns>
        public QueryResultsRow GetLegalEntityIDNumberNotLinkedToOffer(int offerKey)
        {
            string query = String.Format(@" select top 1 le.IDNumber, (select [2am].[dbo].LegalEntityLegalName(le.LegalEntityKey, 0)) as LegalEntityName, BasicIncome
                                            from [2AM].[dbo].LegalEntity le
                                            join [2AM].[dbo].OfferRole ofr on le.LegalEntityKey = ofr.LegalEntityKey
                                            join [2AM].[dbo].Employment emp on le.LegalEntityKey = emp.LegalEntityKey
                                            where LegalEntityTypeKey = 2
                                            and	GeneralStatusKey = 1
                                            and le.IDNumber is not null
                                            and	len(le.IDNumber) = 13 and offerKey <> {0}", offerKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            QueryResults results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0);
        }

        /// <summary>
        /// Update the DOB for a Legal Entity
        /// </summary>
        /// <param name="legalEntityKey">legalEntityKey</param>
        /// <param name="dateOfBirth">New Date Of Birth</param>
        public void UpdateDateOfBirth(int legalEntityKey, DateTime dateOfBirth)
        {
            string query = String.Format(@"update [2AM].[dbo].LegalEntity set DateOfBirth = convert(datetime, '{0}', 103) where	LegalEntityKey = {1}", dateOfBirth.ToString(Formats.DateFormat), legalEntityKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Gets the Legal Entity Legal Name when provided when an ID Number
        /// </summary>
        /// <param name="idNumber">ID Number</param>
        /// <returns>LegalEntityLegalName</returns>
        public string GetLegalEntityLegalNameByIDNumber(string idNumber)
        {
            string query = string.Format(@"select [2AM].[dbo].[LegalEntityLegalName] (le.LegalEntityKey, 0) as legalname From [2am].dbo.LegalEntity le	where le.IDNumber = {0}", Helpers.FormatStringForSQL(idNumber));
            SQLStatement statement = new SQLStatement { StatementString = query };
            QueryResults results = dataContext.ExecuteSQLScalar(statement);
            return results.SQLScalarValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public QueryResults ClientSuperSearchFirstLegalEntity()
        {
            string query = @"select top 01
                                legalentity.firstnames,
                                legalentity.surname,
                                legalentity.idnumber,
                                subsidy.salarynumber,
                                account.accountkey
                             from [2AM].[dbo].legalentity
                                inner join [2AM].[dbo].role
                                    on legalentity.legalentitykey = role.legalentitykey
                                inner join [2AM].[dbo].account
                                    on role.accountkey = account.accountkey
                                inner join [2AM].[dbo].accountsubsidy
                                    on account.accountkey = accountsubsidy.accountkey
                                inner join [2AM].[dbo].subsidy
                                    on accountsubsidy.subsidykey = subsidy.subsidykey";
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets a list of legal entities related to the legal entity passed to the method, as well as their relationship type.
        /// </summary>
        /// <param name="legalentityKey">legalEntityKey</param>
        /// <returns></returns>
        public QueryResults GetLegalEntityRelationship(int legalentityKey)
        {
            string query =
                string.Format(@"select le.idnumber, lrt.description, ler.legalentityrelationshipkey
                                from [2AM].[dbo].legalentityrelationship ler (nolock)
                                join [2AM].[dbo].legalentity le (nolock)
                                on ler.relatedlegalentitykey=le.legalentitykey
                                join [2AM].[dbo].legalEntityRelationshipType lrt (nolock)
                                on ler.relationshiptypekey=lrt.relationshiptypekey
                                where ler.legalentitykey={0}", legalentityKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// This will get all assets an liabilities saved against a legal entity.
        /// </summary>
        /// <param name="IdNumber">i.e. 8606185144082</param>
        /// <returns></returns>
        public QueryResults GetAssetsAndLiabilitiesByLegalEntityIdNumber(string IdNumber)
        {
            string query = String.Format(@"select * from [2AM].[dbo].legalentity
                                                inner join [2AM].[dbo].legalentityassetLiability
                                                    on legalentity.legalentitykey = legalentityassetLiability.legalentitykey
                                                inner join [2AM].[dbo].AssetLiability
                                                    on legalentityassetLiability.AssetLiabilityKey = AssetLiability.AssetLiabilityKey
                                           where legalentity.idnumber = '{0}'", IdNumber);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Get a legalentitykey provided the trading name.
        /// </summary>
        /// <returns></returns>
        public string GetLegalEntityKeyByTradingName(string tradingname)
        {
            string query = String.Format(@"select legalentity.legalentitykey from [2AM].[dbo].legalentity
                                           where legalentity.registeredname = '{0}'", tradingname);
            SQLStatement statement = new SQLStatement { StatementString = query };
            QueryResults results = dataContext.ExecuteSQLScalar(statement);
            return results.SQLScalarValue;
        }

        /// <summary>
        /// Gets LegalEntity LegalName when provided with a passport number
        /// </summary>
        /// <param name="passportNumber">passportNumber</param>
        /// <returns>LegalEntityLegalName</returns>
        public string GetLegalEntityLegalNameByPassportNumber(string passportNumber)
        {
            string query =
                string.Format(@"select [2AM].[dbo].[LegalEntityLegalName] (le.LegalEntityKey, 0), le.legalEntityKey
                From
                [2AM].[dbo].LegalEntity le
                where le.PassportNumber = {0}", Helpers.FormatStringForSQL(passportNumber));
            SQLStatement statement = new SQLStatement { StatementString = query };
            QueryResults results = dataContext.ExecuteSQLScalar(statement);
            return results.SQLScalarValue;
        }

        /// <summary>
        /// Get a legalentitykey provided the firstnames.
        /// </summary>
        /// <returns></returns>
        public string GetLegalEntityKeyByFirstNames(string firstNames)
        {
            string query = String.Format(@"select legalentity.legalentitykey from [2AM].[dbo].legalentity
                                           where legalentity.FirstNames = '{0}'", firstNames);
            SQLStatement statement = new SQLStatement { StatementString = query };
            QueryResults results = dataContext.ExecuteSQLScalar(statement);
            return results.SQLScalarValue;
        }

        /// <summary>
        /// Get a legalentitykey provided the firstnames.
        /// </summary>
        /// <returns></returns>
        public string GetLegalEntityCitizenType(string idNumber)
        {
            string query = String.Format(@"select legalentity.citizentypekey from [2AM].[dbo].legalentity
                                           where legalentity.idnumber = '{0}'", idNumber);
            SQLStatement statement = new SQLStatement { StatementString = query };
            QueryResults results = dataContext.ExecuteSQLScalar(statement);
            return results.SQLScalarValue;
        }

        /// <summary>
        /// Get a legalentitykey provided the IdNumber.
        /// </summary>
        /// <returns></returns>
        public int GetLegalEntityKeyByIdNumber(string IdNumber)
        {
            string query = String.Format(@"select legalentity.legalentitykey from [2AM].[dbo].legalentity
                                           where legalentity.IDNumber = '{0}'", IdNumber);
            SQLStatement statement = new SQLStatement { StatementString = query };
            QueryResults results = dataContext.ExecuteSQLScalar(statement);
            if (!results.HasResults)
                return 0;
            return Int32.Parse(results.SQLScalarValue);
        }

        /// <summary>
        /// Gets a single legalentity that plays a role in more than one mortgage account
        /// </summary>
        /// <returns></returns>
        public void GetFirstLegalEntityOnOpenMortgageAccounts(out int legalentityKey, out string legalentityIdNumber)
        {
            string query = @"select distinct top 01
                            le.legalentitykey,
                            le.IDNumber
                            from [2AM].[dbo].legalentity le
                            inner join [2AM].[dbo].role r
                            on le.legalentitykey = r.legalentitykey
                            and r.generalstatuskey = 1
                            inner join [2AM].[dbo].account a
                            on r.accountkey = a.accountkey
                            and a.accountstatuskey = 1
                            and a.rrr_originationsourcekey  = 1
                            and a.rrr_productkey in (1,2,5,6,9,11)
                            group by le.legalentitykey, r.accountkey,le.IDNumber
                            having count(le.legalentitykey) >=2
                            order by  le.legalentitykey desc";
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLQuery(statement);
            legalentityKey = results.Rows(0).Column("legalentitykey").GetValueAs<int>();
            legalentityIdNumber = results.Rows(0).Column("IDNumber").Value;
        }

        /// <summary>
        /// Get the first legal entity on account
        /// </summary>
        /// <param name="accountKey"></param>
        public QueryResultsRow GetFirstLegalEntityOnAccount(int accountKey)
        {
            string query = String.Format(@"select top 01	le.* from dbo.legalentity le
                                        inner join dbo.role r
                                        on le.legalentitykey = r.legalentitykey
                                        where r.accountkey={0}", accountKey);
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0);
        }

        /// <summary>
        /// Gets the Legal Entity Legal Name when provided a registration number.
        /// </summary>
        /// <param name="regNumber">RegistrationNumber</param>
        /// <returns>LegalEntityLegalName</returns>
        public string GetLegalEntityLegalNameByRegistrationNumber(string regNumber)
        {
            string query =
                string.Format(@"select [2AM].[dbo].[LegalEntityLegalName] (le.LegalEntityKey, 0), le.legalEntityKey
                                from LegalEntity le where le.RegistrationNumber = '{0}'", regNumber);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLScalar(statement).SQLScalarValue;
        }

        /// <summary>
        /// Get a Legal Entity's ID Number that is not linked to the Offer
        /// </summary>
        /// <param name="accountkeys">one or more accounts</param>
        /// <returns>return IdNumberr</returns>
        public string GetLegalEntityIDNumberNotLinkedToAccount(int[] accountkeys)
        {
            string accountkeyStr = Helpers.GetDelimitedString<int>(accountkeys, ",");
            string query = String.Format(@" select top 1 le.IDNumber
                                            from [2AM].[dbo].LegalEntity le join
                                            [2AM].[dbo].role r on le.LegalEntityKey = r.LegalEntityKey
                                            where LegalEntityTypeKey = 2 and GeneralStatusKey = 1 and
                                            le.IDNumber is not null and	len(le.IDNumber) = 13 and accountkey not in ({0}) and le.legalentitykey not in
                                            (
                                                select legalentitykey from dbo.LegalEntityException
                                            )", accountkeyStr);
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLScalar(statement);
            return results.SQLScalarValue;
        }

        /// <summary>
        /// Get first legalentity on open account with an email address.
        /// </summary>
        /// <returns>return idnumber</returns>
        public string GetLegalEntityIDWithEmailOnOpenAccount(out int accountkey)
        {
            string query = @"select top 1 le.idnumber, a.accountkey
                            from [2AM].[dbo].legalentity le
                            join [2AM].[dbo].role r	on le.legalentitykey = r.legalentitykey
                            join [2AM].[dbo].account a on r.accountkey = a.accountkey
                            where a.accountstatuskey=1
                            and a.rrr_productkey=1
                            and a.rrr_originationsourcekey=1
                            and r.generalstatuskey=1";
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLQuery(statement);
            accountkey = results.Rows(0).Column("accountkey").GetValueAs<int>();
            return results.Rows(0).Column("idnumber").Value;
        }

        /// <summary>
        /// Get the first legalentity and formatted active address for the provided offerkey
        /// </summary>
        /// <param name="offerkey"></param>
        /// <param name="legalentityAddressStatus"></param>
        /// <returns></returns>
        public QueryResults GetFirstLegalEntityAndFormattedAddressOnOffer(int offerkey, GeneralStatusEnum legalentityAddressStatus)
        {
            string query = string.Format(@" select distinct top 1 le.*,
                                            dbo.fGetFormattedAddressDelimited (lea.addresskey,0) as formattedaddress
                                            from [2AM].[dbo].legalentity le
                                            inner join [2AM].[dbo].legalentityaddress lea on le.legalentitykey = lea.legalentitykey
                                            inner join [2AM].[dbo].offerrole ofr on le.legalentitykey = ofr.legalentitykey
                                            where lea.generalstatuskey={0} and ofr.offerkey={1} and le.legalentitytypekey=2",
                                                        (int)legalentityAddressStatus, offerkey);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Get a single legalentity record provided the legalentitykey
        /// </summary>
        /// <param name="legalentityKey"></param>
        /// <returns></returns>
        public QueryResults GetLegalEntityByLegalEntityKey(int? legalentityKey)
        {
            string query = string.Format(@" select * from [2AM].[dbo].legalentity where legalentity.legalentitykey={0}", legalentityKey);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Fetches a list of legalentitys ID numbers for an account with the specified role type.
        /// </summary>
        /// <param name="accountKey">accountKey</param>
        /// <param name="roleType">roleType</param>
        /// <returns></returns>
        public QueryResults GetLegalEntityIDForAccountByRole(int accountKey, RoleTypeEnum roleType)
        {
            var query = String.Format(@"select le.IDNumber, r.generalStatusKey
                                    from [2AM].dbo.role r
                                    join [2AM].dbo.legalentity le
                                    on r.legalentityKey=le.legalentityKey and r.roleTypeKey={1}
                                    where r.accountKey={0}", accountKey, (int)roleType);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntityLoginStatus"></param>
        /// <param name="externalRoleStatus"></param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.LegalEntityLogin> GetLegalEntityLogin(GeneralStatusEnum legalEntityLoginStatus, GeneralStatusEnum externalRoleStatus)
        {
            string q =
                string.Format(@"select l.LegalEntityLoginKey, l.Username, l.GeneralStatusKey, l.LegalEntityKey, er.GenericKey as AttorneyKey
                                from [2am].dbo.legalentitylogin l
                                join [2am].dbo.ExternalRole er on l.legalEntityKey=er.legalEntityKey
                                and externalRoleTypeKey=10
                                where l.generalStatusKey={0}
                                and er.generalStatusKey={1}
                                order by 1 desc", (int)legalEntityLoginStatus, (int)externalRoleStatus);
            var legalEntityLogin = dataContext.Query<Automation.DataModels.LegalEntityLogin>(q);
            return legalEntityLogin;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="legalEntityLoginStatus"></param>
        /// <param name="externalRoleStatus"></param>
        public void UpdateLegalEntityLogin(string emailAddress, GeneralStatusEnum legalEntityLoginStatus, GeneralStatusEnum externalRoleStatus)
        {
            string query =
                string.Format(@"update ll
                                set ll.generalstatuskey={0}
                                from [2am].dbo.legalentitylogin ll
                                join [2am].dbo.externalRole er on ll.legalEntityKey=er.legalEntityKey
                                and er.externalRoleTypeKey=10
                                join [2am].dbo.legalentity l on ll.legalentitykey=l.legalentitykey and ll.username=l.emailAddress
                                where ll.username='{1}'
                                ", (int)legalEntityLoginStatus, emailAddress);
            var statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);

            query =
                 string.Format(@"update er
                                set er.generalstatuskey={0}
                                from [2am].dbo.legalentitylogin ll
                                join [2am].dbo.externalRole er on ll.legalEntityKey=er.legalEntityKey
                                and er.externalRoleTypeKey=10
                                join [2am].dbo.legalentity l on ll.legalentitykey=l.legalentitykey and ll.username=l.emailAddress
                                where ll.username='{1}'
                                ", (int)externalRoleStatus, emailAddress);
            statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Sets all the passwords to encrypted version of Natal1
        /// </summary>
        public void UpdateLegalEntityLoginPasswords()
        {
            string query = string.Format(@"update [2am].dbo.legalEntityLogin set password='{0}'", Common.Constants.Passwords.HashedVersionofNatal1);
            var statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Get a single legalentity record provided the idnumber
        /// </summary>
        /// <param name="idNumber"></param>
        /// <param name="registrationNumber"></param>
        /// <param name="registeredname"></param>
        /// <param name="legalname"></param>
        /// <param name="legalentitykey"></param>
        /// <returns></returns>
        public Automation.DataModels.LegalEntity GetLegalEntity(string idNumber, string registrationNumber, string registeredname, string legalname, int legalentitykey)
        {
            var where = "";
            var query = @"select
                                le.*,
                                let.Description as LegalEntityTypeDescription,
                                ms.Description as MaritalStatusDescription,
                                g.Description as GenderDescription,
                                pg.Description as PopulationGroupDescription,
                                st.Description as SalutationDescription,
                                ct.Description as CitizenTypeDescription,
                                les.Description as LegalEntityStatusDescription,
                                e.Description as EducationDescription,
                                l.Description as HomeLanguageDescription,
                                dl.Description as DocumentLanguageDescription
                            from dbo.LegalEntity le
                            inner join dbo.LegalEntityType let on le.LegalEntityTypeKey = let.LegalEntityTypeKey
                            left join dbo.CitizenType ct on le.CitizenTypeKey = ct.CitizenTypeKey
                            left join dbo.LegalEntityStatus les on le.LegalEntityStatusKey = les.LegalEntityStatusKey
                            left join dbo.Language dl on le.DocumentLanguageKey = dl.LanguageKey
                            left join dbo.MaritalStatus ms on le.MaritalStatusKey = ms.MaritalStatusKey
                            left join dbo.Gender g on le.GenderKey = g.GenderKey
                            left join dbo.PopulationGroup pg on le.PopulationGroupKey = pg.PopulationGroupKey
                            left join dbo.SalutationType st	on le.SalutationKey = st.SalutationKey
                            left join dbo.Education e on le.EducationKey= e.EducationKey
                            left join dbo.Language l on le.HomeLanguageKey = l.LanguageKey [token]";
            if (!String.IsNullOrEmpty(idNumber))
                where = String.Format("where le.idnumber='{0}'", idNumber);
            else if (!String.IsNullOrEmpty(registrationNumber))
                where = string.Format(@"where le.registrationNumber='{0}'", registrationNumber);
            else if (!String.IsNullOrEmpty(registeredname))
                where = string.Format(@"where le.registeredname='{0}'", registeredname);
            else if (!String.IsNullOrEmpty(legalentitykey.ToString()))
                where = string.Format(@"where le.legalentitykey={0}", legalentitykey);
            query = query.Replace("[token]", where);
            var leEnum = dataContext.Query<Automation.DataModels.LegalEntity>(query).GetEnumerator();
            leEnum.MoveNext();
            return leEnum.Current;
        }

        /// <summary>
        /// This will get all legalentities
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.LegalEntity> GetManyLegalEntities(CitizenTypeEnum citizenShipTypeKey, LegalEntityTypeEnum legalEntityTypeKey, LegalEntityExceptionStatusEnum legalEntityExceptionStatusKey)
        {
            var p = new DynamicParameters();
            p.Add("@citizenShipTypeKey", value: (int)citizenShipTypeKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@legalEntityTypeKey", value: (int)legalEntityTypeKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@legalEntityExceptionStatusKey", value: (int)legalEntityExceptionStatusKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            return dataContext.Query<Automation.DataModels.LegalEntity>("test.GetLegalEntities", parameters: p, commandtype: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Get a random legalentity row from the legalentity table.
        /// </summary>
        public QueryResultsRow GetRandomLegalEntityRecord(LegalEntityTypeEnum leType)
        {
            var statement = new SQLStatement { StatementString = "exec [test].[GetRandomLegalEntity]" };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0);
        }

        /// <summary>
        /// gets a legal entity to use in the debt counselling create screen
        /// </summary>
        /// <returns></returns>
        public string GetRandomLegalEntityIdNumberOnAccount()
        {
            string query =
                String.Format(@"select top 1 le.IDNumber from [2am].dbo.legalentity le (nolock)
                                join [2am].dbo.role r (nolock) on le.legalentitykey=r.legalentitykey
                                join [2am].dbo.account a (nolock) on r.accountkey=a.accountkey
                                where a.accountstatuskey=1
                                and rrr_productkey = 9
                                and LEN(le.IDNumber) = 13");
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column(0).Value;
        }

        /// <summary>
        /// Returns the SQL string that is executed in order to update the ID number for our test cases
        /// </summary>
        /// <param name="offerKey">offerKey</param>
        /// <param name="testIdentifier">testIdentifier</param>
        /// <returns>SQL string</returns>
        public void UpdateLegalEntityIDNumber(int offerKey, string testIdentifier)
        {
            string query = string.Format(@"
                                update test.CreditScoringTestApplicants
                                set legalEntityID = (
                                select le.idnumber from offerRole ofr
                                join legalEntity le on ofr.legalEntityKey=le.legalEntityKey
                                where offerKey= {0} and offerroletypeKey in (8,10,11,12)
                                )
                                where testIdentifier= '{1}'", offerKey, testIdentifier);
            var statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Updates the citizenship type for the LE's linked to an application
        /// </summary>
        /// <param name="citizenTypeKey"></param>
        /// <param name="offerKey">Application Number</param>
        public void UpdateCitizenTypeForApplicants(int citizenTypeKey, int offerKey)
        {
            string query;
            if (citizenTypeKey == 0)
            {
                query =
                    string.Format(@"update le
                                    set le.citizenTypeKey = null
                                    from [2am].dbo.offerRole o
                                    join [2am].dbo.legalEntity le on o.legalEntityKey=le.legalEntityKey
                                    where o.offerRoleTypeKey in (8,10,11,12)
                                    and o.offerKey = {0}", offerKey);
            }
            else
            {
                query =
                string.Format(@"update le
                                set le.citizenTypeKey = {0}
                                from [2am].dbo.offerRole o
                                join [2am].dbo.legalEntity le on o.legalEntityKey=le.legalEntityKey
                                where o.offerRoleTypeKey in (8,10,11,12)
                                and o.offerKey = {1}", citizenTypeKey, offerKey);
            }
            var statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Returns an ID Number for a legal entity relationship
        /// </summary>
        /// <returns></returns>
        public string GetIDNumberForRelationship()
        {
            string sql =
                string.Format(@"select top 1 idnumber from [2am]..legalentity le (nolock)
                                join [2am].dbo.role r (nolock) on le.legalentitykey=r.legalentitykey
                                join [2am].dbo.account a (nolock) on r.accountkey=a.accountkey and a.accountstatuskey=1
                                and a.rrr_productkey=9 and le.idnumber is not null
                                and len(le.idnumber)=13
                                order by le.legalentitykey desc");
            var statement = new SQLStatement { StatementString = sql };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0).Column(0).Value;
        }

        /// <summary>
        /// Returns all the legal entity roles against an account.
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.LegalEntityRole> GetLegalEntityRoles(int accountKey, bool isfullLegalName = true)
        {
            int getfullLegalName = isfullLegalName == false ? 1 : 0;
            string q =
                string.Format(@"select r.*,dbo.legalentitylegalname(r.legalentitykey,0) as LegalEntityLegalName, le.IDNumber,
                                le.LegalEntityTypeKey, rt.Description as RoleDescription
                                from [2am].dbo.Role r
                                join [2am].dbo.LegalEntity le on r.legalEntityKey=le.legalEntityKey
                                join [2am].dbo.RoleType rt on r.RoleTypeKey=rt.RoleTypeKey
                                where accountKey={0}", accountKey, getfullLegalName);
            return dataContext.Query<Automation.DataModels.LegalEntityRole>(q);
        }

        /// <summary>
        /// Returns all the legal entity roles against an account.
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.LegalEntityRole> GetLegalEntityRoles(string idnumber)
        {
            string q =
                string.Format(@"select r.*, le.IDNumber, rt.Description as RoleDescription
                                from [2am].dbo.Role r
                                join [2am].dbo.LegalEntity le on r.legalEntityKey=le.legalEntityKey
                                join [2am].dbo.RoleType rt on r.RoleTypeKey=rt.RoleTypeKey
                                where le.IDNumber='{0}'", idnumber);
            return dataContext.Query<Automation.DataModels.LegalEntityRole>(q);
        }

        /// <summary>
        /// Updates a legal entity's ID Number
        /// </summary>
        /// <param name="idNumber">new ID Number</param>
        /// <param name="legalEntityKey">legalEntityKey</param>
        public void UpdateLegalEntityIDNumber(string idNumber, int legalEntityKey)
        {
            var query =
                 string.Format(@"update [2am].dbo.legalEntity
                                set idnumber = '{0}'
                                where legalEntityKey = {1}", idNumber, legalEntityKey);
            var statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Returns a data set containing Legal Entities who have more than 1 mortgage loan account. It includes the
        /// legalEntityKey, a count of accounts and the latest account.
        /// </summary>
        public QueryResults LegalEntitiesWithMoreThanOneAccount()
        {
            var query =
                string.Format(@"with roles as (
                                select r.*
                                from [2am].dbo.role r
                                join [2am].dbo.account a  on r.accountkey=a.accountkey and a.accountstatuskey=1
                                and rrr_productkey in (1,9)
                                where roletypekey in (2,3)
                                and generalstatuskey=1
                                and a.accountstatuskey = 1
                                )

                                select legalentitykey, count(accountkey) as CountOfAccounts, max(accountKey) as MaxAccountNumber
                                from roles
                                group by legalentitykey
                                having count(accountKey) > 1
                                order by 2 asc, newid()");
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Returns all the legal entity roles against an account.
        /// </summary>
        ///<param name="isfullLegalName">True = Full LE Name, False = Initials</param>
        ///<param name="legalEntityKey">LegalEntityKey</param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.LegalEntityRole> GetAllLegalEntityRoles(int legalEntityKey, bool isfullLegalName = true)
        {
            int getfullLegalName = isfullLegalName == false ? 1 : 0;
            string q =
                string.Format(@"select r.*,dbo.legalentitylegalname(r.legalentitykey, {0}) as LegalEntityLegalName, le.IDNumber, le.LegalEntityTypeKey,
                                rt.Description as RoleDescription, a.rrr_productKey as ProductKey
                                from [2am].dbo.Role r
                                join [2am].dbo.Account a on r.AccountKey=a.AccountKey
                                join [2am].dbo.LegalEntity le on r.legalEntityKey=le.legalEntityKey
                                join [2am].dbo.RoleType rt on r.RoleTypeKey=rt.RoleTypeKey
                                where r.legalEntityKey={1} and r.generalStatusKey=1", getfullLegalName, legalEntityKey);
            return dataContext.Query<Automation.DataModels.LegalEntityRole>(q);
        }

        /// <summary>
        /// Inserts an Asset Liability record for an offer, is the LAA > R1.5mil
        /// </summary>
        /// <param name="offerKey"></param>
        public void InsertOfferAssetLiability(int offerKey)
        {
            var proc = new SQLStoredProcedure { Name = "test.InsertOfferAssetLiability" };
            proc.AddParameter(new SqlParameter("@offerKey", offerKey.ToString()));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// Inserts a Seller
        /// </summary>
        /// <param name="offerKey"></param>
        public void InsertSeller(int offerKey)
        {
            SQLStoredProcedure proc = new SQLStoredProcedure { Name = "test.InsertSeller" };
            proc.AddParameter(new SqlParameter("@offerKey", offerKey.ToString()));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// Returns a legal entity
        /// </summary>
        /// <returns></returns>
        public QueryResultsRow GetUnrelatedLegalEntity(LegalEntityTypeEnum legalEntityType)
        {
            string query = string.Format(@"select top 1 * from [2am].[dbo].legalentity le
                left join [2am].[dbo].role r on le.legalentitykey=r.legalentitykey
                left join [2am].[dbo].offerRole ofr on le.legalEntityKey=ofr.legalEntityKey
                where r.legalentitykey is null and ofr.legalEntityKey is null and le.legalEntityTypeKey = {0} [token]", (int)legalEntityType);
            string token = string.Empty;
            switch (legalEntityType)
            {
                case LegalEntityTypeEnum.Unknown:
                    break;

                case LegalEntityTypeEnum.NaturalPerson:
                    token = "and len(le.idnumber)=13 and len(le.emailAddress) > 0 order by newid()";
                    break;

                case LegalEntityTypeEnum.Company:
                    token = "and len(le.registrationNumber) > 5 and len(le.emailAddress) > 0 order by newid()";
                    break;

                case LegalEntityTypeEnum.CloseCorporation:
                    token = "and len(le.registrationNumber) > 5 and len(le.emailAddress) > 0 order by newid()";
                    break;

                case LegalEntityTypeEnum.Trust:
                    token = "and len(le.registrationNumber) > 5 and len(le.emailAddress) > 0 order by newid()";
                    break;

                default:
                    break;
            }
            query = query.Replace("[token]", token);
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLQuery(statement);
            return (from r in results select r).FirstOrDefault();
        }

        public Automation.DataModels.LegalEntityAssetLiabilities InsertLegalEntityAssetLiability(Automation.DataModels.LegalEntityAssetLiabilities legalEntityAssetLiability)
        {
            var p = new DynamicParameters();
            p.Add("@AssetLiabilityTypeKey", value: (int)legalEntityAssetLiability.AssetLiabilityTypeKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@AssetLiabilityKey", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@AssetLiabilitySubTypeKey", value: (int)legalEntityAssetLiability.AssetLiabilitySubTypeKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@AddressKey", value: legalEntityAssetLiability.AddressKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@AssetValue", value: legalEntityAssetLiability.AssetValue, dbType: DbType.Currency, direction: ParameterDirection.Input);
            p.Add("@LiabilityValue", value: legalEntityAssetLiability.LiabilityValue, dbType: DbType.Currency, direction: ParameterDirection.Input);
            p.Add("@CompanyName", value: legalEntityAssetLiability.CompanyName, dbType: DbType.String, direction: ParameterDirection.Input);
            p.Add("@Cost", value: legalEntityAssetLiability.Cost, dbType: DbType.Currency, direction: ParameterDirection.Input);
            p.Add("@Date", value: legalEntityAssetLiability.Date, dbType: DbType.DateTime, direction: ParameterDirection.Input);
            p.Add("@Description", value: legalEntityAssetLiability.OtherDescription, dbType: DbType.String, direction: ParameterDirection.Input);
            p.Add("@LegalEntityKey", value: legalEntityAssetLiability.LegalEntityKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@GeneralStatusKey", value: (int)legalEntityAssetLiability.GeneralStatusKey, dbType: DbType.Int32, direction: ParameterDirection.Input);

            dataContext.Execute("test.InsertLegalEntityAssetLiability", parameters: p, commandtype: CommandType.StoredProcedure);

            var assetLiabilityKey = p.Get<int>("@AssetLiabilityKey");

            return (from assetLiab in this.GetLegalEntityAssetLiabilities(legalEntityAssetLiability.LegalEntityKey)
                    where assetLiab.AssetLiabilityKey == assetLiabilityKey
                    select assetLiab).FirstOrDefault();
        }

        public IEnumerable<Automation.DataModels.LegalEntityAssetLiabilities> GetLegalEntityAssetLiabilities(int legalentitykey)
        {
            var p = new DynamicParameters();
            p.Add("@LegalEntityKey", value: legalentitykey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            var leAssetLiabilities = dataContext.Query<Automation.DataModels.LegalEntityAssetLiabilities>(@"test.GetLegalEntityAssetsLiabilities", parameters: p, commandtype: CommandType.StoredProcedure);
            foreach (var leAssLib in leAssetLiabilities)
            {
                if (leAssLib.AddressKey > 0)
                    leAssLib.Address = this.GetAddresses(addresskey: leAssLib.AddressKey).FirstOrDefault();
            }
            return leAssetLiabilities;
        }

        public void DeleteLegalEntityAssetLiabilities(int legalentitykey, int assetLiabilitykey)
        {
            dataContext.Execute(String.Format(@"delete from dbo.legalentityassetliability
                                           where legalentityassetliability.LegalEntityKey = {0}
                                           delete from dbo.assetliability
                                           where assetliabilitykey = {1}
                                         ", legalentitykey, assetLiabilitykey));
        }

        public IEnumerable<Automation.DataModels.AssetLiabilityType> GetAssetLiabilityTypes()
        {
            return dataContext.Query<Automation.DataModels.AssetLiabilityType>("select * from dbo.assetliabilitytype");
        }

        public IEnumerable<Automation.DataModels.Account> GetLegalEntityAccounts(int legalentitykey)
        {
            var query = String.Format(@"select * from dbo.legalentity
                                                inner join dbo.role
                                                    on legalentity.legalentitykey = role.legalentitykey
                                                inner join dbo.account
                                                    on role.accountkey = account.accountkey
                                            where legalentity.legalentitykey = {0}", legalentitykey);
            return dataContext.Query<Automation.DataModels.Account>(query);
        }

        /// <summary>
        /// Gets all of the mortgage loan accounts that a legal entity plays a main applicant role in when provided with the legal entity's
        /// id number
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        public List<Automation.DataModels.Account> GetLegalEntityLoanAccountsByIDNumber(string idNumber)
        {
            var query = String.Format(@"
                        select distinct a.accountKey
                        from [2am].dbo.legalEntity le with (nolock)
                        join [2am].dbo.role r  with (nolock) on le.legalentitykey = r.legalentitykey
                            and r.roletypekey in (2)
                            and r.generalStatusKey=1
                        join [2am].dbo.account a  with (nolock) on r.accountkey = a.accountkey
                            and a.accountStatusKey=1
                            and rrr_productKey in (1,2,5,6,9,11,12)
                        where le.idnumber = '{0}'", idNumber);
            return dataContext.Query<Automation.DataModels.Account>(query).ToList();
        }

        public IEnumerable<Automation.DataModels.LegalEntity> GetLegalEntitiesNotPlayRole(RoleTypeEnum roleTypeEnum)
        {
            var query = String.Format(@"select top 01 legalentity.* from dbo.legalentity
                                                inner join dbo.role
                                                    on legalentity.legalentitykey = role.legalentitykey
                                                inner join dbo.financialservice
                                                    on role.accountkey = financialservice.accountkey
                                            where legalentity.legalentitykey not in
                                            (
                                                select role.legalentitykey from dbo.role
                                                where roletypekey = {0}
                                            )
                                            and financialservice.accountstatuskey = 1 and maritalstatuskey is not null
                                            and	legalentity.populationgroupkey is not null and	legalentity.genderkey is not null
                                            and	legalentity.salutationkey is not null and legalentity.initials is not null
                                            and legalentity.salutationkey is not null", (int)roleTypeEnum);
            return dataContext.Query<Automation.DataModels.LegalEntity>(query);
        }

        public IEnumerable<Automation.DataModels.LegalEntityRole> GetLegalEntityWithMoreThanOneSameRole(RoleTypeEnum roleTypeEnum, bool inclHOCAccounts = true)
        {
            string loanAccountFilter = inclHOCAccounts ? string.Empty : "and rrr_productKey in (1,2,5,6,9,11)";

            string query = String.Format(@"select le.*, r.*
                                    from [2am].dbo.legalentity le
                                    inner join
                                    (
                                        select legalentitykey, count(r.accountKey) as NoOfAccounts
                                        from dbo.role r
                                        join dbo.account a on r.accountkey = a.accountkey
                                            and a.accountstatuskey = 1
                                            {0}
                                        left join debtcounselling.debtcounselling dc on a.accountKey=dc.accountKey
                                            and dc.debtcounsellingStatusKey=1
                                        where r.roletypekey = {1}
                                        and generalstatuskey = 1
                                        and dc.accountkey is null
                                        group by legalentitykey
                                        having count(r.accountKey) > 1
                                    ) as m on le.legalentitykey=m.legalentitykey
                                    join [2am].dbo.role r on r.legalentitykey = le.legalentitykey
                                    join [2am].dbo.account acc on acc.accountkey = r.accountkey
                                        {2}
                                    where maritalstatuskey is not null
                                    and  le.populationgroupkey is not null
                                    and  le.genderkey is not null
                                    and  le.salutationkey is not null
                                    and le.initials is not null
                                    and le.salutationkey is not null
                                    and r.roletypekey = {3}
                                    order by NoOfAccounts asc", loanAccountFilter, (int)roleTypeEnum, loanAccountFilter, (int)roleTypeEnum);
            return dataContext.Query<Automation.DataModels.LegalEntityRole>(query);
        }

        public Automation.DataModels.SubsidyProvider GetSubsidyProvider(string registeredName)
        {
            var query = String.Format(@"select
                                            sp.ContactPerson,
                                            spt.SubsidyProviderTypeKey,
                                            description as SubsidyProviderTypeDescription,
                                            le.*
                                        from [2am].dbo.subsidyprovider sp
                                            inner join [2am].dbo.legalentity le on sp.legalentitykey = le.legalentitykey
                                            inner join [2am].dbo.SubsidyProviderType spt on sp.SubsidyProviderTypeKey = spt.SubsidyProviderTypeKey
                                        where le.registeredname = '{0}' or sp.contactperson = '{0}'", registeredName);

            return dataContext.Query<Automation.DataModels.SubsidyProvider, Automation.DataModels.LegalEntity, Automation.DataModels.SubsidyProvider>(query,
                (sp, le) =>
                {
                    sp.LegalEntity = le;
                    return sp;
                },
                splitOn: "LegalEntityKey", commandtype: CommandType.Text).FirstOrDefault();
        }

        public Automation.DataModels.SubsidyProvider GetAllSubsidyProvider(string registeredName)
        {
            var query = String.Format(@"select  sp.SubsidyProviderTypeKey,
                                                ContactPerson,
                                                WorkPhoneCode,
                                                WorkPhoneNumber,
                                                BoxNumber,
                                                RRR_PostalCode,
                                                RRR_SuburbDescription,
                                                description as SubsidyProviderTypeDescription
                                            from [2am].dbo.subsidyprovider sp
                                                join [2am].dbo.legalentity le on sp.legalentitykey = le.legalentitykey
                                                join [2am].dbo.SubsidyProviderType spt on sp.SubsidyProviderTypeKey = spt.SubsidyProviderTypeKey
                                                join [2am].dbo.legalentityaddress lea on le.legalentitykey = lea.legalentitykey
                                                join [2am].dbo.address a on lea.Addresskey = a.Addresskey
                                                where le.registeredname = '{0}'", registeredName);
            return dataContext.Query<Automation.DataModels.SubsidyProvider>(query).FirstOrDefault();
        }

        public Automation.DataModels.LegalEntity InsertLegalEntity(Automation.DataModels.LegalEntity legalentity)
        {
            var query = String.Format(@"
                                BEGIN TRAN
                                    INSERT INTO [2AM].[dbo].[LegalEntity]
                                           ([LegalEntityTypeKey]
                                           ,[MaritalStatusKey]
                                           ,[GenderKey]
                                           ,[PopulationGroupKey]
                                           ,[IntroductionDate]
                                           ,[Salutationkey]
                                           ,[FirstNames]
                                           ,[Initials]
                                           ,[Surname]
                                           ,[PreferredName]
                                           ,[IDNumber]
                                           ,[PassportNumber]
                                           ,[TaxNumber]
                                           ,[RegistrationNumber]
                                           ,[RegisteredName]
                                           ,[TradingName]
                                           ,[DateOfBirth]
                                           ,[HomePhoneCode]
                                           ,[HomePhoneNumber]
                                           ,[WorkPhoneCode]
                                           ,[WorkPhoneNumber]
                                           ,[CellPhoneNumber]
                                           ,[EmailAddress]
                                           ,[FaxCode]
                                           ,[FaxNumber]
                                           ,[Password]
                                           ,[CitizenTypeKey]
                                           ,[LegalEntityStatusKey]
                                           ,[Comments]
                                           ,[LegalEntityExceptionStatusKey]
                                           ,[UserID]
                                           ,[ChangeDate]
                                           ,[EducationKey]
                                           ,[HomeLanguageKey]
                                           ,[DocumentLanguageKey]
                                           ,[ResidenceStatusKey])
                                     VALUES
                                           ({0},{1},{2},{3},'{4}',{5},'{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}',
                                            '{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}',{26},{27},'{28}',{29},
                                            '{30}','{31}',{32},{33},{34},{35})
                                    select top 01 * from [2AM].[dbo].[LegalEntity]
                                    order by 1 desc
                                COMMIT TRAN",
                                    Helpers.IfZeroReturnNULL((int)legalentity.LegalEntityTypeKey),
                                    Helpers.IfZeroReturnNULL((int)legalentity.MaritalStatusKey),
                                    Helpers.IfZeroReturnNULL((int)legalentity.GenderKey),
                                    Helpers.IfZeroReturnNULL((int)legalentity.PopulationGroupKey),
                                    legalentity.IntroductionDate.Value.ToString(Formats.DateTimeFormatSQL),
                                    legalentity.SalutationKey.HasValue ? Helpers.IfZeroReturnNULL((int)legalentity.SalutationKey) : "NULL",
                                    legalentity.FirstNames,
                                    legalentity.Initials,
                                    legalentity.Surname,
                                    legalentity.PreferredName,
                                    legalentity.IdNumber,
                                    legalentity.PassportNumber,
                                    legalentity.TaxNumber,
                                    legalentity.RegistrationNumber,
                                    legalentity.RegisteredName,
                                    legalentity.TradingName,
                                    legalentity.DateOfBirth.Value.ToString(Formats.DateTimeFormatSQL),
                                    legalentity.HomePhoneCode,
                                    legalentity.HomePhoneNumber,
                                    legalentity.WorkPhoneCode,
                                    legalentity.WorkPhoneNumber,
                                    legalentity.CellPhoneNumber,
                                    legalentity.EmailAddress,
                                    legalentity.FaxCode,
                                    legalentity.FaxNumber,
                                    legalentity.Password,
                                    Helpers.IfZeroReturnNULL((int)legalentity.CitizenTypeKey),
                                    Helpers.IfZeroReturnNULL((int)legalentity.LegalEntityStatusKey),
                                    legalentity.Comments,
                                    Helpers.IfZeroReturnNULL((int)legalentity.LegalEntityExceptionStatusKey),
                                    legalentity.UserID,
                                    legalentity.ChangeDate.Value.ToString(Formats.DateTimeFormatSQL),
                                    Helpers.IfZeroReturnNULL((int)legalentity.EducationKey),
                                    Helpers.IfZeroReturnNULL((int)legalentity.HomeLanguageKey),
                                    Helpers.IfZeroReturnNULL((int)legalentity.DocumentLanguageKey),
                                    Helpers.IfZeroReturnNULL((int)legalentity.ResidenceStatusKey));
            return dataContext.Query<Automation.DataModels.LegalEntity>(query).FirstOrDefault();
        }

        public Automation.DataModels.SubsidyProvider InsertSubsidyProvider(Automation.DataModels.SubsidyProvider subsidyProvider)
        {
            var query = String.Format(@"
                                            BEGIN TRAN
                                                INSERT INTO [2AM].[dbo].[SubsidyProvider]
                                                       ([SubsidyProviderTypeKey]
                                                       ,[PersalOrganisationCode]
                                                       ,[ContactPerson]
                                                       ,[UserID]
                                                       ,[ChangeDate]
                                                       ,[LegalEntityKey])
                                                 VALUES
                                                       (
                                                            {0},NULL,'{1}',NULL,NULL,{2}
                                                       )
                                                 SELECT TOP 01 sp.*, Description as SubsidyProviderTypeDescription FROM [2AM].[dbo].[SubsidyProvider] sp
                                                 JOIN SubsidyProviderType spt on spt.SubsidyProviderTypeKey = sp.SubsidyProviderTypeKey
                                                 ORDER BY 1 DESC
                                            COMMIT TRAN
                                        ", (int)subsidyProvider.SubsidyProviderTypeKey,
                                        subsidyProvider.ContactPerson,
                                        subsidyProvider.LegalEntity.LegalEntityKey);
            return dataContext.Query<Automation.DataModels.SubsidyProvider>(query).FirstOrDefault();
        }

        public Automation.DataModels.LegalEntityAddress InsertLegalEntityAddress(Automation.DataModels.LegalEntityAddress legalEntityAddress)
        {
            var p = new DynamicParameters();
            p.Add("@legalentitykey", value: legalEntityAddress.LegalEntity.LegalEntityKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@addresskey", value: legalEntityAddress.AddressKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@AddressTypeKey", value: (int)legalEntityAddress.AddressTypeKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@effectiveDate", value: DateTime.Now.ToString(Formats.DateTimeFormatSQL), dbType: DbType.DateTime, direction: ParameterDirection.Input);
            p.Add("@GeneralStatusKey", value: (int)legalEntityAddress.GeneralStatusKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            dataContext.Execute("test.InsertLegalEntityAddress", parameters: p, commandtype: CommandType.StoredProcedure);
            return this.GetLegalEntityAddresses(legalEntityAddress.LegalEntity.LegalEntityKey).Where(x => x.AddressKey == legalEntityAddress.AddressKey).FirstOrDefault();
        }

        public void DeleteSubsidyProvider(int legalentitykey)
        {
            var query = String.Format("delete from dbo.SubsidyProvider where legalentitykey = {0}", legalentitykey);
            dataContext.Execute(query);
        }

        public void DeleteLegalEntityAddress(int legalentitykey)
        {
            var query = String.Format("delete from dbo.legalentityaddress where legalentitykey = {0}", legalentitykey);
            dataContext.Execute(query);
        }

        public Automation.DataModels.LegalEntity GetMainApplicantLegalEntityWithoutPersonalLoanOffer(LegalEntityTypeEnum legalEntityType)
        {
            var query = String.Format(@"
                                select top 01 le.* from dbo.legalentity as le
                                    inner join [2am].dbo.role as r on le.legalentitykey = r.legalentitykey
                                        and r.roletypekey = 2
                                    inner join [2am].dbo.account as a  on r.accountkey = a.accountkey
                                        and a.accountstatuskey = 1
                                    left join [2am].dbo.externalrole as er on le.legalentitykey = er.legalentitykey
                                        and er.generickeytypekey = 2
                                        and er.externalRoleTypeKey = 1
                                    left join [2am].dbo.offer as o on er.generickey = o.offerkey
                                        and o.offertypekey = 11
                                    left join [2am].debtcounselling.debtcounselling dc on a.accountkey = dc.accountkey
                                        and dc.debtcounsellingstatuskey = 1
                                where er.externalrolekey is null and o.offerkey is null and dc.accountkey is null
                                    and le.legalentitytypekey = {0}", (int)legalEntityType);
            return dataContext.Query<Automation.DataModels.LegalEntity>(query).FirstOrDefault();
        }

        public Automation.DataModels.LegalEntity GetMainApplicantLegalEntityWithPersonalLoanOffer(int offerkey)
        {
            var query = String.Format(@"
                                select top 01 le.* from [2am].dbo.legalentity as le
                                    inner join [2am].dbo.role as r
                                        on le.legalentitykey = r.legalentitykey
                                        and r.roletypekey = 2
                                    inner join [2am].dbo.account as a
                                        on r.accountkey = a.accountkey
                                        and a.accountstatuskey = 1
                                    left join [2am].dbo.externalrole as er
                                        on le.legalentitykey = er.legalentitykey
                                        and er.generickeytypekey = 2
                                        and er.externalRoleTypeKey = 1
                                    left join [2am].dbo.offer as o
                                        on er.generickey = o.offerkey
                                        where o.offerkey={0}", offerkey);
            return dataContext.Query<Automation.DataModels.LegalEntity>(query).FirstOrDefault();
        }

        public void DeleteITC(int legalEntityKey)
        {
            var query = String.Format(@"delete from [2AM].[dbo].[ITC] where legalentitykey = {0}", legalEntityKey);
            dataContext.Execute(query);
        }

        public void CreateLegalEntityAffordabilities(int genericKey)
        {
            var p = new DynamicParameters();
            p.Add("@offerKey", value: genericKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            dataContext.Execute("test.insertLegalEntityAffordability", parameters: p, commandtype: CommandType.StoredProcedure);
        }

        public void CreateApplicationAffordabilities(int genericKey, int affordabilityAssessmentStatus)
        {
            var p = new DynamicParameters();
            p.Add("@offerKey", value: genericKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@affordabilityAssessmentStatusKey", value: affordabilityAssessmentStatus, dbType: DbType.Int32, direction: ParameterDirection.Input);
            dataContext.Execute("test.InsertAffordabilityAssessment", parameters: p, commandtype: CommandType.StoredProcedure);
        }

        public void DeleteLegalEntityAffordabilities(int legalEntityKey, bool isExpense)
        {
            var isExp = 0;
            if (isExpense)
                isExp = 1;
            var query = String.Format(@"
                declare @affordabilityTypes table
                (
                    affordabilitytypekey int
                )
               insert into @affordabilityTypes
               select at.AffordabilityTypeKey
               from [2AM].[dbo].[LegalEntityAffordability] as lea
                    inner join [2AM].[dbo].[AffordabilityType] as at
                        on lea.AffordabilityTypeKey = at.AffordabilityTypeKey
               where lea.legalentitykey = {0} and isexpense = {1}

               delete from [2AM].[dbo].[LegalEntityAffordability]
               where AffordabilityTypeKey in (select affordabilitytypekey from @affordabilityTypes)
               and legalentitykey = {0}", legalEntityKey, isExp);
            dataContext.Execute(query);
        }

        public void DeleteLegalEntitySubsidy(int legalEntityKey)
        {
            var query = String.Format(@"delete from [2am].dbo.subsidy where legalentitykey = 0", legalEntityKey);
            dataContext.Execute(query);
        }

        public void UpdateITC(int legalEntityKey, DateTime changeDate)
        {
            var query = String.Format(@"update [2am].dbo.itc set changedate = '{0}' where legalentitykey = {1}", changeDate.ToString(Formats.DateTimeFormatSQL), legalEntityKey);
            dataContext.Execute(query);
        }

        /// <summary>
        /// Gets a related mortgage loan account for an offer via the external role table.
        /// </summary>
        /// <param name="offerKey"></param>
        /// <returns></returns>
        public int GetRelatedMortgageLoanAccountKeyByOfferKey(int offerKey)
        {
            string query = String.Format(@"select a.* from [2am].dbo.externalrole er
                        join [2am].dbo.role r on er.legalentitykey=r.legalentitykey
                        join [2am].dbo.account a on r.accountkey = a.accountkey
                            and a.rrr_productkey not in (3,4,10,12,13)
                            and a.accountStatusKey = 1
                        where er.generickey={0}
                        and er.generickeytypekey=2
                        and er.generalstatuskey=1
                        ", offerKey);
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLQuery(statement);
            return (from r in results select r.Column("AccountKey").GetValueAs<int>()).FirstOrDefault();
        }

        public void UpdateLegalEntityInitials(int leKey, string initials)
        {
            var query = String.Format(@"update [2am].dbo.legalentity set initials = '{0}' where legalentitykey = {1}", initials, leKey);
            dataContext.Execute(query);
        }

        public Automation.DataModels.LegalEntityDomicilium CreateLegalEntityDomicilium(int legalEntityKey, GeneralStatusEnum status)
        {
            var random = new Random();
            //remove existing domicilium
            this.DeleteLegalEntityDomiciliumAddress(legalEntityKey);
            var address = this.InsertAddress(new Automation.DataModels.Address
            {
                StreetNumber = random.Next(0, 999).ToString(),
                StreetName = string.Format("{0} Street", random.Next(0, 999)),
                RRR_SuburbDescription = "La Lucia Ridge",
                RRR_ProvinceDescription = Common.Constants.Province.Kwazulunatal,
                UnitNumber = string.Format("{0} Unit", random.Next(0, 100)),
                BuildingName = string.Format("{0} Building Name", random.Next(0, 999)),
                BuildingNumber = string.Format("{0}", random.Next(0, 999)),
                UserID = @"SAHL\ClintonS",
                ChangeDate = DateTime.Now.ToString(Common.Constants.Formats.DateTimeFormatSQL),
                AddressFormatKey = AddressFormatEnum.Street
            });
            var legalEntityAddress = new Automation.DataModels.LegalEntityAddress
            {
                AddressKey = address.AddressKey,
                LegalEntityKey = legalEntityKey,
                LegalEntity = new Automation.DataModels.LegalEntity() { LegalEntityKey = legalEntityKey },
                GeneralStatusKey = GeneralStatusEnum.Active,
                AddressTypeKey = AddressTypeEnum.Residential
            };
            var lea = this.InsertLegalEntityAddress(legalEntityAddress);
            return this.InsertLegalEntityDomiciliumAddress(lea.LegalEntityAddressKey, legalEntityKey, status);
        }

        public void UpdateEmailAddress(int legalEntityKey, string emailAddress)
        {
            var query = String.Format(@"update [2am].dbo.legalentity set emailAddress = '{0}' where legalentitykey = {1}", emailAddress, legalEntityKey);
            dataContext.Execute(query);
        }

        public void UpdateCellphone(int legalEntityKey, string cellphone)
        {
            var query = String.Format(@"update [2am].dbo.legalentity set cellphonenumber = '{0}' where legalentitykey = {1}", cellphone, legalEntityKey);
            dataContext.Execute(query);
        }

        public Automation.DataModels.LegalEntity GetClientWithExistingPassword()
        {
            string query = @"select top 1 le.LegalEntityKey, le.EmailAddress, ltrim(rtrim(le.Password)) as Password from [2am].dbo.Account a
                    join [2am].dbo.Role r on a.accountKey = r.accountKey
                        and r.generalStatusKey = 1
                    join [2am].dbo.legalEntity le on r.legalEntityKey = le.legalEntityKey
                        and le.password is not null
                        and len(le.password) > 0
                    left join [2am].dbo.legalEntityLogin lel on le.legalEntityKey = lel.legalEntityKey
                    where lel.legalEntityKey is null and a.RRR_ProductKey in (1,9) and a.accountStatusKey = 1
                    order by newid()";
            return dataContext.Query<Automation.DataModels.LegalEntity>(query).First();
        }

        public Automation.DataModels.LegalEntity GetClientWithAccessToSecureWebsite()
        {
            string query = @"select top 1 le.* from [2am].dbo.Account a
                    join [2am].dbo.Role r on a.accountKey = r.accountKey
                        and r.generalStatusKey = 1
                    join [2am].dbo.legalEntity le on r.legalEntityKey = le.legalEntityKey
                        and le.password=''
                    join [2am].dbo.legalEntityLogin lel on le.legalEntityKey = lel.legalEntityKey
                    where a.RRR_ProductKey in (1,9) and a.accountStatusKey = 1
                    order by newid()";
            return dataContext.Query<Automation.DataModels.LegalEntity>(query).First();
        }

        public Automation.DataModels.LegalEntity GetClientWhoHasNeverRegisteredForSecureWebsite()
        {
            string query = @"select top 1 le.* from [2am].dbo.Account a
                    join [2am].dbo.Role r on a.accountKey = r.accountKey
                        and r.generalStatusKey = 1
                    join [2am].dbo.legalEntity le on r.legalEntityKey = le.legalEntityKey
                        and le.password is null
                    left join [2am].dbo.legalEntityLogin lel on le.legalEntityKey = lel.legalEntityKey
                    where lel.legalEntityKey is null and a.RRR_ProductKey in (1,9) and a.accountStatusKey = 1
                    order by newid()";
            return dataContext.Query<Automation.DataModels.LegalEntity>(query).First();
        }

        public IEnumerable<Automation.DataModels.LegalEntityLogin> GetClientSecureWebsiteLogin(int legalEntityKey)
        {
            string query =
                string.Format(@"select * from [2AM].dbo.legalEntityLogin where legalEntityKey={0}", legalEntityKey);
            return dataContext.Query<Automation.DataModels.LegalEntityLogin>(query);
        }

        public IEnumerable<Automation.DataModels.LegalEntityReturningDiscountQualifyingData> GetLegalEntityReturningDiscountQualifyingData()
        {
            string query = @"SELECT TOP 100 le.LegalEntityKey,
                    [2AM].[dbo].[LegalEntityLegalName] (le.LegalEntityKey, 0) as LegalEntityLegalName,
                    le.IDNumber,
                    SUM(CASE WHEN a.AccountKey IS NOT NULL AND r.RoleTypeKey = 2 AND a.AccountStatusKey = 1 THEN 1 ELSE 0 END) AS MainApplicantOpenAccountsCount,
                    SUM(CASE WHEN a.AccountKey IS NOT NULL AND r.RoleTypeKey = 2 AND a.AccountStatusKey = 2 THEN 1 ELSE 0 END) AS MainApplicantClosedAccountsCount,
                    SUM(CASE WHEN a.AccountKey IS NOT NULL AND r.RoleTypeKey = 1 AND a.AccountStatusKey = 1 THEN 1 ELSE 0 END) AS SuretorOpenAccountsCount,
                    SUM(CASE WHEN a.AccountKey IS NOT NULL AND r.RoleTypeKey = 1 AND a.AccountStatusKey = 2 THEN 1 ELSE 0 END) AS SuretorClosedAccountsCount,
                    SUM(CASE WHEN ft.FinancialTransactionKey IS NOT NULL AND r.RoleTypeKey = 2 AND a.AccountStatusKey = 1 THEN 1 ELSE 0 END) AS ReturningClientDiscountOpenAccountsCount,
                    SUM(CASE WHEN ft.FinancialTransactionKey IS NOT NULL AND r.RoleTypeKey = 2 AND a.AccountStatusKey = 2 THEN 1 ELSE 0 END) AS ReturningClientDiscountClosedAccountsCount
                    FROM [2AM].dbo.LegalEntity le (NOLOCK)
	                    JOIN [2AM].dbo.Role r (NOLOCK) ON r.LegalEntityKey = le.LegalEntityKey
						    AND r.RoleTypeKey IN (1,2)
	                    JOIN [2AM].dbo.Account a (NOLOCK) ON a.AccountKey = r.AccountKey 
                            AND a.RRR_ProductKey in (1,2,9,11) --Variable Loan, Varifix Loan, New Variable Loan, Edge
                            AND (a.AccountStatusKey = 1 or (a.AccountStatusKey = 2 and a.CloseDate > DATEADD(yy, -1, GetDate()-1)))
	                    JOIN [2AM].dbo.FinancialService fs (NOLOCK) ON a.AccountKey = fs.AccountKey and fs.FinancialServiceTypeKey = 1
					    LEFT JOIN [2AM].fin.FinancialTransaction ft (NOLOCK) ON fs.FinancialServiceKey = ft.FinancialServiceKey AND ft.TransactionTypeKey = 1442
                    GROUP BY le.LegalEntityKey, le.IDNumber";
            return dataContext.Query<Automation.DataModels.LegalEntityReturningDiscountQualifyingData>(query);
        }

        public void UpdateLegalEntityContactDetails(int legalEntityKey, LegalEntityContactDetails newContactDetails)
        {
            string query = string.Format(@"
                            Update [2am].dbo.LegalEntity
                            set HomePhoneCode='{0}', HomePhoneNumber='{1}', WorkPhoneCode='{2}', WorkPhoneNumber='{3}', FaxCode='{4}', FaxNumber = '{5}',
                            EmailAddress = '{6}', CellphoneNumber = '{7}' where legalEntityKey = {8}",
                            newContactDetails.HomePhoneNumber.Code,
                            newContactDetails.HomePhoneNumber.Number,
                            newContactDetails.WorkPhoneNumber.Code,
                            newContactDetails.WorkPhoneNumber.Number,
                            newContactDetails.FaxNumber.Code,
                            newContactDetails.FaxNumber.Number,
                            newContactDetails.EmailAddress,
                            newContactDetails.CellphoneNumber,
                            legalEntityKey);
            dataContext.Execute(query);
        }

        public void UpdateDocumentLanguage(int legalEntityKey, LanguageEnum language)
        {
            string query = string.Format("Update [2am].dbo.LegalEntity set DocumentLanguageKey = {0} where LegalEntityKey = {1}", (int)language, legalEntityKey);
            dataContext.Execute(query);
        }

        public void UpdateHomeLanguage(int legalEntityKey, LanguageEnum language)
        {
            string query = string.Format("Update [2am].dbo.LegalEntity set HomeLanguageKey = {0} where LegalEntityKey = {1}", (int)language, legalEntityKey);
            dataContext.Execute(query);
        }

        public void UpdateEducationLevel(int legalEntityKey, EducationEnum educationLevel)
        {
            string query = string.Format("Update [2am].dbo.LegalEntity set EducationKey = {0} where LegalEntityKey = {1}", (int)educationLevel, legalEntityKey);
            dataContext.Execute(query);
        }

        public void UpdatePreferredName(int legalEntityKey, string name)
        {
            string query = string.Format("Update [2am].dbo.LegalEntity set PreferredName = '{0}' where LegalEntityKey = {1}", name, legalEntityKey);
            dataContext.Execute(query);
        }

        public void UpdateLegalEntitySalutationToNull(int legalEntityKey)
        {
            string query = string.Format("UPDATE [2AM].[dbo].LegalEntity SET SalutationKey = NULL WHERE LegalEntityKey = {0}", legalEntityKey);
            dataContext.Execute(query);
        }

        public void UpdateLegalEntityInitialsToNull(int legalEntityKey)
        {
            string query = string.Format("UPDATE [2AM].[dbo].LegalEntity SET Initials = NULL WHERE LegalEntityKey = {0}", legalEntityKey);
            dataContext.Execute(query);
        }

        public void UpdateLegalEntityGenderToNull(int legalEntityKey)
        {
            string query = string.Format("UPDATE [2AM].[dbo].LegalEntity SET GenderKey = NULL WHERE LegalEntityKey = {0}", legalEntityKey);
            dataContext.Execute(query);
        }

        public void UpdateLegalEntityMaritalStatusToNull(int legalEntityKey)
        {
            string query = string.Format("UPDATE [2AM].[dbo].LegalEntity SET MaritalStatusKey = NULL WHERE LegalEntityKey = {0}", legalEntityKey);
            dataContext.Execute(query);
        }

        public void UpdateLegalEntityCitizenTypeToEmpty(int legalEntityKey)
        {
            string query = string.Format("UPDATE [2AM].[dbo].LegalEntity SET CitizenTypeKey = NULL WHERE LegalEntityKey = {0}", legalEntityKey);
            dataContext.Execute(query);
        }

        public void UpdateLegalEntityDateOfBirthToEmpty(int legalEntityKey)
        {
            string query = string.Format("UPDATE [2AM].[dbo].LegalEntity SET DateOfBirth = NULL WHERE LegalEntityKey = {0}", legalEntityKey);
            dataContext.Execute(query);
        }
        public IEnumerable<ITC> GetITCs(int expectedLegalEntityKey)
        {
            var query = @"select * from dbo.ITC where legalentitykey = {0}";
            query = String.Format(query, expectedLegalEntityKey);
            return dataContext.Query<ITC>(query);
        }
        public IEnumerable<LegalEntity> GetDirtyNaturalPeopleOnAccounts(GeneralStatusEnum accountStatus)
        {
            var query = String.Format(@"select top 01 * from [2AM].dbo.legalentity le (nolock)
                                    join [2AM].dbo.role r
                                        on le.LegalEntityKey=r.legalentitykey
                                        and r.GeneralStatusKey = 1
                                where le.LegalEntityKey not in
                                (
                                    select le.LegalEntityKey from [2AM].dbo.legalentity le (nolock)
                                        join [2am].dbo.role r (nolock)
                                            on le.LegalEntityKey=r.LegalEntityKey
                                        join [2AM].dbo.Account acc (nolock)
                                            on  r.AccountKey= acc.AccountKey
                                    where acc.AccountStatusKey = {0} and r.GeneralStatusKey = 1
                                ) 
                                and le.LegalEntityKey not in
                                (
                                    select distinct ofr.LegalEntityKey from [2AM].dbo.offer o
                                        join [2AM].dbo.offerrole ofr
                                            on ofr.offerkey = o.offerkey 
                                    where o.offerstatuskey = 1 and ofr.LegalEntityKey = le.LegalEntityKey
                                )
                                and 
                                (
                                    FirstNames  = '' 
                                    or FirstNames  is null
                                ) 
                                and 
                                (
                                    le.LegalEntityTypeKey = 2 
                                    and EmailAddress not like '%sahomeloans%' 
                                    and Surname != '' and Surname  is not null 
                                    and DateOfBirth  != '' and DateOfBirth  is not null 
                                    and Salutationkey  != '' or Salutationkey  is not null
                                )

                            union
                                select top 01 * from [2AM].dbo.legalentity le (nolock)
                                    join [2AM].dbo.role r
                                        on le.LegalEntityKey=r.legalentitykey
                                        and r.GeneralStatusKey = 1
                                where le.LegalEntityKey not in
                                (
                                    select le.LegalEntityKey from [2AM].dbo.legalentity le (nolock)
                                        join [2am].dbo.role r (nolock)
                                            on le.LegalEntityKey=r.LegalEntityKey
                                        join [2AM].dbo.Account acc (nolock)
                                            on  r.AccountKey= acc.AccountKey
                                    where acc.AccountStatusKey = {0} and r.GeneralStatusKey = 1
                                ) 
                                and le.LegalEntityKey not in
                                (
                                    select distinct ofr.LegalEntityKey from [2AM].dbo.offer o
                                        join [2AM].dbo.offerrole ofr
                                            on ofr.offerkey = o.offerkey 
                                    where o.offerstatuskey = 1 and ofr.LegalEntityKey = le.LegalEntityKey
                                )
                                and 
                                (
                                    Surname  = '' 
                                    or Surname  is null
                                ) 
                                and 
                                (
                                    le.LegalEntityTypeKey = 2 
                                    and EmailAddress not like '%sahomeloans%' 
                                    and FirstNames != '' and FirstNames  is not null 
                                    and DateOfBirth  != '' and DateOfBirth  is not null 
                                    and Salutationkey  != '' or Salutationkey  is not null
                                )
                            union
                                select top 01 * from [2AM].dbo.legalentity le (nolock)
                                    join [2AM].dbo.role r
                                        on le.LegalEntityKey=r.legalentitykey
                                        and r.GeneralStatusKey = 1
                                where le.LegalEntityKey not in
                                (
                                    select le.LegalEntityKey from [2AM].dbo.legalentity le (nolock)
                                        join [2am].dbo.role r (nolock)
                                            on le.LegalEntityKey=r.LegalEntityKey
                                        join [2AM].dbo.Account acc (nolock)
                                            on  r.AccountKey= acc.AccountKey
                                    where acc.AccountStatusKey = {0} and r.GeneralStatusKey = 1
                                ) 
                                and le.LegalEntityKey not in
                                (
                                    select distinct ofr.LegalEntityKey from [2AM].dbo.offer o
                                        join [2AM].dbo.offerrole ofr
                                            on ofr.offerkey = o.offerkey 
                                    where o.offerstatuskey = 1 and ofr.LegalEntityKey = le.LegalEntityKey
                                )
                                and 
                                (
                                    DateOfBirth  = '' 
                                    or DateOfBirth  is null
                                ) 
                                and 
                                (
                                    le.LegalEntityTypeKey = 2 
                                    and EmailAddress not like '%sahomeloans%' 
                                    and FirstNames != '' and FirstNames  is not null 
                                    and Surname  != '' and Surname  is not null 
                                    and Salutationkey  != '' or Salutationkey  is not null
                                )
                            union
                                select top 01 * from [2AM].dbo.legalentity le (nolock)
                                    join [2AM].dbo.role r
                                        on le.LegalEntityKey=r.legalentitykey
                                        and r.GeneralStatusKey = 1
                                where le.LegalEntityKey not in
                                (
                                    select le.LegalEntityKey from [2AM].dbo.legalentity le (nolock)
                                        join [2am].dbo.role r (nolock)
                                            on le.LegalEntityKey=r.LegalEntityKey
                                        join [2AM].dbo.Account acc (nolock)
                                            on  r.AccountKey= acc.AccountKey
                                    where acc.AccountStatusKey = {0} and r.GeneralStatusKey = 1
                                ) 
                                and le.LegalEntityKey not in
                                (
                                    select distinct ofr.LegalEntityKey from [2AM].dbo.offer o
                                        join [2AM].dbo.offerrole ofr
                                            on ofr.offerkey = o.offerkey 
                                    where o.offerstatuskey = 1 and ofr.LegalEntityKey = le.LegalEntityKey
                                )
                                and 
                                (
                                    Salutationkey  = '' 
                                    or Salutationkey  is null
                                ) 
                                and 
                                (
                                    le.LegalEntityTypeKey = 2 
                                    and EmailAddress not like '%sahomeloans%' 
                                    and FirstNames != '' and FirstNames  is not null 
                                    and Surname  != '' and Surname  is not null 
                                    and DateOfBirth  != '' or DateOfBirth  is not null
                                )", (int)accountStatus);
            return dataContext.Query<LegalEntity>(query);
        }
    }
}