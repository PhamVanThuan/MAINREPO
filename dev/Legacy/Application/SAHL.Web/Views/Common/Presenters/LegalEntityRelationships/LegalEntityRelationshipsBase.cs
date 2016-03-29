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
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.Globals;

using SAHL.Common;

namespace SAHL.Web.Views.Common.Presenters.LegalEntityRelationships
{
    /// <summary>
    /// 
    /// </summary>
    public class LegalEntityRelationshipsBase : SAHLCommonBasePresenter<ILegalEntityRelationships>
    {
        private int _selectedLegalEntityKey;
        private CBOMenuNode _cboCurrentNode;
        private ILegalEntityRepository _legalEntityRepository;
        private ILegalEntity _legalEntity;
        private ILegalEntity _relatedLegalEntity;
        private IAddress _address;
        private ILegalEntity _newLegalEntity;
        private ILookupRepository _lookupRepository;

        /// <summary>
        /// Class contructor.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntityRelationshipsBase(ILegalEntityRelationships view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view.OnAddToCBO += new KeyChangedEventHandler(OnAddToCBO);
            _view.OnSubmitButtonClick += new KeyChangedEventHandler(OnSubmitButtonClick);
            _view.OnCancelButtonClick += new EventHandler(OnCancelButtonClick);
            _view.OnGridItemSelected += new KeyChangedEventHandler(OnGridItemSelected);

            _legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
        }

        protected virtual void OnGridItemSelected(object sender, KeyChangedEventArgs e)
        {

        }

        protected virtual void OnCancelButtonClick(object sender, EventArgs e)
        {
            Navigator.Navigate("Cancel");
        }

        protected virtual void OnSubmitButtonClick(object sender, KeyChangedEventArgs e)
        {

        }

        protected bool RelationshipsExists
        {
            get
            {
                return (_legalEntity != null && _legalEntity.LegalEntityRelationships.Count > 0);
            }
        }

        protected ILegalEntity NewLegalEntity
        {
            set { _newLegalEntity = value; }
            get {return _newLegalEntity; }
        }

        protected ILookupRepository LookupRepository
        {
            set { _lookupRepository = value; }
            get { return _lookupRepository; }
        }

        protected virtual void OnAddToCBO(object sender, KeyChangedEventArgs e)
        {
            // add the selected legal entity to the cbo and navigate
            int legalEntityKey = Convert.ToInt32(e.Key);

            // get the top level legal entity static node
            CBOMenuNode topParentNode = CBOManager.GetTopParentCBOMenuNode(_cboCurrentNode);
            bool alreadyAdded = false;

            // do a check to ensure that the legal entity hasn't already been added
            foreach (CBOMenuNode childNode in topParentNode.ChildNodes)
            {
                if (childNode.GenericKey == legalEntityKey)
                {
                    CBOManager.SetCurrentCBONode(_view.CurrentPrincipal, childNode, CBONodeSetType.CBO);
                    alreadyAdded = true;
                    break;
                }
            }

            if (!alreadyAdded)
            {
                ICBOMenu ClientNameTemplate = GetLegalEntityTemplate(topParentNode);
                CBOManager.AddCBOMenuToNode(_view.CurrentPrincipal, topParentNode, ClientNameTemplate, legalEntityKey, GenericKeyTypes.LegalEntity, CBONodeSetType.CBO);
            }

            // navigate to selected node
            base.Navigator.Navigate(CBOManager.GetCurrentCBONode(_view.CurrentPrincipal).URL);
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

        protected int SelectedLegalEntityKey
        {
            get { return _selectedLegalEntityKey; }
            set { _selectedLegalEntityKey = value; }
        }

        protected ILegalEntity LegalEntity
        {
            get { return _legalEntity; }
            set { _legalEntity = value; }
        }

        protected ILegalEntity RelatedLegalEntity
        {
            get { return _relatedLegalEntity; }
            set { _relatedLegalEntity = value; }
        }

        protected IAddress Address
        {
            set { _address = value; }
            get { return _address; }
        }

        protected ILegalEntityRepository LegalEntityRepository
        {
            set { _legalEntityRepository = value; }
            get { return _legalEntityRepository; }
        }

        protected void LoadRelatedLEFromCache()
        {
            if (GlobalCacheData.ContainsKey(ViewConstants.LegalEntity))
                _relatedLegalEntity = (ILegalEntity)GlobalCacheData[ViewConstants.LegalEntity];
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

             // Get the selected LegalEntity from the CBO
            _cboCurrentNode = (CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode);

            if (_cboCurrentNode != null)
            {
                _selectedLegalEntityKey = (int)_cboCurrentNode.GenericKey;

                _legalEntity = _legalEntityRepository.GetLegalEntityByKey(_selectedLegalEntityKey);
                _view.BindRelationshipGrid(_legalEntity.LegalEntityRelationships);
            }
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return; 

            if (RelationshipsExists)
            {
                _view.AddToCBOButtonVisible = true;
                _view.SubmitButtonEnabled = true;
            }
            else
            {
                _view.AddToCBOButtonVisible = false;
                _view.SubmitButtonEnabled = false;
            }
        }

        protected string LabelString
        {
            get
            {
                string label = String.Empty;

                if (_relatedLegalEntity != null)
                {
                    switch ((LegalEntityTypes)_relatedLegalEntity.LegalEntityType.Key)
                    {
                        case LegalEntityTypes.CloseCorporation:
                            label = String.Format("{0} - Registration Number: {1}", _relatedLegalEntity.GetLegalName(LegalNameFormat.Full), ((ILegalEntityCloseCorporation)_relatedLegalEntity).RegistrationNumber);
                            break;
                        case LegalEntityTypes.Company:
                            label = String.Format("{0} - Registration Number: {1}", _relatedLegalEntity.GetLegalName(LegalNameFormat.Full), ((ILegalEntityCompany)_relatedLegalEntity).RegistrationNumber);
                            break;
                        case LegalEntityTypes.NaturalPerson:
                            label = String.Format("{0} - ID Number: {1}", _relatedLegalEntity.GetLegalName(LegalNameFormat.Full), ((ILegalEntityNaturalPerson)_relatedLegalEntity).IDNumber);
                            break;
                        case LegalEntityTypes.Trust:
                            label = String.Format("{0} - Registration Number: {1}", _relatedLegalEntity.GetLegalName(LegalNameFormat.Full), ((ILegalEntityTrust)_relatedLegalEntity).RegistrationNumber);
                            break;
                        case LegalEntityTypes.Unknown:
                            break;
                        default:
                            break;
                    }
                }

                return label;
            }
        }
    }
}
