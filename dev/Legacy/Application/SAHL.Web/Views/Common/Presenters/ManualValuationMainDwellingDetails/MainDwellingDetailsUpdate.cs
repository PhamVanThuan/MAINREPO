using System;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System.Collections.Generic;
using SAHL.Common.CacheData;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.Factories;


namespace SAHL.Web.Views.Common.Presenters.ManualValuationMainDwellingDetails
{
    public class MainDwellingDetailsUpdate : SAHLCommonBasePresenter<IValuationManualPropertyDetailsView>
    {
        IEventList<IProperty> _properties;
        protected ILookupRepository lookups;
        protected IPropertyRepository propRepo;
        protected ICommonRepository commonRepo;
        protected IList<ICacheObjectLifeTime> LifeTimes;
        protected IValuationDiscriminatedSAHLManual valManual;
        protected IValuationMainBuilding valMainBuilding;
        protected IValuationCombinedThatch valCombinedThatch;
        protected IValuationCottage valCottage; 
        protected int valuationKey;

        public MainDwellingDetailsUpdate(IValuationManualPropertyDetailsView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            
            if (!View.ShouldRunPage) 
                return;

            if (_view.IsMenuPostBack)
                ClearGlobalCache();

            propRepo = RepositoryFactory.GetRepository<IPropertyRepository>();
            lookups = RepositoryFactory.GetRepository<ILookupRepository>();
            commonRepo = RepositoryFactory.GetRepository<ICommonRepository>();

            if (GlobalCacheData.ContainsKey(ViewConstants.Properties))
                _properties = GlobalCacheData[ViewConstants.Properties] as IEventList<IProperty>;

            // get the origional valuation from the database
            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedValuationKey))
            {
                // get the details from the database
                valuationKey = Convert.ToInt32(GlobalCacheData[ViewConstants.SelectedValuationKey]);
                valManual = propRepo.GetValuationByKey(valuationKey) as IValuationDiscriminatedSAHLManual;
                valMainBuilding = valManual.ValuationMainBuilding;
                valCottage = valManual.ValuationCottage;
            }

            // if we have made changes then get them from cache
            if (GlobalCacheData.ContainsKey(ViewConstants.ValuationClassification))
                valManual.ValuationClassification = GlobalCacheData[ViewConstants.ValuationClassification] as IValuationClassification;
            if (GlobalCacheData.ContainsKey(ViewConstants.ValuationEscalationPercentage))
                valManual.ValuationEscalationPercentage = Convert.ToDouble(GlobalCacheData[ViewConstants.ValuationEscalationPercentage]);
            
            if (GlobalCacheData.ContainsKey(ViewConstants.ValuationMainBuilding))
                valMainBuilding = GlobalCacheData[ViewConstants.ValuationMainBuilding] as IValuationMainBuilding;
            
            if (GlobalCacheData.ContainsKey(ViewConstants.ValuationCottage))
                valCottage = GlobalCacheData[ViewConstants.ValuationCottage] as IValuationCottage;

            _view.Property = _properties[0];
            _view.HideAllPanels();
            _view.ShowPanelsForAdd();

            _view.BindBuildingClassification(lookups.ValuationClassification);
            _view.BindCottageRoofTypes(lookups.ValuationRoofTypes);
            _view.BindMainBuildingRoofType(lookups.ValuationRoofTypes);

            if (valManual != null)
                _view.BindUpdateableFields(valManual, valMainBuilding, valCottage);

            _view.OnNextButtonClicked += _view_OnNextButtonClicked;
            _view.OnCancelButtonClicked += _view_OnCancelButtonClicked;
        }

        protected void ClearGlobalCache()
        {
            //GlobalCacheData.Remove(ViewConstants.ValuationManual);
            GlobalCacheData.Remove(ViewConstants.ValuationClassification);
            GlobalCacheData.Remove(ViewConstants.ValuationEscalationPercentage);
            GlobalCacheData.Remove(ViewConstants.ValuationMainBuilding);
            GlobalCacheData.Remove(ViewConstants.ValuationCottage);
        }

        protected void _view_OnNextButtonClicked(object sender, EventArgs e)
        {
            // get and validate any Manual Valuation changes
            valManual = _view.GetValuationManual(valManual);
            ExclusionSets.Add(RuleExclusionSets.ValuationMainBuildingView);
            valManual.ValidateEntity();
            ExclusionSets.Remove(RuleExclusionSets.ValuationMainBuildingView);

            // get and validate any Main Building changes
            if (valManual.ValuationMainBuilding == null)
                valMainBuilding = _view.GetValuationMainBuilding(propRepo.CreateEmptyValuationMainBuilding());
            else
                valMainBuilding = _view.GetValuationMainBuilding(valManual.ValuationMainBuilding);

            if (valMainBuilding != null)
            {
                valMainBuilding.Valuation = valManual;
                valMainBuilding.ValidateEntity();
            }

            // get and validate any Cottage changes
            if (valManual.ValuationCottage == null)
                valCottage = _view.GetValuationCottage(propRepo.CreateEmptyValuationCottage());
            else
                valCottage = _view.GetValuationCottage(valManual.ValuationCottage);

            if (valCottage != null)
            {
                valCottage.Valuation = valManual;
                valCottage.ValidateEntity();
            }
            
            //only navigate if the view is valid
            if (_view.IsValid)
            {
                ClearGlobalCache();

                GlobalCacheData.Add(ViewConstants.ValuationClassification, valManual.ValuationClassification, LifeTimes);
                GlobalCacheData.Add(ViewConstants.ValuationEscalationPercentage, valManual.ValuationEscalationPercentage, LifeTimes);
                GlobalCacheData.Add(ViewConstants.ValuationMainBuilding, valMainBuilding, LifeTimes);
                GlobalCacheData.Add(ViewConstants.ValuationCottage, valCottage, LifeTimes);

                Navigator.Navigate("Next");
            }
        }

        protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            ClearGlobalCache();
            Navigator.Navigate("Cancel");
        }
    }
}
