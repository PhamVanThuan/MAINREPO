using System;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.CacheData;
using System.Collections.Generic;
using SAHL.Common.Globals;

using SAHL.Common.UI;

namespace SAHL.Web.Views.Common.Presenters
{
    public class LegalEntityAssetLiabilityBase : SAHLCommonBasePresenter<ILegalEntityAssetLiabilityDetails>
    {
        protected int _genericKey;
        protected CBOMenuNode _node;
        protected ILegalEntityRepository leRepo;
        protected ILegalEntity legalEntity;
        protected ILookupRepository lookups;
        protected IList<ICacheObjectLifeTime> LifeTimes = new List<ICacheObjectLifeTime>();
        protected int gridSelectedIndex;

        public LegalEntityAssetLiabilityBase(ILegalEntityAssetLiabilityDetails view, SAHLCommonBaseController controller)
        : base(view, controller)
    
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            if (_node != null) _genericKey = Convert.ToInt32(_node.GenericKey);

            lookups = RepositoryFactory.GetRepository<ILookupRepository>();

            if (_node != null)
                if (_node.GenericKeyTypeKey == (int)GenericKeyTypes.LegalEntity
                    || _node.GenericKeyTypeKey == (int)GenericKeyTypes.RelatedLegalEntity)
                {
                    leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                    legalEntity = leRepo.GetLegalEntityByKey(_genericKey);
                }

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
        }

        public void SetUpViewForType(int selectedType)
        {
            if (selectedType > 0)
            {
                switch (selectedType)
                {
                    case ((int)AssetLiabilityTypes.FixedProperty):
                        {
                            _view.SetControlsForAddUpdateFixedProperty();
                            break;
                        }
                    case ((int)AssetLiabilityTypes.ListedInvestments):
                        {
                            _view.SetControlsForAddInvestment();
                            break;
                        }
                    case ((int)AssetLiabilityTypes.UnlistedInvestments):
                        {
                            _view.SetControlsForAddInvestment();
                            break;
                        }
                    case ((int)AssetLiabilityTypes.LiabilityLoan):
                        {
                            _view.BindAssetLiabilitySubTypes(lookups.AssetLiabilitySubTypes);
                            _view.SetControlsForAddUpdateLiabilityLoan();
                            break;
                        }
                    case ((int)AssetLiabilityTypes.LiabilitySurety):
                        {
                            _view.SetControlsForAddUpdateLiabilitySurtey();
                            break;
                        }
                    case ((int)AssetLiabilityTypes.LifeAssurance):
                        {
                            _view.SetControlsForAddUpdateLifeAssurance();
                            break;
                        }
                    case ((int)AssetLiabilityTypes.OtherAsset):
                        {
                            _view.SetControlsForAddUpdateOther();
                            break;
                        }
                    case ((int)AssetLiabilityTypes.FixedLongTermInvestment):
                        {
                            _view.SetControlsForAddUpdateFixedLongTermInvestment();
                            break;
                        }
                }
            }
        }

        public void _view_OnCancelButtonClicked (object sender, EventArgs e)
        {
            Navigator.Navigate("Cancel");
        }

    }
}
