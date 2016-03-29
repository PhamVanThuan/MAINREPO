using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Security.Principal;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Security;
using SAHL.Common.UI;
using SAHL.Common;
using SAHL.Common.Collections.Interfaces;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.BusinessModel.Interfaces.UI;

namespace SAHL.Common.Service.Interfaces
{
    public interface ICBOService
    {

        void RefreshInstanceNode(SAHLPrincipal Principal);
        //void RefreshInstanceNode(SAHLPrincipal Principal, CBONode CurrentNode);
        //void AddPropertyNode(SAHLPrincipal Principal, CBONode Node, IProperty property);
        //void AddApplicantNode(SAHLPrincipal Principal, CBONode Node, IApplicationRole Role);

        /// <summary>
        /// Retrieves the current list of CBONodes for the current <see cref="SAHLPrincipal">SAHL security principal</see>.
        /// </summary>
        /// <param name="Principal">The current <see cref="SAHLPrincipal">SAHL security principal</see>.</param>
        /// <param name="NodeSetName">The current NodeSetName.</param>
        /// <returns>The list of <see cref="CBONode">CBONodes</see>.</returns>
        IEventList<CBONode> GetMenuNodes(SAHLPrincipal Principal, MenuNodeSet NodeSetName);

        void RefreshWorkFlowNodes(SAHLPrincipal Principal);

        /// <summary>
        /// Retrieves a list of <see cref="IContextMenu">ContextMenus</see> for the currently selected <see cref="CBONode">CBONode</see>.
        /// </summary>
        /// <param name="Principal">The current <see cref="SAHLPrincipal">SAHL security principal</see>.</param>
        /// <param name="NodeSetName">The current NodeSetName.</param>
        /// <returns>The list of <see cref="IContextMenu">ContextMenus</see>.</returns>
        IEventList<CBONode> GetContextNodes(SAHLPrincipal Principal, MenuNodeSet NodeSetName);

        /// <summary>
        /// Retrieves the currently selected <see cref="CBONode">CBONode</see> from the current list of CBONodes for the current <see cref="SAHLPrincipal">SAHL security principal</see>.
        /// </summary>
        /// <param name="Principal">The current <see cref="SAHLPrincipal">SAHL security principal</see>.</param>
        /// <returns>The currently selected <see cref="CBONode">CBONode</see>.</returns>
        CBONode GetCurrentCBONode(SAHLPrincipal Principal);

        /// <summary>
        /// Retrieves the currently selected <see cref="CBONode">CBONode</see> from the current list of CBONodes for the current <see cref="SAHLPrincipal">SAHL security principal</see>.
        /// </summary>
        /// <param name="Principal">The current <see cref="SAHLPrincipal">SAHL security principal</see>.</param>
        /// <param name="NodeSetName">The current NodeSetName.</param>
        /// <returns>The currently selected <see cref="CBONode">CBONode</see>.</returns>
        CBONode GetCurrentCBONode(SAHLPrincipal Principal, MenuNodeSet NodeSetName);
        
        /// <summary>
        /// Retrieves the top parent <see cref="CBOMenuNode">CBOMenuNode</see> from the specified node.
        /// </summary>
        /// <param name="cboMenuNode">The specified <see cref="CBOMenuNode">CBOMenuNode</see>..</param>
        /// <returns>The top level <see cref="CBOMenuNode">CBOMenuNode</see>.</returns>
        CBOMenuNode GetTopParentCBOMenuNode(CBOMenuNode cboMenuNode);
        /// <summary>
        /// Retrieves the currently selected <see cref="IContextMenu">ContextMenu</see> for the selected <see cref="CBONode">CBONode</see> for the current <see cref="SAHLPrincipal">SAHL security principal</see>.
        /// </summary>
        /// <param name="Principal">The current <see cref="SAHLPrincipal">SAHL security principal</see>.</param>
        /// <param name="NodeSetName">The current NodeSetName.</param>
        /// <returns>The currently selected <see cref="IContextMenu">ContextMenu</see>.</returns>
        CBOContextNode GetCurrentContextNode(SAHLPrincipal Principal, MenuNodeSet NodeSetName);

        /// <summary>
        /// Sets the currently selected <see cref="CBONode">CBONode</see> in the current list of CBONodes for the current <see cref="SAHLPrincipal">SAHL security principal</see>.
        /// </summary>
        /// <param name="Principal">The current <see cref="SAHLPrincipal">SAHL security principal</see>.</param>
        /// <param name="NodeSetName">The current NodeSetName.</param>
        /// <param name="Node">The <see cref="CBONode">CBONode</see> to select.</param>
        void SetCurrentCBONode(SAHLPrincipal Principal, CBOMenuNode Node, MenuNodeSet NodeSetName);

        /// <summary>
        /// Sets the currently selected <see cref="IContextMenu">ContextMenu</see> for the selected <see cref="CBONode">CBONode</see> for the current <see cref="SAHLPrincipal">SAHL security principal</see>.
        /// </summary>
        /// <param name="Principal">The current <see cref="SAHLPrincipal">SAHL security principal</see>.</param>
        /// <param name="Node">The CBOContextNode to set.</param>
        /// <param name="NodeSetName">The current NodeSetName.</param>
        void SetCurrentContextNode(SAHLPrincipal Principal, CBOContextNode Node, MenuNodeSet NodeSetName);

        void ClearContextNodes(SAHLPrincipal Principal, MenuNodeSet NodeSetName);

        /// <summary>
        /// Inserts a new node into the CBO menu 
        /// </summary>
        /// <param name="Principal">The current <see cref="SAHLPrincipal">SAHL security principal</see>.</param>
        /// <param name="ParentNode">The parent node to which the new node will be added as a child. Null to add as a root node.</param>
        /// <param name="NewNode">The node to insert</param>
        /// <param name="NodeSetName"></param>
        void AddCBOMenuNode(SAHLPrincipal Principal, CBONode ParentNode, CBONode NewNode, MenuNodeSet NodeSetName);

        /// <summary>
        /// Adds a CBOMenu to the specified cbo node
        /// </summary>
        /// <param name="Principal">The current <see cref="SAHLPrincipal">SAHL security principal</see>.</param>
        /// <param name="ParentNode">The parent node to which the new node will be added as a child. Null to add as a root node.</param>
        /// <param name="MenuTemplate">The <see cref="ICBOMenu">CoreBusinessObjectMenu</see> to use to base the <see cref="CBONode">CBONode</see> on. 
        /// If the <see cref="ICBOMenu">CoreBusinessObjectMenu</see> represents a dynamic node the childs nodes will be built automatically.</param>
        /// <param name="GenericKey">The GenericKey value.</param>
        /// <param name="GenericKeyType">The GenericKeyType enumerator value.</param>
        /// <param name="NodeSetName"></param>
        void AddCBOMenuToNode(SAHLPrincipal Principal, CBOMenuNode ParentNode, ICBOMenu MenuTemplate, Int64 GenericKey, SAHL.Common.Globals.GenericKeyTypes GenericKeyType, MenuNodeSet NodeSetName);

        /// <summary>
        /// Adds a node to the current <see cref="SAHLPrincipal">SAHL security principal's</see> list of nodes.
        /// </summary>
        /// <param name="Principal">The current <see cref="SAHLPrincipal">SAHL security principal</see>.</param>
        /// <param name="MenuTemplate">The <see cref="ICBOMenu">CoreBusinessObjectMenu</see> to use to base the <see cref="CBONode">CBONode</see> on. 
        /// If the <see cref="ICBOMenu">CoreBusinessObjectMenu</see> represents a dynamic node the childs nodes will be built automatically.</param>
        /// <param name="GenericKey">The GenericKey value.</param>
        /// <param name="NodeSetName"></param>
        void AddCBOMenuNodeToSelection(SAHLPrincipal Principal, ICBOMenu MenuTemplate, Int64 GenericKey, MenuNodeSet NodeSetName);

        /// <summary>
        /// Adds nodes based on a Corebuinessobjectmenu template to a collection.
        /// </summary>
        /// <param name="Principal"></param>
        /// <param name="Nodes"></param>
        /// <param name="MenuTemplate"></param>
        /// <param name="GenericKey"></param>
        /// <param name="NodeSetName"></param>
        void AddCBOMenuNodeFromTemplate(SAHLPrincipal Principal, IEventList<CBONode> Nodes, ICBOMenu MenuTemplate, long GenericKey, MenuNodeSet NodeSetName);

        /// <summary>
        /// Removes a <see cref="CBONode">CBONode</see> from the current list of CBONodes for the current <see cref="SAHLPrincipal">SAHL security principal</see>.
        /// </summary>
        /// <param name="Principal">The current <see cref="SAHLPrincipal">SAHL security principal</see>.</param>
        /// <param name="CBOUniqueKey">The CBONodeKey of the <see cref="CBONode">Node</see> to remove.</param>
        /// <param name="NodeSetName"></param>
        void RemoveCBOMenuNode(SAHLPrincipal Principal, string CBOUniqueKey, MenuNodeSet NodeSetName);
        //void RemoveCBOMenuNode(IDomainMessageCollection Messages, SAHLPrincipal Principal, int CBONodeKey, MenuNodeSet NodeSetName);

        /// <summary>
        /// Get a CBONode given the key
        /// </summary>
        /// <param name="Principal"></param>
        /// <param name="CBOUniqueKey"></param>
        /// <param name="NodeSetName"></param>
        /// <returns></returns>
        CBOMenuNode GetCBOMenuNodeByKey(SAHLPrincipal Principal, string CBOUniqueKey, MenuNodeSet NodeSetName);

        CBOContextNode GetCBOContextNodeByKey(SAHLPrincipal Principal, string CBOUniqueKey, MenuNodeSet NodeSetName);

        /// <summary>
        /// Gets the first CBONode in the node set with the URL matching <c>url</c>.
        /// </summary>
        /// <param name="principal">The current SAHLPrincipal.</param>
        /// <param name="url">The UIP url.</param>
        /// <param name="nodeSetName">Which nodeset to check.</param>
        /// <returns></returns>
        CBOMenuNode GetCBOMenuNodeByUrl(SAHLPrincipal principal, string url, MenuNodeSet NodeSetName);

        /// <summary>
        /// Sets the current Node Set
        /// </summary>
        /// <param name="Principal"></param>
        /// <param name="NodeSetName"></param>
        /// <returns></returns>
        bool SetCurrentNodeSet(SAHLPrincipal Principal, MenuNodeSet NodeSetName);

        /// <summary>
        /// Retrieves the current NodeSetName
        /// </summary>
        /// <param name="Principal"></param>
        /// <returns></returns>
        MenuNodeSet GetCurrentNodeSetName(SAHLPrincipal Principal);

        /// <summary>
        /// Retrieves a value which is incremented each time the list of menus is changed, this aids in knowing when a change has occurred.
        /// </summary>
        /// <param name="Principal">The current <see cref="SAHLPrincipal">SAHL security principal</see>.</param>
        /// <returns></returns>
        int GetMenuVersion(SAHLPrincipal Principal);

        /// <summary>
        /// Iterates up the tree to check for the existance of the specified parent node type
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="currentNode"></param>
        /// <param name="parentNodeType">eg : typeof(InstanceNode)</param>
        /// <returns></returns>
        bool CheckForParentNodeByType(SAHLPrincipal principal, CBONode currentNode, Type parentNodeType);
    }
}
