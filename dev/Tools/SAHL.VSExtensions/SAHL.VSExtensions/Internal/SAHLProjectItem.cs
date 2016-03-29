using EnvDTE;
using Microsoft.VisualStudio;
using SAHL.VSExtensions.Interfaces;
using SAHL.VSExtensions.Interfaces.Configuration;
using StructureMap;
using System.IO;

namespace SAHomeloans.SAHL_VSExtensions.Internal
{
    public class SAHLProjectItem : ISAHLProjectItem
    {
        private ProjectItem projectItem;
        private ISAHLProject project;
        private IContainer container;

        public string Name
        {
            get;
            protected set;
        }

        public string ItemPath
        {
            get;
            protected set;
        }

        public string ProjectName
        {
            get;
            protected set;
        }

        public string Namespace
        {
            get
            {
                return projectItem.Properties.Item("DefaultNamespace").Value.ToString();
            }
        }

        public ISAHLProject CurrentProject
        {
            get
            {
                return this.project;
            }
        }

        public SAHLProjectItem(ProjectItem projectItem, IContainer container)
        {
            this.projectItem = projectItem;
            this.ItemPath = projectItem.FileNames[0];
            this.ProjectName = projectItem.ContainingProject.Name;
            this.container = container;
            this.project = new SAHLProject(container.GetInstance<IVSServices>(), projectItem.ContainingProject);
        }

        public ISAHLProjectItem GetOrAddFolder(string folderName)
        {
            ProjectItem folderItem = null;
            foreach (ProjectItem subItem in this.projectItem.ProjectItems)
            {
                if (subItem.Name == folderName && subItem.Kind == VSConstants.ItemTypeGuid.PhysicalFolder_string)
                {
                    folderItem = subItem;
                }
            }
            if (folderItem == null)
            {
                folderItem = projectItem.ProjectItems.AddFolder(folderName);
            }
            return new SAHLProjectItem(folderItem, container);
        }

        public ISAHLProjectItem AddFile(string fileName, string fileExtension, string fileContent)
        {
            string fullName = Path.Combine(ItemPath, string.Format("{0}.{1}", fileName, fileExtension));
            using (StreamWriter sw = new StreamWriter(fullName))
            {
                sw.Write(fileContent);
            }
            ProjectItem fileItem = projectItem.ProjectItems.AddFromFile(fullName);
            return new SAHLProjectItem(fileItem, container);
        }
    }
}