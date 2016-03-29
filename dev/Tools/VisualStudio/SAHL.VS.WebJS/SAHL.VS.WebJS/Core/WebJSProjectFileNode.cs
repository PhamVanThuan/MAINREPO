using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Project.Automation;
using System;

namespace SAHomeloans.SAHL_VS_WebJS.Core
{
    public class WebJSProjectFileNode : FileNode
    {
        #region Fields

        private OAWebJSProjectFileItem automationObject;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MyCustomProjectFileNode"/> class.
        /// </summary>
        /// <param name="root">The project node.</param>
        /// <param name="e">The project element node.</param>
        internal WebJSProjectFileNode(ProjectNode root, ProjectElement e)
            : base(root, e)
        {
        }

        #endregion Constructors

        #region Overriden implementation

        /// <summary>
        /// Gets the automation object for the file node.
        /// </summary>
        /// <returns></returns>
        public override object GetAutomationObject()
        {
            if (automationObject == null)
            {
                automationObject = new OAWebJSProjectFileItem(this.ProjectMgr.GetAutomationObject() as OAProject, this);
            }

            return automationObject;
        }

        #endregion Overriden implementation

        #region Private implementation

        internal OleServiceProvider.ServiceCreatorCallback ServiceCreator
        {
            get { return new OleServiceProvider.ServiceCreatorCallback(this.CreateServices); }
        }

        private object CreateServices(Type serviceType)
        {
            object service = null;
            if (typeof(EnvDTE.ProjectItem) == serviceType)
            {
                service = GetAutomationObject();
            }
            return service;
        }

        #endregion Private implementation
    }
}