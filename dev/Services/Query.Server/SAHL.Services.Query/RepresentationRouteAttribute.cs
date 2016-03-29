using System;

namespace SAHL.Services.Query
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class RepresentationRouteAttribute : Attribute
    {
        public string Action { get; private set; }
        public string UrlTemplate { get; private set; }
        public string Name { get; private set; }
        public Type[] RepresentationTypesToInclude { get; private set; }

        public RepresentationRouteAttribute(string urlTemplate, Type representationTypeToInclude, string action = null)
            : this(urlTemplate, null, representationTypeToInclude, action)
        {
        }

        public RepresentationRouteAttribute(string urlTemplate, Type[] representationTypeToInclude, string action = null)
            : this(urlTemplate, null, representationTypeToInclude, action)
        {
        }

        public RepresentationRouteAttribute(string urlTemplate, string name, Type representationTypeToInclude, string action = null)
            : this(urlTemplate, name, new[] { representationTypeToInclude }, action)
        {
        }

        private RepresentationRouteAttribute(string urlTemplate, string name, Type[] representationTypeToInclude, string action = null)
        {
            this.Action = action;
            this.UrlTemplate = urlTemplate;
            this.Name = name;
            this.RepresentationTypesToInclude = representationTypeToInclude;
        }
    }
}