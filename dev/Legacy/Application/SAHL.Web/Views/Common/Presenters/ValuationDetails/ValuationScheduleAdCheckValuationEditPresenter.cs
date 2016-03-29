using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common;
using SAHL.Common.UI;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Common.Presenters.ValuationDetails
{

    public class ValuationScheduleAdCheckValuationEditPresenter : SAHLCommonBasePresenter<IValuationScheduleAdCheckValuation>
    {
        readonly IDomainMessageCollection messages = new DomainMessageCollection();
        private ILookupRepository lookuprepository;
        private IAddressRepository addressrepository;
        private IPropertyRepository propertyrepository;
        private IApplicationRepository applicationrepository;
        private IAccountRepository accountrepository;
        private IBondRepository bondrepository;
        private int genericKey;
        private int genericKeyTypeKey;
        private CBONode node;
        private DataSet dsProperties = new DataSet();
        private DataTable dtValuatorData = new DataTable();
        private IApplication application;
        private IAccount account;
        private IProperty property;
        private IApplicationMortgageLoan applicationMortgageLoan;
        //private DataTable dtDeedsTransfers;
        private int propertyKey;
        private string AdcheckPropertyID;
        private IInstance instance;
        //private string applicationtype = "";
        private int valuationKey;

        string applicationtypedescription;
        // int applicationtypekey;
        private List<ICacheObjectLifeTime> lifeTimes;

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

        private IX2Repository x2repository;
        protected IX2Repository X2Repository
        {
            get
            {
                if (x2repository == null)
                    x2repository = RepositoryFactory.GetRepository<IX2Repository>();
                return x2repository;
            }
        }

        public ValuationScheduleAdCheckValuationEditPresenter(IValuationScheduleAdCheckValuation view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            // Set up EventHandlers for the interface
            View.btnInstructClicked += btnInstructClicked;
            View.grdPropertiesSelectedIndexChanged += PropertiesGridSelectedIndexChanged;

            //if (!PrivateCacheData.ContainsKey("PropertiesIndex"))
            //    PrivateCacheData.Add("PropertiesIndex", 0);
            if (!GlobalCacheData.ContainsKey("PropertiesIndex"))
                GlobalCacheData.Add("PropertiesIndex", 0,LifeTimes);

            //if (PrivateCacheData.ContainsKey("AdCheckProperties"))
            //    dsProperties = PrivateCacheData["AdCheckProperties"] as DataSet;
            if (GlobalCacheData.ContainsKey("AdCheckProperties"))
                dsProperties = GlobalCacheData["AdCheckProperties"] as DataSet;
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
            }

            switch (genericKeyTypeKey)
            {
                case (int)GenericKeyTypes.Account:
                    // get the account
                    account = AccountRepository.GetAccountByKey(genericKey);
                    application = ApplicationRepository.GetApplicationByKey(genericKey);
                    //applicationtype = application.ApplicationType.Description;
                    //tk = account.Key;
                    property = GetSecuredPropertiesAccountKey(account.Key);
                    break;
                case (int)GenericKeyTypes.Offer:
                    // get the application
                    application = ApplicationRepository.GetApplicationFromInstance(instance);
                    if (application != null && application is IApplicationMortgageLoan)
                    {
                        applicationMortgageLoan = application as IApplicationMortgageLoan;
                        //applicationtype = applicationMortgageLoan.ApplicationType.Description;
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
                GlobalCacheData.Add("applicationtypedescription", applicationtypedescription,LifeTimes);
            }

            //if (PrivateCacheData.ContainsKey("ValuatorData"))
            //    dtValuatorData = PrivateCacheData["ValuatorData"] as DataTable;
            //else
            //{
            //    dtValuatorData = PropertyRepository.GetValuatorDataFromXMLHistory(genericKeyTypeKey, genericKey);
            //    PrivateCacheData.Remove("dtValuatorData");
            //    PrivateCacheData.Add("dtValuatorData", dtValuatorData);

            //}
            if (GlobalCacheData.ContainsKey("ValuatorData"))
                dtValuatorData = GlobalCacheData["ValuatorData"] as DataTable;
            else
            {
                dtValuatorData = PropertyRepository.GetValuatorDataFromXMLHistory(genericKeyTypeKey, genericKey, "AdCheck");
                GlobalCacheData.Remove("dtValuatorData");
                GlobalCacheData.Add("dtValuatorData", dtValuatorData,LifeTimes);

            }

            if (property != null)
            {
                if (PropertyRepository.HasAdCheckPropertyID(property.Key))
                    for (int i = 0; i < property.PropertyDatas.Count; i++)
                        if (property.PropertyDatas[i].PropertyDataProviderDataService.Key == 2)
                        {
                            AdcheckPropertyID = property.PropertyDatas[i].PropertyID; // ADcheck Property ID
                            //PrivateCacheData.Remove("AdcheckPropertyID");
                            //PrivateCacheData.Add("AdcheckPropertyID", AdcheckPropertyID);
                            GlobalCacheData.Remove("AdcheckPropertyID");
                            GlobalCacheData.Add("AdcheckPropertyID", AdcheckPropertyID,LifeTimes);

                            // there is already property data - do load the dataset and cache it
                            InitialiseXMLDataset(property.PropertyDatas[i].Data);
                            //PrivateCacheData.Remove("AdCheckProperties");
                            //PrivateCacheData.Add("AdCheckProperties", dsProperties);
                            GlobalCacheData.Remove("AdCheckProperties");
                            GlobalCacheData.Add("AdCheckProperties", dsProperties,LifeTimes);
                            break;
                        }


                //GetLatestPropertyData(property);

                //PrivateCacheData.Remove("PropertyKey");
                //PrivateCacheData.Add("PropertyKey", property.Key);
                GlobalCacheData.Remove("PropertyKey");
                GlobalCacheData.Add("PropertyKey", property.Key,LifeTimes);

                SetupInterface();
                SetupPropertyAccessDetails();
                SetupPropertyDetailsDisplay();
            }

        }

        void PropertiesGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            //PrivateCacheData.Remove("PropertiesIndex");
            //PrivateCacheData.Add("PropertiesIndex", View.PropertyItemIndex);
            GlobalCacheData.Remove("PropertiesIndex");
            GlobalCacheData.Add("PropertiesIndex", View.PropertyItemIndex,LifeTimes);
        }

        void btnInstructClicked(object sender, EventArgs e)
        {
            //Do Checks on fields (Business Rules)
            //if (PrivateCacheData.ContainsKey("AdcheckPropertyID"))
            if (GlobalCacheData.ContainsKey("AdcheckPropertyID"))
                AdcheckPropertyID = (string)GlobalCacheData["AdcheckPropertyID"];
            else
            {
                AdcheckPropertyID = dsProperties.Tables[0].Rows[View.PropertyItemIndex]["PropertyID"].ToString();
                SaveAdScheckPropertyID(AdcheckPropertyID);
                //PrivateCacheData.Add("AdcheckPropertyID", AdcheckPropertyID);
                GlobalCacheData.Add("AdcheckPropertyID", AdcheckPropertyID,LifeTimes);
            }

            //if (PrivateCacheData.ContainsKey("applicationtypedescription"))
            //    applicationtypedescription = (string)PrivateCacheData["applicationtypedescription"];
            //else
            //    applicationtypedescription = "2";
            if (GlobalCacheData.ContainsKey("applicationtypedescription"))
                applicationtypedescription = (string)GlobalCacheData["applicationtypedescription"];
            else
                applicationtypedescription = "2";

            if (CheckValuationEntries() & CheckContactEntries())
            {
                if (AddValuationRecord() & UpdatePropertyAccessDetails())
                {
                    //Create Workflow Case in ValualtionsWorkflow pointing to valuations record created above
                    Dictionary<string, string> inputFields = new Dictionary<string, string>();
                    inputFields.Clear();
                    //if (PrivateCacheData.ContainsKey("valuationKey"))
                    //    valuationKey = (int)PrivateCacheData["valuationKey"];
                    if (GlobalCacheData.ContainsKey("valuationKey"))
                        valuationKey = (int)GlobalCacheData["valuationKey"];
                    inputFields.Add("PropertyKey", propertyKey.ToString());
                    inputFields.Add("RequestingAdUser", _view.CurrentPrincipal.Identity.Name);
                    inputFields.Add("AdcheckPropertyID", AdcheckPropertyID);
                    inputFields.Add("ValuationKey", valuationKey.ToString());
                    //assesmentpriority#specialinstructions#valuatorid#applicationtypedescription#assesmentDate
                    string X2String = Convert.ToString(View.SetddlAssessmentPriorityValue) + "#" + View.SettxtSpecialInstructions;
                    //if (PrivateCacheData.ContainsKey("ValuatorID"))
                    //    X2String += "#" + Convert.ToString(PrivateCacheData["ValuatorID"]);
                    if (GlobalCacheData.ContainsKey("ValuatorID"))
                        X2String += "#" + Convert.ToString(GlobalCacheData["ValuatorID"]);
                    else
                        X2String += "#";
                    X2String += "#" + applicationtypedescription + "#" + Convert.ToString(View.SetdtAssessmentByDateValue);

                    //string X2String = "";
                    //// PriorityValue
                    //switch (View.SetddlAssessmentPriorityValue)
                    //{
                    //    case 0:
                    //    case 1:
                    //        X2String += "#Low";
                    //        break;
                    //    case 2:
                    //        X2String += "#Medium";
                    //        break;
                    //    case 3:
                    //        X2String += "#High";
                    //        break;
                    //}

                    //X2String += "#" + View.SettxtSpecialInstructions;
                    //if (PrivateCacheData.ContainsKey("ValuatorID"))
                    //    X2String += "#" + Convert.ToString(PrivateCacheData["ValuatorID"]);
                    //else
                    //    X2String += "#";
                    //X2String += "#" + applicationtypedescription + "#" + View.SetdtAssessmentByDateValue;

                    X2Service.CompleteActivity(View.CurrentPrincipal, inputFields, false, X2String);
                    X2Service.WorkflowNavigate(View.CurrentPrincipal, View.Navigator);
                }
            }
            else
            {
                // Setup the error messages and return
                //PrivateCacheData.Remove("IsPostback");
                //PrivateCacheData.Add("IsPostback", true);
                GlobalCacheData.Remove("IsPostback");
                GlobalCacheData.Add("IsPostback", true,LifeTimes);
            }
        }


        // Save the Newly retrieved ADCheckPropertyID to the database
        private void SaveAdScheckPropertyID(string ACPID)
        {
            // Set it in PropertyDatas
            //if (PrivateCacheData.ContainsKey("PropertyKey"))
            //    propertyKey = Convert.ToInt32(PrivateCacheData["PropertyKey"]);
            if (GlobalCacheData.ContainsKey("PropertyKey"))
                propertyKey = Convert.ToInt32(GlobalCacheData["PropertyKey"]);
            IProperty prop = PropertyRepository.GetPropertyByKey(propertyKey);

            IPropertyData propdata = PropertyRepository.CreateEmptyPropertyData();
            propdata.PropertyDataProviderDataService = PropertyRepository.GetPropertyDataProviderDataServiceByKey(PropertyDataProviderDataServices.AdCheckPropertyIdentification);
            propdata.PropertyID = ACPID;
            propdata.InsertDate = DateTime.Now;
            propdata.Property = prop;
            //dsProperties = PrivateCacheData["AdCheckProperties"] as DataSet;
            dsProperties = GlobalCacheData["AdCheckProperties"] as DataSet;
            if (dsProperties != null) propdata.Data = dsProperties.GetXml();
            if (prop != null)
                prop.PropertyDatas.Add(messages, propdata);

            TransactionScope txn = new TransactionScope();
            try
            {
                // Save all of the objects to the Database
                PropertyRepository.SaveProperty(prop);
                txn.VoteCommit();
            }

            catch (Exception)
            {
                txn.VoteRollBack();
                if (View.IsValid)
                    throw;
            }

            finally
            {
                txn.Dispose();
            }

        }


        void SetupPropertyDetailsDisplay()
        {
            // Set the Dropdown List Indexes
            int ValuerIndex = View.SetddlValuerSelectedIndex;
            int AssessmentPriorityIndex = View.SetddlAssessmentPriorityValue;
            //int AssessmentReasonIndex = View.SetddlAssessmentReasonValue;
            //View.SetlblAssessmentReasonValue = applicationtypedescription;
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
                View.SetlblDeedsPropertyTypeValue = property.PropertyType.Description;
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

                //Setup the Valuer Data 
                DataTable Valuators = SetupValuators();
                //PrivateCacheData.Remove("Valuators");
                //PrivateCacheData.Add("Valuators", Valuators);
                GlobalCacheData.Remove("Valuators");
                GlobalCacheData.Add("Valuators", Valuators,LifeTimes);

                View.bindddlValuer(Valuators);
                View.bindddlAssessmentReasonValue(GetLoanTypes());

                //TODO Fix this to match the new field structures.
                DataRow valuatorrow = dtValuatorData.Rows[0];
                //if (PrivateCacheData.ContainsKey("IsPostback"))
                if (GlobalCacheData.ContainsKey("IsPostback"))
                {
                    View.SetddlValuerSelectedIndex = ValuerIndex;
                    View.SetddlAssessmentPriorityValue = AssessmentPriorityIndex;
                    //View.SetddlAssessmentReasonValue = AssessmentReasonIndex;
                }
                else
                {
                    View.SettxtSpecialInstructions = Convert.ToString(valuatorrow["instructions"]);
                    View.SetdtAssessmentByDateValue = Convert.ToDateTime(valuatorrow["requested_perform_date"]);
                    // Set and compensate for the select row
                    View.SetddlAssessmentPrioritySelectedIndex = Convert.ToInt32(valuatorrow["val_priority_id"]) - 1;
                    View.ddlAssessmentReasonValueSelectedIndex = Convert.ToInt32(valuatorrow["val_request_reason_type_id"]);

                    // Valuation Request Details
                    for (int i = 0; i < Valuators.Rows.Count; i++)
                        if (Convert.ToInt32(valuatorrow["val_company_id"]) == Convert.ToInt32(Valuators.Rows[i]["ValuatorKey"]))
                        {
                            View.SetddlValuerSelectedIndex = Convert.ToInt32(Valuators.Rows[i]["ValuatorKey"]);
                            break;
                        }
                }

                // Top Panel 
                View.SetlblRequestNumberValue = Convert.ToString(valuatorrow["alternate_valuation_id"]);
                View.SetlblAssessmentNumberValue = AdcheckPropertyID;
                View.SetlblApplicationNameValue = instance.Subject;
                View.SetlblRequestedByValue = instance.CreatorADUserName;


                //View.SetlblSAPTGPropertyNumberValue = lstProperties[0].
            }

            //PrivateCacheData.Remove("AdCheckProperties");
            //PrivateCacheData.Add("AdCheckProperties", dsProperties);
            GlobalCacheData.Remove("AdCheckProperties");
            GlobalCacheData.Add("AdCheckProperties", dsProperties,LifeTimes);
        }


        void SetupInterface()
        {
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

            View.SettxtSpecialInstructionsReadOnly = false;
            View.SettxtPropertyDescriptionReadOnly = true;

            // Show the top Panel This is not displayed on the add
            View.ShowpnlValuationAssignmentDetails = true;

        }




        void SetupPropertyAccessDetails()
        {
            //if (!PrivateCacheData.ContainsKey("propertyAccessdetails"))
            if (!GlobalCacheData.ContainsKey("propertyAccessdetails"))
            {
                //propertyKey = Convert.ToInt32(PrivateCacheData["PropertyKey"]);
                propertyKey = Convert.ToInt32(GlobalCacheData["PropertyKey"]);
                IEventList<IPropertyAccessDetails> pad = PropertyRepository.GetPropertyAccesDetailsByPropertyKey(propertyKey);
                if (pad.Count > 0)
                {
                    //PrivateCacheData.Remove("propertyAccessdetails");
                    //PrivateCacheData.Add("propertyAccessdetails", pad[0]);
                    GlobalCacheData.Remove("propertyAccessdetails");
                    GlobalCacheData.Add("propertyAccessdetails", pad[0],LifeTimes);
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


        public static IProperty GetSecuredPropertiesAccountKey(int AccountKey)
        {
            IAccountRepository accrep = RepositoryFactory.GetRepository<IAccountRepository>();
            IMortgageLoanAccount acc = (IMortgageLoanAccount)accrep.GetAccountByKey(AccountKey);
            IProperty prop = acc.SecuredMortgageLoan.Property;
            return prop;
        }

        void InitialiseXMLDataset(string XMLData)
        {
            if (XMLData != null)
            {
                StringReader TextReader = new StringReader(XMLData);
                dsProperties.ReadXml(TextReader, XmlReadMode.Auto);
            }
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
                //if (!PrivateCacheData.ContainsKey("Contact2Details"))
                if (!GlobalCacheData.ContainsKey("Contact2Details"))
                {
                    // If it contains the key, its already checked once
                    if (View.SettxtContact2Name.Length == 0)
                    {
                        View.Messages.Add(new Warning("If you have a second contact, please add them. If not, ignore this message and click Instruct Valuer again.", "If you have a second contact, please add them. If not, ignore this message and click Instruct Valuer again."));
                        //PrivateCacheData.Add("Contact2Details", true);
                        GlobalCacheData.Add("Contact2Details", true,LifeTimes);
                        retval = false;
                    }
                    else
                    {
                        //PrivateCacheData.Add("Contact2Details", true);
                        GlobalCacheData.Add("Contact2Details", true,LifeTimes);
                    }
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

            if (View.SetddlAssessmentReasonValue == 0)
            {
                View.Messages.Add(new Warning("Please select an Assessment Reason before continuing.", "Please select an Assessment Reason before continuing."));
                retval = false;
            }

            DateTime DateNow = DateTime.Now;
            DateTime DateSelected = View.SetdtAssessmentByDateValue.ToString().Length > 0 ? View.SetdtAssessmentByDateValue : DateNow;

            if (DateSelected <= DateNow)
            {
                View.Messages.Add(new Warning("Please select a future Assessment date before continuing.", "Please select a future Assessment date before continuing."));
                retval = false;
            }

            return retval;
        }

        // Update the valuation record with the valuers details
        private bool AddValuationRecord()
        {
            bool retval = true;
            //string SQL = " select data.*, s.name, i.* from x2.instance i join x2data.Valuations data on i.id=data.instanceid join x2.state s on i.stateid=s.id where data.application.key= " + genericKey + " order by data.application.key desc";
            //DataTable Valuators = PrivateCacheData["Valuators"] as DataTable;
            //dsProperties = PrivateCacheData["AdCheckProperties"] as DataSet;
            DataTable Valuators = GlobalCacheData["Valuators"] as DataTable;
            dsProperties = GlobalCacheData["AdCheckProperties"] as DataSet;
            IValuationDiscriminatedAdCheckPhysical valuation = PropertyRepository.CreateEmptyValuation(ValuationDataProviderDataServices.AdCheckPhysicalValuation) as IValuationDiscriminatedAdCheckPhysical;
            if (valuation != null)
            {
                valuation.ValuationStatus = LookupRepository.ValuationStatus.ObjectDictionary[((int)ValuationStatuses.Pending).ToString()];
                valuation.IsActive = false;
                if (dsProperties != null) valuation.Data = dsProperties.GetXml();
                //valuation.Property = PropertyRepository.GetPropertyByKey(Convert.ToInt32(PrivateCacheData["PropertyKey"]));
                valuation.Property = PropertyRepository.GetPropertyByKey(Convert.ToInt32(GlobalCacheData["PropertyKey"]));
                if (Valuators != null)
                {
                    valuation.Valuator = PropertyRepository.GetValuatorByKey((Convert.ToInt32(Valuators.Rows[View.SetddlValuerSelectedIndex]["ValuatorKey"])));
                    //PrivateCacheData.Remove("ValuatorID");
                    //PrivateCacheData.Add("ValuatorID", valuation.Valuator.Key);
                    GlobalCacheData.Remove("ValuatorID");
                    GlobalCacheData.Add("ValuatorID", valuation.Valuator.Key,LifeTimes);
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
                //PrivateCacheData.Remove("valuationKey");
                //PrivateCacheData.Add("valuationKey", valuationKey);
                GlobalCacheData.Remove("valuationKey");
                GlobalCacheData.Add("valuationKey", valuationKey,LifeTimes);
            }

            return retval;

        }


        bool UpdatePropertyAccessDetails()
        {
            bool retval = true;
            //propertyKey = Convert.ToInt32(PrivateCacheData["PropertyKey"]);
            propertyKey = Convert.ToInt32(GlobalCacheData["PropertyKey"]);
            IPropertyRepository propertyRep = RepositoryFactory.GetRepository<IPropertyRepository>();
            IProperty prop = propertyRep.GetPropertyByKey(propertyKey);
            IPropertyAccessDetails propertyAccessdetails = PropertyRepository.CreateEmptyPropertyAccessDetails();

            //if (PrivateCacheData.ContainsKey("propertyAccessdetails"))
            if (GlobalCacheData.ContainsKey("propertyAccessdetails"))
            {
                IEventList<IPropertyAccessDetails> PAD = PropertyRepository.GetPropertyAccesDetailsByPropertyKey(propertyKey);
                propertyAccessdetails = PAD[0];
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

        private DataTable SetupValuators()
        {
            DataTable Valuator = new DataTable("Valuator");
            Valuator.Columns.Add("ValuatorKey", Type.GetType("System.String")); //0
            Valuator.Columns.Add("ValuatorName", Type.GetType("System.String")); //1

            //IEventList<IValuator> valuators = PropertyRepository.GetActiveValuators();
            IEventList<IValuator> valuators = PropertyRepository.GetActiveValuatorsFiltered((int)GeneralStatuses.Active);
            valuators.Sort(delegate(IValuator c1, IValuator c2)
            {
                return c1.LegalEntity.GetLegalName(LegalNameFormat.Full).CompareTo(c2.LegalEntity.GetLegalName(LegalNameFormat.Full));
            });


            LegalEntityRepository legalentityrepository = (LegalEntityRepository)RepositoryFactory.GetRepository<ILegalEntityRepository>();

            DataRow selectRow = Valuator.NewRow();
            selectRow["ValuatorKey"] = "0";
            selectRow["ValuatorName"] = "-select-";
            Valuator.Rows.Add(selectRow);

            for (int i = 0; i < valuators.Count; i++)
            {
                DataRow valRow = Valuator.NewRow();
                valRow["ValuatorKey"] = valuators[i].Key.ToString();
                ILegalEntity le = legalentityrepository.GetLegalEntityByKey(valuators[i].LegalEntity.Key);
                valRow["ValuatorName"] = le.GetLegalName(LegalNameFormat.Full);
                Valuator.Rows.Add(valRow);
            }
            return Valuator;
        }

        private DataTable GetLoanTypes()
        {
            DataTable LoanTypes = new DataTable("LoanTypes");
            LoanTypes.Columns.Add("LoanType", Type.GetType("System.String")); //0
            LoanTypes.Columns.Add("Key", Type.GetType("System.String")); //1

            DataRow selectRow = LoanTypes.NewRow();
            selectRow["Key"] = "0";
            selectRow["LoanType"] = "-select-";
            LoanTypes.Rows.Add(selectRow);
            IEventList<IApplicationType> applicationtypes = LookupRepository.ApplicationTypes;
            for (int i = 0; i < applicationtypes.Count; i++)
            {
                switch (applicationtypes[i].Key)
                {
                    case 4:
                    case 6:
                    case 7:
                    case 8:
                        DataRow valRow = LoanTypes.NewRow();
                        valRow["Key"] = applicationtypes[i].Key.ToString();
                        valRow["LoanType"] = applicationtypes[i].Description;
                        LoanTypes.Rows.Add(valRow);
                        break;
                }
            }
            return LoanTypes;
        }

    }
}


