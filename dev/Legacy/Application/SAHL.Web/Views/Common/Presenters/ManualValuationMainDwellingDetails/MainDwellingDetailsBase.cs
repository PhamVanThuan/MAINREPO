using System;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Web.Views.Common.Presenters.ManualValuationMainDwellingDetails
{
    /// <summary>
    /// 
    /// </summary>
    public class MainDwellingDetailsBase : SAHLCommonBasePresenter<IValuationManualPropertyDetailsView>
    {
        protected IProperty _property;
        protected IValuationDiscriminatedSAHLManual _valManual;
        protected IValuationMainBuilding _valMainBuilding;
        protected IValuationCottage _valCottage;
        protected double _valCombinedThatchValue;
        protected IPropertyRepository _propRepo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public MainDwellingDetailsBase(IValuationManualPropertyDetailsView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            
        }
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _propRepo = RepositoryFactory.GetRepository<IPropertyRepository>();

            if (GlobalCacheData.ContainsKey(ViewConstants.ValuationManual)) // this should only be populated on an add
            {
                _valManual = GlobalCacheData[ViewConstants.ValuationManual] as IValuationDiscriminatedSAHLManual;
                _valMainBuilding = _valManual.ValuationMainBuilding;
                _valCottage = _valManual.ValuationCottage;
            }
            else // this will be for an update / display
            {
                int ValuationKey;
                if (GlobalCacheData.ContainsKey(ViewConstants.SelectedValuationKey))
                    ValuationKey = Convert.ToInt32(GlobalCacheData[ViewConstants.SelectedValuationKey]);
                else
                    ValuationKey = _property.Valuations[0].Key;

                // get origional valuation from database
                _valManual = _propRepo.GetValuationByKey(ValuationKey) as IValuationDiscriminatedSAHLManual;
                _valMainBuilding = _valManual.ValuationMainBuilding;
                _valCottage = _valManual.ValuationCottage;

                // if we have made changes then get them from cache
                if (GlobalCacheData.ContainsKey(ViewConstants.ValuationMainBuilding))
                    _valMainBuilding = GlobalCacheData[ViewConstants.ValuationMainBuilding] as IValuationMainBuilding;

                if (GlobalCacheData.ContainsKey(ViewConstants.ValuationCottage))
                    _valCottage = GlobalCacheData[ViewConstants.ValuationCottage] as IValuationCottage;

                if (GlobalCacheData.ContainsKey(ViewConstants.ValuationClassification))
                    _valManual.ValuationClassification = GlobalCacheData[ViewConstants.ValuationClassification] as IValuationClassification;

                if (GlobalCacheData.ContainsKey(ViewConstants.ValuationEscalationPercentage))
                    _valManual.ValuationEscalationPercentage = Convert.ToDouble(GlobalCacheData[ViewConstants.ValuationEscalationPercentage]);


            }
            // calculate combined thatch value
            _valCombinedThatchValue = _propRepo.CalculateCombinedThatchValue(_valMainBuilding, _valCottage, _valManual.ValuationOutBuildings);

            if (GlobalCacheData.ContainsKey(ViewConstants.Properties))
            {
                IEventList<IProperty> properties = GlobalCacheData[ViewConstants.Properties] as IEventList<IProperty>;
                _property = properties[0];
            }
        }

        protected void CopyCacheValuesToTarget(IValuationDiscriminatedSAHLManual valSAHLManualExistingRec)
        {
            valSAHLManualExistingRec.ValuationClassification = _valManual.ValuationClassification;
            valSAHLManualExistingRec.ValuationEscalationPercentage = _valManual.ValuationEscalationPercentage;
            valSAHLManualExistingRec.ValuationUserID = _view.CurrentPrincipal.Identity.Name;

            //valSAHLManualExistingRec.Data = _valManual.Data;
            //valSAHLManualExistingRec.HOCConventionalAmount = _valManual.HOCConventionalAmount;
            //valSAHLManualExistingRec.HOCShingleAmount = _valManual.HOCShingleAmount;
            //valSAHLManualExistingRec.HOCThatchAmount = _valManual.HOCThatchAmount;
            //valSAHLManualExistingRec.IsActive = _valManual.IsActive;
            //valSAHLManualExistingRec.ValuationAmount = _valManual.ValuationAmount;
            //valSAHLManualExistingRec.ValuationDate = _valManual.ValuationDate;
            //valSAHLManualExistingRec.ValuationHOCValue = _valManual.ValuationHOCValue;
            //valSAHLManualExistingRec.ValuationMunicipal = _valManual.ValuationMunicipal;
            //valSAHLManualExistingRec.ValuationStatus = _valManual.ValuationStatus;
            //valSAHLManualExistingRec.Valuator = _valManual.Valuator;

            if (_valMainBuilding != null)
            {
                if (valSAHLManualExistingRec.ValuationMainBuilding != null) // update existing MainBuilding rec
                {
                    valSAHLManualExistingRec.ValuationMainBuilding.ValuationRoofType = _valMainBuilding.ValuationRoofType;
                    valSAHLManualExistingRec.ValuationMainBuilding.Extent = _valMainBuilding.Extent;
                    valSAHLManualExistingRec.ValuationMainBuilding.Rate = _valMainBuilding.Rate;
                }
                else
                {
                    if (_valMainBuilding.Extent.HasValue && _valMainBuilding.Extent.Value > 0) // new rec for MainBuilding
                    {
                        IValuationMainBuilding valMainBuild = _propRepo.CreateEmptyValuationMainBuilding();
                        valMainBuild.ValuationRoofType = _valMainBuilding.ValuationRoofType;
                        valMainBuild.Extent = _valMainBuilding.Extent;
                        valMainBuild.Rate = _valMainBuilding.Rate;
                        valMainBuild.Valuation = valSAHLManualExistingRec;

                        valSAHLManualExistingRec.ValuationMainBuilding = valMainBuild;
                    }
                }
            }
            if (_valCottage != null)
            {
                if (valSAHLManualExistingRec.ValuationCottage != null) // update existing Cottage rec
                {
                    valSAHLManualExistingRec.ValuationCottage.ValuationRoofType = _valCottage.ValuationRoofType;
                    valSAHLManualExistingRec.ValuationCottage.Extent = _valCottage.Extent;
                    valSAHLManualExistingRec.ValuationCottage.Rate = _valCottage.Rate;
                }
                else
                {
                    if (_valCottage.Extent.HasValue && _valCottage.Extent.Value > 0) // new rec for Cottage
                    {
                        IValuationCottage valCotNew = _propRepo.CreateEmptyValuationCottage();
                        valCotNew.ValuationRoofType = _valCottage.ValuationRoofType;
                        valCotNew.Extent = _valCottage.Extent;
                        valCotNew.Rate = _valCottage.Rate;
                        valCotNew.Valuation = valSAHLManualExistingRec;
                        valSAHLManualExistingRec.ValuationCottage = valCotNew;
                    }
                }
            }

            if (_valCombinedThatchValue > 0)
            {
                if (valSAHLManualExistingRec.ValuationCombinedThatch != null) // update existing Combined Thatch rec
                    valSAHLManualExistingRec.ValuationCombinedThatch.Value = _valCombinedThatchValue;
                else // new rec for Combined Thatch
                {
                    IValuationCombinedThatch valCombinedThatchNew = _propRepo.CreateEmptyValuationCombinedThatch();
                    valCombinedThatchNew.Value = _valCombinedThatchValue;
                    valCombinedThatchNew.Valuation = valSAHLManualExistingRec;
                    valSAHLManualExistingRec.ValuationCombinedThatch = valCombinedThatchNew;
                }
            }

            // Improvements - Removed
            // loop thru the existing list of improvements and check to see if they exist in the cached list
            // if they dont then it means they have been deleted and we must remove them
            IEventList<IValuationImprovement> existingValuationImprovements = new EventList<IValuationImprovement>(valSAHLManualExistingRec.ValuationImprovements);
            foreach (IValuationImprovement improvement in existingValuationImprovements)
            {
                if (improvement.Key > 0 && ImprovementExistsInCacheList(improvement.Key) == false)
                    valSAHLManualExistingRec.ValuationImprovements.Remove(_view.Messages, improvement);
            }

            // Improvements - Added
            // loop thru the list of cached improvements and if the records have no key assigned then these are new ones and must be added
            foreach (IValuationImprovement improvement in _valManual.ValuationImprovements)
            {
                if (improvement.Key == 0)
                    valSAHLManualExistingRec.ValuationImprovements.Add(_view.Messages, improvement);
            }

            // OutBuildings - Removed
            // loop thru the existing list of outbuildings and check to see if they exist in the cached list
            // if they dont then it means they have been deleted and we must remove them
            IEventList<IValuationOutbuilding> existingValuationOutBuildings = new EventList<IValuationOutbuilding>(valSAHLManualExistingRec.ValuationOutBuildings);
            foreach (IValuationOutbuilding outbuilding in existingValuationOutBuildings)
            {
                if (outbuilding.Key > 0 && OutbuildingExistsInCacheList(outbuilding.Key) == false)
                    valSAHLManualExistingRec.ValuationOutBuildings.Remove(_view.Messages, outbuilding);
            }

            // OutBuildings - Added
            // loop thru the list of cached outbuildings and if the records have no key assigned then these are new ones and must be added
            foreach (IValuationOutbuilding outbuilding in _valManual.ValuationOutBuildings)
            {
                if (outbuilding.Key == 0)
                    valSAHLManualExistingRec.ValuationOutBuildings.Add(_view.Messages, outbuilding);
            }
        }

        private bool ImprovementExistsInCacheList(int improvementKey)
        {
            bool improvementExists = false;
            foreach (IValuationImprovement improvement in _valManual.ValuationImprovements)
            {
                if (improvement.Key == improvementKey)
                {
                    improvementExists = true;
                    break;
                }
            }

            return improvementExists;
        }

        private bool OutbuildingExistsInCacheList(int outbuildingKey)
        {
            bool outbuildingExists = false;
            foreach (IValuationOutbuilding outbuilding in _valManual.ValuationOutBuildings)
            {
                if (outbuilding.Key == outbuildingKey)
                {
                    outbuildingExists = true;
                    break;
                }
            }

            return outbuildingExists;
        }

        protected void ClearCache()
        {
            GlobalCacheData.Remove(ViewConstants.ValuationManual);
            GlobalCacheData.Remove(ViewConstants.ValuationMainBuilding);
            GlobalCacheData.Remove(ViewConstants.ValuationCottage);
            GlobalCacheData.Remove(ViewConstants.ValuationClassification);
            GlobalCacheData.Remove(ViewConstants.ValuationEscalationPercentage);

        }
    }
}
