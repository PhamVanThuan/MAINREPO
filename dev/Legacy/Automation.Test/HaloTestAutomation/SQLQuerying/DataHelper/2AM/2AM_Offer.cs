using Common.Enums;
using Common.Extensions;
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
        ///
        /// </summary>
        /// <param name="offerkey"></param>
        /// <param name="offerRoleType"></param>
        /// <returns></returns>
        public QueryResults GetActiveOfferRolesByOfferRoleType(int offerkey, OfferRoleTypeEnum offerRoleType)
        {
            string query = string.Empty;
            SQLStatement statement = new SQLStatement();

            query = string.Format(@"Select o.*, a.ADUserKey, a.ADUserName, a.GeneralStatusKey as ADUserStatus, o.GeneralStatusKey as OfferRoleStatus, uos.generalStatusKey as UOSStatus
                from [2am].dbo.OfferRole o with (nolock)
				    left join ADUser a with (nolock) on o.LegalEntityKey = a.LegalEntityKey and o.GeneralStatusKey = 1
				    left join offerRoleTypeOrganisationStructureMapping map
				        join userOrganisationStructure uos on map.organisationstructurekey=uos.organisationstructurekey
				    on o.offerRoleTypeKey=map.offerRoleTypeKey and a.adUserKey = uos.adUserKey
				where o.OfferKey = {0}
                    and o.OfferRoleTypeKey = {1}
				    and o.GeneralStatusKey = 1", offerkey, (int)offerRoleType);

            statement.StatementString = query;

            return dataContext.ExecuteSQLQuery(statement);
        }

        public IEnumerable<Automation.DataModels.OfferRole> GetOfferRolesByOfferKey(int offerkey)
        {
            var query = string.Format(@"select ofr.*,ort.*
                                            from [2am].dbo.OfferRole ofr with (nolock)
	                                            join [2am].dbo.OfferRoleType as ort
		                                            on ofr.offerroletypekey=ort.offerroletypekey
                                            where ofr.OfferKey = {0}", offerkey);
            return dataContext.Query<Automation.DataModels.OfferRole>(query);
        }

        public IEnumerable<Automation.DataModels.ExternalRole> GetExternalRolesByOfferKey(int offerkey)
        {
            var query = string.Format(@"SELECT
	                                            er.*,
	                                            ert.*
                                            FROM [2AM].dbo.ExternalRole er WITH (NOLOCK)
	                                            INNER JOIN [2AM].dbo.ExternalRoleType ert ON ert.ExternalRoleTypeKey = er.ExternalRoleTypeKey
                                            WHERE er.GenericKey = {0}", offerkey);
            return dataContext.Query<Automation.DataModels.ExternalRole>(query);
        }

        public IEnumerable<Automation.DataModels.OfferRole> GetOfferRolesByLegalEntityKey(int legalentityKey)
        {
            var query = string.Format(@"select ofr.*,ort.*
                                            from [2am].dbo.OfferRole ofr with (nolock)
	                                            join [2am].dbo.OfferRoleType as ort
		                                            on ofr.offerroletypekey=ort.offerroletypekey
                                            where ofr.legalentitykey = {0}", legalentityKey);
            return dataContext.Query<Automation.DataModels.OfferRole>(query);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="adUserName"></param>
        /// <returns></returns>
        public QueryResults GetLastOfferCreatedByADUser(string adUserName)
        {
            string query =
                string.Format(@"select
				max(OfferRole.OfferKey) as [OfferKey]
				from dbo.LegalEntity with (nolock)
				inner join dbo.OfferRole  with (nolock)
				on LegalEntity.LegalEntityKey = OfferRole.LegalEntityKey
				inner join dbo.ADUser with (nolock)
				on OfferRole.LegalEntityKey = ADUser.LegalEntityKey
				where ADUser.ADUsername like '%{0}%'", adUserName);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// returns the Offer record when provided with an OfferKey
        /// </summary>
        /// <param name="offerKey"></param>
        /// <returns></returns>
        public QueryResults GetOfferData(int offerKey)
        {
            string query = string.Format(@"select o.*, ot.description as OfferType
                                from [2am].dbo.Offer o (nolock)
								join [2am].dbo.OfferType ot (nolock) on ot.OfferTypeKey = o.OfferTypeKey
								where OfferKey = {0}", offerKey);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Retrieves a record from the OfferInformationFinancialAdjustment table when provided with an OfferKey and a FinancialAdjustmentTypeSource
        /// </summary>
        /// <param name = "offerKey">OfferKey</param>
        /// <param name = "financialAdjustmentTypeSource">RateOverrideTypeKey</param>
        /// <returns></returns>
        public QueryResults GetOfferFinancialAdjustmentsByType(int offerKey, FinancialAdjustmentTypeSourceEnum financialAdjustmentTypeSource)
        {
            string query =
                string.Format(
                    @"select oifa.* from offer o (nolock)
                        join (select max(offerinformationkey) as oikey, offerkey
                        from [2am].dbo.OfferInformation (nolock) group by OfferKey)
                        as maxoi on o.offerkey=maxoi.offerkey
                        join offerinformation oi (nolock)
                        on maxoi.oikey=oi.offerinformationkey
                        join offerinformationFinancialAdjustment oifa (nolock)
                        on oi.offerinformationkey=oifa.offerinformationkey
                        where o.offerkey= {0}
                        and oifa.FinancialAdjustmentTypeSourceKey = {1}",
                    offerKey, (int)financialAdjustmentTypeSource);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        public IEnumerable<Automation.DataModels.OfferExpense> GetOfferExpenses(int offerkey)
        {
            return dataContext.Query<Automation.DataModels.OfferExpense>(String.Format(@"select * from [2am].dbo.offerexpense where offerkey = {0}", offerkey));
        }

        /// <summary>
        ///   Retrieves all of the OfferInformation records for an Offer
        /// </summary>
        /// <param name = "offerKey"></param>
        /// <returns>oi.*</returns>
        public QueryResults GetOfferInformationRecordsByOfferKey(int offerKey)
        {
            string query = string.Format(@"select * from [2am].dbo.OfferInformation oi (nolock) where oi.offerkey={0}", offerKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Returns the further lending offer key to write back to the database table
        /// </summary>
        /// <param name = "accountKey">Mortgage Loan Account Key</param>
        /// <param name = "offerStatus">Status of the Further Lending Offer</param>
        /// <returns>Offer.OfferKey, Offer.OfferTypeKey, Offer.OfferStatusKey</returns>
        public QueryResults GetOffersByAccountKeyAndStatus(string accountKey, OfferStatusEnum offerStatus)
        {
            string query =
                string.Format(@"SELECT OfferKey, OfferTypeKey, OfferStatusKey
                    FROM [2am].dbo.Offer o (nolock)
				  WHERE o.AccountKey = {0} and OfferStatusKey = {1}", accountKey, (int)offerStatus);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Gets the conditions that have been saved against an Offer.
        /// </summary>
        /// <param name = "offerKey">OfferKey</param>
        /// <returns>Condition.*</returns>
        public QueryResults GetOfferConditions(int offerKey)
        {
            string query =
                string.Format(@"select c.* from [2am].dbo.offercondition oc with (nolock)
						   join [2am].dbo.condition c on oc.conditionkey=c.conditionkey
						   where offerkey={0}", offerKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Gets any Offer Roles that do not exist on the Account
        /// </summary>
        /// <param name = "offerKey">OfferKey</param>
        /// <returns>ofr.LegalEntityKey,ofr.OfferRoleTypeKey,rt.RoleTypeKey,o.AccountKey </returns>
        public QueryResults GetClientOfferRolesNotOnAccount(int offerKey)
        {
            SQLStatement statement = new SQLStatement();
            string query =
                String.Format(
                    @"select distinct ofr.LegalEntityKey,ofr.OfferRoleTypeKey,rt.RoleTypeKey,o.AccountKey
							from [2am].[dbo].offer o (nolock)
							inner join [2am].[dbo].offerRole ofr (nolock)
							on ofr.offerKey = o.offerKey
							inner join [2am].[dbo].offerRoleType ort (nolock)
							on ofr.OfferRoleTypeKey = ort.OfferRoleTypeKey
							inner join [2am].[dbo].RoleType rt (nolock)
							on ort.description = rt.description
							left join [2am].[dbo].role r (nolock)
							on ofr.legalEntityKey = r.legalEntityKey
							and o.accountKey = r.accountKey
							and rt.RoleTypeKey = r.RoleTypeKey
							where ofr.offerKey = {0}
							and
							ort.OfferRoleTypeGroupKey = 3
							and r.LegalEntityKey is null",
                    offerKey);
            statement.StatementString = query;
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Will return the number of offerinformation records for the specified offerkey
        /// </summary>
        /// <returns></returns>
        public int GetOfferInformationRecordCount(int offerKey)
        {
            SQLStatement statement = new SQLStatement();
            string query = string.Format(@"select count(*) from [2am].dbo.offerinformation
						   where offerinformation.offerkey = {0}", offerKey);
            statement.StatementString = query;
            QueryResults results = dataContext.ExecuteSQLScalar(statement);
            return int.Parse(results.SQLScalarValue);
        }

        /// <summary>
        /// Get OfferInformationVariableLoan records by OfferKey ordered by OfferInformationKey descending
        /// </summary>
        /// <returns>OfferKey, OfferInformationKey, CategoryKey, Term, ExistingLoan, CashDeposit, PropertyValuation, HouseholdIncome, FeesTotal,
        /// InterimInterest, MonthlyInstalment, LifePremium, HOCPremium, MinLoanRequired, MinBondRequired, PreApprovedAmount,
        /// MinCashAllowed, MaxCashAllowed, LoanAmountNoFees, RequestedCashAmount, LoanAgreementAmount, BondToRegister, LTV,
        /// PTI, MarketRate, SPVKey, EmploymentTypeKey, RateConfigurationKey, CreditMatrixKey, CreditCriteriaKey</returns>
        public QueryResults GetOfferInformationVariableLoanRecordsByOfferKeyOrderedDesc(int offerKey)
        {
            string query = string.Format(@"SELECT  o.*, oi.*,  oivl.OfferInformationKey, oivl.CategoryKey, oivl.Term, oivl.ExistingLoan,
	                                    oivl.CashDeposit,  oivl.PropertyValuation,  oivl.HouseholdIncome, oivl.FeesTotal,
	                                    oivl.InterimInterest, oivl.MonthlyInstalment, oivl.LifePremium, oivl.HOCPremium,
	                                    oivl.MinLoanRequired, oivl.MinBondRequired, oivl.PreApprovedAmount, oivl.MinCashAllowed,
	                                    oivl.MaxCashAllowed, oivl.LoanAmountNoFees, oivl.RequestedCashAmount, oivl.LoanAgreementAmount,
	                                    oivl.BondToRegister, oivl.LTV, oivl.PTI,oivl.MarketRate,oivl.SPVKey, oivl.EmploymentTypeKey,
	                                    oivl.RateConfigurationKey,oivl.CreditMatrixKey, oivl.CreditCriteriaKey
                                        FROM [2am].dbo.Offer o with (nolock)
                                        INNER JOIN [2am].dbo.OfferInformation oi (nolock) ON o.OfferKey = oi.OfferKey
                                        INNER JOIN [2am].dbo.OfferInformationVariableLoan as oivl with (nolock) ON oi.OfferInformationKey = oivl.OfferInformationKey
                                        WHERE   (o.OfferKey = {0})
                                        ORDER BY oi.OfferInformationKey DESC", offerKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Retrieves the latest offer information record for an offer and an indication of whether or not OfferInformationRateOverride
        ///   or OfferInformationVarifixLoan records exist for the offer.
        /// </summary>
        /// <param name = "offerKey">OfferKey</param>
        /// <returns>OfferInformation.*,OfferInformationVarifixLoan.OfferInformationKey,OfferInformationRateOverride.OfferInformationKey</returns>
        public QueryResults GetLatestOfferInformationByOfferKey(int offerKey)
        {
            string query =
                string.Format(@"select oi.*, isnull(oiv.offerinformationkey,0) as VariFixOI, isnull(oifa.offerinformationkey,0) as EdgeOI,
                p.description as ProductDescription, oivl.CategoryKey, oivl.Term, oivl.ExistingLoan, oivl.CashDeposit, oivl.PropertyValuation, oivl.HouseholdIncome, oivl.FeesTotal, oivl.InterimInterest, oivl.MonthlyInstalment, oivl.LifePremium,
                oivl.HOCPremium, oivl.MinLoanRequired, oivl.MinBondRequired, oivl.PreApprovedAmount, oivl.MinCashAllowed, oivl.MaxCashAllowed, oivl.LoanAmountNoFees, oivl.RequestedCashAmount, oivl.LoanAgreementAmount, oivl.BondToRegister,
                oivl.LTV, oivl.PTI, oivl.MarketRate, oivl.SPVKey, oivl.EmploymentTypeKey, oivl.RateConfigurationKey, oivl.CreditMatrixKey, oivl.CreditCriteriaKey
                from [2am].dbo.offer o (nolock)
                join (
                select max(offerinformationkey) oikey, offerkey
                from [2am].dbo.offerinformation oi (nolock) where oi.offerkey = {0}
                group by oi.offerkey
                ) as maxoi on o.offerkey=maxoi.offerkey
                join [2am].dbo.offerinformation oi (nolock) on maxoi.oikey=oi.offerinformationkey
                left join [2am].dbo.offerinformationvariableloan oivl (nolock) on oi.offerinformationkey =oivl.offerinformationkey
                left join [2am].dbo.offerinformationFinancialAdjustment oifa (nolock) on oi.offerinformationkey = oifa.offerinformationkey
                left join [2am].dbo.offerinformationvarifixloan oiv (nolock) on oi.offerinformationkey =oiv.offerinformationkey
                left join [2am].dbo.product p (nolock) on oi.productkey=p.productkey
				where o.offerkey = {1}", offerKey, offerKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Inserts Declarations for all Natural Person, Main Applicant or Suretor LegalEntity's linked to a new business offer or personal loan offer
        /// </summary>
        /// <param name = "offerKey">OfferKey</param>
        /// <param name = "genericKeyTypeKey">OfferRoleType or ExternalRoleType</param>
        /// <param name = "originationSourceProductKey">OriginationSourceProductKey</param>
        public void InsertDeclarations(int offerKey, GenericKeyTypeEnum genericKeyTypeKey, OriginationSourceProductEnum originationSourceProductKey)
        {
            var param = new DynamicParameters();
            param.Add("@OfferKey", value: offerKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@GenericKeyTypeKey", value: (int)genericKeyTypeKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@OriginationSourceProductKey", value: (int)originationSourceProductKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            dataContext.Execute("[2am].[test].[InsertDeclarations]", parameters: param, commandtype: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Cehcks if a legal entity plays an inactive/active role against
        /// </summary>
        /// <param name="legalEntityKey">LegalEntityKey</param>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="generalStatus">GeneralStatusKey</param>
        /// <returns>TRUE = exists, FALSE = does not exist</returns>
        public bool OfferRoleExists(int legalEntityKey, int offerKey, GeneralStatusEnum generalStatus)
        {
            string query = String.Format(@"SELECT Count(LegalEntityKey)
					FROM [2am].[dbo].OfferRole ofr with (nolock)
					WHERE ofr.LegalEntityKey = {0}
					AND ofr.OfferKey = {1}
					AND ofr.GeneralStatusKey = {2}", legalEntityKey, offerKey, (int)generalStatus);

            SQLStatement statement = new SQLStatement { StatementString = query };
            int count = Convert.ToInt32(dataContext.ExecuteSQLScalar(statement).SQLScalarValue);
            if (count > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Inserts into the OfferRole table
        /// </summary>
        /// <param name="legalEntityKey">LegalEntityKey</param>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="offerRoleType">OfferRoleTypeKey</param>
        /// <param name="generalStatus">GeneralStatusKey</param>
        /// <returns>bool</returns>
        public bool CreateOfferRole(int legalEntityKey, int offerKey, OfferRoleTypeEnum offerRoleType, GeneralStatusEnum generalStatus)
        {
            string query = String.Format(@"INSERT INTO [2am].[dbo].OfferRole
					  (LegalEntityKey, OfferKey, OfferRoleTypeKey, GeneralStatusKey, StatusChangeDate)
					VALUES ({0},{1},{2},{3},GetDate())", legalEntityKey, offerKey, (int)offerRoleType, (int)generalStatus);
            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
            return true;
        }

        /// <summary>
        /// Deletes all the offer role records for a legal entity against an offer
        /// </summary>
        /// <param name="legalEntityKey">LegalEntitykey</param>
        /// <param name="offerKey">OfferKey</param>
        public void DeleteOfferRole(Int32 legalEntityKey, int offerKey)
        {
            string query = string.Format(@"Delete from [2am].[dbo].OfferRole where LegalEntitykey = {0} and OfferKey = {1}", legalEntityKey, offerKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Gets a bank account that is not currently being used as a legal entity bank account
        /// </summary>
        /// <returns>bankAccountKey</returns>
        public int GetUnusedBankAccountKey()
        {
            string query = string.Format(@"
            select top 1 ba.BankAccountKey from [2AM].dbo.BankAccount ba
            left join [2AM].dbo.LegalEntityBankAccount leb on ba.BankAccountKey=leb.BankAccountKey
            where leb.LegalEntityBankAccountKey is null and ba.ACBTypeNumber=1 and ba.ACBBranchCode > 0
            order by newid()");
            SQLStatement statement = new SQLStatement { StatementString = query };
            int bankAccountKey = Convert.ToInt32(dataContext.ExecuteSQLScalar(statement).SQLScalarValue);
            return bankAccountKey;
        }

        /// <summary>
        /// Inserts a Legal Entity Bank Account record
        /// </summary>
        /// <param name="legalEntityKey">LegalEntityKey</param>
        /// <param name="bankAccountKey">BankAccountKey</param>
        /// <returns>LegalEntityBankAccountKey</returns>
        public Automation.DataModels.LegalEntityBankAccount InsertLegalEntityBankAccount(int legalEntityKey, int bankAccountKey)
        {
            string query = string.Format(@"INSERT INTO [2AM].[dbo].[LegalEntityBankAccount]
		   ([LegalEntityKey]
		   ,[BankAccountKey]
		   ,[GeneralStatusKey]
		   ,[UserID]
		   ,[ChangeDate])
			VALUES
			({0},{1},{2},'System',getdate())

			select Scope_Identity();", legalEntityKey, bankAccountKey, 1);

            SQLStatement statement = new SQLStatement { StatementString = query };
            int legalBankAccountKey = Convert.ToInt32(dataContext.ExecuteSQLScalar(statement).SQLScalarValue);
            return new Automation.DataModels.LegalEntityBankAccount
            {
                LegalEntityBankAccountKey = legalBankAccountKey,
                BankAccountKey = bankAccountKey,
                LegalEntityKey = legalEntityKey
            };
        }

        /// <summary>
        /// Inserts an OfferDebitOrder record
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="bankAccountKey">BankAccountKey</param>
        /// <returns>OfferDebitOrderKey</returns>
        public int InsertOfferDebitOrder(int offerKey, int bankAccountKey)
        {
            string query = string.Format(@"
			DELETE FROM [2AM].[dbo].[OfferDebitOrder] WHERE OfferKey = {0}

			INSERT INTO [2AM].[dbo].[OfferDebitOrder]
			   ([OfferKey]
			   ,[BankAccountKey]
			   ,[Percentage]
			   ,[DebitOrderDay]
			   ,[FinancialServicePaymentTypeKey])
			VALUES
				({0},{1},0,25,1)

			select Scope_Identity();", offerKey, bankAccountKey);

            SQLStatement statement = new SQLStatement { StatementString = query };
            int offerDebitOrderKey = Convert.ToInt32(dataContext.ExecuteSQLScalar(statement).SQLScalarValue);
            return offerDebitOrderKey;
        }

        /// <summary>
        /// Deletes a record from the LegalEntityBankAccount table
        /// </summary>
        /// <param name="legalEntitybankAccountKey">LegalEntityBankAccountKey</param>
        public void DeleteLegalEntityBankAccount(int legalEntitybankAccountKey)
        {
            string query = string.Format(@"DELETE FROM [2AM].[dbo].[LegalEntityBankAccount]
			WHERE LegalEntityBankAccountKey = {0}", legalEntitybankAccountKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Deletes a record from OfferDebitOrder table.
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        public void DeleteOfferDebitOrder(int offerKey)
        {
            string query = string.Format(@"DELETE FROM [2AM].[dbo].[OfferDebitOrder] WHERE OfferKey = {0}", offerKey);
            var statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Add an Income Contributor
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <param name="offerKey"></param>
        public void AddIncomeContributor(int legalEntityKey, int offerKey)
        {
            string query = String.Format(@"declare @OfferRoleKey int
										   select
											  @OfferRoleKey = OfferRoleKey
										   from
											  [2am].[dbo].OfferRole
										   where
											  OfferRole.OfferKey = {0} and
											  OfferRole.OfferRoleTypeKey = 8 and
											  OfferRole.LegalEntityKey = {1}

											if( not exists(select OfferRoleKey from [2am].[dbo].OfferRoleAttribute where OfferRoleKey = @OfferRoleKey))
											begin
												insert into [2am].[dbo].OfferRoleAttribute values(@OfferRoleKey, 1)
											end", offerKey, legalEntityKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets an OfferMortgageLoan record when provided with an offerKey
        /// </summary>
        /// <param name="offerKey">offerKey</param>
        /// <returns></returns>
        public QueryResults GetOfferMortgageLoanByOfferKey(int offerKey)
        {
            string query = String.Format("select * from [2AM].[dbo].[OfferMortgageLoan] (nolock) where OfferKey={0}", offerKey);

            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Finds an application capture case with two applicants
        /// </summary>
        /// <returns>offerKey</returns>
        public int GetApplicationCaptureOfferWith2Applicants()
        {
            string query = @"select top 1 o.OfferKey
							from [x2].[x2data].Application_Capture ac (nolock)
							join [x2].[x2].Instance i (nolock) on i.id = ac.InstanceID
							join [x2].[x2].State s (nolock) on s.id = i.StateID
							join [2am]..Offer o (nolock) on o.OfferKey = ac.ApplicationKey
							join [2am]..OfferRole ofr (nolock) on ofr.OfferKey = o.OfferKey
							join [2am]..LegalEntity le (nolock) on ofr.legalEntityKey=le.legalEntityKey and le.legalEntityTypeKey = 2
							join [2am]..OfferRoleType ort (nolock) on ort.OfferRoleTypeKey = ofr.OfferRoleTypeKey
							where ac.CaseOwnerName is not null and s.Name = 'Application Capture' and o.OfferStatusKey = 1 and ort.OfferRoleTypeGroupKey = 3
							group by o.OfferKey
							having count(ofr.OfferRoleKey) = 2 order by o.OfferKey desc";
            SQLStatement statement = new SQLStatement { StatementString = query };
            int offerKey = int.Parse(dataContext.ExecuteSQLScalar(statement).SQLScalarValue);
            return offerKey;
        }

        /// <summary>
        /// Finds an application capture case with two applicants
        /// </summary>
        /// <returns>offerKey</returns>
        public int GetApplicationCaptureOfferWith2ApplicantsWhereLegalEntitiesHaveSalutationAndInitials()
        {
            string query = @"select top 1 o.OfferKey
                            from [x2].[x2data].Application_Capture ac (nolock)
                            join [x2].[x2].Instance i (nolock) on i.id = ac.InstanceID
                            join [x2].[x2].State s (nolock) on s.id = i.StateID
                            join [2am]..Offer o (nolock) on o.OfferKey = ac.ApplicationKey
                            join [2am]..OfferRole ofr (nolock) on ofr.OfferKey = o.OfferKey
                            join [2am]..LegalEntity le (nolock) on ofr.legalEntityKey=le.legalEntityKey and le.legalEntityTypeKey = 2 
                            join [2am]..OfferRoleType ort (nolock) on ort.OfferRoleTypeKey = ofr.OfferRoleTypeKey
                            where ac.CaseOwnerName is not null and s.Name = 'Application Capture' and o.OfferStatusKey = 1 and ort.OfferRoleTypeGroupKey = 3 and le.Salutationkey is not null and le.Initials is not null and le.Salutationkey <> '' and le.Initials <> ''
                            group by o.OfferKey
                            having count(ofr.OfferRoleKey) = 2 order by o.OfferKey desc";
            SQLStatement statement = new SQLStatement { StatementString = query };
            int offerKey = int.Parse(dataContext.ExecuteSQLScalar(statement).SQLScalarValue);
            return offerKey;
        }

        /// <summary>
        /// Gets a further lending offer that is linked to a property that has a lightstone property ID.
        /// </summary>
        /// <param name="lightstoneValOlderThan2Months">TRUE = , FALSE =</param>
        /// <returns>OfferKey</returns>
        public int GetFurtherLendingOfferWithLightstonePropertyID(bool lightstoneValOlderThan2Months)
        {
            string join = string.Empty;
            string where = string.Empty;
            //then we need to find a further lending offer that has not had a lighstone valuation in the last 2 months
            if (lightstoneValOlderThan2Months)
            {
                join = @" left join [2am].dbo.valuation v on p.propertykey = v.propertyKey and ValuationDataProviderDataServiceKey = 3 ";
                where = "  and v.propertyKey is null";
            }
            else
            {
                join = @" join [2am].dbo.valuation v on p.propertykey = v.propertyKey and ValuationDataProviderDataServiceKey = 3
							and datediff(mm, valuationdate,getdate()) <= 2";
            }
            string query =
                string.Format(@"select o.offerKey from [2am].dbo.offer o
								join [2am].dbo.offerMortgageLoan oml on
								o.offerKey=oml.offerKey
								join [2am].dbo.property p on oml.propertyKey=p.propertyKey
								join [2am].dbo.propertyData pd on p.propertyKey=pd.propertyKey and propertyDataProviderDataServiceKey=1
								join x2.x2data.readvance_payments data on o.offerkey=data.applicationkey
								join x2.x2.instance i on data.instanceid=i.id
								join x2.x2.state s on i.stateid=s.id
								{0}
								where o.offerTypeKey in (2,3,4)
								and o.offerStatusKey=1
								{1}
								and s.name in (
								select distinct s.name from x2.x2.Activity a
								join x2.x2.state s on a.StateID=s.ID
								where a.name = 'Request Lightstone Valuation'
								and a.workflowID in (select max(id) from x2.x2.workflow where name = 'readvance payments')
								)
								", join, where);
            SQLStatement statement = new SQLStatement { StatementString = query };
            QueryResults r = dataContext.ExecuteSQLQuery(statement);
            if (!r.HasResults)
            {
                return 0;
            }
            return r.Rows(0).Column("offerKey").GetValueAs<int>();
        }

        /// <summary>
        /// Gets an open new business offer.
        /// </summary>
        /// <returns></returns>
        public QueryResults GetOpenApplicationCaptureOffer()
        {
            const string query =
                @"select top 1 o.offerKey, le.legalEntityKey, propertyKey, idNumber,
                    dbo.legalentitylegalname(le.legalentitykey,0) as legalentitylegalname, o.reservedAccountKey
                    from [2am].dbo.offer o with (nolock)
                    join x2.x2data.Application_Capture ac with (nolock)
                    on o.offerkey = ac.applicationKey
                    join x2.x2.instance i on ac.instanceid = i.id
                    join x2.x2.state s on i.stateid = s.id
	                    and s.name = 'Application Capture'
                    join [2am].dbo.offerMortgageLoan oml with (nolock)
                    on o.offerKey=oml.offerKey
                    join [2am].dbo.offerRole ofr with (nolock)
                    on o.offerKey=ofr.offerKey and offerRoleTypeKey in (8,10,11,12)
                    join [2am].dbo.legalEntity le with (nolock)
                    on ofr.legalentitykey=le.legalentitykey and len(le.idnumber) = 13
                    where o.offerStatusKey=1 and oml.propertykey is not null
                    and o.offerTypeKey in (6,7,8)
                    order by newid() desc";
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// This method will query 2am for an offerkey that has no mailing address and where the at least one legalentity that play a role
        /// on the application have an address
        /// </summary>
        /// <param name="noOffers">number of rows to return i.e. select top 10 </param>
        /// <returns>list of offerkeys</returns>
        public List<int> GetOfferWithoutMailingAddress(int noOffers)
        {
            string query = String.Format(@"select distinct top {0} offer.offerkey
                                from [2am].dbo.offer offer
                                inner join x2.x2data.application_capture ac on offer.offerKey=ac.applicationKey
                                inner join x2.x2.instance i on ac.instanceid=i.id
                                inner join x2.x2.state s on i.stateid=s.id
                                    and s.name <> 'InvalidAppHold'
                                inner join [2am].dbo.offerrole  on offer.offerkey = offerrole.offerkey
                                    and offerrole.OfferRoleTypeKey in (8,10)
                                inner join (select legalentityKey from [2am].dbo.legalentity where emailaddress <> '' and emailaddress is not null) as legalentity
                                on offerrole.legalentitykey = legalentity.legalentitykey
                                inner join [2am].dbo.legalentityaddress on offerrole.legalentitykey = legalentityaddress.legalentitykey
                                inner join [2am].dbo.address on legalentityaddress.addresskey = address.addresskey
                                    and address.addressformatkey = 1
                                left join [2am].dbo.offermailingaddress on offer.offerkey =offermailingaddress.offerkey
                                where offermailingaddress.offerkey is null
                                and offer.offerstatuskey = 1
                                and originationsourcekey = 1
                                and offer.offerstartdate > (getdate()-365)
                                and offer.offertypekey in (6,7,8)
                                and legalentityaddress.generalstatuskey = 1
                                order by offer.offerKey desc", noOffers);
            SQLStatement statement = new SQLStatement { StatementString = query };
            QueryResults results = dataContext.ExecuteSQLQuery(statement);
            return results.GetColumnValueList<int>("offerkey");
        }

        /// <summary>
        /// Get the mailing address that is saved against the offer.
        /// </summary>
        /// <param name="offerKey">offerkey from 2am.dbo.offer table</param>
        public Automation.DataModels.OfferMailingAddress GetOfferMailingAddress(int offerKey)
        {
            string query =
                String.Format(@"select
                                a.addresskey as [AddressKey],
                                oma.legalentitykey as [LegalEntityKey],
                                oma.onlinestatement as [OnlineStatement],
                                l.description as [Language],
	                            cm.description as [CorrespondenceMedium],
                                osf.description as [OnlineStatementFormat],
                                dbo.fGetFormattedAddress(a.addressKey) as [FormattedAddress],
                                dbo.fGetFormattedAddressDelimited(a.addressKey,0) as [FormattedAddressDelimited]
                                from [2am].dbo.offermailingaddress oma with (nolock)
	                            inner join [2am].dbo.address a with (nolock) on a.addresskey = oma.addresskey
                                inner join [2am].dbo.language l  with (nolock) on oma.languagekey = l.languagekey
	                            inner join [2am].dbo.onlinestatementformat osf with (nolock) on oma.onlinestatementformatkey = osf.onlinestatementformatkey
	                            inner join [2am].dbo.correspondencemedium cm with (nolock) on oma.correspondencemediumkey = cm.correspondencemediumkey
                                where oma.offerkey={0}", offerKey);
            return dataContext.Query<Automation.DataModels.OfferMailingAddress>(query).FirstOrDefault();
        }

        /// <summary>
        /// Get lastest random Offer, OfferInformation, OfferInformationVariable. OfferInformationVarifix and OfferMortgageLoan record.
        /// </summary>
        /// <returns></returns>
        public QueryResultsRow GetRandomLatestOfferInformationMortgageLoanRecord(MortgageLoanPurposeEnum purpose, ProductEnum product, OfferStatusEnum status)
        {
            var whereClause = String.Empty;
            switch (purpose)
            {
                case MortgageLoanPurposeEnum.Newpurchase:
                    {
                        whereClause =
                               String.Format(@"where oml.mortgageloanpurposekey = {0}
													and oml.clientestimatepropertyvaluation > 0
                                                    and oivl.marketrate != 0
                                                    and cashdeposit > 0
                                                    and productkey = {1}
                                                    and oml.purchaseprice != 0
                                                    and o.offerstatuskey = {2}", (int)purpose, (int)product, (int)status);
                        break;
                    }
                case MortgageLoanPurposeEnum.Switchloan:
                    {
                        whereClause =
                              String.Format(@"where oml.mortgageloanpurposekey = {0}
													and oml.clientestimatepropertyvaluation > 0
                                                    and requestedcashamount > 0
                                                    and oivl.marketrate != 0
                                                    and productkey = {1}
                                                    and o.offerstatuskey = {2}", (int)purpose, (int)product, (int)status);
                        break;
                    }
            }
            var query = String.Format(@"select top 01 * from [2am].dbo.offer o with (nolock)
                                                inner join [2am].dbo.OfferInformation oi (nolock)
                                                    on o.OfferKey = oi.OfferKey
                                                inner join [2am].dbo.offerInformationvariableloan as oivl with (nolock)
                                                    on oi.OfferInformationKey = oivl.OfferInformationKey
                                                inner join dbo.OfferMortgageLoan as oml
		                                            on o.offerkey = oml.offerkey
		                                        left join dbo.offerInformationvarifixloan as oivfl
													on  oi.OfferInformationKey = oivfl.OfferInformationKey
                                                {0}
                                           order by oi.OfferInformationKey desc", whereClause);
            SQLStatement statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0);
        }

        /// <summary>
        /// Get random Offer provided the parameters
        /// </summary>
        /// <returns></returns>
        public QueryResultsRow GetRandomOfferRecord(ProductEnum product, OfferTypeEnum offerType, OfferStatusEnum status)
        {
            var query = String.Format(@"select top 1 o.*,oi.productkey, o.reservedAccountKey
                                        from [2am].dbo.offer o with (nolock)
                                        inner join [2am].dbo.OfferInformation oi (nolock) on o.OfferKey = oi.OfferKey
                                        where oi.productkey = {0} and o.offertypekey = {1} and o.offerstatuskey={2}
                                        order by newid()", (int)product, (int)offerType, (int)status);
            var statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLQuery(statement);
            return results.Rows(0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stateName"></param>
        /// <param name="adUserName"></param>
        public QueryResults GetOfferKeysAtStateByAdUser(string stateName, string adUserName)
        {
            try
            {
                var db = new _2AMDataHelper();
                var sql =
                    string.Format(@"SELECT i.Name, o.offerTypeKey, o.reservedAccountKey
                                FROM x2.x2.instance i
                                join x2.x2.state s on i.stateid=s.id
                                join x2.x2.worklist w on i.id=w.instanceid
                                join [2am]..Offer o on i.Name=o.offerKey
                                WHERE s.name = '{0}'
                                AND w.adusername='{1}'", stateName, adUserName);
                var statement = new SQLStatement { StatementString = sql };
                return dataContext.ExecuteSQLQuery(statement);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all the application declarations against the offer legal entity.
        /// </summary>
        /// <param name="offerKey"></param>
        /// <param name="legalEntityKey"></param>
        /// <returns></returns>
        public QueryResults GetApplicationDeclarations(int offerKey, int legalEntityKey)
        {
            string sql =
                string.Format(@"select d.*, ans.description from [2am]..offerRole r (nolock)
                                        join [2am]..OfferDeclaration d (nolock)
                                        on r.offerRoleKey=d.offerRoleKey
                                        join [2am]..OfferDeclarationAnswer ans (nolock) on d.OfferDeclarationAnswerKey=ans.OfferDeclarationAnswerKey
                                        where r.offerKey = {0} and r.legalentityKey = {1}", offerKey, legalEntityKey);
            var statement = new SQLStatement { StatementString = sql };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Get all the external application declarations against the offer legal entity.
        /// </summary>
        /// <param name="offerKey"></param>
        /// <param name="legalEntityKey"></param>
        /// <returns></returns>
        public QueryResults GetExternalApplicationDeclarations(int offerKey, int legalEntityKey)
        {
            string sql =
                string.Format(@"SELECT
	                                erd.*,
	                                oda.Description
                                FROM [2AM].dbo.ExternalRole er (NOLOCK)
	                                INNER JOIN [2AM].dbo.ExternalRoleDeclaration erd (NOLOCK) ON er.ExternalRoleKey = erd.ExternalRoleKey
	                                INNER JOIN dbo.OfferDeclarationAnswer oda (NOLOCK) ON oda.OfferDeclarationAnswerKey = erd.OfferDeclarationAnswerKey
                                WHERE er.GenericKey = {0}
                                AND er.LegalEntityKey = {1}", offerKey, legalEntityKey);
            var statement = new SQLStatement { StatementString = sql };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Get Offer By State and FinancialAdjustmentSourceType
        /// </summary>
        /// <param name="numberOfRecords"></param>
        /// <param name="workflowState"></param>
        /// <param name="financialAdjustmentSourceType"></param>
        /// <param name="minLTV"></param>
        /// <param name="maxLTV"></param>
        /// <returns></returns>
        public QueryResults GetOfferByStateAndfinancialAdjustmentSourceType(int numberOfRecords, string workflowState,
            FinancialAdjustmentTypeSourceEnum financialAdjustmentSourceType, int minLTV, int maxLTV)
        {
            var sql =
                    String.Format(@"select top {0}
										xam.ApplicationKey,
										xs.Name
									from x2.x2data.Application_Management xam
                                    join x2.x2.instance xi on xam.instanceID = xi.ID
                                    join x2.x2.state xs on xi.StateID = xs.ID
                                    join x2.x2.worklist xwl on xwl.InstanceID = xi.ID
                                    join [2am].[dbo].OfferInformation oi on	oi.OfferKey = xam.ApplicationKey
                                    join [2am].[dbo].OfferInformationFinancialAdjustment oifa on
											oifa.OfferInformationKey = oi.OfferInformationKey and
											oifa.FinancialAdjustmentTypeSourceKey = {1}
                                    join [2am].[dbo].OfferInformationVariableLoan oivl on oivl.OfferInformationKey = oi.OfferInformationKey
									where xs.Name = '{2}' and oivl.LTV between {3} and {4}",
                                         numberOfRecords, (int)financialAdjustmentSourceType, workflowState, minLTV, maxLTV);
            var statement = new SQLStatement { StatementString = sql };
            return dataContext.ExecuteSQLQuery(statement);
        }

        ///<summary>
        ///</summary>
        ///<param name="records"></param>
        ///<param name="state"></param>
        ///<param name="offerTypeKeys"></param>
        ///<param name="adUsername"></param>
        ///<returns></returns>
        public QueryResults GetOfferByStateAndAdUserForDuplicatePropertyRuleTest(int records, string state, string offerTypeKeys, string adUsername)
        {
            string sql =
                String.Format(@"
				    select top {0} xam.ApplicationKey,xwl.adusername
				    from x2.X2DATA.Application_Management xam (nolock)
				    join x2.X2.Instance xi (nolock)
					    on xi.ID = xam.InstanceID
				    join x2.X2.WorkList xwl (nolock)
					    on xwl.InstanceID = xi.ID
				    join x2.x2.state xs (nolock)
					    on xs.ID = xi.StateID
				    join [2am].[dbo].offer o (nolock)
					    on o.OfferKey = xam.ApplicationKey
				    join [2am].[dbo].offermortgageloan oml (nolock)
					    on oml.offerkey=o.offerkey
				    join [2am].[dbo].stagetransitioncomposite stc (nolock)
					    on stc.generickey=o.offerkey and stc.stagedefinitionstagedefinitiongroupkey in (1694)
				    left join [2am].dbo.Account acc (nolock)
					    on acc.accountKey = o.reservedAccountKey and acc.accountStatusKey in (1,2,5)
				    where  xs.name = '{1}' and  o.OfferTypeKey in ({2}) and o.offerstatuskey in (1,3) and acc.accountKey is null", records, state, offerTypeKeys);
            var statement = new SQLStatement { StatementString = sql };
            return dataContext.ExecuteSQLQuery(statement);
        }

        ///<summary>
        ///</summary>
        ///<param name="records"></param>
        ///<param name="state"></param>
        ///<param name="offerTypeKeys"></param>
        ///<param name="adUsername"></param>
        ///<returns></returns>
        public QueryResults GetTestOfferByStateAndAdUserForDuplicatePropertyRuleTest(int records, string state, string offerTypeKeys, string adUsername)
        {
            string sql = String.Format(@"
				                        select top {0} xam.ApplicationKey,xwl.adusername,ofr.OfferRoleKey
				                        from x2.X2DATA.Application_Management xam (nolock)
				                        join x2.X2.Instance xi (nolock) on xi.ID = xam.InstanceID
				                        join x2.X2.WorkList xwl (nolock) on xwl.InstanceID = xi.ID
				                        join x2.x2.state xs (nolock)  on xs.ID = xi.StateID
				                        join [2am].[dbo].offer o (nolock) on o.OfferKey = xam.ApplicationKey
				                        join [2am].[dbo].offermortgageloan oml (nolock) on oml.offerkey=o.offerkey
				                        left join [2am].[dbo].stagetransitioncomposite stc (nolock)
					                    on stc.generickey=o.offerkey and stc.stagedefinitionstagedefinitiongroupkey in (2132)
				                        left join [2am].[dbo].OfferRole ofr (nolock) on o.OfferKey = ofr.OfferKey and ofr.OfferRoleTypeKey = 4 and
					                    ofr.GeneralStatusKey = 1
				                        where xs.name = '{1}' and o.OfferTypeKey in ({2}) and o.offerstatuskey in (1,3)
                                        order by newid()", records, state, offerTypeKeys);
            var statement = new SQLStatement { StatementString = sql };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Returns a count of the new roles that have been added to a further lending application
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="offerType">OfferType</param>
        /// <returns></returns>
        public int NewRolesCount(int offerKey, OfferTypeEnum offerType)
        {
            string sql = string.Format(@"select count(ofr.legalentityKey) as countLE
                                        from [2am].dbo.offer o
                                        join [2am].dbo.offerRole ofr on o.offerKey=ofr.offerKey
                                        left join [2am].dbo.role a on ofr.legalentitykey=a.legalentitykey
                                        where offertypekey= {0}
                                        and offerStatusKey=1 and ofr.offerroleTypeKey in (8,10,11,12)
                                        and a.legalentitykey is null and o.offerkey= {1}", (int)offerType, offerKey);
            var statement = new SQLStatement { StatementString = sql };
            var r = dataContext.ExecuteSQLScalar(statement);
            return Int32.Parse(r.SQLScalarValue);
        }

        /// <summary>
        /// This will set the set the application start date to the supplied date.
        /// </summary>
        /// <param name="OfferStartDate">Application Start Date</param>
        /// <param name="OfferKey">Application Number</param>
        /// <returns>String</returns>
        public void UpdateOfferStartDate(DateTime offerStartDate, int offerKey)
        {
            var sql = String.Format(@"update [2AM].dbo.Offer set OfferStartDate='{0}' where OfferKey={1}", offerStartDate, offerKey);
            var statement = new SQLStatement { StatementString = sql };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Inserts Offer Mailing Address
        /// </summary>
        /// <param name="offerKey"></param>
        public void InsertOfferMailingAddress(int offerKey)
        {
            SQLStoredProcedure proc = new SQLStoredProcedure { Name = "test.InsertOfferMailingAddress" };
            proc.AddParameter(new SqlParameter("@offerKey", offerKey.ToString()));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// Populate minimum data to submit from AppCap
        /// </summary>
        /// <param name="offerKey"></param>
        public void CleanupNewBusinessOffer(int offerKey)
        {
            SQLStoredProcedure proc = new SQLStoredProcedure { Name = "test.CleanupNewBusinessOffer" };
            proc.AddParameter(new SqlParameter("@offerKey", offerKey.ToString()));
            dataContext.ExecuteStoredProcedure(proc);
        }

        /// <summary>
        /// Returns an offer debit order when provided with the offerKey
        /// </summary>
        /// <param name="offerKey"></param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.OfferDebitOrder> GetOfferDebitOrder(int offerKey)
        {
            string sql = string.Format(@"select * from [2am].dbo.OfferDebitOrder where offerKey = {0}", offerKey);
            return dataContext.Query<Automation.DataModels.OfferDebitOrder>(sql);
        }

        /// <summary>
        /// Gets the Offer Attribute Types
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.OfferAttributeType> GetOfferAttributeTypes()
        {
            string sql = "select * from [2am].dbo.OfferAttributeType";
            return dataContext.Query<Automation.DataModels.OfferAttributeType>(sql);
        }

        /// <summary>
        /// Get all the offers for the given legalentity
        /// </summary>
        /// <param name="legalEntityKey"></param>
        public IEnumerable<Automation.DataModels.Offer> GetOffersByExternalRoleLegalEntity(int legalEntityKey)
        {
            string sql = String.Format(@"select * from dbo.offer as o
	                                             inner join dbo.externalrole as er
		                                             on o.offerkey = er.generickey
		                                             and er.generickeytypekey = 2
                                             where er.legalentitykey = {0}", legalEntityKey);
            return dataContext.Query<Automation.DataModels.Offer>(sql);
        }

        /// <summary>
        /// Get OfferInformationVariableLoan records by OfferKey ordered by OfferInformationKey descending
        /// </summary>

        /// </returns>
        public QueryResults GetOfferInformationPersonalLoanRecordsByOfferKey(int offerKey)
        {
            string query = string.Format(@"SELECT oipl.loanamount, oipl.term, oipl.monthlyinstalment, oipl.lifepremium
                                        FROM OFFER o
                                        INNER JOIN [2am].dbo.OfferInformation oi (nolock)
                                        ON o.OfferKey = oi.OfferKey
                                        INNER JOIN [2am].dbo.OfferInformationPersonalLoan as oipl with (nolock)
                                        ON oi.OfferInformationKey = oipl.OfferInformationKey
                                        WHERE   (o.OfferKey = {0})", offerKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Get OfferExpense records by OfferKey
        /// </summary>

        /// </returns>
        public QueryResults GetOfferExpenseByOfferKey(int offerKey, string expectedexpenseType)
        {
            string query = string.Format(@"SELECT oe.expensetypekey, oe.TotalOutstandingAmount, et.description
                                        FROM offerexpense oe
                                        INNER JOIN expensetype et
                                        on oe.expensetypekey=et.expensetypekey
                                        WHERE   (oe.OfferKey = {0}) and (et.description = '{1}')", offerKey, expectedexpenseType);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Delete an offer debit order
        /// </summary>
        /// <param name="offerKey"></param>
        public void DeleteOfferDebitOrderByOffer(int offerKey)
        {
            string query = string.Format(@"
			DELETE FROM [2AM].[dbo].[OfferDebitOrder] WHERE OfferKey = {0}", offerKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Delete mailing address
        /// </summary>
        /// <param name="offerKey"></param>
        public void DeleteOfferMailingAddress(int offerKey)
        {
            string query = string.Format(@"
			DELETE FROM [2AM].[dbo].[OfferMailingAddress] WHERE OfferKey = {0}", offerKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Update Corresponding Medium to Email
        /// </summary>
        /// <param name="offerKey"></param>
        public void UpdateCorrespondenceMedium(int offerKey, CorrespondenceMediumEnum correspondenceMedium)
        {
            string query = string.Format(@"
			UPDATE [2AM].[dbo].[OfferMailingAddress] SET CORRESPONDENCEMEDIUMKEY={0} WHERE OfferKey = {1}", (int)correspondenceMedium, offerKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="offerKey"></param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.OfferAttribute> GetOfferAttributes(int offerKey)
        {
            var query = string.Format(@"select * from [2am].dbo.OfferAttribute where offerKey = {0}", offerKey);
            return dataContext.Query<Automation.DataModels.OfferAttribute>(query);
        }

        /// <summary>
        /// Returns a personal loan application
        /// </summary>
        /// <param name="offerKey"></param>
        /// <returns></returns>
        public Automation.DataModels.PersonalLoanApplication GetPersonalLoanApplication(int offerKey)
        {
            var personalLoanApplication =
                dataContext.Query<Automation.DataModels.PersonalLoanApplication>
                (string.Format(@"
                    select ofr.offerKey, offerStatusKey, OfferStartDate,
                    ReservedAccountKey, LoanAmount, term, MonthlyInstalment, LifePremium,
                    FeesTotal, mgn.Value AS LinkRate, mr.Value AS MarketRate,
                    mgn.value + mr.Value AS TotalRate, OfferInformationTypeKey,
                    CASE WHEN oa.OfferKey IS NULL THEN 0 ELSE 1 END AS CreditLifeTakenUp
                     from offer ofr
                     left join(select max(OfferInformationKey) as OfferInformationKey, offerKey
                    from offerInformation WHERE ProductKey = 12
                     group by offerKey) as oiKeys
                     on oiKeys.OfferKey = ofr.OfferKey
                     LEFT JOIN dbo.OfferInformation oi ON oi.OfferInformationKey = oiKeys.OfferInformationKey
                     LEFT JOIN OfferInformationPersonalLoan oivl on oivl.OfferInformationKey = oiKeys.OfferInformationKey
                     LEFT JOIN dbo.MarketRate mr ON oivl.MarketRateKey = mr.MarketRateKey
                     LEFT JOIN dbo.Margin mgn ON oivl.MarginKey = mgn.MarginKey
                     LEFT JOIN dbo.OfferAttribute oa ON ofr.OfferKey = oa.OfferKey
                     AND oa.OfferAttributeTypeKey = 12
                     where ofr.OfferKey = {0}", offerKey));
            return (from pl in personalLoanApplication select pl).FirstOrDefault();
        }

        public void UpdateOfferInformationMortgageLoan(int offerKey, float householdIncome, float loanAgreementAmount, float propertyValuation, float feesTotal,
            float cashDeposit, float instalment, float LTV, float PTI, float bondToRegister, float term, MarketRateEnum marketRateKey, int rateConfigurationKey,
            EmploymentTypeEnum employmentType, float purchasePrice)
        {
            var sql = String.Format(@"
                                          declare @offerInformationKey int
                                          select @offerInformationKey = max(offerinformationkey) from [2am].dbo.offerInformation
                                          where offerkey = {0}

                                          update [2am].dbo.offermortgageloan
                                          set purchaseprice = {14}
                                          where offerkey = {0}

                                          declare @marketRate float
                                          select @marketRate=value from dbo.marketrate
                                          where marketratekey = {1}

                                          update
                                            [2am].dbo.offerinformationvariableloan
                                          set
                                            HouseHoldIncome = {2},
                                            LoanAgreementAmount = {3},
                                            propertyValuation = {4},
                                            feesTotal= {5},
                                            cashdeposit={6},
                                            loanamountnofees={7},
                                            LTV = {8},
                                            PTI = {9},
                                            BondToRegister={10},
                                            Term = {11},
                                            MarketRate = @marketRate,
                                            rateConfigurationKey = {12},
                                            employmentTypekey = {13}
                                          where
                                            offerinformationkey = @offerInformationKey", offerKey, (int)marketRateKey, householdIncome, loanAgreementAmount,
                                            propertyValuation, feesTotal, cashDeposit, loanAgreementAmount, LTV, PTI, bondToRegister, term, rateConfigurationKey,
                                            (int)employmentType, purchasePrice);
            dataContext.Execute(sql);
        }

        public Automation.DataModels.LegalEntityDomicilium InsertOfferRoleDomicilium(int legalEntityDomiciliumKey, int offerrolekey, int offerKey)
        {
            var param = new DynamicParameters();
            param.Add("@legalEntityDomiciliumKey", value: legalEntityDomiciliumKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@offerrolekey", value: offerrolekey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            dataContext.Execute("[2am].[test].[InsertOfferRoleDomicilium]", parameters: param, commandtype: CommandType.StoredProcedure);
            return GetOfferRoleDomiciliums(offerKey).FirstOrDefault(x => x.OfferRoleKey == offerrolekey);
        }

        public Automation.DataModels.LegalEntityDomicilium InsertExternalRoleDomicilium(int legalEntityDomiciliumKey, int externalRoleKey, int offerKey)
        {
            var param = new DynamicParameters();
            param.Add("@LegalEntityDomiciliumKey", value: legalEntityDomiciliumKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@ExternalRoleKey", value: externalRoleKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            dataContext.Execute("[2am].[test].[InsertExternalRoleDomicilium]", parameters: param, commandtype: CommandType.StoredProcedure);
            return GetExternalRoleDomiciliums(offerKey).FirstOrDefault(x => x.OfferRoleKey == externalRoleKey);
        }

        public IEnumerable<Automation.DataModels.LegalEntityDomicilium> GetOfferRoleDomiciliums(int offerKey)
        {
            var sql = String.Format(@"select ord.OfferRoleKey, led.*,lea.legalentitykey, lea.addresskey, dbo.fGetFormattedAddress(lea.addressKey) as FormattedAddress,
                                            dbo.fGetFormattedAddressDelimited(lea.addressKey, 0) as DelimitedAddress, lea.LegalEntityAddressKey
                                            from [2am].dbo.offerrole ofr
	                                      join [2am].dbo.offerroledomicilium ord on ord.offerrolekey = ofr.offerrolekey
                                          join [2am].dbo.legalEntityDomicilium led on ord.legalEntityDomiciliumKey=led.legalEntityDomiciliumKey
                                          join [2am].dbo.LegalEntityAddress lea on led.LegalEntityAddressKey=lea.LegalEntityAddressKey
                                          where ofr.offerkey = {0}", offerKey);
            return dataContext.Query<Automation.DataModels.LegalEntityDomicilium>(sql);
        }

        public IEnumerable<Automation.DataModels.LegalEntityDomicilium> GetExternalRoleDomiciliums(int offerKey)
        {
            var sql = String.Format(@"SELECT
	                                        er.ExternalRoleKey,
	                                        led.*,
	                                        lea.LegalEntityKey,
	                                        lea.AddressKey,
	                                        dbo.fGetFormattedAddress(lea.AddressKey) as FormattedAddress,
	                                        dbo.fGetFormattedAddressDelimited(lea.AddressKey, 0) as DelimitedAddress,
                                            lea.LegalEntityAddressKey
                                        FROM [2am].dbo.ExternalRole er
	                                        INNER JOIN [2am].dbo.ExternalRoleDomicilium erd ON erd.ExternalRoleKey = er.ExternalRoleKey
	                                        INNER JOIN [2am].dbo.LegalEntityDomicilium led ON led.LegalEntityDomiciliumKey = erd.LegalEntityDomiciliumKey
	                                        INNER JOIN [2am].dbo.LegalEntityAddress lea ON lea.LegalEntityAddressKey = led.LegalEntityAddressKey
                                        WHERE er.GenericKey = {0}", offerKey);
            return dataContext.Query<Automation.DataModels.LegalEntityDomicilium>(sql);
        }

        public void CleanUpOfferDomicilium(int offerKey)
        {
            var offerRoles = this.GetOfferRolesByOfferKey(offerKey).Where(x => x.OfferRoleTypeGroupKey == OfferRoleTypeGroupEnum.Client
                                                                            && x.GeneralStatusKey == GeneralStatusEnum.Active
                                                                             && x.OfferRoleTypeKey != OfferRoleTypeEnum.AssuredLife);
            foreach (var role in offerRoles)
            {
                var domicilium = this.CreateLegalEntityDomicilium(role.LegalEntityKey, GeneralStatusEnum.Pending);
                this.InsertOfferRoleDomicilium(domicilium.LegalEntityDomiciliumKey, role.OfferRoleKey, offerKey);
            }
        }

        public void InsertExternalRoleDomicilium(int offerKey)
        {
            var externalRoles = this.GetExternalRolesByOfferKey(offerKey).Where(x => x.ExternalRoleTypeKey == ExternalRoleTypeEnum.Client
                                                                                    && x.GeneralStatusKey == GeneralStatusEnum.Active);
            foreach (var role in externalRoles)
            {
                var domicilium = this.CreateLegalEntityDomicilium(role.LegalEntityKey, GeneralStatusEnum.Pending);
                this.InsertExternalRoleDomicilium(domicilium.LegalEntityDomiciliumKey, role.ExternalRoleKey, offerKey);
            }
        }

        public void DeleteExternalRoleDeclarations(int offerKey)
        {
            string query = string.Format(@"delete erd
                                            from [2AM].dbo.externalroledeclaration erd
                                            join [2AM].dbo.externalrole er on erd.externalrolekey=er.externalrolekey
	                                            and er.generickeytypekey=2 and er.externalroletypekey=1
                                            where er.generickey={0}", offerKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        public int GetOfferByPropertyKey(int propertyKey)
        {
            string query = string.Format(@"select offerkey from dbo.offermortgageloan where propertykey = {0}", propertyKey);
            return Int32.Parse(dataContext.ExecuteSQLScalar(new SQLStatement { StatementString = query }).SQLScalarValue);
        }

        public void RemoveCreditLifePolicyFromPersonalLoanOffer(int offerKey)
        {
            string query = string.Format(@"Update oipl set lifepremium = 0
                                            from [2AM].dbo.offer o
                                            join [2AM].dbo.offerinformation oi on o.offerkey = oi.offerkey
                                            join [2AM].dbo.offerinformationpersonalloan oipl on oi.offerinformationkey = oipl.offerinformationkey
                                            where o.offerkey = {0}", offerKey);
            dataContext.Execute(query);
        }

        public void AddExternalLife(int offerKey, Automation.DataModels.ExternalLifePolicy externalLifePolicy)
        {
            string query = string.Format(@"declare @externallifepolicykey int
                                        declare @legalentitykey int

                                        select @legalentitykey = legalentitykey
                                        from [2am].dbo.externalrole
                                        where generickey = {7}
                                        and generickeytypekey = 2
                                        and externalroletypekey = 1

                                        update elp set lifepolicystatuskey = 12
                                        from [2am]..offer o
                                        join [2am]..offerexternallife eol on o.offerkey = eol.offerkey
                                        join [2am]..externallifepolicy elp on eol.externallifepolicykey = elp.externallifepolicykey
                                        where o.offerkey = {7}

                                        insert into [2am]..externallifepolicy
	                                    (insurerkey, policynumber, commencementdate, lifepolicystatuskey, closedate, suminsured, policyceded, legalentitykey)
	                                    values ({0}, {1}, {2}, {3}, {4}, {5}, {6}, @legalentitykey)

	                                    select @externallifepolicykey = SCOPE_IDENTITY()

	                                    insert into [2am]..offerexternallife
	                                    (offerkey, externallifepolicykey)
	                                    values ({7}, @externallifepolicykey)",
                                        externalLifePolicy.InsurerKey,
                                        externalLifePolicy.PolicyNumber.ToNullableSQLString(),
                                        externalLifePolicy.CommencementDate.ToString(Common.Constants.Formats.DateTimeFormatSQL).ToNullableSQLString(),
                                        externalLifePolicy.LifePolicyStatusKey,
                                        externalLifePolicy.CloseDate.ToNullableSQLString(),
                                        externalLifePolicy.SumInsured,
                                        Convert.ToInt32(externalLifePolicy.PolicyCeded),
                                        offerKey);
            dataContext.Execute(query);
        }

        public void AddSAHLLife(int offerKey)
        {
            string query = string.Format(@"declare @lifeMultiplier float
                                        declare @initFee float

                                        select @lifeMultiplier = controlNumeric from [2am].dbo.[control] where controlDescription = 'PersonalLoanCreditLifePremium'
                                        select @initFee = controlNumeric from [2am].dbo.[control] where controlDescription = 'PersonalLoanInitiationFee'

                                        ;with oipl as
	                                        (select top 1 oipl.* from [2am].dbo.offer o
	                                        join [2am].dbo.offerinformation oi on o.offerkey = oi.offerkey
	                                        join [2am].dbo.offerinformationpersonalloan oipl on oi.offerinformationkey = oipl.offerinformationkey
	                                        where o.offerkey = {0}
	                                        order by offerinformationkey)
                                        Update oipl set lifePremium = @lifeMultiplier*(oipl.loanAmount+@initFee)

                                        insert into [2am].dbo.offerattribute (offerkey, offerattributetypekey) values ({0}, 12)", offerKey);
            dataContext.Execute(query);
        }

        public void InsertOfferAttribute(int offerKey, OfferAttributeTypeEnum offerAttributeType)
        {
            string query = string.Format(@"
                                    INSERT INTO [2AM].dbo.offerAttribute
                                    (offerKey, offerAttributeTypeKey)
                                    VALUES
                                    ({0},{1})", offerKey, (int)offerAttributeType);
            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        public void DeleteOfferAttribute(int offerKey, OfferAttributeTypeEnum offerAttributeType)
        {
            string query = string.Format(@"
                                    DELETE FROM [2AM].dbo.offerAttribute
                                    WHERE offerKey={0} and offerAttributeTypeKey={1}", offerKey, (int)offerAttributeType);
            SQLStatement statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        public void AddRecuringDiscountAttributeToOffer(int offerKey)
        {
            string query = string.Format(@"INSERT INTO [2AM].dbo.OfferAttribute
                SELECT o.OfferKey,
	                29
                FROM [2AM].dbo.Offer o
	                LEFT JOIN [2AM].dbo.OfferAttribute oa (NOLOCK) ON o.OfferKey = oa.OfferKey AND oa.OfferAttributeTypeKey = 29
                WHERE o.OfferKey = {0}
	                AND oa.OfferAttributeKey IS NULL", offerKey);
            dataContext.Execute(query);
        }

        public IEnumerable<Automation.DataModels.OfferRole> GetOfferRoleAttributes(int offerKey)
        {
            var query = string.Format(@"SELECT *
                FROM [2AM].dbo.OfferRole r
	                JOIN [2AM].dbo.OfferRoleAttribute ora ON r.OfferRoleKey = ora.OfferRoleKey
                WHERE R.OfferKey = {0}", offerKey);
            return dataContext.Query<Automation.DataModels.OfferRole, Automation.DataModels.OfferRoleAttribute, Automation.DataModels.OfferRole>(query, (r, ora) => { r.OfferRoleAttribute = ora; return r; }, splitOn: "OfferRoleKey", commandtype: CommandType.Text);
        }

        public IEnumerable<DataModels.Offer> GetOpenFurtherLendingOffersAtStateByAccountKey(int accountKey, string stateName)
        {
            string query =
                string.Format(@"Select o.* from [2AM]..Offer o
                        join [X2].[X2DATA].Application_Management am on o.OfferKey = am.ApplicationKey
                        join [X2].[X2].Instance i on am.InstanceID = i.id
                        join [X2].[X2].State s on i.StateID = s.ID
                        where o.OfferStatusKey = 1
                        and o.OfferTypeKey in (2,3,4)
                        and s.name = '{0}'
                        and o.AccountKey = {1}", stateName, accountKey);
            return dataContext.Query<Automation.DataModels.Offer>(query).DefaultIfEmpty();
        }

        public void UpdateAllMainApplicantEmploymentRecords(int offerKey, int employmentType, float householdIncome, bool GEPFfunded)
        {
            var sql = string.Format(@"declare @offerKey int = {0}
                declare @employmentType int = {1}
                declare @householdIncome float = {2}
                declare @GEPFfunded bit = '{3}'
                declare @leCount int = 0
                declare @employmentTable table(EmploymentKey int, LegalEntityKey int)
				declare @subsidyTable table(SubsidyKey int)

                select @leCount = count(r.LegalEntityKey)
                from [2am]..OfferRole r
	                join [2AM]..OfferRoleType ort on r.OfferRoleTypeKey = ort.OfferRoleTypeKey and ort.OfferRoleTypeGroupKey = 3
                where r.OfferKey = @offerKey
	                and r.GeneralStatusKey = 1

                update e set
                EmploymentEndDate = getdate(), EmploymentStatusKey = 2
                from [2am]..OfferRole r
	                join [2AM]..OfferRoleType ort on r.OfferRoleTypeKey = ort.OfferRoleTypeKey and ort.OfferRoleTypeGroupKey = 3
	                join [2AM]..LegalEntity le on r.LegalEntityKey = le.LegalEntityKey
	                join [2AM]..Employment e on le.LegalEntityKey = e.LegalEntityKey
                where r.OfferKey = @offerKey
	                and r.GeneralStatusKey = 1
	                and e.EmploymentStatusKey = 1

                Insert into [2AM]..Employment (EmployerKey, EmploymentTypeKey, RemunerationTypeKey, EmploymentStatusKey, LegalEntityKey, EmploymentStartDate, BasicIncome)
                output inserted.EmploymentKey, inserted.LegalEntityKey into @employmentTable(EmploymentKey, LegalEntityKey)
                select EmployerKey = case @employmentType when 3 then 69326 else 7 end,
	                EmploymentTypeKey = @employmentType,
	                RemunerationTypeKey = case @employmentType when 2 then 11 else 2 end,
	                EmploymentStatusKey = 1,
	                LegalEntityKey = r.LegalEntityKey,
	                EmploymentStartDate = dateadd(yy,-2,getdate()),
	                BasicIncome = @householdIncome/@leCount
                from [2am]..OfferRole r
	                join [2AM]..OfferRoleType ort on r.OfferRoleTypeKey = ort.OfferRoleTypeKey and ort.OfferRoleTypeGroupKey = 3
                where r.OfferKey = @offerKey
	                and r.GeneralStatusKey = 1

                if @employmentType = 3
                begin
	                insert into [2AM]..Subsidy (SubsidyProviderKey, EmploymentKey, LegalEntityKey, SalaryNumber, Paypoint, Rank, GeneralStatusKey, StopOrderAmount, GEPFMember)
					output inserted.SubsidyKey into @subsidyTable(SubsidyKey)
	                Select SubsidyProviderKey = 583,
		                EmploymentKey = e.EmploymentKey,
		                LegalEntityKey = e.LegalEntityKey,
		                SalaryNumber = 'TestData',
		                Paypoint = 'Durban',
		                Rank = 'Manager',
		                GeneralStatusKey = 1,
		                StopOrderAmount = @householdIncome/(2*@leCount),
                        GEPFMember = @GEPFfunded
	                from @employmentTable e

					insert into [2AM]..OfferSubsidy (OfferKey, SubsidyKey)
					select OfferKey = @offerKey,
						SubsidyKey = s.SubsidyKey
					from @subsidyTable s
                end", offerKey, employmentType, householdIncome, GEPFfunded);
            var statement = new SQLStatement { StatementString = sql };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        public IEnumerable<DataModels.OfferAccountRelationship> GetOfferAccountRelationships(int mortgageLoanOfferKey)
        {
            var query = string.Format(@"select * from [2am].dbo.OfferAccountRelationship where OfferKey = {0}", mortgageLoanOfferKey);
            return dataContext.Query<Automation.DataModels.OfferAccountRelationship>(query);
        }

        public int GetMaxOfferKey()
        {
            var results = dataContext.ExecuteSQLScalar(new SQLStatement { StatementString = "select max(offerkey) from [2am].dbo.Offer" });
            return Int32.Parse(results.SQLScalarValue);
        }

        public IEnumerable<Automation.DataModels.Offer> GetCapitecMortgageLoanApplications()
        {
            string query = string.Format(@" SELECT TOP 10
	                                            le.LegalentityKey,
	                                            o.OfferKey
                                            FROM [dbo].Offer o
	                                            INNER JOIN dbo.OfferAttribute oat (NOLOCK) ON oat.OfferKey = o.OfferKey
	                                            INNER JOIN dbo.OfferRole ofr (NOLOCK) ON ofr.OfferKey = o.OfferKey
	                                            INNER JOIN dbo.LegalEntity le (NOLOCK) ON le.LegalEntityKey = ofr.LegalEntityKey
                                            WHERE o.OfferStatusKey = 1
                                            AND oat.OfferAttributeTypeKey = 30
                                            AND ofr.OfferRoleTypeKey = 8");
            var statement = new SQLStatement { StatementString = query };
            return dataContext.Query<Automation.DataModels.Offer>(query);
        }

        public int GetOfferByLegalEntityKey(int legalEntityKey)
        {
            string query = string.Format(@"SELECT o.OfferKey
                                FROM [2AM].[dbo].Offer o
	                                INNER JOIN [2AM].[dbo].Externalrole er ON o.OfferKey = er.GenericKey AND er.GenericKeyTypeKey = 2
                                WHERE er.Legalentitykey = {0}", legalEntityKey);
            return Int32.Parse(dataContext.ExecuteSQLScalar(new SQLStatement { StatementString = query }).SQLScalarValue);
        }

        public int GetLegalEntityKeyFromOffer(int offerKey)
        {
            string query = string.Format(@" USE [2AM]
                                                    SELECT
        	                                        le.LegalEntityKey
                                                FROM [2AM].[dbo].LegalEntity le
        	                                        INNER JOIN [2AM].[dbo].ExternalRole er ON er.LegalEntityKey = le.LegalEntityKey
                                                WHERE er.GenericKey = {0}", offerKey);
            return Int32.Parse(dataContext.ExecuteSQLScalar(new SQLStatement { StatementString = query }).SQLScalarValue);
        }

        public IEnumerable<Automation.DataModels.Offer> GetAlphaOffersAtAppCapWithoutCapitalisedInitiationFeeOfferAttribute()
        {
            string query = string.Format(@"SELECT
	                                        o.OfferKey
                                        FROM Offer o
	                                        INNER JOIN OfferAttribute t on o.OfferKey=t.OfferKey
	                                        INNER JOIN (select m.OfferKey, max(m.OfferInformationKey) as MaxOIKey from OfferInformation m group by m.OfferKey) maxoi on o.OfferKey = maxoi.OfferKey
	                                        INNER JOIN OfferInformation oi on maxoi.MaxOIKey = oi.OfferInformationKey
	                                        INNER JOIN OfferInformationVariableLoan vl on oi.OfferInformationKey = vl.OfferInformationKey
	                                        INNER JOIN x2.x2data.Application_Capture data on o.OfferKey=data.ApplicationKey
	                                        INNER JOIN x2.x2.Instance i on data.InstanceID=i.ID
	                                        INNER JOIN x2.x2.State s on i.StateID=s.ID
                                        WHERE o.OfferStatusKey=1 and o.OfferTypeKey=7
                                        AND t.OfferAttributeTypeKey=26 and t.OfferAttributeTypeKey<>35
                                        AND i.ParentInstanceID is null
                                        AND s.Name = 'Application Capture'
                                        AND vl.LTV < 0.85");
            var statement = new SQLStatement { StatementString = query };
            return dataContext.Query<Automation.DataModels.Offer>(query);
        }

        public IEnumerable<Automation.DataModels.Offer> GetAlphaOffersAtAppCapeWith100PercentLTV()
        {
            string query = string.Format(@"SELECT
	                                        o.OfferKey
                                        FROM Offer o
	                                        INNER JOIN OfferAttribute t on o.OfferKey=t.OfferKey
	                                        INNER JOIN (select m.OfferKey, max(m.OfferInformationKey) as MaxOIKey from OfferInformation m group by m.OfferKey) maxoi on o.OfferKey = maxoi.OfferKey
	                                        INNER JOIN OfferInformation oi on maxoi.MaxOIKey = oi.OfferInformationKey
	                                        INNER JOIN OfferInformationVariableLoan vl on oi.OfferInformationKey = vl.OfferInformationKey
	                                        INNER JOIN x2.x2data.Application_Capture data on o.OfferKey=data.ApplicationKey
	                                        INNER JOIN x2.x2.Instance i on data.InstanceID=i.ID
	                                        INNER JOIN x2.x2.State s on i.StateID=s.ID
                                        WHERE o.OfferStatusKey=1 and o.OfferTypeKey=7
                                        AND t.OfferAttributeTypeKey=26 and t.OfferAttributeTypeKey<>35
                                        AND i.ParentInstanceID is null
                                        AND s.Name = 'Application Capture'
                                        AND vl.LTV = 1.00");
            var statement = new SQLStatement { StatementString = query };
            return dataContext.Query<Automation.DataModels.Offer>(query);
        }

        public IEnumerable<Automation.DataModels.Offer> GetAlphaOffersAtAppMan()
        {
            string query = string.Format(@"SELECT
	                                            o.OfferKey
                                            FROM [2AM].[dbo].Offer o
	                                            INNER JOIN [2AM].[dbo].OfferAttribute oa ON oa.OfferKey = o.OfferKey
	                                            INNER JOIN [2AM].[dbo].OfferAttributeType oat ON oat.OfferAttributeTypeKey = oa.OfferAttributeTypeKey
	                                            INNER JOIN [X2].[X2DATA].Application_Management x2AppMan ON x2AppMan.ApplicationKey = o.OfferKey
	                                            INNER JOIN [X2].[X2].Instance x2i ON x2i.ID = x2AppMan.InstanceID
	                                            INNER JOIN [X2].[X2].State x2s ON x2s.ID = x2i.StateID
                                            WHERE o.OfferStatusKey = 1
                                            AND o.OfferTypeKey = 7
                                            AND oat.OfferAttributeTypeKey = 26
                                            AND oat.OfferAttributeTypeKey <> 35
                                            AND x2s.Name = 'Manage Application'");
            var statement = new SQLStatement { StatementString = query };
            return dataContext.Query<Automation.DataModels.Offer>(query);
        }

        public int GetOfferByWorkflowAndState(string workflowState)
        {
            string query = string.Format(@" SELECT TOP 1
	                                            o.OfferKey
                                            FROM [2AM].[dbo].Offer o
	                                            INNER JOIN [X2].[X2DATA].Application_Management x2AppMan ON x2AppMan.ApplicationKey = o.OfferKey
	                                            INNER JOIN [X2].[X2].Instance x2i ON x2i.ID = x2AppMan.InstanceID AND x2i.ParentInstanceID IS NULL
	                                            INNER JOIN [X2].[X2].State x2s ON x2s.ID = x2i.StateID AND x2s.Name LIKE '%{0}%'
                                            WHERE o.OfferStatusKey = 1
                                            ORDER BY NEWID()", workflowState);
            return Int32.Parse(dataContext.ExecuteSQLScalar(new SQLStatement { StatementString = query }).SQLScalarValue);
        }

        public int GetOfferByOfferTypeAndWorkflowState(int offerType, string workflowState)
        {
            string query = string.Format(@" SELECT TOP 1
	                                            o.OfferKey
                                            FROM [2AM].[dbo].Offer o
	                                            INNER JOIN [X2].[X2DATA].Application_Management x2AppMan ON x2AppMan.ApplicationKey = o.OfferKey
	                                            INNER JOIN [X2].[X2].Instance x2i ON x2i.ID = x2AppMan.InstanceID AND x2i.ParentInstanceID IS NULL
	                                            INNER JOIN [X2].[X2].State x2s ON x2s.ID = x2i.StateID AND x2s.Name LIKE '%{1}%'
                                            WHERE o.OfferStatusKey = 1
                                            AND o.OfferTypeKey = {0}
                                            ORDER BY NEWID()", offerType, workflowState);
            return Int32.Parse(dataContext.ExecuteSQLScalar(new SQLStatement { StatementString = query }).SQLScalarValue);
        }
    }
}