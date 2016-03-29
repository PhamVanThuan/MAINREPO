using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using SAHL.Common.Web;
using SAHL.Common.Factories;
using SAHL.Common.Security;
using SAHL.Common.Logging;
using SAHL.Communication;

namespace SAHL.Web.Services.Internal
{
	public class Global : System.Web.HttpApplication
	{

		protected void Application_Start(object sender, EventArgs e)
		{
			LogPlugin.Logger = new MessageBusLogger(new EasyNetQMessageBus(new EasyNetQMessageBusConfigurationProvider()), new MessageBusLoggerConfiguration());
			MetricsPlugin.Metrics = new MessageBusMetrics(new EasyNetQMessageBus(new EasyNetQMessageBusConfigurationProvider()), new MessageBusMetricsConfiguration());
            ActiveRecordHelper.InitialiseActiveRecord();
            SAHL.Web.Services.Internal.DataModel.AutoMapperHelper.SetUp();
		}

		protected void Session_Start(object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{
		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{

		}

		protected void Application_Error(object sender, EventArgs e)
		{

		}

		protected void Session_End(object sender, EventArgs e)
		{

		}

		protected void Application_End(object sender, EventArgs e)
		{

		}
	}
}