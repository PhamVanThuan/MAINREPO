using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using System.Collections.Generic;

namespace SAHL.Web.Views.Administration.Interfaces
{
    public interface IViewMapUsersToOrgStructure : IViewBase
    {
        /// <summary>
        /// Binds a list of ORganisationSTructures to a tree view. The TopLevel key is the the parentkey value that the
        /// view will accept as top level tree nodes.
        /// </summary>
        /// <param name="OrganisationStructure"></param>
        /// <param name="TopLevelKey"></param>
        void BindOrganisationStructure(List<IBindableTreeItem> OrganisationStructure, int TopLevelKey);
        /// <summary>
        /// Fires when a node in the tree is seleced
        /// </summary>
        event EventHandler OnTreeNodeSeleced;
        /// <summary>
        /// Sets the add remove section of the page to visible / not
        /// </summary>
        bool VisibleMaint { set; }
        /// <summary>
        /// Sets the add remove buttons to be visible or not
        /// </summary>
        bool VisibleButtons { set; }
        /// <summary>
        /// Fires when an ADUser is added to an org structure
        /// </summary>
        event EventHandler OnAddClick;
        /// <summary>
        /// Fires when an ADUser is removed from an organisation structure
        /// </summary>
        event EventHandler OnRemoveClick;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="In"></param>
        /// <param name="NotIn"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        void BindMapping(List<ADUserBind> In, List<ADUserBind> NotIn);
    }
}
