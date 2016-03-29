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
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common;



namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class RelatedLegalEntity : SAHLCommonBasePresenter<IRelatedLegalEntity>
    {
        private IEventList<IRole> _legalEntityRoles;
        private CBOMenuNode _cboCurrentNode;
        private ILegalEntityRepository _legalEntityRepository;

        private IAccountRepository _accountRepository;

        public IAccountRepository AccountRepo
        {
            get { return _accountRepository; }
            set { _accountRepository = value; }
        }
	
        /// <summary>
        /// Exposed to allow the presenter to be tested properly.
        /// </summary>
        public IEventList<IRole> LegalEntityRoles
        {
            set { _legalEntityRoles = value; }
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public RelatedLegalEntity(IRelatedLegalEntity view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Hook additional events
            _view.OnSelectLegalEntity += new KeyChangedEventHandler(OnSelectLegalEntity);

            _legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            _accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();

            _view.AllowGridSelect = true;
            _view.AllowGridDoubleClick = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            // get the current node, parent account node and parent legalentity node.
            _cboCurrentNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            // check the type of the parent node
            switch (_cboCurrentNode.ParentNode.GenericKeyTypeKey)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.LegalEntity:
                case (int)SAHL.Common.Globals.GenericKeyTypes.RelatedLegalEntity:
                    ILegalEntity legalEntity = _legalEntityRepository.GetLegalEntityByKey(_cboCurrentNode.ParentNode.GenericKey);
                    _legalEntityRoles = new EventList<IRole>();
                    if (legalEntity != null)
                    {
                        // get the roles for all legal entities for all accounts
                        foreach (IRole role in legalEntity.Roles)
                        {
                            foreach (IRole rr in role.Account.Roles)
	                        {
                                _legalEntityRoles.Add(_view.Messages,rr);		 
	                        }
                        }
                    }
                    break;
                case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
                case (int)SAHL.Common.Globals.GenericKeyTypes.ParentAccount:
                    IAccount account = _accountRepository.GetAccountByKey(_cboCurrentNode.ParentNode.GenericKey);
                    if (account != null)
                        _legalEntityRoles = account.Roles; 
                    break;

                default:
                    break;
            }

            _view.BindLegalEntityGrid(_legalEntityRoles);
        }

        protected void OnSelectLegalEntity(object sender, KeyChangedEventArgs e)
        {
            // add the selected legal entity to the cbo and navigate
            int legalEntityKey = Convert.ToInt32(e.Key);

            CBONodeSetType nodeSet = CBONodeSetType.CBO;

            // get the top level legal entity static node
            CBOMenuNode topParentNode = CBOManager.GetTopParentCBOMenuNode(_cboCurrentNode);

            if (topParentNode is TaskListNode)
            {
                nodeSet = CBONodeSetType.X2;
                // if we are on a workflow case, we need to travers up the tree until be hit the InstanceNode
                // this will then be our "topParentNode"
                topParentNode = GetTopParentInstanceNode(_cboCurrentNode);
            }

            bool alreadyAdded = false;

            // do a check to ensure that the legal entity hasn't already been added
            foreach (CBOMenuNode childNode in topParentNode.ChildNodes)
            {
                if (childNode.GenericKey == legalEntityKey)
                {
                    CBOManager.SetCurrentCBONode(_view.CurrentPrincipal, childNode, nodeSet);

                    alreadyAdded = true;
                    break;
                }
            }

            if (!alreadyAdded)
            {
                ICBOMenu ClientNameTemplate = GetLegalEntityTemplate(topParentNode);
                CBOManager.AddCBOMenuToNode(_view.CurrentPrincipal, topParentNode, ClientNameTemplate, legalEntityKey, GenericKeyTypes.LegalEntity, nodeSet);
            }


            // navigate to selected node
            base.Navigator.Navigate(CBOManager.GetCurrentCBONode(_view.CurrentPrincipal).URL);
        }

        private static CBOMenuNode GetTopParentInstanceNode(CBOMenuNode cboMenuNode)
        {
            CBOMenuNode node = null;
            if (cboMenuNode.ParentNode is TaskListNode)
                node = cboMenuNode;
            else
                node = GetTopParentInstanceNode(cboMenuNode.ParentNode as CBOMenuNode);

            return node;
        }

        private static ICBOMenu GetLegalEntityTemplate(CBOMenuNode topParentNode)
        {
            if (topParentNode is InstanceNode)
            {
                CBONode node = topParentNode as CBONode;
                topParentNode = node.ChildNodes[0] as CBOMenuNode;
                if (topParentNode.CBOMenu.Description == "ClientName")
                    return topParentNode.CBOMenu;
            }
            else
            {
                for (int i = 0; i < topParentNode.CBOMenu.ChildMenus.Count; i++)
                {
                    if (topParentNode.CBOMenu.ChildMenus[i].Description == "ClientName")
                        return topParentNode.CBOMenu.ChildMenus[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;

            base.OnViewPreRender(sender, e);

            if (_legalEntityRoles != null && _legalEntityRoles.Count > 0)
                _view.AddToMenuButtonEnabled = true;
            else
                _view.AddToMenuButtonEnabled = false;

            _view.RemoveButtonEnabled = false;
            _view.CancelButtonEnabled = false;
        }
    }
}
