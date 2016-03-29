using SAHL.Core.Configuration;
using System;
using System.Net;
using System.Web;
using System.Linq;

namespace SAHL.Core.Web.CORS
{
    public class CORSModule : IHttpModule
    {
        private ConfigurationProvider configProvider;

        public void Dispose()
        {
            configProvider = null;
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
            this.configProvider = new ConfigurationProvider();
        }

        private void OnBeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;

            string allowedOriginsConfig = this.configProvider.Config.AppSettings.Settings["AllowedOrigins"].Value;
            string[] allowedOrigins = allowedOriginsConfig.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string origin = context.Request.Headers["Origin"];
            
            if (context.Request.RequestType == "OPTIONS")
            {
                string accessHeaders = context.Request.Headers["Access-Control-Request-Headers"];
                if (!string.IsNullOrEmpty(origin) && allowedOrigins.Any(x => x.ToLower().Trim() == origin.ToLower().Trim()))
                {
                    context.Response.Headers.Add("Access-Control-Allow-Origin", origin);
                    context.Response.Headers.Add("Access-Control-Allow-Headers", accessHeaders != null ? accessHeaders : "content-type");
                    context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.End();
                    application.CompleteRequest();
                }
                return;
            }
            else
            {
                context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            }
        }
    }
}