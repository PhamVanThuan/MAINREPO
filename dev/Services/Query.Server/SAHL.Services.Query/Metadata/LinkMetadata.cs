using System;
using System.Security.Policy;
using WebApi.Hal;

namespace SAHL.Services.Query.Metadata
{
    public class LinkMetadata
    {
        public LinkMetadata(Type representationType, string urlTemplate, string controllerName = null, string name = null, string action = null)
        {
            if (name == null)
            {
                if (representationType == null)
                {
                    throw new InvalidOperationException("ReprsentationType and/or name must have a value. Both cannot be null.");
                }

                Name = RemoveFromString(representationType.Name, "Representation");
                
            }
            else
            {
                Name = name;
            }

            Relationship = Name;
            RepresentationType = representationType;
            UrlTemplate = GetUrlTemplate(urlTemplate).ToLower();
            Controller = GetControllerName(controllerName);
            Action = action;
        }

        public string Name { get; private set; }
        public Type RepresentationType { get; private set; }
        public string UrlTemplate { get; private set; }
        public string Relationship { get; private set; }
        public string Controller { get; private set; }
        public string Action { get; private set; }

        private static string GetUrlTemplate(string urlTemplate)
        {
            if (string.IsNullOrWhiteSpace(urlTemplate))
            {
                return "~/";
            }
            if (urlTemplate.StartsWith("~"))
            {
                return urlTemplate;
            }
            if (urlTemplate.StartsWith("/"))
            {
                return "~" + urlTemplate;
            }
            return "~/" + urlTemplate;
        }

        private string GetControllerName(string controller)
        {
            if (string.IsNullOrWhiteSpace(controller))
            {
                return RepresentationType == null ? string.Empty : RemoveFromString(RepresentationType.Name, "ListRepresentation", "Representation");
            }
            return controller.Replace("controller", string.Empty, StringComparison.OrdinalIgnoreCase);
        }

        private string RemoveFromString(string source, params string[] subStringsToRemove)
        {
            if (subStringsToRemove == null)
            {
                return source;
            }
            var temp = source;
            foreach (var item in subStringsToRemove)
            {
                temp = temp.Replace(item, string.Empty);
            }
            return temp;
        }

        public Link ToHalLink(string relationship = null)
        {
            var rel = (relationship ?? Relationship);
            return new Link(rel, UrlTemplate);
        }

        public override string ToString()
        {
            return Name + ": " + UrlTemplate;
        }
    }
}
