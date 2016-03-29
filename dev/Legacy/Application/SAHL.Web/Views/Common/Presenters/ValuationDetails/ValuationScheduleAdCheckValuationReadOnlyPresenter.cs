using System;
using System.Data;
using System.IO;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Web.Views.Common.Interfaces;

using SAHL.Common;
using SAHL.Common.UI;
using SAHL.Common.CacheData;
using System.Collections.Generic;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.ValuationDetails
{

    public class ValuationScheduleAdCheckValuationReadOnlyPresenter : SAHLCommonBasePresenter<IValuationScheduleAdCheckValuation>
    {

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
        // private string applicationtype = "";
        private List<ICacheObjectLifeTime> lifeTimes;

        // int applicationtypekey;

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

        public ValuationScheduleAdCheckValuationReadOnlyPresenter(IValuationScheduleAdCheckValuation view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            // Set up EventHandlers for the interface
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
                    // applicationtype = application.ApplicationType.Description;
                    //tk = account.Key;
                    property = GetSecuredPropertiesAccountKey(account.Key);
                    break;
                case (int)GenericKeyTypes.Offer:
                    // get the application
                    application = ApplicationRepository.GetApplicationFromInstance(instance);
                    if (application != null && application is IApplicationMortgageLoan)
                    {
                        applicationMortgageLoan = application as IApplicationMortgageLoan;
                        // applicationtype = applicationMortgageLoan.ApplicationType.Description;
                    }
                    property = applicationMortgageLoan.Property;
                    break;
                case (int)GenericKeyTypes.Property:
                    // get the property
                    break;
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

            if (application != null)
            {
                IRuleService svc = ServiceFactory.GetService<IRuleService>();
                svc.ExecuteRule(_view.Messages, "ValuationRecentExists", application);
            }

            //IPropertyRepository propertyRepository = RepositoryFactory.GetRepository<IPropertyRepository>();
            //const int requiredKey = 690812;
            //application = ApplicationRepository.GetApplicationByKey(requiredKey);

            //application = ApplicationRepository.GetApplicationByKey(genericKey);
            //legalentities = application.GetLegalEntitiesByRoleType(offerroletypes);

            //property = PropertyRepository.GetPropertyByKey(527047);
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



                //PrivateCacheData.Remove("PropertyKey");
                //PrivateCacheData.Add("PropertyKey", property.Key);
                GlobalCacheData.Remove("PropertyKey");
                GlobalCacheData.Add("PropertyKey", property.Key,LifeTimes);

                SetupInterface();
                SetupPropertyAccessDetails();
                SetupPropertyDetailsDisplay();
            }

        }


        void SetupPropertyDetailsDisplay()
        {

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

                // Valuation Request Details
                if (dtValuatorData.Rows.Count > 0)
                {
                    DataRow valuatorrow = dtValuatorData.Rows[0];
                    View.SetlblValuerValue = GetValuatorName(Convert.ToInt32(valuatorrow["val_company_id"]));
                    switch (Convert.ToInt32(valuatorrow["val_priority_id"]))
                    {
                        case 0:
                        case 1:
                            View.SetlblAssessmentPriorityValue = "Low";
                            break;
                        case 2:
                            View.SetlblAssessmentPriorityValue = "Medium";
                            break;
                        case 3:
                            View.SetlblAssessmentPriorityValue = "High";
                            break;
                    }

                    //
                    View.SettxtSpecialInstructions = Convert.ToString(valuatorrow["instructions"]);
                    View.SetlblAssessmentByDateValue = Convert.ToDateTime(valuatorrow["requested_perform_date"]).ToShortDateString();
                    View.SetlblRequestNumberValue = Convert.ToString(valuatorrow["alternate_valuation_id"]);
                }

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
                // Top Panel 
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

        private string GetValuatorName(int p)
        {
            IValuator valuator = PropertyRepository.GetValuatorByKey(p);
            LegalEntityRepository legalentityrepository = (LegalEntityRepository)RepositoryFactory.GetRepository<ILegalEntityRepository>();
            ILegalEntity le = legalentityrepository.GetLegalEntityByKey(valuator.LegalEntity.Key);
            return le.GetLegalName(LegalNameFormat.Full);
        }

        void SetupInterface()
        {
            View.ShowtxtContact1Name = false;
            View.ShowtxtContact1Phone = false;
            View.ShowtxtContact1WorkPhone = false;
            View.ShowtxtContact1MobilePhone = false;
            View.ShowtxtContact2Name = false;
            View.ShowtxtContact2Phone = false;

            View.ShowddlValuer = false;
            View.ShowdtAssessmentByDateValue = false;
            View.ShowddlAssessmentReasonValue = false;
            View.ShowddlAssessmentPriorityValue = false;

            View.ShowlblContact1Value = true;
            View.ShowlblPhone1Value = true;
            View.ShowlblWorkPhone1Value = true;
            View.ShowlblCellPhone1Value = true;
            View.ShowlblContact2Value = true;
            View.ShowlblPhone2Value = true;

            View.ShowlblValuerValue = true;
            View.ShowlblAssessmentByDateValue = true;
            View.ShowlblAssessmentReasonValue = true;
            View.ShowlblAssessmentPriorityValue = true;

            // TODO Make Readonly settings for fields below
            View.SettxtSpecialInstructionsReadOnly = true;
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


    }
}


