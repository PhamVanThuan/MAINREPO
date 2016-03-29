using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Mono.Cecil;

namespace SAHL.Tools.RestServiceRoutenator.Templates.Partials
{
    public partial class RouteDefinitionTemplate : ITemplate, ITemplateForModel<MethodDefinition>
    {
        public string Process(dynamic model)
        {
            Model = model;
            return TransformText();
        }

        internal string GetMethodRoute()
        {
            var routeGiven = OfficialRoute;
            return Sanatise(routeGiven);
        }

        private static string Sanatise(string routeGiven)
        {
            var tempRoute = routeGiven
                .Replace("/api/", "").Replace("{", "' + ").Replace("}", " + '");
            return tempRoute;
            //return tempRoute.EndsWith("+ '")? tempRoute.Remove(tempRoute.Length -4, 3) : tempRoute;
        }


        private string GetName()
        {
            var fields = GetExtraFields();
            IEnumerable<string> inclusions = GetExtraInclusions();
            var functionName = Model.Name.MakeFirstLetterLowerCase();
            if (fields.Any())
            {
                var enumerable = fields.Select(x => x.MakeFirstLetterCapitalCase());
                var withParameters = functionName + "By" + enumerable.Aggregate((current, next) => current + next);

                return withParameters +(inclusions.Any()? "Include" + inclusions.Aggregate((current, next) => current + "And" + next) :"" ) ;
            }
            return functionName;
        }

        private IEnumerable<string> GetExtraInclusions()
        {
            var inclustions = Regex.Matches(OfficialRoute, @"(\}\/\w+)");
            foreach (Match inclustion in inclustions)
            {
                yield return inclustion.Value.Replace("}/", "").MakeFirstLetterCapitalCase();
            }
        }

        private string cachedRoute;

        private string OfficialRoute
        {
            get
            {
                if (cachedRoute != null)
                {
                    return cachedRoute;
                }
                var customAttribute =
                    Model.CustomAttributes.First(x => x.AttributeType.Name == "RepresentationRouteAttribute");
                cachedRoute= customAttribute.ConstructorArguments.First().Value.ToString();
                return cachedRoute;
            }
        }

        internal IEnumerable<String> GetParameters()
        {
            var parameters = GetExtraFields();
            if (!parameters.Any())
                yield return "";
            foreach (string parameter in parameters)
            {
                yield return parameter;
            }
        }

        private IEnumerable<string> GetExtraFields()
        {

            var parameters = Regex.Matches(OfficialRoute, @"(\/\{\w+})");
            foreach (Match match in parameters)
            {
                yield return match.Value.Replace(@"/{", "").Replace("}", "");
            }

            //var theParameters = OfficialRoute.Split('{').Where(x => !x.Contains("api/")).Select(x => x.Replace("}", "").Replace("/", ""));
        }

        public MethodDefinition Model { get; private set; }
    }
}