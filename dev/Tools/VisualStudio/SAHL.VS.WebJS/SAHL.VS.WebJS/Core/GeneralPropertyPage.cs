using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Shell.Interop;
using SAHomeloans.SAHL_VS_WebJS.Core.Attributes;
using SAHomeloans.SAHL_VS_WebJSTemplate;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SAHomeloans.SAHL_VS_WebJS.Core
{
    /// <summary>
    /// This class implements general property page for the project type.
    /// </summary>
    [ComVisible(true)]
    [Guid(GuidList.GeneralPropertyPage)]
    public class GeneralPropertyPage : SettingsPage
    {
        #region Fields

        private RootDirectoryHelper dirHelper;

        private string assemblyName;
        private string sahlSourceRoot;
        private string defaultNamespace;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Explicitly defined default constructor.
        /// </summary>
        public GeneralPropertyPage()
        {
            dirHelper
                = new RootDirectoryHelper();
            this.Name = Resource.GetString(Resource.GeneralCaption);
        }

        #endregion Constructors

        #region Properties

        [ResourcesCategory(Resource.Application)]
        [LocalDisplayName(Resource.DefaultNamespace)]
        [ResourcesDescription(Resource.DefaultNamespaceDescription)]
        /// <summary>
        /// Gets or sets Default Namespace.
        /// </summary>
        /// <remarks>IsDirty flag was switched to true.</remarks>
        public string DefaultNamespace
        {
            get { return this.defaultNamespace; }
            set { this.defaultNamespace = value; this.IsDirty = true; }
        }

        [ResourcesCategory(Resource.Application)]
        [LocalDisplayName("SAHLSourceRoot")]
        [ResourcesDescription("SAHL Source Root")]
        /// <summary>
        /// Gets or sets Default Namespace.
        /// </summary>
        /// <remarks>IsDirty flag was switched to true.</remarks>
        public string SAHLSourceRoot
        {
            get { return this.sahlSourceRoot; }
            set { this.sahlSourceRoot = value; this.IsDirty = true; }
        }

        [ResourcesCategory(Resource.Project)]
        [LocalDisplayName(Resource.ProjectFile)]
        [ResourcesDescription(Resource.ProjectFileDescription)]
        /// <summary>
        /// Gets the path to the project file.
        /// </summary>
        /// <remarks>IsDirty flag was switched to true.</remarks>
        public string ProjectFile
        {
            get { return Path.GetFileName(this.ProjectMgr.ProjectFile); }
        }

        [ResourcesCategory(Resource.Project)]
        [LocalDisplayName(Resource.ProjectFolder)]
        [ResourcesDescription(Resource.ProjectFolderDescription)]
        /// <summary>
        /// Gets the path to the project folder.
        /// </summary>
        /// <remarks>IsDirty flag was switched to true.</remarks>
        public string ProjectFolder
        {
            get { return Path.GetDirectoryName(this.ProjectMgr.ProjectFolder); }
        }

        #endregion Properties

        #region Overriden Implementation

        /// <summary>
        /// Returns class FullName property value.
        /// </summary>
        public override string GetClassName()
        {
            return this.GetType().FullName;
        }

        /// <summary>
        /// Bind properties.
        /// </summary>
        protected override void BindProperties()
        {
            if (this.ProjectMgr == null)
            {
                return;
            }

            this.assemblyName = this.ProjectMgr.GetProjectProperty("AssemblyName", true);
            string outputType = this.ProjectMgr.GetProjectProperty("OutputType", false);
            this.defaultNamespace = this.ProjectMgr.GetProjectProperty("RootNamespace", false);
            this.sahlSourceRoot = this.ProjectMgr.GetProjectProperty("SAHLSourceRoot", false);

            if (string.IsNullOrEmpty(sahlSourceRoot))
            {
                this.sahlSourceRoot = dirHelper.GetSourceRoot(this.ProjectMgr.ProjectFolder);
                this.ApplyChanges();
                this.Apply();
            }
        }

        /// <summary>
        /// Apply Changes on project node.
        /// </summary>
        /// <returns>E_INVALIDARG if internal ProjectMgr is null, otherwise applies changes and return S_OK.</returns>
        protected override int ApplyChanges()
        {
            if (this.ProjectMgr == null)
            {
                return VSConstants.E_INVALIDARG;
            }

            IVsPropertyPageFrame propertyPageFrame = (IVsPropertyPageFrame)this.ProjectMgr.Site.GetService((typeof(SVsPropertyPageFrame)));
            bool reloadRequired = false;

            this.ProjectMgr.SetProjectProperty("AssemblyName", this.assemblyName);
            this.ProjectMgr.SetProjectProperty("RootNamespace", this.defaultNamespace);
            this.ProjectMgr.SetProjectProperty("SAHLSourceRoot", this.sahlSourceRoot);

            if (reloadRequired)
            {
                if (MessageBox.Show(SR.GetString(SR.ReloadPromptOnTargetFxChanged), SR.GetString(SR.ReloadPromptOnTargetFxChangedCaption), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                }
            }

            this.IsDirty = false;

            if (reloadRequired)
            {
                // This prevents the property page from displaying bad data from the zombied (unloaded) project
                propertyPageFrame.HideFrame();
                propertyPageFrame.ShowFrame(this.GetType().GUID);
            }

            return VSConstants.S_OK;
        }

        #endregion Overriden Implementation
    }
}