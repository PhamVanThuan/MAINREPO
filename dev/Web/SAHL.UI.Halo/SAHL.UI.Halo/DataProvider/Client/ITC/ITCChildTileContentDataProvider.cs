using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.Client.ITC;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Client.ITC
{
    public class ITCChildTileContentDataProvider : HaloTileBaseContentDataProvider<ITCChildModel>
    {
        public ITCChildTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"select top 1
                    ITCAge = dbo.fCalculateAge(i.ChangeDate, getdate()),
                    CreditScore = case
                                    when cast(convert(varchar(20),i.ResponseXML.query(N'declare namespace TUBureau=""https://secure.transunion.co.za/TUBureau"";
(BureauResponse/TUBureau:EmpiricaEM07/TUBureau:EmpiricaScore/text())')) as int) = 0
                                        then cast(isnull(convert(varchar(20),i.ResponseXML.query(N'declare namespace TUBureau=""https://secure.transunion.co.za/TUBureau"";
(BureauResponse/TUBureau:EmpiricaEM05/TUBureau:EmpiricaScore/text())')),'-999') as int)
                                        else cast(isnull(convert(varchar(20),ResponseXML.query(N'declare namespace TUBureau=""https://secure.transunion.co.za/TUBureau"";
(BureauResponse/TUBureau:EmpiricaEM07/TUBureau:EmpiricaScore/text())')),'-999') as int)
                                    end,
                    CreditScoreVersion = case
                                        when cast(convert(varchar(20),i.ResponseXML.query(N'declare namespace TUBureau=""https://secure.transunion.co.za/TUBureau"";
(BureauResponse/TUBureau:EmpiricaEM07/TUBureau:EmpiricaScore/text())')) as int) = 0
                                        then 'Empirica V3'
                                            else 'Empirica V4'
                                        end,
                    cast(convert(varchar(20),i.ResponseXML.query(N'declare namespace TUBureau=""https://secure.transunion.co.za/TUBureau"";
(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:Judgements1YrBack/text())')) as int)
                    +cast(convert(varchar(20),i.ResponseXML.query(N'declare namespace TUBureau=""https://secure.transunion.co.za/TUBureau"";
(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:Judgements2YrBack/text())')) as int)
                    +cast(convert(varchar(20),i.ResponseXML.query(N'declare namespace TUBureau=""https://secure.transunion.co.za/TUBureau"";
(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:JudgementsMoreThen2YrsBack/text())')) as int)
                    as Judgements,
                    cast(convert(varchar(20),i.ResponseXML.query(N'declare namespace TUBureau=""https://secure.transunion.co.za/TUBureau"";
(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:Notices1YrBack/text())')) as int)
                    +cast(convert(varchar(20),i.ResponseXML.query(N'declare namespace TUBureau=""https://secure.transunion.co.za/TUBureau"";
(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:Notices2YrBack/text())')) as int)
                    +cast(convert(varchar(20),i.ResponseXML.query(N'declare namespace TUBureau=""https://secure.transunion.co.za/TUBureau"";
(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:NoticesMoreThen2YrsBack/text())')) as int)
                    as Notices,
                    cast(convert(varchar(20),i.ResponseXML.query(N'declare namespace TUBureau=""https://secure.transunion.co.za/TUBureau"";
(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:Defaults1YrBack/text())')) as int)
                    +cast(convert(varchar(20),i.ResponseXML.query(N'declare namespace TUBureau=""https://secure.transunion.co.za/TUBureau"";
(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:Defaultss2YrBack/text())')) as int)
                    +cast(convert(varchar(20),i.ResponseXML.query(N'declare namespace TUBureau=""https://secure.transunion.co.za/TUBureau"";
(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:DefaultsMoreThen2YrsBack/text())')) as int)
                    as Defaults,
                    cast(convert(varchar(20),i.ResponseXML.query(N'declare namespace TUBureau=""https://secure.transunion.co.za/TUBureau"";
(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:TraceAlerts1YrBack/text())')) as int)
                    +cast(convert(varchar(20),i.ResponseXML.query(N'declare namespace TUBureau=""https://secure.transunion.co.za/TUBureau"";
(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:TraceAlerts2YrBack/text())')) as int)
                    +cast(convert(varchar(20),i.ResponseXML.query(N'declare namespace TUBureau=""https://secure.transunion.co.za/TUBureau"";
(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:TraceAlertsMoreThen2YrsBack/text())')) as int)
                    as TraceAlerts
                from
                    [2am].[dbo].[itc] i
                where
                    i.LegalEntityKey={0}
                order by
                    i.ChangeDate desc", businessContext.BusinessKey.Key);
        }
    }
}