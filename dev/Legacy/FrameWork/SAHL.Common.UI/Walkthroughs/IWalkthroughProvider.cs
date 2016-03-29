using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.Common.UI.Walkthroughs
{
    public interface IWalkthroughProvider
    {
        // properties

        /// <summary>
        /// Determines whether the provider should be applied to sub cbo nodes.
        /// </summary>
        bool ApplyToChildren { get;}

        /// <summary>
        /// Determines whether the provider should be applied to parent cbo node.
        /// </summary>
        bool ApplyToParent { get;}
        
        /// <summary>
        /// Gets a list of walkthroughitems that will be displayed.
        /// </summary>
        IList<IWalkthroughItem> Items { get;}

        // methods
        /// <summary>
        /// Called when a walkthroughitem is clicked.
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        string ItemClicked(IWalkthroughItem Item);
    }
}
