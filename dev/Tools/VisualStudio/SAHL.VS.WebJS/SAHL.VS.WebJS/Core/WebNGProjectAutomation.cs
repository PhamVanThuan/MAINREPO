using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Project.Automation;
using SAHomeloans.SAHL_VS_WebJS;
using System;
using System.Runtime.InteropServices;

namespace SAHomeloans.SAHL_VS_WebJS.Core
{
    [ComVisible(true)]
    public class OAWebNGProject : OAProject
    {
        #region Constructors

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="project">Custom project.</param>
        public OAWebNGProject(WebNGProjectNode project)
            : base(project)
        {
        }

        #endregion Constructors
    }

    [ComVisible(true)]
    [Guid(GuidList.OAWebNGProjectFileItem)]
    public class OAWebNGProjectFileItem : OAFileItem
    {
        #region Constructors

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="project">Automation project.</param>
        /// <param name="node">Custom file node.</param>
        public OAWebNGProjectFileItem(OAProject project, FileNode node)
            : base(project, node)
        {
        }

        #endregion Constructors
    }
}