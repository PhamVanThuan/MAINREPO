using SAHL.Core.Data;
using SAHL.Services.EventProjection.Models;

namespace SAHL.Services.EventProjection.Projections
{
    public class ProjectionsUIStatementProvider : IUIStatementsProvider
    {
        public string UIStatementContext
        {
            get { return "SAHL.Services.EventProjection.Models"; }
        }

        public const string salesconsultantdailypipelinedatamodel_selectwhere =
            @"select Consultant, ApplicationStage, NumberOfApplicationsAtStage, Period, SalesValue from projection.SalesConsultantDailyPipeline where";

        public const string salesconsultantdailypipelinedatamodel_insert =
            @"insert into projection.SalesConsultantDailyPipeline (Consultant, ApplicationStage, NumberOfApplicationsAtStage, Period, SalesValue)
                values (@Consultant,@ApplicationStage, @NumberOfApplicationsAtStage, @Period, @SalesValue)";

        public const string salesconsultantdailypipelinedatamodel_update =
            @"update projection.SalesConsultantDailyPipeline set ApplicationStage = @ApplicationStage, NumberOfApplicationsAtStage = @NumberOfApplicationsAtStage,
                Period = @Period, SalesValue = @SalesValue where Consultant = @Consultant and ApplicationStage = @ApplicationStage";
    }

    public class EmptyProjectionSqlStatement : ISqlStatement<SalesConsultantDailyPipelineDataModel>
    {
        public string GetStatement()
        {
            return "select 1";
        }
    }
}