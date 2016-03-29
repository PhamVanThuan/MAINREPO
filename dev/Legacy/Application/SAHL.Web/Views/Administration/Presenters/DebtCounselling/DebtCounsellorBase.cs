using System;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.CacheData;
using SAHL.Web.Views.Administration.Interfaces;
using System.Data;
using SAHL.Common.DomainMessages;
using DevExpress.Web.ASPxTreeList;
using Castle.ActiveRecord;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.UI;
using SAHL.Common;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Administration.Presenters.DebtCounselling
{
    /// <summary>
    ///
    /// </summary>
    public class DebtCounsellorBase : SAHLCommonBasePresenter<IExternalOrganisationStructure>
    {
        protected enum DataTableColumn
        {
            PrimaryKey = 0, //LE OrgStruct Key
            ParentKey,
            OSDescription,
            OSTypeDescription,
            DisplayName,
            OrganisationStructureKey,
            LegalEntityKey
        }
        protected int _osRootKey;
        protected int _selectedOSKey;
        protected int _selectedLegalEntityKey;
        protected string _selectedTreeNodeKey, _selectedTreeNodeParentKey;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public DebtCounsellorBase(IExternalOrganisationStructure view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        #region Events

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

            // Bind the Data
            BindData();

            _view.CancelButtonClicked += new EventHandler(CancelClick);
            _view.AddButtonClicked += new EventHandler(AddClick);
            _view.RemoveButtonClicked += new EventHandler(RemoveClick);
            _view.UpdateButtonClicked += new EventHandler(UpdateClick);
            _view.ViewButtonClicked += new EventHandler(ViewClick);

            _view.TreeNodeDragged += new TreeListNodeDragEventHandler(TreeNodeDragged);
            _view.OnAddToCBO += new EventHandler(OnAddToCBO);

            // Set Button Visibility
            _view.SelectButtonVisible = false;
            _view.CancelButtonVisible = false;

            // Set DragDrop Functionality
            _view.AllowNodeDragging = false;

            // Set TreeView Heading
            _view.TreeViewHeading = "Debt Counsellors";

            // Set "Add to CBO" button visibility
            _view.AllowAddToCBO = true;

            // check if we have stuff in the globalcache as a result of an add/update/view or remove of a node
            // if we do the we must expand the treelist to the correct node
            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedTreeNodeKey))
            {
                _view.SelectedNodeKey = GlobalCacheData[ViewConstants.SelectedTreeNodeKey].ToString();
                GlobalCacheData.Remove(ViewConstants.SelectedTreeNodeKey);
            }
            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedTreeNodeParentKey))
            {
                _view.SelectedNodeParentKey = GlobalCacheData[ViewConstants.SelectedTreeNodeParentKey].ToString();
                GlobalCacheData.Remove(ViewConstants.SelectedTreeNodeParentKey);
            }
        }


        private void BindData()
        {
            IControl ct = CTRepo.GetControlByDescription("DebtCounsellorRoot");
            _osRootKey = Convert.ToInt32(ct.ControlNumeric.Value);

            DataSet orgStructListDS = OSRepo.GetOrganisationStructureAllDSForKey(_osRootKey);
            _view.BindOrganisationStructure(orgStructListDS);
        }

        void TreeNodeDragged(object sender, TreeListNodeDragEventArgs e)
        {

            TransactionScope txn = new TransactionScope();

            try
            {
                DataRowView drCurrentNode = e.Node.DataItem as DataRowView;
                DataRowView drTargetNode = e.NewParentNode.DataItem as DataRowView;

                int currentOrgStructureKey = Convert.ToInt32(drCurrentNode.Row["OrganisationStructureKey"]);
                int currentLegalEntityKey = Convert.ToInt32(drCurrentNode.Row["LegalEntityKey"]);
                int targetOrgStructureKey = Convert.ToInt32(drTargetNode.Row["OrganisationStructureKey"]);

                if (currentOrgStructureKey > 0 && targetOrgStructureKey > 0)
                {
                    // Add exclusion set
                    this.ExclusionSets.Add(RuleExclusionSets.DebtCounsellorDragDrop);

                    // current node details
                    IDebtCounsellorOrganisationNode currentDebtCounsellorOrganisationNode = DCRepo.GetDebtCounsellorOrganisationNodeForKey(currentOrgStructureKey);
                    ILegalEntity currentLegalEntity = LegalEntityRepo.GetLegalEntityByKey(currentLegalEntityKey);

                    //Execute the rule
                    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                    IRuleService svc = ServiceFactory.GetService<IRuleService>();
                    svc.ExecuteRule(spc.DomainMessages, "DebtCounsellingOpenCasesExist", currentLegalEntity);

                    if (_view.IsValid)
                    {
                        // target node details                    
                        IDebtCounsellorOrganisationNode targetDebtCounsellorOrganisationNode = DCRepo.GetDebtCounsellorOrganisationNodeForKey(targetOrgStructureKey);
                        // do the move
                        IDebtCounsellorOrganisationNode debtCounsellorOrganisationNode = currentDebtCounsellorOrganisationNode.MoveMe(targetDebtCounsellorOrganisationNode, currentLegalEntity);
                        //save the new org structure
                        DCRepo.SaveDebtCounsellorOrganisationStructure(debtCounsellorOrganisationNode);
                        this.ExclusionSets.Remove(RuleExclusionSets.DebtCounsellorDragDrop);
                        txn.VoteCommit();
                        // rebind the data
                        BindData();
                        
                    }
                    //e.Cancel = true;
                }
            }
            catch (Exception)
            {
                txn.VoteRollBack();

                if (_view.IsValid)
                    throw;
            }
            finally
            {
                txn.Dispose();

            }

            //e.Cancel = false;
            e.Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            if (!_view.ShouldRunPage)
                return;

        }

        protected virtual void CancelClick(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", Justification = "New rule added to FxCop, avoiding refactor")]
        protected void AddClick(object sender, EventArgs e)
        {
            //need to set up a cached variable for the selected OSKey to add the item to
            GlobalCacheData.Clear();

            Int32.TryParse(GetSelectedItemValue(DataTableColumn.OrganisationStructureKey), out _selectedOSKey);
            _selectedTreeNodeKey = GetSelectedItemValue(DataTableColumn.PrimaryKey);

            if (_selectedOSKey > 0)
            {
                GlobalCacheData.Add(ViewConstants.ParentOrganisationStructureKey, _selectedOSKey, LifeTimes);
                GlobalCacheData.Add(ViewConstants.SelectedTreeNodeKey, _selectedTreeNodeKey, LifeTimes);
                _view.Navigator.Navigate("Add");
            }
            else
            {
                _view.Messages.Add(new Error("No item selected to add to.", "No item selected to add to."));
            }
        }

        protected void UpdateClick(object sender, EventArgs e)
        {
            ProcessSelectedLegalEntity("Update");
        }

        protected void RemoveClick(object sender, EventArgs e)
        {
            ProcessSelectedLegalEntity("Remove");
        }

        protected void ViewClick(object sender, EventArgs e)
        {
            ProcessSelectedLegalEntity("View");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", Justification = "New rule added to FxCop, avoiding refactor")]
        private void ProcessSelectedLegalEntity(string viewName)
        {
            //need to set up a cached variable for the LE Key
            GlobalCacheData.Clear();

            _selectedTreeNodeKey = GetSelectedItemValue(DataTableColumn.PrimaryKey);
            _selectedTreeNodeParentKey = GetSelectedItemValue(DataTableColumn.ParentKey);

            Int32.TryParse(GetSelectedItemValue(DataTableColumn.LegalEntityKey), out _selectedLegalEntityKey);
            Int32.TryParse(GetSelectedItemValue(DataTableColumn.OrganisationStructureKey), out _selectedOSKey);

            if (_selectedLegalEntityKey > 0 && _selectedOSKey > 0)
            {
                GlobalCacheData.Add(ViewConstants.LegalEntityKey, _selectedLegalEntityKey, LifeTimes);
                GlobalCacheData.Add(ViewConstants.OrganisationStructureKey, _selectedOSKey, LifeTimes);

                GlobalCacheData.Add(ViewConstants.SelectedTreeNodeParentKey, _selectedTreeNodeParentKey, LifeTimes);
                GlobalCacheData.Add(ViewConstants.SelectedTreeNodeKey, _selectedTreeNodeKey, LifeTimes);

                _view.Navigator.Navigate(viewName);
            }
            else
            {
                _view.Messages.Add(new Error("No item selected.", "No item selected."));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", Justification = "New rule added to FxCop, avoiding refactor")]
        protected virtual void OnAddToCBO(object sender, EventArgs e)
        {
            // add the selected legal entity to the cbo and navigate
            Int32.TryParse(GetSelectedItemValue(DataTableColumn.LegalEntityKey), out _selectedLegalEntityKey);

            if (_selectedLegalEntityKey <= 0)
                return;

            // get the top level legal entity static node
            CBOMenuNode topParentNode = CBOManager.GetCBOMenuNodeByUrl(_view.CurrentPrincipal, "ClientSuperSearch", CBONodeSetType.CBO);
            bool alreadyAdded = false;

            // do a check to ensure that the legal entity hasn't already been added
            foreach (CBOMenuNode childNode in topParentNode.ChildNodes)
            {
                if (childNode.GenericKey == _selectedLegalEntityKey)
                {
                    CBOManager.SetCurrentCBONode(_view.CurrentPrincipal, childNode, CBONodeSetType.CBO);
                    alreadyAdded = true;
                    break;
                }
            }

            if (!alreadyAdded)
            {
                ICBOMenu ClientNameTemplate = GetLegalEntityTemplate(topParentNode);
                CBOManager.AddCBOMenuToNode(_view.CurrentPrincipal, topParentNode, ClientNameTemplate, _selectedLegalEntityKey, GenericKeyTypes.LegalEntity, CBONodeSetType.CBO);
            }

            // navigate to selected node
            //base.Navigator.Navigate(CBOManager.GetCurrentCBONode(_view.CurrentPrincipal).URL);
        }

        private static ICBOMenu GetLegalEntityTemplate(CBOMenuNode LegalEntitiesNode)
        {
            for (int i = 0; i < LegalEntitiesNode.CBOMenu.ChildMenus.Count; i++)
            {
                if (LegalEntitiesNode.CBOMenu.ChildMenus[i].Description == "ClientName")
                    return LegalEntitiesNode.CBOMenu.ChildMenus[i];
            }
            return null;
        }
        #endregion

        #region Methods

        protected string GetSelectedItemValue(DataTableColumn col)
        {
            DataRow dr = _view.GetFocusedNode;

            if (dr != null)
                return dr[(int)col].ToString();

            return null;
        }

        #endregion

        #region Properties

        private IControlRepository _ctRepo;

        public IControlRepository CTRepo
        {
            get
            {
                if (_ctRepo == null)
                    _ctRepo = RepositoryFactory.GetRepository<IControlRepository>();

                return _ctRepo;
            }
        }


        private IOrganisationStructureRepository _osRepo;
            
        public IOrganisationStructureRepository OSRepo
        {
            get
            {
                if (_osRepo == null)
                    _osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

                return _osRepo;
            }
        }

        private IDebtCounsellingRepository _dcRepo;

        public IDebtCounsellingRepository DCRepo
        {
            get
            {
                if (_dcRepo == null)
					_dcRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                return _dcRepo;
            }
        }

        private ILegalEntityRepository _legalEntityRepo;

        public ILegalEntityRepository LegalEntityRepo
        {
            get
            {
                if (_legalEntityRepo == null)
                    _legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

                return _legalEntityRepo;
            }
        }

        private IList<ICacheObjectLifeTime> _lifeTimes;

        public IList<ICacheObjectLifeTime> LifeTimes
        {
            get
            {
                if (_lifeTimes == null)
                    _lifeTimes = new List<ICacheObjectLifeTime>();

                return _lifeTimes;
            }
        }

        #endregion

    }
}
