using System.Linq;

namespace SAHL.Tools.RestServiceRoutenator.Templates.Partials
{
    public partial class RoutePartTemplate : ITemplate, ITemplateForModel<Routes>
    {

        private readonly ITemplateManager TemplateManager;

        private string GetHeader()
        {
            if (!ShouldBeFunction)
            {
                return Model.RoutePart + " : ";
            }

            var parameter = Model.RoutePart.Replace("{", "").Replace("}", "");
            return "getBy" + parameter.MakeFirstLetterCapitalCase() + " : function(" + parameter + ") {   settingsToPass.pushParameter(" + parameter + ", " + (this.Model.QualifiedRoute.Count(x => x == '[') - 1) + ");  return ";
        }

        private bool ShouldBeFunction
        {
            get { return Model.RoutePart.StartsWith("{"); }
        }

        private string GetQualifiedRoute()
        {
            return this.Model.QualifiedRoute.Replace("[", "{").Replace("]", "}");
        }

        public RoutePartTemplate(ITemplateManager templateManager)
        {
            TemplateManager = templateManager;
        }

        public string Process(dynamic model)
        {
            this.Model = model;
            return this.TransformText();
        }

        public Routes Model { get; private set; }
    }
}