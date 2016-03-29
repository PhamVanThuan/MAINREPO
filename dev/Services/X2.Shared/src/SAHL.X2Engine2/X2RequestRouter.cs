using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Communication;
using SAHL.Core.X2.Exceptions;
using System;

namespace SAHL.X2Engine2
{
	public class X2RequestRouter : IX2RequestRouter
	{
		private IX2RequestInterrogator interrogator;
		private IX2RoutePlanner routePlanner;
		private IX2RequestPublisher requestPublisher;

		public X2RequestRouter(IX2RequestInterrogator interrogator, IX2RoutePlanner routePlanner, IX2RequestPublisher requestPublisher)
		{
			this.interrogator = interrogator;
			this.routePlanner = routePlanner;
			this.requestPublisher = requestPublisher;
		}

		public void Initialise()
		{
			routePlanner.Initialise();
		}

		public void RouteRequest<T>(T request, IX2RequestMonitor requestMonitor) where T : class, IX2Request
		{
			X2Workflow workflow = interrogator.GetRequestWorkflow(request);
            bool monitoredRequest = interrogator.IsRequestMonitored(request);
            IX2RouteEndpoint plannedRoute = routePlanner.PlanRoute(monitoredRequest, workflow);
			if (plannedRoute == null)
			{
				throw new NoRoutesAvailableException(String.Format("No routes available for {0} to service the request.", workflow.WorkflowName));
			}
            if (monitoredRequest)
            {
                requestMonitor.MonitorRequest(request);
            }
			requestPublisher.Publish(plannedRoute, request);
		}
    }
}