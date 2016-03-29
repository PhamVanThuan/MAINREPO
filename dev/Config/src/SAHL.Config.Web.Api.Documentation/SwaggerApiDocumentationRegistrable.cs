using Swashbuckle.Application;
using System.Linq;
using System.Web.Http;

namespace SAHL.Config.Web.Api.Documentation
{
    public class SwaggerApiDocumentationRegistrable : IApiDocumentationRegistrable
    {
        private string ServiceName;
        private string XmlCommentsFileName;

        public SwaggerApiDocumentationRegistrable(string serviceName, string xmlCommentsFile)
        {
            this.ServiceName = serviceName;
            this.XmlCommentsFileName = xmlCommentsFile;
        }

        public void Register()
        {
            GlobalConfiguration.Configuration.EnableSwagger("docs/api/{apiVersion}", c =>
            {
                c.SingleApiVersion("v2", string.Format("API - {0}", ServiceName));

                c.ResolveConflictingActions(x => x.First());
                c.IgnoreObsoleteActions();
                c.IgnoreObsoleteProperties();
                c.IncludeXmlComments(GetXmlCommentsPath(XmlCommentsFileName));
            })
           .EnableSwaggerUi("docs/{*assetPath}");
        }

        protected static string GetXmlCommentsPath(string commentFileName)
        {
            return System.String.Format(@"{0}\bin\{1}.XML", System.AppDomain.CurrentDomain.BaseDirectory, commentFileName);
        }
    }
}