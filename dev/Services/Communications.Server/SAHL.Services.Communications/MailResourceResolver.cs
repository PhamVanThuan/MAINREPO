using ActionMailerNext.Standalone;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Communications
{
    public class MailResourceResolver : ITemplateResolver
    {
        private readonly string viewPath;

        public MailResourceResolver(string viewPath)
        {
            this.viewPath = viewPath;
        }

        public string Resolve(string name)
        {
            if (string.IsNullOrWhiteSpace(name)){
                throw new ArgumentNullException("name");
            }

            var csViewName = name;
            var vbViewName = name;

            if (!csViewName.EndsWith(".cshtml")){
                csViewName += ".cshtml";
            }

            if (!vbViewName.EndsWith(".vbhtml")){
                vbViewName += ".vbhtml";
            }

            // an e.g. key to the resource looks like Namespace.To.Views.Mail.html.cshtml
            var csViewPath = string.Format("{0}.{1}", viewPath, csViewName);
            var vbViewPath = string.Format("{0}.{1}", viewPath, vbViewName);

            var csView = GetView(csViewPath);
            if (csView != null)
            {
                return csView;
            }

            var vbView = GetView(vbViewPath);
            if (vbView != null)
            {
                return vbView;
            }

            throw new TemplateResolvingException { SearchPaths = new List<string> { csViewPath, vbViewPath } };
        }

        private string GetView(string resourcePath)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream(viewPath))
                using (var sr = new StreamReader(stream))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
