using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Administration.Interfaces
{
    public class BindableCBO : IBindableTreeItem
    {
        internal int _key;
        public int Key { get { return _key; } }
        internal string _Desc;
        public string Desc { get { return _Desc; } }
        internal int _ParentKey;
        public int ParentKey { get { return _ParentKey; } }
        internal string _ParentDesc;
        public string ParentDesc { get { return _ParentDesc; } }
        internal List<IBindableTreeItem> _Children = new List<IBindableTreeItem>();
        public List<IBindableTreeItem> Children { get { return _Children; } }
        public BindableCBO() { }
        public BindableCBO(ICBOMenu node, bool LoadChildren)
        {
            Populate(node, LoadChildren);
        }

        private void Populate(ICBOMenu node, bool LoadChildren)
        {
            _key = node.Key;
            _Desc = node.Description;
            if (node.ParentMenu != null)
            {
                _ParentKey = node.ParentMenu.Key;
                _ParentDesc = node.ParentMenu.Description;
            }
            if (LoadChildren)
            {
                foreach (ICBOMenu childNode in node.ChildMenus)
                {
                    _Children.Add(new BindableCBO(childNode, LoadChildren));
                }
            }
        }
    }

    public interface IViewCBOMenu2 : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnFinishClick;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnTreeNodeSelect;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CBONodes"></param>
        /// <param name="TopLevelKey"></param>
        /// <param name="Selected"></param>
        void BindCBOList(List<IBindableTreeItem> CBONodes, int TopLevelKey, int Selected);
    }
}
