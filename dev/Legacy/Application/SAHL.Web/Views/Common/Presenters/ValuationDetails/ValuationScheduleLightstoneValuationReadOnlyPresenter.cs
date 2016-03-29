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

namespace SAHL.Web.Views.Common.Presenters.ValuationDetails
{

    public class ValuationScheduleLightstoneValuationReadOnlyPresenter : SAHLCommonBasePresenter<IValuationScheduleLightstoneValuation>
    {
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
        private string lightstonePropertyId;
        private IInstance instance;
        private List<ICacheObjectLifeTime> lifeTimes;
 

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

        public ValuationScheduleLightstoneValuationReadOnlyPresenter(IValuationScheduleLightstoneValuation view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            if (node == null)
                return;

            genericKeyTypeKey = node.GenericKeyTypeKey;
            genericKey = node.GenericKey;
            instance = X2Repository.GetLatestInstanceForGenericKey(genericKey, SAHL.Common.Constants.WorkFlowName.Valuations, SAHL.Common.Constants.WorkFlowProcessName.Origination);

            GlobalCacheData.Remove("genericKey");
            GlobalCacheData.Add("genericKey", genericKey, LifeTimes);
            GlobalCacheData.Remove("genericKeyTypeKey");
            GlobalCacheData.Add("genericKeyTypeKey", genericKeyTypeKey, LifeTimes);

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
            }

            GlobalCacheData.Remove("PropertyKey");
            GlobalCacheData.Add("PropertyKey", property.Key, LifeTimes);
            GlobalCacheData.Remove("ApplicationKey");
            GlobalCacheData.Add("ApplicationKey", application.Key, LifeTimes);

            if (property != null)
            {
                if (PropertyRepository.HasLightStonePropertyID(property.Key))
                {
                    // Find LightStone Property Id
                    lightstonePropertyId =  property.PropertyDatas
                                            .Where(x => x.PropertyDataProviderDataService.Key == (int)PropertyDataProviderDataServices.LightstonePropertyIdentification)
                                            .Select(x => x.PropertyID).FirstOrDefault();

                    GlobalCacheData.Add("lightstonePropertyId", lightstonePropertyId, LifeTimes);
                }

                SetupInterface();
                SetupPropertyDetailsDisplay(property.Key);
                SetupPropertyAccessDetails(property.Key);
            }
        }

        void SetupPropertyDetailsDisplay(int propertyKey)
        {
            if (!string.IsNullOrEmpty(lightstonePropertyId))
            {
                string xml = PropertyRepository.GetXMLHistoryData(genericKeyTypeKey, genericKey, "Lightstone", "newPhysicalInstruction");

                if (string.IsNullOrEmpty(xml))
                {
                    View.Messages.Add(new Error("The XMLHistory record for this case was not found", "The XMLHistory record for this case was not found"));
                }
                else
                {
                    XDocument xdoc = XDocument.Parse(xml);
                    XElement xe = xdoc.Root.Descendants().Where(x => x.Name.LocalName == "LightstoneValidatedProperty").FirstOrDefault();
 
                    if (xe != null)
                    {
                        StringReader reader = new StringReader(xe.ToString());
                        LightstoneValidatedProperty DS = new LightstoneValidatedProperty();
                        DS.ReadXml(reader, XmlReadMode.Auto);

                        if (DS != null && DS.PropertyDetails != null && DS.PropertyDetails.Count > 0)
                        {
                            View.SetlblAssessmentReasonValue = DS.PropertyDetails[0].ValuationReason;
                            View.SetlblAssessmentByDateValue = DS.PropertyDetails[0].ValuationRequiredDate;
                            View.SettxtSpecialInstructions = DS.PropertyDetails[0].ValuationInstructions;
                            View.SetlblRequestNumberValue = DS.PropertyDetails[0].UniqueID.ToString();
                        }
                    }
                }
            }
            else
            {
                string xml = PropertyRepository.GetXMLHistoryData(genericKeyTypeKey, genericKey, "Lightstone", "newPhysicalInstruction_Unvalidated");
               
                if (string.IsNullOrEmpty(xml))
                {
                    View.Messages.Add(new Error("The XMLHistory record for this case was not found", "The XMLHistory record for this case was not found"));
                }
                else
                {
                    XDocument xdoc = XDocument.Parse(xml);
                    XElement xe = xdoc.Root.Descendants().Where(x => x.Name.LocalName == "LightstoneNonValidatedProperty").FirstOrDefault();

                    if (xe != null)
                    {
                        StringReader reader = new StringReader(xe.ToString());
                        LightstoneNonValidatedProperty DS = new LightstoneNonValidatedProperty();
                        DS.ReadXml(reader, XmlReadMode.Auto);

                        if (DS != null && DS.PropertyDetails != null && DS.PropertyDetails.Count > 0)
                        {
                            View.SetlblAssessmentReasonValue = DS.PropertyDetails[0].ValuationReason;
                            View.SetlblAssessmentByDateValue = DS.PropertyDetails[0].ValuationRequiredDate;
                            View.SettxtSpecialInstructions = DS.PropertyDetails[0].ValuationInstructions;
                            View.SetlblRequestNumberValue = DS.PropertyDetails[0].UniqueID.ToString();
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(View.SetlblAssessmentReasonValue))
                View.SetlblAssessmentReasonValue = application.ApplicationType.Description;
    
            View.SetlblAssessmentNumberValue = lightstonePropertyId;
            View.SetlblApplicationNameValue = instance.Subject;
            View.SetlblRequestedByValue = instance.CreatorADUserName;

            property = PropertyRepository.GetPropertyByKey(propertyKey);

            if (property != null)
                View.BindPropertyDetails(property);
        }

        void SetupPropertyAccessDetails(int propertyKey)
        {
            if (!GlobalCacheData.ContainsKey("propertyAccessdetails"))
            {
                IEventList<IPropertyAccessDetails> pad = PropertyRepository.GetPropertyAccesDetailsByPropertyKey(propertyKey);

                if (pad.Count > 0)
                {
                    // there is already a contact record... populate the view with it..
                    GlobalCacheData.Add("propertyAccessdetails", pad[0], LifeTimes);
                    View.BindPropertyAccessDetails(pad[0]);
                }
            }
        }

        void SetupInterface()
        {
            View.ShowpnlValuationAssignmentDetails = true;

            View.ShowdtAssessmentByDateValue = false;
            View.ShowlblAssessmentByDateValue = true;
            View.ShowddlAssessmentReasonValue = false;
            View.ShowlblAssessmentReasonValue = true;
            View.ShowtxtSpecialInstructions = true;
            View.ShowbtnInstruct = false;
            View.ShowbtnValidateProperty = false;

            View.ShowtxtContact1Name = false;
            View.ShowlblContact1Value = true;
            View.ShowtxtContact1Phone = false;
            View.ShowlblPhone1Value = true;
            View.ShowtxtContact1WorkPhone = false;
            View.ShowlblWorkPhone1Value = true;
            View.ShowtxtContact1MobilePhone = false;
            View.ShowlblCellPhone1Value = true;
            View.ShowtxtContact2Name = false;
            View.ShowlblContact2Value = true;
            View.ShowtxtContact2Phone = false;
            View.ShowlblPhone2Value = true;

            // TODO Make Readonly settings for fields below
            View.SettxtSpecialInstructionsReadOnly = true;
            View.SettxtPropertyDescriptionReadOnly = true;

        
        }


        IProperty GetSecuredPropertiesAccountKey(int accountKey)
        {
            IMortgageLoanAccount acc = (IMortgageLoanAccount)AccountRepository.GetAccountByKey(accountKey);
            IProperty prop = acc.SecuredMortgageLoan.Property;
            return prop;
        }


    }
}


