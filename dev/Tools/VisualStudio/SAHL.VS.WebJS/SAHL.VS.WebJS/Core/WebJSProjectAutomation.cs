using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Project.Automation;
using System;
using System.Runtime.InteropServices;

namespace SAHomeloans.SAHL_VS_WebJS.Core
{
    [ComVisible(true)]
    public class OAWebJSProject : OAProject
    {
        #region Constructors

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="project">Custom project.</param>
        public OAWebJSProject(WebJSProjectNode project)
            : base(project)
        {
        }

        #endregion Constructors
    }

    [ComVisible(true)]
    [Guid(GuidList.OAWebJSProjectFileItem)]
    public class OAWebJSProjectFileItem : OAFileItem
    {
        #region Constructors

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="project">Automation project.</param>
        /// <param name="node">Custom file node.</param>
        public OAWebJSProjectFileItem(OAProject project, FileNode node)
            : base(project, node)
        {
        }

        #endregion Constructors
    }
}