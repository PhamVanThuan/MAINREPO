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
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Administration.Interfaces
{

    public class BindOrganisationStructure: IBindableTreeItem
    {

        internal int _Key;
        public int Key { get { return _Key; } }
        internal int _OrganisationTypeKey;
        public int OrganisationTypeKey { get { return _OrganisationTypeKey; } }
        internal int _ParentKey = -1;
        public int ParentKey { get { return _ParentKey; } }
        internal string _ParentDescription;
        public string ParentDescription { get { return _ParentDescription; } }
        internal string _Desc;
        public string Desc { get { return _Desc; } }
        public int _GeneralStatusKey;
        internal int GeneralStatusKey { get { return _GeneralStatusKey; } }
        internal List<IBindableTreeItem> _Children = new List<IBindableTreeItem>();
        public List<IBindableTreeItem> Children { get { return _Children; } }
        internal List<string> _Users = new List<string>();
        public List<string> Users { get { return _Users; } }
        public BindOrganisationStructure() { }
        public BindOrganisationStructure(IOrganisationStructure os, bool LoadLittleBrats)
        {
            Populate(os, LoadLittleBrats);
        }

        public BindOrganisationStructure(IOrganisationStructure os)
        {
            Populate(os, true);
        }

        private void Populate(IOrganisationStructure os, bool LoadLittleBrats)
        {
            this._Key = os.Key;
            this._OrganisationTypeKey = os.OrganisationType.Key;
            if (null != os.Parent)
            {
                this._ParentKey = os.Parent.Key;
                this._ParentDescription = os.Parent.Description;
            }
            this._Desc = os.Description;
            
            this._GeneralStatusKey = os.GeneralStatus.Key;
            if (os.OrganisationType.Key == 7) //  we have a designation, populate.
            {
                DoUsers(os);
            }

            // dont load all the child nodes
            if (!LoadLittleBrats) return;
            if (os.ChildOrganisationStructures.Count > 0)
            {
                foreach (IOrganisationStructure child in os.ChildOrganisationStructures)
                {
                    _Children.Add(new BindOrganisationStructure(child));
                }
            }
        }

        private void DoUsers(IOrganisationStructure os)
        { 
            // if the collection is null, we cannot populate nothing.
            if (os.ADUsers == null) return;
            List<int> ADUserOrgKeys = new List<int>();

            for (int i = 0; i < os.ADUsers.Count; i++)
            {
                if (!ADUserOrgKeys.Contains(os.ADUsers[i].Key))
                {
                    ADUserOrgKeys.Add(os.ADUsers[i].Key);
                    _Children.Add(new BindADUser(os.ADUsers[i], os.Key));
                }
            }
        }
    }
    
    public class BindADUser : IBindableTreeItem
    {

        int _parentKey;
        int _key;
        string _description;        

        public BindADUser(IADUser ADUser, int ParentKey)
        {
            _parentKey = ParentKey;
            _description = ADUser.ADUserName;
            _key = ADUser.Key;
        }

        #region IBindableTreeItem Members

        /// <summary>
        /// Returning an empty list as this kind of tree item should never have children
        /// </summary>
        public List<IBindableTreeItem> Children
        {
            get { return new List<IBindableTreeItem>(); }
        }

        public string Desc
        {
            get { return _description; }
        }

        public int Key
        {
            get { return _key; }
        }

        public int ParentKey
        {
            get { return _parentKey; }
        }

        #endregion
    }

    public interface IViewOrganisationStructure : IViewBase
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
        /// Fires when the clear button is clicked. Is used to nullify the parent object
        /// </summary>
        event EventHandler OnClearClicked;
        /// <summary>
        /// Fires when the user saves the data.
        /// </summary>
        event EventHandler OnSubmitClick;

        /// <summary>
        /// Binds a single ORganisation STructure to be edited.
        /// </summary>
        /// <param name="OS"></param>
        /// <param name="SelectedValue"></param>
        void BindSingleOrganisationStructure(BindOrganisationStructure OS, int SelectedValue);
        /// <summary>
        /// Sets the parentkey in the view of the new OrganisationStructure the user is about to create.
        /// </summary>
        /// <param name="OS"></param>
        /// <param name="SelectedValue"></param>
        void BindParentOrganisationStructure(BindOrganisationStructure OS, int SelectedValue);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="osType"></param>
        void BindLookups(IEventList<IGeneralStatus> status, IEventList<IOrganisationType> osType);
        string Desc { get; }
        int ParentKey { get; }
        int Key { get; }
        int OSType { get; }
        int GeneralStatusKey { get; }
        bool VisibleMaint { set; }
    }
}
