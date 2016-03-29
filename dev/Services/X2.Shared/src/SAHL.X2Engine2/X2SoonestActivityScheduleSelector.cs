using SAHL.Core.Data.Models.X2;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.X2Engine2
{
    public class X2SoonestActivityScheduleSelector : IX2SoonestActivityScheduleSelector
    {
        public ScheduledActivityDataModel GetNextActivityToSchedule(IEnumerable<ScheduledActivityDataModel> scheduledActivities)
        {
            lock (scheduledActivities)
            {
                if (scheduledActivities.Any())
                {
                    ScheduledActivityDataModel scheduledActivityDataModel = scheduledActivities.OrderBy(x => x.Time).First();
                    return scheduledActivityDataModel;
                }
                return null;
            }
        }
    }
}