using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.Client.Detail.EmploymentSummary;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.ContentProvider.Client.Detail.EmploymentSummary
{
    public class EmploymentSummaryChildTileContentDataProvider : HaloTileBaseContentDataProvider<EmploymentSummaryChildModel>
    {
        public EmploymentSummaryChildTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"select
                        isnull(CurrentTotalUnconfirmedIncome,0) as CurrentTotalUnconfirmedIncome,
                        isnull(CurrentTotalConfirmedIncome,0) as CurrentTotalConfirmedIncome,
                        PrimaryEmploymentType = case
                                                    when PrimaryConfirmedEmploymentType is not null then PrimaryConfirmedEmploymentType
                                                    when PrimaryUnConfirmedEmploymentType is not null then PrimaryUnConfirmedEmploymentType
                                                    else 'Unknown'
                                                end
                    from
                        [2am].[dbo].LegalEntity le
                    left join
                    (
                        select
                            e.LegalEntityKey,
                            sum(e.MonthlyIncome) as CurrentTotalUnconfirmedIncome
                        from
                            [2am].[dbo].[Employment] e
                        where
                            e.employmentstatuskey = 1
                            and e.legalentitykey = {0}
                            and ConfirmedDate is NULL
                            or ConfirmedIncomeFlag = 0
                            and EmploymentStartDate <= getdate()
                            and (employmentenddate is null or employmentenddate >= getdate())
                        group by
                            e.LegalEntityKey
                    ) UnconfirmedIncome on UnconfirmedIncome.LegalEntityKey=le.LegalEntityKey
                    left join
                    (
                        select
                            e.LegalEntityKey,
                            sum(e.ConfirmedIncome) as CurrentTotalConfirmedIncome
                        from
                            [2am].[dbo].[Employment] e
                        where
                            e.employmentstatuskey = 1
                            and e.legalentitykey = {0}
                            and ConfirmedDate is not NULL
                            and   (ConfirmedDate <= getdate() or ConfirmedIncomeFlag=1)
                            and   EmploymentStartDate <= getdate()
                            and (employmentenddate is null or employmentenddate >= getdate())
                        group by
                            e.LegalEntityKey
                    ) ConfirmedIncome on ConfirmedIncome.LegalEntityKey=le.LegalEntityKey
                    left join
                    (
                        select top 1
                            e.LegalEntityKey,
                            et.Description as PrimaryConfirmedEmploymentType
                        from
                            [2am].[dbo].[Employment] e
                        join
                            [2am].[dbo].[EmploymentType] et on et.EmploymentTypeKey=e.EmploymentTypeKey
                        where
                            e.employmentstatuskey = 1
                            and e.legalentitykey = {0}
                            and (ConfirmedDate <= getdate() or ConfirmedIncomeFlag=1)
                            and EmploymentStartDate <= getdate()
                            and (employmentenddate is null or employmentenddate >= getdate())
                        order by
                            e.ConfirmedIncome desc
                    ) IncomeType on IncomeType.LegalEntityKey=le.LegalEntityKey
                    left join
                    (
                        select top 1
                            e.LegalEntityKey,
                            et.Description as PrimaryUnconfirmedEmploymentType
                        from
                            [2am].[dbo].[Employment] e
                        join
                            [2am].[dbo].[EmploymentType] et on et.EmploymentTypeKey=e.EmploymentTypeKey
                        where
                            e.employmentstatuskey = 1
                            and e.legalentitykey = {0}
                            and (ConfirmedDate is null or ConfirmedIncomeFlag=0)
                            and EmploymentStartDate <= getdate()
                            and (employmentenddate is null or employmentenddate >= getdate())
                        order by
                            e.MonthlyIncome desc
                    ) UnconfirmedIncomeType on UnconfirmedIncomeType.LegalEntityKey=le.LegalEntityKey
                    where
                        le.LegalEntityKey={0}", businessContext.BusinessKey.Key);
        }
    }
}