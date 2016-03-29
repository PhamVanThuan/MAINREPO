using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.SystemMessages;
﻿using SAHL.Core.Data.Models.X2;
﻿using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Factories;
using SAHL.X2Engine2.MessageHandlers;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.Services;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SAHL.X2Engine2
{
    public class X2ActivityScheduler : IX2ActivityScheduler, IX2MessageHandler<X2NotificationOfNewScheduledActivityRequest>
    {
        private IWorkflowDataProvider workflowDataProvider;
        private IX2SoonestActivityScheduleSelector soonestActivityScheduleSelector;
        private List<ScheduledActivityDataModel> scheduledActivities;
        private IX2ScheduledActivityTimerFactory scheduledActivityTimerFactory;
        private IX2ManagementSubscriber managementSubscriber;
        private IX2EngineConfigurationProvider engineConfigurationProvider;
        private IX2Engine engine;
        private IX2ScheduledActivityTimer timer;
        private IServiceRequestMetadata serviceRequestMetadata;

        private List<string> availableRoutes = new List<string>();
        private static object victim = new object();

        public X2ActivityScheduler(IWorkflowDataProvider workflowDataProvider, IX2SoonestActivityScheduleSelector soonestActivityScheduleSelector,
            IX2ScheduledActivityTimerFactory scheduledActivityTimerFactory, IX2ManagementSubscriber managementSubscriber,
            IX2EngineConfigurationProvider engineConfigurationProvider)
        {
            this.workflowDataProvider = workflowDataProvider;
            this.soonestActivityScheduleSelector = soonestActivityScheduleSelector;
            this.scheduledActivityTimerFactory = scheduledActivityTimerFactory;
            this.managementSubscriber = managementSubscriber;
            this.engineConfigurationProvider = engineConfigurationProvider;
            this.serviceRequestMetadata = new ServiceRequestMetadata(new Dictionary<string, string>() 
                            {
                                { SAHL.Core.Services.ServiceRequestMetadata.HEADER_USERNAME, "X2" }
                            });
            timer = scheduledActivityTimerFactory.CreateTimer();
        }

        public void Initialise(IX2Engine engine)
        {
            Initialise();
            this.engine = engine;
            scheduledActivities = new List<ScheduledActivityDataModel>(workflowDataProvider.GetAllScheduledTimerActivities());
            WaitXSecondsForNodesToRegister();
            ProcessScheduledActivities();
        }

        private void WaitXSecondsForNodesToRegister()
        {
            int milliseconds = this.engineConfigurationProvider.GetTimeToWaitUntilSchedulingActivities();
            Thread.Sleep(milliseconds);
        }

        public void Initialise()
        {
            var scheduledActivityHandler = this as IX2MessageHandler<X2NotificationOfNewScheduledActivityRequest>;
            managementSubscriber.Subscribe(new X2RouteEndpoint(X2QueueManager.X2EngineTimersRefreshExchange, X2QueueManager.X2EngineTimersRefreshQueue), scheduledActivityHandler);
        }

        private void InvalidateExistingTimer()
        {
            if (timer != null)
            {
                timer.Stop();
            }
        }

        private int GetMillisecondsTillNextActivity(DateTime nextActivityTime)
        {
            int millisecondsUntilActivity = 0;
            long ticks = nextActivityTime.Ticks - DateTime.Now.Ticks;
            if (ticks > 10000)
            {
                long milliseconds = ticks / 10000;
                if (milliseconds > int.MaxValue)
                    return -1;
            }
            return millisecondsUntilActivity;
        }

        private void ProcessScheduledActivities()
        {
            InvalidateExistingTimer();
            lock (victim)
            {
                ScheduledActivityDataModel scheduledActivityDataModel = soonestActivityScheduleSelector.GetNextActivityToSchedule(scheduledActivities);
                if (scheduledActivityDataModel != null && scheduledActivityDataModel.Time <= DateTime.Now)
                {
                    lock (this.scheduledActivities)
                    {
                        while (scheduledActivityDataModel != null && scheduledActivityDataModel.Time <= DateTime.Now)
                        {
                            var request = BuildSystemRequestGroupForTimer(scheduledActivityDataModel);
                            engine.ReceiveSystemRequest(request);
                            RemoveScheduledActivity(scheduledActivityDataModel);
                            scheduledActivityDataModel = soonestActivityScheduleSelector.GetNextActivityToSchedule(scheduledActivities);
                        }
                    }
                }
            }
            ScheduleNextActivity();
        }

        private void ScheduleNextActivity()
        {
            ScheduledActivityDataModel scheduledActivityDataModel = soonestActivityScheduleSelector.GetNextActivityToSchedule(scheduledActivities);
            if (scheduledActivityDataModel != null)
            {
                lock (this.scheduledActivities)
                {
                    int millisecondsUntilActivity = GetMillisecondsTillNextActivity(scheduledActivityDataModel.Time);
                    timer.Start(millisecondsUntilActivity, ProcessScheduledActivities);
                }
            }
        }

        private void RemoveScheduledActivity(ScheduledActivityDataModel scheduledActivityDataModel)
        {
            lock (this.scheduledActivities)
            {
                this.scheduledActivities.Remove(scheduledActivityDataModel);
            }
        }

        private X2SystemRequestGroup BuildSystemRequestGroupForTimer(ScheduledActivityDataModel scheduledActivityDataModel)
        {
            ActivityDataModel activityDataModel = workflowDataProvider.GetActivity(scheduledActivityDataModel.ActivityID);
            return new X2SystemRequestGroup(Guid.NewGuid(), this.serviceRequestMetadata, X2RequestType.Timer, scheduledActivityDataModel.InstanceID, new List<string> { activityDataModel.Name }, scheduledActivityDataModel.Time);
        }

        private void InnerHandle(ScheduledActivityDataModel scheduledActivityDataModel)
        {
            lock (this.scheduledActivities)
            {
                this.scheduledActivities.Add(scheduledActivityDataModel);
                ScheduleNextActivity();
            }
        }

        public void HandleCommand(X2NotificationOfNewScheduledActivityRequest message)
        {
            ScheduledActivityDataModel scheduledActivityDataModel = workflowDataProvider.GetScheduledActivity(message.InstanceId, message.ActivityId);
            InnerHandle(scheduledActivityDataModel);
        }

        public void TearDown()
        {
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }
        }
    }
}