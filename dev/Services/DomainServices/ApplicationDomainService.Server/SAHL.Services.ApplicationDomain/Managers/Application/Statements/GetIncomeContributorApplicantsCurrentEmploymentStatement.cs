using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ApplicationDomain.Managers.Application.Statements
{
    public class GetIncomeContributorApplicantsCurrentEmploymentStatement : ISqlStatement<EmploymentDataModel>
    {
        public int ApplicationNumber { get; protected set; }

        public GetIncomeContributorApplicantsCurrentEmploymentStatement(int applicationNumber)
        {
            this.ApplicationNumber = applicationNumber;
        }

        public string GetStatement()
        {
            return @"select e.[EmploymentKey]
                  ,e.[EmployerKey]
                  ,e.[EmploymentTypeKey]
                  ,e.[RemunerationTypeKey]
                  ,e.[EmploymentStatusKey]
                  ,e.[LegalEntityKey]
                  ,e.[EmploymentStartDate]
                  ,e.[EmploymentEndDate]
                  ,e.[ContactPerson]
                  ,e.[ContactPhoneNumber]
                  ,e.[ContactPhoneCode]
                  ,e.[ConfirmedBy]
                  ,e.[ConfirmedDate]
                  ,e.[UserID]
                  ,e.[ChangeDate]
                  ,e.[Department]
                  ,e.[BasicIncome]
                  ,e.[Commission]
                  ,e.[Overtime]
                  ,e.[Shift]
                  ,e.[Performance]
                  ,e.[Allowances]
                  ,e.[PAYE]
                  ,e.[UIF]
                  ,e.[PensionProvident]
                  ,e.[MedicalAid]
                  ,e.[ConfirmedBasicIncome]
                  ,e.[ConfirmedCommission]
                  ,e.[ConfirmedOvertime]
                  ,e.[ConfirmedShift]
                  ,e.[ConfirmedPerformance]
                  ,e.[ConfirmedAllowances]
                  ,e.[ConfirmedPAYE]
                  ,e.[ConfirmedUIF]
                  ,e.[ConfirmedPensionProvident]
                  ,e.[ConfirmedMedicalAid]
                  ,e.[JobTitle]
                  ,e.[MonthlyIncome]
                  ,e.[ConfirmedIncome]
                  ,e.[ConfirmedEmploymentFlag]
                  ,e.[ConfirmedIncomeFlag]
                  ,e.[EmploymentConfirmationSourceKey]
                  ,e.[SalaryPaymentDay]
                  ,e.[UnionMember]
            from [2AM].dbo.Employment e
            join [2AM].dbo.OfferRole ofr on ofr.LegalEntityKey = e.LegalEntityKey
            join [2AM].dbo.[OfferRoleType] ofrt on ofrt.OfferRoleTypeKey = ofr.OfferRoleTypeKey
                and ofrt.OfferRoleTypeGroupKey = 3 -- Client
            join [2AM].[dbo].[OfferRoleAttribute] ofra on ofra.OfferRoleKey = ofr.OfferRoleKey
                and ofra.OfferRoleAttributeTypeKey = 1 -- Income Contributor
            where e.EmploymentStatusKey = 1 -- Current
            and (e.EmploymentEndDate is null
                or e.EmploymentEndDate > dateadd(day, 1, getdate()))
            and ofr.OfferKey = @ApplicationNumber";
        }
    }
}