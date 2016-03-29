using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace SAHL.Web.Views.Common.Presenters
{
    public class PropertyCapturePresenter : SAHLCommonBasePresenter<IPropertyCapture>
    {
        private ILookupRepository _lookupRepository;
        private IAddressRepository _addressRepository;
        private IPropertyRepository _propertyRepository;
        private IApplicationRepository _appRepository;
        private ILightStoneService _avm;
        private List<ICacheObjectLifeTime> _lifeTimes;
        private int _genericKey;
        private int _genericKeyTypeKey;

        #region properties

        private ILightStoneService AVM
        {
            get
            {
                if (_avm == null)
                    _avm = ServiceFactory.GetService<ILightStoneService>();
                return _avm;
            }
        }

        protected ILookupRepository LookupRepository
        {
            get
            {
                if (_lookupRepository == null)
                    _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
                return _lookupRepository;
            }
        }

        protected IAddressRepository AddressRepository
        {
            get
            {
                if (_addressRepository == null)
                    _addressRepository = RepositoryFactory.GetRepository<IAddressRepository>();
                return _addressRepository;
            }
        }

        protected IPropertyRepository PropertyRepository
        {
            get
            {
                if (_propertyRepository == null)
                    _propertyRepository = RepositoryFactory.GetRepository<IPropertyRepository>();
                return _propertyRepository;
            }
        }

        protected IApplicationRepository ApplicationRepository
        {
            get
            {
                if (_appRepository == null)
                    _appRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
                return _appRepository;
            }
        }

        protected List<ICacheObjectLifeTime> LifeTimes
        {
            get
            {
                if (_lifeTimes == null)
                {
                    List<string> views = new List<string>();
                    views.Add(this.View.ViewName);
                    _lifeTimes = new List<ICacheObjectLifeTime>();
                    _lifeTimes.Add(new SimplePageCacheObjectLifeTime(views));
                }

                return _lifeTimes;
            }
        }

        #endregion properties

        public PropertyCapturePresenter(IPropertyCapture view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals")]
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            if (_view.IsMenuPostBack)
                ClearCache();

            //NOTE: the values entered here should be GenericKey and GenericTypeKey!
            //this page is for an application, so has an offer key (GenericKeyTypeKey = 2)

            CBONode node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal);

            if (node != null)
            {
                if (!this.GlobalCacheData.ContainsKey("OfferKey"))
                    this.GlobalCacheData.Add("OfferKey", node.GenericKey, LifeTimes);
                _genericKey = node.GenericKey;
                _genericKeyTypeKey = node.GenericKeyTypeKey;
            }
            else
            {
                throw new Exception("CBONode is null");
            }

            base._view.OnSearchButtonClicked += new KeyChangedEventHandler(_view_SearchButtonClicked);
            base._view.OnPropertiesGridSelectedIndexChanged += new KeyChangedEventHandler(_view_PropertiesGridSelectedIndexChanged);
            base._view.OnPropertiesGridDoubleClick += new KeyChangedEventHandler(_view_PropertiesGridDoubleClick);
            base._view.OnExistingAddressSelected += new KeyChangedEventHandler(_view_OnExistingAddressSelected);
            base._view.OnNewAddressSelected += new KeyChangedEventHandler(_view_OnNewAddressSelected);
            base._view.OnPageChanged += new KeyChangedEventHandler(_view_OnPageChanged);
            base._view.OnSavePropertyData += new KeyChangedEventHandler(_view_OnSavePropertyData);
            base._view.OnPropertySave += new KeyChangedEventHandler(_view_OnPropertySave);

            if (!this.GlobalCacheData.ContainsKey("PropertyIdx"))
                this.GlobalCacheData.Add("PropertyIdx", -1, LifeTimes);

            _view.PropertyIndex = Convert.ToInt32(this.GlobalCacheData["PropertyIdx"]);

            if (this.GlobalCacheData.ContainsKey("LightStonePropertyData"))
                _view.BindPropertiesGrid(this.GlobalCacheData["LightStonePropertyData"] as DataTable);

            if (!this.GlobalCacheData.ContainsKey("PropertyCapturePageNo"))
                this.GlobalCacheData.Add("PropertyCapturePageNo", "Property Search", LifeTimes);

            _view.PageNo = Convert.ToString(this.GlobalCacheData["PropertyCapturePageNo"]);

            if (!this.GlobalCacheData.ContainsKey("SellerID"))
                this.GlobalCacheData.Add("SellerID", "", LifeTimes);
            else
                _view.SellerID = Convert.ToString(this.GlobalCacheData["SellerID"]);

            if (this.GlobalCacheData.ContainsKey("AddressKey"))
                _view.AddressKey = (int)this.GlobalCacheData["AddressKey"];

            if (this.GlobalCacheData.ContainsKey("PropertyDataCapturedFromVew"))
                _view.PropertyData = this.GlobalCacheData["PropertyDataCapturedFromVew"] as Dictionary<string, string>;
            else
                _view.PropertyData = null;

            if (this.GlobalCacheData.ContainsKey("SelectedPropertyDataRow"))
                _view.SelectedPropertyData = this.GlobalCacheData["SelectedPropertyDataRow"] as DataRow;
            else
                _view.SelectedPropertyData = null;

            _view.BindPropertyTypes(LookupRepository.PropertyTypes.BindableDictionary);
            _view.BindTitleTypes(LookupRepository.TitleTypes.BindableDictionary);
            _view.BindOccupancyTypes(LookupRepository.OccupancyTypes.BindableDictionary);

            BindComcorpOfferPropertyDetail();
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
        }

        private void _view_OnPageChanged(object sender, KeyChangedEventArgs e)
        {
            string pageNo = Convert.ToString(e.Key);
            this.GlobalCacheData.Add("PropertyCapturePageNo", pageNo, LifeTimes);
        }

        private void _view_SearchButtonClicked(object sender, KeyChangedEventArgs e)
        {
            string ID = e.Key.ToString();
            GlobalCacheData.Remove("Property");
            GlobalCacheData.Remove("PropertyIdx");
            GlobalCacheData.Remove("LightStonePropertyData");
            GlobalCacheData.Remove("LightStonePropertyID");
            GlobalCacheData.Remove("DeedsData");
            GlobalCacheData.Remove("ValuationData");
            GlobalCacheData.Remove("PropertyDataCapturedFromVew");
            GlobalCacheData.Remove("SelectedPropertyDataRow");
            _view.SelectedPropertyData = null;

            this.GlobalCacheData.Add("AddressKey", -1, LifeTimes);
            this.GlobalCacheData.Add("SellerID", ID, LifeTimes);
            this.GlobalCacheData.Add("PropertyIdx", -1, LifeTimes);

            _view.SellerID = ID;
            _view.PropertyIndex = -1;

            DataTable DT = null;

            try
            {
                DT = AVM.ReturnProperties(_genericKey, _genericKeyTypeKey, null, ID, null, null, null, null, null, null, null);
                DT.Columns.Add("PROPERTY_DESCRIPTION_1");
                DT.Columns.Add("PROPERTY_DESCRIPTION_2");
                DT.Columns.Add("PROPERTY_DESCRIPTION_3");
                DT.Columns.Add("DEEDS_PROPERTY_TYPE");
                this.GlobalCacheData.Add("LightStonePropertyData", DT, LifeTimes);
                _view.BindPropertiesGrid(DT);
            }
            catch (Exception ex)
            {
                _view.Messages.Add(new Warning("The LightStone web service call failed. Click 'Search' to try again, or 'Next' to proceed with manual capture.", ex.Message));
            }
        }

        private void _view_PropertiesGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            int idx = Convert.ToInt32(e.Key);
            this.GlobalCacheData.Add("PropertyIdx", idx, LifeTimes);
            _view.PropertyIndex = idx;

            if (this.GlobalCacheData.ContainsKey("LightStonePropertyData"))
                _view.BindPropertiesGrid(this.GlobalCacheData["LightStonePropertyData"] as DataTable);
        }

        private void _view_PropertiesGridDoubleClick(object sender, KeyChangedEventArgs e)
        {
            GlobalCacheData.Remove("Property");
            GlobalCacheData.Remove("PropertyIdx");
            GlobalCacheData.Remove("DeedsData");
            GlobalCacheData.Remove("ValuationData");
            GlobalCacheData.Remove("PropertyDataCapturedFromVew");
            this.GlobalCacheData.Add("AddressKey", -1, LifeTimes);
            _view.AddressKey = -1;

            int idx = Convert.ToInt32(e.Key);
            this.GlobalCacheData.Add("PropertyIdx", idx, LifeTimes);

            if (idx > -1 && this.GlobalCacheData.ContainsKey("LightStonePropertyData"))
            {
                DataTable DT = this.GlobalCacheData["LightStonePropertyData"] as DataTable;

                if (DT.Rows.Count >= idx)
                {
                    DataRow row = DT.Rows[idx];
                    string prov = null;

                    if (row["PROVINCE"] != null)
                    {
                        prov = Convert.ToString(row["PROVINCE"]);

                        switch (prov)
                        {
                            case "NW":
                                prov = "North West";
                                break;

                            case "WC":
                                prov = "Western Cape";
                                break;

                            case "MP":
                                prov = "Mpumalanga";
                                break;

                            case "KN":
                                prov = "Kwazulu-natal";
                                break;

                            case "NC":
                                prov = "Northern Cape";
                                break;

                            case "EC":
                                prov = "Eastern Cape";
                                break;

                            case "LIM":
                                prov = "Limpopo";
                                break;

                            case "GA":
                                prov = "Gauteng";
                                break;

                            case "FS":
                                prov = "free state";
                                break;

                            default:
                                prov = null;
                                break;
                        }
                    }

                    string erf = row["ERF"] != null ? Convert.ToString(row["ERF"]).Trim() : null;
                    string erfPortionNumber = row["PORTION"] != null ? Convert.ToString(row["PORTION"]).Trim() : null;
                    string erfSuburbDescription = row["SUBURB"] != null ? Convert.ToString(row["SUBURB"]).Trim() : null;
                    string sectionalSchemeName = row["SECTIONAL_TITLE"] != null ? Convert.ToString(row["SECTIONAL_TITLE"]).Trim() : null;
                    string sectionalUnitNumber = row["UNIT"] != null ? Convert.ToString(row["UNIT"]).Trim() : null;

                    int dpt = String.IsNullOrEmpty(sectionalUnitNumber) ? 1 : 2;
                    row["DEEDS_PROPERTY_TYPE"] = dpt;

                    if (dpt == 1)
                    {
                        if (!String.IsNullOrEmpty(erfPortionNumber) && erfPortionNumber != "0")
                            row["PROPERTY_DESCRIPTION_1"] = String.Format("Ptn {0} of Erf {1}", erfPortionNumber, erf);
                        else
                            row["PROPERTY_DESCRIPTION_1"] = String.Format("Erf {0}", erf);

                        row["PROPERTY_DESCRIPTION_2"] = erfSuburbDescription;
                    }
                    else if (dpt == 2)
                    {
                        row["PROPERTY_DESCRIPTION_1"] = String.Format("Unit {0}", sectionalUnitNumber);
                        row["PROPERTY_DESCRIPTION_2"] = sectionalSchemeName;
                    }

                    row["PROPERTY_DESCRIPTION_3"] = prov;

                    this.GlobalCacheData.Add("SelectedPropertyDataRow", row, LifeTimes);
                    _view.SelectedPropertyData = row;
                    int propID = Convert.ToInt32(row["PROP_ID"]);
                    this.GlobalCacheData.Add("LightStonePropertyID", propID, LifeTimes);
                }
            }
        }

        private void _view_OnNewAddressSelected(object sender, KeyChangedEventArgs e)
        {
            IAddress address = e.Key as IAddress;

            TransactionScope txn = new TransactionScope();

            try
            {
                AddressRepository.SaveAddress(ref address);
                this.GlobalCacheData.Add("AddressKey", address.Key, LifeTimes);
                GetPropertyInfo(address);
                txn.VoteCommit();
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

        private void _view_OnExistingAddressSelected(object sender, KeyChangedEventArgs e)
        {
            int addressKey = Convert.ToInt32(e.Key);
            IAddressStreet address = AddressRepository.GetAddressByKey(addressKey) as IAddressStreet;
            this.GlobalCacheData.Add("AddressKey", address.Key, LifeTimes);
            GetPropertyInfo(address);
        }

        private void GetPropertyInfo(IAddress address)
        {
            IProperty property = null;
            List<IProperty> list = PropertyRepository.GetPropertyByAddressKey(address.Key);

            if (list.Count > 0)
                property = list[0]; //there should only ever be one
            else
                property = PropertyRepository.CreateEmptyProperty();

            property.Address = address;

            if (this.GlobalCacheData.ContainsKey("SelectedPropertyDataRow"))
            {
                DataRow row = this.GlobalCacheData["SelectedPropertyDataRow"] as DataRow;

                property.ErfNumber = row["ERF"] != null ? Convert.ToString(row["ERF"]).Trim() : null;
                property.ErfPortionNumber = row["PORTION"] != null ? Convert.ToString(row["PORTION"]).Trim() : null;
                property.ErfSuburbDescription = row["SUBURB"] != null ? Convert.ToString(row["SUBURB"]).Trim() : null;
                property.SectionalSchemeName = row["SECTIONAL_TITLE"] != null ? Convert.ToString(row["SECTIONAL_TITLE"]).Trim() : null;
                property.SectionalUnitNumber = row["UNIT"] != null ? Convert.ToString(row["UNIT"]).Trim() : null;
                property.PropertyDescription1 = row["PROPERTY_DESCRIPTION_1"] != null ? Convert.ToString(row["PROPERTY_DESCRIPTION_1"]).Trim() : null;
                property.PropertyDescription2 = row["PROPERTY_DESCRIPTION_2"] != null ? Convert.ToString(row["PROPERTY_DESCRIPTION_2"]).Trim() : null;
                property.PropertyDescription3 = row["PROPERTY_DESCRIPTION_3"] != null ? Convert.ToString(row["PROPERTY_DESCRIPTION_3"]).Trim() : null;

                string dpt = String.IsNullOrEmpty(property.SectionalUnitNumber) ? "1" : "2";
                property.DeedsPropertyType = LookupRepository.DeedsPropertyTypes.ObjectDictionary[dpt];
            }

            this.GlobalCacheData.Add("Property", property, LifeTimes);

            if (property.Key > 0)
                _view.SetOccupancyTypeValue = property.OccupancyType.Key.ToString();

            DataSet deedsInfo = null;

            if (this.GlobalCacheData.ContainsKey("LightStonePropertyID"))
            {
                int propID = Convert.ToInt32(this.GlobalCacheData["LightStonePropertyID"]);
                deedsInfo = null;

                //get the deeds info
                try
                {
                    deedsInfo = AVM.ReturnTransferData(_genericKey, _genericKeyTypeKey, propID);
                    this.GlobalCacheData.Add("DeedsData", deedsInfo, LifeTimes);
                }
                catch (Exception ex)
                {
                    _view.Messages.Add(new Warning("The LightStone web service call failed. Please proceed with manual capture.", ex.Message));
                }
                this.GlobalCacheData.Remove("PropertyDataCapturedFromVew");
            }
        }

        private void BindComcorpOfferPropertyDetail()
        {
            _view.HasComcorpOfferPropertyDetails = false;

            if (!this.GlobalCacheData.ContainsKey("OfferKey"))
                return;

            int offerKey = Convert.ToInt32(GlobalCacheData["OfferKey"]);
            IApplication application = ApplicationRepository.GetApplicationByKey(offerKey);

            if (!application.IsComcorp())
                return;

            IComcorpOfferPropertyDetails comcorpOfferPropertyDetails = PropertyRepository.GetComcorpOfferPropertyDetails(offerKey);

            if (comcorpOfferPropertyDetails != null)
            {
                string pageNo = Convert.ToString(this.GlobalCacheData["PropertyCapturePageNo"]);
                _view.BindComcorpOfferPropertyDetail(pageNo, comcorpOfferPropertyDetails);
                _view.HasComcorpOfferPropertyDetails = true;
            }
        }

        private void ClearCache()
        {
            GlobalCacheData.Remove("PropertyCapturePageNo");
            GlobalCacheData.Remove("Property");
            GlobalCacheData.Remove("PropertyIdx");
            GlobalCacheData.Remove("LightStonePropertyData");
            GlobalCacheData.Remove("LightStonePropertyID");
            GlobalCacheData.Remove("AddressKey");
            GlobalCacheData.Remove("DeedsData");
            GlobalCacheData.Remove("ValuationData");
            GlobalCacheData.Remove("OfferKey");
            GlobalCacheData.Remove("PropertyDataCapturedFromVew");
        }

        private void _view_OnSavePropertyData(object sender, KeyChangedEventArgs e)
        {
            Dictionary<string, string> pData = e.Key as Dictionary<string, string>;
            this.GlobalCacheData.Add("PropertyDataCapturedFromVew", pData, LifeTimes);
        }

        private bool ValidateClientInputs(Dictionary<string, string> inputs)
        {
            string errorMsg = string.Empty;

            if (Convert.ToInt32(inputs["OccupancyType"]) == -1)
            {
                errorMsg = "Please select an Occupancy Type.";
            }

            if (string.IsNullOrEmpty(errorMsg))
                return true;
            else
            {
                _view.Messages.Add(new Error(errorMsg, errorMsg));
                return false;
            }
        }

        private void _view_OnPropertySave(object sender, KeyChangedEventArgs e)
        {
            Dictionary<string, string> pData = this.GlobalCacheData["PropertyDataCapturedFromVew"] as Dictionary<string, string>;

            if (!ValidateClientInputs(pData))
                return;

            // exclude the relevant rules
            this.ExclusionSets.Add(RuleExclusionSets.PropertyCaptureView);

            int addressKey = (int)this.GlobalCacheData["AddressKey"];
            IAddress address = AddressRepository.GetAddressByKey(addressKey);

            IProperty cachedProperty = this.GlobalCacheData["Property"] as IProperty;
            IProperty property = null;

            if (cachedProperty.Key > 0)
                property = PropertyRepository.GetPropertyByKey(cachedProperty.Key);
            else
                property = PropertyRepository.CreateEmptyProperty();

            property.Address = address;

            DataSet deedsInfo = null;

            if (this.GlobalCacheData.ContainsKey("DeedsData"))
                deedsInfo = this.GlobalCacheData["DeedsData"] as DataSet;

            TransactionScope txn = new TransactionScope();

            DataRow row = null;

            //load the lightstone data into the property object
            if (this.GlobalCacheData.ContainsKey("SelectedPropertyDataRow"))
                row = this.GlobalCacheData["SelectedPropertyDataRow"] as DataRow;

            if (deedsInfo != null)
            {
                DataTable transfers = deedsInfo.Tables["Transfers"];

                if (transfers != null && transfers.Rows.Count > 0)
                {
                    transfers.DefaultView.Sort = "registration_date DESC";
                    object regDate = transfers.DefaultView[0].Row["registration_date"];
                    object purchPrice = transfers.DefaultView[0].Row["purch_price"];

                    if (regDate != null)
                        property.CurrentBondDate = ConvertYYYYMMDDToDateTime(transfers.DefaultView[0].Row["registration_date"].ToString());

                    double temp;

                    if (Double.TryParse(purchPrice.ToString(), out temp))
                        property.DeedsOfficeValue = temp;
                }

                if (deedsInfo.Tables["Property"].Rows[0]["Township"] != null)
                    property.ErfMetroDescription = deedsInfo.Tables["Property"].Rows[0]["Township"].ToString().Trim();
            }

            //Riiiight...are we allowed to update?
            bool canUpdateDetails = true;

            //if the property is associated with an open account, do not update, just reuse,
            //else update the details and reuse
            if (property.Key > 0) //existing property
            {
                for (int i = 0; i < property.MortgageLoanProperties.Count; i++)
                {
                    if (property.MortgageLoanProperties[i].Account.AccountStatus.Key == (int)AccountStatuses.Open)
                    {
                        canUpdateDetails = false;
                        break;
                    }
                }
            }

            if (canUpdateDetails)
            {
                if (row != null)
                {
                    property.ErfNumber = cachedProperty.ErfNumber;
                    property.ErfPortionNumber = cachedProperty.ErfPortionNumber;
                    property.ErfSuburbDescription = cachedProperty.ErfSuburbDescription;
                    property.SectionalSchemeName = cachedProperty.SectionalSchemeName;
                    property.SectionalUnitNumber = cachedProperty.SectionalUnitNumber;
                }

                string dpt = String.IsNullOrEmpty(cachedProperty.SectionalUnitNumber) ? "1" : "2";
                property.DeedsPropertyType = LookupRepository.DeedsPropertyTypes.ObjectDictionary[dpt];

                //now load the selections from the view
                property.PropertyDescription1 = pData["Description1"].Trim();
                property.PropertyDescription2 = pData["Description2"].Trim();
                property.PropertyDescription3 = pData["Description3"].Trim();
                property.AreaClassification = LookupRepository.AreaClassifications.ObjectDictionary["1"];

                int occtype = int.Parse(pData["OccupancyType"]);
                int proptype = int.Parse(pData["PropertyType"]);
                int titletype = int.Parse(pData["TitleType"]);

                if (occtype >= 0)
                    property.OccupancyType = LookupRepository.OccupancyTypes.ObjectDictionary[pData["OccupancyType"]];

                if (proptype >= 0)
                    property.PropertyType = LookupRepository.PropertyTypes.ObjectDictionary[pData["PropertyType"]];

                if (titletype >= 0)
                    property.TitleType = LookupRepository.TitleTypes.ObjectDictionary[pData["TitleType"]];

                if (deedsInfo != null)
                {
                    IPropertyTitleDeed titleDeed = null;

                    if (deedsInfo.Tables.Contains("TitleDeed") && deedsInfo.Tables["TitleDeed"].Columns.Contains("title_deed_no") && deedsInfo.Tables["TitleDeed"].Rows.Count > 0)
                    {
                        string deedNo = Convert.ToString(deedsInfo.Tables["TitleDeed"].Rows[0]["title_deed_no"]).Trim();
                        titleDeed = PropertyRepository.GetPropertyTitleDeedByTitleDeedNumber(property.Key, deedNo);

                        if (titleDeed == null)
                        {
                            titleDeed = PropertyRepository.CreateEmptyPropertyTitleDeed();
                            titleDeed.TitleDeedNumber = deedNo;
                            titleDeed.Property = property;
                            //PropertyRepository.SavePropertyTitleDeed(titleDeed);
                        }

                        property.PropertyTitleDeeds.Add(_view.Messages, titleDeed);
                    }
                }

                if (property.PropertyAccessDetails == null)
                    property.PropertyAccessDetails = PropertyRepository.CreateEmptyPropertyAccessDetails();

                property.PropertyAccessDetails.Property = property;
                property.PropertyAccessDetails.Contact1 = pData["InspectionContact"].Trim();
                property.PropertyAccessDetails.Contact1Phone = pData["InspectionPhone"].Trim();
                property.PropertyAccessDetails.Contact2 = pData["InspectionContact2"].Trim();
                property.PropertyAccessDetails.Contact2Phone = pData["InspectionPhone2"].Trim();
            }

            //do this regardless
            if (row != null)
                property.DataProvider = LookupRepository.DataProviders.ObjectDictionary[((int)DataProviders.LightStone).ToString()];
            else
                property.DataProvider = LookupRepository.DataProviders.ObjectDictionary[((int)DataProviders.SAHL).ToString()];

            if (deedsInfo != null)
            {
                //Create empty IPropertyData and populate with DeedsOffice dataset
                IPropertyData propertyData = PropertyRepository.CreateEmptyPropertyData();
                propertyData.Data = deedsInfo.GetXml();
                propertyData.InsertDate = DateTime.Now;
                propertyData.Property = property;
                propertyData.PropertyDataProviderDataService = PropertyRepository.GetPropertyDataProviderDataServiceByKey(PropertyDataProviderDataServices.LightstonePropertyIdentification);
                propertyData.PropertyID = Convert.ToString(this.GlobalCacheData["LightStonePropertyID"]);

                property.PropertyDatas.Add(_view.Messages, propertyData);
            }

            property.ErfMetroDescription = RemoveDuplicateSpaces(property.ErfMetroDescription);
            property.ErfNumber = RemoveDuplicateSpaces(property.ErfNumber);
            property.ErfPortionNumber = RemoveDuplicateSpaces(property.ErfPortionNumber);
            property.ErfSuburbDescription = RemoveDuplicateSpaces(property.ErfSuburbDescription);
            property.PropertyDescription1 = RemoveDuplicateSpaces(property.PropertyDescription1);
            property.PropertyDescription2 = RemoveDuplicateSpaces(property.PropertyDescription2);
            property.PropertyDescription3 = RemoveDuplicateSpaces(property.PropertyDescription3);
            property.SectionalSchemeName = RemoveDuplicateSpaces(property.SectionalSchemeName);
            property.SectionalUnitNumber = RemoveDuplicateSpaces(property.SectionalUnitNumber);

            try
            {
                int offerKey = Convert.ToInt32(GlobalCacheData["OfferKey"]);
                IRuleService svcRule = ServiceFactory.GetService<IRuleService>();
                svcRule.ExecuteRule(_view.Messages, "DetermineDuplicateApplication", offerKey, property.Key);

                PropertyRepository.SaveProperty(property);
                this.GlobalCacheData.Add("Property", property, LifeTimes);

                IApplication offer = ApplicationRepository.GetApplicationByKey(offerKey);
                IApplicationMortgageLoan oml = offer as IApplicationMortgageLoan;

                IAccount _accountRel = oml.Account != null ? oml.Account : null;
                bool _doSave = false;

                foreach (IAccount ca in oml.RelatedAccounts)
                {
                    if (ca.Product.Key == (int)Products.HomeOwnersCover)
                        oml.RelatedAccounts.Remove(_view.Messages, ca);
                }

                svcRule.ExecuteRule(_view.Messages, "DetermineLegalEntityHasBeenDeclined", oml, property);

                if (_doSave)
                {
                    IAccountRepository _accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                    _accRepo.SaveAccount(_accountRel);
                }

                oml.Property = property;

                //check if there is a recent val
                //this is a hack that needs to be addressed by the IgnoreMessages architecture
                //this is a duplicate implementation of the rule ValuationRecentExists as a temp hack fix
                //TRAC 15158
                if (oml.Property != null && oml.Property.LatestCompleteValuation != null
                    && oml.Property.LatestCompleteValuation.IsActive
                    && oml.Property.LatestCompleteValuation.ValuationDate > DateTime.Now.AddMonths(-12)
                    && (oml.ApplicationType.Key == (int)OfferTypes.NewPurchaseLoan || oml.ApplicationType.Key == (int)OfferTypes.SwitchLoan || oml.ApplicationType.Key == (int)OfferTypes.RefinanceLoan)
                    )
                {
                    if (svcRule.ExecuteRule(_view.Messages, "ValuationRecentExists", oml) == 1)
                    {
                        //If we have an active valuation < 12 then we use it.
                        IValuation val = oml.Property.LatestCompleteValuation;
                        ISupportsVariableLoanApplicationInformation svli = oml.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                        IApplicationInformationVariableLoan vli = svli.VariableLoanInformation;

                        vli.PropertyValuation = val.ValuationAmount;
                    }
                }

                ApplicationRepository.SaveApplication(oml);
                oml.CalculateApplicationDetail(false, false);

                txn.VoteCommit();
            }
            catch (Exception)
            {
                txn.VoteRollBack();

                if (_view.IsValid)
                    throw;
                else
                    return;
            }
            finally
            {
                txn.Dispose();
            }

            if (_view.IsValid)
            {
                CBOManager.RefreshInstanceNode(_view.CurrentPrincipal);
                Navigator.Navigate("Submit");
            }
        }

        private static DateTime? ConvertYYYYMMDDToDateTime(string dateString)
        {
            if (String.IsNullOrEmpty(dateString) || dateString.Length != 8)
                return null;

            int yyyy = int.Parse(dateString.Substring(0, 4));
            int mm = int.Parse(dateString.Substring(4, 2));
            int dd = int.Parse(dateString.Substring(6, 2));

            return new DateTime(yyyy, mm, dd);
        }

        private static string RemoveDuplicateSpaces(string offendingString)
        {
            if (offendingString == null)
                return null;

            while (offendingString.Contains("  "))
                offendingString = offendingString.Replace("  ", " ");

            return offendingString;
        }
    }
}