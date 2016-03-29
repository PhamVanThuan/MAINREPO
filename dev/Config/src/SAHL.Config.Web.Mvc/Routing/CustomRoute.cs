using SAHL.Config.Web.Mvc.Routing.Configuration.Transforms;

namespace SAHL.Config.Web.Mvc.Routing
{
    public abstract class CustomRoute : ICustomRoute
    {
        protected CustomRoute(string name, string url, object defaults = null, object constraints = null)
        {
            this.Name = name;
            this.Url = GetUrlTemplate(url);
            this.Defaults = defaults;
            this.Constraints = constraints;
        }

        private static string GetUrlTemplate(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }
            var temp = url;
            temp = RemoveRelativePlaceholder(temp);
            temp = FixDuplicateParameterPlaceholders(temp);
            return temp;
        }

        private static string FixDuplicateParameterPlaceholders(string url)
        {
            var parameterNumberer = new CustomRouteDuplicateParameterNumberer();
            return parameterNumberer.Number(url);
        }

        private static string RemoveRelativePlaceholder(string url)
        {
            if (url.StartsWith("~/"))
            {
                return url.Substring(2);
            }
            if (url.StartsWith("~") || url.StartsWith("/"))
            {
                return url.Substring(1);
            }
            return url;
        }

        public override string ToString()
        {
            return Url;
        }

        public string Name { get; set; }
        public string Url { get; private set; }
        public object Defaults { get; private set; }
        public object Constraints { get; private set; }
    }
}
