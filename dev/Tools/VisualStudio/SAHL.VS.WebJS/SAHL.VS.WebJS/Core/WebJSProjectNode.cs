using EnvDTE;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Project.Automation;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VSLangProj;
using System.Linq;

namespace SAHomeloans.SAHL_VS_WebJS.Core
{
    [System.Runtime.InteropServices.Guid(GuidList.WebJSProjectNode)]
    public class WebJSProjectNode : ProjectNode
    {
        #region constants

        internal const string ProjectTypeName = "WebJS";
        internal static ImageList imageList;
        internal static int imageOffset;

        #endregion constants

        #region variables

        private WebJSPackage package;
        private VSLangProj.VSProject vsProject;

        #endregion variables

        #region properties

        public override Guid ProjectGuid
        {
            get { return typeof(WebJSProjectFactory).GUID; }
        }

        public override string ProjectType
        {
            get { return ProjectTypeName; }
        }

        public override int ImageIndex
        {
            get { return imageOffset + 0; }
        }

        protected internal VSLangProj.VSProject VSProject
        {
            get
            {
                if (vsProject == null)
                {
                    vsProject = new OAVSProject(this);
                }

                return vsProject;
            }
        }

        #endregion properties

        #region constructors

        static WebJSProjectNode()
        {
            imageList = Microsoft.VisualStudio.Project.Utilities.GetImageList(typeof(WebJSProjectNode).Assembly.GetManifestResourceStream("SAHomeloans.SAHL_VS_WebJS.Resources.webjs.bmp"));
        }

        public WebJSProjectNode(WebJSPackage package)
        {
            this.package = package;
            InitializeImageList();
            this.CanProjectDeleteItems = true;
            this.CanFileNodesHaveChilds = true;
        }

        #endregion constructors

        #region

        public override FileNode CreateFileNode(ProjectElement item)
        {
            WebJSProjectFileNode node = new WebJSProjectFileNode(this, item);

            node.OleServiceProvider.AddService(typeof(EnvDTE.Project), new OleServiceProvider.ServiceCreatorCallback(this.CreateServices), false);
            node.OleServiceProvider.AddService(typeof(ProjectItem), node.ServiceCreator, false);
            node.OleServiceProvider.AddService(typeof(VSProject), new OleServiceProvider.ServiceCreatorCallback(this.CreateServices), false);

            return node;
        }

        public override object GetAutomationObject()
        {
            return new OAWebJSProject(this);
        }

        protected override Guid[] GetPriorityProjectDesignerPages()
        {
            Guid[] result = new Guid[1];
            result[0] = typeof(GeneralPropertyPage).GUID;
            return result;
        }

        protected override Guid[] GetConfigurationIndependentPropertyPages()
        {
            Guid[] result = new Guid[1];
            result[0] = typeof(GeneralPropertyPage).GUID;
            return result;
        }

        public override void AddFileFromTemplate(string source, string target)
        {
            if (!File.Exists(source))
            {
                throw new FileNotFoundException(string.Format("Template file not found: {0}", source));
            }
            // The class name is based on the new file name
            string className = Path.GetFileNameWithoutExtension(target).Replace(".spec", "");
            string namespce = this.FileTemplateProcessor.GetFileNamespace(target, this).Replace(".lib","").Split(new char[] { '.' }).Select(x => StringHelpers.ToCamelCase(x)).Aggregate((x, y) => x + "." + y);
            string directory = Path.GetDirectoryName(target);
            string location = RootDirectoryHelper.GetAppRoot(target).Replace(".js", ".tpl.html");
            string state = RootDirectoryHelper.GetAppRoot(directory).Replace("./app/", "").Replace("/", ".").Replace(".js", "");

            this.FileTemplateProcessor.AddReplace("%pcClassName%", StringHelpers.ToPascalCase(className));
            this.FileTemplateProcessor.AddReplace("%ccClassName%", StringHelpers.ToCamelCase(className));
            this.FileTemplateProcessor.AddReplace("%namespace%", namespce);

            this.FileTemplateProcessor.AddReplace("%location%", location);
            this.FileTemplateProcessor.AddReplace("%state%", state);

            try
            {
                this.FileTemplateProcessor.UntokenFile(source, target);
                this.FileTemplateProcessor.Reset();
            }
            catch (Exception e)
            {
                throw new FileLoadException("Failed to add template file to project", target, e);
            }
        }

        public override void AddChild(HierarchyNode node)
        {
            base.AddChild(node);
        }

        #endregion

        #region private methods

        private void InitializeImageList()
        {
            imageOffset = this.ImageHandler.ImageList.Images.Count;
            foreach (Image img in imageList.Images)
            {
                this.ImageHandler.AddImage(img);
            }
        }

        private object CreateServices(Type serviceType)
        {
            object service = null;
            if (typeof(VSLangProj.VSProject) == serviceType)
            {
                service = this.VSProject;
            }
            else if (typeof(EnvDTE.Project) == serviceType)
            {
                service = this.GetAutomationObject();
            }
            return service;
        }

        #endregion
    }
}