using SAHL.Core.Data;
using SAHL.Core.Data.Context;
using SAHL.Services.LegacyEventGenerator.Services.Models;
using SAHL.Services.LegacyEventGenerator.Services.Models.Events;
using SAHL.Services.LegacyEventGenerator.Services.Statements;
using SAHL.Services.LegacyEventGenerator.Services.Statements.Events;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.LegacyEventGenerator.Services
{
    public class LegacyEventDataService : ILegacyEventDataService
    {
        private IDbFactory dbFactory;

        public LegacyEventDataService(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public StageTransitionCompositeEventDataModel GetModelByStageTransitionCompositeKey(int stageTransitionCompositeKey)
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                GetStageTransitionCompositeDataStatement query = new GetStageTransitionCompositeDataStatement(stageTransitionCompositeKey);
                return db.SelectOne<StageTransitionCompositeEventDataModel>(query);
            }
        }

        public IDictionary<string, int> GetEventTypeMapping()
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                GetEventTypeMappingStatement query = new GetEventTypeMappingStatement();
                return db.Select(query).ToDictionary(item => item.Name, item => item.EventTypeKey);
            }
        }

        public AppProgressInApplicationCaptureModel GetAppProgressModelByStageTransitionCompositeKey(int stageTransitionCompositeKey)
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var query = new GetAppProgressInApplicationCaptureModelStatement(stageTransitionCompositeKey);
                return db.SelectOne<AppProgressInApplicationCaptureModel>(query);
            }
        }
    }
}