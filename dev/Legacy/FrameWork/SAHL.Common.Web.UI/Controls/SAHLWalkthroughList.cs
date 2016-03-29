using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SAHL.Common.UI.Walkthroughs;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;


namespace SAHL.Common.Web.UI.Controls
{
    public class SAHLWalkthroughList : SAHLWebControl
    {
        List<IWalkthroughItem> _beforeItems = new List<IWalkthroughItem>();
        List<IWalkthroughItem> _afterItems = new List<IWalkthroughItem>();
        List<IWalkthroughItem> _allItems = new List<IWalkthroughItem>();

        public IList<IWalkthroughItem> BeforeItems
        {
            get
            {
                return _beforeItems;
            }
        }

        public IList<IWalkthroughItem> AfterItems
        {
            get
            {
                return _afterItems;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // get the items we will render

        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            base.Render(writer);

            IViewBase view = this.Page as IViewBase;
            if (view != null)
            {

                // get the items from the cbo service
                //ICBOService CBO = ServiceFactory.GetService<ICBOService>();
                CBOManager CBOManager = new CBOManager();

                CBONodeSetType CurrentNodeSetName = CBOManager.GetCurrentNodeSetName(view.CurrentPrincipal);
                CBONode Node = CBOManager.GetCurrentCBONode(view.CurrentPrincipal, CurrentNodeSetName);
                if (Node == null)
                    return;

                IWalkthroughProvider Provider = null;
                Provider = GetChildWalkthroughProvider(Node);
                if(Provider == null)
                    Provider = GetWalkthroughProvider(Node);
                if (Provider != null)
                {
                    _allItems.Clear();
                    _allItems.AddRange(_beforeItems);
                    _allItems.AddRange(Provider.Items);
                    _allItems.AddRange(_afterItems);
                    // render the items
                    // create a table and add a cell for each item
                    Table T = new Table();
                    TableRow TR = new TableRow();
                    T.Rows.Add(TR);
                    for (int i = 0; i < _allItems.Count; i++)
                    {
                        IWalkthroughItem Item = _allItems[i];
                        if (Item.Visible == true)
                        {
                            TableCell TC = new TableCell();
                            TR.Cells.Add(TC);
                            //Image I = new Image();
                            //string ImgBase = I.ResolveUrl(@"~\Images\");
                            //if (Item.Enabled)
                            //    I.ImageUrl = ImgBase + Item.Image;
                            //else
                            //    I.ImageUrl = ImgBase + Item.DisabledImage;

                            //TC.Controls.Add(I);

                            Button B = new Button();
                            B.Text = Item.Caption;
                            TC.Controls.Add(B);

                        }
                    }

                    T.RenderControl(writer);
                }
            }
        }

        private IWalkthroughProvider GetChildWalkthroughProvider(CBONode Node)
        {
            for (int i = 0; i < Node.ChildNodes.Count; i++)
            {
                IWalkthroughProvider WP = Node.ChildNodes[i] as IWalkthroughProvider;
                if (WP != null)
                {
                    if(WP.ApplyToParent)
                        return WP;
                }
            }
            return null;
        }

        private IWalkthroughProvider GetWalkthroughProvider(CBONode Node)
        {
            IWalkthroughProvider WP = Node as IWalkthroughProvider;
            if (WP != null)
                return WP;
            CBONode N = Node.ParentNode;
            if (N == null)
                return null;
            if (Node.ParentNode is IWalkthroughProvider)
                return Node.ParentNode as IWalkthroughProvider;
            else
            {
                return GetWalkthroughProvider(Node.ParentNode);
            }
        }
    }
}
