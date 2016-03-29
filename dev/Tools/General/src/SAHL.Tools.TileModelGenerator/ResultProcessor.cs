using Mono.Cecil;
using SAHL.Tools.TileModelGenerator.Model;
using SAHL.Tools.TileModelGenerator.Reflection;
using SAHL.Tools.TileModelGenerator.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.TileModelGenerator
{
    public class ResultProcessor
    {
        public IFileManager fileManager;
        private string location;

        public ResultProcessor(IFileManager fileManager,string location)
        {
            this.fileManager = fileManager;
            this.location = location;
        }

        public void GenerateEditorViews(TileEditorConvention editorConfiguration, TileModelConvention models)
        {
            var modelsThatCanBeEdited = editorConfiguration.Result.SelectMany(x => x.Interfaces).Where(y => y.IsGenericInstance && y.Name == "IHaloTileModel`1").Select(z => new TileModel(((GenericInstanceType)z).GenericArguments[0].Resolve()));
            

            foreach (TileModel model in modelsThatCanBeEdited)
            {
                string expectedEditPath = Path.Combine(location,"tiles", model.FilePath.Replace(".tpl.", ".edit.tpl."));

                if (!fileManager.DoesFileExist(expectedEditPath))
                {
                    TileModelEdit template = new TileModelEdit(model);
                    string templateContent = template.TransformText();
                    fileManager.SaveNewFile(templateContent, expectedEditPath);
                }
            }
        }

        public void GenerateViews(TileModelConvention result)
        {
            foreach (TileModel model in result.Result.Select(x => new TileModel(x)))
            {
                string expectedPath = Path.Combine(location,"tiles", model.FilePath);

                if (!fileManager.DoesFileExist(expectedPath))
                {
                    TileModelView viewTemplate = new TileModelView(model);
                    string templateContent = viewTemplate.TransformText();
                    fileManager.SaveNewFile(templateContent, expectedPath);
                }
            }
        }

        public void GeneratePages(TileToPageConvention result)
        {
            foreach (PageModel model in result.Result.Select(x => new PageModel(x)))
            {
                string pageBasePath = Path.Combine(location, model.TypePage);
                if (!fileManager.DoesFileExist(pageBasePath))
                {
                    string name = StringHelper.toCamelCase(model.Name);
                    if (!fileManager.DoesFileExist(Path.Combine(pageBasePath, model.FilePath, name + ".tpl.html"))) { 
                        HtmlPageTemplate htmlTemplate = new HtmlPageTemplate(model);
                        string htmlText = htmlTemplate.TransformText();
                        fileManager.SaveNewFile(htmlText, Path.Combine(pageBasePath, model.FilePath, name + ".tpl.html"));
                    
                        HtmlPageJSTemplate jsTemplate = new HtmlPageJSTemplate(model);
                        string jsText = jsTemplate.TransformText();
                        fileManager.SaveNewFile(jsText, Path.Combine(pageBasePath, model.FilePath, name + ".js"));
                    }
                }
            }
        }


    }
}
