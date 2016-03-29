using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Repositories;

using SAHL.Common.CacheData;
using SAHL.Common;
using SAHL.Common.UI;

namespace SAHL.Web.Views.Common.Presenters.ValuationDetails
{

    public class ValuationScheduleAdCheckValuationAddPresenter : SAHLCommonBasePresenter<IValuationScheduleAdCheckValuation>
    {

        IAdCheckService avm;
        private ILookupRepository lookuprepository;
        private IAddressRepository addressrepository;
        private IPropertyRepository propertyrepository;
        private IApplicationRepository applicationrepository;
        private IAccountRepository accountrepository;
        private IBondRepository bondrepository;
        private int genericKey;
        private int genericKeyTypeKey;
        private CBONode node;
        readonly IDomainMessageCollection messages = new DomainMessageCollection();
        private DataSet dsProperties = new DataSet();
        private IApplication application;
        private IAccount account;
        private IProperty property;
        private IApplicationMortgageLoan applicationMortgageLoan;
        private int propertyKey;
        private int valuationKey;
        private string adcheckPropertyId;
        private IInstance instance;
        private List<ICacheObjectLifeTime> lifeTimes;
        string applicationtypedescription;

        protected IBondRepository BondRepository
        {

            get
            {
                if (bondrepository == null)
                    bondrepository = RepositoryFactory.GetRepository<IBondRepository>();
                return bondrepository;
            }

        }

        protected IAccountRepository AccountRepository
        {
            get
            {
                if (accountrepository == null)
                    accountrepository = RepositoryFactory.GetRepository<IAccountRepository>();
                return accountrepository;
            }
        }

        protected ILookupRepository LookupRepository
        {
            get
            {
                if (lookuprepository == null)
                    lookuprepository = RepositoryFactory.GetRepository<ILookupRepository>();
                return lookuprepository;
            }
        }

        protected IAddressRepository AddressRepository
        {
            get
            {
                if (addressrepository == null)
                    addressrepository = RepositoryFactory.GetRepository<IAddressRepository>();
                return addressrepository;
            }
        }

        protected IApplicationRepository ApplicationRepository
        {
            get
            {
                if (applicationrepository == null)
                    applicationrepository = RepositoryFactory.GetRepository<IApplicationRepository>();
                return applicationrepository;
            }
        }

        protected IPropertyRepository PropertyRepository
        {
            get
            {
                if (propertyrepository == null)
                    propertyrepository = RepositoryFactory.GetRepository<IPropertyRepository>();
                return propertyrepository;
            }
        }

        private IX2Repository x2Repository;
        protected IX2Repository X2Repository
        {
            get
            {
                if (x2Repository == null)
                    x2Repository = RepositoryFactory.GetRepository<IX2Repository>();
                return x2Repository;
            }
        }

        protected List<ICacheObjectLifeTime> LifeTimes
        {
            get
            {
                if (lifeTimes == null)
                {
                    List<string> views = new List<string>();
                    views.Add(View.ViewName);
                    lifeTimes = new List<ICacheObjectLifeTime>();
                    lifeTimes.Add(new SimplePageCacheObjectLifeTime(views));
                }

                return lifeTimes;
            }
        }

        public ValuationScheduleAdCheckValuationAddPresenter(IValuationScheduleAdCheckValuation view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            // Set up EventHandlers for the interface
            View.btnInstructClicked += BtnInstructClicked;
            View.grdPropertiesSelectedIndexChanged += PropertiesGridSelectedIndexChanged;

            if (!GlobalCacheData.ContainsKey("PropertiesIndex"))
            {
                //PrivateCacheData.Add("PropertiesIndex", 0);
                GlobalCacheData.Add("PropertiesIndex", 0, LifeTimes);
            }
            else
            {
                if (GlobalCacheData.ContainsKey("PropertiesIndex"))
                    View.SetddlPropertiesSelectedSelectedIndex = (int)GlobalCacheData["PropertiesIndex"];
                else
                {
                    //PrivateCacheData.Add("PropertiesIndex", 0);
                    GlobalCacheData.Add("PropertiesIndex", 0, LifeTimes);
                    View.SetddlPropertiesSelectedSelectedIndex = 0;
                }
            }

            //if (PrivateCacheData.ContainsKey("AdCheckProperties"))
            //    dsProperties = PrivateCacheData["AdCheckProperties"] as DataSet;
            //else 
            if (GlobalCacheData.ContainsKey("AdCheckProperties"))
                dsProperties = GlobalCacheData["AdCheckProperties"] as DataSet;
        }

        void PropertiesGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            //PrivateCacheData.Add("PropertiesIndex", View.PropertyItemIndex);
            GlobalCacheData.Add("PropertiesIndex", View.PropertyItemIndex, LifeTimes);

        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            //switch (CBOManager.GetCurrentNodeSetName(View.CurrentPrincipal))
            //{
            //    case CBONodeSetType.CBO:
            //        node = CBOManager.GetCurrentCBONode(View.CurrentPrincipal, CBONodeSetType.CBO);
            //        break;
            //    case CBONodeSetType.X2:
            //        node = CBOManager.GetCurrentCBONode(View.CurrentPrincipal, CBONodeSetType.X2);

            //        break;
            //    default:
            //        break;
            //}

            node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            if (node != null)
            {
                genericKeyTypeKey = node.GenericKeyTypeKey;
                genericKey = node.GenericKey;
                instance = X2Repository.GetLatestInstanceForGenericKey(genericKey, SAHL.Common.Constants.WorkFlowName.Valuations, SAHL.Common.Constants.WorkFlowProcessName.Origination);
                //PrivateCacheData.Remove("genericKey");
                //PrivateCacheData.Add("genericKey", genericKey);
                GlobalCacheData.Remove("genericKey");
                GlobalCacheData.Add("genericKey", genericKey, LifeTimes);
            }

            switch (genericKeyTypeKey)
            {
                case (int)GenericKeyTypes.Account:
                    // get the account
                    account = AccountRepository.GetAccountByKey(genericKey);
                    application = ApplicationRepository.GetApplicationByKey(genericKey);

                    //tk = account.Key;
                    property = GetSecuredPropertiesAccountKey(account.Key);
                    break;
                case (int)GenericKeyTypes.Offer:
                    // get the application
                    application = ApplicationRepository.GetApplicationFromInstance(instance);
                    if (application != null && application is IApplicationMortgageLoan)
                    {
                        applicationMortgageLoan = application as IApplicationMortgageLoan;
                    }
                    property = applicationMortgageLoan.Property;
                    break;
                case (int)GenericKeyTypes.Property:
                    // get the property
                    break;
            }


            if (application != null)
            {
                switch (application.ApplicationType.Key)
                {
                    case (int)OfferTypes.NewPurchaseLoan:
                        applicationtypedescription = "1";
                        break;
                    case (int)OfferTypes.SwitchLoan:
                        applicationtypedescription = "2";
                        break;
                    case (int)OfferTypes.RefinanceLoan:
                        applicationtypedescription = "2";
                        break;
                    case (int)OfferTypes.ReAdvance:
                        applicationtypedescription = "3";
                        break;
                    case (int)OfferTypes.FurtherAdvance:
                        applicationtypedescription = "3";
                        break;
                    case (int)OfferTypes.FurtherLoan:
                        applicationtypedescription = "3";
                        break;
                }
                //PrivateCacheData.Remove("applicationtypedescription");
                //PrivateCacheData.Add("applicationtypedescription", applicationtypedescription);
                GlobalCacheData.Remove("applicationtypedescription");
                GlobalCacheData.Add("applicationtypedescription", applicationtypedescription, LifeTimes);

                IRuleService svc = ServiceFactory.GetService<IRuleService>();
                svc.ExecuteRule(_view.Messages, "ValuationRecentExists", application);
            }


            //property = PropertyRepository.GetPropertyByKey(527047);
            if (property != null)
            {
                bool adChecked = PropertyRepository.HasAdCheckPropertyID(property.Key);
                bool adCheckInvalidData = false;
                int i = 0;
                if (adChecked)
                {
                    // Find AdCheck Property Identification
                    for (i = 0; i < property.PropertyDatas.Count; i++)
                    {
                        //if (property.PropertyDatas[i].PropertyDataProviderDataService.Key == 2)
                        if (property.PropertyDatas[i].PropertyDataProviderDataService.Key == (int)PropertyDataProviderDataServices.AdCheckPropertyIdentification)
                        {
                            adcheckPropertyId = property.PropertyDatas[i].PropertyID; // ADcheck Property ID

                            //PrivateCacheData.Add("adcheckPropertyId", adcheckPropertyId);
                            GlobalCacheData.Add("adcheckPropertyId", adcheckPropertyId, LifeTimes);

                            // there is already property data - do load the dataset and cache it
                            InitialiseXmlDataset(property.PropertyDatas[i].Data);

                            break;
                        }
                    }

                    // Find AdCheck InvalidData
                    for (i = 0; i < property.PropertyDatas.Count; i++)
                    {
                        if (property.PropertyDatas[i].PropertyDataProviderDataService.Key == (int)PropertyDataProviderDataServices.AdCheckInvalidData)
                        {
                            adCheckInvalidData = true;
                            break;
                        }
                    }
                }

                //PrivateCacheData.Remove("PropertyKey");
                //PrivateCacheData.Add("PropertyKey", property.Key);
                GlobalCacheData.Remove("PropertyKey");
                GlobalCacheData.Add("PropertyKey", property.Key, LifeTimes);


                SetupInterface();
                SetupPropertyAccessDetails();
                SetupPropertyDetailsDisplay();

                /*
                - IF the PropertyData table doesn’t already have an AdCheck ProperyID against the Property linked to the loan you are working on. 
                (This can occur on New loan or existing Open Loan)
                - IF the AdCheckID does exist in the PropertyData table AND the Property table has a Data Provider of SAHL. 
                A PropertyData record that has an AdCheckID and a Property Data Provider of SAHL means that this specific record originally 
                from AdCheck has since been amended by a user.  Requesting this again would be to ensure data integrity.
                - IF the AdCheckID does exist in the PropertyData table AND that record is marked as Invalid Data (Dataservicekey=7).
                */
                if (!adChecked || (adChecked && property != null && property.DataProvider.Key == (int)DataProviders.SAHL) || (adChecked && adCheckInvalidData))
                {
                    SetupAdCheckPropertyGrid();
                }
            }
        }

        void SetupAdCheckPropertyGrid()
        {
            //  get the external provider property id's
            if (GetAVMPropertyList(property))
            {
                View.ShowpnlSelectProperty = true;
                View.ShowgrdProperties = true;
                View.BindgrdProperties(dsProperties.Tables[0]);
            }
            else
            {
                // We cannot isolate an adcheck ID because no properties have been returned.
                View.ShowbtnInstruct = false;
            }
        }

        void SetupPropertyDetailsDisplay()
        {

            // Set the Dropdown List Indexes
            int valuerIndex = View.SetddlValuerSelectedIndex;
            int assessmentPriorityIndex = View.SetddlAssessmentPriorityValue;
            switch (application.ApplicationType.Key)
            {
                case (int)OfferTypes.NewPurchaseLoan:
                    View.SetlblAssessmentReasonValue = "New Purchase Loan";
                    break;
                case (int)OfferTypes.SwitchLoan:
                    View.SetlblAssessmentReasonValue = "Switch Loan";
                    break;
                case (int)OfferTypes.RefinanceLoan:
                    View.SetlblAssessmentReasonValue = "Refinance Loan";
                    break;
                case (int)OfferTypes.ReAdvance:
                    View.SetlblAssessmentReasonValue = "Re Advance";
                    break;
                case (int)OfferTypes.FurtherAdvance:
                    View.SetlblAssessmentReasonValue = "Further Advance";
                    break;
                case (int)OfferTypes.FurtherLoan:
                    View.SetlblAssessmentReasonValue = "further Loan";
                    break;
            }
            // Get the propertyData
            //if (PrivateCacheData.ContainsKey("PropertyKey"))
            //    propertyKey = Convert.ToInt32(PrivateCacheData["PropertyKey"]);
            if (GlobalCacheData.ContainsKey("PropertyKey"))
                propertyKey = Convert.ToInt32(GlobalCacheData["PropertyKey"]);
            property = PropertyRepository.GetPropertyByKey(propertyKey);



            if (property != null)
            {
                IAddressStreet addStreet = property.Address as IAddressStreet;
                if (addStreet != null)
                {
                    View.SetlblStreetValue = addStreet.StreetNumber + " " + addStreet.StreetName;
                    View.SetlblBuildingValue = addStreet.BuildingNumber + " " + addStreet.BuildingName;
                }
                View.SetlblSuburbValue = property.Address.RRR_SuburbDescription;
                View.SetlblCityValue = property.Address.RRR_CityDescription;
                View.SetlblProvinceValue = property.Address.RRR_ProvinceDescription;
                View.SetlblCountryValue = property.Address.RRR_CountryDescription;
                View.SetlblPostalCodeValue = property.Address.RRR_PostalCode;
                View.SettxtPropertyDescription = property.PropertyDescription1 + " " + property.PropertyDescription2 + " " + property.PropertyDescription3;
                if (property.DeedsPropertyType != null)
                    View.SetlblDeedsPropertyTypeValue = property.DeedsPropertyType.Description;
                View.SetlblTitleTypeValue = property.TitleType.Description;

                if (property.PropertyTitleDeeds.Count > 0)
                    View.SettxtTitleDeedNumber = property.PropertyTitleDeeds[property.PropertyTitleDeeds.Count - 1].TitleDeedNumber;

                View.SetlblOccupancyTypeValue = property.OccupancyType.Description;
                View.SetlblAreaClassificationValue = property.AreaClassification.Description;
                View.SetlblERFNumberValue = property.ErfNumber;
                View.SetlblPortionNumberValue = property.ErfPortionNumber;
                View.SetlblERFMetroDescriptionValue = property.ErfMetroDescription;
                View.SetlblERFSuburbValue = property.Address.RRR_SuburbDescription;
                View.SetlblSectionalSchemeNameValue = property.SectionalSchemeName;
                View.SetlblSectionalUnitNumberValue = property.SectionalUnitNumber;
                View.SetlblAssessmentByDateValue = DateTime.Now.ToShortDateString();
            }

            //Setup the Valuer Data 
            DataTable valuators = SetupValuators();

            //PrivateCacheData.Add("Valuators", valuators);
            GlobalCacheData.Add("Valuators", valuators, LifeTimes);

            View.bindddlValuer(valuators);
            //View.bindddlAssessmentReasonValue(GetLoanTypes());

            if (GlobalCacheData.ContainsKey("IsPostback"))
            {
                View.SetddlValuerSelectedIndex = valuerIndex;
                View.SetddlAssessmentPriorityValue = assessmentPriorityIndex;

                switch (application.ApplicationType.Key)
                {
                    case (int)OfferTypes.NewPurchaseLoan:
                        View.SetlblAssessmentReasonValue = "New Purchase Loan";
                        break;
                    case (int)OfferTypes.SwitchLoan:
                        View.SetlblAssessmentReasonValue = "Switch Loan";
                        break;
                    case (int)OfferTypes.RefinanceLoan:
                        View.SetlblAssessmentReasonValue = "Refinance Loan";
                        break;
                    case (int)OfferTypes.ReAdvance:
                        View.SetlblAssessmentReasonValue = "Re Advance";
                        break;
                    case (int)OfferTypes.FurtherAdvance:
                        View.SetlblAssessmentReasonValue = "Further Advance";
                        break;
                    case (int)OfferTypes.FurtherLoan:
                        View.SetlblAssessmentReasonValue = "further Loan";
                        break;
                }
            }

        }

        void SetupInterface()
        {
            // Set up the initial visibility on the screen components
            View.ShowtxtContact1MobilePhone = true;
            View.ShowtxtContact1Name = true;
            View.ShowtxtContact1Phone = true;
            View.ShowtxtContact1WorkPhone = true;
            View.ShowtxtContact2Name = true;
            View.ShowtxtContact2Phone = true;
            View.ShowddlValuer = true;

            View.ShowddlAssessmentReasonValue = false;
            View.ShowlblAssessmentReasonValue = true;

            View.ShowddlAssessmentPriorityValue = true;
            View.ShowbtnInstruct = true;
            View.ShowbtnValidateProperty = false;
            View.ShowdtAssessmentByDateValue = true;
            View.ShowtxtSpecialInstructions = true;
            // Hide the top Panel This is not displayed on the add
            View.ShowpnlValuationAssignmentDetails = false;

        }

        void BtnInstructClicked(object sender, EventArgs e)
        {
            //Do Checks on fields (Business Rules)

            //if (PrivateCacheData.ContainsKey("applicationtypedescription"))
            //    applicationtypedescription = (string)PrivateCacheData["applicationtypedescription"];
            //else
            //    applicationtypedescription = "2";
            if (GlobalCacheData.ContainsKey("applicationtypedescription"))
                applicationtypedescription = (string)GlobalCacheData["applicationtypedescription"];
            else
                applicationtypedescription = "2";

            string appKey = "0";
            //if (PrivateCacheData.ContainsKey("genericKey"))
            //    appKey = Convert.ToString(PrivateCacheData["genericKey"]);
            if (GlobalCacheData.ContainsKey("genericKey"))
                appKey = Convert.ToString(GlobalCacheData["genericKey"]);

            //PrivateCacheData.Remove("genericKey");
            //PrivateCacheData.Add("genericKey", genericKey);
            GlobalCacheData.Remove("genericKey");
            GlobalCacheData.Add("genericKey", genericKey, LifeTimes);

            //if (PrivateCacheData.ContainsKey("adcheckPropertyId"))
            //    adcheckPropertyId = (string)PrivateCacheData["adcheckPropertyId"];
            //else 
            if (GlobalCacheData.ContainsKey("adcheckPropertyId"))
            {
                adcheckPropertyId = (string)GlobalCacheData["adcheckPropertyId"];
                SaveAdScheckPropertyId(adcheckPropertyId, false);
            }
            else
            {
                adcheckPropertyId = dsProperties.Tables[0].Rows[(int)GlobalCacheData["PropertiesIndex"]]["PropertyID"].ToString();
                SaveAdScheckPropertyId(adcheckPropertyId, true);
                //PrivateCacheData.Add("adcheckPropertyId", adcheckPropertyId);
                GlobalCacheData.Add("adcheckPropertyId", adcheckPropertyId, LifeTimes);
            }


            if (CheckValuationEntries() & CheckContactEntries())
            {
                if (AddValuationRecord() & UpdatePropertyAccessDetails())
                {
                    //Create Workflow Case in ValualtionsWorkflow pointing to valuations record created above
                    Dictionary<string, string> inputFields = new Dictionary<string, string>();
                    inputFields.Clear();

                    //if (PrivateCacheData.ContainsKey("valuationKey"))
                    //    valuationKey = (int)PrivateCacheData["valuationKey"];
                    //else 
                    if (GlobalCacheData.ContainsKey("valuationKey"))
                        valuationKey = (int)GlobalCacheData["valuationKey"];

                    inputFields.Add("PropertyKey", propertyKey.ToString());
                    inputFields.Add("RequestingAdUser", View.CurrentPrincipal.Identity.Name);
                    inputFields.Add("AdcheckPropertyId", adcheckPropertyId);
                    inputFields.Add("ValuationKey", valuationKey.ToString());
                    inputFields.Add("ValuationDataProviderDataServiceKey", ((int)ValuationDataProviderDataServices.AdCheckPhysicalValuation).ToString());
                    //assesmentpriority#specialinstructions#valuatorid#applicationtypedescription#assesmentDate
                    string specialinstructions = "App:" + appKey + ": " + View.SettxtSpecialInstructions;
                    string x2String = Convert.ToString(View.SetddlAssessmentPriorityValue) + "#" + specialinstructions;
                    if (GlobalCacheData.ContainsKey("ValuatorID"))
                        x2String += "#" + Convert.ToString(GlobalCacheData["ValuatorID"]);
                    else
                        x2String += "#";
                    x2String += "#" + applicationtypedescription + "#" + Convert.ToString(View.SetdtAssessmentByDateValue);

                    GlobalCacheData.Remove("IsPostback");
                    GlobalCacheData.Remove("PropertiesIndex");
                    GlobalCacheData.Remove("AdCheckProperties");
                    GlobalCacheData.Remove("adcheckPropertyId");
                    GlobalCacheData.Remove("Property");
                    GlobalCacheData.Remove("Valuators");
                    GlobalCacheData.Remove("ValuatorID");
                    GlobalCacheData.Remove("valuationKey");
                    GlobalCacheData.Remove("propertyAccessdetails");
                    GlobalCacheData.Remove("PropertyKey");
                    GlobalCacheData.Remove("Contact2Details");

                    X2Service.CompleteActivity(View.CurrentPrincipal, inputFields, false, x2String);
                    X2Service.WorkflowNavigate(View.CurrentPrincipal, View.Navigator);
                }
            }
            else
            {
                // Setup the error messages and return
                //PrivateCacheData.Add("IsPostback", true);
                GlobalCacheData.Add("IsPostback", true, LifeTimes);
            }
        }


        // Save the Newly retrieved ADCheckPropertyID to the database
        private void SaveAdScheckPropertyId(string acpid, bool createNew)
        {
            // Rule exclusion notes
            //PropertyDeedsPropertyTypeMandatory 50218
            //15,7
            //bool createNew;
            //createNew = true;
            ExclusionSets.Add(RuleExclusionSets.ValuationScheduleAdCheckValuationAddView);

            //if (PrivateCacheData.ContainsKey("PropertyKey"))
            //    propertyKey = Convert.ToInt32(PrivateCacheData["PropertyKey"]);
            using (TransactionScope txn = new TransactionScope())
            {
                if (GlobalCacheData.ContainsKey("PropertyKey"))
                    propertyKey = Convert.ToInt32(GlobalCacheData["PropertyKey"]);
                IProperty prop = PropertyRepository.GetPropertyByKey(propertyKey);
                IPropertyData propdata;

                if (createNew)
                {
                    propdata = PropertyRepository.CreateEmptyPropertyData();
                    propdata.PropertyID = acpid;
                    propdata.InsertDate = DateTime.Now;
                    propdata.Property = prop;
                }

                else
                {
                    propdata = PropertyRepository.GetLatestPropertyData(prop, PropertyDataProviderDataServices.AdCheckPropertyIdentification);
                }


                propdata.PropertyDataProviderDataService = PropertyRepository.GetPropertyDataProviderDataServiceByKey(PropertyDataProviderDataServices.AdCheckPropertyIdentification);

                try
                {
                    if (createNew)
                    {

                        if (dsProperties != null)
                            propdata.Data = dsProperties.GetXml();

                        if (prop != null)
                        {
                            IDataProvider dp = propdata.PropertyDataProviderDataService.DataProviderDataService.DataProvider;
                            prop.PropertyDatas.Add(messages, propdata);
                            prop.DataProvider = dp;
                        }

                        // Save all of the objects to the Database
                    }
                    else
                    {

                        if (prop != null)
                        {
                            //Set Dataprovider back to adcheck
                            IDataProvider dp = propdata.PropertyDataProviderDataService.DataProviderDataService.DataProvider;
                            prop.DataProvider = dp;
                        }
                    }
                    PropertyRepository.SaveProperty(prop);
                    
                    txn.VoteCommit();
                    
                    ExclusionSets.Remove(RuleExclusionSets.ValuationScheduleAdCheckValuationAddView);

                }
                catch (Exception)
                {
                    txn.VoteRollBack();

                    ExclusionSets.Remove(RuleExclusionSets.ValuationScheduleAdCheckValuationAddView);
                    if (View.IsValid)
                        throw;
                }

            }
        }


        // Update the valuation record with the valuers details
        private bool AddValuationRecord()
        {

            ExclusionSets.Add(RuleExclusionSets.ValuationUpdatePreviousToInactive);
            bool retval = true;
            //string SQL = " select data.*, s.name, i.* from x2.instance i join x2data.Valuations data on i.id=data.instanceid join x2.state s on i.stateid=s.id where data.application.key= " + genericKey + " order by data.application.key desc";
            DataTable valuators = null;

            //if (PrivateCacheData.ContainsKey("Valuators"))
            //    valuators = PrivateCacheData["Valuators"] as DataTable;
            //else 
            if (GlobalCacheData.ContainsKey("Valuators"))
                valuators = GlobalCacheData["Valuators"] as DataTable;

            //dsProperties = PrivateCacheData["AdCheckProperties"] as DataSet;
            IValuationDiscriminatedAdCheckPhysical valuation = PropertyRepository.CreateEmptyValuation(ValuationDataProviderDataServices.AdCheckPhysicalValuation) as IValuationDiscriminatedAdCheckPhysical;
            if (valuation != null)
            {
                valuation.ValuationStatus = LookupRepository.ValuationStatus.ObjectDictionary[((int)ValuationStatuses.Pending).ToString()];
                valuation.IsActive = false;

                if (dsProperties != null) valuation.Data = dsProperties.GetXml();

                //if (PrivateCacheData.ContainsKey("PropertyKey"))
                //    propertyKey = Convert.ToInt32(PrivateCacheData["PropertyKey"]);

                //if (PrivateCacheData.ContainsKey("PropertyKey"))
                //    propertyKey = Convert.ToInt32(PrivateCacheData["PropertyKey"]);
                //else 
                if (GlobalCacheData.ContainsKey("PropertyKey"))
                    propertyKey = Convert.ToInt32(GlobalCacheData["PropertyKey"]);



                valuation.Property = PropertyRepository.GetPropertyByKey(propertyKey);


                if (valuators != null)
                {
                    valuation.Valuator = PropertyRepository.GetValuatorByKey(Convert.ToInt32(View.SetddlValuerSelectedIndex));
                    //PrivateCacheData.Add("ValuatorID", valuation.Valuator.Key);
                    GlobalCacheData.Add("ValuatorID", valuation.Valuator.Key, LifeTimes);
                }
                valuation.ValuationUserID = View.CurrentPrincipal.Identity.Name;
                //Select Conventional Roof Type
                valuation.HOCRoof = LookupRepository.HOCRoof.ObjectDictionary["2"];
                valuation.ValuationDate = DateTime.Now;

            }
            TransactionScope txn = new TransactionScope();
            try
            {
                //Save the New Valuation
                PropertyRepository.SaveValuation(valuation);
                txn.VoteCommit();
                ExclusionSets.Remove(RuleExclusionSets.ValuationUpdatePreviousToInactive);
            }

            catch (Exception)
            {
                txn.VoteRollBack();
                retval = false;
                if (View.IsValid)
                    throw;
            }

            finally
            {
                txn.Dispose();
            }

            if (valuation != null)
            {
                valuationKey = valuation.Key;

                //PrivateCacheData.Add("valuationKey", valuationKey);
                GlobalCacheData.Add("valuationKey", valuationKey, LifeTimes);
            }

            return retval;

        }

        /// <summary>
        /// 
        /// </summary>
        void SetupPropertyAccessDetails()
        {

            if (!GlobalCacheData.ContainsKey("propertyAccessdetails"))
            {
                //if (PrivateCacheData.ContainsKey("PropertyKey"))
                //    propertyKey = Convert.ToInt32(PrivateCacheData["PropertyKey"]);

                if (GlobalCacheData.ContainsKey("PropertyKey"))
                    propertyKey = Convert.ToInt32(GlobalCacheData["PropertyKey"]);

                IEventList<IPropertyAccessDetails> pad = PropertyRepository.GetPropertyAccesDetailsByPropertyKey(propertyKey);
                if (pad.Count > 0)
                {
                    //PrivateCacheData.Add("propertyAccessdetails", pad[0]);
                    GlobalCacheData.Add("propertyAccessdetails", pad[0], LifeTimes);
                    // there is already a contact record... populate the view with it..
                    // TextBoxes
                    View.SettxtContact1Name = pad[0].Contact1;
                    View.SettxtContact1MobilePhone = pad[0].Contact1MobilePhone;
                    View.SettxtContact1Phone = pad[0].Contact1Phone;
                    View.SettxtContact1WorkPhone = pad[0].Contact1WorkPhone;
                    View.SettxtContact2Name = pad[0].Contact2;
                    View.SettxtContact2Phone = pad[0].Contact2Phone;
                    //Labels
                    View.SetlblContact1Value = pad[0].Contact1;
                    View.SetlblCellPhone1Value = pad[0].Contact1MobilePhone;
                    View.SetlblPhone1Value = pad[0].Contact1Phone;
                    View.SetlblWorkPhone1Value = pad[0].Contact1WorkPhone;
                    View.SetlblContact2Value = pad[0].Contact2;
                    View.SetlblPhone2Value = pad[0].Contact2Phone;
                }
            }
        }

        bool UpdatePropertyAccessDetails()
        {
            bool retval = true;
            //if (PrivateCacheData.ContainsKey("PropertyKey"))
            //    propertyKey = Convert.ToInt32(PrivateCacheData["PropertyKey"]);
            if (GlobalCacheData.ContainsKey("PropertyKey"))
                propertyKey = Convert.ToInt32(GlobalCacheData["PropertyKey"]);

            IPropertyRepository propertyRep = RepositoryFactory.GetRepository<IPropertyRepository>();
            IProperty prop = propertyRep.GetPropertyByKey(propertyKey);
            IPropertyAccessDetails propertyAccessdetails = PropertyRepository.CreateEmptyPropertyAccessDetails();

            if (GlobalCacheData.ContainsKey("propertyAccessdetails"))
            {
                IEventList<IPropertyAccessDetails> pad = PropertyRepository.GetPropertyAccesDetailsByPropertyKey(propertyKey);
                propertyAccessdetails = pad[0];
            }
            else
                propertyAccessdetails.Property = prop;

            propertyAccessdetails.Contact1 = View.SettxtContact1Name;
            propertyAccessdetails.Contact1MobilePhone = View.SettxtContact1MobilePhone;
            propertyAccessdetails.Contact1Phone = View.SettxtContact1Phone;
            propertyAccessdetails.Contact1WorkPhone = View.SettxtContact1WorkPhone;
            propertyAccessdetails.Contact2 = View.SettxtContact2Name;
            propertyAccessdetails.Contact2Phone = View.SettxtContact2Phone;



            //Store Information in PropertyAccessDetails
            TransactionScope txn = new TransactionScope();
            try
            {
                // Save all of the objects to the Database
                PropertyRepository.SavePropertyAccessDetails(propertyAccessdetails);
                txn.VoteCommit();
            }

            catch (Exception)
            {
                txn.VoteRollBack();
                retval = false;
                if (View.IsValid)
                    throw;
            }

            finally
            {
                txn.Dispose();
            }
            return retval;
        }

        bool CheckValuationEntries()
        {

            bool retval = true;

            if (View.SetddlValuerSelectedIndex == 0)
            {
                View.Messages.Add(new Warning("Please select an Valuator before continuing. ", "Please select an Valuator before continuing."));
                retval = false;
            }

            if (View.SetddlAssessmentPriorityValue == 0)
            {
                View.Messages.Add(new Warning("Please select an Assessment Priority before continuing.", "Please select an Assessment Priority before continuing."));
                retval = false;
            }

            DateTime dateNow = DateTime.Now;
            DateTime dateSelected = View.SetdtAssessmentByDateValue.ToString().Length > 0 ? View.SetdtAssessmentByDateValue : dateNow;

            if (dateSelected <= dateNow)
            {
                View.Messages.Add(new Warning("Please select a future Assessment date before continuing.", "Please select a future Assessment date before continuing."));
                retval = false;
            }

            return retval;
        }


        bool CheckContactEntries()
        {
            bool retval = true;

            if (View.SettxtContact1Name.Length == 0)
            {
                View.Messages.Add(new Warning("Contact 1 Name is required.", "Contact 1 Name is required."));
                retval = false;
            }
            if (View.SettxtContact1Phone.Length == 0 & View.SettxtContact1WorkPhone.Length == 0 & View.SettxtContact1MobilePhone.Length == 0)
            {
                View.Messages.Add(new Warning("At least one of Contact 1's Home phone number, Work phone number or Cell phone number must be provided.", "At least one of Contact 1's Home phone number, Work phone number or Cell phone number must be provided."));
                retval = false;
            }
            if (retval)
                if (!GlobalCacheData.ContainsKey("Contact2Details"))
                {
                    // If it contains the key, its already checked once
                    if (View.SettxtContact2Name.Length == 0)
                    {
                        View.Messages.Add(new Warning("If you have a second contact, please add them. If not, ignore this message and click Instruct Valuer again.", "If you have a second contact, please add them. If not, ignore this message and click Instruct Valuer again."));
                        retval = false;
                    }

                    //PrivateCacheData.Add("Contact2Details", true);
                    GlobalCacheData.Add("Contact2Details", true, LifeTimes);

                }
            return retval;
        }

        private DataTable SetupValuators()
        {
            DataTable valuator = new DataTable("Valuator");
            valuator.Columns.Add("ValuatorKey", Type.GetType("System.String")); //0
            valuator.Columns.Add("ValuatorName", Type.GetType("System.String")); //1

            //IEventList<IValuator> valuators = PropertyRepository.GetActiveValuators();
            IEventList<IValuator> valuators = PropertyRepository.GetActiveValuatorsFiltered((int)GeneralStatuses.Active);

            valuators.Sort(delegate(IValuator c1, IValuator c2)
                {
                    return c1.LegalEntity.GetLegalName(LegalNameFormat.Full).CompareTo(c2.LegalEntity.GetLegalName(LegalNameFormat.Full));
                });

            LegalEntityRepository legalentityrepository = (LegalEntityRepository)RepositoryFactory.GetRepository<ILegalEntityRepository>();

            DataRow selectRow = valuator.NewRow();
            selectRow["ValuatorKey"] = "0";
            selectRow["ValuatorName"] = "-select-";
            valuator.Rows.Add(selectRow);

            for (int i = 0; i < valuators.Count; i++)
            {
                DataRow valRow = valuator.NewRow();
                valRow["ValuatorKey"] = valuators[i].Key.ToString();
                ILegalEntity le = legalentityrepository.GetLegalEntityByKey(valuators[i].LegalEntity.Key);
                valRow["ValuatorName"] = le.GetLegalName(LegalNameFormat.Full);
                valuator.Rows.Add(valRow);
            }
            return valuator;
        }

        /// <summary>
        ///  Get a list of Matching Properties from Adcheck
        /// </summary>
        /// <param name="prop"></param>
        /// <returns> true if valid</returns>
        private bool GetAVMPropertyList(IProperty prop)
        {
            InitialiseAVM();

            string erfnumber = prop.ErfNumber;
            string suburb = prop.Address.RRR_SuburbDescription;
            string province = prop.Address.RRR_ProvinceDescription;
            string city = prop.Address.RRR_CityDescription;
            string schemename = prop.SectionalSchemeName;
            string unitnumber = prop.SectionalUnitNumber;
            string portion = prop.ErfPortionNumber;

            try
            {
                //dsProperties = avm.ValidateAddress(-1, null, unitnumber, schemename, null, null, suburb, city, province, erfnumber, null);
                dsProperties = avm.ValidateAddress(-1, null, unitnumber, schemename, null, null, suburb, city, province, erfnumber, portion);

                if (dsProperties == null || dsProperties.Tables.Count == 0)
                {
                    View.Messages.Add(new Warning("Adcheck ValidateAddress returned no data", "Raise your hand and shout 'Tech Support!'"));
                    return false;
                }

                //PrivateCacheData.Add("AdCheckProperties", dsProperties);
                GlobalCacheData.Add("AdCheckProperties", dsProperties, LifeTimes);

                if (dsProperties.Tables[0].Rows.Count > 0)
                {
                    if ((dsProperties.Tables[0].Rows[0]["PropertyID"].ToString() == "-1"))
                    {
                        View.Messages.Add(new Warning("AdCheck cannot find any matching properties. Please ensure that the correct property details have been captured.", "AdCheck cannot find any matching properties. Please ensure that the correct property details have been captured."));
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                View.Messages.Add(new Warning("The AdCheck web service call failed with error: " + ex.Message + "\n The Service is Offline - Please try again later.", ex.Message));
            }

            return false;
        }

        private void InitialiseAVM()
        {
            avm = ServiceFactory.GetService<IAdCheckService>();
            if (node != null)
                avm.Initialise(node.GenericKey, node.GenericKeyTypeKey);
            else
                avm.Initialise(1, 2);
        }

        public static IProperty GetSecuredPropertiesAccountKey(int accountKey)
        {
            IAccountRepository accrep = RepositoryFactory.GetRepository<IAccountRepository>();
            IMortgageLoanAccount acc = (IMortgageLoanAccount)accrep.GetAccountByKey(accountKey);
            IProperty prop = acc.SecuredMortgageLoan.Property;
            return prop;
        }

        void InitialiseXmlDataset(string xmlData)
        {
            if (xmlData != null)
            {
                StringReader textReader = new StringReader(xmlData);
                dsProperties.ReadXml(textReader, XmlReadMode.Auto);
            }
        }
    }
}


