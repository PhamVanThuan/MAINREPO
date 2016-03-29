using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;
using System;

namespace SAHL.Core.UI.Halo.Tiles.Employment.Default
{
    public class LegalEntityEmploymentDetailMinorTileContentProvider : AbstractSqlTileContentProvider<LegalEntityEmploymentDetailMinorTileModel>
    {
        public override string GetStatement(BusinessKey businessKey)
        {
            return String.Format(@"SELECT 
                    emp.Name as EmployerName
                    , et.[Description] as EmploymentType, 
                    CASE
	                    WHEN isnull(leEmp.ConfirmedIncome, 0) > 1 THEN 'Confirmed'
	                    ELSE 'Unconfirmed'
                    END as Confirmed
                    , leEmp.ConfirmedDate as ConfirmedDate
                    , 
                    CASE
	                    WHEN isnull(leEmp.ConfirmedIncome, 0) > 1 THEN ConfirmedIncome
	                    ELSE MonthlyIncome
                    END as IncomeAmount, 
                    SalaryPaymentDay as SalaryPaymentDay
                    from [2am].dbo.Employment leEmp
                    join [2am].dbo.Employer emp on leEmp.EmployerKey = emp.EmployerKey
                    join [2am].dbo.EmploymentType et on leEmp.EmploymentTypeKey = et.EmploymentTypeKey
                    where 
                    leEmp.EmploymentKey = {0}", businessKey.Key.ToString());
        }
    }
}