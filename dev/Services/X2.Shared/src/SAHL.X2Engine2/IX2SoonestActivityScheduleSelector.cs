using System.Collections.Generic;
using SAHL.Core.Data.Models.X2;

namespace SAHL.X2Engine2
{
    public interface IX2SoonestActivityScheduleSelector
    {
        ScheduledActivityDataModel GetNextActivityToSchedule(IEnumerable<ScheduledActivityDataModel> scheduledActivities);
    }
}