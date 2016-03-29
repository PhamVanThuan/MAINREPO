using SAHL.Services.LegacyEventGenerator.Services.Models;
using SAHL.Services.LegacyEventGenerator.Services.Models.Events;
using System.Collections.Generic;

namespace SAHL.Services.LegacyEventGenerator.Services
{
    public interface ILegacyEventDataService
    {
        StageTransitionCompositeEventDataModel GetModelByStageTransitionCompositeKey(int StageTransitionCompositeKey);

        IDictionary<string, int> GetEventTypeMapping();

        AppProgressInApplicationCaptureModel GetAppProgressModelByStageTransitionCompositeKey(int stageTransitionCompositeKey);
    }
}