var SAHLTreeView_documentLoaded = false;    // flag set to true when the document has finished loading

// function that converts image names to web resource URLs
function SAHLTreeView_getResourceImage(imageName)
{
    var arrExpand = new Array(
        new Array('tree_plus_bottom.gif', '<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_plus_bottom.gif") %>'),
        new Array('tree_plus_middle.gif', '<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_plus_middle.gif") %>'),
        new Array('tree_plus_single.gif', '<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_plus_single.gif") %>'),
        new Array('tree_plus_top.gif', '<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_plus_top.gif") %>')
    );
    var arrContract = new Array(
        new Array('tree_minus_bottom.gif', '<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_minus_bottom.gif") %>'),
        new Array('tree_minus_middle.gif', '<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_minus_middle.gif") %>'),
        new Array('tree_minus_single.gif', '<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_minus_single.gif") %>'),
        new Array('tree_minus_top.gif', '<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_minus_top.gif") %>')
    );
    var arrOther = new Array(
        new Array('tree_blank.gif', '<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_blank.gif") %>'),
        new Array('tree_line_vert.gif', '<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_line_vert.gif") %>'),
        new Array('tree_lines_bottom.gif', '<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_lines_bottom.gif") %>'),
        new Array('tree_lines_middle.gif', '<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_lines_middle.gif") %>'),
        new Array('tree_lines_single.gif', '<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_lines_single.gif") %>'),
        new Array('tree_lines_top.gif', '<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.tree_lines_top.gif") %>')
    );

    for (var i=0; i<arrExpand.length; i++)
    {
        if (imageName == arrExpand[i][0])
            return arrExpand[i][1];
        else if (imageName == arrContract[i][0])
            return arrContract[i][1];
    }
    for (var i=0; i<arrOther.length; i++)
    {
        if (imageName == arrOther[i][0])
            return arrOther[i][1];
    }
    
    return imageName;
}

// sets SAHLTreeView_documentLoaded to true once the document has finished loading - this is necessary to ensure that 
// the correct offsets are returned in IE when working out if scrolling is needed
function SAHLTreeView_loadComplete()
{
    SAHLTreeView_documentLoaded = true;
}
registerEvent(window, 'load', SAHLTreeView_loadComplete);

// scrolls to the selected node in the tree view
function SAHLTreeView_scrollToSelectedNode(treeViewID)
{
    var treeView = document.getElementById(treeViewID);
    
    // if the document has not finished loading, then set a timer for the method again and exit, otherwise 
    // the scrolling doesn't work
    if (!SAHLTreeView_documentLoaded)
    {
        setTimeout('SAHLTreeView_scrollToSelectedNode("' + treeViewID + '")', 500);
        return;
    }
    
    // if the treeview has a y-position of 0 and height of 0 after the document has completed loading, 
    // it's not visible, so exit
    var treeTop = getAbsolutePosition(treeView).y;
    if (treeTop == 0 && treeView.offsetHeight == 0)
        return;   
    
    // find the selected node and scroll it into view if is below the bottom of the scrolling viewport
    var nodes = treeView.getElementsByTagName('a');
    for (var i=0; i<nodes.length; i++)
    {
        var node = nodes[i];
        if (node.className.indexOf('SelectedNode') > -1)
        {
            var nodeTop = getAbsolutePosition(node).y;
            var nodeBottom = nodeTop + node.offsetHeight;
            if (nodeBottom > (treeTop + treeView.offsetHeight))
                treeView.scrollTop = nodeTop - treeTop;
            break;
        }
    }
    
}

// toggles the visibility of a treenode
function SAHLTreeView_toggleNode(srcImage, treeViewID, treeNodeID, treeNodeValuePath, hidExpandedID)
{
    var treeView = document.getElementById(treeViewID);
    var treeNode = document.getElementById(treeNodeID);
    var nodesExpanded = 0;
    var nodes = treeNode.getElementsByTagName('div');
    var expand = (srcImage.alt.indexOf('Expand') > -1);
    
    // go through all divs that are a child of the clicked div, and if they are a direct descendent 
    // then toggle the expand status
    for (var i=0; i<nodes.length; i++) 
    {
        // make sure the node's parent in the tree is the node that was clicked
        if (nodes[i].parentNode != treeNode) continue;
        if (nodes[i].className.indexOf('TreeNode') == -1) continue;
        
        // expand/contract the image
        if (expand)
        {
            nodes[i].style.display = 'inline';
            nodesExpanded++;
        }
        else
        {
            nodes[i].style.display = 'none';
        }
    }

    // update the source image
    srcImage.alt = (expand ? 'Contract node' : 'Expand node');
    var imageName = srcImage.getAttribute('name');
    imageName = (expand ? imageName.replace('plus', 'minus') : imageName.replace('minus', 'plus'));
    srcImage.src = SAHLTreeView_getResourceImage(imageName);
    srcImage.setAttribute('name', imageName);

    // update the set of expanded values
    SAHLTreeView_updateExpanded(hidExpandedID, treeNodeValuePath, expand);
   
}

// maintains the values in the hidden control that is used to check which nodes were expanded
function SAHLTreeView_updateExpanded(hidExpandedID, treeNodeValuePath, expanded)
{
    var hidExpanded = document.getElementById(hidExpandedID);
    var item = '|' + treeNodeValuePath + '|';
    if (expanded)
    {
        if (hidExpanded.value.indexOf(item) == -1)
            hidExpanded.value = hidExpanded.value + item;
    }
    else 
    {
        if (hidExpanded.value.indexOf(item) > -1)
            hidExpanded.value = hidExpanded.value.replace(item, '');
    }
}
