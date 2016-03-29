using Automation.DataModels;
using Common.Constants;
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
        ///
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <returns></returns>
        public QueryResults GetEmploymentByGenericKey(int generickey, bool legalEntityKey, bool employmentKey)
        {
            var query = "select * from [2AM].[dbo].[Employment] (nolock) where {0}={1}";
            if (legalEntityKey)
                query = String.Format(query, "LegalEntityKey", generickey);
            else if (employmentKey)
                query = String.Format(query, "EmploymentKey", generickey);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Gets an employment record
        /// </summary>
        /// <param name="legalEntityKey">LegalEntityKey</param>
        /// <param name="employmentType">EmploymentType</param>
        /// <param name="remunerationType">RemunerationType</param>
        /// <param name="employmentStatus">EmploymentStatus</param>
        /// <returns>Employment.*</returns>
        public QueryResults GetEmploymentByCriteria(int legalEntityKey, EmploymentTypeEnum employmentType, RemunerationTypeEnum remunerationType,
            EmploymentStatusEnum employmentStatus)
        {
            string query =
                String.Format(@"select * from [2AM].[dbo].[Employment] (nolock) where LegalEntityKey={0}
                                and EmploymentTypeKey={1} and RemunerationTypeKey={2} and EmploymentStatusKey={3}",
                                legalEntityKey, (int)employmentType, (int)remunerationType, (int)employmentStatus);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }
        public IEnumerable<Employment> GetEmployments(int legalEntityKey, EmploymentStatusEnum employmentStatus)
        {
            string query =
                String.Format(@"select * from [2AM].[dbo].[Employment] (nolock) where LegalEntityKey={0}
                                and EmploymentStatusKey={1}",
                                legalEntityKey, (int)employmentStatus);
            return dataContext.Query<Employment>(query);
        }
        /// <summary>
        /// Get a single employment record.
        /// </summary>
        /// <param name="employmentType"></param>
        /// <param name="remunerationType"></param>
        /// <param name="employmentStatus"></param>
        /// <param name="idnumber"></param>
        /// <param name="legalname"></param>
        /// <returns></returns>
        public QueryResultsRow GetLegalEntityEmploymentRecord
            (
                EmploymentTypeEnum employmentType,
                RemunerationTypeEnum remunerationType,
                EmploymentStatusEnum employmentStatus,
                string firstnames = "",
                string surname = "",
                string idnumber = ""
            )
        {
            string query =
                String.Format(@"select top 01 * from [2AM].[dbo].[LegalEntity]
		                        inner join [2AM].[dbo].[Employment] (nolock)
			                    on [LegalEntity].[LegalEntityKey] = [Employment].[LegalEntityKey]
	                            where ([LegalEntity].[IdNumber] = '{0}' or ([LegalEntity].FirstNames = '{1}' and [LegalEntity].Surname = '{2}'))
	                            and EmploymentTypeKey={3} and RemunerationTypeKey={4}
	                            and EmploymentStatusKey={5}", idnumber, firstnames, surname, (int)employmentType, (int)remunerationType, (int)employmentStatus);
            SQLStatement statement = new SQLStatement { StatementString = query };
            var results = dataContext.ExecuteSQLQuery(statement);
            if (results.RowList.Count == 0)
                throw new Exception(string.Format(@"There are no employment records for the legal entity with IDNumber: {0}", idnumber));
            return results.Rows(0);
        }

        /// <summary>
        /// Inserts Employment Records
        /// </summary>
        /// <param name="offerKey"></param>
        public void InsertEmploymentRecords(int offerKey)
        {
            var p = new DynamicParameters();
            p.Add("@offerKey", value: offerKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            dataContext.Execute("[2am].[test].[InsertEmploymentRecords]", parameters: p, commandtype: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Inserts Employment for a legal entity
        /// </summary>
        /// <param name="employment"></param>
        public Automation.DataModels.Employment InsertEmployment(Automation.DataModels.Employment employment)
        {
            var p = new DynamicParameters();

            p.Add("@EmployerKey", value: employment.EmployerKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@EmploymentTypeKey", value: (int)employment.EmploymentTypeKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@RemunerationTypeKey", value: (int)employment.RemunerationTypeKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@EmploymentStatusKey", value: (int)employment.EmploymentStatusKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@LegalEntityKey", value: employment.LegalEntityKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@EmploymentStartDate", value: employment.EmploymentStartDate, dbType: DbType.DateTime, direction: ParameterDirection.Input);
            p.Add("@EmploymentEndDate", value: employment.EmploymentEndDate, dbType: DbType.DateTime, direction: ParameterDirection.Input);
            p.Add("@ContactPerson", value: employment.ContactPerson, dbType: DbType.String, direction: ParameterDirection.Input);
            p.Add("@ContactPhoneNumber", value: employment.ContactPhoneNumber, dbType: DbType.String, direction: ParameterDirection.Input);
            p.Add("@ContactPhoneCode", value: employment.ContactPhoneCode, dbType: DbType.String, direction: ParameterDirection.Input);
            p.Add("@ConfirmedBy", value: employment.ConfirmedBy, dbType: DbType.String, direction: ParameterDirection.Input);
            p.Add("@ConfirmedDate", value: employment.ConfirmedDate, dbType: DbType.DateTime, direction: ParameterDirection.Input);
            p.Add("@UserID", value: employment.UserID, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@ChangeDate", value: employment.ChangeDate, dbType: DbType.DateTime, direction: ParameterDirection.Input);
            p.Add("@Department", value: employment.Department, dbType: DbType.String, direction: ParameterDirection.Input);
            p.Add("@BasicIncome", value: employment.BasicIncome, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            p.Add("@Commission", value: employment.Commission, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            p.Add("@Overtime", value: employment.Overtime, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            p.Add("@Shift", value: employment.Shift, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            p.Add("@Performance", value: employment.Performance, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            p.Add("@Allowances", value: employment.Allowances, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            p.Add("@PAYE", value: employment.PAYE, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            p.Add("@UIF", value: employment.UIF, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            p.Add("@PensionProvident", value: employment.PensionProvident, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            p.Add("@MedicalAid", value: employment.MedicalAid, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            p.Add("@ConfirmedBasicIncome", value: employment.ConfirmedBasicIncome, dbType: DbType.Double, direction: ParameterDirection.Input);
            p.Add("@ConfirmedCommission", value: employment.ConfirmedCommission, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            p.Add("@ConfirmedOvertime", value: employment.ConfirmedOvertime, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            p.Add("@ConfirmedShift", value: employment.ConfirmedShift, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            p.Add("@ConfirmedPerformance", value: employment.ConfirmedPerformance, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            p.Add("@ConfirmedAllowances", value: employment.ConfirmedAllowances, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            p.Add("@ConfirmedPAYE", value: employment.ConfirmedPAYE, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            p.Add("@ConfirmedUIF", value: employment.ConfirmedUIF, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            p.Add("@ConfirmedPensionProvident", value: employment.ConfirmedPensionProvident, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            p.Add("@ConfirmedMedicalAid", value: employment.ConfirmedMedicalAid, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            p.Add("@JobTitle", value: employment.JobTitle, dbType: DbType.String, direction: ParameterDirection.Input);
            p.Add("@ConfirmedEmploymentFlag", value: employment.ConfirmedEmploymentFlag, dbType: DbType.Boolean, direction: ParameterDirection.Input);
            p.Add("@ConfirmedIncomeFlag", value: employment.ConfirmedIncomeFlag, dbType: DbType.Boolean, direction: ParameterDirection.Input);
            p.Add("@EmploymentConfirmationSourceKey", value: employment.EmploymentConfirmationSourceKey, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@SalaryPaymentDay", value: employment.SalaryPaymentDay, dbType: DbType.Int32, direction: ParameterDirection.Input);

            return dataContext.Query<Automation.DataModels.Employment>("test.InsertEmployment", parameters: p, commandtype: CommandType.StoredProcedure).FirstOrDefault();
        }

        /// <summary>
        /// Deletes employment records that match the employment type provided for a legal entity
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <param name="employmentType"></param>
        public void DeleteLegalEntityEmployment(int legalEntityKey, EmploymentTypeEnum employmentType)
        {
            var sql = string.Format(@"declare @employmentKey int
                declare @subsidyKey int

                select top 1 @employmentKey = employmentKey from [2am].dbo.Employment
                where legalEntityKey = {0} and employmentTypeKey = {1}
                select @subsidyKey = subsidyKey from [2am].dbo.Subsidy where employmentKey = @employmentKey

                delete from [2am].dbo.OfferSubsidy where subsidyKey = @SubsidyKey
                delete from [2am].dbo.Subsidy where employmentKey = @employmentKey
                delete from [2am].dbo.Employment where employmentKey = @employmentKey", legalEntityKey, (int)employmentType);
            var statement = new SQLStatement { StatementString = sql };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        public Automation.DataModels.Employer InsertEmployer(Automation.DataModels.Employer employer)
        {
            var query = String.Format(@"
                                              BEGIN TRAN
                                              INSERT INTO [2AM].[dbo].[Employer]
                                                   ([Name]
                                                   ,[TelephoneNumber]
                                                   ,[TelephoneCode]
                                                   ,[ContactPerson]
                                                   ,[ContactEmail]
                                                   ,[AccountantName]
                                                   ,[AccountantContactPerson]
                                                   ,[AccountantTelephoneCode]
                                                   ,[AccountantTelephoneNumber]
                                                   ,[AccountantEmail]
                                                   ,[EmployerBusinessTypeKey]
                                                   ,[UserID]
                                                   ,[ChangeDate]
                                                   ,[EmploymentSectorKey])
                                             VALUES
                                                   ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}','{12}',{13})
                                              SELECT TOP 01
	                                            Employer.*,
	                                            EmployerBusinessType.Description as EmployerBusinessTypeDescription,
	                                            EmploymentSector.Description as EmploymentSectorDescription
                                              FROM [2AM].[dbo].[Employer]
	                                            INNER JOIN dbo.EmployerBusinessType
		                                            on [Employer].EmployerBusinessTypeKey = EmployerBusinessType.EmployerBusinessTypeKey
	                                            INNER JOIN dbo.EmploymentSector
		                                            on [Employer].EmploymentSectorKey = EmploymentSector.EmploymentSectorKey
                                               ORDER BY 1 DESC
                                             COMMIT TRAN",
                                       employer.Name,
                                       employer.TelephoneNumber,
                                       employer.TelephoneCode,
                                       employer.ContactPerson,
                                       employer.ContactEmail,
                                       employer.AccountantName,
                                       employer.AccountantContactPerson,
                                       employer.AccountantTelephoneCode,
                                       employer.AccountantTelephoneNumber,
                                       employer.AccountantEmail,
                                       (int)employer.EmployerBusinessTypeKey,
                                       employer.UserID,
                                       employer.ChangeDate.ToString(Formats.DateTimeFormatSQL),
                                       (int)employer.EmploymentSectorKey);
            return dataContext.Query<Automation.DataModels.Employer>(query).FirstOrDefault();
        }

        public void DeleteEmployer(int employerkey)
        {
            var query = String.Format(@"DELETE FROM [dbo].[Employer]
                                            WHERE employerkey = {0}", employerkey);
            dataContext.Execute(query);
        }

        public Automation.DataModels.Employer GetEmployer(string employerName)
        {
            var query = String.Format(@"select * from dbo.employer
                                                where name = '{0}'", employerName);
            return dataContext.Query<Automation.DataModels.Employer>(query).FirstOrDefault();
        }

        public void DeleteLegalEntityEmployment(int legalEntityKey)
        {
            var query = String.Format(@"delete from dbo.employment
                                            where legalentitykey = 0", legalEntityKey);
            dataContext.Execute(query);
        }

        public void UpdateAllEmploymentStatus(int legalEntityKey, EmploymentStatusEnum employmentStatus)
        {
            var query = String.Format(@"update dbo.employment
                                            set employmentenddate = getdate()
                                            where legalentitykey = {0}

                                            update dbo.employment
                                            set employmentstatuskey = {1}
                                            where legalentitykey = {0}", legalEntityKey, (int)employmentStatus);
            dataContext.Execute(query);
        }

        public void UpdateSalaryPaymentDay(int legalEntityKey, int? salaryPaymentDay)
        {
            var query = "update dbo.employment set salarypaymentday = {0} where legalEntityKey = {1}";
            if (salaryPaymentDay == null)
                query = String.Format(query, "NULL", legalEntityKey);
            else
                query = String.Format(query, salaryPaymentDay.Value, legalEntityKey);
            dataContext.Execute(query);
        }

        public void ConfirmEmployment(Automation.DataModels.Employment employment)
        {
            var query = string.Format(@"update e
                    set ContactPerson = '{0}',
                        ContactPhoneNumber = {1},
                        ContactPhoneCode = {2},
                        ConfirmedBy = '{3}',
                        ConfirmedDate = '{4}',
                        ConfirmedBasicIncome = BasicIncome,
                        ConfirmedEmploymentFlag = {5},
                        ConfirmedIncomeFlag = {6},
                        EmploymentConfirmationSourceKey = {7}
                    from dbo.employment e
                    where EmploymentKey = {8}
                        and employmentstatuskey = 1
                        and (isnull(ConfirmedIncomeFlag,0) = 0
                        or isnull(ConfirmedEmploymentFlag,0) = 0) ",
                employment.ContactPerson,
                employment.ContactPhoneNumber,
                employment.ContactPhoneCode,
                employment.ConfirmedBy,
                employment.ConfirmedDate.Value.ToString(Formats.DateTimeFormatSQL),
                Convert.ToInt32(employment.ConfirmedEmploymentFlag),
                Convert.ToInt32(employment.ConfirmedIncomeFlag),
                (int)employment.EmploymentConfirmationSourceKey,
                employment.EmploymentKey);

            dataContext.Execute(query);
        }

        public void UpdateEmploymentIncome(int offerKey, double income)
        {
            var query = string.Format(@"update e set BasicIncome = {0}, Commission = null, Overtime = null, [Shift] = null, Performance = null, Allowances = null, PAYE = null, UIF = null, PensionProvident = null, MedicalAid = null,
                        ConfirmedBasicIncome = {0}, ConfirmedCommission = null, ConfirmedOvertime = null, ConfirmedShift = null, ConfirmedPerformance = null, ConfirmedAllowances = null, ConfirmedPAYE = null, ConfirmedUIF = null, ConfirmedPensionProvident = null, ConfirmedMedicalAid = null
                        from [2am]..offerrole r
                        join [2am]..offerroletype ort on r.OfferRoleTypeKey = ort.OfferRoleTypeKey
                        join [2am]..Employment e on r.LegalEntityKey = e.LegalEntityKey
                        where r.offerkey = {1}
                        and ort.OfferRoleTypeGroupKey = 3
                        and e.EmploymentStatusKey = 1", income, offerKey);
            dataContext.Execute(query);
        }
    }
}