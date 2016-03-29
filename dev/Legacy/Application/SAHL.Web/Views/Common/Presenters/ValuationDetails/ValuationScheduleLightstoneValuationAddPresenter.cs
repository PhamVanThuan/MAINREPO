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
using System.Linq;
using SAHL.Common.Service.Interfaces.DataSets;
using System.Xml.Linq;
using SAHL.Common.Security;

namespace SAHL.Web.Views.Common.Presenters.ValuationDetails
{

    public class ValuationScheduleLightstoneValuationAddPresenter : SAHLCommonBasePresenter<IValuationScheduleLightstoneValuation>
    {
        #region Properties
        ILightStoneService lightstoneService;
        private ILookupRepository lookuprepository;
        private IPropertyRepository propertyrepository;
        private IApplicationRepository applicationrepository;
        private IAccountRepository accountrepository;
        private IX2Repository x2Repository;
        private int genericKey;
        private int genericKeyTypeKey;
        private CBONode node;
        private IApplication application;
        private IProperty property;
        private IApplicationMortgageLoan applicationMortgageLoan;
        private IPropertyAccessDetails pad;
        private IInstance instance;
        private List<ICacheObjectLifeTime> lifeTimes;
        string applicationtypedescription;


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
        #endregion

        public ValuationScheduleLightstoneValuationAddPresenter(IValuationScheduleLightstoneValuation view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            // Set up EventHandlers for the interface
            View.btnInstructClicked += BtnInstructClicked;
            lightstoneService = ServiceFactory.GetService<ILightStoneService>();

            node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            if (node == null)
                return;

            genericKeyTypeKey = node.GenericKeyTypeKey;
            genericKey = node.GenericKey;
            instance = X2Repository.GetLatestInstanceForGenericKey(genericKey, SAHL.Common.Constants.WorkFlowName.Valuations, SAHL.Common.Constants.WorkFlowProcessName.Origination);

            switch (genericKeyTypeKey)
            {
                case (int)GenericKeyTypes.Account:
                    IAccount account = AccountRepository.GetAccountByKey(genericKey);
                    property = GetSecuredPropertiesAccountKey(account.Key);
                    application = ApplicationRepository.GetApplicationFromInstance(instance);
                    break;

                case (int)GenericKeyTypes.Offer:
                    application = ApplicationRepository.GetApplicationByKey(genericKey);
                    applicationMortgageLoan = application as IApplicationMortgageLoan;
                    property = applicationMortgageLoan.Property;
                    break;

                case (int)GenericKeyTypes.Property:
                    property = PropertyRepository.GetPropertyByKey(genericKey);
                    application = ApplicationRepository.GetApplicationFromInstance(instance);
                    break;
                default:
                    throw new Exception("Unsupported GenericKeyType");
            }

            switch (application.ApplicationType.Key)
            {
                case (int)OfferTypes.NewPurchaseLoan:
                    applicationtypedescription = "1";
                    break;
                case (int)OfferTypes.ReAdvance:
                case (int)OfferTypes.FurtherAdvance:
                case (int)OfferTypes.FurtherLoan:
                    applicationtypedescription = "3";
                    break;
                //case (int)OfferTypes.SwitchLoan:
                //case (int)OfferTypes.RefinanceLoan:
                //    applicationtypedescription = "2";
                //    break;
                default:
                    applicationtypedescription = "2";
                    break;
            }

            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            svc.ExecuteRule(_view.Messages, "ValuationRecentExists", application);

            if (property != null)
            {
                SetupInterface();
                SetupPropertyDetailsDisplay(property);
                SetupPropertyAccessDetails(property.Key);
            }
        }

        void SetupPropertyDetailsDisplay(IProperty property)
        {
            View.SetlblAssessmentReasonValue = application.ApplicationType.Description;

            if (property != null)
                View.BindPropertyDetails(property);
        }

        void SetupPropertyAccessDetails(int propertyKey)
        {
            IEventList<IPropertyAccessDetails> lstPAD = PropertyRepository.GetPropertyAccesDetailsByPropertyKey(propertyKey);
            if (lstPAD.Count > 0)
            {
                // there is already a contact record... populate the view with it..
                pad = lstPAD[0];
                View.BindPropertyAccessDetails(pad);
            }
        }

        void SetupInterface()
        {
            View.ShowpnlValuationAssignmentDetails = false;
            View.ShowdtAssessmentByDateValue = true;
            View.ShowlblAssessmentByDateValue = false;
            View.ShowddlAssessmentReasonValue = false;
            View.ShowlblAssessmentReasonValue = true;
            View.ShowtxtSpecialInstructions = true;
            View.ShowbtnInstruct = true;
            View.ShowbtnValidateProperty = false;
        }

        
        void BtnInstructClicked(object sender, EventArgs e)
        {
            TransactionScope txn = new TransactionScope();
            try
            {
                //Do Checks on fields (Business Rules)
                if (!Validate())
                    return;

                pad = SetPropertyAccessDetails(property);

                bool isReview = CheckForReview();
                string lightstonePropertyId = String.Empty;
                int valuationKey = 0;

                ExclusionSets.Add(RuleExclusionSets.ValuationUpdatePreviousToInactive);

                // Save propert access details
                PropertyRepository.SavePropertyAccessDetails(pad);

                // Call LS web svc 
                PropertyRepository.RequestLightStoneValuation(pad, property, genericKey, genericKeyTypeKey, isReview, View.SetdtAssessmentByDateValue, application.ApplicationType.Description, View.SettxtSpecialInstructions, out lightstonePropertyId);

                // Save valuation
                valuationKey = PropertyRepository.CreateValuationLightStone(property).Key;
                
                ExclusionSets.Remove(RuleExclusionSets.ValuationUpdatePreviousToInactive);

                txn.VoteCommit();

                //Create Workflow Case in ValuationsWorkflow pointing to valuations record created above
                X2CompleteAndNavigate(lightstonePropertyId, valuationKey);

                GlobalCacheData.Remove("Contact2Details");
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

        private bool CheckForReview()
        {
            bool isReview = false;
            X2Data x2Data = X2Repository.GetX2DataForInstance(instance);

            if (x2Data == null || x2Data.Data == null || x2Data.Data.Rows.Count < 1)
                throw new Exception("X2Data not found");

            DataRow x2Row = x2Data.Data.Rows[0];

            if (x2Data.Data.Columns.Contains("IsReview") && x2Row["IsReview"] != DBNull.Value)
                isReview = Convert.ToBoolean(x2Row["IsReview"]);

            return isReview;
        }

        private void X2CompleteAndNavigate(string lightstonePropertyId, int valuationKey)
        {
            Dictionary<string, string> inputFields = new Dictionary<string, string>();
            inputFields.Clear();

            inputFields.Add("PropertyKey", property.Key.ToString());
            inputFields.Add("RequestingAdUser", View.CurrentPrincipal.Identity.Name);
            inputFields.Add("LightstonePropertyID", lightstonePropertyId);
            inputFields.Add("ValuationKey", valuationKey.ToString());
            inputFields.Add("ValuationDataProviderDataServiceKey", ((int)ValuationDataProviderDataServices.LightstonePhysicalValuation).ToString());

            #region MagicStrings

            //assesmentpriority#specialinstructions#valuatorid#applicationtypedescription#assesmentDate
            string x2String = "#App:" + application.Key + ": " + View.SettxtSpecialInstructions;

            if (GlobalCacheData.ContainsKey("ValuatorID"))
                x2String += "#" + Convert.ToString(GlobalCacheData["ValuatorID"]);
            else
                x2String += "#";

            x2String += "#" + applicationtypedescription + "#" + Convert.ToString(View.SetdtAssessmentByDateValue);

            #endregion

            X2Service.CompleteActivity(View.CurrentPrincipal, inputFields, false, x2String);
            X2Service.WorkflowNavigate(View.CurrentPrincipal, View.Navigator);
        }

        IPropertyAccessDetails SetPropertyAccessDetails(IProperty prop)
        {
            if (pad == null)
                pad = PropertyRepository.CreateEmptyPropertyAccessDetails();

            //Store Information in PropertyAccessDetails
            pad.Property = prop;
            pad.Contact1 = View.SettxtContact1Name;
            pad.Contact1MobilePhone = View.SettxtContact1MobilePhone;
            pad.Contact1Phone = View.SettxtContact1Phone;
            pad.Contact1WorkPhone = View.SettxtContact1WorkPhone;
            pad.Contact2 = View.SettxtContact2Name;
            pad.Contact2Phone = View.SettxtContact2Phone;

            return pad;
        }

        bool Validate()
        {
            bool retval = true;

            DateTime dateNow = DateTime.Now;
            DateTime dateSelected = View.SetdtAssessmentByDateValue.ToString().Length > 0 ? View.SetdtAssessmentByDateValue : dateNow;

            if (dateSelected <= dateNow)
            {
                View.Messages.Add(new Warning("Please select a future Assessment date before continuing.", "Please select a future Assessment date before continuing."));
                retval = false;
            }
            if (string.IsNullOrEmpty(View.SettxtContact1Name))
            {
                View.Messages.Add(new Warning("Contact 1 Name is required.", "Contact 1 Name is required."));
                retval = false;
            }
            if (string.IsNullOrEmpty(View.SettxtContact1Phone) & string.IsNullOrEmpty(View.SettxtContact1WorkPhone) & string.IsNullOrEmpty(View.SettxtContact1MobilePhone))
            {
                View.Messages.Add(new Warning("At least one of Contact 1's Home phone number, Work phone number or Cell phone number must be provided.", "At least one of Contact 1's Home phone number, Work phone number or Cell phone number must be provided."));
                retval = false;
            }

            if (retval && !GlobalCacheData.ContainsKey("Contact2Details"))
            {
                // If it contains the key, its already checked once
                if (string.IsNullOrEmpty(View.SettxtContact2Name))
                {
                    View.Messages.Add(new Warning("If you have a second contact, please add them. If not, ignore this message and click Instruct Valuer again.", "If you have a second contact, please add them. If not, ignore this message and click Instruct Valuer again."));
                    retval = false;
                }

                GlobalCacheData.Add("Contact2Details", true, LifeTimes);
            }

            return retval;
        }

        IProperty GetSecuredPropertiesAccountKey(int accountKey)
        {
            IMortgageLoanAccount acc = (IMortgageLoanAccount)AccountRepository.GetAccountByKey(accountKey);
            IProperty prop = acc.SecuredMortgageLoan.Property;
            return prop;
        }

    }
}


