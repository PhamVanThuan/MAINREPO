using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Common.Presenters
{
    public class PropertyDetailsUpdate : PropertyDetailsBase
    {
        private bool _excludePropertyUpdateRules;
        public bool ExcludePropertyUpdateRules
        {
            get { return _excludePropertyUpdateRules; }
            set { _excludePropertyUpdateRules = value; }
        }
	
        /// <summary>
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public PropertyDetailsUpdate(IPropertyDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // setup screen mode
            _view.PropertyDetailsUpdateMode = PropertyDetailsUpdateMode.Property;
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;

            // setup events
            _view.OnUpdateButtonClicked += new EventHandler(OnUpdateButtonClicked);

            _view.BindDropDownLists();

            base.OnViewInitialised(sender, e);

            _excludePropertyUpdateRules = true;
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.ShowPropertyGrid = true;
            _view.ShowDeedsTransfersGrid = false;
        }

        void OnUpdateButtonClicked(object sender, EventArgs e)
        {
            IProperty updatedProperty = _view.SelectedProperty;
            string updatedBondAccountNumber = _view.UpdatedBondAccountNumber;
            string updatedTitleDeedNumbers = _view.UpdatedTitleDeedNumbers;
            int updatedDeedsOfficeKey = _view.UpdatedDeedsOfficeKey;

            // if we are in 'deeds' mode then fire the rule/warning to validate title deed numbers
            if (_view.PropertyDetailsUpdateMode == PropertyDetailsUpdateMode.Deeds)
            {
                IRuleService svc = ServiceFactory.GetService<IRuleService>();
                int rulePassed = svc.ExecuteRule(_view.Messages, "PropertyTitleDeedNumberMandatory", updatedProperty, updatedTitleDeedNumbers);
                if (rulePassed == 0)
                    return;
            }

            if (_view.ValuesChanged == false)
            {
                _view.Navigator.Navigate("Update");
                return;
            }

            // 1. if we update any of the fields then we change the dataprovider to SAHL
            // 2. and create a new PropertyData record with the PropertyDataProviderDataService set to SAHL Manual Valuation

            // exclude the relevant rules
            if (_excludePropertyUpdateRules)
                this.ExclusionSets.Add(RuleExclusionSets.PropertyUpdateView);

            TransactionScope txn = new TransactionScope();

            try
            {
                if (null != updatedProperty.Address)
                {
                    IRuleService ruleService = ServiceFactory.GetService<IRuleService>();
                    ruleService.ExecuteRule(_view.Messages, "WhenChangingPropertyAddressDetailsWarnUserOfLegalEntitiesUsingAsDomicilium", updatedProperty.Address);
                }
                // update/ create a new propertydata record
                IPropertyDataProviderDataService propertyDataProviderDataService = PropertyRepo.GetPropertyDataProviderDataServiceByKey(PropertyDataProviderDataServices.SAHLPropertyManualValuation);
                if (updatedProperty.PropertyDatas == null || updatedProperty.PropertyDatas.Count <= 0)
                {
                    IPropertyData propertyData = PropertyRepo.CreateEmptyPropertyData();
                    propertyData.Property = updatedProperty;
                    propertyData.PropertyDataProviderDataService = propertyDataProviderDataService;
                    propertyData.PropertyID = updatedProperty.Key.ToString();
                    propertyData.InsertDate = System.DateTime.Now;
                    propertyData.Data = PropertyRepo.BuildSAHLPropertyDataXML(updatedBondAccountNumber, updatedDeedsOfficeKey);

                    // add the propertydata to the property
                    updatedProperty.PropertyDatas.Add(_view.Messages, propertyData);
                }
                else
                {
                    foreach (IPropertyData propertyData in updatedProperty.PropertyDatas)
                    {
                        if (propertyData.PropertyDataProviderDataService.Key == propertyDataProviderDataService.Key)
                        {
                            propertyData.Data = PropertyRepo.BuildSAHLPropertyDataXML(updatedBondAccountNumber, updatedDeedsOfficeKey);
                            break;
                        }
                    }
                }

                // Title deeds
                if (_view.TitleDeedNumbersChanged && _view.UpdatedTitleDeedNumbers != null)
                {
                    string[] titleDeedNumbers = updatedTitleDeedNumbers.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    //remove the existing title deed numbers
                    int count = updatedProperty.PropertyTitleDeeds.Count;
                    for (int i = 0; i < count; i++)
                    {
                        updatedProperty.PropertyTitleDeeds.RemoveAt(_view.Messages, 0);
                    }

                    // add the updated number back to the collection
                    foreach (string titleDeedNumber in titleDeedNumbers)
                    {
                        IPropertyTitleDeed propertyTitleDeed = PropertyRepo.CreateEmptyPropertyTitleDeed();
                        propertyTitleDeed.Property = updatedProperty;
                        propertyTitleDeed.TitleDeedNumber = titleDeedNumber;
                        if (updatedDeedsOfficeKey > 0)
                            propertyTitleDeed.DeedsOffice = LookupRepo.DeedsOffice.ObjectDictionary[Convert.ToString(updatedDeedsOfficeKey)];

                        updatedProperty.PropertyTitleDeeds.Add(_view.Messages, propertyTitleDeed);
                    }
                }

                // save the property
                PropertyRepo.SaveProperty(updatedProperty);

                txn.VoteCommit();

                _view.Navigator.Navigate("Update");
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
        }
    }
}
